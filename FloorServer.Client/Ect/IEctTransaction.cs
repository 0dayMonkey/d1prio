#region

using FloorServer.Client.Enums;

#endregion

namespace FloorServer.Client.Ect
{
    public interface IEctTransaction
    {
        EctResultCodes Result { get; }
        double BookedAmount { get; }
        string CasinoId { get; }
        string EgmId { get; }
        string TransferId { get; }
    }
}
