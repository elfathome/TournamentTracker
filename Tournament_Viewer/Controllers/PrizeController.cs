using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DataAccess.DBModels;
using DataAccess.DataContext;
using Tracker_UI.Models;

namespace Tracker_UI.Controllers
{
    public class PrizeController : Controller
    {
        private readonly ILogger<PrizeController> _logger;

        private readonly IDataConnection _dataConnection;

        public PrizeController(ILogger<PrizeController> logger, IDataConnection dataConn)
        {
            _logger = logger;
            _dataConnection = dataConn;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PrizeViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isValid = ValidateForm(model);
                if (isValid)
                {
                    var prize = new Prize()
                    {
                        PrizeAmount = (decimal)model.PrizeAmount,
                        PrizePercentage = (double)model.PrizePercentage,
                        PlaceNumber = model.PlaceNumber,
                        PlaceName = model.PlaceName
                    };

                    _dataConnection.CreatePrize(prize);

                    TempData["success"] = "Prize successfully created!";
                    return RedirectToAction("Create", "Tournament");
                }

                ModelState.AddModelError("Invalid Prize", "The Prize entered has imcomplete data.");

                return View();
            }
           
            return View();
        }


        private bool ValidateForm(PrizeViewModel model)
        {
            bool output = true;
            int placeNumber = 0;
            decimal prizeAmount = 0;
            double prizePercentage = 0;
            bool prizeAmountValid = decimal.TryParse(model.PrizeAmount.ToString(), out prizeAmount);
            bool prizePercentageValid = double.TryParse(model.PrizePercentage.ToString(), out prizePercentage);

            if (model == null)
            {
                output = false;
            }
            else
            {

                if (model.PlaceName != null)
                {
                    if (model.PlaceName.Length == 0)
                    {
                        output = false;
                    }
                }
                else
                {
                    output = false;
                }

                bool placeNumberValidNumber = int.TryParse(model.PlaceNumber.ToString(), out placeNumber);

                if (!placeNumberValidNumber)
                {
                    output = false;
                }

                if (model.PlaceNumber < 1)
                {
                    output = false;
                }

                if (prizeAmountValid == false || prizePercentageValid == false)
                {
                    output = false;
                }

                if (prizeAmount <= 0 && prizePercentage <= 0)
                {
                    
                    ModelState.AddModelError("PrizeAmount", "The Prize Amount and Prize Percentage cannot both be 0.");
                    
                    output = false;
                }

                if (prizePercentage < 0 || prizePercentage > 100)
                {
                    output = false;
                }
            }

            return output;
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}

