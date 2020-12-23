#!/bin/bash
function showport() {
  echo `netstat -ano | findstr LISTENING | findstr $1 `
}

showport 5001
