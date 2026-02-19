using AutoMapper;
using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Product;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.DataAccess.Abstract.Product;
using ByteMarket.Entities.Concrete;

namespace ByteMarket.Business.Concrete
{
	public class ProductManager : IProductService
	{
		private readonly IProductReadRepository _productReadRepository;
		private readonly IProductWriteRepository _productWriteRepository;
		private readonly IMapper _mapper;

		public ProductManager(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IMapper mapper)
		{
			_productReadRepository = productReadRepository;
			_productWriteRepository = productWriteRepository;
			_mapper = mapper;
		}

		public async Task<IDataResult<List<ListProductDto>>> GetAllProductsAsync()
		{
			var products = _productReadRepository.GetAll(false).ToList();

			if (products == null || !products.Any())
			{
				return new ErrorDataResult<List<ListProductDto>>("Listelenecek ürün bulunamadı.");
			}

			var productDtos = _mapper.Map<List<ListProductDto>>(products);

			return new SuccessDataResult<List<ListProductDto>>(productDtos, "Ürünler başarıyla listelendi.");
		}

		public async Task<IDataResult<SingleProductDto>> GetProductByIdAsync(string id)
		{
			Product product = await _productReadRepository.GetByIdAsync(id, false);

			if (product == null)
			{
				return new ErrorDataResult<SingleProductDto>("Ürün bulunamadı.");
			}

			var productDto = _mapper.Map<SingleProductDto>(product);
			return new SuccessDataResult<SingleProductDto>(productDto, "Ürün başarıyla getirildi.");
		}

		public async Task<IResult> CreateProductAsync(CreateProductDto createProductDto)
		{
			var product = _mapper.Map<Product>(createProductDto);

			bool result = await _productWriteRepository.AddAsync(product);

			if (result)
			{
				await _productWriteRepository.SaveAsync();
				return new SuccessResult("Ürün başarıyla eklendi.");
			}

			return new ErrorResult("Ürün eklenirken bir hata oluştu.");
		}
	}
}
