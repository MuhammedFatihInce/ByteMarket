using AutoMapper;
using AutoMapper.QueryableExtensions;
using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Product;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.DataAccess.Abstract.Category;
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
		private readonly ICategoryReadRepository _categoryReadRepository;

		public ProductManager(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IMapper mapper, IProductImageService productImageService, ICategoryReadRepository categoryReadRepository)
		{
			_productReadRepository = productReadRepository;
			_productWriteRepository = productWriteRepository;
			_mapper = mapper;
			_productImageService = productImageService;
			_categoryReadRepository = categoryReadRepository;
		}

		public async Task<IDataResult<List<ListProductDto>>> GetAllProductsAsync(string? categoryId = null)
		{
			var query = _productReadRepository.GetAll(false);

			if (!string.IsNullOrEmpty(categoryId))
			{
				query = query.Where(p => p.Categories.Any(c => c.Id.ToString() == categoryId));
			}

			var products = await query
				.ProjectTo<ListProductDto>(_mapper.ConfigurationProvider)
				.ToListAsync();

			if (products == null || !products.Any())
			{
				return new ErrorDataResult<List<ListProductDto>>("Listelenecek ürün bulunamadı.");
			}

			return new SuccessDataResult<List<ListProductDto>>(products, "Ürünler başarıyla listelendi.");
		}

		public async Task<IDataResult<SingleProductDto>> GetProductByIdAsync(string id)
		{
			var product = await _productReadRepository
				.GetWhere(p => p.Id == Guid.Parse(id), false)
				.Include(p =>p.ProductImageFiles)
				.Include(p=> p.Categories)
				.FirstOrDefaultAsync();

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

			if (createProductDto.CategoryIds != null && createProductDto.CategoryIds.Any())
			{
				var categories = _categoryReadRepository.GetWhere(c => createProductDto.CategoryIds.Contains(c.Id.ToString())).ToList();

				product.Categories = categories;
			}

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
			var product = await _productReadRepository
				.GetWhere(p => p.Id == Guid.Parse(updateProductDto.Id))
				.Include(p => p.Categories).FirstOrDefaultAsync();

			if (product == null) return new ErrorResult("Ürün bulunamadı.");

			_mapper.Map(updateProductDto, product);

			if (updateProductDto.CategoryIds != null && updateProductDto.CategoryIds.Any())
			{
				var newCategories = _categoryReadRepository.GetWhere(c => updateProductDto.CategoryIds.Contains(c.Id.ToString())).ToList();

				product.Categories = newCategories;
			}
			else
			{
				product.Categories.Clear();
			}

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
