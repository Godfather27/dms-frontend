'use strict'

var source = $("#some-template").html(); 
var template = Handlebars.compile(source);

Handlebars.registerHelper('img', function(name){
  const width = Math.max(Math.floor(Math.random()*500),100);
  const height = Math.max(Math.floor(Math.random()*500),100);
  const html = `<img src="http://dummyimage.com/${width}x${height}">`;
  return new Handlebars.SafeString(html);
});

$('.button-group button').click(function () {
  search();
});

$('#search').keyup(function(e) {
  if(e.which === 13) {
    search();
  }
});


function search() {
  // SEARCH INVOCATION CODE GOES IN HERE
  let data = {
    docs: [{img: 'http://lorempixel.com/400/200'},{img: 'http://lorempixel.com/400/200'},{img: 'http://lorempixel.com/400/200'},{img: 'http://lorempixel.com/400/200'},{img: 'http://lorempixel.com/400/200'},{img: 'http://lorempixel.com/400/200'},{img: 'http://lorempixel.com/400/200'},{img: 'http://lorempixel.com/400/200'},{img: 'http://lorempixel.com/400/200'},{img: 'http://lorempixel.com/400/200'},{img: 'http://lorempixel.com/400/200'},{img: 'http://lorempixel.com/400/200'},{img: 'http://lorempixel.com/400/200'},{img: 'http://lorempixel.com/400/200'},{img: 'http://lorempixel.com/400/200'},{img: 'http://lorempixel.com/400/200'},{img: 'http://lorempixel.com/400/200'},{img: 'http://lorempixel.com/400/200'},{img: 'http://lorempixel.com/400/200'},{img: 'http://lorempixel.com/400/200'},{img: 'http://lorempixel.com/400/200'},{img: 'http://lorempixel.com/400/200'},{img: 'http://lorempixel.com/400/200'},{img: 'http://lorempixel.com/400/200'},{img: 'http://lorempixel.com/400/200'}]
  }

  renderResult(data);
  transitionToResult();
}

function renderResult(data){
  $('#wrapper').html('');
  $('#wrapper').append(template(data));
}

function transitionToResult() {
  $('header').removeClass('column');
  $('header').addClass('result-mode');
  $('#filters').addClass('show');
}