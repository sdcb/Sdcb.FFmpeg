using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sdcb.FFmpeg.Toolboxs.Extensions
{
    public class MediaQueue<T> : IDisposable where T : IDisposable
    {
        readonly BlockingCollection<T> _queue;
        private readonly Task _task;

        public string? Name { get; init; }

        public MediaQueue(BlockingCollection<T> queue, Task task, string? name = null)
        {
            _queue = queue;
            _task = task;
            Name = name;
        }

        public int Count => _queue.Count;

        public IEnumerable<T> GetConsumingEnumerable(bool dispose = true)
        {
            foreach (T frame in _queue.GetConsumingEnumerable())
            {
                try
                {
                    yield return frame;
                }
                finally
                {
                    if (dispose) frame.Dispose();
                }
            }
        }

        public void Dispose()
        {
            _task.Dispose();
            foreach (T item in _queue)
            {
                item.Dispose();
            }
            _queue.Dispose();
        }
    }

    public static class MediaQueueExtensions
    {
        public static MediaQueue<Frame> ToQueue(this IEnumerable<Frame> frames, int boundedCapacity = 512,
            string? name = null,
            ManualResetEventSlim? startEvent = null,
            bool unref = true,
            CancellationToken cancellationToken = default)
        {
            BlockingCollection<Frame> queue = new BlockingCollection<Frame>(boundedCapacity);
            MediaQueue<Frame> result = new MediaQueue<Frame>(queue, Task.Run(() =>
            {
                if (startEvent != null) startEvent.Wait(cancellationToken);

                try
                {
                    bool blockingWarned = false;
                    foreach (Frame frame in frames)
                    {
                        if (cancellationToken.IsCancellationRequested) break;

                        Frame storable = frame.Clone();
                        if (unref) frame.Unref();

                        storable.MakeWritable();
                        if (!queue.TryAdd(storable))
                        {
                            if (!blockingWarned)
                            {
                                FFmpegLogger.LogWarning($"{name ?? "Frame"} queue blocking, consider raising the {nameof(boundedCapacity)}(current value: {boundedCapacity})\n");
                                blockingWarned = true;
                            }

                            queue.Add(storable);
                        }
                    }
                }
                finally
                {
                    queue.CompleteAdding();
                }
            }), name);

            return result;
        }

        public static MediaQueue<Packet> ToQueue(this IEnumerable<Packet> packets, int boundedCapacity = 512,
            string? name = null,
            ManualResetEventSlim? startEvent = null,
            bool unref = true,
            CancellationToken cancellationToken = default)
        {
            BlockingCollection<Packet> queue = new BlockingCollection<Packet>(boundedCapacity);
            MediaQueue<Packet> result = new MediaQueue<Packet>(queue, Task.Run(() =>
            {
                if (startEvent != null) startEvent.Wait(cancellationToken);

                try
                {
                    bool blockingWarned = false;
                    foreach (Packet packet in packets)
                    {
                        if (cancellationToken.IsCancellationRequested) break;

                        Packet storable = packet.Clone();
                        if (unref) packet.Unref();

                        storable.MakeWritable();
                        if (!queue.TryAdd(storable))
                        {
                            if (!blockingWarned)
                            {
                                FFmpegLogger.LogWarning($"{name ?? "Frame"} queue blocking, consider raising the {nameof(boundedCapacity)}(current value: {boundedCapacity})\n");
                                blockingWarned = true;
                            }

                            queue.Add(storable);
                        }
                    }
                }
                finally
                {
                    queue.CompleteAdding();
                }
            }), name);

            return result;
        }
    }
}
