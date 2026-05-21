using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Dtos
{
  public record OrderItemDto
   (
       Guid OrderId,
       Guid ProductId,
       decimal UnitPrice,
       int Quantity
   );   
}
