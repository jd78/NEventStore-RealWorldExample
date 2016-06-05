using System;

namespace FootballMatchStatEventStore.Contracts
{
    public class MatchStatusUpdated : IEvent
    {
        public Guid Id { get; set; }
        public MatchStatus Status { get; set; }
        public int Version { get; set; }
    }
}
