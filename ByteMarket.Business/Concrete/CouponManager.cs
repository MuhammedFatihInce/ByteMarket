
using AutoMapper;
using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Coupon;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.DataAccess.Abstract.Coupon;
using ByteMarket.DataAccess.Abstract.Product;
using ByteMarket.Entities.Concrete;
using ByteMarket.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace ByteMarket.Business.Concrete
{
	public class CouponManager:ICouponService
	{
		private readonly ICouponReadRepository _couponReadRepository;
		private readonly ICouponWriteRepository _couponWriteRepository;
		private readonly IProductReadRepository _productReadRepository;
		private readonly IMapper _mapper;
		public CouponManager(ICouponWriteRepository couponWriteRepository, ICouponReadRepository couponReadRepository, IMapper mapper, IProductReadRepository productReadRepository)
		{
			_couponWriteRepository = couponWriteRepository;
			_couponReadRepository = couponReadRepository;
			_mapper = mapper;
			_productReadRepository = productReadRepository;
		}

		public async Task<IResult> AddAsync(CreateCouponDto couponDto)
		{
			var existCoupon = await _couponReadRepository.AnyAsync(c => c.Code == couponDto.Code, false);

			if (existCoupon)
			{
				return new ErrorResult("Bu kupon kodu zaten kullanımda.");
			}

			var coupon = _mapper.Map<Coupon>(couponDto);

			if (couponDto.Target == (int)DiscountTarget.SpecificProducts && couponDto.ProductIds != null && couponDto.ProductIds.Any())
			{
				List<Product> products = await _productReadRepository.GetWhere(p => couponDto.ProductIds.Contains(p.Id.ToString())).ToListAsync();

				coupon.Products = products;
			}

			var result = await _couponWriteRepository.AddAsync(coupon);

			if (result)
			{
				await _couponWriteRepository.SaveAsync();
				return new SuccessResult("Kupon başarılıyla oluşturuldu.");
			}

			return new ErrorResult("Kupon kaydedilirken teknik bir hata oluştu.");
		}

		public async Task<IDataResult<ListCouponDto>> GetAllAsync()
		{
			var coupons = await _couponReadRepository.GetAll(false)
				.Include(c => c.Products).ThenInclude(p=>p.ProductImageFiles)
				.ToListAsync();

			if (coupons == null || !coupons.Any())
			{
				return new ErrorDataResult<ListCouponDto>("Listelenecek Kupon bulunamadı.");
			}

			var couponDtosList = coupons.Select(c => new SingleCouponDto
			{
				Id = c.Id.ToString(),
				Code = c.Code,
				DiscountValue = c.DiscountValue,
				IsPercentage = c.IsPercentage,
				Target = (int)c.Target,
				Products = c.Products.Select(p=> new CouponProductDto()
				{
					Id = p.Id.ToString(),
					Name = p.Name,
					ImagePath = (p.ProductImageFiles != null && p.ProductImageFiles.Any())
						? p.ProductImageFiles.FirstOrDefault().Path
						: null
				}).ToList()
			}).ToList();

			var response = new ListCouponDto
			{
				Coupons = couponDtosList
			};

			return new SuccessDataResult<ListCouponDto>(response, "Kuponlar başarıyla listelendi.");
		}


		public async Task<IDataResult<SingleCouponDto>> GetCouponByIdAsync(string id)
		{
			var coupon = await _couponReadRepository
				.GetWhere(c => c.Id == Guid.Parse(id), false)
				.Include(c=>c.Products)
				.ThenInclude(p=>p.ProductImageFiles)
				.FirstOrDefaultAsync();

			if (coupon == null)
			{
				return new ErrorDataResult<SingleCouponDto>("Kupon bulunamadı.");
			}

			var couponDto = new SingleCouponDto
			{
				Id = coupon.Id.ToString(),
				Code = coupon.Code,
				DiscountValue = coupon.DiscountValue,
				IsPercentage = coupon.IsPercentage,
				Target = (int)coupon.Target,
				Products = coupon.Products.Select(p => new CouponProductDto()
				{
					Id = p.Id.ToString(),
					Name = p.Name,
					ImagePath = (p.ProductImageFiles != null && p.ProductImageFiles.Any())
						? p.ProductImageFiles.FirstOrDefault().Path
						: null
				}).ToList()
			};


			return new SuccessDataResult<SingleCouponDto>(couponDto, "Kupon başarıyla getirildi.");
		}

		public async Task<IResult> UpdateCouponAsync(UpdateCouponDto updateCouponDto)
		{
			var coupon = await _couponReadRepository
				.GetWhere(c => c.Id == Guid.Parse(updateCouponDto.Id))
				.Include(c => c.Products).FirstOrDefaultAsync();

			if (coupon == null) return new ErrorResult("Kupon bulunamadı.");

			coupon.Id = Guid.Parse(updateCouponDto.Id);
			coupon.Code = updateCouponDto.Code;
			coupon.DiscountValue = updateCouponDto.DiscountValue;
			coupon.IsPercentage = updateCouponDto.IsPercentage;
			coupon.Target = (DiscountTarget)updateCouponDto.Target;

			if (updateCouponDto.Target == 2 && updateCouponDto.ProductIds != null && updateCouponDto.ProductIds.Any())
			{
				var newProducts = await _productReadRepository
					.GetWhere(p => updateCouponDto.ProductIds.Contains(p.Id.ToString()))
					.ToListAsync();

				coupon.Products.Clear();
				foreach (var product in newProducts)
				{
					coupon.Products.Add(product);
				}
			}
			else
			{
				coupon.Products.Clear();
			}

			

			_couponWriteRepository.Update(coupon);
			await _couponWriteRepository.SaveAsync();

			return new SuccessResult("Kupon bilgileri güncellendi.");
		}

		public async Task<IResult> DeleteAsync(string id)
		{
			await _couponWriteRepository.RemoveAsync(id);
			await _couponWriteRepository.SaveAsync();

			return new SuccessResult("Kupon başarıyla silindi.");
		}


	}
}
