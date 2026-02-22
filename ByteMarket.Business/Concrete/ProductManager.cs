using AutoMapper;
using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Product;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.DataAccess.Abstract.Product;
using ByteMarket.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using IResult = ByteMarket.Business.Utilities.Results.IResult;

namespace ByteMarket.Business.Concrete
{
	public class ProductManager : IProductService
	{
		private readonly IProductReadRepository _productReadRepository;
		private readonly IProductWriteRepository _productWriteRepository;
		private readonly IMapper _mapper;
		private readonly IProductImageService _productImageService;

		public ProductManager(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IMapper mapper, IProductImageService productImageService)
		{
			_productReadRepository = productReadRepository;
			_productWriteRepository = productWriteRepository;
			_mapper = mapper;
			_productImageService = productImageService;
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

		public async Task<IResult> UpdateProductAsync(UpdateProductDto updateProductDto)
		{
			var product = await _productReadRepository.GetByIdAsync(updateProductDto.Id);

			if (product == null) return new ErrorResult("Ürün bulunamadı.");

			_mapper.Map(updateProductDto, product);

			_productWriteRepository.Update(product);
			await _productWriteRepository.SaveAsync();

			return new SuccessResult("Ürün bilgileri güncellendi.");
		}

		public async Task<IResult> DeleteProductAsync(string id)
		{
			await _productImageService.DeleteImagesByProductIdAsync(id);

			await _productWriteRepository.RemoveAsync(id);
			await _productWriteRepository.SaveAsync();

			return new SuccessResult("Ürün ve bağlı tüm görseller başarıyla temizlendi.");
		}
	}
}
