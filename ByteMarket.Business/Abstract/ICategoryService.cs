
using ByteMarket.Business.DTOs.Category;
using ByteMarket.Business.Utilities.Results;

namespace ByteMarket.Business.Abstract
{
	public interface ICategoryService
	{
		Task<IResult> AddAsync(CreateCategoryDto categoryDto);
		Task<IDataResult<List<ListCategoryDto>>> GetAllAsync();
		Task<IResult> DeleteAsync(string id);
		Task<IResult> UpdateAsync(UpdateCategoryDto updatecategorytDto);
		Task<IDataResult<SingleCategoryDto>> GetByIdAsync(string id);
	}
}
