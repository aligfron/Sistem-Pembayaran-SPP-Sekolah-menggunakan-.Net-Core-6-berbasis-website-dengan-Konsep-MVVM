using Microsoft.AspNetCore.Mvc;
using SPP_Sekolah.AddOns;
using SPP_Sekolah.Models;
using System.Net;
using ViewModel;

namespace SPP_Sekolah.Controllers
{
    public class KelasController : Controller
    {
        private readonly KelasModel kelas;
        private readonly JurusanModel jurusan;
        private readonly int pageZise;
        private int? custId = null;
        private int? roleId = null;

        public KelasController(IConfiguration _config)
        {
            kelas = new KelasModel(_config);
            jurusan = new JurusanModel(_config);
            pageZise = int.Parse(_config["PageSize"]);
        }

        private IActionResult? CheckSession()
        {

            int? custId = HttpContext.Session.GetInt32("custId");
            int? roleId = HttpContext.Session.GetInt32("custRole");

            if (custId == null)
            {
                HttpContext.Session.SetString("errMsg", "Please Login first");
                return RedirectToAction("Index", "Home");
            }

            if (roleId != 1)
            {
                HttpContext.Session.SetString("errMsg", "you are not autor");
                return RedirectToAction("Index", "Home");
            }

            return null;
        }
        public async Task<IActionResult> Index(string? filter, int? pageNumber, int? currPageSize)
        {
            //IActionResult? sessionResult = CheckSession();
            //if (sessionResult != null)
            //{
            //    return sessionResult;
            //}
            List<VMTbMKela>? data = new List<VMTbMKela>();
            try
            {
                data = (string.IsNullOrEmpty(filter)) ? await kelas.getByFilter("") : await kelas.getByFilter(filter);
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
            }

            ViewBag.Title = "Class Index";
            ViewBag.filter = filter;
            ViewBag.PageSize = (currPageSize ?? pageZise);

            return View(Pagination<VMTbMKela>.Create(data ?? new List<VMTbMKela>(), pageNumber ?? 1, ViewBag.PageSize));
        }
        public IActionResult Delete(int id)
        {

            ViewBag.Title = "Delete Class";
            ViewData["userId"] = HttpContext.Session.GetString("userId");
            return View(id);
        }
        [HttpPost]
        public async Task<VMResponse<VMTbMKela>?> DeleteAsync(int id, int userId)
        {
            return (await kelas.DeleteAsync(id, userId));
        }

        public async Task<IActionResult> Details(int id)
        {
            VMTbMKela? data = await kelas.getById(id);
            ViewBag.Title = "Major Detail";
            return View(data);
        }
        public async Task<IActionResult> Edit(int id)
        {
            List<VMTbMJurusan>? datajurusan = new List<VMTbMJurusan>();
            datajurusan = await jurusan.getByFilter("");
            ViewBag.Jurusan = datajurusan;
            VMTbMKela? data = await kelas.getById(id);

            ViewBag.Title = "Major Edit";
            return View(data);
        }
        [HttpPost]
        public async Task<VMResponse<VMTbMKela>?> EditAsync(VMTbMKela data)
        {
            VMResponse<VMTbMKela>? response = null;

            try
            {
                data.ModifiedBy = int.Parse(HttpContext.Session.GetString("userId")!);
                response = await kelas.UpdateAsync(data);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    HttpContext.Session.SetString("successMsg", response.Message);
                }
                else
                {
                    HttpContext.Session.SetString("errMsg", response.Message);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
            }
            return (response);
        }
        public async Task<IActionResult> Create()
        {
            List<VMTbMJurusan>? datajurusan = new List<VMTbMJurusan>();
            datajurusan = await jurusan.getByFilter("");
            ViewBag.Jurusan = datajurusan;
            ViewBag.Title = "New Major";

            return View();
        }
        [HttpPost]
        public async Task<VMResponse<VMTbMKela>?> CreateAsync(VMTbMKela data)
        {
            VMResponse<VMTbMKela>? response = null;

            try
            {
                data.CreatedBy = int.Parse(HttpContext.Session.GetString("userId")!);
                response = await kelas.CreateAsync(data);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    HttpContext.Session.SetString("successMsg", response.Message);
                }
                else
                {
                    HttpContext.Session.SetString("errMsg", response.Message);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
            }
            return (response);
        }
    }
}
