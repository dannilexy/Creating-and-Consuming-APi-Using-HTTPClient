using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository  _trRepo;

        public HomeController(ILogger<HomeController> logger, INationalParkRepository _npRepo, ITrailRepository _trRepo)
        {
            _logger = logger;
            this._npRepo = _npRepo;
            this._trRepo = _trRepo;
        }

        public async Task<IActionResult> Index()
        {
            IndexVM indexVM = new IndexVM()
            {
                NationalParkList = await _npRepo.GetAllAsync(SD.NationalParkApiPath),
                TrailList = await _trRepo.GetAllAsync(SD.TrailApiPath),

            };
            return View(indexVM);
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
