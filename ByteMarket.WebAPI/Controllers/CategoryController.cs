using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Category;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebAPI.Controllers
{
	public class CategoryController : BaseController
	{
		private readonly ICategoryService _categoryService;
		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpPost("Add")]
		public async Task<IActionResult> Add(CreateCategoryDto createCategoryDto)
		{
			var result = await _categoryService.AddAsync(createCategoryDto);
			return CreateActionResult(result, 201);
		}

		[HttpGet("GetAll")]
		public async Task<IActionResult> GetAll()
		{
			var result = await _categoryService.GetAllAsync();
			return CreateActionResult(result);
		}

		[HttpDelete("Delete/{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			var result = await _categoryService.DeleteAsync(id);
			return CreateActionResult(result);
		}

		[HttpPut("Update")]
		public async Task<IActionResult> Update([FromBody] UpdateCategoryDto updateCategoryDtoDto)
		{
			var result = await _categoryService.UpdateAsync(updateCategoryDtoDto);
			return CreateActionResult(result);
		}

		[HttpGet("GetById/{id}")]
		public async Task<IActionResult> GetById(string id)
		{
			var result = await _categoryService.GetByIdAsync(id);

			return CreateActionResult(result);
		}
	}
}
