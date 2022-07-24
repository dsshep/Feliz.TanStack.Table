
const path = require('path');

module.exports = (env, argv) => {
    const mode = argv.mode;
    const isProduction = mode === 'production';
    const isTest = mode === 'test';

    return {
        mode: mode,
        stats: {
            errorDetails: true
        },
        devtool: isProduction ? 'eval-source-map' : 'source-map',
        entry: isTest ? './src/App.Tests.fs.js' : './src/App.fs.js',
        output: {
            path: path.join(__dirname, "./public"),
            filename: "bundle.js",
        },
        devServer: {
            historyApiFallback: {
                index: '/'
            },
            host: '0.0.0.0',
            port: 8080,
            static: isTest ? './tests/public' : './public',
        },
        plugins: [
        ],
        module: {
            rules: [
            {
                test: /\.js$/,
                enforce: "pre",
                use: ["source-map-loader"],
            }]
        }
    }
}
