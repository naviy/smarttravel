module.exports = {
    host: 'travel.luxena.com',
    port: 8080,
    secret: 'aCTed^crUs66',
    interval: 500,
    pauseOnFail: 10000,
	log: {
		config: {
			appenders: [
				{ type: 'console' },
				{ type: 'dateFile', filename: 'log.txt', pattern: '-yyyy-MM-dd' }
			]
		},
		options: {
			cwd: __dirname + '/../logs'
		},
		replaceConsole: true
	},
    inbox: {
            path: 'C:/Luxena/inbox'
    },
    outbox: [
        {
            target: 'accounting',
            source: 'C:/Luxena/outbox',
            filter: /\.xml$/i
        }
    ],
    sent: {
            path: 'c:/Luxena/sent'
    }
};