using Ordering.Domain.Abstarctions;
using Ordering.Domain.Enums;
using Ordering.Domain.Events;
using Ordering.Domain.ValudObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Models
{
    public class Order:Aggregate<Guid>
    {
        private readonly List<OrderItem> _orderItems= new();
        public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();    

        public Guid CustomerId { get; private set; }
        [MaxLength(5)]    
        public string OrderName { get; private set; }
        public Address ShippingAddress { get; private set; } = default;
        public Address BiilingAddress { get; private set; } = default;
        public Payment Payment { get; private set; } = default;
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;  

        public decimal TotalPrice
        {
            get => OrderItems.Sum(x => x.Price * x.Quantity);
            private set { }
        }  

        public static Order Create(Guid orderId, Guid customerId, string OrderName,Address shippingAddress, Address
             billingAddress,Payment payment)
        {
            var order = new Order
            {
                Id = orderId,
                CustomerId = customerId,
                OrderName = OrderName,
                ShippingAddress = shippingAddress,
                BiilingAddress = billingAddress,
                Payment = payment,
                Status= OrderStatus.Pending
            };

            order.AddDomainEvent(new OrderCreateEvent(order));

            return order;

        }

        public void Update(string orderName, Address shippingAddress, Address
             billingAddress, Payment payment, OrderStatus status)

        {
            OrderName = OrderName;
            ShippingAddress = shippingAddress;
            BiilingAddress=billingAddress;  
            Payment = payment;
            Status = status;

            AddDomainEvent(new OrderUpdatedEvent(this));
        }
        public void AddOrderItem(Guid productId, decimal price, int quantity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);   
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

            var orderItem = new OrderItem(Id, productId, price, quantity);
            _orderItems.Add(orderItem);
        }   

        public void RemoveOrderItem(Guid productId)
        {
            var orderItem = _orderItems.FirstOrDefault(x => x.ProductId == productId);
            if (orderItem != null)
            {
                _orderItems.Remove(orderItem);
            }
        }   
    }
}
