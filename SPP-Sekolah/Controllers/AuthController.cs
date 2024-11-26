using Microsoft.AspNetCore.Mvc;
using SPP_Sekolah.Models;
using ViewModel;

namespace SPP_Sekolah.Controllers
{
    public class AuthController : Controller
    {
        private readonly ProfilSekolahModel profilSekolah;
        private AuthModel user;
        private string? _custId;

        public AuthController(IConfiguration _config)
        {
            user = new AuthModel(_config); 
            profilSekolah = new ProfilSekolahModel(_config);
        }
        public IActionResult Index()
        {
            ViewBag.Title = "Login Page";
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Regis()
        {
            return View();
        }
        private bool inSession()
        {
            _custId = HttpContext.Session.GetString("userId");
            return !string.IsNullOrEmpty(_custId);
        }
        [HttpPost]
        public async Task<IActionResult> Login(VMTbMUser data)
        {
            VMTbSekolah dataSekolah = new VMTbSekolah();
            try
            {
                
                VMTbMUser? dataApi = await user.GetByEmail(data.Email!);
                if (dataApi != null)
                {
                    if (data.Password == dataApi.Password)
                    {
                        long value = dataApi.Id;
                        HttpContext.Session.SetString("userId", value.ToString());
                        HttpContext.Session.SetString("userEmail", dataApi.Email!);
                        HttpContext.Session.SetString("userName", dataApi.Fullname!);
                        HttpContext.Session.SetInt32("userRoleId", (int)dataApi.RoleId!);
                        HttpContext.Session.SetInt32("userSiswaId", (int)dataApi.SiswaId!);
                        HttpContext.Session.SetString("userRole", dataApi.RoleName!);
                        TempData["AlertMessage"] = "Login successful!";
                        TempData["AlertType"] = "success";
                        dataSekolah = await profilSekolah.GetAll();
                        
                        if (dataSekolah == null && dataApi.RoleId == 1)
                        {
                            TempData["sekolah"] = "kosong";
                            TempData["profil"] = "Sekolah data tidak ditemukan.";
                            TempData["AlertType"] = "warning";
                            return RedirectToAction("Create", "ProfilSekolah");
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["AlertMessage"] = "Invalid Password";
                        TempData["AlertType"] = "error";
                        throw new Exception("Invalid Password");
                    }
                }
                else
                {
                    TempData["AlertMessage"] = "Email not registered!";
                    TempData["AlertType"] = "error";
                    throw new Exception("Email not registered!");
                }
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = ex.Message;
                TempData["AlertType"] = "error";
            }            
                return RedirectToAction("Index", "Home");            
            
        }
        public IActionResult Logout()
        {
            TempData["AlertMessage"] = "Logout successful!";
            TempData["AlertType"] = "success";
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
