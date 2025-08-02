using System;
public static partial class Utility
{
    public static class Time
    {
        /// <summary>
        /// 一秒的时间戳（单位：秒）
        /// </summary>
        public static long SECOND_TIME_STAMP = 1;

        /// <summary>
        /// 一分的时间戳（单位：秒）
        /// </summary>
        public static long MINUTES_TIME_STAMP = 60;

        /// <summary>
        /// 一小时的时间戳（单位：秒）
        /// </summary>
        public static long HOUR_TIME_STAMP = 3600;

        /// <summary>
        /// 一天的时间戳（单位：秒）
        /// </summary>
        public static long DAY_TIME_STAMP = 86400;
        /// <summary>
        /// 一天的时间戳（单位：毫秒）
        /// </summary>
        public static long DAY_TIME_STAMP_MILL = 86400000;

        /// <summary>
        /// 获取UTC时间戳（秒）
        /// </summary>
        /// <returns></returns>
        public static long GetCurUtcTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        /// <summary>
        /// UTC时间戳转UTC时间(秒)
        /// </summary>
        /// <param name="utcTimestamp"></param>
        /// <returns></returns>
        public static DateTime UtcTimestampToDateTime(long utcTimestamp)
        {
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(utcTimestamp);
            var dateTime = dateTimeOffset.DateTime;

            return dateTime;
        }

        /// <summary>
        /// UTC时间戳转本地时间
        /// </summary>
        /// <param name="utcTimestamp"></param>
        /// <returns></returns>
        public static DateTime UtcTimeToLocalDateTime(long utcTimestamp)
        {
            DateTime Adddate = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), TimeZoneInfo.Local);
            return Adddate.AddSeconds(utcTimestamp);
        }
    }
}
