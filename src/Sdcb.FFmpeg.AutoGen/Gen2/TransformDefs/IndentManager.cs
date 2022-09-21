using System;
using System.Linq;

namespace Sdcb.FFmpeg.AutoGen.Gen2.TransformDefs
{
    internal record IndentManager
    {
        public int Level { get; set; } = 0;
        public const string Ind = "    ";

        public IDisposable BeginScope()
        {
            Level += 1;
            return new DisposableWrapper(() => Level -= 1);
        }

        public string Wrap(string src)
        {
            return Level switch
            {
                0 => src,
                1 => Ind + src,
                2 => Ind + Ind + src,
                3 => Ind + Ind + Ind + src,
                4 => Ind + Ind + Ind + Ind + src,
                _ => string.Concat(Enumerable.Range(1, Level).Select(_ => Ind)) + src,
            };
        }

        private record DisposableWrapper(Action DisposeAction) : IDisposable
        {
            public void Dispose() => DisposeAction();
        }
    }
}
