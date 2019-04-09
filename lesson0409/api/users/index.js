const express = require('express');
const router = express.Router();
const usrCtrl=require('./user.controller');

router.get('/', usrCtrl.list);
router.get('/:id', (req,res)=> {
  var id = req.params.id;
  res.send('hello '+id);
})

module.exports = router;
