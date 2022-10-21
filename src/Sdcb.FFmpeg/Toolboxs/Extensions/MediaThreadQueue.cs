using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Sdcb.FFmpeg.Toolboxs.Extensions
{
    public class MediaThreadQueue<T> where T : IBufferRefed
    {
        private readonly BlockingCollection<T> _queue;
        private readonly Task _task;

        public string Name { get; }

        public int Count => _queue.Count;

        internal MediaThreadQueue(BlockingCollection<T> queue, Task task, string name)
        {
            _queue = queue;
            _task = task;
            Name = name;
        }

        public IEnumerable<T> GetConsumingEnumerable(bool dispose = true, bool discardAll = true)
        {
            try
            {
                foreach (T item in _queue.GetConsumingEnumerable())
                {
                    try
                    {
                        yield return item;
                    }
                    finally
                    {
                        if (dispose) item.Dispose();
                    }
                }
            }
            finally
            {
                if (discardAll)
                {
                    foreach (T item in _queue.GetConsumingEnumerable())
                    {
                        if (dispose) item.Dispose();
                    }
                }
                GC.KeepAlive(_task);
            }
        }
    }

    public static class MediaQueueExtensions
    {
        public static MediaThreadQueue<T> ToThreadQueue<T>(this IEnumerable<T> sources, int boundedCapacity = 64,
            string? name = null,
            ManualResetEventSlim? startEvent = null,
            bool unref = true,
            CancellationToken cancellationToken = default) where T : IBufferRefed
        {
            name = name ?? typeof(T).Name;
            BlockingCollection<T> queue = new BlockingCollection<T>(boundedCapacity);

            return new MediaThreadQueue<T>(queue, Task.Run(() =>
            {
                if (startEvent != null) startEvent.Wait(cancellationToken);

                try
                {
                    bool blockingWarned = false;
                    foreach (T source in sources)
                    {
                        if (cancellationToken.IsCancellationRequested) break;

                        T storable = (T)source.Clone();
                        if (unref) source.Unref();

                        storable.MakeWritable();
                        if (!queue.TryAdd(storable))
                        {
                            if (!blockingWarned)
                            {
                                FFmpegLogger.LogWarning($"{name} queue blocking, consider raising the {nameof(boundedCapacity)}(current value: {boundedCapacity})\n");
                                blockingWarned = true;
                            }

                            queue.Add(storable, cancellationToken);
                        }
                    }
                }
                finally
                {
                    queue.CompleteAdding();
                }
            }), name);
        }
    }
}
