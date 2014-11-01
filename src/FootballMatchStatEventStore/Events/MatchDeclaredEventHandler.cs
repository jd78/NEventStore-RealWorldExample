using System;
using FootballMatchStatEventStore.Contracts;
using FootballMatchStatEventStore.Infrastructure;

namespace FootballMatchStatEventStore.Events
{
    public class MatchDeclaredEventHandler : IEventHandler<MatchDeclared>
    {
        public void Handle(MatchDeclared @event)
        {
            Console.WriteLine("Match {0} - {1} declared", @event.HomeTeam, @event.AwayTeam);
        }
    }
}
