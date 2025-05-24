using Marketplace.Web.Infrastructure;
using Marketplace.Web.Modules.Carts.Domain.Entities;
using Marketplace.Web.Modules.Carts.Infrastructure;
using Marketplace.Web.Modules.Carts.Presentation.GraphQL;
using Marketplace.Web.Modules.Orders.Presentation.GraphQL;
using Marketplace.Web.Modules.Products.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using Marketplace.Web.Modules.Identity.Application.Interfaces;
using Marketplace.Web.Modules.Identity.Infrastructure;
using Marketplace.Web.Modules.Identity.Domain.Entities;
using Marketplace.Web.Modules.Identity.Application.Commands.Register;
using Marketplace.Web.Modules.Identity.Presentation.GraphQL.Queries;
using Marketplace.Web.Modules.Identity.Presentation.GraphQL.Mutations;
using Marketplace.Web.Modules.Identity.Domain.Enums;
using Marketplace.Web.Modules.Categories.Application.Interfaces;
using Marketplace.Web.Modules.Categories.Infrastructure;
using Marketplace.Web.Modules.Categories.Presentation.GraphQL.Queries;
using Marketplace.Web.Modules.Categories.Presentation.GraphQL.Mutations;
using Marketplace.Web.Modules.Categories.Domain.Entities;
using Marketplace.Web.Infrastructure.RabbitMQ;
using Marketplace.Web.Modules.Products.Application.Commands.CreateProduct;
using Marketplace.Web.Modules.Products.Application.Commands.DeleteProduct;
using Marketplace.Web.Modules.Products.Application.Commands.UpdateProduct;
using Marketplace.Web.Modules.Products.Application.Queries.GetProductById;
using Marketplace.Web.Modules.Products.Application.Queries.GetProducts;
using MediatR;
using Marketplace.Web.Modules.Products.Presentation.GraphQL.Types;
using Marketplace.Web.Modules.Products.Presentation.GraphQL.Queries;
using Marketplace.Web.Modules.Products.Presentation.GraphQL.Mutations;
using Marketplace.Web.Modules.Identity.Presentation.GraphQL.Types;
using Marketplace.Web.Modules.Categories.Presentation.GraphQL.Types;
using HotChocolate.Types.Pagination;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

//DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("IdentityDb")));

//Identity
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppIdentityDbContext>()
.AddDefaultTokenProviders();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
        options.SaveTokens = true;
    })
    .AddYandex(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Yandex:ClientId"]!;
        options.ClientSecret = builder.Configuration["Authentication:Yandex:ClientSecret"]!;
        options.SaveTokens = true;
    });

//Identity сервисы
builder.Services.AddScoped<ITokenService, TokenService>();



//GraphQL
builder.Services
    .AddGraphQLServer()
    .ModifyPagingOptions(options =>
    {
        options.DefaultPageSize = 10;
        options.MaxPageSize = 50;
        options.IncludeTotalCount = true;
        options.RequirePagingBoundaries = false;
    })
    .AddAuthorization()
    .AddProjections()
    .AddQueryType(q => q.Name("Query"))
        .AddType<ProductQueries>()
        .AddType<CategoryQueries>()
        .AddType<OrderQueries>()
        .AddType<CartQueries>()
        .AddType<AuthQuery>()
    .AddMutationType(m => m.Name("Mutation"))
        .AddType<ProductMutations>()
        .AddType<CategoryMutations>()
        .AddType<OrderMutations>()
        .AddType<CartMutations>()
        .AddType<AuthMutation>()
    .AddType<ProductType>()
    .AddType<ProductInputType>()
    .AddType<UpdateProductInputType>()
    .AddType<OrderType>()
    .AddType<UserType>()
    .AddType<AuthResponseType>()
    .AddType<RegisterInputType>()
    .AddType<RoleEnumType>()
    .AddType<CategoryType>()
    .AddType<CategoryInputType>()
    .AddMutationConventions()
    .AddFiltering()
    .AddSorting();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost5173", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

//MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        typeof(Product).Assembly,
        typeof(Category).Assembly,
        typeof(Marketplace.Web.Modules.Orders.Domain.Entities.Order).Assembly,
        typeof(Cart).Assembly,
        typeof(AppDbContext).Assembly,
        typeof(RegisterCommand).Assembly
    );
});

//Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetSection("Redis")["ConnectionString"]));
builder.Services.AddScoped<RedisCartRepository>();

// Авторизация по ролям
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("SellerOnly", policy => policy.RequireRole("Seller"));
});

//Category сервисы
builder.Services
    .AddScoped<ICategoryRepository, CategoryRepository>()
    .AddScoped<ICategoryService, CategoryService>();

//Product сервисы
builder.Services
    .AddScoped<IRequestHandler<CreateProductCommand, Product>, CreateProductHandler>()
    .AddScoped<IRequestHandler<UpdateProductCommand, Product>, UpdateProductHandler>()
    .AddScoped<IRequestHandler<DeleteProductCommand, bool>, DeleteProductHandler>()
    .AddScoped<IRequestHandler<GetProductsQuery, List<Product>>, GetProductsHandler>()
    .AddScoped<IRequestHandler<GetProductByIdQuery, Product?>, GetProductByIdHandler>();

//RabbitMQ
builder.Services.AddSingleton<IMessageBusPublisher>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("RabbitMQ") ?? "amqp://guest:guest@localhost:5672";
    return RabbitMqPublisher.CreateAsync(connectionString)
        .GetAwaiter()
        .GetResult();
});

var app = builder.Build();

//Роли
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

    foreach (var roleName in Enum.GetNames(typeof(RoleEnum)))
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
        }
    }
}

app.UseCors("AllowLocalhost5173");

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/signin-google", () => Results.Challenge(
    new AuthenticationProperties { RedirectUri = "/graphql" },
    new[] { "Google" }
));

app.MapGet("/signin-yandex", () => Results.Challenge(
    new AuthenticationProperties { RedirectUri = "/graphql" },
    new[] { "Yandex" }
));

app.MapGraphQL();
app.UseHttpsRedirection();
app.Run();
