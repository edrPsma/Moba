namespace Pool
{
    public interface IPool
    {
        EPoolType PoolType { get; }
        /// <summary>
        /// 容量
        /// </summary>
        /// <value></value>
        int Capacity { get; }

        /// <summary>
        /// 已使用
        /// </summary>
        /// <value></value>
        int Used { get; }

        /// <summary>
        /// 空闲数
        /// </summary>
        /// <value></value>
        int Free { get; }

        /// <summary>
        /// 从池内获取一个对象
        /// </summary>
        /// <returns></returns>
        object Spawn();

        /// <summary>
        /// 将对象放回池内
        /// </summary>
        /// <param name="obj"></param>
        void Release(object obj);

        /// <summary>
        /// 创建一个对象
        /// </summary>
        /// <returns></returns>
        object Create();

        /// <summary>
        /// 关闭
        /// </summary>
        void Dispose();
    }
}