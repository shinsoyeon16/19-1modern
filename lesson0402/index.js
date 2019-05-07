var express = require('express');
var app = express();

app.set('view engine', 'pug');
app.set('views','./views');
// app.use(bodyParer.urlencoded({extended:true}));
app.get('/new', function(req,res){
  res.render('new');
})
app.get('/topic', function(req,res){
  // fs.readdir('data',function(err,files){
  //   if(err)
  //     res.send('Internal File Error');
  //   res.render('view',{topics:files});
  // })
  var title = req.query.title;
  var desc = req.query.desc;

  fs.writeFile('data/'+title,desc,function(err){
    if(err)
      res.send('Internal File Error');
    res.send('Success!');
  })
})

app.listen(3000,function(){
  console.log('hello Nodejs');
})
