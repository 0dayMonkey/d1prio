#region

using System;

#endregion

namespace FloorServer.Client
{
    public interface IQueryReaderHelper
    {
        string GetString(IQueryReader reader, string columName);

        int? GetInt(IQueryReader reader, string columName);
        int GetInt(IQueryReader reader, string columName, int defaultValue);

        long? GetLong(IQueryReader reader, string columName);
        long GetLong(IQueryReader reader, string columName, long defaultValue);

        double? GetDouble(IQueryReader reader, string columName);
        double GetDouble(IQueryReader reader, string columName, double defaultValue);

        short? GetShort(IQueryReader reader, string columName);
        short GetShort(IQueryReader reader, string columName, short defaultValue);

        DateTime? GetDateTime(IQueryReader reader, string columName);
        DateTime GetDateTime(IQueryReader reader, string columName, DateTime defaultValue);
    }
}