## Sdcb.FFmpeg [![main](https://github.com/sdcb/FFmpeg.AutoGen/actions/workflows/main.yml/badge.svg)](https://github.com/sdcb/FFmpeg.AutoGen/actions/workflows/main.yml)
FFmpeg auto generated unsafe bindings for C#/.NET, forked from https://github.com/Ruslan-B/FFmpeg.AutoGen.

Compared to original version, For low-level APIs, `Sdcb.FFmpeg` optimized for:
* Using standard `[DllImport]` instead of `LoadLibrary`
* Deleted common prefix or enums, for example it emited `AVCodecID.H264` instead of `AVCodecID.AV_CODEC_ID_H264`
* Combined same prefix macros into a enum, for example `AV_DICT_READ.MatchCase` instead of `AV_DICT_MATCH_CASE`
* Other optimization and fixs...

`Sdcb.FFmpeg` also provided some high level APIs:
* wrapper of class-like APIs like `FormatContext`/`CodecContext`/`MediaDictionary`
* helper of wrapping existing APIs like `FramesExtensions.ApplyFilters`
* some source generators like `VideoFrameGenerator.Yuv420pSequence`

For code generations, `Sdcb.FFmpeg` have benifits from:
* Minimized repository size, removed all ffmpeg `*.dll` binaries using `bfg`
* Auto download FFmpeg binaries from known existing sources

## NuGet Packages
* FFmpeg 4.4.2:
  | Package                         | Link                                                                                                                                       |
  | ------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------ |
  | Sdcb.FFmpeg                     | [![NuGet](https://img.shields.io/badge/nuget-4.4.2-blue)](https://www.nuget.org/packages/Sdcb.FFmpeg/4.4.2-preview.6)                      |
  | Sdcb.FFmpeg.runtime.windows-x64 | [![NuGet](https://img.shields.io/badge/nuget-4.4.2-blue)](https://www.nuget.org/packages/Sdcb.FFmpeg.runtime.windows-x64/4.4.2)            |

* FFmpeg 5.x:
  | Package                         | Link                                                                                                                                       |
  | ------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------ |
  | Sdcb.FFmpeg                     | [![NuGet](https://img.shields.io/nuget/vpre/Sdcb.FFmpeg.svg?color=red)](https://nuget.org/packages/Sdcb.FFmpeg)                            |
  | Sdcb.FFmpeg.runtime.windows-x64 | [![NuGet](https://img.shields.io/nuget/v/Sdcb.FFmpeg.runtime.windows-x64.svg)](https://nuget.org/packages/Sdcb.FFmpeg.runtime.windows-x64) |


## Install

Install the PInvoke nuget package(please try install the latest version of **4.x**):
* https://www.nuget.org/packages/Sdcb.FFmpeg/4.4.2-preview.6

Note: 
* I'm maintaince both `4.4.2` and `5.x` version at same time, but `4.4.2` is my higher priority. because current LTS version of Ubuntu is install `4.x` version by default.
* some API isn't stable enough and subjected to change at any time, please try install the specific version and keep a eye on latest updates.

You also need to install FFmpeg binaries native assets or related nuget packages:
- Windows:  
  Install `NuGet` package:
  * For FFmpeg 4.4.2: https://www.nuget.org/packages/Sdcb.FFmpeg.runtime.windows-x64/4.4.2
  * For FFmpeg 5.1.1: https://www.nuget.org/packages/Sdcb.FFmpeg.runtime.windows-x64/5.1.1

  **Note**: these packages is under published under **GPL** license, you can also download/compile your own native assets, `Sdcb.FFmpeg` will link to specific `ffmpeg` native dynamic libraries automatically according your environment variable.

- Linux:  
  Use your package manager of choice, in `Ubuntu 22.04` & `ffmpeg 4.4.2` specificly, you can write following commands:
  ```bash
  apt update
  apt install software-properties-common
  add-apt-repository ppa:savoury1/ffmpeg4 -y
  apt update
  apt install ffmpeg -y
  ```

  For `ffmpeg 5.x`, you can write following commands:
  ```bash
  apt update
  apt install software-properties-common
  add-apt-repository ppa:savoury1/ffmpeg4 -y
  add-apt-repository ppa:savoury1/ffmpeg5 -y
  apt update
  apt install ffmpeg -y
  ```

- Mac OS X:  
  Install ffmpeg via [Homebrew](https://formulae.brew.sh/formula/ffmpeg):
  ```bash
  brew install ffmpeg
  ```

For the more sophisticated operations please refer to offical [ffmpeg Documentation](https://www.ffmpeg.org/documentation.html) expecially API section of it.

## Tutorial and examples

You can refer to [Examples.cs](src/Sdcb.FFmpeg.Tests/Examples.cs) for multiple tutorial and examples.


## Build & Generation

The bindings generator uses [CppSharp](https://github.com/mono/CppSharp).

Prerequisites:
 - Visual Studio 2022 with C# and C++ desktop development workloads and Windows SDK for desktop.

Steps to generate:
- Run ```Sdcb.FFmpeg.AutoGen```
- All files with extension ```*.g.cs```  in ```Sdcb.FFmpeg``` project will be regenerated.


## License

Copyright © Sdcb, Ruslan Balanukhin 2022
All rights reserved.

Distributed under the GNU Lesser General Public License (LGPL) version 3.  
http://www.gnu.org/licenses/lgpl.html