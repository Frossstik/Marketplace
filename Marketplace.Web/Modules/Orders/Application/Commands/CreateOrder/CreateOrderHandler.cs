using Marketplace.Web.Infrastructure;
using Marketplace.Web.Infrastructure.RabbitMQ;
using Marketplace.Web.Modules.Orders.Domain;
using Marketplace.Web.Modules.Orders.Domain.Entities;
using Marketplace.Web.Modules.Orders.Domain.Events;
using MediatR;

namespace Marketplace.Web.Modules.Orders.Application.Commands.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly AppDbContext _db;
        private readonly IPublisher _publisher;
        private readonly IMessageBusPublisher _bus;

        public CreateOrderHandler(AppDbContext db, IPublisher publisher, IMessageBusPublisher messageBusPublisher)
        {
            _db = db;
            _publisher = publisher;
            _bus = messageBusPublisher;
        }

        public async Task<Guid> Handle(CreateOrderCommand command, CancellationToken token)
        {
            var order = new Order
            {
                UserId = command.UserId,
                Items = command.Items
            };

            await _db.Orders.AddAsync(order, token);
            await _db.SaveChangesAsync(token);

            // Отправка события для платежного сервиса
            await _bus.PublishAsync(new OrderCreatedEvent(order.Id, order.TotalPrice), token);

            return order.Id;
        }
    }
}
