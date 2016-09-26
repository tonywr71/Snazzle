var webpack = require('webpack');
var path = require('path');
var HtmlWebpackPlugin = require('html-webpack-plugin');
var CommonsChunkPlugin = webpack.optimize.CommonsChunkPlugin;
var helpers = require('./webpack.config.helpers');

console.log("@@@@@@@@@ USING PRODUCTION @@@@@@@@@@@@@@@");

module.exports = {
  devtool: 'source-map',
  output: {
    path: path.join(__dirname, 'wwwroot', 'dist'),
    publicPath: '/dist/',
    filename: '[name].js',
    chunkFilename: '[id].chunk.js'
  },
  plugins: [
      new webpack.NoErrorsPlugin(),
      new webpack.optimize.DedupePlugin(),
      new webpack.optimize.OccurenceOrderPlugin(),
      new HtmlWebpackPlugin({
        template: 'index.template.ejs',
        inject: 'body',
        filename: '../index.html'
      }),
      new webpack.optimize.UglifyJsPlugin({
        compress: { warnings: false },
        minimize: true,
        mangle: false // Due to https://github.com/angular/angular/issues/6678
      })
  ]
};
