#region

using System;

#endregion

namespace FloorServer.Client.Enums
{
    [Flags]
    public enum LiveMessagingFlags : short
    {
        Defaut = LiveMessageSynchronous | LiveMessagePermanent | LiveMessageExpiry | ClearBuffer,

        CardNumberMatchesOnly = 0x0001,
        CardNumberMismatchesOnly = 0x0002,
        CardNumberNotInsertedOnly = 0x0003,

        LiveMessageSynchronous = 0x0004,
        LiveMessagePermanent = 0x0008,
        LiveMessageTechnician = 0x0010,
        LiveMessageExpiry = 0x0800,
        
        RequestResult = 0x0100,
        ClearBuffer = 0x0400,
        NotDiscarded = 0x1000,
        MMIDMatchesOnly = 0x2000
    }
}
