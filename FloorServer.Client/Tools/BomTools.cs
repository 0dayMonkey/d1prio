#region

using FloorServer.Client.Boss;
using FloorServer.Client.Communication;

#endregion

namespace FloorServer.Client.Tools
{
    public class BomTools
    {
        /// <summary>
        /// Needs the ack.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns></returns>
        public static bool AckRequired(Bom msg)
        {
            if (msg.GetString(Tags.ACT).Equals(Tags.CMD))
                return true;

            return false;
        }


        /// <summary>
        /// Determines whether [is ack valid] [the specified MSG].
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="ack">The ack.</param>
        /// <returns>
        /// 	<c>true</c> if [is ack valid] [the specified MSG]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAckValid(Bom msg, Bom ack)
        {
            if (ack.ContainsKey(Keys.SEQ_NBR))
            {
                if (ack.GetString(Keys.SEQ_NBR).Equals(msg.GetString(Keys.SEQ_NBR)))
                {
                    if (ack.GetString(Tags.STATUS).Equals("OK"))
                        return true;
                }
            }
            return false;
        }
    }
}
