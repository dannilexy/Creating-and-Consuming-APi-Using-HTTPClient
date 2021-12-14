using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class NationalParkController : Controller
    {
        private readonly INationalParkRepository _national;
        public NationalParkController(INationalParkRepository _national)
        {
            this._national = _national;
        }
        public IActionResult Index()
        {
            return View(new NationalPark() { });
        }

        public async Task<IActionResult> GetAllNationalPark()
        {
            return Json(new { data = await _national.GetAllAsync(SD.NationalParkApiPath) });
        }
    }
}
