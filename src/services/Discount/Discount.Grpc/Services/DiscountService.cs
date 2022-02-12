
using AutoMapper;
using Discount.Grpc.Protos;
using DiscountGrpc.Grpc.Entities;
using DiscountGrpc.Grpc.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using static Discount.Grpc.Protos.DiscountProtoService;

namespace DiscountGrpc.Grpc.Services
{
    public class DiscountService: DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository discountRepository;
        private readonly IMapper mapper;
        private readonly ILogger<DiscountService> logger;

        public DiscountService(IDiscountRepository discountRepository,IMapper mapper,ILogger<DiscountService> logger)
        {
            this.discountRepository = discountRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await discountRepository.GetDiscount(request.ProductName);
            if (coupon == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Product with name {request.ProductName} Not Found"));


            var model = mapper.Map<CouponModel>(coupon);
            logger.LogInformation($"Product with name {coupon.ProductName} retireved with amoount {coupon.Amount}");
            return model;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon=mapper.Map<Coupon>(request.Coupon);

            logger.LogInformation($"Created Coupon with product name {coupon.ProductName}");

            await discountRepository.CreateDiscount(coupon);

            return mapper.Map<CouponModel>(coupon);
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = mapper.Map<Coupon>(request.Coupon);

            logger.LogInformation($"updated coupon with product name {coupon.ProductName}");

            await discountRepository.UpdateDiscount(coupon);

            return mapper.Map<CouponModel>(coupon);
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted=await discountRepository.DeleteDiscount(request.ProductName);

            return new DeleteDiscountResponse
            {
                Success = deleted
            };
        }
    }
}
