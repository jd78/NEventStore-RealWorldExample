using System;

namespace FootballMatchStatEventStore.Contracts
{
    public interface IEvent
    {
        Guid Id { get; set; }
        int Version { get; set; }
    }
}
