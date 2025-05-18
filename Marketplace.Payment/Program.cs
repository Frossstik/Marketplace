using Marketplace.Payment.Application.Commands.ProcessPayment;
using Marketplace.Payment.Domain.Enums;
using Marketplace.Payment.Infrastructure.Interfaces;
using Marketplace.Payment.Infrastructure.PaymentProcessor;
using Marketplace.Payment.Infrastructure;
using Marketplace.Payment.Presentation.GraphQL.Mutations;
using Marketplace.Payment.Presentation.GraphQL.Queries;
using Marketplace.Payment.Presentation.GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Marketplace.Payment.Infrastructure.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PaymentDatabase")));

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(ProcessPaymentCommand).Assembly));

// Payment processors
builder.Services.AddScoped<BankCardService>();
builder.Services.AddScoped<YooMoneyService>();
builder.Services.AddScoped<IPaymentProcessorFactory, PaymentProcessorFactory>();

//var rabbitMqConfig = builder.Configuration.GetConnectionString("RabbitMQ");

builder.Services.AddSingleton<IMessageBusPublisher>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("RabbitMQ") ?? "amqp://guest:guest@localhost:5672";
    return RabbitMqPublisher.CreateAsync(connectionString)
        .GetAwaiter()
        .GetResult();
});

builder.Services.AddHostedService<RabbitMqDisposerService>();

// GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType(d => d.Name("Query"))
        .AddTypeExtension<PaymentQueries>()
    .AddMutationType(d => d.Name("Mutation"))
        .AddTypeExtension<PaymentMutations>()
    .AddType<PaymentType>()
    .AddType<PaymentInputType>()
    .AddType<EnumType<PaymentMethod>>()
    .AddType<EnumType<PaymentStatus>>();

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapGraphQL();

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.Run();