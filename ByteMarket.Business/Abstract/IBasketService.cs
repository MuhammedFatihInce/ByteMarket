using ByteMarket.Business.DTOs.Basket;
using ByteMarket.Business.Utilities.Results;
using IResult = ByteMarket.Business.Utilities.Results.IResult;

namespace ByteMarket.Business.Abstract
{
	public interface IBasketService
	{
		Task<IResult> AddItemToBasketAsync(CreateBasketDto creaBasketDto);
		Task<IDataResult<ListBasketDto>> GetBasketItemsAsync();
		Task<IResult> RemoveBasketItemAsync(string basketItemId);
		Task<IResult> UpdateQuantityAsync(UpdateBasketItemQuantityDto dto);
	}
}
