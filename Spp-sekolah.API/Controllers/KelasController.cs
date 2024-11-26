using DataAccess;
using DataModel;
using Microsoft.AspNetCore.Mvc;
using ViewModel;

namespace Spp_sekolah.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KelasController : ControllerBase
    {
        public DAKelas kelas;
        public KelasController(db_sppContext _db)
        {
            kelas = new DAKelas(_db);
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                VMResponse<List<VMTbMKela>> response = await Task.Run(() => kelas.GetByFilter(""));
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
                Console.WriteLine("kelasController.GetAll: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{jurusanid?}")]
        public async Task<ActionResult> GetByJurusanId(int? jurusanid)
        {
            try
            {
                VMResponse<List<VMTbMKela>> response = await Task.Run(() => kelas.GetByJurusanId(jurusanid));
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
                Console.WriteLine("kelasController.GetAll: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{filter?}")]
        public async Task<ActionResult> GetBy(string? filter)
        {
            try
            {
                return (filter != null)
                    ? Ok(await Task.Run(() => kelas.GetByFilter(filter)))
                    : BadRequest("Class name or description must be.... ");
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
                VMResponse<VMTbMKela?> response = await Task.Run(() => kelas.GetById(id));
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
                Console.WriteLine("ClassController.GetBy " + ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id?}/{userId?}")]
        public async Task<ActionResult> Delete(int id, int userId)
        {
            try
            {
                VMResponse<VMTbMKela> response = await Task.Run(() => kelas.Delete(id, userId));
                if (response.Data != null) { return Ok(response); }
                else
                {
                    Console.WriteLine(response.Message);
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ClassController.Delete " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Create(VMTbMKela data)
        {
            try
            {
                return Created("api/Kelas", await Task.Run(() => kelas.Create(data)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("ClassController.Create " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(VMTbMKela data)
        {
            try
            {
                VMResponse<VMTbMKela?> response = await Task.Run(() => kelas.Update(data));
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
                Console.WriteLine("ClassController.Update " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
