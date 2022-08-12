
using DataAccess.DBModels;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DataAccess.DataContext.TextHelpers
{
    public static class TextConnectorProcessor
    {
        public static string FullFilePath(this string fileName)
        {
            string path = Directory.GetCurrentDirectory();
            string fullPath = $"{ path }/{ fileName }";
            return fullPath;
        }


        public static List<string> LoadFile(this string file)
        {
            if (!File.Exists(file))
            {
                return new List<string>();
            }

            return File.ReadAllLines(file).ToList();
        }

        public static List<Person> ConvertToPersonModels(this List<string> lines)
        {
            List<Person> output = new List<Person>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                Person p = new Person();
                p.Id = int.Parse(cols[0]);
                p.FirstName = cols[1];
                p.LastName = cols[2];
                p.EmailAddress = cols[3];
                p.CellphoneNumber = cols[4];

                output.Add(p);
            }
            return output;
        }

        public static List<Tournament> ConvertToTournamentModels(this List<string> lines, string teamFileName, string peopleFileName, string prizesFileName)
        {
            //id,TournamentName,EntryFee,id|id|id- enteredTeams,id|id|id-Prizes,id^id^id|id^id^id|id^id^id-Rounds
            List<Tournament> output = new List<Tournament>();
            List<TeamDB> teams = teamFileName.FullFilePath().LoadFile().ConvertToTeamModels(peopleFileName);
            List<Prize> prizes = prizesFileName.FullFilePath().LoadFile().ConvertToPrizeModels();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                decimal fee = 0;
                bool feeAcceptable = decimal.TryParse(cols[2], out fee); 

                Tournament t = new Tournament();
                t.Id = int.Parse(cols[0]);
                t.TournamentName = cols[1];
                t.EntryFee = fee;

                string[] teamIds = cols[3].Split('|');

                foreach (string teamId in teamIds)
                {
                    var team = teams.Where(x => x.Id == int.Parse(teamId)).First();
                    t.TournamentEntries.Add(new TournamentEntry()
                    {
                        TeamId = team.Id,
                        Team = team,
                        Tournament = t,
                        TournamentId = t.Id
                    });
                }

                string[] prizeIds = cols[4].Split('|');

                foreach (string prizeId in prizeIds)
                {
                    var prize = prizes.Where(x => x.Id == int.Parse(prizeId)).First();
                    t.TournamentPrizes.Add(new TournamentPrize()
                    {
                        Tournament = t,
                        TournamentId = t.Id,
                        Prize = prize,
                        PrizeId = prize.Id
                    });
                }

                //TODO Capture Rounds Information.

                output.Add(t);
            }

            return output;
        }

        public static List<Prize> ConvertToPrizeModels(this List<string> lines)
        {
            List<Prize> output = new List<Prize>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                Prize p = new Prize();
                p.Id = int.Parse(cols[0]);
                p.PlaceNumber = int.Parse(cols[1]);
                p.PlaceName = cols[2];
                p.PrizeAmount = decimal.Parse(cols[3]);
                p.PrizePercentage = double.Parse(cols[4]);

                output.Add(p);
            }

            return output;
        }

        public static List<TeamDB> ConvertToTeamModels(this List<string> lines, string peopleFileName)
        {
            //id,teamName,list of ids seperated by pipe
            //3,Goofy Foot,1|3|5
            List<TeamDB> output = new List<TeamDB>();
            List<Person> people = peopleFileName.FullFilePath().LoadFile().ConvertToPersonModels();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                TeamDB t = new TeamDB();
                t.Id = int.Parse(cols[0]);
                t.TeamName = cols[1];
                t.TeamMembers = new List<TeamMember>();

                string[] personIds = cols[2].Split('|');

                foreach (string personId in personIds)
                {
                    Person p = people.Where(x => x.Id == int.Parse(personId)).First();

                    TeamMember tm = new TeamMember()
                    {
                        TeamId = t.Id,
                        PersonId = int.Parse(personId),
                        Person = p,
                        Team = t
                    };

                    t.TeamMembers.Add(tm);
                }

                output.Add(t);
            }
            return output;
        }

        public static void SaveToPersonFile(this List<Person> models, string filename)
        {
            List<string> lines = new List<string>();

            foreach (Person p in models)
            {
                lines.Add($"{ p.Id },{ p.FirstName },{ p.LastName },{ p.EmailAddress },{ p.CellphoneNumber }");
            }

            string x = filename.FullFilePath();

            File.WriteAllLines(x, lines);
        }

        public static void SaveToTournamentsFile(this List<Tournament> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (Tournament t in models)
            {
                lines.Add($"{ t.Id },{ t.TournamentName },{ t.EntryFee },{ ConvertTeamListToString(t.TournamentEntries) },{ ConvertPrizeListToString(t.TournamentPrizes) },{ ConvertRoundListToString(t.Rounds) }");
            }

            string x = fileName.FullFilePath();
            File.WriteAllLines(x, lines);
        }

        private static string ConvertRoundListToString(List<List<Matchup>> rounds)
        {
            string output = "";

            if (rounds.Count == 0)
            {
                return "";
            }

            foreach (List<Matchup> r in rounds)
            {
                output += $"{ ConvertMatchupListToString(r) }|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertMatchupListToString(List<Matchup> matchups)
        {
            string output = "";

            if (matchups.Count == 0)
            {
                return "";
            }

            foreach (Matchup m in matchups)
            {
                output += $"{ m.Id }^";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertPrizeListToString(ICollection<TournamentPrize> prizes)
        {
            string output = "";

            if (prizes.Count == 0)
            {
                return "";
            }

            foreach (TournamentPrize tp in prizes)
            {
                output += $"{ tp.PrizeId }|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertTeamListToString(ICollection<TournamentEntry> entries)
        {
            string output = "";

            if (entries.Count == 0)
            {
                return "";
            }

            foreach (TournamentEntry te in entries)
            {
                output += $"{ te.TeamId }|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }

        public static void SaveToPrizeFile(this List<Prize> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (Prize p  in models)
            {
                lines.Add($"{ p.Id }, { p.PlaceNumber }, { p.PlaceName }, { p.PrizeAmount }, { p.PrizePercentage }");
            }

            string x = fileName.FullFilePath();
            File.WriteAllLines(x, lines);
        }

        public static void SaveToTeamsFile(this List<TeamDB> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (TeamDB t in models)
            {
                lines.Add($"{ t.Id },{ t.TeamName },{ ConvertPeopleListToString(t.TeamMembers.ToList())}");
            }

            string x = fileName.FullFilePath();
            File.WriteAllLines(x, lines);
        }

        private static object ConvertPeopleListToString(List<TeamMember> people)
        {
            string output = "";

            if (people.Count == 0)
            {
                return "";
            }

            foreach (TeamMember p in people)
            {
                output += $"{ p.PersonId }|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }
    }
}

