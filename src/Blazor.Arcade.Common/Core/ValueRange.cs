//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Text.Json.Serialization;

namespace Blazor.Arcade.Common.Core
{
    public struct ValueRange : IEquatable<ValueRange>
    {
        public ValueRange(int min, int? max)
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException(
                    "Invalid range: minimum value must be less or equal to maximum.");
            }

            Min = min;
            Max = max;
        }

        [JsonPropertyName("min")]
        public int Min { get; set; } = 1;

        [JsonPropertyName("max")]
        public int? Max { get; set; }

        public static bool operator ==(ValueRange lhs, ValueRange rhs) =>
            lhs.Equals(rhs);

        public static bool operator !=(ValueRange lhs, ValueRange rhs) =>
            !lhs.Equals(rhs);

        public bool InRange(int value) =>
            (value >= Min && value <= (Max ?? int.MaxValue));

        public bool Equals(ValueRange other) =>
            (Min == other.Min && Max == other.Max);

        public override bool Equals(object? obj)
        {
            if (obj is ValueRange range)
            {
                return Equals(range);
            }

            return false;
        }

        public override int GetHashCode() =>
            Min.GetHashCode() ^ Max.GetHashCode();

        public override string ToString()
        {
            string? result;
            if (Max == null)
            {
                result = $"{Min}+";
            }
            else if (Min == Max)
            {
                result = $"{Min}";
            }
            else
            {
                result = $"{Min} - {Max}";
            }
            return result;
        }
    }
}
