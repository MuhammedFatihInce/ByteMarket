using ByteMarket.WebUI.Services.Implementations;
using ByteMarket.WebUI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var apiBaseAddress = builder.Configuration.GetSection("ApiSettings:BaseAddress").Value;

builder.Services.AddHttpClient("MyApiClient", client =>
{
	client.BaseAddress = new Uri(apiBaseAddress);
});
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<IProductService, ProductService>();

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
