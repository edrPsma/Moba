using System;
using System.Collections.Generic;
using Google.Protobuf;

namespace KCPNetwork
{
    public class KCPTool
    {
        public static Action<string> Log;
        public static Action<string> Error;
        public static Action<string> Warn;

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] Serialize<T>(T msg) where T : IMessage
        {
            return msg.ToByteArray();
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static T DeSerialize<T>(byte[] bytes) where T : IMessage, new()
        {
            IMessage message = new T();
            try
            {
                return (T)message.Descriptor.Parser.ParseFrom(bytes);
            }
            catch (Exception e)
            {
                Error?.Invoke(e.ToString());
                throw;
            }
        }

        public static IMessage DeSerialize(short messageID, byte[] bytes)
        {
            IMessage message = null;
            try
            {
                return message.Descriptor.Parser.ParseFrom(bytes);
            }
            catch (Exception e)
            {
                Error?.Invoke(e.ToString());
                throw;
            }
        }
    }
}