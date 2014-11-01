using FootballMatchStatEventStore.Contracts;

namespace FootballMatchStatEventStore.Infrastructure
{
    public interface IEventHandler<in TEvent>  where TEvent : IEvent
    {
        void Handle(TEvent @event);
    }
}
