using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DataContext;
using DataAccess.DBModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tracker_UI.Models;

namespace Tracker_UI.Controllers
{
    public class TeamController : Controller
    {
        private readonly ILogger<TeamController> _logger;
        private readonly IDataConnection _dataConnection;

        public TeamController(ILogger<TeamController> logger, IDataConnection dataConn)
        {
            _logger = logger;
            _dataConnection = dataConn;
        }

        // GET: Team/Create
        public ActionResult Create()
        {
            TeamPersonViewModel tpvm = new TeamPersonViewModel();

            tpvm.Team = new TeamViewModel();

            tpvm.AvailablePeopleForTeam = ConvertToPersonViewModel(_dataConnection.GetPerson_All()).ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = $"{ a.FirstName } { a.LastName }",
                    Value = a.Id.ToString(),
                    Selected = false
                };
            });

            return View(tpvm);
        }

        // POST: Team/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TeamPersonViewModel model)
        {
            try
            {
                var team = new TeamDB()
                {
                    TeamName = model.Team.TeamName,
                    TeamMembers = new List<TeamMember>()
                };

                foreach (var selectedId in model.SelectedPeople.Split(',').ToList())
                {
                    var t = selectedId;
                    team.TeamMembers.Add(new TeamMember() { PersonId = int.Parse(selectedId)});
                }

                var x = _dataConnection.CreateTeam(team);

                TempData["success"] = "Team successfully created!";

                //TODO maybe redirect to Tournament dashboard
                return RedirectToAction("Create", "Team");
            }
            catch
            {
                ModelState.AddModelError("Invalid Team", "The team you entered has imcomplete data.");

                return View();
            }

            ModelState.AddModelError("Invalid Team", "The team you entered has imcomplete data.");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePerson(TeamPersonViewModel model)
        {
            if (model.Person != null)
            {   
                var person = new Person()
                {
                    FirstName = model.Person.FirstName,
                    LastName = model.Person.LastName,
                    EmailAddress = model.Person.EmailAddress,
                    CellphoneNumber = model.Person.CellphoneNumber
                };

                _dataConnection.CreatePerson(person);

                //TODO this toastr isn't working.
                TempData["success"] = "Member successfully created!";

                return RedirectToAction("Create", "Team");
            }

            ModelState.AddModelError("Invalid Member", "The member you entered has imcomplete data.");

            return View();
        }

        public List<PersonViewModel> ConvertToPersonViewModel(List<Person> people)
        {
            List<PersonViewModel> output = new List<PersonViewModel>();

            foreach (var person in people)
            {
                PersonViewModel p = new PersonViewModel();
                p.Id = person.Id;
                p.FirstName = person.FirstName;
                p.LastName = person.LastName;
                p.EmailAddress = person.EmailAddress;
                p.CellphoneNumber = person.CellphoneNumber;
                p.FullName = $"{ person.FirstName } { person.LastName }";

                output.Add(p);
            }
  
            return output;
        }
    }
}