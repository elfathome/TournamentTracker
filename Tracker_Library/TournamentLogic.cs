using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.DBModels;

namespace Tracker_Library
{
    public static class TournamentLogic
    {
        // order list randomly
        // check if it is big enough, add in byes - 2*2*2*2 - 2^4
        // create our 1st round of matchus
        // create every other round after that - 8, 4, 2, 1 matchups
        public static void CreateRounds(Tournament model)
        {
            ICollection<TournamentEntry> randomizedTeams = RandomizeTeamOrder(model.TournamentEntries);
            int rounds = FindNumberOfRounds(randomizedTeams.Count);
            int byes = NumberOfByes(rounds, randomizedTeams.Count);

            model.Rounds.Add(CreateFirstRound(byes, randomizedTeams));

            CreateOtherRounds(model, rounds);
        }

        private static void CreateOtherRounds(Tournament model, int rounds)
        {
            int round = 2;
            List<Matchup> previousRound = model.Rounds[0];
            List<Matchup> currRound = new List<Matchup>();
            Matchup currMatchup = new Matchup();

            while (round <= rounds)
            {
                foreach (Matchup match in previousRound)
                {
                    currMatchup.MatchupEntries.Add(new MatchupEntry { ParentMatchup = match });

                    if (currMatchup.MatchupEntries.Count > 1)
                    {
                        currMatchup.MatchupRound = round;
                        currRound.Add(currMatchup);
                        currMatchup = new Matchup();
                    }
                }

                model.Rounds.Add(currRound);
                previousRound = currRound;

                currRound = new List<Matchup>();
                round += 1;
            }
        }

        private static List<Matchup> CreateFirstRound(int byes, ICollection<TournamentEntry> entries)
        {
            List<Matchup> output = new List<Matchup>();
            Matchup curr = new Matchup();

            foreach (var entry in entries)
            {
                curr.MatchupEntries.Add(new MatchupEntry { TeamCompetingId = entry.TeamId });

                if (byes > 0 || curr.MatchupEntries.Count > 1)
                {
                    curr.MatchupRound = 1;
                    output.Add(curr);
                    curr = new Matchup();

                    if (byes > 0)
                    {
                        byes -= 1;
                    }
                }
            }

            return output;
        }

        private static int NumberOfByes(int rounds, int numberOfEntries)
        {
            int output = 0;
            int totalEntries = 1;

            for (int i = 1; i <= rounds; i++)
            {
                totalEntries *= 2;
            }

            output = totalEntries - numberOfEntries;

            return output;
        }

        private static int FindNumberOfRounds(int entryCount)
        {
            int output = 1;
            int val = 2;

            while (val < entryCount)
            {
                output += 1;

                val *= 2;
            }

            return output;
        }

        private static ICollection<TournamentEntry> RandomizeTeamOrder(ICollection<TournamentEntry> entries)
        {
            return entries.OrderBy(x => Guid.NewGuid()).ToList();
        }
    }
}

