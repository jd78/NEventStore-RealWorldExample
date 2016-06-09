using System;
using Autofac;
using FootballMatchStatEventStore.Services;

namespace FootballMatchStatEventStore
{
    class Program
    {
        private static void Main()
        {
            var matchService = IocBootstrapper.Container.Resolve<IMatchService>();
            Create(matchService);
            Console.ReadLine();
        }

        public static void Create(IMatchService matchService)
        {
            var match = matchService.Create("Napoli", "Roma");

            match.UpdateMatchStatus(Domain.MatchStatus.FirstHalf);

            match.UpdateHomeScore("Higuain");
            match.UpdateHomeScore("Higuain");
            match.UpdateHomeScore("Callejon");
            match.UpdateHomeScore("Mertens");

            match.UpdateMatchStatus(Domain.MatchStatus.HalfTime);

            matchService.Update(match);

            Console.WriteLine("Half time score {0}", match.Result);

            match.UpdateMatchStatus(Domain.MatchStatus.SecondHalf);

            match.UpdateAwayScore("Totti");

            match.UpdateHomeScore("Jorginho");

            match.UpdateMatchStatus(Domain.MatchStatus.Ended);

            Console.Write(match.GetMatchStatus());
            Console.WriteLine("Final score {0}", match.Result);

            matchService.Update(match);

            Console.WriteLine("***Some stats***");
            Console.WriteLine("Higuain scored {0} goal", match.GoalsPerHomePlayer("Higuain"));
            Console.WriteLine("Totti scored {0} goal", match.GoalsPerAwayPlayer("Totti"));

            Console.WriteLine("________________________FSharp Domain________________________");

            var matchFsharp = matchService.CreateFSharp("Napoli", "Roma");

            matchFsharp.updateMatchStatus(Domain.FSharp.MatchStatus.FirstHalf);

            matchFsharp.updateHomeScore("Higuain");
            matchFsharp.updateHomeScore("Higuain");
            matchFsharp.updateHomeScore("Callejon");
            matchFsharp.updateHomeScore("Mertens");

            matchFsharp.updateMatchStatus(Domain.FSharp.MatchStatus.HalfTime);

            matchService.UpdateFsharp(matchFsharp);

            Console.WriteLine("Half time score {0}", matchFsharp.Result);

            matchFsharp.updateMatchStatus(Domain.FSharp.MatchStatus.SecondHalf);

            matchFsharp.updateAwayScore("Totti");

            matchFsharp.updateHomeScore("Jorginho");

            matchFsharp.updateMatchStatus(Domain.FSharp.MatchStatus.Ended);

            Console.Write(matchFsharp.GetMatchStatus);
            Console.WriteLine("Final score {0}", matchFsharp.Result);

            matchService.UpdateFsharp(matchFsharp);

            Console.WriteLine("***Some stats***");
            Console.WriteLine("Higuain scored {0} goal", matchFsharp.GoalsPerHomePlayer("Higuain"));
            Console.WriteLine("Totti scored {0} goal", matchFsharp.GoalsPerAwayPlayer("Totti"));

        }
    }
}
