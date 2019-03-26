var express = require('express');
var app = express();

app.use(express.static('public'));
app.set('view engine', 'pug');
app.set('views','./views');
app.locals.pretty=true;

app.get('/topic', function(req,res){
  var topics = [
    'Javascript is............',
    'Nodejs is ............',
    'Express is .............'
  ];

  var str = `
    <a href="/topic?id=0">Javascript</a><br>
    <a href="/topic?id=1">Nodejs</a><br>
    <a href="/topic?id=2">Express</a><br>
  `;
  var output = str+topics[req.query.id];
  res.send(output);
})


app.get('/template',function(req,res){
  res.render('temp',{time:Date(), _title:'hi Javascript'});
})
app.get('/', function(req,res){
  res.send('Hello Javascript');
});

app.get('/login', function(req,res){
  res.send('Login Page!!')
})

app.listen(3000,function(){
  console.log('Connected to Port 3000');
});
