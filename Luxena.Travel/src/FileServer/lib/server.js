var consts = require('constants');
var fs = require('fs');
    fs.mkdirp = require('mkdirp');
var http = require('http');
var path = require('path');
var url = require('url');

var async = require('async');
var io = require('socket.io');

var config = require('../conf/config');
var logging = require('./logging');


var logger = logging.getLogger('server');


process.on('uncaughtException', function (err) {
    logger.fatal('Caught exception\n', err);
});


if (!config.accounts) {
    logger.fatal('No accounts registered');
    process.exit(-1);
}


config.port = config.port || 80;

if (config.basedir) {
    fs.mkdirp.sync(config.basedir);
} else {
    config.basedir = __dirname;
}

var secretToAccount = {};
var galileoCodeToPath = {};

for (var key in config.accounts) {
    var account = config.accounts[key];

    account.name = key;

    fs.mkdirp.sync(account.inbox = path.join(config.basedir, account.name, 'inbox'));
    fs.mkdirp.sync(account.outbox = path.join(config.basedir, account.name, 'outbox'));

    if (account.secret) {
        if (account.secret in secretToAccount) {
            logger.fatal('Config error: duplicate secret %s', account.secret);
            process.exit(-1);
        }
        secretToAccount[account.secret] = account;
    }

    if (account.galileoCodes) {
        for (var i = 0; i < account.galileoCodes.length; ++i) {
            var code = account.galileoCodes[i];
            if (code in galileoCodeToPath) {
                logger.fatal('Config error: %s in the %s account already mapped to %s', code, account.name, mirPathMap[id]);
                process.exit(-1);
            }
            galileoCodeToPath[code] = account.inbox;
        }
    }
}


var server = http.createServer(function (req, res) {
    if (req.method !== 'POST') {
        return returnError(405);
    }

    var account = getAccount();

    if (!account) {
        return returnError(401);
    }

    var fileName = decodeURI(url.parse(req.url).pathname);

    return saveFile(req, account, fileName, function (err) {
        res.writeHead(err ? 500 : 201);
        res.end();
    });

    function returnError(status) {
        logger.error('%s %s %s', status, req.method, req.url);
        res.writeHead(status);
        res.end();
    }

    function getAccount() {
        if (!req.headers.cookie) return false;

        var cookies = req.headers.cookie.split(';');
        for (var i = 0; i < cookies.length; ++i) {
            var parts = cookies[i].split('=');
            if (parts[0].trim().toLowerCase() === 'secret') {
                return secretToAccount[parts[1].trim()];
            }
        }

        return null;
    }
});


function saveFile(stream, account, fileName, cb) {

    var filePath = path.join(account.inbox, fileName);

    logger.info('Receiving file %s', filePath);

    try {
        fs.mkdirp.sync(path.dirname(filePath));
    } catch (err) {
        logger.error('Cannot create %s directory\n', path.dirname(filePath), err);
        cb(err);
    }

    var output = fs.createWriteStream(filePath, { flags: 'wx' });

    function onclose() {
        cleanup();
        cb();
    }

    function onerror(err) {
        cleanup();
        logger.error('File %s. Stream error\n', filePath, err);
        cb(err);
    }

    stream.on('error', onerror);
    output.on('close', onclose);
    output.on('error', onerror);

    function cleanup() {
        stream.removeListener('error', onerror);
        output.removeListener('close', onclose);
        output.removeListener('error', onerror);
    }

    if (account.routeMirFiles && fileName.toLowerCase().indexOf('/mir') === 0) {
        routeMir(stream, output, filePath, fileName);
    }

    stream.pipe(output);
}


function routeMir(input, output, filePath, fileName) {

    var secondLine = null;
    var requiredLen = 8;
    var targetPath = null;

    function ondata(chunk) {

        function appendToSecondLine(buf) {
            if (secondLine.length === 0 && (buf[0] === 10 || buf[0] === 13))
                buf = buf.slice(1);
            var toAppend = requiredLen - secondLine.length;
            toAppend = buf.length < toAppend ? buf.length : toAppend;
            secondLine += buf.slice(0, toAppend).toString();
        }

        if (secondLine === null) {
            for (var i = 0; i < chunk.length; ++i) {
                if (chunk[i] === 10 || chunk[i] === 13) {
                    secondLine = '';
                    appendToSecondLine(chunk.slice(i + 1));
                    break;
                }
            }
        } else {
            appendToSecondLine(chunk);
        }

        if (secondLine !== null && secondLine.length === 8) {
            input.removeListener('data', ondata);
            var code = secondLine.slice(4, 8);
            targetPath = galileoCodeToPath[code];
            if (!targetPath) {
                logger.error('MIR %s. Queue is not defined for office id - %s', filePath, code);
            }
        }
    }

    function onclose() {
        cleanup();

        if (targetPath) {
            var renameTo = path.join(targetPath, fileName);
            fs.mkdirp(path.dirname(renameTo), function (err) {
                if (err) {
                    return logger.error('Cannot create %s directory\n', path.dirname(targetPath), err);
                }
                logger.info('Move MIR %s to %s', filePath, renameTo);
                fs.rename(filePath, renameTo, function (err) {
                    if (err) {
                        logger.error('Failed to rename MIR %s to %s\n', filePath, renameTo, err);
                    }
                });
            });
        }
    }

    input.on('data', ondata);
    input.on('error', cleanup);
    output.on('close', onclose);
    output.on('error', cleanup);

    function cleanup() {
        input.removeListener('data', ondata);
        input.removeListener('error', cleanup);
        output.removeListener('close', onclose);
        output.removeListener('error', cleanup);
    }
}


function listen() {
    server.listen(config.port);
}


server.on('listening', function () {
    logger.info('Server listening on port %d', config.port);
    var iologger = logging.getLogger('socket.io');
    var options = {
        logger: {
            error: function() {
                iologger.error.apply(iologger, arguments);
            },
            warn: function() {
                iologger.warn.apply(iologger, arguments);
            },
            info: function() {
                iologger.info.apply(iologger, arguments);
            },
            debug: function() {
                iologger.debug.apply(iologger, arguments);
            }
        },
        authorization: function (data, callback) {
            var secret = data.query.secret;
            var account = secretToAccount[secret];
            if (account) {
                data.account = account;
                callback(null, true);
            } else {
                logger.warn('Unknown secret: %s\n', secret, data);
                callback('unknown secret', false);
            }
        }
    };
    io.listen(server, options).sockets.on('connection', onsocket);
});


function onsocket(socket) {
    var account = socket.handshake.account;

    logger.info('%s connected to outbox', account.name);

    var timeout;

    socket.on('disconnect', function () {
        logger.info('%s disconnected from outbox', account.name);
        if (timeout) {
            clearTimeout(timeout);
            timeout = undefined;
        }
    });

    function sendFile(file, cb) {
        var pathName = path.join(account.outbox, file);

        fs.readFile(pathName, { encoding : 'base64', flag: consts.O_RDWR | consts.O_EXLOCK }, function (err, data) {
            if (err) {
                if (err.code === 'EBUSY') {
                    logger.info('File %s is busy\n', pathName, err);
                    return cb('break');
                }
                logger.error('Cannot read file %s\n', pathName, err);
                return cb(err);
            }
            socket.emit('file', { name: file, content: data }, function (err) {
                if (err) {
                    logger.error('Client error on %s\n', pathName, err);
                    return cb(err);
                }
                logger.info('File %s has been sent', pathName);
                fs.unlink(pathName, function (err) {
                    if (err) {
                        logger.error('Failed to delete %s', pathName, err);
                    }
                });
                cb();
            });
        });
    }

    function processFolder(cb) {
        fs.readdir(account.outbox, function (err, files) {
            if (err) {
                logger.error('Failed to list files of %s\n', account.outbox, err);
                return cb(err);
            }

            async.forEachSeries(files.sort(), function (file, iterator) { sendFile(file, iterator); }, cb);
        });

    }

    function process() {
        processFolder(function (err) {
            timeout = setTimeout(process, (err && err !== 'break') ? config.pauseOnFail : config.interval);
        });
    }

    process();
}


server.on('error', function (err) {
    logger.error('Server error\n', err);
    function relisten() {
        setTimeout(listen, config.pauseOnFail);
    }
    if (server._handle) {
        server.close(relisten);
    } else {
        relisten();
    }
});


process.on('SIGINT', function () {
	server.close(function () {
		logger.info('Server stopped');
		process.exit(0);
	});
});


listen();