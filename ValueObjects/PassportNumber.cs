using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace StudentsVisitationsWPF.ValueObjects
{
    public class PassportNumber
    {
        public string Value { get; private set; }

        protected PassportNumber() { }

        public PassportNumber(string number)
        {
            if (number == null) { throw new ArgumentNullException(number); }
            if (!IsCorrectPassportNumber(number)) { throw new ArgumentException("Invalid Passport Number!", nameof(number)); }

            Value = number;
        }

        private static bool IsCorrectPassportNumber(string number)
        {
            return(Regex.IsMatch(number, @"^\d+$") && number.Trim().Length == 9);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool Equals(object? obj)
        {
            return obj is PassportNumber number &&
                   Value == number.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }

        public static bool operator ==(PassportNumber? left, PassportNumber? right)
        {
            return EqualityComparer<PassportNumber>.Default.Equals(left, right);
        }

        public static bool operator !=(PassportNumber? left, PassportNumber? right)
        {
            return !(left == right);
        }
    }
}
