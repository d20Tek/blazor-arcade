//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Blazor.Arcade.Common.Core
{
    public sealed class IdGenerator
    {
        private const string Encode_32_Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUV";
        private static long _lastId = DateTime.UtcNow.Ticks;

        private static readonly ThreadLocal<char[]> _charBufferThreadLocal =
            new ThreadLocal<char[]>(() =>
            {
                var buffer = new char[9];
                return buffer;
            });

        private IdGenerator() { }

        public static IdGenerator Instance { get; } = new IdGenerator();

        public string GetNext() => Generate(Interlocked.Increment(ref _lastId));

        public string GetNext(string prefix)
        {
            prefix.ThrowIfEmpty(nameof(prefix));
            var id = Generate(Interlocked.Increment(ref _lastId));
            return $"{prefix}-{id}";
        }

        private static string Generate(long id)
        {
            var buffer = _charBufferThreadLocal.Value;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            buffer[0] = Encode_32_Chars[(int)(id >> 40) & 31];
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            buffer[1] = Encode_32_Chars[(int)(id >> 35) & 31];
            buffer[2] = Encode_32_Chars[(int)(id >> 30) & 31];
            buffer[3] = Encode_32_Chars[(int)(id >> 25) & 31];
            buffer[4] = Encode_32_Chars[(int)(id >> 20) & 31];
            buffer[5] = Encode_32_Chars[(int)(id >> 15) & 31];
            buffer[6] = Encode_32_Chars[(int)(id >> 10) & 31];
            buffer[7] = Encode_32_Chars[(int)(id >> 5) & 31];
            buffer[8] = Encode_32_Chars[(int)id & 31];

            return new string(buffer, 0, buffer.Length);
        }
    }
}
