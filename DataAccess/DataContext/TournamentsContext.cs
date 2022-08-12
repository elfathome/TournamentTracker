using System.Collections.Generic;
using System.Data;
using DataAccess.DBModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.DataContext;
public partial class TournamentsContext : DbContext
{
    private readonly IConfiguration? _configuration;
    public TournamentsContext()
    {
    }

    public TournamentsContext(DbContextOptions<TournamentsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Matchup> Matchups { get; set; } = null!;
    public virtual DbSet<MatchupEntry> MatchupEntries { get; set; } = null!;
    public virtual DbSet<Person> People { get; set; } = null!;
    public virtual DbSet<Prize> Prizes { get; set; } = null!;
    public virtual DbSet<TeamDB> Teams { get; set; } = null!;
    public virtual DbSet<TeamMember> TeamMembers { get; set; } = null!;
    public virtual DbSet<Tournament> Tournaments { get; set; } = null!;
    public virtual DbSet<TournamentEntry> TournamentEntries { get; set; } = null!;
    public virtual DbSet<TournamentPrize> TournamentPrizes { get; set; } = null!;

    public virtual Prize InsertPrize(Prize prize)
    {
        SqlParameter? placeNumParam = new SqlParameter("@PlaceNumber", prize.PlaceNumber);
        SqlParameter? placeNameParam = new SqlParameter("@PlaceName", prize.PlaceName);
        SqlParameter? prizeAmountParam = new SqlParameter("@PrizeAmount", prize.PrizeAmount);
        SqlParameter? prizePercentageParam = new SqlParameter("@PrizePercentage", prize.PrizePercentage);
        SqlParameter? idParam = new SqlParameter("@id", SqlDbType.Int) { Direction = ParameterDirection.Output };

        var theId = Prizes.FromSqlRaw($"EXECUTE dbo.spPrizes_Insert @PlaceNumber, @PlaceName, @PrizeAmount, @PrizePercentage, @id = @id OUTPUT",
            placeNumParam,
            placeNameParam,
            prizeAmountParam,
            prizePercentageParam,
            idParam).ToList();
        Prize x = theId.First();

        return x;
    }

    public virtual Person InsertPerson(Person person)
    {
        SqlParameter? firstNameParam = new SqlParameter("@FirstName", person.FirstName);
        SqlParameter? lastNameParam = new SqlParameter("@LastName", person.LastName);
        SqlParameter? emailParam = new SqlParameter("@EmailAddress", person.EmailAddress);
        SqlParameter? cellParam = new SqlParameter("@CellPhoneNumber", person.CellphoneNumber);
        SqlParameter? idParam = new SqlParameter("@id", SqlDbType.Int) { Direction = ParameterDirection.Output };

        var theId = People.FromSqlRaw($"EXECUTE dbo.spPeople_Insert @FirstName, @LastName, @EmailAddress, @CellPhoneNumber, @id = @id OUTPUT",
            firstNameParam,
            lastNameParam,
            emailParam,
            cellParam,
            idParam).ToList();
        Person x = theId.First();

        return x;
    }

    public virtual TeamDB InsertTeam(TeamDB team)
    {
        SqlParameter teamNameParam = new SqlParameter("@TeamName", team.TeamName);
        SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int) { Direction = ParameterDirection.Output };

        var theId = Teams.FromSqlRaw($"EXECUTE dbo.spTeam_Insert @TeamName, @id = @id OUTPUT", teamNameParam, idParam).ToList();
        team.Id = theId.First().Id;

        foreach (var member in team.TeamMembers)
        {
            SqlParameter? teamIdParam = new SqlParameter("@TeamId", team.Id);
            SqlParameter? personIdParam = new SqlParameter("@PersonId", member.PersonId);
            idParam = new SqlParameter("@id", SqlDbType.Int) { Direction = ParameterDirection.Output };

            var theMember = TeamMembers.FromSqlRaw($"EXECUTE dbo.spTeamMembers_Insert @TeamId, @PersonId, @id = @id OUTPUT",
                teamIdParam,
                personIdParam,
                idParam).ToList();
            TeamMember x = theMember.First();
        }

        return team;
    }

    public virtual Tournament InsertTournament(Tournament tournament)
    {
        SaveTournament(tournament);
        SaveTournamentEntries(tournament);
        SaveTournamentPrizes(tournament);
        SaveTournamentRounds(tournament);

        return tournament;
    }

    private void SaveTournament(Tournament tournament)
    {
        SqlParameter tournamentNameParam = new SqlParameter("@TournamentName", tournament.TournamentName);
        SqlParameter tournamentFeeParam = new SqlParameter("@EntryFee", tournament.EntryFee);
        SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int) { Direction = ParameterDirection.Output };

        var tournies = Tournaments.FromSqlRaw($"EXECUTE dbo.spTournaments_Insert @TournamentName, @EntryFee, @id = @id OUTPUT",
            tournamentNameParam,
            tournamentFeeParam,
            idParam).ToList();

        tournament.Id = tournies.First().Id;
    }

    private void SaveTournamentEntries(Tournament tournament)
    {
        foreach (var entry in tournament.TournamentEntries)
        {
            SqlParameter teamIdParam = new SqlParameter("@TeamId", entry.TeamId);
            SqlParameter tournamentIdParam = new SqlParameter("@TournamentId", tournament.Id);
            SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int) { Direction = ParameterDirection.Output };

            var theEntry = TournamentEntries.FromSqlRaw($"EXECUTE dbo.spTournamentEntries_Insert @TournamentId, @TeamId, @id = @id OUTPUT",
                tournamentIdParam,
                teamIdParam,
                idParam).ToList();
        }
    }

    private void SaveTournamentRounds(Tournament tournament)
    {
        foreach (List<Matchup> round in tournament.Rounds)
        {
            foreach (Matchup matchup in round)
            {
                SqlParameter matchupRoundParam = new SqlParameter("@MatchupRound", matchup.MatchupRound);
                SqlParameter tournamentIdParam = new SqlParameter("@TournamentId", tournament.Id);
                SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int) { Direction = ParameterDirection.Output };

                Matchup theMatchup = Matchups.FromSqlRaw($"EXECUTE dbo.spMatchups_Insert @TournamentId, @MatchupRound, @id = @id OUTPUT",
                    tournamentIdParam,
                    matchupRoundParam,
                    idParam).ToList().First();

                matchup.Id = theMatchup.Id;

                foreach (MatchupEntry entry in matchup.MatchupEntries)
                {
                    SqlParameter matchupIdParam = new SqlParameter("@MatchupId", matchup.Id);
                    SqlParameter parentMatchupIdParam;
                    SqlParameter teamCompetingIdParam;

                    if (entry.ParentMatchupId == null)
                    {
                        parentMatchupIdParam = new SqlParameter("@ParentMatchupId", 49);
                    }
                    else
                    {
                        parentMatchupIdParam = new SqlParameter("@ParentMatchupId", entry.ParentMatchupId);
                    }

                    if (entry.TeamCompetingId == null)
                    {
                        teamCompetingIdParam = new SqlParameter("@TeamCompetingId", null);
                    }
                    else
                    {
                        teamCompetingIdParam = new SqlParameter("@TeamCompetingId", entry.TeamCompetingId);
                    }

                    SqlParameter theIdParam = new SqlParameter("@id", SqlDbType.Int) { Direction = ParameterDirection.Output };

                MatchupEntry? y = MatchupEntries.FromSqlRaw($"EXECUTE dbo.spMatchupEntries_Insert @MatchupId, @ParentMatchupId, @TeamCompetingId, @id = @id OUTPUT",
                        matchupIdParam,
                        parentMatchupIdParam,
                        teamCompetingIdParam,
                        theIdParam).ToList().First();
                    
                    var x = y;
                }
            }
        }
    }

    private void SaveTournamentPrizes(Tournament tournament)
    {
        foreach (var prize in tournament.TournamentPrizes)
        {
            SqlParameter prizeIdParam = new SqlParameter("@PrizeId", prize.PrizeId);
            SqlParameter tournamentIdParam = new SqlParameter("@TournamentId", tournament.Id);
            SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int) { Direction = ParameterDirection.Output };

            var thePrize = TournamentPrizes.FromSqlRaw($"EXECUTE dbo.spTournamentPrizes_Insert @TournamentId, @PrizeId, @id = @id OUTPUT",
                tournamentIdParam,
                prizeIdParam,
                idParam).ToList();
        }
    }

    public virtual List<Person> GetAllPeople()
    {
        var people = People.FromSqlRaw($"EXECUTE dbo.spPeople_GetAll").ToList();

        return people;
    }

    public virtual List<Prize> GetAllPrizes()
    {
        var prizes = Prizes.FromSqlRaw($"EXECUTE dbo.spPrizes_GetAll").ToList();

        return prizes;
    }

    public virtual List<TeamDB> GetAllTeams()
    {
        List<TeamDB>? teams = Teams.FromSqlRaw($"EXECUTE dbo.spTeam_GetAll").ToList();

        foreach (var team in teams)
        {
            SqlParameter teamParam = new SqlParameter("@TeamId", team.Id);
            var x = People.FromSqlRaw("EXECUTE dbo.spTeamMembers_GetByTeam @TeamId", teamParam).ToList();

            foreach (var person in x)
            {
                TeamMember member = new TeamMember();
                member.Person = person;
                member.Team = team;
                member.PersonId = person.Id;
                member.TeamId = team.Id;

                team.TeamMembers.Add(member);
            }
        }

        return teams;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {

            string conString = _configuration.GetConnectionString("DefaultConnectionString");

            optionsBuilder.UseSqlServer("Server=localhost;Database=Tournaments;User=sa;Password=4cliffkitty;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Matchup>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");

            entity.HasOne(d => d.Winner)
                .WithMany(p => p.Matchups)
                .HasForeignKey(d => d.WinnerId)
                .HasConstraintName("FK_Matchups_Teams");
        });

        modelBuilder.Entity<MatchupEntry>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");

            entity.HasOne(d => d.Matchup)
                .WithMany(p => p.MatchupEntries)
                .HasForeignKey(d => d.MatchupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MatchupEntries_Matchups");

            entity.HasOne(d => d.TeamCompeting)
                .WithMany(p => p.MatchupEntries)
                .HasForeignKey(d => d.TeamCompetingId)
                .HasConstraintName("FK_MatchupEntries_Teams");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.CellphoneNumber).HasMaxLength(12);

            entity.Property(e => e.EmailAddress)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.Property(e => e.FirstName).HasMaxLength(100);

            entity.Property(e => e.LastName).HasMaxLength(150);
        });

        modelBuilder.Entity<Prize>(entity =>
        {
            
            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.PlaceName).HasMaxLength(150);

            entity.Property(e => e.PrizeAmount).HasColumnType("money");

        });

        modelBuilder.Entity<TeamDB>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.TeamName).HasMaxLength(200);
        });

        modelBuilder.Entity<TeamMember>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");

            entity.HasOne(d => d.Person)
                .WithMany(p => p.TeamMembers)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeamMembers_People");

            entity.HasOne(d => d.Team)
                .WithMany(p => p.TeamMembers)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeamMembers_Teams");
        });

        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.EntryFee).HasColumnType("money");

            entity.Property(e => e.TournamentName).HasMaxLength(150);
        });

        modelBuilder.Entity<TournamentEntry>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.TeamId).HasColumnName("TeamID");

            entity.HasOne(d => d.Team)
                .WithMany(p => p.TournamentEntries)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TournamentEntries_Teams");

            entity.HasOne(d => d.Tournament)
                .WithMany(p => p.TournamentEntries)
                .HasForeignKey(d => d.TournamentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TournamentEntries_Tournaments");
        });

        modelBuilder.Entity<TournamentPrize>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");

            entity.HasOne(d => d.Prize)
                .WithMany(p => p.TournamentPrizes)
                .HasForeignKey(d => d.PrizeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TournamentPrizes_Prizes");

            entity.HasOne(d => d.Tournament)
                .WithMany(p => p.TournamentPrizes)
                .HasForeignKey(d => d.TournamentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TournamentPrizes_Tournaments");
        });

        //new
        modelBuilder.Entity<List<Matchup>>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<List<List<Matchup>>>(entity =>
        {
            entity.HasNoKey();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

