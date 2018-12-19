osuTK [![dev chat](https://discordapp.com/api/guilds/188630481301012481/widget.png?style=shield)](https://discord.gg/ppy)
======

A fork of [OpenTK](https://github.com/opentk/opentk), graphics library, for use with osu/osu-framework. Adds .net standard, iOS and android support. 

osuTK is mainly OpenTK 3.0 compatible.

## Requirements

- A desktop platform with the .NET Core SDK 2.1 or higher installed or Android 5.0+ device or iOS device.
- When working with the codebase, we recommend using an IDE with intellisense and syntax highlighting, such as Visual Studio Community Edition (Windows), Visual Studio Code (with the C# plugin installed) or Jetbrains Rider (commercial). For development of mobile versions it's necessary to have Visual Studio Community Edition (Windows) or Visual Studio for Mac (MacOS) with Xamarin support and proper SDKs installed.

Features
========

- Create cutting-edge graphics with OpenGL 4.6 and OpenGL ES 3.0
- Spice up your GUI with 3d acceleration
- Improve your code flow with strong types and inline documentation
- Write once run everywhere

osuTK is available for Windows, Linux, Mac OS X, *BSD, SteamOS, Android and iOS.

Download [NuGet packages](http://www.nuget.org/packages/osuTK/)


Instructions
============

The simplest way to use osuTK in your project is to install the NuGet package from https://www.nuget.org/packages/ppy.osuTK.NS20/ or https://www.nuget.org/packages/ppy.osuTK.Android/ or https://www.nuget.org/packages/ppy.osuTK.iOS/.
If you want to try out the latest development commits, build from the `netstandard` branch.

To build osuTK from source, enter the source directory and run `./build.cmd` on Windows and `./build.sh` on Linux and Mac OS.
After this is done at least once, you can build osuTK normally through your IDE.

Documentation
=============

Your favorite IDE will display inline documentation for all osuTK APIs. Additional information can be found in the [OpenTK Repository](https://github.com/opentk/opentk).
