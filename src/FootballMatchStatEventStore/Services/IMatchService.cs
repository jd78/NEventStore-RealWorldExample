using System;
using FootballMatchStatEventStore.Domain;

namespace FootballMatchStatEventStore.Services
{
    public interface IMatchService
    {
        Match Create(string homeTeam, string awayTeam);
        Match Read(Guid id);
        void Update(Match match);
    }
}
