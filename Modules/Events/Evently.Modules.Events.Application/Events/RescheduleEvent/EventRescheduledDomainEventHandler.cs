using Evently.Common.Application.Messaging;
using Evently.Modules.Events.Domain.Events;

namespace Evently.Modules.Events.Application.Events.RescheduleEvent;

internal sealed class EventRescheduledDomainEventHandler
    : IDomainEventHandler<EventRescheduledDomainEvent>
{
    public ValueTask Handle(
        EventRescheduledDomainEvent domainEvent,
        CancellationToken cancellationToken
    )
    {
        return ValueTask.CompletedTask;
    }
}
