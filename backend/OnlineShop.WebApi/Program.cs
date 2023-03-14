using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineShop.Data;
using OnlineShop.Data.Repositories;
using OnlineShop.Domain.RepositoriesInterfaces;
using OnlineShop.Domain.Services;
using OnlineShop.WebApi.Configurations;
using OnlineShop.WebApi.Filters;
using OnlineShop.WebApi.Middlewares;
using OnlineShop.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionsFilter>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var dbPath = "myapp.db";
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlite($"Data Source={dbPath}"));
builder.Services.AddHttpLogging(options => 
{
    options.LoggingFields = HttpLoggingFields.RequestHeaders
                            | HttpLoggingFields.ResponseHeaders
                            | HttpLoggingFields.RequestBody
                            | HttpLoggingFields.ResponseBody;
});
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
//builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CartService>();
builder.Services.Configure<PasswordHasherOptions>(opt => opt.IterationCount = 10000);
builder.Services.AddSingleton<IPasswordHasherService, Pbkd2PasswordHasher>();
builder.Services.AddCors();
builder.Services.AddScoped<ITokenService,JwtTokenService>();
JwtConfig jwtConfig = builder.Configuration   //перед методом AddAuthentication()
    .GetSection("JwtConfig")
    .Get<JwtConfig>()!;
builder.Services.AddSingleton(jwtConfig);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(jwtConfig.SigningKeyBytes),
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            RequireSignedTokens = true,
          
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidAudiences = new[] { jwtConfig.Audience },
            ValidIssuer = jwtConfig.Issuer
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();




app.UseHttpsRedirection();
app.UseHttpLogging();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseCors(policy =>
    policy.WithOrigins(
            "https://localhost:44383", "https://api.mysite.com")
        .AllowAnyHeader()
        .AllowAnyMethod()
);



app.UseMiddleware<CountGoingOnPagesMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();



//{
//     app.MapGet("/Products", async (
//             [FromServices]IProductRepository productRepository,
//             CancellationToken cancellationToken
//         )
//         => await productRepository.GetAll( cancellationToken));
// }
//
//
// app.MapGet(pattern: "/Products/{id:guid}", async (
//         [FromRoute] Guid id,
//         CancellationToken cancellationToken,
//         [FromServices] IProductRepository productRepository
//         )
//     =>
// {
//     //Product? product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
//     Product? product = await productRepository.GetById(id,cancellationToken);
//     if (product is null)
//     {
//         return Results.NotFound(new { message = "Товар не найден!" });
//     }
//
//     return Results.Ok(product);
// });
//
//
// app.MapPost("/Products", async (
//         [FromServices] IProductRepository productRepository,
//         [FromBody] Product product,
//         CancellationToken cancellationToken,
//         HttpResponse response)
//     =>
// {
//     product.Id = Guid.NewGuid();
//     await productRepository.Add(product,cancellationToken);
//     response.StatusCode = StatusCodes.Status201Created;
// });
//
//
// app.MapPut("/Products/{id:guid}", async (
//     [FromRoute] Guid id,
//     [FromBody] Product product,
//     CancellationToken cancellationToken,
//   
//     [FromServices] IProductRepository productRepository) =>
// {
//     // product = await productRepository.GetById(id,cancellationToken);
//     if (product is null)
//     {
//         return Results.NotFound();
//     }
//     else
//     {
//         await productRepository.Update(product,cancellationToken);
//         return Results.Ok();
//     }
// });
// app.MapDelete("/Products/{id:guid}", async (
//         [FromRoute] Guid id,
//         CancellationToken cancellationToken,
//         [FromServices] IProductRepository productRepository)
//     =>
// {
//       await productRepository.Delete(id,cancellationToken);
// });
