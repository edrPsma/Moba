public static partial class Extension
{
    /// <summary>
    /// 是否为空字符串
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }

    /// <summary>
    /// 是否为非空字符串
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static bool IsNonEmpty(this string s)
    {
        if (s != null)
        {
            return !string.IsNullOrEmpty(s);
        }

        return false;
    }

    /// <summary>
    /// 删除所有空格
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string TrimAll(this string str)
    {
        return str.Replace(" ", "");
    }

    /// <summary>
    /// 删除所有空格回车换行符制表符
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string Condense(this string str)
    {
        return str.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
    }
}
