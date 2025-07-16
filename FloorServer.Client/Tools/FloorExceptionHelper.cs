#region

using System;
using System.Collections.Specialized;
using FloorServer.Client.Boss;
using FloorServer.Client.Communication;

#endregion

namespace FloorServer.Client.Tools
{
    public class FloorExceptionHelper
    {
        public static FloorEventArgs ConvertToGenericFloorEvent(Bom e)
        {
            FloorEventArgs args = null;
            long exception = e.GetLong(Tags.EXC);
            long seq = e.GetLong(Keys.SEQ);
            long timestamp = e.GetLong(Keys.CR_TIM);
            switch (exception)
            {
                default:

                    StringDictionary readedFields = null;
                    if (e.ContainsKey(Tags.VALUES))
                    {
                        Bom values = e.GetMsg(Tags.VALUES);
                        readedFields = TramParser.ParseTrame(values.ToString());
                    }

                    args = new FloorEventArgs(exception, readedFields, seq, DateTools.ToUTC(DateTools.UnixTimeToDateTime(timestamp)));
                    break;
            }
            return args;
        }
    }
}
