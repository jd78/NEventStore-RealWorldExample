using System;
using System.Collections.Generic;
using System.Linq;
using CommonDomain;

namespace FootballMatchStatEventStore.Domain
{
    public class MatchMemento : IMemento
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public ICollection<string> HomeTeamScorers { get; set; }
        public ICollection<string> AwayTeamScorers { get; set; }
        public MatchStatus Status { get; set; }

        public static MatchMemento Create(IAggregate matchAggregate)
        {
            var match = matchAggregate as Match;
            if (match == null)
            {
                throw new ApplicationException("Aggregate version mismatch");
            }

            return new MatchMemento
            {
                Id = match.Id,
                AwayTeam = match.GetAwayTeam(),
                AwayTeamScorers = match.GetAwayTeamScorers().ToList(),
                HomeTeam = match.GetHomeTeam(),
                HomeTeamScorers = match.GetHomeTeamScorers().ToList(),
                Status = match.GetMatchStatus(),
                Version = match.Version
            };
        }
    }
}
