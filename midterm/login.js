const express = require('express');
const app = express();

app.get('/',function(req,res){
  res.send(`
    <form action="/timeline" method="post">
    ID : <input type="text" name="id"> <br>
    Password : <input type="password" name="password"> <br>
    <input type="submit" value="로그인">  <input type="button" value="회원가입">
    </form>
    `);
});


app.listen(3000,function(){
  console.log('Connected to Port 3000');
});
