namespace OpenMarketingApi.Models.Loyalty.Credential
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class CredentialPatch
    {
        /// <summary>
        /// Gets or Sets IsManualLocked
        /// </summary>

        [DataMember(Name = "isManualLocked")]
        public bool? IsManualLocked { get; set; }

        /// <summary>
        /// Pass null to reset the player PIN, else a 4 digits PIN
        /// </summary>
        /// <value>Pass null to reset the player PIN, else a 4 digits PIN</value>
        [DataMember(Name = "pinCode")]
        public string? PinCode { get; set; }

        /// <summary>
        /// Pass null to reset the player PIN, else a hash of the 4 digits PIN. If Provided, pinCode will be ignored
        /// </summary>
        /// <value>Pass null to reset the player PIN, else a hash of the 4 digits PIN. If Provided, pinCode will be ignored</value>
        [DataMember(Name = "pinCodeHash")]
        public string? PinCodeHash { get; set; }

        /// <summary>
        /// Gets or Sets FailedAttempt
        /// </summary>
        [DataMember(Name = "failedAttempt")]
        public int? FailedAttempt { get; set; }

        /// <summary>
        /// Gets or Sets LastUpdatedTimestamp
        /// </summary>
        [DataMember(Name = "lastUpdatedTimestamp")]
        public DateTime? LastUpdatedTimestamp { get; set; }
        
        public bool IsAllNull()
        {
            return IsManualLocked == null && PinCode == null && PinCodeHash == null && FailedAttempt == null && LastUpdatedTimestamp == null;
        }
    }
}
