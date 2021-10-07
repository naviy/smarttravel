module.exports = {
    port: 8080,
    basedir: 'd:/data/fileserver',
    interval: 500,
    pauseOnFail: 10000,
    log: {
        config: {
            appenders: [
                { type: 'console' },
                { type: 'dateFile', filename: 'log.txt', pattern: '-yyyy-MM-dd' }
            ],
            levels: {
                'socket.io': 'WARN'
            }
        },
        options: {
            cwd: __dirname + '/../logs'
        },
        replaceConsole: true
    },
    accounts: {
        anex: {
            secret: 'WAsH99LeaNT=',
            galileoCodes: ['3AO9', '6GH9', ' 6E0']
        },
        anexeur: {
            secret: 'BuSt98amuCk%',
            galileoCodes: [' AY0']
        },
        bsv: {
            secret: 'pOm*Gut57leGs',
            galileoCodes: ['3L0B', '7Y5X', '5ES9', '69B3']
        },
        demo: {
            galileoCodes: ['7J8J', '9999']
        },
        fgr: {
            secret: 'aCTed^crUs66',
            galileoCodes: ['353K', '373C']
        },
        galileo: {
            secret: 'q -:41tu)ix1aEy',
            routeMirFiles: true
        },
        merci: {
            secret: 's63T6v*%b[Fs',
            galileoCodes: ['30AD']
        },
        persey: {
            secret: 'C82hX8A1N@',
            galileoCodes: ['39J3', '5G07', ' 5O0']
        },
        ufsa: {
            secret: 'zek$sabRE61',
            galileoCodes: ['5R3Y', '52TL', '783T', '30FT', '37N3', '39XN', '39XO', '3A66', '3A8Z', '3AM9', '3AN9', '5T75', '5T79', '71Q3', '71Q4', '3AT3', '7G6U', '7E8F', '5L7R', '5R2I', '5Z8W', '5R3Z', '5R2H', ' 39C', '5Q7K', '33GN', '6E6T']
        }
    }
};
