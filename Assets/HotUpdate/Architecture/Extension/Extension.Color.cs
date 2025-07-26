using UnityEngine;

public static partial class Extension
{
    /// <summary>
    /// 16进制颜色代码转颜色
    /// </summary>
    /// <param name="htmlString"></param>
    /// <returns></returns>
    public static Color HtmlStringToColor(this string htmlString)
    {
        if (ColorUtility.TryParseHtmlString(htmlString, out Color color))
        {
            return color;
        }
        else
        {
            return Color.white;
        }
    }
}
