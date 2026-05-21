using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.ValudObjects
{
    public record Address
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string EmailAddress { get; init; }
        public string AddressLine { get; init; }
        public string Country { get; init; }
        public string State { get; init; }
        public string ZipCode { get; init; }

        protected Address()
        {
        }

        private Address(string firstName, string lastName, string emailAddress, string addressLine, string country, string state, string zipCode)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            AddressLine = addressLine;
            Country = country;
            State = state;
            ZipCode = zipCode;
        }
        public static Address Of(string firstName, string lastName, string emailAddress, string addressLine, string country, string state, string zipCode)
        {

            ArgumentException.ThrowIfNullOrWhiteSpace(emailAddress);
            ArgumentException.ThrowIfNullOrWhiteSpace(addressLine);

            return new Address(firstName, lastName, emailAddress, addressLine, country, state, zipCode);
        }
    }
}
