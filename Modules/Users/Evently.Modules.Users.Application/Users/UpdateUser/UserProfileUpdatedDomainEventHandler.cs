using Evently.Common.Application.Messaging;
using Evently.Modules.Users.Domain.Users;

namespace Evently.Modules.Users.Application.Users.UpdateUser;

internal sealed class UserProfileUpdatedDomainEventHandler()
    : IDomainEventHandler<UserProfileUpdatedDomainEvent>
{
    public async ValueTask Handle(
        UserProfileUpdatedDomainEvent notification,
        CancellationToken cancellationToken
    ) { }
}
