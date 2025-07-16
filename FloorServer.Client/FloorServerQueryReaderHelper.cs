#region

using FloorServer.Client.Tools;
using System;
using System.Globalization;

#endregion

namespace FloorServer.Client
{
    public class FloorServerQueryReaderHelper : IQueryReaderHelper
    {
        #region Implementation of IQueryReaderHelper

        public string GetString(IQueryReader reader, string columName)
        {
            return reader.Current[columName];
        }

        public int? GetInt(IQueryReader reader, string columName)
        {
            var strValue = reader.Current[columName];

            if (string.IsNullOrEmpty(strValue))
                return null;

            return int.Parse(strValue);
        }

        public int GetInt(IQueryReader reader, string columName, int defaultValue)
        {
            int returnValue;
            var strValue = reader.Current[columName];
            return int.TryParse(strValue, out returnValue) ? returnValue : defaultValue;
        }

        public long? GetLong(IQueryReader reader, string columName)
        {
            var strValue = reader.Current[columName];

            if (string.IsNullOrEmpty(strValue))
                return null;

            return long.Parse(strValue);
        }

        public long GetLong(IQueryReader reader, string columName, long defaultValue)
        {
            long returnValue;
            var strValue = reader.Current[columName];
            return long.TryParse(strValue, out returnValue) ? returnValue : defaultValue;
        }

        public double? GetDouble(IQueryReader reader, string columName)
        {
            var strValue = reader.Current[columName];

            if (string.IsNullOrEmpty(strValue))
                return null;

            return double.Parse(strValue, CultureInfo.InvariantCulture);
        }

        public double GetDouble(IQueryReader reader, string columName, double defaultValue)
        {
            var strValue = reader.Current[columName];
            return string.IsNullOrEmpty(strValue) ? defaultValue : double.Parse(strValue, CultureInfo.InvariantCulture);
        }

        public short? GetShort(IQueryReader reader, string columName)
        {
            var strValue = reader.Current[columName];

            if (string.IsNullOrEmpty(strValue))
                return null;

            return short.Parse(strValue);
        }

        public short GetShort(IQueryReader reader, string columName, short defaultValue)
        {
            var strValue = reader.Current[columName];
            return string.IsNullOrEmpty(strValue) ? defaultValue : short.Parse(strValue);
        }

        public DateTime? GetDateTime(IQueryReader reader, string columName)
        {
            var strValue = reader.Current[columName];
            return GetDateTime(strValue);
        }

        public DateTime? GetDateTime(string dateTimeString)
        {
            long seconds;
            if (!long.TryParse(dateTimeString, out seconds) || seconds == 0)
                return null;

            return DateTools.UnixTimeToDateTime(seconds);
        }

        public DateTime GetDateTime(IQueryReader reader, string columName, DateTime defaultValue)
        {
            var value = GetDateTime(reader, columName);

            return value.HasValue ? value.Value : defaultValue;
        }

        #endregion
    }
}
