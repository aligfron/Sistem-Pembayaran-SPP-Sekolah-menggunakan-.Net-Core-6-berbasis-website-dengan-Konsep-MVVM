using DataAccess;
using DataModel;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using ViewModel;

namespace Spp_sekolah.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilSekolahController : ControllerBase
    {
        public DAProfilSekolah sekolah;
        public ProfilSekolahController(db_sppContext _db)
        {
            sekolah = new DAProfilSekolah(_db);
        }
        [HttpGet("[action]")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                VMResponse<VMTbSekolah?> response = await Task.Run(() => sekolah.GetAll());
                if (response.Data != null )
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
                // Console Logging
                Console.WriteLine("SchoolProfileController.GetAll: " + ex.Message);

                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                VMResponse<VMTbSekolah?> response = await Task.Run(() => sekolah.GetById(id));
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
                Console.WriteLine("SchoolProfileController.GetByID " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Create(VMTbSekolah data)
        {
            try
            {
                return Created("api/ProfilSekolah", await Task.Run(() => sekolah.Create(data)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("SchoolProfile.Create " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(VMTbSekolah data)
        {
            try
            {
                VMResponse<VMTbSekolah?> response = await Task.Run(() => sekolah.Update(data));
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
                Console.WriteLine("SchoolProfile.Update " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
