using ByteMarket.WebUI.Constants;
using ByteMarket.WebUI.Services.Implementations;
using ByteMarket.WebUI.Services.Interfaces;
using ByteMarket.WebUI.Utilities.Handlers;
using ByteMarket.WebUI.Utilities.Helpers.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ICategoryService = ByteMarket.WebUI.Services.Interfaces.ICategoryService;
using IProductService = ByteMarket.WebUI.Services.Interfaces.IProductService;
using IRoleService = ByteMarket.WebUI.Services.Interfaces.IRoleService;
using IUserService = ByteMarket.WebUI.Services.Interfaces.IUserService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAntiforgery(options =>
{
	options.HeaderName = "RequestVerificationToken";
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AuthTokenHandler>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.LoginPath = "/Account/Login";
		options.AccessDeniedPath = "/Account/AccessDenied";

		options.Events = new CookieAuthenticationEvents
		{
			OnValidatePrincipal = async context =>
			{
				var jwt = context.HttpContext.Request.Cookies["jwt"];

				if (!string.IsNullOrEmpty(jwt))
				{
					var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
					var token = handler.ReadJwtToken(jwt);

					
					if (token.ValidTo < DateTime.UtcNow.AddSeconds(30))
					{
						var accountService = context.HttpContext.RequestServices.GetRequiredService<IAccountService>();
						var isRefreshed = await accountService.RefreshTokenAsync();

						if (!isRefreshed)
						{
							context.RejectPrincipal();
							await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
						}
					}
				}
			}
		};
	});

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy(AuthorizePolicies.AdminOnly, policy =>
		policy.RequireRole("Admin"));

	options.AddPolicy(AuthorizePolicies.CustomerOnly, policy =>
		policy.RequireRole("Customer"));
});

var apiBaseAddress = builder.Configuration.GetSection("ApiSettings:BaseAddress").Value;

builder.Services.AddHttpClient("MyApiClient", client =>
{
	client.BaseAddress = new Uri(apiBaseAddress);
}).AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAuthHelper, AuthHelper>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWishListService, WishListService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
