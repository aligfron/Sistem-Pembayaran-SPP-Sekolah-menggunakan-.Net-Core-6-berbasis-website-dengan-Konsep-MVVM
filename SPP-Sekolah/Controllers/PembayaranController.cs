using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SelectPdf;
using SPP_Sekolah.AddOns;
using SPP_Sekolah.Models;
using System.Drawing;
using System.Net;
using ViewModel;

namespace SPP_Sekolah.Controllers
{
    public class PembayaranController : Controller
    {
        private readonly KelasModel kelas;
        private readonly JurusanModel jurusan;
        private readonly SiswaModel siswa;
        private readonly PembayaranModel pembayaran;
        private readonly ProfilSekolahModel profil;
        private readonly int pageZise;
        private int? custId = null;
        private int? roleId = null;

        public PembayaranController(IConfiguration _config)
        {
            kelas = new KelasModel(_config);
            jurusan = new JurusanModel(_config);
            siswa = new SiswaModel(_config);
            pembayaran = new PembayaranModel(_config);
            profil = new ProfilSekolahModel(_config);
            pageZise = int.Parse(_config["PageSize"]);
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
            ViewData["ActivePage"] = "Bayar SPP";
            ViewBag.Title = "Student Index";
            ViewBag.kelasvalue = kelasId;
            ViewBag.jurusanvalue = jurusanId;
            ViewBag.filter = filter;
            ViewBag.PageSize = currPageSize ?? pageZise;

            return View(data);
        }
        public async Task<IActionResult> Bayar(int id)
        {
            List<VMTbTPembayaran>? dataPembayaran = new List<VMTbTPembayaran>();
            VMTbMSiswa? data = await siswa.getById(id);
            dataPembayaran = await pembayaran.getByIdSiswa(id);
            VMTbMJurusan? datajurusan = null;
            VMTbMKela? datakelas = null;
            datakelas = await kelas.getById(data!.KelasId!.Value);
            datajurusan = await jurusan.getById(data!.JurusanId!.Value);
            string lastMonth = dataPembayaran?.OrderBy(p => p.Bulan).LastOrDefault()?.Bulan;

            // Buat daftar bulan mulai dari bulan setelah bulan terakhir hingga akhir tahun
            List<string> nextMonths = GetNextMonths(lastMonth);
            ViewBag.NextMonths = nextMonths;
            ViewBag.Siswa = data;
            ViewBag.Kelas = datakelas;
            ViewBag.Jurusan = datajurusan;
            ViewBag.Title = "Bayar Spp";
            ViewData["ActivePage"] = "Bayar SPP";

            return View(dataPembayaran);
        }
        public async Task<IActionResult> Create(int id)
        {
            List<VMTbTPembayaran>? dataPembayaran = new List<VMTbTPembayaran>();
            VMTbSekolah? profilskl = new VMTbSekolah();
            VMTbMSiswa? data = await siswa.getById(id);
            profilskl = await profil.GetAll();
            dataPembayaran = await pembayaran.getByIdSiswa(id);

            ViewBag.Title = "New Payment";

            // Tentukan bulan terakhir, atau null jika dataPembayaran kosong
            string lastMonth = dataPembayaran?.OrderBy(p => p.Bulan).FirstOrDefault()?.Bulan;

            // Buat daftar bulan mulai dari bulan setelah bulan terakhir hingga akhir tahun
            List<string> nextMonths = GetNextMonths(lastMonth);

            int? kelasid = data?.KelasId;
            int? siswaid = data?.Id;
            ViewBag.JumlahSpp = profilskl.BiayaSpp;
            ViewBag.KelasId = kelasid;
            ViewBag.SiswaId = siswaid;
            ViewBag.NextMonths = nextMonths;

            return View();
        }

        private List<string> GetNextMonths(string? lastMonth)
        {
            List<string> months = new List<string>
    { "Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "November", "Desember" };

            // Jika lastMonth null atau tidak ditemukan, kembalikan semua bulan
            if (string.IsNullOrEmpty(lastMonth) || !months.Contains(lastMonth))
            {
                return months;
            }

            // Cari posisi bulan terakhir yang ada
            int lastMonthIndex = months.IndexOf(lastMonth);

            // Mulai dari bulan berikutnya setelah bulan terakhir
            return months.Skip(lastMonthIndex + 1).ToList();
        }

        [HttpPost]
        public async Task<VMResponse<VMTbTPembayaran>?> CreateAsync(VMTbTPembayaran data)
        {
            VMResponse<VMTbTPembayaran>? response = null;

            try
            {
                data.CreatedBy = int.Parse(HttpContext.Session.GetString("userId")!);
                response = await pembayaran.CreateAsync(data);
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


        public async Task<IActionResult> PrescriptionPdf(int id)
        {
            VMTbTPembayaran? data = new VMTbTPembayaran();
            try
            {
                 data = await pembayaran.getById(id);

                // Set up the PDF converter with Blink rendering engine
                HtmlToPdf converter = new HtmlToPdf
                {
                    Options =
                    {
                        RenderingEngine = RenderingEngine.Blink,
                        PdfPageSize = PdfPageSize.Custom,
                        PdfPageOrientation = PdfPageOrientation.Portrait,
                        WebPageWidth = 302, // 80mm in pixels
                        AutoFitWidth = HtmlToPdfPageFitMode.ShrinkOnly,
                    },
                };

                // Set the custom page size dynamically based on prescription count
                float pageWidth = 130 * 2.83465f; // 80mm converted to points
                float baseHeight = 75 * 2.83465f; // Base height for 1 prescription (77mm)
                float additionalHeight = (94 - 75) * 2.83465f; // Additional height per prescription

                int numPrescriptions = 6;
                float pageHeight = baseHeight + (additionalHeight * Math.Max(0, numPrescriptions - 1));

                converter.Options.PdfPageCustomSize = new SizeF(pageWidth, pageHeight);

                // Render the HTML content
                string htmlContent = await Render.ViewToStringAsync(
                    this,
                    "PrescriptionPdf",
                    data,
                    true
                );

                // Convert the HTML content to PDF
                PdfDocument doc = converter.ConvertHtmlString(htmlContent);

                // Save the PDF to a byte array
                byte[] pdf = doc.Save();

                // Close the PDF document
                doc.Close();

                // Return the PDF file as a download
                return File(pdf, "application/pdf", "cetakpembayaran.pdf");
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while generating the PDF."
                );
            }
        }

        
    }
}
