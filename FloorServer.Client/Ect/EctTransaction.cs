#region

using System;
using FloorServer.Client.Communication;
using FloorServer.Client.Enums;
using FloorServer.Client.Tools;

#endregion

namespace FloorServer.Client.Ect
{
    public class EctTransaction : IEctTransaction
    {
        public string TransferId { get; set; }
        public EctResultCodes Result { get; set; }
        public double BookedAmount { get; set; }
        public string CasinoId { get; set; }
        public string EgmId { get; set; }
        public string PlayerCardNumber { get; set; }
        public string CardMediaNumber { get; set; }

        public EctTransaction(EctResultCodes result, double amount, string casinoId, string egmId, string plCardNumber)
        {
            Result = result;
            BookedAmount = amount;
            CasinoId = casinoId;
            EgmId = egmId;
            PlayerCardNumber = plCardNumber;
            CardMediaNumber = "";
        }

        public EctTransaction(string transferId, EctResultCodes result, double amount, string casinoId, string egmId, string plCardNumber)
        {
            TransferId = transferId;
            Result = result;
            BookedAmount = amount;
            CasinoId = casinoId;
            EgmId = egmId;
            PlayerCardNumber = plCardNumber;
            CardMediaNumber = "";
        }

        public EctTransaction(FloorEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Values[Keys.ECT_RESULT]) &&
                !string.IsNullOrEmpty(e.Values[Keys.ECT_AMOUNT]) &&
                !string.IsNullOrEmpty(e.Values[Keys.PL_HOME_CASINO]) &&
                !string.IsNullOrEmpty(e.Values[Keys.SMDBID]) &&
                !string.IsNullOrEmpty(e.Values[Keys.ECT_TRANSFER_ID])
                )
            {
                Result = (EctResultCodes)Convert.ToInt32(e.Values[Keys.ECT_RESULT]);
                BookedAmount = Convert.ToDouble(e.Values[Keys.ECT_AMOUNT]);
                CasinoId = e.Values[Keys.PL_HOME_CASINO];
                EgmId = e.Values[Keys.SMDBID];
                PlayerCardNumber = e.Values[Keys.PL_CARD_NR];
                CardMediaNumber = e.Values[Keys.CARD_MEDIA_TYPE];
                TransferId = e.Values[Keys.ECT_TRANSFER_ID];
            }
        }




    }
}
