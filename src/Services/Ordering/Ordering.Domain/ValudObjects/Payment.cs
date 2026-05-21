using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.ValudObjects
{
    public class Payment
    {
        public string CardNumber { get; init; }=default!;
        public string? CardName { get; init; }=default!;
        public string Expiration { get; init; } = default!; 
        public string CVV { get; init; }   = default!;  
        public int PaymentMethod { get; init; } = default!; 

        protected Payment()
        {
        }   

        private Payment(string cardNumber, string? cardName, string expiration, string cvv, int paymentMethod)
        {
            CardNumber = cardNumber;
            CardName = cardName;
            Expiration = expiration;
            CVV = cvv;
            PaymentMethod = paymentMethod;
        }

        public static Payment Of(string cardNumber, string? cardName, string expiration, string cvv, int paymentMethod)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber);
            ArgumentException.ThrowIfNullOrWhiteSpace(expiration);
            ArgumentException.ThrowIfNullOrWhiteSpace(cvv);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3);  
            return new Payment(cardNumber, cardName, expiration, cvv, paymentMethod);
        }
    }
}
