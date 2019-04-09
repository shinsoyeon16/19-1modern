const express = require('express');
const app = express();

app.use('/users', require('./api/users'));

app.listen(1234, () => {
  console.log('Connected!');
});
