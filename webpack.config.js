
const path = require('path');
const webpack = require("webpack");
const HtmlWebpackPlugin = require('html-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const Dotenv = require('dotenv-webpack');
const realFs = require('fs');
const gracefulFs = require('graceful-fs');

gracefulFs.gracefulify(realFs);

let mode = process.env.NODE_ENV;
mode = mode ? mode : "production"
// If we're running the webpack-dev-server, assume we're in development mode
const isProduction = mode === 'production'

const CONFIG = {
    // The tags to include the generated JS and CSS will be automatically injected in the HTML template
    // See https://github.com/jantimon/html-webpack-plugin
    indexHtmlTemplate: './src/Docs/index.html',
    fsharpEntry: './.fable-build/App.js',
    cssEntry: './src/Docs/styles/styles.css',
    outputDir: './publish/docs',
    assetsDir: './src/Docs/public',
    devServerPort: 8080,
    // When using webpack-dev-server, you may need to redirect some calls
    // to a external API server. See https://webpack.js.org/configuration/dev-server/#devserver-proxy
    devServerProxy: {
        '/api/**': {
            target: 'http://localhost:' + (process.env.SERVER_PROXY_PORT || "5000"),
            changeOrigin: true
        },
        '/socket/**': {
            target: 'http://localhost:' + (process.env.SERVER_PROXY_PORT || "5000"),
            ws: true
        }
    }
}

const commonPlugins = [
    new HtmlWebpackPlugin({
        filename: 'index.html',
        template: resolve(CONFIG.indexHtmlTemplate)
    }),

    new Dotenv({
        path: "./.env",
        silent: false,
        systemvars: true
    })
];

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
        entry: {
            app: [resolve(CONFIG.fsharpEntry), resolve(CONFIG.cssEntry)]
        },
        output: {
            path: resolve(CONFIG.outputDir),
            filename: isProduction ? '[name].[fullhash].js' : '[name].js'
        },
        devServer: {
            static: {
                directory: resolve(CONFIG.assetsDir),
                publicPath: '/'
            },
            host: '0.0.0.0',
            port: CONFIG.devServerPort,
            proxy: CONFIG.devServerProxy,
            hot: true,
            historyApiFallback: true,
        },
        plugins:  isProduction ?
            commonPlugins.concat([
                new MiniCssExtractPlugin({ filename: '[name].[chunkhash].css' }),
                new CopyWebpackPlugin({
                    patterns: [
                        { from: resolve(CONFIG.assetsDir) }
                    ]
                }),
            ])
            : commonPlugins.concat([
                new webpack.HotModuleReplacementPlugin(),
                //new ReactRefreshWebpackPlugin()
            ]),
        module: {
            rules: [
                {
                    test: /\.js$/,
                    enforce: "pre",
                    use: ["source-map-loader"],
                },
                {
                    test: /\.(sass|scss|css)$/,
                    use: [
                        isProduction
                            ? MiniCssExtractPlugin.loader
                            : 'style-loader',
                        'css-loader',
                        'resolve-url-loader',
                        'postcss-loader'
                    ],
                }]
        }
    }
}

function resolve(filePath) {
    return path.isAbsolute(filePath) ? filePath : path.join(__dirname, filePath);
}