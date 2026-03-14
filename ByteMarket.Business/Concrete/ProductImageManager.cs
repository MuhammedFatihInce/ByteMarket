using ByteMarket.Business.Abstract;
using ByteMarket.Business.Abstract.Storage;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.DataAccess.Abstract.Product;
using ByteMarket.DataAccess.Abstract.ProductImageFile;
using ByteMarket.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using IResult = ByteMarket.Business.Utilities.Results.IResult;

namespace ByteMarket.Business.Concrete
{
	public class ProductImageManager : IProductImageService
	{
		private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
		private readonly IProductImageFileReadRepository _productImageFileReadRepository;
		private readonly IProductReadRepository _productReadRepository;
		private readonly IStorageService _storageService;

		public ProductImageManager(IProductImageFileWriteRepository productImageFileWriteRepository, IProductImageFileReadRepository productImageFileReadRepository, IProductReadRepository productReadRepository, IStorageService storageService)
		{
			_productImageFileReadRepository = productImageFileReadRepository;
			_productImageFileWriteRepository = productImageFileWriteRepository;
			_productReadRepository = productReadRepository;
			_storageService = storageService;
		}

		public async Task<IResult> AddImagesAsync(string productId, IFormFileCollection files)
		{
			var product = await _productReadRepository.GetByIdAsync(productId);


			var existingImageCount = _productImageFileReadRepository
				.GetWhere(x => x.Products.Any(p => p.Id == Guid.Parse(productId)))
				.Count();

			List<(string fileName, string pathOrContainerName)> fileResult = await _storageService.UploadAsync("photo-images", files);
			   

			var productImages = new List<ProductImageFile>();

			for (int i = 0; i < fileResult.Count; i++)
			{
				var r = fileResult[i];
				productImages.Add(new ProductImageFile
				{
					FileName = r.fileName,
					Path = r.pathOrContainerName,
					Storage = _storageService.StorageName,
					Products = new List<Product> { product },
					DisplayOrder = existingImageCount + i + 1 
				});
			}
			   
			var result = await _productImageFileWriteRepository.AddRangeAsync(productImages);
			   
			if (result)
			{
			   	await _productImageFileWriteRepository.SaveAsync();
			   	return new SuccessResult("Ürün görselleri başarıyla yüklendi.");
			}
			   
			return new ErrorResult("Ürün görselleri yüklenirken bir hata oluştu.");
		}

		public async Task<IResult> DeleteImagesByProductIdAsync(string productId)
		{
			var product = await _productReadRepository.GetAll(false)
				.Include(p => p.ProductImageFiles)
				.FirstOrDefaultAsync(p => p.Id == Guid.Parse(productId));

			if (product == null)
				return new ErrorResult("Ürün bulunamadı.");

			if (product.ProductImageFiles != null && product.ProductImageFiles.Any())
			{
				foreach (var image in product.ProductImageFiles)
				{
					await _storageService.DeleteAsync(image.Path, image.FileName);
				}
				_productImageFileWriteRepository.RemoveRange(product.ProductImageFiles.ToList());
			}

			await _productImageFileWriteRepository.SaveAsync();
			return new SuccessResult();
		}

		public async Task<IResult> DeleteImageAsync(string imageId)
		{
			var productImage = await _productImageFileReadRepository.GetByIdAsync(imageId);
			if (productImage == null) return new ErrorResult("Resim kaydı bulunamadı.");

			await _storageService.DeleteAsync(productImage.Path, productImage.FileName);

			await _productImageFileWriteRepository.RemoveAsync(imageId);
			await _productImageFileWriteRepository.SaveAsync();

			return new SuccessResult("Resim hem diskten hem veritabanından başarıyla silindi.");
		}
	}
}
