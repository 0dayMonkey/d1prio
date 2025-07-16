#region

using System.Collections.Generic;
using System.Text;

#endregion

namespace FloorServer.Client.Tools
{
    /// <summary>
    /// The FloorCommandHelper helps you to create a where clause with the valid syntax according to the operation you specified.
    /// For example: FloorCommandHelper.Equal(Keys.SMDBID," 2002000") whill results in WHERE=#$SMDBID=" 2002000"#
    /// </summary>
    public class FloorCommandHelper
    {
        private static readonly string GenFormat = "{0}{1}\"{2}\"";
        private static readonly string AndFormat = "{0}{1}{2}";
        private static readonly string OrFormat = "{0}{1}{2}";
        private static readonly string ParenthesisFormat = "({0})";

        private static readonly string OperatorEqual = "==";
        private static readonly string OperatorNotEqual = "!=";
        private static readonly string OperatorGreater = ">";
        private static readonly string OperatorGreaterOrEqual = ">=";

        private static readonly string OperatorLower = "<";
        private static readonly string OperatorLowerOrEqual = "<=";

        private static readonly string OperatorAnd = "&&";
        private static readonly string OperatorOr = "||";

        private static readonly string NoWhereClause = "1";

        public static string Create(string operation, string leftValue, string rightValue)
        {
            return string.Format(GenFormat, leftValue, operation, rightValue);
        }

        public static string LowerOrEqual(string leftValue, string rightValue)
        {
            return Create(OperatorLowerOrEqual, leftValue, rightValue);
        }

        public static string Lower(string leftValue, string rightValue)
        {
            return Create(OperatorLower, leftValue, rightValue);
        }


        public static string GreaterOrEqual(string leftValue, string rightValue)
        {
            return Create(OperatorGreaterOrEqual, leftValue, rightValue);
        }

        public static string Greater(string leftValue, string rightValue)
        {
            return Create(OperatorGreater, leftValue, rightValue);
        }

        public static string Equal(string leftValue, string rightValue)
        {
            return Create(OperatorEqual, leftValue, rightValue);
        }

        public static string NotEqual(string leftValue, string rightValue)
        {
            return Create(OperatorNotEqual, leftValue, rightValue);
        }

        public static string Or(string leftValue, string rightValue)
        {
            // return Create(OperatorOr, leftValue, rightValue);
            return string.Format(OrFormat, leftValue, OperatorOr, rightValue);
        }

        public static string And(string leftValue, string rightValue)
        {
            //return Create(OperatorAnd, leftValue, rightValue);
            return string.Format(AndFormat, leftValue, OperatorAnd, rightValue);
        }

        public static string Parenthesis(string value)
        {
            return string.Format(ParenthesisFormat, value);
        }

        public static string Equal(string leftValue, IList<string> rightValues)
        {
            if (rightValues == null || rightValues.Count == 0)
                return NoWhereClause;

            if (rightValues.Count == 1)
                return Equal(leftValue, rightValues[0]);

            var sb = new StringBuilder();
            for (int i = 0; i < rightValues.Count; i++)
            {
                var op = string.Empty;
                if (i != 0)
                    op = string.Format(" {0} ", OperatorOr);

                sb.Append(string.Format("{0}{1}", op, Equal(leftValue, rightValues[i])));
            }

            return sb.ToString();
        }
    }
}
