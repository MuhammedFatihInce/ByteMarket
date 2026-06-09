
using ByteMarket.Business.DTOs.ProductReview;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.DataAccess.Abstract.Basket;
using ByteMarket.DataAccess.Abstract.ProductReview;
using ByteMarket.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ByteMarket.Business.Abstract;
using Microsoft.EntityFrameworkCore;
using IResult = ByteMarket.Business.Utilities.Results.IResult;

namespace ByteMarket.Business.Concrete
{
	public class ProductReviewManager:IProductReviewService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IBasketReadRepository _basketReadRepository;
		private readonly IProductReviewWriteRepository _productReviewWriteRepository;
		private readonly IProductReviewReadRepository _productReviewReadRepository;
		public ProductReviewManager(IHttpContextAccessor httpContextAccessor, IBasketReadRepository basketReadRepository, IProductReviewWriteRepository productReviewWriteRepository, IProductReviewReadRepository productReviewReadRepository)
		{
			_httpContextAccessor = httpContextAccessor;
			_basketReadRepository = basketReadRepository;
			_productReviewWriteRepository = productReviewWriteRepository;
			_productReviewReadRepository = productReviewReadRepository;
		}

		public async Task<IResult> CreateProductReviewAsync(CreateProductReviewDto createProductReviewDto)
		{
			var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

			if (string.IsNullOrEmpty(currentUserId))
				return new ErrorResult("Kullanıcı girişi yapılmamış.");

			var userGuid = Guid.Parse(currentUserId);
			var productGuid = Guid.Parse(createProductReviewDto.ProductId);

			var hasPurchased = await _basketReadRepository
				.AnyAsync(b => b.UserId == userGuid &&
				               b.Order != null &&
				               b.BasketItems.Any(bi => bi.ProductId == productGuid));

			if (!hasPurchased)
				return new ErrorResult("Sadece satın aldığınız ürünlere yorum yapabilirsiniz.");

			var productReview = new ProductReview
			{
				ProductId = productGuid,
				UserId = userGuid,
				Comment = createProductReviewDto.Comment,
				Rating = createProductReviewDto.Rating
			};

			var result = await _productReviewWriteRepository.AddAsync(productReview);

			if (result)
			{
				await _productReviewWriteRepository.SaveAsync();
				return new SuccessResult("Yorum başarıyla eklendi.");
			}

			return new ErrorResult("Yorum eklenirken bir sorun oluştu.");
		}

		public async Task<IDataResult<List<ProductReviewListDto>>> GetReviewsByProductIdAsync(string productId)
		{
			var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
			var productGuid = Guid.Parse(productId);

			var reviews = await _productReviewReadRepository
				.GetWhere(r=>r.ProductId == productGuid, false)
				.Include(r=>r.User)
				.OrderByDescending(r=>r.CreateDate)
				.Select(r=> new ProductReviewListDto
				{
					Id = r.Id.ToString(),
					UserId = currentUserId,
					UserName = r.User.UserName,
					Comment = r.Comment,
					CreateDate = r.CreateDate,
					Rating =r.Rating,
				}).ToListAsync();

			return new SuccessDataResult<List<ProductReviewListDto>>(reviews);
		}

		public async Task<IResult> UpdateProductReviewAsync(UpdateProductReviewDto updateDto)
		{
			var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

			var review = await _productReviewReadRepository.GetByIdAsync(updateDto.Id);

			if (review == null)
				return new ErrorResult("Yorum bulunamadı.");

			if(review.UserId != Guid.Parse(currentUserId))
				return new ErrorResult("Sadece kendi yorumunuzu güncelleyebilirsiniz.");

			review.Comment = updateDto.Comment;
			review.Rating = updateDto.Rating;

			var result = _productReviewWriteRepository.Update(review);

			if (result)
			{
				await _productReviewWriteRepository.SaveAsync();
				return new SuccessResult("Yorum güncellendi.");
			}

			return new ErrorResult("Yorum güncellenemedi.");
		}

		public async Task<IResult> DeleteProductReviewAsync(string reviewId)
		{
			var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

			var review = await _productReviewReadRepository.GetByIdAsync(reviewId);

			if (review == null)
				return new ErrorResult("Yorum bulunamadı.");

			bool isAdmin = _httpContextAccessor.HttpContext.User.IsInRole("Admin");

			if (review.UserId != Guid.Parse(currentUserId) && !isAdmin)
			{
				return new ErrorResult("Bu işlem için yetkiniz yok.");
			}

			var result = await _productReviewWriteRepository.RemoveAsync(reviewId);

			if (result)
			{
				await _productReviewWriteRepository.SaveAsync();
				return new SuccessResult("Yorum başarıyla silindi.");
			}

			return new ErrorResult("Yorum silinemedi.");
		}

		public async Task<IDataResult<bool>> HasUserReviewedProductAsync(string userId, string productId)
		{
			var hasReviewed = await _productReviewReadRepository
				.AnyAsync(r => r.UserId == Guid.Parse(userId) && r.ProductId == Guid.Parse(productId));
			return new SuccessDataResult<bool>(hasReviewed);
		}

	}
}
