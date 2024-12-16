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
* FFmpeg 7.0:
  | Package                         | Link                                                                                                                                       |
  | ------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------ |
  | Sdcb.FFmpeg                     | [![NuGet](https://img.shields.io/nuget/v/Sdcb.FFmpeg.svg)](https://nuget.org/packages/Sdcb.FFmpeg)                            |
  | Sdcb.FFmpeg.runtime.windows-x64 | [![NuGet](https://img.shields.io/nuget/v/Sdcb.FFmpeg.runtime.windows-x64.svg)](https://nuget.org/packages/Sdcb.FFmpeg.runtime.windows-x64) |

* FFmpeg 6.1:
  | Package                         | Link                                                                                                                            |
  | ------------------------------- | ------------------------------------------------------------------------------------------------------------------------------- |
  | Sdcb.FFmpeg                     | [![NuGet](https://img.shields.io/badge/nuget-6.1.0.2-blue)](https://www.nuget.org/packages/Sdcb.FFmpeg/6.1.0.2)                     |
  | Sdcb.FFmpeg.runtime.windows-x64 | [![NuGet](https://img.shields.io/badge/nuget-6.1.1-blue)](https://www.nuget.org/packages/Sdcb.FFmpeg.runtime.windows-x64/6.1.1) |

* FFmpeg 4.4.3:
  | Package                         | Link                                                                                                                            |
  | ------------------------------- | ------------------------------------------------------------------------------------------------------------------------------- |
  | Sdcb.FFmpeg                     | [![NuGet](https://img.shields.io/badge/nuget-4.4.3-blue)](https://www.nuget.org/packages/Sdcb.FFmpeg/4.4.3)                     |
  | Sdcb.FFmpeg.runtime.windows-x64 | [![NuGet](https://img.shields.io/badge/nuget-4.4.3-blue)](https://www.nuget.org/packages/Sdcb.FFmpeg.runtime.windows-x64/4.4.3) |

* FFmpeg 5.1.2:
  | Package                         | Link                                                                                                                                       |
  | ------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------ |
  | Sdcb.FFmpeg                     | [![NuGet](https://img.shields.io/badge/nuget-5.1.2-blue)](https://nuget.org/packages/Sdcb.FFmpeg/5.1.2)                            |
  | Sdcb.FFmpeg.runtime.windows-x64 | [![NuGet](https://img.shields.io/badge/nuget-5.1.2-blue)](https://nuget.org/packages/Sdcb.FFmpeg.runtime.windows-x64/5.1.2) |

* FFmpeg 6.0:
  | Package                         | Link                                                                                                                                       |
  | ------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------ |
  | Sdcb.FFmpeg                     | [![NuGet](https://img.shields.io/badge/nuget-6.0-blue)](https://nuget.org/packages/Sdcb.FFmpeg/6.0.26)                            |
  | Sdcb.FFmpeg.runtime.windows-x64 | [![NuGet](https://img.shields.io/badge/nuget-6.0-blue)](https://nuget.org/packages/Sdcb.FFmpeg.runtime.windows-x64/6.0.26) |


## Install

Install the Native asset nuget package:
* https://www.nuget.org/packages/Sdcb.FFmpeg

Note: 
* some API isn't stable enough and subjected to change at any time, please try install the specific version and keep a eye on latest updates.

You also need to install FFmpeg binaries native assets or related nuget packages:
- Windows:  
  Install `NuGet` package:
  * For FFmpeg 4.4.3: https://www.nuget.org/packages/Sdcb.FFmpeg.runtime.windows-x64/4.4.3
  * For FFmpeg 5.1.2: https://www.nuget.org/packages/Sdcb.FFmpeg.runtime.windows-x64/5.1.2

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

## Featured examples

### Example 1: Generate a video from code:

<details>

```csharp
// this example is based on Sdcb.FFmpeg 5.1.2
FFmpegLogger.LogWriter = (level, msg) => Console.Write(msg);

using FormatContext fc = FormatContext.AllocOutput(formatName: "mp4");
fc.VideoCodec = Codec.CommonEncoders.Libx264;
MediaStream vstream = fc.NewStream(fc.VideoCodec);
using CodecContext vcodec = new CodecContext(fc.VideoCodec)
{
	Width = 800,
	Height = 600,
	TimeBase = new AVRational(1, 30),
	PixelFormat = AVPixelFormat.Yuv420p,
	Flags = AV_CODEC_FLAG.GlobalHeader, 
};
vcodec.Open(fc.VideoCodec);
vstream.Codecpar!.CopyFrom(vcodec);
vstream.TimeBase = vcodec.TimeBase;

string outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "muxing.mp4");
fc.DumpFormat(streamIndex: 0, outputPath, isOutput: true);

using IOContext io = IOContext.OpenWrite(outputPath);
fc.Pb = io;
fc.WriteHeader();
VideoFrameGenerator.Yuv420pSequence(vcodec.Width, vcodec.Height, 600)
	.ConvertFrames(vcodec)
	.EncodeAllFrames(fc, null, vcodec)
	.WriteAll(fc);
fc.WriteTrailer();
```
</details>

### Example 2: Decode and remuxing mp4:

<details>

```csharp
// this example is based on Sdcb.FFmpeg 4.4.2
void A7r3VideoToWechat(string mp4Path)
{
	using FormatContext inFc = FormatContext.OpenInputUrl(mp4Path);
	inFc.LoadStreamInfo();

	// prepare input stream/codec
	MediaStream inAudioStream = inFc.GetAudioStream();
	using CodecContext audioDecoder = new(Codec.FindDecoderById(inAudioStream.Codecpar!.CodecId));
	audioDecoder.FillParameters(inAudioStream.Codecpar);
	audioDecoder.Open();
	audioDecoder.ChannelLayout = (ulong)ffmpeg.av_get_default_channel_layout(audioDecoder.Channels);

	MediaStream inVideoStream = inFc.GetVideoStream();
	using CodecContext videoDecoder = new(Codec.FindDecoderByName("h264_qsv"));
	videoDecoder.FillParameters(inVideoStream.Codecpar!);
	videoDecoder.Open();

	// dest file
	string destFile = Path.Combine(Path.GetDirectoryName(mp4Path)!, Path.GetFileNameWithoutExtension(mp4Path) + "_wechat.mp4");
	using FormatContext outFc = FormatContext.AllocOutput(fileName: destFile);

	// dest encoder and streams
	outFc.AudioCodec = Codec.CommonEncoders.AAC;
	MediaStream outAudioStream = outFc.NewStream(outFc.AudioCodec);
	using CodecContext audioEncoder = new(outFc.AudioCodec)
	{
		Channels = 1,
		SampleFormat = outFc.AudioCodec.Value.NegociateSampleFormat(AVSampleFormat.Fltp),
		SampleRate = outFc.AudioCodec.Value.NegociateSampleRates(48000),
		BitRate = 48000
	};
	audioEncoder.ChannelLayout = (ulong)ffmpeg.av_get_default_channel_layout(audioEncoder.Channels);
	audioEncoder.TimeBase = new AVRational(1, audioEncoder.SampleRate);
	audioEncoder.Open(outFc.AudioCodec);
	outAudioStream.Codecpar!.CopyFrom(audioEncoder);

	outFc.VideoCodec = Codec.FindEncoderByName("libx264");
	MediaStream outVideoStream = outFc.NewStream(outFc.VideoCodec);
	using VideoFilterContext vfilter = VideoFilterContext.Create(inVideoStream, "scale=1024:-1");
	using CodecContext videoEncoder = new(outFc.VideoCodec)
	{
		Flags = AV_CODEC_FLAG.GlobalHeader,
		ThreadCount = Environment.ProcessorCount, 
		ThreadType = ffmpeg.FF_THREAD_FRAME,
	};
	vfilter.ConfigureEncoder(videoEncoder);
	var dict = new MediaDictionary
	{
		["crf"] = "30",
		["preset"] = "veryslow"
	};
	videoEncoder.Open(outFc.VideoCodec, dict);
	dict.Dump();
	outVideoStream.Codecpar!.CopyFrom(videoEncoder);
	outVideoStream.TimeBase = videoEncoder.TimeBase;

	// begin write
	using IOContext io = IOContext.OpenWrite(destFile);
	outFc.Pb = io;
	outFc.WriteHeader();

	MediaThreadQueue<Frame> decodingQueue = inFc
		.ReadPackets(inVideoStream.Index, inAudioStream.Index)
		.DecodeAllPackets(inFc, audioDecoder, videoDecoder)
		.ToThreadQueue(cancellationToken: QueryCancelToken, boundedCapacity: 64);

	MediaThreadQueue<Packet> encodingQueue = decodingQueue.GetConsumingEnumerable()
		.ApplyVideoFilters(vfilter)
		.ConvertAllFrames(audioEncoder, videoEncoder)
		.AudioFifo(audioEncoder)
		.EncodeAllFrames(outFc, audioEncoder, videoEncoder)
		.ToThreadQueue(cancellationToken: QueryCancelToken);

	CancellationTokenSource end = new();
	QueryCancelToken.Register(() => end.Cancel());
	Dictionary<int, PtsDts> ptsDts = new();
	Task.Run(async () =>
	{
		double totalDuration = Math.Max(inVideoStream.GetDurationInSeconds(), inAudioStream.GetDurationInSeconds());
		try
		{
			while (!end.IsCancellationRequested)
			{
				Log();
				await Task.Delay(1000, end.Token);
			}
		}
		finally
		{
			Log();
		}

		void Log() => Console.WriteLine($"{GetStatusText()}, dec/enc queue: {decodingQueue.Count}/{encodingQueue.Count}");
		string GetStatusText() => $"{(outVideoStream.TimeBase * ptsDts.GetValueOrDefault(outVideoStream.Index, PtsDts.Default).Dts).ToDouble():F2} of {totalDuration:F2}";
	});
	encodingQueue.GetConsumingEnumerable()
		.RecordPtsDts(ptsDts)
		.WriteAll(outFc);
	end.Cancel();
	outFc.WriteTrailer();
}
```
</details>

### Example 3: Create gif emoji based on video

This demo is also avaiable here: https://ffmpeg-sorry-demo.starworks.cc:88/

This demo Visual Studio source code is also available here: https://github.com/sdcb/ffmpeg-wjz-sorry-generator

<details>

```csharp
// This example is initially written based on Sdcb.FFmpeg 4.4.3 + Vortice.Direct2D1
#nullable enable

void Main()
{
	FFmpegLogger.LogWriter = (level, msg) => Console.Write(msg);
	byte[] videoBytes = CreateGif(239, 239, timebase: new AVRational(1, 30), duration: new AVRational(1, 1), RenderOneFrame);
	File.WriteAllBytes(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "muxing.gif"), videoBytes);
	Util.Image(videoBytes, Util.ScaleMode.Unscaled).Dump(videoBytes.Length.ToString());
}

static void RenderOneFrame(VideoTime time, ID2D1RenderTarget ctx, DxRes res)
{
	using IDWriteTextFormat font = res.DWriteFactory.CreateTextFormat("Consolas", 40.0f);
	ctx.Clear(Colors.Transparent);
	ctx.Transform = Matrix3x2.CreateRotation(time.Percent * MathF.PI * 2, new Vector2(ctx.Size.Width / 2, ctx.Size.Height / 2));
	using var layout = res.DWriteFactory.CreateTextLayout("Test1234!", font, int.MaxValue, int.MaxValue);
	ctx.DrawTextLayout(new Vector2(ctx.Size.Width / 2 - layout.Metrics.Width / 2, ctx.Size.Height / 2 - layout.Metrics.Height / 2), layout, res.GetColor(Colors.Red));
}

public static byte[] CreateGif(int width, int height, AVRational timebase, AVRational duration, FrameRendererDelegate frameRenderer)
{
    using FormatContext fc = FormatContext.AllocOutput(formatName: "gif");
    fc.VideoCodec = Codec.FindEncoderById(AVCodecID.Gif);
    MediaStream vstream = fc.NewStream(fc.VideoCodec);
    using CodecContext vcodec = new CodecContext(fc.VideoCodec)
    {
        Width = width,
        Height = height,
        TimeBase = timebase,
        PixelFormat = AVPixelFormat.Pal8,
    };
    vcodec.Open(fc.VideoCodec);
    vstream.Codecpar!.CopyFrom(vcodec);
    vstream.TimeBase = vcodec.TimeBase;

    using DynamicIOContext io = IOContext.OpenDynamic();
    fc.Pb = io;
    fc.WriteHeader();
	int frameCount = (int)Math.Ceiling(duration.ToDouble() / timebase.ToDouble());
    RenderAll(vcodec, frameRenderer, frameCount: frameCount)
		//.ConvertFrames(vcodec)
		.ApplyVideoFilters(timebase, AVPixelFormat.Pal8, $"scale=flags=lanczos,split[s0][s1];[s0]palettegen[p];[s1][p]paletteuse")
        .EncodeAllFrames(fc, null, vcodec)
        .WriteAll(fc);
    fc.WriteTrailer();
    return io.GetBuffer().ToArray();

	static IEnumerable<Frame> RenderAll(CodecContext codecCtx, FrameRendererDelegate frameRenderer, int frameCount)
	{
		using DxRes basic = new(codecCtx.Width, codecCtx.Height);
		using VideoFrameConverter frameConverter = new();
		using Frame rgbFrame = new Frame()
		{
			Width = codecCtx.Width,
			Height = codecCtx.Height,
			Format = (int)AVPixelFormat.Bgra
		};
		using Frame refFrame = new();

		for (int i = 0; i < frameCount; ++i)
		{
			ID2D1RenderTarget ctx = basic.RenderTarget;
			VideoTime time = new(i, TimeSpan.FromSeconds(1.0 * i * codecCtx.TimeBase.Num / codecCtx.TimeBase.Den), frameCount);
			ctx.BeginDraw();
			frameRenderer(time, ctx, basic);
			ctx.EndDraw();

			using (IWICBitmapLock bmpLock = basic.WicBmp.Lock(BitmapLockFlags.Read))
			{
				rgbFrame.Data._0 = bmpLock.Data.DataPointer;
				rgbFrame.Linesize[0] = bmpLock.Data.Pitch;
				refFrame.Ref(rgbFrame);
				yield return refFrame;
			}
		}
	}
}

public delegate void FrameRendererDelegate(VideoTime time, ID2D1RenderTarget ctx, DxRes res);

public record struct VideoTime(int Frame, TimeSpan Elapsed, int TotalFrame)
{
	public float Percent => 1.0f * Frame / TotalFrame;
}

public class DxRes : IDisposable
{
    public readonly IWICImagingFactory WicFactory = new IWICImagingFactory();
    public readonly ID2D1Factory2 D2dFactory = D2D1.D2D1CreateFactory<ID2D1Factory2>();
    public readonly IWICBitmap WicBmp;
    public readonly ID2D1RenderTarget RenderTarget;
    private readonly ID2D1SolidColorBrush DefaultColor;
    public readonly IDWriteFactory DWriteFactory = DWrite.DWriteCreateFactory<IDWriteFactory>();

    public DxRes(int width, int height)
    {
        WicBmp = WicFactory.CreateBitmap(width, height, Vortice.WIC.PixelFormat.Format32bppPBGRA, BitmapCreateCacheOption.CacheOnLoad);
        RenderTarget = D2dFactory.CreateWicBitmapRenderTarget(WicBmp, new RenderTargetProperties(new Vortice.DCommon.PixelFormat(Format.B8G8R8A8_UNorm, Vortice.DCommon.AlphaMode.Premultiplied)));
        DefaultColor = RenderTarget.CreateSolidColorBrush(Colors.CornflowerBlue);
    }

    public ID2D1SolidColorBrush GetColor(Color4 color)
	{
		DefaultColor.Color = color;
		return DefaultColor;
	}

	public void Dispose()
	{
		DefaultColor.Dispose();
		RenderTarget.Dispose();
		WicBmp.Dispose();
		D2dFactory.Dispose();
		WicFactory.Dispose();
		DWriteFactory.Dispose();
	}
}
```
</details>

### Example 4: Streaming screen and transfer via network

Server side code:

<details>

```csharp
// This example was initially written based on Sdcb.FFmpeg 4.4.3 & Sdcb.ScreenCapture
void Main()
{
	StartService(QueryCancelToken);
}

void StartService(CancellationToken cancellationToken = default)
{
	var tcpListener = new TcpListener(IPAddress.Any, 5555);
	cancellationToken.Register(() => tcpListener.Stop());
	tcpListener.Start();

	while (!cancellationToken.IsCancellationRequested)
	{
		TcpClient client = tcpListener.AcceptTcpClient();
		Task.Run(() => ServeClient(client, cancellationToken));
	}
}

void ServeClient(TcpClient tcpClient, CancellationToken cancellationToken = default)
{
	try
	{
		using var _ = tcpClient;
		using NetworkStream stream = tcpClient.GetStream();
		using BinaryWriter writer = new(stream);
		RectI screenSize = ScreenCapture.GetScreenSize(screenId: 0);
		RdpCodecParameter rcp = new(AVCodecID.H264, screenSize.Width, screenSize.Height, AVPixelFormat.Bgr0);

		using CodecContext cc = new(Codec.CommonEncoders.Libx264RGB)
		{
			Width = rcp.Width,
			Height = rcp.Height,
			PixelFormat = rcp.PixelFormat,
			TimeBase = new AVRational(1, 20),
		};
		cc.Open(null, new MediaDictionary
		{
			["crf"] = "30",
			["tune"] = "zerolatency",
			["preset"] = "veryfast"
		});

		writer.Write(rcp.ToArray());
		using Frame source = new();
		foreach (Packet packet in ScreenCapture
			.CaptureScreenFrames(screenId: 0)
			.ToBgraFrame()
			.ConvertFrames(cc)
			.EncodeFrames(cc))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				break;
			}
			writer.Write(packet.Data.Length);
			writer.Write(packet.Data.AsSpan());
		}
	}
	catch (IOException ex)
	{
		// Unable to write data to the transport connection: 远程主机强迫关闭了一个现有的连接。.
		// Unable to write data to the transport connection: 你的主机中的软件中止了一个已建立的连接。
		ex.Dump();
	}
}

public class Filo<T> : IDisposable
{
	private T? Item { get; set; }
	private ManualResetEventSlim Notify { get; } = new ManualResetEventSlim();

	public void Update(T item)
	{
		Item = item;
		Notify.Set();
	}

	public IEnumerable<T> Consume(CancellationToken cancellationToken = default)
	{
		while (!cancellationToken.IsCancellationRequested)
		{
			Notify.Wait(cancellationToken);
			yield return Item!;
		}
	}

	public void Dispose() => Notify.Dispose();
}

public static class BgraFrameExtensions
{
	public static IEnumerable<Frame> ToBgraFrame(this IEnumerable<LockedBgraFrame> bgras)
	{
		using Frame frame = new Frame();
		foreach (LockedBgraFrame bgra in bgras)
		{
			frame.Width = bgra.Width;
			frame.Height = bgra.Height;
			frame.Format = (int)AVPixelFormat.Bgra;
			frame.Data[0] = bgra.DataPointer;
			frame.Linesize[0] = bgra.RowPitch;
			yield return frame;
		}
	}
}

record RdpCodecParameter(AVCodecID CodecId, int Width, int Height, AVPixelFormat PixelFormat)
{
	public byte[] ToArray()
	{
		byte[] data = new byte[16];
		Span<byte> span = data.AsSpan();
		BinaryPrimitives.WriteInt32LittleEndian(span, (int)CodecId);
		BinaryPrimitives.WriteInt32LittleEndian(span[4..], Width);
		BinaryPrimitives.WriteInt32LittleEndian(span[8..], Height);
		BinaryPrimitives.WriteInt32LittleEndian(span[12..], (int)PixelFormat);
		return data;
	}
}
```
</details>

Client side code:

<details>

```csharp
// This example was initially written based on Sdcb.FFmpeg 4.4.3 & FlysEngine.Desktop
#nullable enable

ManagedBgraFrame? managedFrame = null;
bool cancel = false;

unsafe void Main()
{
	using RenderWindow w = new();
	w.FormClosed += delegate { cancel = true; };
	Task decodingTask = Task.Run(() => DecodeThread(() => (3840, 2160)));

	w.Draw += (_, ctx) =>
	{
		ctx.Clear(Colors.CornflowerBlue);
		if (managedFrame == null) return;

		ManagedBgraFrame frame = managedFrame.Value;

		fixed (byte* ptr = frame.Data)
		{
			//new System.Drawing.Bitmap(frame.Width, frame.Height, frame.RowPitch, System.Drawing.Imaging.PixelFormat.Format32bppPArgb, (IntPtr)ptr).DumpUnscaled();
			BitmapProperties1 props = new(new PixelFormat(Format.B8G8R8A8_UNorm, Vortice.DCommon.AlphaMode.Premultiplied));
			using ID2D1Bitmap bmp = ctx.CreateBitmap(new SizeI(frame.Width, frame.Height), (IntPtr)ptr, frame.RowPitch, props);
			ctx.UnitMode = UnitMode.Dips;
			ctx.DrawBitmap(bmp, 1.0f, InterpolationMode.NearestNeighbor);
		}
	};
	RenderLoop.Run(w, () => w.Render(1, Vortice.DXGI.PresentFlags.None));
}

async Task DecodeThread(Func<(int width, int height)> sizeAccessor)
{
	using TcpClient client = new TcpClient();
	await client.ConnectAsync(IPAddress.Loopback, 5555);
	using NetworkStream stream = client.GetStream();

	using BinaryReader reader = new(stream);
	RdpCodecParameter rcp = RdpCodecParameter.FromSpan(reader.ReadBytes(16));

	using CodecContext cc = new(Codec.FindDecoderById(rcp.CodecId))
	{
		Width = rcp.Width,
		Height = rcp.Height,
		PixelFormat = rcp.PixelFormat,
	};
	cc.Open(null);

	foreach (var frame in reader
		.ReadPackets()
		.DecodePackets(cc)
		.ConvertVideoFrames(sizeAccessor, AVPixelFormat.Bgra)
		.ToManaged()
		)
	{
		if (cancel) break;
		managedFrame = frame;
	}
}


public static class FramesExtensions
{
	public static IEnumerable<ManagedBgraFrame> ToManaged(this IEnumerable<Frame> bgraFrames, bool unref = true)
	{
		foreach (Frame frame in bgraFrames)
		{
			int rowPitch = frame.Linesize[0];
			int length = rowPitch * frame.Height;
			byte[] buffer = new byte[length];
			Marshal.Copy(frame.Data._0, buffer, 0, length);
			ManagedBgraFrame managed = new(buffer, length, length / frame.Height);
			if (unref) frame.Unref();
			yield return managed;
		}
	}
}

public record struct ManagedBgraFrame(byte[] Data, int Length, int RowPitch)
{
	public int Width => RowPitch / BytePerPixel;
	public int Height => Length / RowPitch;

	public const int BytePerPixel = 4;
}


public static class ReadPacketExtensions
{
	public static IEnumerable<Packet> ReadPackets(this BinaryReader reader)
	{
		using Packet packet = new();
		while (true)
		{
			int packetSize = reader.ReadInt32();
			if (packetSize == 0) yield break;

			byte[] data = reader.ReadBytes(packetSize);
			GCHandle dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
			try
			{
				packet.Data = new DataPointer(dataHandle.AddrOfPinnedObject(), packetSize);
				yield return packet;
			}
			finally
			{
				dataHandle.Free();
			}
		}
	}
}

record RdpCodecParameter(AVCodecID CodecId, int Width, int Height, AVPixelFormat PixelFormat)
{
	public static RdpCodecParameter FromSpan(ReadOnlySpan<byte> data)
	{
		return new RdpCodecParameter(
			CodecId: (AVCodecID)BinaryPrimitives.ReadInt32LittleEndian(data),
			Width: BinaryPrimitives.ReadInt32LittleEndian(data[4..]),
			Height: BinaryPrimitives.ReadInt32LittleEndian(data[8..]),
			PixelFormat: (AVPixelFormat)BinaryPrimitives.ReadInt32LittleEndian(data[12..]));
	}
}
```
</details>

### Example 5: Decode RTSP camera stream and show

<details>

```csharp
// This example was initially written using Sdcb.FFmpeg 4.4.3 & Vortice.Direct2D1
#nullable enable

FFmpegBmp? ffBmp = null;
FFmpegBmp? lastFFbmp = null;
FFmpegLogger.LogWriter = (level, msg) => Util.FixedFont(msg).Dump();
CancellationTokenSource cts = new ();

using RenderWindow w = new();
Task.Run(() => DecodeRTSP(Util.GetPassword("home-rtsp-ipc"), cts.Token));
w.Draw += (_, ctx) =>
{
	if (ffBmp == null) return;
	if (lastFFbmp == ffBmp) return;

	GCHandle handle = GCHandle.Alloc(ffBmp.Data, GCHandleType.Pinned);
	try
	{
		using ID2D1Bitmap bmp = ctx.CreateBitmap(new SizeI(ffBmp.Width, ffBmp.Height), handle.AddrOfPinnedObject(), ffBmp.RowPitch, new BitmapProperties(new Vortice.DCommon.PixelFormat(Format.B8G8R8A8_UNorm, Vortice.DCommon.AlphaMode.Premultiplied)));
		lastFFbmp = ffBmp;
		Size clientSize = ctx.Size;
		float top = (clientSize.Height - ffBmp.Height) / 2;
		ctx.Transform = Matrix3x2.CreateTranslation(0, top);
		ctx.DrawBitmap(bmp, 1.0f, InterpolationMode.Linear);
	}
	finally
	{
		handle.Free();
	}
};
w.FormClosing += delegate { cts.Cancel(); };
RenderLoop.Run(w, () => w.Render(1, Vortice.DXGI.PresentFlags.None));

void DecodeRTSP(string url, CancellationToken cancellationToken = default)
{
	using FormatContext fc = FormatContext.OpenInputUrl(url);
	fc.LoadStreamInfo();
	MediaStream videoStream = fc.GetVideoStream();
	
	using CodecContext videoDecoder = new CodecContext(Codec.FindDecoderByName("hevc_qsv"));
	videoDecoder.FillParameters(videoStream.Codecpar!);
	videoDecoder.Open();
	
	var dc = new DumpContainer().Dump();
	foreach (Frame frame in fc
		.ReadPackets(videoStream.Index)
		.DecodePackets(videoDecoder)
		.ConvertVideoFrames(() => new (w.ClientSize.Width, w.ClientSize.Width * videoDecoder.Height / videoDecoder.Width), AVPixelFormat.Bgr0))
	{
		if (cancellationToken.IsCancellationRequested) break;
		
		try
		{
			byte[] data = new byte[frame.Linesize[0] * frame.Height];
			Marshal.Copy(frame.Data._0, data, 0, data.Length);
			ffBmp = new FFmpegBmp(frame.Width, frame.Height, frame.Linesize[0], data);
		}
		finally
		{
			frame.Unref();
		}
	}
}

public record FFmpegBmp(int Width, int Height, int RowPitch, byte[] Data);
```
</details>

### Example 6: Read RTSP stream and save to multiple mp4/mov

<details>

```csharp
// The example was initially written using Sdcb.FFmpeg 4.4.3
FFmpegLogger.LogWriter = (level, msg) => Console.Write(Util.FixedFont(msg));

using FormatContext inFc = FormatContext.OpenInputUrl(Util.GetPassword("home-rtsp-ipc"));
inFc.LoadStreamInfo();
MediaStream inAudioStream = inFc.GetAudioStream();
MediaStream inVideoStream = inFc.GetVideoStream();
long gpts_v = 0, gpts_a = 0, gdts_v = 0, gdts_a = 0;

while (!QueryCancelToken.IsCancellationRequested)
{
	using FormatContext outFc = FormatContext.AllocOutput(formatName: "mov");
	string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "rtsp", DateTime.Now.ToString("yyyy-MM-dd"));
	Directory.CreateDirectory(dir.Dump());
	using IOContext io = IOContext.OpenWrite(Path.Combine(dir, $"{DateTime.Now:HHmmss}.mov"));
	outFc.Pb = io;

	MediaStream videoStream = outFc.NewStream(Codec.FindEncoderById(inVideoStream.Codecpar!.CodecId));
	videoStream.Codecpar!.CopyFrom(inVideoStream.Codecpar);
	videoStream.TimeBase = inVideoStream.RFrameRate.Inverse();
	videoStream.SampleAspectRatio = inVideoStream.SampleAspectRatio;

	MediaStream audioStream = outFc.NewStream(Codec.FindEncoderById(inAudioStream.Codecpar!.CodecId));
	audioStream.Codecpar!.CopyFrom(inAudioStream.Codecpar);
	audioStream.TimeBase = inAudioStream.TimeBase;
	audioStream.Codecpar.ChannelLayout = (ulong)ffmpeg.av_get_default_channel_layout(inAudioStream.Codecpar.Channels);

	outFc.WriteHeader();
	
	FilterPackets(inFc.ReadPackets(inAudioStream.Index, inVideoStream.Index), videoFrameCount: 60 * 20)
		.WriteAll(outFc);
	outFc.WriteTrailer();

	IEnumerable<Packet> FilterPackets(IEnumerable<Packet> packets, int videoFrameCount)
	{
		long pts_v = gpts_v, pts_a = gpts_a, dts_v = gdts_v, dts_a = gdts_a;
		long[] buffer = new long[200];
		long ithreshold = -1;
		int videoFrame = 0;

		foreach (Packet pkt in packets)
		{
			pkt.StreamIndex = pkt.StreamIndex == inAudioStream.Index ?
					audioStream.Index :
					videoStream.Index;
			if (pkt.StreamIndex == inAudioStream.Index)
			{
				// audio
				(gpts_a, gdts_a, pkt.Pts, pkt.Dts) = (pkt.Pts, pkt.Dts, pkt.Pts - pts_a, pkt.Dts - dts_a);
				pkt.RescaleTimestamp(inAudioStream.TimeBase, audioStream.TimeBase);
			}
			else
			{
				// video
				if (videoFrame < buffer.Length)
				{
					buffer[videoFrame] = pkt.Data.Length;
					ithreshold = -1;
				}
				else if (videoFrame == buffer.Length)
				{
					ithreshold = buffer.Order().ToArray()[buffer.Length / 2] * 4;
				}
				
				if (videoFrame >= videoFrameCount && pkt.Data.Length > ithreshold)
				{
					break;
				}

				(gpts_v, gdts_v, pkt.Pts, pkt.Dts) = (pkt.Pts, pkt.Dts, pkt.Pts - pts_v, pkt.Dts - dts_v);
				pkt.RescaleTimestamp(inVideoStream.TimeBase, videoStream.TimeBase);
				videoFrame++;
			}
			yield return pkt;
		}
	}
}
```
</details>

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
