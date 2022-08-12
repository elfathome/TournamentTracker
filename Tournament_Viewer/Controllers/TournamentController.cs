using System;
using System.Diagnostics;
using DataAccess.DataContext;
using DataAccess.DBModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tournament_UI.Models;
using Tracker_Library;
using Tracker_UI.Models;

namespace Tracker_UI.Controllers
{
    public class TournamentController : Controller
    {
        private readonly ILogger<TournamentController> _logger;
        private readonly IDataConnection _dataConnection;

        public TournamentController(ILogger<TournamentController> logger, IDataConnection dataConn)
        {
            _logger = logger;
            _dataConnection = dataConn;
        }

        public IActionResult Create()
        {
            TournamentViewModel tvm = InitializeModel();

            return View(tvm);
        }

        // POST: Team/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TournamentViewModel model)
        {
            try
            {
                var tournament = new Tournament()
                {
                    TournamentName = model.TournamentName,
                    EntryFee = model.EntryFee,
                    TournamentPrizes = new List<TournamentPrize>(),
                    TournamentEntries = new List<TournamentEntry>()
                };

                foreach (var selectedTeamId in model.SelectedTeams.Split(',').ToList())
                {
                    tournament.TournamentEntries.Add(new TournamentEntry() {
                        TeamId = int.Parse(selectedTeamId),
                    });
                }

                foreach (var selectedPrizeId in model.SelectedPrizes.Split(',').ToList())
                {
                    tournament.TournamentPrizes.Add(new TournamentPrize()
                    {
                        PrizeId = int.Parse(selectedPrizeId)
                    });
                }

                // wire our matchups
                TournamentLogic.CreateRounds(tournament);

                var x = _dataConnection.CreateTournament(tournament);

                TempData["success"] = "Tournament successfully created!";

                //TODO maybe redirect to Tournament dashboard
                return RedirectToAction("Create", "Tournament");
            }
            catch (Exception ex)
            {
                var x = ex.Message;
                Console.Write(ex.Message);
                ModelState.AddModelError("Invalid Tournament", "The tournament you entered has imcomplete data.");

                return View();
            }

            ModelState.AddModelError("Invalid Tournament", "The tournament you entered has imcomplete data.");

            return View();
        }



        private List<TeamViewModel> ConvertToTeamViewModel(List<TeamDB> teamDBs)
        {
            List<TeamViewModel> output = new List<TeamViewModel>();

            foreach (var team in teamDBs)
            {
                TeamViewModel t = new TeamViewModel();
                t.Id = team.Id;
                t.TeamName = team.TeamName;

                output.Add(t);
            }

            return output;
        }

        private List<PrizeViewModel> ConvertToPrizeViewModel(List<Prize> prizes)
        {
            List<PrizeViewModel> output = new List<PrizeViewModel>();

            foreach (var prize in prizes)
            {
                PrizeViewModel t = new PrizeViewModel();
                t.Id = prize.Id;
                t.PlaceNumber = prize.PlaceNumber;
                t.PlaceName = prize.PlaceName;
                t.PrizeAmount = prize.PrizeAmount;
                t.PrizePercentage = prize.PrizePercentage;

                output.Add(t);
            }

            return output;
        }

        private TournamentViewModel InitializeModel()
        {
            TournamentViewModel tvm = new TournamentViewModel();
            tvm.EntryFee = 0;

            tvm.AvailableTeamsForTourny = ConvertToTeamViewModel(_dataConnection.GetTeam_All()).ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = $"{ a.TeamName }",
                    Value = a.Id.ToString(),
                    Selected = false
                };
            });

            tvm.AvailablePrizesForTourny = ConvertToPrizeViewModel(_dataConnection.GetPrizes_All()).ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = $"{ a.PlaceName }",
                    Value = a.Id.ToString(),
                    Selected = false
                };
            });

            return tvm;
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

