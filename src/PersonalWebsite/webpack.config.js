const Path = require('path');
const Webpack = require('webpack');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const MonacoWebpackPlugin = require('monaco-editor-webpack-plugin');

/**
 * Css type locator - used with MiniCssExtractPlugin.
 */
class CssTypeLocator {
    static getCssTypeLocator(m, entry) {
       return m.constructor.name === "CssModule" && CssTypeLocator.getIssuer(m) === entry; 
    } 
    
    static getIssuer(m) {
        if (m.issuer) {
            return CssTypeLocator.getIssuer(m.issuer);
        } else if (m.name) {
            return m.name;
        } else {
            return false;
        }
    }
}

module.exports = {
    mode: "production",
    entry: {
        app: "./wwwroot/js/Base/App.js",
        pritvateApp: "./wwwroot/js/PrivateBase/App.js"
    },
    output: {
        path: Path.resolve(__dirname, "wwwroot/js/Build"),
        filename: "[name].bundle.js",
        publicPath: "/js/Build/"
    },
    optimization: {
        minimize: true,
        splitChunks: {
            cacheGroups: {
                appStyles: {
                    name: "appStyles",
                    test: (m, c, entry = "appStyles") => CssTypeLocator.getCssTypeLocator(m, entry),
                    chunks: "all",
                    enforce: true
                },
                privateAppStyles: {
                    name: "privateAppStyles",
                    test: (m, c, entry = "privateAppStyles") => CssTypeLocator.getCssTypeLocator(m, entry),
                    chunks: "all",
                    enforce: true
                }
            }
        }
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: {
                    loader: "babel-loader",
                    options: {
                        cacheDirectory: true,
                        presets: ["@babel/preset-env"]
                    }
                }
            },
            {
                test: /\.css$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    "css-loader",
                    "postcss-loader"
                ]
            }
        ]
    },
    devtool: "source-map",
    plugins: [
        new MiniCssExtractPlugin({
            filename: "../../css/Build/[name].css",
        })
        ,
        new Webpack.ProvidePlugin({
            $: 'jquery',
            jQuery: 'jquery',
            'window.jQuery': 'jquery',
            Popper: ['popper.js', 'default']
        }),
        new MonacoWebpackPlugin({ languages: ['html', 'css'] })
    ]
};
