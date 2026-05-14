using Evently.Modules.Events.Application.Events;
using Evently.Modules.Events.Application.Events.GetEvent;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal static class GetEvent
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "events/{id}",
                async (Guid id, ISender sender, CancellationToken ct) =>
                {
                    EventResponse? @event = await sender.Send(new GetEventQuery(id), ct);

                    return @event is null ? Results.NotFound() : Results.Ok(@event);
                }
            )
            .WithTags(Tags.Events);
    }
}
