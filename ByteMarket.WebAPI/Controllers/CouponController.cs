using ByteMarket.Business.Abstract;
using ByteMarket.Business.DTOs.Category;
using ByteMarket.Business.DTOs.Coupon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteMarket.WebAPI.Controllers
{
	public class CouponController : BaseController
	{
		private readonly ICouponService _couponService;
		public CouponController(ICouponService couponService)
		{
			_couponService = couponService;
		}

		[HttpPost]
		public async Task<IActionResult> Add(CreateCouponDto createCouponDto)
		{
			var result = await _couponService.AddAsync(createCouponDto);
			return CreateActionResult(result, successStatusCode: 201);
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var result = await _couponService.GetAllAsync();
			return CreateActionResult(result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetbyId(string id)
		{
			var result = await _couponService.GetCouponByIdAsync(id);
			return CreateActionResult(result);
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody] UpdateCouponDto updateCouponDto)
		{
			var result = await _couponService.UpdateCouponAsync(updateCouponDto);
			return CreateActionResult(result, errorStatusCode: 404);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			var result = await _couponService.DeleteAsync(id);
			return CreateActionResult(result, errorStatusCode: 404);
		}
	}
}
