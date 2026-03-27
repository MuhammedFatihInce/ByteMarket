

using ByteMarket.Business.Abstract;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.DataAccess.Abstract.Basket;
using ByteMarket.DataAccess.Abstract.Product;
using Microsoft.EntityFrameworkCore;

namespace ByteMarket.Business.Concrete
{
	public class StockManager:IStockService
	{
		private readonly IBasketReadRepository _basketReadRepository;
		private readonly IProductWriteRepository _productWriteRepository;

		public StockManager(IBasketReadRepository basketReadRepository, IProductWriteRepository productWriteRepository)
		{
			_basketReadRepository = basketReadRepository;
			_productWriteRepository = productWriteRepository;
		}

		public async Task<IResult> CheckAndDecreaseStockAsync(string basketId)
		{
			return await ProcessStockAsync(basketId, true);
		}

		public async Task<IResult> CheckStockAsync(string basketId)
		{
			return await ProcessStockAsync(basketId, false);
		}


		private async Task<IResult> ProcessStockAsync(string basketId, bool decrease)
		{
			var basket = await _basketReadRepository.Table
				.Include(b => b.BasketItems)
				.ThenInclude(bi => bi.Product)
				.FirstOrDefaultAsync(b => b.Id == Guid.Parse(basketId));

			if (basket == null || !basket.BasketItems.Any())
			{
				return new ErrorResult();
			}

			foreach (var basketItem in basket.BasketItems)
			{
				var product = basketItem.Product;

				if (product.Stock < basketItem.Quantity)
				{
					return new ErrorResult($"'{product.Name}' isimli ürün için yeterli stok bulunmuyor. Kalan Stok: {product.Stock}");
				}

				if (decrease)
				{
					product.Stock -= basketItem.Quantity;
				}

			}
			return new SuccessResult();
		}
	}
}
