#!/usr/bin/env node

const path = require('path');
const { spawn } = require('child_process');
const os = require('os');

try {
  function getPath() {
    switch (os.platform()) {
      case 'win32': return path.join('windows_x64', 'TSBindDotNet.exe');
      case 'darwin': return path.join('osx_x64', 'TSBindDotNet');
      case 'linux': return path.join('darwin_x64', 'TSBindDotNet');
      default: throw new Error('Unsupported platform: ' + os.platform());
    }
  }

  function runCLITool() {
    const exePath = path.join(__dirname, getPath());
    const args = process.argv.slice(2); // skip the first 2 node specific elements
    const child = spawn(exePath, args, { stdio: 'inherit' });

    child.on('close', (code) => {
      process.exit(code);
    });
  }

  runCLITool();
  
} catch (error) {
  console.error(error);
}