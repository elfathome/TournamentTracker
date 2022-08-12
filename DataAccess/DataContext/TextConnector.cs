using System;
using DataAccess.DataContext.TextHelpers;
using DataAccess.DBModels;

namespace DataAccess.DataContext
{
    public class TextConnector : IDataConnection
    {
        private const string PrizesFile = "Prize.csv";
        private const string PeopleFile = "Person.csv";
        private const string TeamsFile = "Team.csv";
        private const string TournamentsFile = "Tournament.csv";

        public Person CreatePerson(Person model)
        {
            List<Person> people = PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();

            int currentId = 1;

            if(people.Count > 0)
            {
                currentId = people.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            people.Add(model);

            people.SaveToPersonFile(PeopleFile);

            return model;
        }

        public Prize CreatePrize(Prize model)
        {
            //Load text file and convert to list
            List<Prize> prizes = PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

            //find id
            int currentId = 1;

            if (prizes.Count > 0)
            {
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            //add new record
            prizes.Add(model);

            //save the list
            prizes.SaveToPrizeFile(PrizesFile);

            return model;
        }

        public TeamDB CreateTeam(TeamDB model)
        {
            List<TeamDB> teams = TeamsFile.FullFilePath().LoadFile().ConvertToTeamModels(PeopleFile);

            int currentId = 1;

            if (teams.Count > 0)
            {
                currentId = teams.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            teams.Add(model);

            teams.SaveToTeamsFile(TeamsFile);

            return model;
        }

        public Tournament CreateTournament(Tournament model)
        {
            List<Tournament> tournaments = TournamentsFile.FullFilePath().LoadFile().ConvertToTournamentModels(TeamsFile, PeopleFile, PrizesFile);

            int currentId = 1;

            if (tournaments.Count > 0)
            {
                currentId = tournaments.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            tournaments.Add(model);

            tournaments.SaveToTournamentsFile(TournamentsFile);

            return model;
        }

        public List<Person> GetPerson_All()
        {
            //Load text file and convert to list
            List<Person> allPeople = PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();

            return allPeople;
        }

        public List<Prize> GetPrizes_All()
        {
            List<Prize> allPrizes = PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

            return allPrizes;
        }

        public List<TeamDB> GetTeam_All()
        {
            List<TeamDB> allTeams = TeamsFile.FullFilePath().LoadFile().ConvertToTeamModels(PeopleFile);

            return allTeams;
        }
    }
}

