using System;
using System.Collections.Generic;
using System.Text;

namespace FloorServer.Client
{
    internal class RegisteredException
    {
        internal long ExceptionNum { get; set; }
        internal string WhereClause { get; set; }
        internal bool Persisted { get; set; }
        internal string ClientName { get; set; }
        internal string[] RequiredFields { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as RegisteredException);
        }

        public bool Equals(RegisteredException other)
        {
            return !(other == null) &&
                   ExceptionNum == other.ExceptionNum &&
                   ClientName == other.ClientName;
        }

        public override int GetHashCode()
        {
            int hashCode = -308291701;
            hashCode = hashCode * -1521134295 + ExceptionNum.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ClientName);
            return hashCode;
        }

        public static bool operator ==(RegisteredException left, RegisteredException right)
        {
            return EqualityComparer<RegisteredException>.Default.Equals(left, right);
        }

        public static bool operator !=(RegisteredException left, RegisteredException right)
        {
            return !(left == right);
        }
    }
}
