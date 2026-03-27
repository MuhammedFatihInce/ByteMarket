using ByteMarket.WebUI.Areas.Admin.Models.Product;
using ByteMarket.WebUI.Models.Product;
using ByteMarket.WebUI.Models.ResultModels;
using ByteMarket.WebUI.Services.Interfaces;

namespace ByteMarket.WebUI.Services.Implementations
{
	public class ProductService : IProductService
	{
		private readonly IApiService _apiService;
		public ProductService(IApiService apiService) => _apiService = apiService;

		public async Task<ApiDataResponse<string>> AddProductAsync(CreateProductViewModel model)
		{
			var productResponse = await _apiService.PostAsync<string>("Product", new {model.Name, model.Price, model.Stock, model.CategoryIds, model.Description});

			if (!productResponse.Success) return productResponse;

			string newProductId = productResponse.Data;

			if (model.Files != null && model.Files.Any())
			{
				var imageResponse = await UploadImagesInternalAsync(newProductId, model.Files);

				if (!imageResponse.Success)
				{
					imageResponse.Message = $"Ürün eklendi fakat resimler yüklenemedi: {imageResponse.Message}";
					return new ApiDataResponse<string> { Success = false, Message = imageResponse.Message, Data = newProductId };
				}
			}

			return productResponse;
		}

		public async Task<ApiDataResponse<List<ProductListAdminViewModel>>> GetProductsForAdminAsync(string? categoryId = null)
		{
			var endpoint = string.IsNullOrEmpty(categoryId)
				? "Product"
				: $"Product?categoryId={categoryId}";

			return await _apiService.GetAllAsync<ProductListAdminViewModel>(endpoint);
		}

		public async Task<ApiDataResponse<List<ProductListViewModel>>> GetProductsForCustomerAsync(string? categoryId = null)
		{
			var endpoint = string.IsNullOrEmpty(categoryId)
				? "Product"
				: $"Product?categoryId={categoryId}";

			return await _apiService.GetAllAsync<ProductListViewModel>(endpoint);
		}

		public async Task<ApiDataResponse<object>> UpdateProductWithImagesAsync(UpdateProductViewModel model)
		{
			var updateResponse = await _apiService.PutAsync<object>("Product", new
			{
				model.Id,
				model.Name,
				model.Price,
				model.Description,
				model.Stock,
				model.CategoryIds,
				model.OrderedImageIds,
				model.RowVersion
			});

			if (!updateResponse.Success) return updateResponse;
			
			if (model.Files != null && model.Files.Any())
			{
				var imageResponse = await UploadImagesInternalAsync(model.Id, model.Files);
				if (!imageResponse.Success) return imageResponse;
			}

			return updateResponse;
		}

		private async Task<ApiDataResponse<object>> UploadImagesInternalAsync(string productId, IEnumerable<IFormFile> files)
		{
			var content = new MultipartFormDataContent();
			foreach (var file in files)
			{
				var streamContent = new StreamContent(file.OpenReadStream());
				streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
				content.Add(streamContent, "files", file.FileName);
			}

			return await _apiService.PostMultipartAsync<object>($"ProductImages/Upload/{productId}", content);
		}

		public async Task<ApiDataResponse<SingleProductViewModel>> GetProductDetailsAsync(string id)
		{
			return await _apiService.GetByIdAsync<SingleProductViewModel>("Product", id);
		}

		public async Task<ApiDataResponse<object>> DeleteProductAsync(string id)
		{
			return await _apiService.DeleteAsync<object>("Product", id);
		}

		public async Task<ApiDataResponse<object>> DeleteProductImageAsync(string id)
		{
			return await _apiService.DeleteAsync<object>("ProductImages", id);
		}

		public async Task<ApiDataResponse<List<GetAllProductByFilterViewModel>>> GetAllProductByFilterAsync(string q)
		{

			return await _apiService.GetAllAsync<GetAllProductByFilterViewModel>($"Product/GetAllProductsByFilter?q={q}");
		}

	}
}
