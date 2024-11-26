using DataAccess;
using DataModel;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ViewModel;

namespace Spp_sekolah.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JurusanController : ControllerBase
    {
        public DAJurusan jurusan;
        public JurusanController(db_sppContext _db) 
        { 
            jurusan = new DAJurusan(_db);
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                VMResponse<List<VMTbMJurusan>> response = await Task.Run(() => jurusan.GetByFilter(""));
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
                Console.WriteLine("JurusanController.GetAll: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{filter?}")]
        public async Task<ActionResult> GetBy(string? filter)
        {
            try
            {
                return (filter != null)
                    ? Ok(await Task.Run(() => jurusan.GetByFilter(filter)))
                    : BadRequest("Major name or description must be.... ");
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
                VMResponse<VMTbMJurusan?> response = await Task.Run(() => jurusan.GetById(id));
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
        public async Task<ActionResult> Create(VMTbMJurusan data)
        {
            try
            {
                return Created("api/jurusan", await Task.Run(() => jurusan.Create(data)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("MajorController.Create " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(VMTbMJurusan data)
        {
            try
            {
                VMResponse<VMTbMJurusan?> response = await Task.Run(() => jurusan.Update(data));
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
                VMResponse<VMTbMJurusan> response = await Task.Run(() => jurusan.Delete(id, userId));
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return Ok(response);
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound(response);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("MajorController.Delete: " + ex.Message);
                return BadRequest(new { Message = ex.Message });
            }
        }

    }
}
