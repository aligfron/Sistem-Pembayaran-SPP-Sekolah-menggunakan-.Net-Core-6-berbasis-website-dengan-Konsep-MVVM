using Microsoft.AspNetCore.Mvc;
using SPP_Sekolah.Models;
using System.Net;
using ViewModel;

namespace SPP_Sekolah.Controllers
{
    public class ProfilSekolahController : Controller
    {
        private readonly ProfilSekolahModel profilSekolah;
        private string? _userId;
        private string? _roleCode;
        private readonly string imageFolder;
        public ProfilSekolahController(IConfiguration _config)
        {
            profilSekolah = new ProfilSekolahModel(_config);
        }
        public async Task<IActionResult> Index()
        {
            VMTbSekolah data = new VMTbSekolah();
            try
            {
                data =  await profilSekolah.GetAll();
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
            }
            ViewData["ActivePage"] = "ProfilSekolah";
            ViewBag.Title = "Login Page";
            return View(data);
        }
        public async Task<IActionResult> Create()
        {
            ViewData["ActivePage"] = "ProfilSekolah";
            ViewBag.Title = "Profile Sekolah Baru";
            return View();
        }
        [HttpPost]
        public async Task<VMResponse<VMTbSekolah>?> CreateAsync(VMTbSekolah data)
        {
            VMResponse<VMTbSekolah>? response = null;

            try
            {
                data.CreatedBy = long.Parse(HttpContext.Session.GetString("userId")!);
                response = await profilSekolah.CreateAsync(data);
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
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["ActivePage"] = "ProfilSekolah";
            VMTbSekolah? data = await profilSekolah.getById(id);
            ViewBag.Title = "ProfilSekolah Edit";
            return View(data);
        }
        [HttpPost]
        public async Task<VMResponse<VMTbSekolah>?> EditAsync(VMTbSekolah data)
        {
            VMResponse<VMTbSekolah>? response = null;

            try
            {
                data.ModifiedBy = long.Parse(HttpContext.Session.GetString("userId")!);
                response = await profilSekolah.UpdateAsync(data);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    TempData["AlertMessage"] = response.Message;
                    TempData["AlertType"] = "success";
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
