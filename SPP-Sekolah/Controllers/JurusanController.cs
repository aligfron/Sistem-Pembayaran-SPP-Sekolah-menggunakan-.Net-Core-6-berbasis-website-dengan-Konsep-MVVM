using SPP_Sekolah.AddOns;
using Microsoft.AspNetCore.Mvc;
using SPP_Sekolah.Models;
using ViewModel;
using System.Net;

namespace SPP_Sekolah.Controllers
{
    public class JurusanController : Controller
    {
        private readonly JurusanModel jurusan;
        private readonly int pageZise;
        private int? custId = null;
        private int? roleId = null;

        public JurusanController(IConfiguration _config)
        {
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
            List<VMTbMJurusan>? data = new List<VMTbMJurusan>();
            try
            {
                data = (string.IsNullOrEmpty(filter)) ? await jurusan.getByFilter("") : await jurusan.getByFilter(filter);
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
            }

            ViewBag.Title = "Major Index";
            ViewBag.filter = filter;
            ViewBag.PageSize = (currPageSize ?? pageZise);

            return View(Pagination<VMTbMJurusan>.Create(data ?? new List<VMTbMJurusan>(), pageNumber ?? 1, ViewBag.PageSize));
        }
        public IActionResult Delete(int id)
        {

            ViewBag.Title = "Delete Customer";
            ViewData["userId"] = HttpContext.Session.GetString("userId");
            return View(id);
        }
        [HttpPost]
        public async Task<VMResponse<VMTbMJurusan>?> DeleteAsync(int id, int userId)
        {
            return (await jurusan.DeleteAsync(id, userId));
        }

        public async Task<IActionResult> Details(int id)
        {
            VMTbMJurusan? data = await jurusan.getById(id);

            ViewBag.Title = "Major Detail";
            return View(data);
        }
        public async Task<IActionResult> Edit(int id)
        {
            VMTbMJurusan? data = await jurusan.getById(id);

            ViewBag.Title = "Major Edit";
            return View(data);
        }
        [HttpPost]
        public async Task<VMResponse<VMTbMJurusan>?> EditAsync(VMTbMJurusan data)
        {
            VMResponse<VMTbMJurusan>? response = null;

            try
            {
                data.ModifiedBy = int.Parse(HttpContext.Session.GetString("userId")!);
                response = await jurusan.UpdateAsync(data);
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
        public IActionResult Create()
        {
            ViewBag.Title = "New Major";

            return View();
        }
        [HttpPost]
        public async Task<VMResponse<VMTbMJurusan>?> CreateAsync(VMTbMJurusan data)
        {
            VMResponse<VMTbMJurusan>? response = null;

            try
            {
                data.CreatedBy = int.Parse(HttpContext.Session.GetString("userId")!);
                response = await jurusan.CreateAsync(data);
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
