using System;
using DataAccess.DBModels;

namespace DataAccess.DataContext
{
    public class SqlConnector : IDataConnection
    {
        private readonly TournamentsContext? _db;
        public SqlConnector(TournamentsContext db)
        {
            _db = db;
        }

        public SqlConnector()
        {

        }

        public Prize CreatePrize(Prize model)
        {
            Prize x = new Prize();
            if (_db != null)
            {

                x = _db.InsertPrize(model);
            }

            return x;
        }

        public Person CreatePerson(Person model)
        {
            Person x = new Person();
            if (_db != null)
            {
                x = _db.InsertPerson(model);
            }
            return x;
        }

        public TeamDB CreateTeam(TeamDB model)
        {
            TeamDB x = new TeamDB();
            if(_db != null)
            {
                x = _db.InsertTeam(model);
            }

            return x;
        }

        public Tournament CreateTournament(Tournament model)
        {
            var x = new Tournament();
            if (_db != null)
            {
                x = _db.InsertTournament(model);
            }

            return x;
        }

        public List<Person> GetPerson_All()
        {
            List<Person>? x = new List<Person>();

            if (_db != null)
            {
                x = _db.GetAllPeople();
            }

            return x;
        }

        public List<TeamDB> GetTeam_All()
        {
            List<TeamDB>? x = new List<TeamDB>();

            if (_db != null)
            {
                x = _db.GetAllTeams();
            }

            return x;
        }

        public List<Prize> GetPrizes_All()
        {
            List<Prize>? x = new List<Prize>();

            if (_db != null)
            {
                x = _db.GetAllPrizes();
            }

            return x;
        }

    }
}

