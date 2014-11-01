using System;
using CommonDomain.Persistence;
using FootballMatchStatEventStore.Domain;

namespace FootballMatchStatEventStore.Services
{
    public class MatchService : IMatchService
    {
        private readonly IRepository _repository;

        public MatchService(IRepository repository)
        {
            _repository = repository;
        }

        public Match Create(string homeTeam, string awayTeam)
        {
            var match = Match.CreateMatch(homeTeam, awayTeam);
            _repository.Save(match, Guid.NewGuid());
            return match;
        }

        public void Update(Match match)
        {
            _repository.Save(match, Guid.NewGuid());
        }

        public Match Read(Guid id)
        {
            return _repository.GetById<Match>(id);
        }
    }
}
