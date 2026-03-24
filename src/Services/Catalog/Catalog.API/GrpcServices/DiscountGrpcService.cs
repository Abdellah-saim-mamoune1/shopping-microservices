using Discount.GRPC.Protos;

namespace Catalog.API.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
        {
            _discountProtoService = discountProtoService ?? throw new ArgumentNullException(nameof(discountProtoService));
        }

        public async Task<CouponModel> GetDiscount(string BookId)
        {
            var discountRequest = new GetDiscountRequest { BookId = BookId };
            return await _discountProtoService.GetDiscountAsync(discountRequest);
        }
    }
}
