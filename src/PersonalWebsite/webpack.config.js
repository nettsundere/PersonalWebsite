const Path = require('path');
const webpack = require('webpack');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const MonacoWebpackPlugin = require('monaco-editor-webpack-plugin');

module.exports = {
    mode: "production",
    entry: {
        app: "./wwwroot/js/Base/App.js",
        privateApp: "./wwwroot/js/PrivateBase/App.js"
    },
    output: {
        path: Path.resolve(__dirname, "wwwroot/js/Build"),
        filename: "[name].bundle.js",
        publicPath: "/js/Build/"
    },
    optimization: {
        minimize: true
    },
    module: {
        rules: [
            {
                test: /\.css$/i,
                use: [
                    MiniCssExtractPlugin.loader,
                    "css-loader"
                ]
            },
            {
                test: /\.ttf$/i,
                use: ['file-loader']
            }
        ]
    },
    devtool: "source-map",
    plugins: [
        new MiniCssExtractPlugin({
            filename: "../../css/Build/[name].css",
        }),
        new webpack.ProvidePlugin({
            $: 'jquery',
            jQuery: 'jquery',
            'window.jQuery': 'jquery',
            Popper: ['popper.js', 'default']
        }),
        new MonacoWebpackPlugin({ languages: ['html', 'css', 'javascript'] })
    ]
};
