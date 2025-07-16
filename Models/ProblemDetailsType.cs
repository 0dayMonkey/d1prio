
namespace ApiTools.Models
{
    /// <summary>
    /// Static class that contains keys for different types of exceptions.
    /// </summary>
    public static class ProblemDetailsType
    {
        /// <summary>
        /// Key for an exception related to ID document  violations.
        /// This key is used to categorize or identify specific exception types
        /// when handling errors in the application.
        /// Examples of rule violations related to this key include:
        /// <see cref="ReservedDocumentNumber"/>,
        /// <see cref="InValidDocumentNumber"/>, and
        /// <see cref="DuplicateDocumentNumber"/>.
        /// </summary>
        public const string IdDocI18N = "id-documents/i18n-violation";
    }
}
