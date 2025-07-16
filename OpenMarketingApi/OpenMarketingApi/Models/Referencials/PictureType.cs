namespace OpenMarketingApi.Models
{

    /// <summary>
    /// Gets or Sets PictureType
    /// </summary>
    public enum PictureType
    {
        /// <summary>
        /// Enum ScanEnum for Scan
        /// </summary>
        [EnumMember(Value = "Scan")]
        ScanEnum = 0,
        /// <summary>
        /// Enum VideoEnum for Video
        /// </summary>
        [EnumMember(Value = "Video")]
        VideoEnum = 1,
        /// <summary>
        /// Enum SignatureEnum for Signature
        /// </summary>
        [EnumMember(Value = "Signature")]
        SignatureEnum = 2          }
}
