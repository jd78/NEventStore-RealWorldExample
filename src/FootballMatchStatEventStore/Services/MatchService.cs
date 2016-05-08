using System;
using System.Linq;
using System.Threading.Tasks;
using CommonDomain;
using CommonDomain.Persistence;
using FootballMatchStatEventStore.Contracts;
using FootballMatchStatEventStore.Domain;
using NEventStore;

namespace FootballMatchStatEventStore.Services
{
    public class MatchService : IMatchService
    {
        private readonly IRepository _repository;
        private readonly IStoreEvents _store;
        private readonly Settings _settings;

        public MatchService(IRepository repository, IStoreEvents store, Settings settings)
        {
            _repository = repository;
            _store = store;
            _settings = settings;
        }

        public Match Create(string homeTeam, string awayTeam)
        {
            var match = Match.CreateMatch(homeTeam, awayTeam);
            _repository.Save(match, Guid.NewGuid());
            return match;
        }

        public void Update(IAggregate match)
        {
            var takeSnapshot = match.GetUncommittedEvents().Cast<IEvent>().Any(p => p.Version > 0 && p.Version % _settings.SnapshotEvery == 0);
            _repository.Save(match, Guid.NewGuid());
            if (!takeSnapshot) return;

            var matchCopy = ((Match)match).Clone() as IAggregate;
            if (matchCopy == null)
            {
                throw new ApplicationException("Something wrong while cloning the aggregate");
            }
            Task.Run(() =>
            {
                _store.Advanced.AddSnapshot(new Snapshot(matchCopy.Id.ToString(), matchCopy.Version, MatchMemento.Create(matchCopy)));
            });
        }

        public Match Read(Guid id)
        {
            return _repository.GetById<Match>(id);
        }
    }
}
