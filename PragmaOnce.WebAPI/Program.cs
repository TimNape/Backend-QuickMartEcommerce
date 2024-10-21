using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PragmaOnce.Core.src.Common;
using PragmaOnce.Core.src.Entities;
using PragmaOnce.Core.src.Entities.Shop.OrderAggregate;
using PragmaOnce.Core.src.Interfaces;
using PragmaOnce.Core.src.Interfaces.Shop;
using PragmaOnce.Core.src.Interfaces.TalentHub;
using PragmaOnce.Service.src.Interfaces;
using PragmaOnce.Service.src.Interfaces.Shop;
using PragmaOnce.Service.src.Interfaces.TalentHub;
using PragmaOnce.Service.src.Services;
using PragmaOnce.Service.src.Services.Shop;
using PragmaOnce.Service.src.Services.TalentHub;
using PragmaOnce.Service.src.Shared;
using PragmaOnce.WebAPI.src.AuthorizationPolicy;
using PragmaOnce.WebAPI.src.Data;
using PragmaOnce.WebAPI.src.ExternalService;
using PragmaOnce.WebAPI.src.Middleware;
using PragmaOnce.WebAPI.src.Repositories;
using PragmaOnce.WebAPI.src.Repositories.Shop;
using PragmaOnce.WebAPI.src.Repositories.TalentHub;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);


// Add AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

//Cloudinary
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

//Cashing
builder.Services.AddMemoryCache();

// Add all controllers
builder.Services.AddControllers(
    Options =>
    {
        Options.SuppressAsyncSuffixInActionNames = false;
    }
);
builder.Services.AddEndpointsApiExplorer();

//Add authorization for Swagger
builder.Services.AddSwaggerGen(
    options =>
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Description = "Bearer token authentication",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Scheme = "Bearer"
        }
        );

        options.OperationFilter<SecurityRequirementsOperationFilter>();
    }
);


var connectionString = builder.Configuration.GetConnectionString("local");

// Add DB context
builder.Services.AddDbContext<AppDbContext>
(
    options =>
    options.UseSqlServer(connectionString)
           .UseLazyLoadingProxies()
           .EnableSensitiveDataLogging()
           .EnableDetailedErrors()
           .AddInterceptors(new TimeStampInteceptor())
);


//var serverVersion = new MySqlServerVersion(new Version(10, 5, 9));

//var connectionString = builder.Configuration.GetConnectionString("local");

//// Add DB context
//var dataSourceBuilder = new MySqlDataSourceBuilder(connectionString);
////var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("local"));


//var dataSource = dataSourceBuilder.Build();
//builder.Services.AddDbContext<AppDbContext>
//(
//options =>
////options.UseNpgsql(dataSource)
////        .UseSnakeCaseNamingConvention()
//options.UseMySql(connectionString, serverVersion)
//        .UseLazyLoadingProxies()
//        .EnableSensitiveDataLogging()
//        .EnableDetailedErrors()
//        .AddInterceptors(new TimeStampInteceptor())
//);

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// service registration -> automatically create all instances of dependencies
builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
// Auth
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();


// Talenthub
builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();
builder.Services.AddScoped<IRecruiterRepository, RecruiterRepository>();
builder.Services.AddScoped<IVacancyRepository, VacancyRepository>();
builder.Services.AddScoped<ICandidateService, CandidateService>();
builder.Services.AddScoped<IRecruiterService, RecruiterService>();
builder.Services.AddScoped<IVacancyService, VacancyService>();

// Category
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
// Review
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewService, ReviewService>();
// Product
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
// ProductImage
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();
//Order and OrderItems
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IBaseRepository<OrderItem, QueryOptions>, OrderItemRepository>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
// Address
builder.Services.AddScoped<IBaseRepository<Address, QueryOptions>, AddressRepository>();
//Cloudinary
builder.Services.AddScoped<ICloudinaryImageService, CloudinaryImageService>();
//Stripe
builder.Services.AddScoped<IStripeService, StripeService>();

// Configure Cloudinary
var cloudinaryAccount = new Account(
    builder.Configuration["CloudinarySettings:CloudName"],
    builder.Configuration["CloudinarySettings:ApiKey"],
    builder.Configuration["CloudinarySettings:ApiSecret"]
);
var cloudinary = new Cloudinary(cloudinaryAccount);
builder.Services.AddSingleton(cloudinary);

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Secrets:JwtKey"]!)),
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Secrets:Issuer"]
    };
}
);

// Resource based auth handlers
builder.Services.AddSingleton<IAuthorizationHandler, AdminOrOwnerAccountHandler>();

// config authorization
builder.Services.AddAuthorization(policy =>
{
    policy.AddPolicy("AdminOrOwnerAccount", policy => policy.Requirements.Add(new AdminOrOwnerAccountRequirement()));
});

var app = builder.Build();

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler("/Error");
app.UseDeveloperExceptionPage();
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
