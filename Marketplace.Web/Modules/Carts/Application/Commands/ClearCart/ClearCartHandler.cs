using Marketplace.Web.Modules.Carts.Infrastructure;
using MediatR;

namespace Marketplace.Web.Modules.Carts.Application.Commands.ClearCart
{
    public class ClearCartHandler : IRequestHandler<ClearCartCommand>
    {
        private readonly RedisCartRepository _repository;

        public ClearCartHandler(RedisCartRepository repository)
            => _repository = repository;

        public async Task Handle(ClearCartCommand command, CancellationToken token)
        {
            await _repository.DeleteCartAsync(command.UserId);
        }
    }
}
