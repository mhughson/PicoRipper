# PicoRipper
Rips content from Pico8 (P8) files into a format that can be used by other engines.

# How to Build

Pico Ripper is a C# console application built in Visual Studio targeting .NET Framework 4.5.2.

Open [PicoRipper.sln](../blob/master/PicoRipper/PicoRipper.sln) in Visual Studio, and build.

.NET 4.5 is the only requirement.

# How to Use

Once Pico Ripper is built, navigate to the bin folder: PicoRipper\PicoRipper\PicoRipper\bin\Debug(or Release)\

There you will find PicoRipper.exe.

This executable requires command line parameters to work, so if you attempt to just run the exe, you will get some fatal errors:

![Error when no arguments are passed via command line.]({{site.baseurl}}//2017-10-01.png)

"Fatal: No arguments passed to program. Path to P8 file required."

