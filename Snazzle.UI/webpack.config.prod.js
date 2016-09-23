var webpack = require('webpack');
var path = require('path');

module.exports = {
  devtool: 'source-map',
  output: {
    path: path.join(__dirname, 'wwwroot', 'dist'),
    publicPath: '/',
    filename: '[name].[hash].js',
    chunkFilename: '[id].[hash].chunk.js'
  },
  plugins: [
      new webpack.optimize.OccurenceOrderPlugin(),
      new webpack.optimize.UglifyJsPlugin({
        compress: { warnings: false },
        minimize: true,
        mangle: false // Due to https://github.com/angular/angular/issues/6678
      })
  ]
};
