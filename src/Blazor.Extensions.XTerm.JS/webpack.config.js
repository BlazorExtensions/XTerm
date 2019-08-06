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
      { from: path.join(__dirname, './node_modules/xterm/dist/xterm.css'), to: path.join(__dirname, "/dist/xterm.css") },
      { from: 'other', to: 'public' },
    ])
  ],
  output: {
    path: path.join(__dirname, "/dist"),
    filename: "[name].js"
  }
};
