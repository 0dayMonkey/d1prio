using System.Text;

namespace AddressLocation.Infrastructure.Database
{
    public static class ContextDbFunctions
    {
        public const string From_String = "ÉÈÊËÀÁÂÄÇÌÍÎÏÑÓÒÔÖÚÙÛÜ'-_*";
        public const string To_String = "EEEEAAAACIIIINOOOOUUUU    ";

        public static string Translate(string expr, string from, string to)
        {
            if (expr == null)
                return null;

            var result = new StringBuilder(expr.Length);

            for (int i = 0; i < expr.Length; i++)
            {
                char c = expr[i];
                int index = from.IndexOf(c);
                if (index != -1)
                {
                    if (index < to.Length)
                        result.Append(to[index]);
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }
    }
}
