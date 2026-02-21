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

			if (model.Files != null && model.Files.Any())
				return await UploadImagesInternalAsync(productResponse.Data, model.Files);

			return true;
		}

		public async Task<List<ProductListViewModel>> GetProductsForAdminAsync()
		{
			var response = await _apiService.GetAllAsync<ProductListViewModel>("Products/GetAll");

			return response.Success ? response.Data : new List<ProductListViewModel>();
		}

		public async Task<bool> UpdateProductWithImagesAsync(UpdateProductViewModel model)
		{
			var updateResponse = await _apiService.PutAsync<object>("Products/Update", new
			{
				model.Id,
				model.Name,
				model.Price,
				model.Stock
			});

			if (!updateResponse.Success) return false;

			
			if (model.Files != null && model.Files.Any())
				return await UploadImagesInternalAsync(model.Id, model.Files);

			return true;
		}

		private async Task<bool> UploadImagesInternalAsync(string productId, IEnumerable<IFormFile> files)
		{
			var content = new MultipartFormDataContent();
			foreach (var file in files)
			{
				var streamContent = new StreamContent(file.OpenReadStream());
				streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
				content.Add(streamContent, "files", file.FileName);
			}

			var response = await _apiService.PostMultipartAsync<object>($"Products/UploadImage/{productId}", content);
			return response.Success;
		}

	}
}
