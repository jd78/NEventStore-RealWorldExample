using System;

namespace FootballMatchStatEventStore.Contracts
{
    public class MatchDeclared : IEvent
    {
        public Guid Id { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int Version { get; set; }
    }
}
