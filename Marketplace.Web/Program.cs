using Marketplace.Web.Infrastructure;
using Marketplace.Web.Modules.Carts.Domain.Entities;
using Marketplace.Web.Modules.Carts.Infrastructure;
using Marketplace.Web.Modules.Carts.Presentation.GraphQL;
using Marketplace.Web.Modules.Orders.Presentation.GraphQL;
using Marketplace.Web.Modules.Products.Domain.Entities;
using Marketplace.Web.Modules.Products.Presentation.GraphQL;
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

var builder = WebApplication.CreateBuilder(args);

//DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("IdentityDb")));

//Identity
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppIdentityDbContext>() // или твой IdentityDbContext
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

//Identity сервисы
builder.Services.AddScoped<ITokenService, TokenService>();

//GraphQL
builder.Services
    .AddGraphQLServer()
    .AddAuthorizationCore()
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
    .AddType<OrderType>()
    .AddMutationConventions()
    .AddFiltering()
    .AddSorting();

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

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();
app.MapGraphQL();
app.UseHttpsRedirection();
app.Run();
