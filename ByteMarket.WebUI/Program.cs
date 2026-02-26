using ByteMarket.WebUI.Services.Implementations;
using ByteMarket.WebUI.Services.Interfaces;
using ByteMarket.WebUI.Utilities.Handlers;
using ByteMarket.WebUI.Utilities.Helpers.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AuthTokenHandler>();

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

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
