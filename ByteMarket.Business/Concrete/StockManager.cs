

using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Stock;
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

		public async Task<IDataResult<List<StockUpdateDto>>> CheckAndDecreaseStockAsync(string basketId)
		{
			var basket = await _basketReadRepository.Table
				.Include(b => b.BasketItems)
				.ThenInclude(bi => bi.Product)
				.FirstOrDefaultAsync(b => b.Id == Guid.Parse(basketId));

			if (basket == null || !basket.BasketItems.Any())
			{
				return new ErrorDataResult<List<StockUpdateDto>>();
			}

			var updatedStocks = new List<StockUpdateDto>();

			foreach (var basketItem in basket.BasketItems)
			{
				var product = basketItem.Product;

				if (product.Stock < basketItem.Quantity)
				{
					return new ErrorDataResult<List<StockUpdateDto>>($"'{product.Name}' isimli ürün için yeterli stok bulunmuyor. Kalan Stok: {product.Stock}");
				}

				product.Stock -= basketItem.Quantity;

				updatedStocks.Add(new StockUpdateDto
				{
					ProductId = product.Id.ToString(),
					NewStock = product.Stock
				});

			}
			return new SuccessDataResult<List<StockUpdateDto>>(updatedStocks);
		}

		public async Task<IResult> CheckStockAsync(string basketId)
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

			}

			return new SuccessResult();
		}
	}
}
