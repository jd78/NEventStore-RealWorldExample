using CommonDomain.Core;
using FootballMatchStatEventStore.Contracts;

namespace FootballMatchStatEventStore.Domain
{
    public class DomainBase : AggregateBase
    {
        public virtual void RaiseDomainEvent(IEvent @event)
        {
            @event.Id = Id;
            @event.Version = Version;
            RaiseEvent(@event);
        }
    }
}
