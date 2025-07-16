namespace OpenMarketingApi.Models.ProblemsDetails
{
    [Serializable]
    public class I18NViolationException : Exception
    {
        public List<object> MessageErrors { get; set; }
        public RuleViolationType Rule { get; set; }
        public I18NViolationException(string message, RuleViolationType rule, List<object> messageErrors) : base(message) { Rule = rule; MessageErrors = messageErrors; }
        public I18NViolationException(string message, RuleViolationType rule, Exception inner, List<object> messageErrors) : base(message, inner) { Rule = rule; MessageErrors = messageErrors; }
    }
}
