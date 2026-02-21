using AutoMapper;
using ByteMarket.Business.Abstract;
using ByteMarket.Business.Abstract.Storage;
using ByteMarket.Business.DTOs.Product;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.DataAccess.Abstract.Product;
using ByteMarket.DataAccess.Abstract.ProductImageFile;
using ByteMarket.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using IResult = ByteMarket.Business.Utilities.Results.IResult;

namespace ByteMarket.Business.Concrete
{
	public class ProductManager : IProductService
	{
		private readonly IProductReadRepository _productReadRepository;
		private readonly IProductWriteRepository _productWriteRepository;
		private readonly IMapper _mapper;
		private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
		private readonly IProductImageFileReadRepository _productImageFileReadRepository;
		private readonly IStorageService _storageService;

		public ProductManager(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IMapper mapper, IProductImageFileWriteRepository productImageFileWriteRepository, IStorageService storageService, IProductImageFileReadRepository productImageFileReadRepository)
		{
			_productReadRepository = productReadRepository;
			_productWriteRepository = productWriteRepository;
			_mapper = mapper;
			_productImageFileWriteRepository = productImageFileWriteRepository;
			_productImageFileReadRepository = productImageFileReadRepository;
			_storageService = storageService;
		}

		public async Task<IDataResult<List<ListProductDto>>> GetAllProductsAsync()
		{
			var products = _productReadRepository.GetAll(false)
				.Include(p => p.ProductImageFiles)
				.ToList();

			if (products == null || !products.Any())
			{
				return new ErrorDataResult<List<ListProductDto>>("Listelenecek ürün bulunamadı.");
			}

			var productDtos = _mapper.Map<List<ListProductDto>>(products);

			return new SuccessDataResult<List<ListProductDto>>(productDtos, "Ürünler başarıyla listelendi.");
		}

		public async Task<IDataResult<SingleProductDto>> GetProductByIdAsync(string id)
		{
			var product = await _productReadRepository.GetAll(false)
				.Include(p =>p.ProductImageFiles)
				.FirstOrDefaultAsync(p=> p.Id == Guid.Parse(id));

			if (product == null)
			{
				return new ErrorDataResult<SingleProductDto>("Ürün bulunamadı.");
			}

			var productDto = _mapper.Map<SingleProductDto>(product);
			return new SuccessDataResult<SingleProductDto>(productDto, "Ürün başarıyla getirildi.");
		}

		public async Task<IDataResult<string>> CreateProductAsync(CreateProductDto createProductDto)
		{
			var product = _mapper.Map<Product>(createProductDto);

			bool result = await _productWriteRepository.AddAsync(product);

			if (result)
			{
				await _productWriteRepository.SaveAsync();
				return new SuccessDataResult<string>(product.Id.ToString(),"Ürün başarıyla eklendi.");
			}

			return new ErrorDataResult<string>("Ürün eklenirken bir hata oluştu.");
		}

		public async Task<IResult> AddProductImagesAsync(string productId, IFormFileCollection files)
		{
			List<(string fileName, string pathOrContainerName)> fileResult = await _storageService.UploadAsync("photo-images", files);

			var product = await _productReadRepository.GetByIdAsync(productId);

			var result = await _productImageFileWriteRepository.AddRangeAsync(fileResult.Select(r => new ProductImageFile
			{
				FileName = r.fileName,
				Path = r.pathOrContainerName,
				Storage = _storageService.StorageName,
				Products = new List<Product>() { product }
			}).ToList());


			if (result)
			{
				await _productImageFileWriteRepository.SaveAsync();
				return new SuccessResult("Ürün görselleri başarıyla yüklendi.");
			}

			return new ErrorResult("Ürün görselleri yüklenirken bir hata oluştu.");
		}

		public async Task<IResult> UpdateProductAsync(UpdateProductDto updateProductDto)
		{
			var product = await _productReadRepository.GetByIdAsync(updateProductDto.Id);

			if (product == null) return new ErrorResult("Ürün bulunamadı.");

			product.Name = updateProductDto.Name;
			product.Price = updateProductDto.Price;
			product.Stock = updateProductDto.Stock;

			_productWriteRepository.Update(product);
			await _productWriteRepository.SaveAsync();

			return new SuccessResult("Ürün bilgileri güncellendi.");
		}

		public async Task<IResult> DeleteProductImageAsync(string imageId)
		{
			var productImage = await _productImageFileReadRepository.GetByIdAsync(imageId);
			if (productImage == null) return new ErrorResult("Resim kaydı bulunamadı.");

			await _storageService.DeleteAsync(productImage.Path, productImage.FileName);

			await _productImageFileWriteRepository.RemoveAsync(imageId);
			await _productImageFileWriteRepository.SaveAsync();

			return new SuccessResult("Resim hem diskten hem veritabanından başarıyla silindi.");
		}

		public async Task<IResult> DeleteProductAsync(string id)
		{
			var product = await _productReadRepository.GetAll()
				.Include(p => p.ProductImageFiles)
				.FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

			if (product == null) return new ErrorResult("Ürün bulunamadı.");

			if (product.ProductImageFiles != null && product.ProductImageFiles.Any())
			{
				foreach (var image in product.ProductImageFiles)
				{
					await DeleteProductImageAsync(image.Id.ToString());
				}
			}

			await _productWriteRepository.RemoveAsync(id);
			await _productWriteRepository.SaveAsync();

			return new SuccessResult("Ürün ve bağlı tüm görseller başarıyla temizlendi.");
		}
	}
}
