using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.IRepository;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    [Authorize]
    public class TrailsController : Controller
    {
        private readonly INationalParkRepository _national;
        private readonly ITrailRepository _trailRepo;
        public TrailsController(INationalParkRepository _national, ITrailRepository _trailRepo)
        {
            this._trailRepo = _trailRepo;
            this._national = _national;
        }
        public IActionResult Index()
        {
            return View(new Trail() { });
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpSert(int? id)
        {
            IEnumerable<NationalPark> npList = await _national.GetAllAsync(SD.NationalParkApiPath, HttpContext.Session.GetString("JWToken"));

            TrailsVM trailsVM = new TrailsVM()
            {
                NationalParkList = npList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Trail = new Trail()
            };

            if (id==null)
            {
                return View(trailsVM);
            }
            trailsVM.Trail = await _trailRepo.GetAsync(SD.TrailApiPath + "GetTrail/", id.Value, HttpContext.Session.GetString("JWToken"));
            if (trailsVM.Trail == null)
            {
                NotFound();
            }
            return View(trailsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpSert(TrailsVM obj)
        {
            if (ModelState.IsValid)
            {
               
                if (obj.Trail.Id == 0)
                {
                    await _trailRepo.CreateAsync(SD.TrailApiPath + "CreateTrail", obj.Trail, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _trailRepo.UpdateAsync(SD.TrailApiPath + "UpdateTrail/" + obj.Trail.Id, obj.Trail, HttpContext.Session.GetString("JWToken"));
                }
                return RedirectToAction(nameof(Index));
            }
            IEnumerable<NationalPark> npList = await _national.GetAllAsync(SD.NationalParkApiPath, HttpContext.Session.GetString("JWToken"));

            TrailsVM trailsVM = new TrailsVM()
            {
                NationalParkList = npList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Trail = new Trail()
            };
            return View(trailsVM);
        }

        public async Task<IActionResult> GetAllTrail()
        {
            return Json(new { data = await _trailRepo.GetAllAsync(SD.TrailApiPath, HttpContext.Session.GetString("JWToken")) });
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepo.DeleteAsync(SD.TrailApiPath + "DeleteTrail/", id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }
    }
}
