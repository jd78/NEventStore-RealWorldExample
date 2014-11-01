using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FootballMatchStatEventStore.Contracts;

namespace FootballMatchStatEventStore.Domain
{
    public sealed class Match : DomainBase
    {
        public string HomeTeam { get; private set; }
        public string AwayTeam { get; private set; }
        public ICollection<string> HomeTeamScorers { get; private set; }
        public ICollection<string> AwayTeamScorers { get; private set; }
        public MatchStatus Status { get; private set; }

        public string Result
        {
            get { return string.Format("{0} - {1}", HomeTeamScorers.Count(), AwayTeamScorers.Count()); }
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
            HomeTeamScorers = new Collection<string>();
            AwayTeamScorers = new Collection<string>();
            Status = MatchStatus.Declared;
        }

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
            Status = status.Status;
        }

        private void Apply(MatchDeclared match)
        {
            HomeTeam = match.HomeTeam;
            AwayTeam = match.AwayTeam;
        }

        private void Apply(HomeGoalScored goal)
        {
            HomeTeamScorers.Add(goal.Scorer);
        }

        private void Apply(AwayGoalScored goal)
        {
            AwayTeamScorers.Add(goal.Scorer);
        }

        #endregion
    }
}
