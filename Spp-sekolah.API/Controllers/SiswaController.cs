using DataAccess;
using DataModel;
using Microsoft.AspNetCore.Mvc;
using ViewModel;

namespace Spp_sekolah.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiswaController : ControllerBase
    {
        public DASiswa siswa;
        public SiswaController(db_sppContext _db)
        {
            siswa = new DASiswa(_db);
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                VMResponse<List<VMTbMSiswa>> response = await Task.Run(() => siswa.GetByFilter(""));
                if (response.Data.Count > 0)
                {
                    return Ok(response);
                }
                else
                {
                    Console.WriteLine(response.Message);
                    return NoContent();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("siswaController.GetAll: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{filter?}")]
        public async Task<ActionResult> GetBy(string? filter)
        {
            try
            {
                return (filter != null)
                    ? Ok(await Task.Run(() => siswa.GetByFilter(filter)))
                    : BadRequest("Student name or description must be.... ");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                VMResponse<VMTbMSiswa?> response = await Task.Run(() => siswa.GetById(id));
                if (response.Data != null)
                {
                    return Ok(response);
                }
                else
                {
                    Console.WriteLine(response.Message);
                    return NoContent();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("MajorController.GetBy " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{jurusanId}/{kelasId}/{filter?}")]
        public async Task<ActionResult> GetByJurusandanKelas(int jurusanId, int kelasId, string? filter)
        {
            try
            {
                // Sesuaikan tipe data response untuk menerima List<VMTbMSiswa>
                VMResponse<List<VMTbMSiswa>> response = await Task.Run(() => siswa.GetByJurusandanKelas(jurusanId, kelasId, filter));

                if (response.Data != null && response.Data.Count > 0)
                {
                    return Ok(response);
                }
                else
                {
                    Console.WriteLine(response.Message);
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("MajorController.GetBy " + ex.Message);
                return BadRequest(new { message = "An error occurred while processing the request", error = ex.Message });
            }
        }
        [HttpGet("[action]/{nis}")]
        public async Task<ActionResult> GetByNis(string nis)
        {
            try
            {
                VMResponse<VMTbMSiswa?> response = await Task.Run(() => siswa.GetByNis(nis));
                if (response.Data != null)
                {
                    return Ok(response);
                }
                else
                {
                    Console.WriteLine(response.Message);
                    return NoContent();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("MajorController.GetBy " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Create(VMTbMUser data)
        {
            try
            {
                return Created("api/user", await Task.Run(() => siswa.Create(data)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("MajorController.Create " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(VMTbMUser data)
        {
            try
            {
                VMResponse<VMTbMUser?> response = await Task.Run(() => siswa.Update(data));
                if (response.Data != null)
                {
                    return Ok(response);
                }
                else
                {
                    Console.WriteLine(response.Message);
                    return NoContent();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("MajorController.Update " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id?}/{userId?}")]
        public async Task<ActionResult> Delete(int id, int userId)
        {
            try
            {
                VMResponse<VMTbMBiodatum> response = await Task.Run(() => siswa.Delete(id, userId));
                if (response.Data != null) { return Ok(response); }
                else
                {
                    Console.WriteLine(response.Message);
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("MajorController.Delete " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
