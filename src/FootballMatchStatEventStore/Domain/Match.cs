using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FootballMatchStatEventStore.Contracts;

namespace FootballMatchStatEventStore.Domain
{
    public sealed class Match : DomainBase
    {
        private string HomeTeam { get; set; }
        private string AwayTeam { get; set; }
        private ICollection<string> HomeTeamScorers { get; set; }
        private ICollection<string> AwayTeamScorers { get; set; }
        private MatchStatus Status { get; set; }

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

        #region Query

        public MatchStatus GetMatchStatus()
        {
            return Status;
        }

        public int GoalsPerHomePlayer(string name)
        {
            return HomeTeamScorers.Count(p => p == name);
        }

        public int GoalsPerAwayPlayer(string name)
        {
            return AwayTeamScorers.Count(p => p == name);
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
