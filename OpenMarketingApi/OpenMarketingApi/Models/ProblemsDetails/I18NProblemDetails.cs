namespace OpenMarketingApi.Models.ProblemsDetails
{
    public class I18NProblemDetails : ProblemDetails
    {
        public I18NRuleProblem ViolatonRule { get; set; }


        internal I18NProblemDetails(int status, string path, I18NViolationException exc)
        {
            Type = ProblemDetailsType.IdDocI18N;
            Detail = exc.Message;
            Title = exc.Message;
            Instance = path;
            Status = status;
            ViolatonRule = new I18NRuleProblem() { Rule = exc.Rule, Errors = exc.MessageErrors };
        }
    }

    public class I18NRuleProblem
    {
        public List<object> Errors { get; set; }
        public RuleViolationType Rule { get; set; }
    }
}
