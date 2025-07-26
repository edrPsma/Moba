using UnityEngine;

public static partial class Extension
{
    /// <summary>
    /// 设置Vector3向量的X值
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public static Vector3 SetX(this Vector3 vector, float x)
    {
        return new Vector3(x, vector.y, vector.z);
    }

    /// <summary>
    /// 设置Vector3向量的Y值
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static Vector3 SetY(this Vector3 vector, float y)
    {
        return new Vector3(vector.x, y, vector.z);
    }

    /// <summary>
    /// 设置Vector3向量的Z值
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public static Vector3 SetZ(this Vector3 vector, float z)
    {
        return new Vector3(vector.x, vector.y, z);
    }

    /// <summary>
    /// 设置Vector2向量的X值
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public static Vector2 SetX(this Vector2 vector, float x)
    {
        return new Vector2(x, vector.y);
    }

    /// <summary>
    /// 设置Vector2向量的Y值
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static Vector2 SetY(this Vector2 vector, float y)
    {
        return new Vector2(vector.x, y);
    }

    /// <summary>
    /// 修改Vector3向量的X值
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="addValue"></param>
    /// <returns></returns>
    public static Vector3 ModifyX(this Vector3 vector, float addValue)
    {
        return new Vector3(vector.x + addValue, vector.y, vector.z);
    }

    /// <summary>
    /// 修改Vector3向量的Y值
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="addValue"></param>
    /// <returns></returns>
    public static Vector3 ModifyY(this Vector3 vector, float addValue)
    {
        return new Vector3(vector.x, vector.y + addValue, vector.z);
    }

    /// <summary>
    /// 修改Vector3向量的Z值
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="addValue"></param>
    /// <returns></returns>
    public static Vector3 ModifyZ(this Vector3 vector, float addValue)
    {
        return new Vector3(vector.x, vector.y, vector.z + addValue);
    }

    /// <summary>
    /// 修改Vector2向量的X值
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="addValue"></param>
    /// <returns></returns>
    public static Vector2 ModifyX(this Vector2 vector, float addValue)
    {
        return new Vector3(vector.x + addValue, vector.y);
    }

    /// <summary>
    /// 修改Vector2向量的Y值
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="addValue"></param>
    /// <returns></returns>
    public static Vector2 ModifyY(this Vector2 vector, float addValue)
    {
        return new Vector3(vector.x, vector.y + addValue);
    }
}
