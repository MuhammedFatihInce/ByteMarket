
using System.Security.Claims;
using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Order;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.DataAccess.Abstract.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using IResult = ByteMarket.Business.Utilities.Results.IResult;

namespace ByteMarket.Business.Concrete
{
	public class OrderManager:IOrderService
	{
		readonly IHttpContextAccessor _httpContextAccessor;
		readonly IOrderWriteRepository _orderWriteRepository;
		readonly IOrderReadRepository _orderReadRepository;
		readonly IMailService _mailService;

		public OrderManager(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository, IMailService mailService, IHttpContextAccessor httpContextAccessor)
		{
			_orderWriteRepository = orderWriteRepository;
			_orderReadRepository = orderReadRepository;
			_mailService = mailService;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<IResult> CreateOrderAsync(CreateOrderDto createOrderDto)
		{
			var orderCode = new Random().Next(100000, 999999).ToString() + DateTime.Now.Ticks.ToString().Substring(10);

			var result = await _orderWriteRepository.AddAsync(new()
			{
				Address = createOrderDto.Address,
				Id = Guid.Parse(createOrderDto.BasketId),
				Description = createOrderDto.Description,
				OrderCode = orderCode,
				DiscountAmount = createOrderDto.DiscountAmount,
				TotalBasePrice = createOrderDto.TotalBasePrice,
				FinalTotalPrice = createOrderDto.FinalTotalPrice
			});

			if (result)
			{
				await _orderWriteRepository.SaveAsync();
				return new SuccessResult("Sipariş oluşturuldu.");
			}

			return new ErrorResult("Sipariş oluşturulamadı.");
		}

		public async Task<IDataResult<List<OrderListDetailDto>>> GetAllOrdersAsync()
		{
			var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (string.IsNullOrEmpty(currentUserId))
			{
				return new ErrorDataResult<List<OrderListDetailDto>>("Kullanıcı oturumu bulunamadı.");
			}

			//var query = _orderReadRepository.Table
			//	.Include(o => o.Basket)
			//	.ThenInclude(b => b.User)
			//	.Include(o => o.Basket)
			//	.ThenInclude(b => b.BasketItems)
			//	.Include(o => o.Basket);

			//if (query == null) new ErrorDataResult<List<OrderListDetailDto>>("Siparişler bulunamadı.");

			//var orders = await query.Select(o => new OrderListDetailDto()
			//{
			//	Id = o.Id.ToString(),
			//	OrderCode = o.OrderCode,
			//	CreatedDate = o.CreateDate,
			//	UserName = o.Basket.User.UserName,
			//	TotalPrice = o.FinalTotalPrice
			//}).ToListAsync();

			var orders = await _orderReadRepository.Table
				.Where(o => o.Basket.UserId == Guid.Parse(currentUserId))
				.Select(o => new OrderListDetailDto()
				{
					Id = o.Id.ToString(),
					OrderCode = o.OrderCode,
					CreatedDate = o.CreateDate,
					UserName = o.Basket.User.UserName,
					TotalPrice = o.FinalTotalPrice
				}).ToListAsync();

			return new SuccessDataResult<List<OrderListDetailDto>>(orders);

		}

		public async Task<IDataResult<SingleOrderDto>> GetOrderByIdAsync(string id)
		{
			var data = await _orderReadRepository.Table
				.Include(o => o.Basket)
				.ThenInclude(b => b.BasketItems)
				.ThenInclude(bi => bi.Product)
				.ThenInclude(p=>p.ProductImageFiles)
				.FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));

			if (data == null) new ErrorDataResult<SingleOrderDto>("Sipariş bulunamadı.");

			var singleOrder =  new SingleOrderDto()
			{
				Id = data.Id.ToString(),
				OrderCode = data.OrderCode,
				Address = data.Address,
				Description = data.Description,
				CreatedDate = data.CreateDate,
				BasketItems = data.Basket.BasketItems.Select(bi => new OrderItemDto
				{
					Name = bi.Product.Name,
					ImagePath = bi.Product.ProductImageFiles.FirstOrDefault() != null
						? bi.Product.ProductImageFiles.FirstOrDefault().Path
						: null,
					Price = bi.Price,
					Quantity = bi.Quantity
				}),
				DiscountAmount = data.DiscountAmount,
				FinalTotalPrice = data.FinalTotalPrice,
				TotalBasePrice = data.TotalBasePrice
			};

			return new SuccessDataResult<SingleOrderDto>(singleOrder);
		}

		public async Task<IResult> SendInvoiceMassegeAsync(string id)
		{
			var email = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

			if (string.IsNullOrEmpty(email))
				return new ErrorResult("Kullanıcı e-posta adresi bulunamadı.");

			var data = await _orderReadRepository.Table
				.Include(o => o.Basket)
				.ThenInclude(b => b.BasketItems)
				.ThenInclude(bi => bi.Product)
				.ThenInclude(p => p.ProductImageFiles)
				.FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));

			if (data == null)
				return new ErrorResult("Sipariş bulunamadı.");

			var singleOrder = new SingleOrderDto()
			{
				Id = data.Id.ToString(),
				OrderCode = data.OrderCode,
				Address = data.Address,
				Description = data.Description,
				CreatedDate = data.CreateDate,
				BasketItems = data.Basket.BasketItems.Select(bi => new OrderItemDto
				{
					Name = bi.Product.Name,
					ImagePath = bi.Product.ProductImageFiles.FirstOrDefault() != null
						? bi.Product.ProductImageFiles.FirstOrDefault().Path
						: null,
					Price = bi.Price,
					Quantity = bi.Quantity
				}),
				DiscountAmount = data.DiscountAmount,
				FinalTotalPrice = data.FinalTotalPrice,
				TotalBasePrice = data.TotalBasePrice
			};

			await _mailService.SendInvoiceMailAsync(email, singleOrder);

			return new SuccessResult("Fatura başarıyla e-posta adresinize gönderildi.");
		}

	}
}
