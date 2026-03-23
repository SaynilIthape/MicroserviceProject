using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountDBContext dbcontext ,ILogger<DiscountService> logger)
        : DiscountProtoService.DiscountProtoServiceBase    
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbcontext.Coupons.FirstOrDefaultAsync(z=>z.ProductName==request.ProductName);

            if (coupon == null) 
                coupon= new Coupon { ProductName = "Not Discount",Amount=0,Description="" };

            return coupon.Adapt<CouponModel>();
            //return base.GetDiscount(request, context);
        }

         public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbcontext.Coupons.FirstOrDefaultAsync(a => a.ProductName == request.ProductName);
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.NotFound,"Product name not exists"), "We did not find product");
           
                 dbcontext.Coupons.Remove(coupon);
                await dbcontext.SaveChangesAsync();
            return new DeleteDiscountResponse { Success = true };
            
        }

        public override async  Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var requestBody = request.Coupon.Adapt<Coupon>();
            if (requestBody == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Coupon Data"));

            dbcontext.Coupons.Update(requestBody);
            await dbcontext.SaveChangesAsync();

            var response = requestBody.Adapt<CouponModel>();
            return response;
        }
         public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var requestBody= request.Coupon.Adapt<Coupon>();
            if (requestBody==null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Coupon Data"));

            dbcontext.Coupons.Add(requestBody);
            await dbcontext.SaveChangesAsync();    

            var response = requestBody.Adapt<CouponModel>();
            return response;
        }   


    }
}
