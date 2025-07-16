using System;

namespace FloorServer.Client.Tools
{
    public class DateTools
    {
        public static DateTime UnixTimeToDateTime(long seconds)
        {
            DateTime converted = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime newDateTime = converted.AddSeconds(seconds);
            return newDateTime.ToLocalTime();
        }

        public static long DateTimeToUnixTime(DateTime date)
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            TimeSpan span = (date - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());

            //return the total seconds (which is a UNIX timestamp)
            return (long)span.TotalSeconds;
        }

        public static DateTime ToUTC(DateTime dateTime)
        {
            if (!TimeZoneInfo.Local.IsAmbiguousTime(dateTime))
            {
                return dateTime.ToUniversalTime();
            }
            else
            {
                var dt = DateTime.UtcNow;

                var tz = TimeZoneInfo.Local;
                var utcOffset = new DateTimeOffset(dt, TimeSpan.Zero);
                return DateTime.SpecifyKind(dateTime - utcOffset.ToOffset(tz.GetUtcOffset(utcOffset)).Offset,
                                            DateTimeKind.Utc);
            }
        }
    }
}
