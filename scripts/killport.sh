#!/bin/bash
function killport() {
  tskill `netstat -ano | findstr LISTENING | findstr $1 | head -n 1 | sed -r 's/^(\s+[^\s]+){4}(\d*)$/\1/'`
  echo "Task listening on $1 closed."
}

killport 5001
