using OpenMarketingApi.Models.Loyalty.Point;
using System.Text.Json.Nodes;
using ApiTools.Converters;
using OpenMarketingApi.Models.Loyalty.Transaction;

namespace OpenMarketingApi.Convertors
{
    public class PointTransactionConverter : JsonConverter<PointTransactionBasePOST>
    {
        public override PointTransactionBasePOST? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var transactionObj = JsonSerializer.Deserialize<JsonObject>(ref reader, options)!;
            if (transactionObj.ContainsKey(nameof(PointCorrectionPOST.Details)))
            {
                var pcc = HydratePointCorrectionPOST(transactionObj);
                return pcc;
            }
            else if (transactionObj.ContainsKey(nameof(PointTransactionPOST.Type)))
            {
                var ptt = HydratePointTransactionPOST(transactionObj);
                return ptt;
            }
            else
            {
                throw new BadHttpRequestException("Discriminator could not be identified : the body shall be of one of the 2 types PointTransactionPOST or PointCorrectionPOST");
            }
        }
        public override void Write(Utf8JsonWriter writer, PointTransactionBasePOST value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if (value is PointTransactionPOST pointTransaction)
            {
                WriteCommonProperties(writer, pointTransaction, options);
                WritePointTransactionProperties(writer, pointTransaction, options);
            }
            else if (value is PointCorrectionPOST pointCorrection)
            {
                WriteCommonProperties(writer, pointCorrection, options);
                WritePointCorrectionProperties(writer, pointCorrection, options);
            }
            else
            {
                throw new ArgumentException("Unsupported type for serialization", nameof(value));
            }

            writer.WriteEndObject();
        }

        private void WriteCommonProperties(Utf8JsonWriter writer, PointTransactionBasePOST value, JsonSerializerOptions options)
        {
            writer.WritePropertyName(nameof(PointTransactionBasePOST.LastUpdatedTimestamp));
            JsonSerializer.Serialize(writer, value.LastUpdatedTimestamp, options);
        }

        private void WritePointTransactionProperties(Utf8JsonWriter writer, PointTransactionPOST value, JsonSerializerOptions options)
        {
            JsonSerializerOptions option = new JsonSerializerOptions();
            option.Converters.Add(new DateOnlyConverter());
            writer.WritePropertyName(nameof(PointTransactionPOST.GamingDay));
            JsonSerializer.Serialize(writer, value.GamingDay, option);
        }

        private void WritePointCorrectionProperties(Utf8JsonWriter writer, PointCorrectionPOST value, JsonSerializerOptions options)
        {
            JsonSerializerOptions option = new JsonSerializerOptions();
            option.Converters.Add(new DateOnlyConverter());
            writer.WritePropertyName(nameof(PointCorrectionPOST.GamingDay));
            JsonSerializer.Serialize(writer, value.GamingDay, option);
        }

        private PointCorrectionPOST HydratePointCorrectionPOST(JsonObject transactionObj)
        {
            PointCorrectionPOST pc = new();
            if (transactionObj.ContainsKey("gamingDay"))
                pc.GamingDay = transactionObj["gamingDay"].GetValue<DateTime>();
            else
                throw new BadHttpRequestException("Property 'gamingDay' cannot be null");

            if (transactionObj.ContainsKey("siteId"))
                pc.SiteId = transactionObj["siteId"].GetValue<int>();
            else
                throw new BadHttpRequestException("Property 'siteId' cannot be null");

            if (transactionObj.ContainsKey("locationId"))
                pc.LocationId = transactionObj["locationId"].GetValue<string>();
            else
                throw new BadHttpRequestException("Property 'locationId' cannot be null");

            if (transactionObj.ContainsKey("locationType"))
            {
                if (Enum.TryParse(typeof(LocationTypeEnum), transactionObj["locationType"].GetValue<string>(), out var result))
                    pc.LocationType = (LocationTypeEnum)result;
                else
                    throw new BadHttpRequestException($"Property 'locationType' cannot be {transactionObj["locationType"]}");
            }
            else
                throw new BadHttpRequestException("Property 'locationType' cannot be null");

            if (transactionObj.ContainsKey("points"))
                pc.Points = transactionObj["points"].GetValue<decimal>();
            else
                throw new BadHttpRequestException("Property 'points' cannot be null");

            if (transactionObj.ContainsKey("impactLifePoints"))
                pc.ImpactLifePoints = transactionObj["impactLifePoints"].GetValue<bool>();
            else
                throw new BadHttpRequestException("Property 'impactLifePoints' cannot be null");

            if (transactionObj.ContainsKey("details"))
            {
                var detailsArray = transactionObj["details"].AsArray();
                pc.Details = new PointCorrectionDetailPOST[detailsArray.Count];
                for (int i = 0; i < detailsArray.Count; i++)
                {
                    var detailObj = detailsArray[i] as JsonObject;
                    pc.Details[i] = new PointCorrectionDetailPOST(detailObj);
                }
            }
            else
            {
                throw new BadHttpRequestException("Property 'details' cannot be null");
            }

            if (transactionObj.ContainsKey("creationUserCode"))
                pc.CreationUserCode = transactionObj["creationUserCode"]?.GetValue<string>();

            if (transactionObj.ContainsKey("lastUpdatedTimestamp"))
                pc.LastUpdatedTimestamp = transactionObj["lastUpdatedTimestamp"]?.GetValue<DateTime>();

            return pc;
        }

        private PointTransactionPOST HydratePointTransactionPOST(JsonObject transactionObj)
        {
            PointTransactionPOST pt = new();
            if (transactionObj.ContainsKey("gamingDay"))
                pt.GamingDay = transactionObj["gamingDay"].GetValue<DateTime>();
            else
                throw new BadHttpRequestException("Property 'gamingDay' cannot be null");

            if (transactionObj.ContainsKey("siteId"))
                pt.SiteId = transactionObj["siteId"].GetValue<int>();
            else
                throw new BadHttpRequestException("Property 'siteId' cannot be null");

            if (transactionObj.ContainsKey("locationId"))
                pt.LocationId = transactionObj["locationId"].GetValue<string>();
            else
                throw new BadHttpRequestException("Property 'locationId' cannot be null");

            if (transactionObj.ContainsKey("locationType"))
                pt.LocationType = (LocationTypeEnum)Enum.Parse(typeof(LocationTypeEnum), transactionObj["locationType"].GetValue<string>());
            else
                throw new BadHttpRequestException("Property 'locationType' cannot be null");

            if (transactionObj.ContainsKey("quantity"))
                pt.Quantity = transactionObj["quantity"].GetValue<int>();
            else
                throw new BadHttpRequestException("Property 'quantity' cannot be null");

            if (transactionObj.ContainsKey("type"))
                pt.Type = transactionObj["type"].GetValue<TransactionType>();
            else
                throw new BadHttpRequestException("Property 'type' cannot be null");

            if (transactionObj.ContainsKey("tenderId"))
                pt.TenderId = transactionObj["tenderId"].GetValue<string>();
            else
                throw new BadHttpRequestException("Property 'tenderId' cannot be null");

            if (transactionObj.ContainsKey("pointAmount"))
                pt.PointAmount = transactionObj["pointAmount"].GetValue<double>();
            else
                throw new BadHttpRequestException("Property 'pointAmount' cannot be null");

            if (transactionObj.ContainsKey("creationUserCode"))
                pt.CreationUserCode = transactionObj["creationUserCode"]?.GetValue<string>();

            if (transactionObj.ContainsKey("lastUpdatedTimestamp"))
                pt.LastUpdatedTimestamp = transactionObj["lastUpdatedTimestamp"]?.GetValue<DateTime>();

            return pt;
        }

    }
}
