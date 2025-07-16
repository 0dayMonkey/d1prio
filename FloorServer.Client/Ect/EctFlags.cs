#region

using System;

#endregion

namespace FloorServer.Client.Ect
{
    [Flags]
    public enum EctFlags : short
    {
        NoUi = 0x0002,
        NoUIAndForceECT = 0x0006 //Used to force ECT for promo credits when no card is inserted in the CMOD
    }
}