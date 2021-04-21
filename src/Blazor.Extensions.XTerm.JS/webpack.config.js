const path = require("path");
const webpack = require("webpack");
const CopyPlugin = require('copy-webpack-plugin');

module.exports = {
  mode: 'production',
  resolve: {
    extensions: [".ts", ".js"]
  },
  devtool: "inline-source-map",
  module: {
    rules: [
      {
        test: /\.ts?$/,
        loader: "ts-loader"
      }
    ]
  },
  entry: {
    "blazor.extensions.xterm": "./src/InitializeXTerm.ts"
  },
  plugins: [
    new CopyPlugin([
      { from: path.join(__dirname, './node_modules/xterm/css/xterm.css'), to: path.join(__dirname, "/wwwroot/xterm.css") }
    ])
  ],
  output: {
    path: path.join(__dirname, "/wwwroot"),
    filename: "[name].js"
  }
};
