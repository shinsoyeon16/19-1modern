const express = require('express');
const app = express();

app.use('/users', require('.//users'));

app.listen(1234, () => {
  console.log('Connected!');
});
