using Discount.Grpc.Protos;
using System.Threading.Tasks;
using static Discount.Grpc.Protos.DiscountProtoService;

namespace Basket.Api.GrpcServices
{
    public class DsicountGrpcService
    {
        public DiscountProtoServiceClient DiscountProtoServiceClient { get; }

        public DsicountGrpcService(DiscountProtoServiceClient discountProtoServiceClient)
        {
            DiscountProtoServiceClient = discountProtoServiceClient;
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var request = new GetDiscountRequest
            {
                ProductName = productName
            };


            return  await DiscountProtoServiceClient.GetDiscountAsync(request);
        }

    }
}
