using System;
using System.Diagnostics;

namespace Sdcb.FFmpeg.AutoGen
{
    internal static class MetricHelper
    {
        public static T RecordTime<T>(string name, Func<T> action)
        {
            Console.WriteLine($"Start {name}...");
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                return action();
            }
            finally
            {
                Console.WriteLine($"Done, elapsed={sw.ElapsedMilliseconds}ms");
            }
        }

        public static void RecordTime(string name, Action action)
        {
            Console.WriteLine($"{name}...");
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                action();
            }
            finally
            {
                Console.WriteLine($"Done, elapsed={sw.ElapsedMilliseconds}ms");
            }
        }
    }
}
