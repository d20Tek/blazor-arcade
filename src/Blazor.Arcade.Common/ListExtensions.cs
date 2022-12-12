//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Blazor.Arcade.Common
{
    public static class ListExtensions
    {
        public static IList<T> ListOrDefault<T>(this IList<T>? target) =>
            target ?? new List<T>();

        public static T ObjectOrDefault<T>(this T? target)
            where T : class, new() =>
            target ?? new T();
    }
}
