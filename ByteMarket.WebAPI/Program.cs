using ByteMarket.Business;
using ByteMarket.Entities.Constants;
using ByteMarket.Business.Validators.Products;
using ByteMarket.DataAccess;
using ByteMarket.DataAccess.Contexts;
using ByteMarket.DataAccess.SeedData;
using ByteMarket.Entities.Concrete.Identity;
using ByteMarket.WebAPI.Filters;
using ByteMarket.WebAPI.Middleware;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataAccessServices(builder.Configuration);
builder.Services.AddBusinessServices();

// Add services to the container.

builder.Services.AddControllers(options =>
{
	options.Filters.Add<ValidationFilter>();
})
.ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowWebUI", policy =>
	{
		policy.WithOrigins("https://localhost:44380")
			.AllowAnyHeader()
			.AllowAnyMethod(); 
	});
});


builder.Services.AddIdentity<AppUser, AppRole>(options =>
	{
		options.Password.RequiredLength = 6;
		options.Password.RequireNonAlphanumeric = false;
		options.Password.RequireDigit = false;
		options.Password.RequireLowercase = false;
		options.Password.RequireUppercase = false;

		options.User.RequireUniqueEmail = true;

		options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
		options.Lockout.MaxFailedAccessAttempts = 5;
	})
	.AddEntityFrameworkStores<ByteMarketDbContext>()
	.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new()
		{
			ValidateAudience = true,
			ValidateIssuer = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,

			ValidAudience = builder.Configuration["Token:Audience"],
			ValidIssuer = builder.Configuration["Token:Issuer"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
			ClockSkew = TimeSpan.Zero,
		};
	}); 


builder.Services.AddAuthorization(options =>
{
	options.AddPolicy(AuthorizePolicies.AdminOnly, policy =>
		policy.RequireRole("Admin"));

	options.AddPolicy(AuthorizePolicies.CustomerOnly, policy =>
		policy.RequireRole("Customer"));

	options.AddPolicy(AuthorizePolicies.FullProductManagement, policy =>
		policy.RequireClaim("Permission",
			AuthorizePolicies.ProductsPermissions));

	options.AddPolicy(AuthorizePolicies.FullCategoryManagement, policy =>
		policy.RequireClaim("Permission",
			AuthorizePolicies.CategoriesPermissions));

	options.AddPolicy(AuthorizePolicies.FullUserManagement, policy =>
		policy.RequireClaim("Permission",
			AuthorizePolicies.UsersPermissions));

	options.AddPolicy(AuthorizePolicies.FullRoleManagement, policy =>
		policy.RequireClaim("Permission",
			AuthorizePolicies.RolesPermissions));
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors("AllowWebUI");

await DbInitializer.SeedAsync(app.Services);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
