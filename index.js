'use strict'

const express = require('express');
const app = express();
const livereload = require('livereload');
const server = livereload.createServer();
const exec = require('child_process').exec;

app.use(function(req, res, next) {
	res.header("Access-Control-Allow-Origin", "*");
	res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
	next()
})

app.use('/', express.static('public'));

app.get('/q/:id', function(req, res, next) {

	exec(`text.exe ${req.params.id}`, (error, stdout, stderr) => {
	  if (error) {
		res.send(`error ${error}`);
	    return;
	  }
		res.send(stdout);
	});
})

app.listen(3000, function () {
  console.log('Listening on :3000');
});

server.watch(__dirname + "/public");
console.log('Livereload');