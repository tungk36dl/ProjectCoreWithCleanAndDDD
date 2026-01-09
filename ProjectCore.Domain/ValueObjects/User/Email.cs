using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Domain.ValueObjects.User
{

    public sealed record Email
    {
        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email is required.");

            if (!IsValid(value))
                throw new ArgumentException("Invalid email format.");

            Value = value.Trim().ToLowerInvariant();
        }

        public static bool IsValid(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public override string ToString() => Value;


    }

}
