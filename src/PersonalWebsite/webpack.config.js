const Path = require('path');
var Webpack = require('webpack');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

const UglifyJsPlugin = require("uglifyjs-webpack-plugin");
const OptimizeCSSAssetsPlugin = require("optimize-css-assets-webpack-plugin");


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
        filename: "[name].bundle.js"
    },
    optimization: {
        minimizer: [
            new UglifyJsPlugin({
                cache: true,
                parallel: true,
                sourceMap: true
            }),
            new OptimizeCSSAssetsPlugin({})
        ],
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
        })
    ]
};
