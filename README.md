# PicoRipper
Rips content from Pico8 (P8) files into a format that can be used by other engines.

# How to Build

Pico Ripper is a C# console application built in Visual Studio targeting .NET Framework 4.5.2.

Open [PicoRipper.sln](../blob/master/PicoRipper/PicoRipper.sln) in Visual Studio, and build.

.NET 4.5 is the only requirement.

# Quick Start Guide

Find PicoRipper.exe in \PicoRipper\bin\Debug(or Release)\

Drag a .p8 file _on to_ PicoRipper.exe.

This will spit out 2 files into the directory where the p8 file lives:

- (filename).tmx (this is a Tiled TMX Map file with sprite flags stored in the SpriteSheet)
- (filename).png (this is the sprite sheet used by the TMX file)

In the future, this will include:

- Sound Effects
- Music
- Game Code

![](Mono8_MapRip2.gif)

This executable requires command line parameters to work, so if you attempt to just run the exe, you will get some fatal errors:

"Fatal: No arguments passed to program. Path to P8 file required."

# Command Line Arguments

If you wish to have more control over how Pico Ripper runs, you can do so with the following command line arguments:

[Documentation of Arguments](https://github.com/mhughson/PicoRipper/blob/master/PicoRipper/PicoRipper/Program.cs#L115-L121)


