using System;
using System.Collections.Generic;
using FloorServer.Client.Boss;

namespace FloorServer.Client.Communication
{
    public interface IBossCommandFactory
    {
        Bom BuildCmd1300(string smHwId, string cardInSeqId, string playerId, string playerTitle, string playerLastName,
                         string playerFirstName, int playerHomeCasinoId, int playerLevelId, string playerLanguageId,
                         double playerPoints,
                         double playerPointsToday, DateTime lastVisitDate, bool isCardExpired, bool isCredentialUnknown,
                         string gender,
                         double ptsMultiplier, List<string> segmentIds, string playerCardNumber, int casinoId,
                         double? dailyMaxPointsAmount,
                         double luckFactor, bool? disablePoints);
    }
}