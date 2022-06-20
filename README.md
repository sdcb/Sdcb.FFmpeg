## FFmpeg.AutoGen 
[![main](https://github.com/sdcb/FFmpeg.AutoGen/actions/workflows/main.yml/badge.svg)](https://github.com/sdcb/FFmpeg.AutoGen/actions/workflows/main.yml)
[![nuget](https://img.shields.io/nuget/v/Sdcb.FFmpeg.AutoGen.svg)](https://www.nuget.org/packages/Sdcb.FFmpeg.AutoGen/)

FFmpeg auto generated unsafe bindings for C#/.NET, forked from https://github.com/Ruslan-B/FFmpeg.AutoGen, optimized for:
* Using standard `[DllImport]` instead of `LoadLibrary`
* Minimized repository size, removed all ffmpeg `*.dll` binaries using `bfg`
* Auto download FFmpeg binaries from known existing sources
* Omiting shorter enum values like `AVCodecID.H264` instead of `AVCodecID.AV_CODEC_ID_H264`
* Omiting same prefix macro into a combined enum like `AVChannels.LayoutStereo` instead of `AV_CH_LAYOUT_STEREO`
* Other optimization and fixs...

## Usage

For the more sophisticated operations please refer to offical [ffmpeg Documentation](https://www.ffmpeg.org/documentation.html) expecially API section of it.

- on Windows:  
Install `NuGet` package: `https://www.nuget.org/packages/Sdcb.FFmpeg.runtime.windows-x64`

- on OS X:  
Install ffmpeg via [Homebrew](https://formulae.brew.sh/formula/ffmpeg):
```bash
brew install ffmpeg
```

- on Linux:  
Use your package manager of choice, in Ubuntu 22.04 & ffmpeg 5.0.1 specificly, you can write following commands:
```
apt update
apt install software-properties-common
add-apt-repository ppa:savoury1/ffmpeg4 -y
add-apt-repository ppa:savoury1/ffmpeg5 -y
apt update
apt install ffmpeg -y
```

## Generation

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

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
