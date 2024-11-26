using Microsoft.AspNetCore.Mvc;
using SPP_Sekolah.AddOns;
using SPP_Sekolah.Models;
using System.Net;
using ViewModel;

namespace SPP_Sekolah.Controllers
{
    public class SiswaController : Controller
    {
        private readonly KelasModel kelas;
        private readonly JurusanModel jurusan;
        private readonly SiswaModel siswa;
        private readonly int pageZise;
        private int? custId = null;
        private int? roleId = null;

        public SiswaController(IConfiguration _config)
        {
            kelas = new KelasModel(_config);
            jurusan = new JurusanModel(_config);
            siswa = new SiswaModel(_config);
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
        public async Task<IActionResult> Index(string? filter, int? pageNumber, int? currPageSize, int? jurusanId, int? kelasId)
        {
            List<VMTbMSiswa>? data = new List<VMTbMSiswa>();
            try
            {
                // Jika `jurusan` dan `kelas` memiliki nilai, gunakan metode filter
                if (jurusanId.HasValue && kelasId.HasValue)
                {
                    // Jika `filter` null atau kosong, gunakan metode GetByJurusandanKelas tanpa filter
                    data = await siswa.GetByJurusandanKelas(jurusanId.Value, kelasId.Value, filter);
                }
                else
                {
                    // Jika `jurusan` dan `kelas` tidak ditentukan, gunakan metode getByFilter
                    data = null;
                }
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
            }
            List<VMTbMJurusan>? datajurusan = new List<VMTbMJurusan>();
            List<VMTbMKela>? datakelas = new List<VMTbMKela>();
            datakelas = await kelas.GetByJurusanId(jurusanId);
            datajurusan = await jurusan.getByFilter("");
            ViewBag.Kelas = datakelas;
            ViewBag.Jurusan = datajurusan;
            ViewBag.Title = "Student Index";
            ViewBag.kelasvalue = kelasId;
            ViewBag.jurusanvalue = jurusanId;
            ViewBag.filter = filter;
            ViewBag.PageSize = currPageSize ?? pageZise;

            return View(Pagination<VMTbMSiswa>.Create(data ?? new List<VMTbMSiswa>(), pageNumber ?? 1, ViewBag.PageSize));
        }

        public async Task<IActionResult> Delete(int id)
        {
            VMTbMSiswa? data = await siswa.getById(id);
            ViewBag.Title = "Delete Class";
            ViewBag.Nama = data!.Fullname;
            ViewData["userId"] = HttpContext.Session.GetString("userId");
            return View(data!.BiodataId);
        }
        [HttpPost]
        public async Task<VMResponse<VMTbMBiodatum>?> DeleteAsync(int id, int userId)
        {
            return (await siswa.DeleteAsync(id, userId));
        }

        public async Task<IActionResult> Details(int id)
        {
            VMTbMSiswa? data = await siswa.getById(id);
            ViewBag.Title = "Major Detail";
            return View(data);
        }
        public async Task<IActionResult> Edit(int id)
        {
            List<VMTbMJurusan>? datajurusan = new List<VMTbMJurusan>();
            List<VMTbMKela>? datakelas = new List<VMTbMKela>();
            datajurusan = await jurusan.getByFilter("");
            ViewBag.Jurusan = datajurusan;
            VMTbMSiswa? data = await siswa.getById(id);
            datakelas = await kelas.GetByJurusanId(data.JurusanId);

            ViewBag.Kelas = datakelas;
            ViewBag.Title = "Student Edit";
            return View(data);
        }
        [HttpPost]
        public async Task<VMResponse<VMTbMUser>?> EditAsync(VMTbMUser data)
        {
            VMResponse<VMTbMUser>? response = null;

            try
            {
                data.ModifiedBy = int.Parse(HttpContext.Session.GetString("userId")!);
                response = await siswa.UpdateAsync(data);
                if (response!.StatusCode == HttpStatusCode.OK)
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
        public async Task<VMResponse<List<VMTbMKela>>?> GetByJurusanId(int? jurusanId)
        {
            VMResponse<List<VMTbMKela>>? response = null;

            try
            {
                // Mengambil data kelas berdasarkan jurusanId
                List<VMTbMKela> kelasData = await kelas.GetByJurusanId(jurusanId);

                // Membuat response jika data ditemukan
                response = new VMResponse<List<VMTbMKela>>()
                {
                    Data = kelasData,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Data successfully retrieved"
                };

                // Jika data tidak ditemukan, set pesan error
                if (kelasData == null || kelasData.Count == 0)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Data not found for the given JurusanId";
                }
            }
            catch (Exception ex)
            {
                // Jika terjadi exception, set error message
                response = new VMResponse<List<VMTbMKela>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = ex.Message
                };
            }

            return response;
        }


        public async Task<IActionResult> Create()
        {
            List<VMTbMJurusan>? datajurusan = new List<VMTbMJurusan>();
            List<VMTbMKela>? datakelas = new List<VMTbMKela>();
            datakelas = await kelas.getByFilter("");
            datajurusan = await jurusan.getByFilter("");
            ViewBag.Kelas = datakelas;
            ViewBag.Jurusan = datajurusan;
            ViewBag.Title = "New Student";

            return View();
        }
        [HttpPost]
        public async Task<VMResponse<VMTbMUser>?> CreateAsync(VMTbMUser data)
        {
            VMResponse<VMTbMUser>? response = null;

            try
            {
                data.CreatedBy = int.Parse(HttpContext.Session.GetString("userId")!);
                response = await siswa.CreateAsync(data);
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
