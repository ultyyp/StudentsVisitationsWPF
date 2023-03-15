using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsVisitationsWPF.ValueObjects
{
    public class Email
    {
        public string Value { get; private set; }

        protected Email() { }

        public Email(string email)
        {
            if (email == null) { throw new ArgumentNullException(email); }
            if (!IsCorrectEmail(email)) { throw new ArgumentException("Invalid Email", nameof(email)); }

            Value = email;
        }

        private static bool IsCorrectEmail(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool Equals(object? obj)
        {
            return obj is Email email &&
                   Value == email.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }

        public static bool operator ==(Email? left, Email? right)
        {
            return EqualityComparer<Email>.Default.Equals(left, right);
        }

        public static bool operator !=(Email? left, Email? right)
        {
            return !(left == right);
        }
    }
}
