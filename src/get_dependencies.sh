#!/bin/sh
cd ../deps
[ -d AdlMidi.NET ]   || git clone https://github.com/CSinkers/AdlMidi.NET
[ -d SerdesNet ]     || git clone https://github.com/CSinkers/SerdesNet
[ -d veldrid ]       || git clone https://github.com/mellinoe/veldrid
[ -d veldrid-spriv ] || git clone https://github.com/mellinoe/veldrid-spirv
cd AdlMidi.NET;   git pull --rebase; cd ..
cd SerdesNet;     git pull --rebase; cd ..
cd veldrid;       git pull --rebase; cd ..
cd veldrid-spirv; git pull --rebase; cd ..
cd ..
