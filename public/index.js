'use strict'
var source = $("#some-template").html();
var template = Handlebars.compile(source);
Handlebars.registerHelper('img', function(name) {
  const width = Math.max(Math.floor(Math.random() * 500), 100);
  const height = Math.max(Math.floor(Math.random() * 500), 100);
  let buff = name.replace(/\.jpg/,'')
  buff = buff.split('/')
  const id = buff[1]
  const html = `<img src="http://www.cp.jku.at/misc/div-2014/devset/img/${name}" onClick="compare(${id})" onerror="this.style.display='none'">`;
  return new Handlebars.SafeString(html);
});
$('.button-group button').click(function() {
  search($('#search').val());
});
$('#search').keyup(function(e) {
  if (e.which === 13) {
    search($('#search').val());
  }
});

function search(query) {
    // SEARCH INVOCATION CODE GOES IN HERE
    //http://www.cp.jku.at/misc/div-2014/devset/img/acropolis_athens/2951950136.jpg
    // GET JSON FROM /q/:query
    $.ajax({
      url: `http://localhost:3000/q/${query}`,
      success: function(res) {
        renderResult(parse(res));
        transitionToResult();
      }
    });
  }

  function parse(res) {
    res = res.replace(/\[/g, '');
    res = res.replace(/\]/g, '');
    res = res.replace(/\"/g, '');
    res = res.split(",")
    let buff = []
    for (let index in res) {
      if (res[index] != "null"){
        buff.push({ url_b: res[index] })
        console.log(typeof res[index])
      }
    }
    return { docs: buff }
  }

  function renderResult(data) {
    $('#wrapper').html('');
    $('#wrapper').append(template(data));
  }

  function compare(id){
    $.ajax({
      url: `http://localhost:3000/p/${id}`,
      success: function(res) {
        renderResult(parse(res));
        transitionToResult();
      }
    })
  }

  function transitionToResult() {
    $('header').removeClass('column');
    $('header').addClass('result-mode');
    $('#filters').addClass('show');
  }

$('#uploadForm').on('submit', function() {
   if(!$('#choose-file').val()) return false;
    $(this).ajaxSubmit({
        error: function(xhr) {
            status('Error: ' + xhr.status);
        },
        success: function(res) {
          renderResult(parse(res));
         transitionToResult();
        }
    });
    //Very important line, it disable the page refresh.
    return false;
  });
// Catch the form submit and upload the files