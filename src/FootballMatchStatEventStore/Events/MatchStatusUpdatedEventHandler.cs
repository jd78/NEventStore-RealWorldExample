using System;
using FootballMatchStatEventStore.Contracts;
using FootballMatchStatEventStore.Domain;
using FootballMatchStatEventStore.Infrastructure;

namespace FootballMatchStatEventStore.Events
{
    public class MatchStatusUpdatedEventHandler : IEventHandler<MatchStatusUpdated>
    {
        public void Handle(MatchStatusUpdated @event)
        {
            if (@event.Status == MatchStatus.Ended)
                Console.WriteLine("The match is ended!");
        }
    }
}
