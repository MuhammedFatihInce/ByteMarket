
using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Order;
using ByteMarket.Business.Utilities.Results;
using ByteMarket.DataAccess.Abstract.Order;
using Microsoft.EntityFrameworkCore;

namespace ByteMarket.Business.Concrete
{
	public class OrderManager:IOrderService
	{
		readonly IOrderWriteRepository _orderWriteRepository;
		readonly IOrderReadRepository _orderReadRepository;

		public OrderManager(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository)
		{
			_orderWriteRepository = orderWriteRepository;
			_orderReadRepository = orderReadRepository;
		}

		public async Task<IResult> CreateOrderAsync(CreateOrderDto createOrderDto)
		{
			var orderCode = new Random().Next(100000, 999999).ToString() + DateTime.Now.Ticks.ToString().Substring(10);

			var result = await _orderWriteRepository.AddAsync(new()
			{
				Address = createOrderDto.Address,
				Id = Guid.Parse(createOrderDto.BasketId),
				Description = createOrderDto.Description,
				OrderCode = orderCode
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
			var query = _orderReadRepository.Table
				.Include(o => o.Basket)
				.ThenInclude(b => b.User)
				.Include(o => o.Basket)
				.ThenInclude(b => b.BasketItems);

			if (query == null) new ErrorDataResult<List<OrderListDetailDto>>("Siparişler bulunamadı.");

			var orders = await query.Select(o => new OrderListDetailDto()
			{
				Id = o.Id.ToString(),
				OrderCode = o.OrderCode,
				CreatedDate = o.CreateDate,
				UserName = o.Basket.User.UserName,
				TotalPrice = o.Basket.BasketItems.Sum(bi => bi.Price * bi.Quantity)
			}).ToListAsync();


			return new SuccessDataResult<List<OrderListDetailDto>>(orders);

		}

		public async Task<IDataResult<SingleOrderDto>> GetOrderByIdAsync(string id)
		{
			var data = await _orderReadRepository.Table
				.Include(o => o.Basket)
				.ThenInclude(b => b.BasketItems)
				.ThenInclude(bi => bi.Product)
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
					Price = bi.Price,
					Quantity = bi.Quantity
				})
			};

			return new SuccessDataResult<SingleOrderDto>(singleOrder);
		}
	}
}
