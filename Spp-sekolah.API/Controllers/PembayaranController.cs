using DataAccess;
using DataModel;
using Microsoft.AspNetCore.Mvc;
using ViewModel;

namespace Spp_sekolah.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PembayaranController : ControllerBase
    {
        public DAPembayaran pembayaran;
        public PembayaranController(db_sppContext _db)
        {
            pembayaran = new DAPembayaran(_db);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetByIdSiswa(int id)
        {
            try
            {
                VMResponse<List<VMTbTPembayaran?>> response = await Task.Run(() => pembayaran.GetByIdSiswa(id));
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
                Console.WriteLine("PaymentController.GetBy " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost]
        public async Task<ActionResult> Create(VMTbTPembayaran data)
        {
            try
            {
                return Created("api/Pembayaran", await Task.Run(() => pembayaran.Create(data)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("PaymentController.Create " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                VMResponse<VMTbTPembayaran?> response = await Task.Run(() => pembayaran.GetById(id));
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
    }
}
