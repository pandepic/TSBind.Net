#!/usr/bin/env node

const path = require('path');
const { spawn } = require('child_process');

const exePath = path.join(__dirname, 'TSBindDotNet.exe');

function runCLITool() {
  const args = process.argv.slice(2); // skip the first 2 node specific elements
  const child = spawn(exePath, args, { stdio: 'inherit' });

  child.on('close', (code) => {
    process.exit(code);
  });
}

runCLITool();