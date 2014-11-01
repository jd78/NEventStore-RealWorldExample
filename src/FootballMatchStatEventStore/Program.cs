using System;
using Autofac;
using FootballMatchStatEventStore.Domain;
using FootballMatchStatEventStore.Services;

namespace FootballMatchStatEventStore
{
    class Program
    {
        private static void Main()
        {
            var matchService = IocBootstrapper.Container.Resolve<IMatchService>();
            
            var match = matchService.Create("Napoli", "Roma");

            match.UpdateMatchStatus(MatchStatus.FirstHalf);

            match.UpdateHomeScore("Higuain");
            match.UpdateHomeScore("Higuain");
            match.UpdateHomeScore("Callejon");
            match.UpdateHomeScore("Mertens");

            match.UpdateMatchStatus(MatchStatus.HalfTime);

            matchService.Update(match);

            Console.WriteLine("Half time score {0}", match.Result);

            match.UpdateMatchStatus(MatchStatus.SecondHalf);

            match.UpdateAwayScore("Totti");

            match.UpdateHomeScore("Jorginho");

            match.UpdateMatchStatus(MatchStatus.Ended);

            Console.Write(match.Status);
            Console.WriteLine("Final score {0}", match.Result);

            matchService.Update(match);

            Console.ReadLine();
        }
    }
}
