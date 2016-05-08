using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommonDomain;
using FootballMatchStatEventStore.Contracts;

namespace FootballMatchStatEventStore.Domain
{
    public sealed class Match : DomainBase, ICloneable
    {
        private string _homeTeam;
        private string _awayTeam;
        private ICollection<string> _homeTeamScorers;
        private ICollection<string> _awayTeamScorers;
        private MatchStatus _status;

        public object Clone()
        {
            var clone = new Match(Id, _homeTeam, _awayTeam)
            {
                _awayTeamScorers = _awayTeamScorers.Select(p => p).ToList(),
                _homeTeamScorers = _homeTeamScorers.Select(p => p).ToList(),
                _status = _status,
                Id = Id,
                Version = Version
            };
            return clone;
        }

        public static Match CreateMatch(string homeTeam, string awayTeam)
        {
            return new Match(Guid.NewGuid(), homeTeam, awayTeam);
        }

        private Match(Guid id, string homeTeam, string awayTeam) : this(id)
        {
            var @event = new MatchDeclared
            {
                AwayTeam = awayTeam,
                HomeTeam = homeTeam
            };
            RaiseDomainEvent(@event);
        }

        private Match(Guid id)
        {
            Id = id;
            _homeTeamScorers = new Collection<string>();
            _awayTeamScorers = new Collection<string>();
            _status = MatchStatus.Declared;
        }
        
        public Match(IMemento matchMemento)
        {
            var snapshot = matchMemento as MatchMemento;
            if (snapshot == null)
                throw new ApplicationException("matchMemento parameter mismatch");
            
            Id = snapshot.Id;
            _homeTeamScorers = snapshot.HomeTeamScorers;
            _awayTeamScorers = snapshot.AwayTeamScorers;
            _status = snapshot.Status;
            _awayTeam = snapshot.AwayTeam;
            _homeTeam = snapshot.HomeTeam;
            Version = matchMemento.Version;
        }

        #region Query

        public string Result
        {
            get { return string.Format("{0} - {1}", _homeTeamScorers.Count(), _awayTeamScorers.Count()); }
        }

        public string GetHomeTeam()
        {
            return _homeTeam;
        }

        public string GetAwayTeam()
        {
            return _awayTeam;
        }

        public IEnumerable<string> GetHomeTeamScorers()
        {
            return _homeTeamScorers;
        }
        public IEnumerable<string> GetAwayTeamScorers()
        {
            return _awayTeamScorers;
        }

        public MatchStatus GetMatchStatus()
        {
            return _status;
        }

        public int GoalsPerHomePlayer(string name)
        {
            return _homeTeamScorers.Count(p => p == name);
        }

        public int GoalsPerAwayPlayer(string name)
        {
            return _awayTeamScorers.Count(p => p == name);
        }

        #endregion

        #region Mutators

        public void UpdateMatchStatus(MatchStatus status)
        {
            var @event = new MatchStatusUpdated
            {
                Status = status
            };
            RaiseDomainEvent(@event);
        }

        public void UpdateHomeScore(string scorer)
        {
            var @event = new HomeGoalScored
            {
                Scorer = scorer
            };
            RaiseDomainEvent(@event);
        }

        public void UpdateAwayScore(string scorer)
        {
            var @event = new AwayGoalScored
            {
                Scorer = scorer
            };
            RaiseDomainEvent(@event);
        }

        #endregion

        #region Event Applies

        private void Apply(MatchStatusUpdated status)
        {
            _status = status.Status;
        }

        private void Apply(MatchDeclared match)
        {
            _homeTeam = match.HomeTeam;
            _awayTeam = match.AwayTeam;
        }

        private void Apply(HomeGoalScored goal)
        {
            _homeTeamScorers.Add(goal.Scorer);
        }

        private void Apply(AwayGoalScored goal)
        {
            _awayTeamScorers.Add(goal.Scorer);
        }

        #endregion
    }
}
