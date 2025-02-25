namespace D20Tek.Arcade.Web.Common;

public static class EnumExtensions
{
    public static int Count<T>(this T _) where T : Enum
    {
        return Enum.GetValues(typeof(T)).Length;
    }

    public static int Count<T>() where T : struct, Enum
    {
        return Enum.GetValues<T>().Length;
    }
}