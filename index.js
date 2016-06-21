'use strict'

const express = require('express');
const app = express();
const livereload = require('livereload');
const server = livereload.createServer();

app.use('/', express.static('public'));

app.listen(3000, function () {
  console.log('Listening on :3000');
});

server.watch(__dirname + "/public");
console.log('Livereload');