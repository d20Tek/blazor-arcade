//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Blazor.Arcade.Common
{
    public static class StringExtensions
    {
        public static void ThrowIfEmpty(this string target, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(target))
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        public static void ThrowIfEmpty(this string target, Exception ex)
        {
            if (string.IsNullOrWhiteSpace(target))
            {
                throw ex;
            }
        }
    }
}
