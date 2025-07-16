using ApiTools.Converters;
using System.Text.Json.Nodes;

namespace OpenMarketingApi.Models.Loyalty.Point
{
    [DataContract]
    public class PointCorrectionDetailPOST
    {
        public PointCorrectionDetailPOST(JsonObject detailObj)
        {
            if (detailObj.ContainsKey("casinoId"))
                CasinoId = detailObj["casinoId"].GetValue<int>();
            else
                throw new BadHttpRequestException("Property 'casinoId' cannot be null");

            if (detailObj.ContainsKey("period"))
                Period = detailObj["period"].GetValue<DateTime>();
            else
                throw new BadHttpRequestException("Property 'period' cannot be null");

            if (detailObj.ContainsKey("points"))
                Points = detailObj["points"].GetValue<decimal>();
            else
                throw new BadHttpRequestException("Property 'points' cannot be null");
        }

        [Required]
        [DataMember(Name = "casinoId")]
        public int CasinoId { get; set; }

        [Required]
        [DataMember(Name = "period")]
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime Period { get; set; }

        [Required]
        [DataMember(Name = "points")]
        public decimal Points { get; set; }
    }
}
