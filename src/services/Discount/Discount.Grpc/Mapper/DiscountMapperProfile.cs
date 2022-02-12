using AutoMapper;
using Discount.Grpc.Protos;
using DiscountGrpc.Grpc.Entities;

namespace Discount.Grpc.Mapper
{
    public class DiscountMapperProfile:Profile
    {
        public DiscountMapperProfile()
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();
        }
    }
}
