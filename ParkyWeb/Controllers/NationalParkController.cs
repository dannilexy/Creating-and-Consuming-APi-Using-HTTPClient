using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using System.IO;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    [Authorize]
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

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpSert(int? id)
        {
            NationalPark obj = new NationalPark();
            if (id==null)
            {
                return View(obj);
            }
            obj = await _national.GetAsync(SD.NationalParkApiPath, id.Value, HttpContext.Session.GetString("JWToken"));
            if (obj == null)
            {
                NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpSert(NationalPark obj)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                       
                    }
                    obj.Picture = p1;
                }
                else
                {
                    var ObjFromDb = await _national.GetAsync(SD.NationalParkApiPath, obj.Id, HttpContext.Session.GetString("JWToken"));
                    obj.Picture = ObjFromDb.Picture;
                }
                if (obj.Id == 0)
                {
                    await _national.CreateAsync(SD.NationalParkApiPath + "CreateNationalPark", obj, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _national.UpdateAsync(SD.NationalParkApiPath + "UpdateNationalPark/" + obj.Id, obj, HttpContext.Session.GetString("JWToken"));
                }
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public async Task<IActionResult> GetAllNationalPark()
        {
            return Json(new { data = await _national.GetAllAsync(SD.NationalParkApiPath, HttpContext.Session.GetString("JWToken")) });
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _national.DeleteAsync(SD.NationalParkApiPath + "DeleteNationalPark/", id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }
    }
}
