using System;
using DataAccess.DBModels;

namespace DataAccess.DataContext
{
    public interface IDataConnection
    {
        Prize CreatePrize(Prize model);

        Person CreatePerson(Person model);

        TeamDB CreateTeam(TeamDB model);

        Tournament CreateTournament(Tournament model);

        List<Person> GetPerson_All();

        List<TeamDB> GetTeam_All();

        List<Prize> GetPrizes_All();
    }
}

