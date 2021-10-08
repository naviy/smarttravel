var consts = require('constants');
var fs = require('fs');
    fs.mkdirp = require('mkdirp');
var http = require('http');
var path = require('path');
var querystring = require('querystring');
var util = require('util');

var async = require('async');
var dateFormat = require('dateformat');
var io = require('socket.io-client');

var config = require('../conf/config');
var logger = require('./logger');


process.on('uncaughtException', function (err) {
    logger.fatal('Caught exception\n', err);
});

logger.info('Application started');

config.port = config.port || 80;

if (config.inbox) {
    var ns = io.connect(config.host + ':' + config.port, { query: 'secret=' + querystring.escape(config.secret) });

    ns.on('connect', function () {
        logger.info('Connected to socket server');
    });

    ns.on('error', function (err) {
        logger.error(err);
        if (!ns.socket.connected) {
            setTimeout(function () { ns.socket.connect(); }, config.pauseOnFail);
        }
    });

    ns.on('file', function (data, fn) {
        fs.mkdirp(config.inbox.path, function (err) {
            if (err) {
                logger.error('Cannot create directory %s\n', config.inbox.path, err);
                return fn(err);
            }
            fs.writeFile(path.join(config.inbox.path, data.name), data.content, 'base64', function (err) {
                if (err) {
                    logger.error('Cannot save %s\n', data.name, err);
                    return fn(err);
                }
                logger.info('Received %s', data.name);
                fn();
            });
        });
    });
}


function sendFile(folder, file, cb) {

    var filePath = path.join(folder.source, file);

    var uploadPath = '/' + (folder.target ?
		(folder.target + '/' + file).replace(/\\/g, '/').replace(/^\/+|\/+(?=\/)|\/+$/g, '') :
		file);

    function uploadError(msg, err) {
		logger.error('Failed to upload %s to %s. %s\n', filePath, uploadPath, msg, err);
		cb(err);
    }

    function archiveError(msg, err) {
		logger.error('Failed to archive %s. %s\n', filePath, msg, err);
		cb(err);
    }

	function archiveFile() {

		function deleteFile() {
			fs.unlink(filePath, function (err) {
				if (err) {
					return archiveError('Cannot delete file', err);
				}
				cb();
			});
		}
	
		if (config.sent && config.sent.path) {
			fs.stat(filePath, function (err, stats) {
				if (err) {
					return archiveError('Cannot get stats ' + filePath, err);
				}
				var moveTo = path.join(config.sent.path, dateFormat(stats.ctime, 'yyyy/mm/dd'), path.dirname(file));
				fs.mkdirp(moveTo, function (err) {
					if (err) {
						return archiveError('Cannot create directory ' + moveTo, err);
					}
					var renameTo = path.join(moveTo, dateFormat(stats.ctime, 'yyyy-mm-dd_HH-MM-ss_') + path.basename(file));
					fs.rename(filePath, renameTo, function (err) {
						if (err) {
							// destination is on the different partition, so copy & delete
							if (err.errno === 52 && err.code === 'EXDEV') {
								var is = fs.createReadStream(filePath);
								var os = fs.createWriteStream(renameTo);
								util.pump(is, os, function (err) {
									if (err) {
										return archiveError('Cannot copy to ' + renameTo, err);
									}
									deleteFile();
								});
							} else {
								archiveError('Cannot rename to ' + renameTo, err);
							}
						} else {
							cb();
						}
					});
				});
			});
		} else {
			deleteFile();
		}
	}

    var options = {
        host: config.host,
        port: config.port,
        path: encodeURI(uploadPath),
        method: 'POST',
        agent: false, // turn off pooling, when after server restart (expload etc.) pooled connections become invalid
        headers: {
            Cookie: 'secret=' + (config.secret || '')
        }
    };

    var req = http.request(options, function (res) {
        var data = '';
        res.on('data', function (chunk) {
            data += chunk;
        });
        res.on('error', function (err) {
            uploadError('Server error ' + res.statusCode, err);
        });
        res.on('end', function () {
            if (res.statusCode != 201) {
                return uploadError(util.format('Server error %d - %s', res.statusCode, http.STATUS_CODES[res.statusCode]), data);
            }
            logger.info('Uploaded %s to %s', filePath, uploadPath);
			archiveFile();
        });
    });

    req.on('error', function (err) {
        uploadError('Request error', err);
    });

    // a must for small files, in the other case the Nagle algorithm effects in abnormal delays while sending small files
    req.setNoDelay(true);

    var stream = fs.createReadStream(filePath, { flags: consts.O_RDWR | consts.O_EXLOCK })
        .on('error', function (err) {
            if (this.fd) {
                return uploadError('Cannot stream file', err);
            }
            logger.info('Skip upload of %s to %s. Cannot open file\n', filePath, uploadPath, err);
            cb('break');
        })
        .pipe(req);
}


function processFolder(folder, cb) {

    fs.readdir(folder.source, function (err, files) {
        if (err) {
            logger.error('Failed to list files of %s', folder.source, err);
            return cb(err);
        }

        files = files
            .filter(function (file) { return folder.filter.test(file); })
            .sort();

        async.forEachSeries(files, function (file, iterator) { sendFile(folder, file, iterator); }, cb);
    });
}


function processOutbox() {

    async.forEachSeries(config.outbox, processFolder, function (err, results) {
        setTimeout(processOutbox, (err && err !== 'break') ? config.pauseOnFail : config.interval);
    });
}


if (config.outbox && config.outbox.length) {
    processOutbox();
}
