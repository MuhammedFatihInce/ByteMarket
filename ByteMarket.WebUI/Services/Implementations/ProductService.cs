using ByteMarket.WebUI.Models.ProductViewModels;
using ByteMarket.WebUI.Services.Interfaces;

namespace ByteMarket.WebUI.Services.Implementations
{
	public class ProductService : IProductService
	{
		private readonly IApiService _apiService;
		public ProductService(IApiService apiService) => _apiService = apiService;

		public async Task<bool> AddProductAsync(CreateProductViewModel model)
		{
			var productResponse = await _apiService.PostAsync<string>("Products/Add", new {model.Name, model.Price, model.Stock});

			if (!productResponse.Success) return false;

			string newProductId = productResponse.Data;

			if (model.Files != null && model.Files.Any())
			{
				var content = new MultipartFormDataContent();
				foreach (var file in model.Files)
				{
					var streamContent = new StreamContent(file.OpenReadStream());
					content.Add(streamContent, "files", file.FileName);
				}

				var imageResponse = await _apiService.PostMultipartAsync<object>($"Products/UploadImage/{newProductId}", content);

				return imageResponse.Success;
			}

			return true;
		}

		public async Task<List<ProductListViewModel>> GetProductsForAdminAsync()
		{
			var response = await _apiService.GetAllAsync<ProductListViewModel>("Products/GetAll");

			return response.Success ? response.Data : new List<ProductListViewModel>();
		}

		public async Task<bool> UploadProductImagesAsync(string productId, IFormFileCollection files)
		{
			var content = new MultipartFormDataContent();

			foreach (var file in files)
			{
				var streamContent = new StreamContent(file.OpenReadStream());
				content.Add(streamContent, "files", file.FileName);
			}

			var response = await _apiService.PostMultipartAsync<bool>($"Products/UploadImage/{productId}", content);
			return response.Success;
		}
	}
}
