using ByteMarket.Business.Abstract;
using ByteMarket.Business.Abstract.Storage;
using ByteMarket.Business.DTOs.Product;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebAPI.Controllers
{
	public class ProductsController : BaseController
	{
		private readonly IProductService _productService;
		private readonly IStorageService _storageService;
		public ProductsController(IProductService productService, IStorageService storageService)
		{
			_productService = productService;
			_storageService = storageService;
		}

		[HttpGet("GetAll")]
		public async Task<IActionResult> GetAll()
		{
			var result = await _productService.GetAllProductsAsync();

			return CreateActionResult(result);
		}

		[HttpGet("GetById/{id}")]
		public async Task<IActionResult> GetById(string id)
		{
			var result = await _productService.GetProductByIdAsync(id);

			return CreateActionResult(result);
		}

		[HttpPost("Add")]
		public async Task<IActionResult> Add(CreateProductDto createProductDto)
		{
			var result = await _productService.CreateProductAsync(createProductDto);

			return CreateActionResult(result, 201);
		}

		[HttpPost("UploadImage/{id}")]
		public async Task<IActionResult> UploadImage(string id, IFormFileCollection files)
		{
			var result = await _productService.AddProductImagesAsync(id, files);

			return CreateActionResult(result);
		}
	}
}
