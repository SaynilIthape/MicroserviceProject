using Ordering.Domain.Abstarctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Models
{
    public  class OrderItem:Entity<Guid>    
    {
        internal OrderItem(Guid orderId, Guid productId, decimal price, int quantity)
        {
            OrderId = orderId;
            ProductId = productId;
            Price = price;
            Quantity = quantity;
        }   

        public Guid OrderId { get; private set; }=default;
        public Guid ProductId { get; private set; }=default;    
        public decimal Price { get; private set; }=default; 
        public int Quantity { get; private set; }  =default;    
    }
}
