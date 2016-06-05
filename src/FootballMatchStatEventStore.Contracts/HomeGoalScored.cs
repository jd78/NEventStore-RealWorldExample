using System;

namespace FootballMatchStatEventStore.Contracts
{
    public class HomeGoalScored : IEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string Scorer { get; set; }
    }
}
