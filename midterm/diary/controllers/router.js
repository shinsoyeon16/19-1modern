const express = require('express');
const router = express.Router();
const usrCtrl=require('./diary/controllers');

router.get('/', useCtrl.list);
router.get('/timeline', funcion(req,res){
  res.send('timeline.js');
});
router.get('/join', funcion(req,res){
  res.send('join.js');
});

module.exports = router;
