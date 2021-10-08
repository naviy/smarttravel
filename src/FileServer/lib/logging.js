var log4js = require('log4js');
var mkdirp = require('mkdirp');

var config = require('../conf/config');


if (config.log) {
	if (config.log.options.cwd) {
		mkdirp.sync(config.log.options.cwd);
	}
	log4js.configure(config.log.config, config.log.options);
}

module.exports = {
    getLogger: log4js.getLogger
};
