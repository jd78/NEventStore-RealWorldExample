using System;
using CommonDomain;
using FootballMatchStatEventStore.Domain;

namespace FootballMatchStatEventStore.Services
{
    public interface IMatchService
    {
        Match Create(string homeTeam, string awayTeam);
        Match Read(Guid id);
        void Update(IAggregate match);

        Domain.FSharp.Match CreateFSharp(string homeTeam, string awayTeam);
        Domain.FSharp.Match ReadFSharp(Guid id);
        void UpdateFsharp(IAggregate match);
    }
}
