'use strict'

const express = require('express');
const app = express();
const livereload = require('livereload');
const server = livereload.createServer();
const exec = require('child_process').exec;
const multer  =   require('multer');

var storage =   multer.diskStorage({
  destination: function (req, file, callback) {
    callback(null, './uploads');
  },
  filename: function (req, file, callback) {
    callback(null, file.fieldname + '-' + Date.now());
  }
});
var upload = multer({ storage : storage}).single('photo');

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

app.get('/p/:id', function(req, res, next) {
	//bsp: 163,215,57,19,37,24
	var vector = "163,215,57,19,37,24";

	exec(`mono Bildersuche.exe vector`, (error, stdout, stderr) => {
	  if (error) {
		res.send(`error ${error}`);
	    return;
	  }
		res.send(stdout);
	});
})

app.post('/upload',function(req,res){
    upload(req,res,function(err) {
        if(err) {
            return res.end("Error uploading file.");
        }

	exec(`mono Bildersuche.exe file res.req.file.path`, (error, stdout, stderr) => {
	  if (error) {
		res.send(`error ${error}`);
	    return;
	  }
		res.send(stdout);
	});;
    });
});



app.listen(3000, function () {
  console.log('Listening on :3000');
});

server.watch(__dirname + "/public");
console.log('Livereload');
