const express = require('express');
const app = express();

app.use('/controllers', require('./diary/controllers'));

app.listen(3000, () => {
  console.log('Server Connected!!!');
});
