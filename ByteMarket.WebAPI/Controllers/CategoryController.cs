using ByteMarket.Business.Abstract;
using ByteMarket.Entities.Constants;
using ByteMarket.Business.DTOs.Category;
using ByteMarket.WebAPI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebAPI.Controllers
{
	public class CategoryController : BaseController
	{
		private readonly ICategoryService _categoryService;
		private const string CacheKey = "all_categories_cache";
		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpPost]
		[Authorize(Policy = AuthorizePolicies.FullCategoryManagement)]
		[CacheInvalidate(CacheKey)]
		public async Task<IActionResult> Add(CreateCategoryDto createCategoryDto)
		{
			var result = await _categoryService.AddAsync(createCategoryDto);
			return CreateActionResult(result, successStatusCode:201);
		}

		[HttpGet]
		[Cache(CacheKey)]
		public async Task<IActionResult> GetAll()
		{
			var result = await _categoryService.GetAllAsync();
			return CreateActionResult(result);
		}

		[HttpDelete("{id}")]
		[Authorize(Policy = AuthorizePolicies.FullCategoryManagement)]
		[CacheInvalidate(CacheKey)]
		public async Task<IActionResult> Delete(string id)
		{
			var result = await _categoryService.DeleteAsync(id);
			return CreateActionResult(result, errorStatusCode: 404);
		}

		[HttpPut]
		[Authorize(Policy = AuthorizePolicies.FullCategoryManagement)]
		[CacheInvalidate(CacheKey)]
		public async Task<IActionResult> Update([FromBody] UpdateCategoryDto updateCategoryDtoDto)
		{
			var result = await _categoryService.UpdateAsync(updateCategoryDtoDto);
			return CreateActionResult(result, errorStatusCode: 404);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(string id)
		{
			var result = await _categoryService.GetByIdAsync(id);
			return CreateActionResult(result, errorStatusCode: 404);
		}
	}
}
