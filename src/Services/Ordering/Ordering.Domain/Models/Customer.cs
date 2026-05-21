using Ordering.Domain.Abstarctions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Models
{
    public class Customer : Entity<Guid>
    {
        public string Name { get; private set; }    
        public string Email { get; private set; }    
         
        public static Customer Create(Guid id, string Name, String Email)
        {

            ArgumentException.ThrowIfNullOrWhiteSpace(Name);
            ArgumentException.ThrowIfNullOrWhiteSpace(Email);

            var customer = new Customer
            {
                Id = id,
                Name = Name,
                Email = Email
            };

            return customer;
        }
    }
}
