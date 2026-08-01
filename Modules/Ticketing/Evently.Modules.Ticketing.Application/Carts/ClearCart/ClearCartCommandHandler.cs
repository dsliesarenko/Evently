using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Users.PublicApi;

namespace Evently.Modules.Ticketing.Application.Carts.ClearCart;

internal sealed class ClearCartCommandHandler(IUsersApi usersApi, CartService cartService)
    : ICommandHandler<ClearCartCommand>
{
    public async ValueTask<Result> Handle(
        ClearCartCommand request,
        CancellationToken cancellationToken
    )
    {
        UserResponse? customer = await usersApi.GetAsync(request.CustomerId, cancellationToken);

        if (customer is null)
        {
            return Result.Failure(CustomerErrors.NotFound(request.CustomerId));
        }

        await cartService.ClearAsync(customer.Id, cancellationToken);

        return Result.Success();
    }
}
