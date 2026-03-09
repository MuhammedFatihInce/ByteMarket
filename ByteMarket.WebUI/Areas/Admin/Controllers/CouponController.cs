using ByteMarket.WebUI.Areas.Admin.Models.Coupon;
using ByteMarket.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace ByteMarket.WebUI.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CouponController : Controller
	{
		private readonly ICouponService _couponService;
		private readonly IProductService _productService;
		private readonly IConfiguration _configuration;
		public CouponController(ICouponService couponService, IProductService productService, IConfiguration configuration)
		{
			_couponService = couponService;
			_productService = productService;
			_configuration = configuration;
		}
		public async Task<IActionResult> Index()
		{
			var coupons = await _couponService.GetCouponsForAdminAsync();
			if(coupons.Success)
				return View(coupons.Data);

			return View(new ListCouponViewModel());

		}

		[HttpPost("Admin/Coupon/Create")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([FromBody]CreateCouponViewModel model)
		{
			var result = await _couponService.AddCategoryAsync(model);

			return Json(new { success = result.Success, message = result.Message });
		}

		[HttpGet]
		public async Task<IActionResult> GetAllProductsForSelect([FromQuery] string q)
		{
			var apiBaseUrl = _configuration["ApiSettings:BaseStorageAdress"];
			string noImageUrl = "https://placehold.co/100x100?text=Resim+Yok";


			var products = await _productService.GetAllProductByFilterAsync(q);

			if (products?.Data == null)
			{
				return Json(new List<object>());
			}

			var result = products.Data.Select(p =>
			{
				string fullImagePath = !string.IsNullOrEmpty(p.ImagePath)
					? $"{apiBaseUrl?.TrimEnd('/')}/{p.ImagePath.TrimStart('\\', '/').Replace("\\", "/")}"
					: noImageUrl;

				return new
				{
					id = p.Id,
					text = p.Name,
					image = fullImagePath 
				};
			}).ToList();

			return Json(result);
		}

		[HttpGet("/Admin/Coupon/GetCouponForUpdate/{id}")]
		public async Task<IActionResult> GetCouponForUpdate(string id)
		{
			var result = await _couponService.GetCouponsForUpdateAsync(id);

			return Json(result.Data);
		}

		[HttpPut("/Admin/Coupon/Update")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update([FromBody] UpdateCouponViewModel model)
		{
			var result = await _couponService.UpdateCouponAsync(model);

			return Json(new { success = result.Success, message = result.Message });
		}

		[HttpDelete]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(string id)
		{
			var result = await _couponService.DeleteCouponAsync(id);

			return Json(new { success = result.Success, message = result.Message });
		}

	}
}
