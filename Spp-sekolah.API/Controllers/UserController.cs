using DataAccess;
using DataModel;
using Microsoft.AspNetCore.Mvc;
using ViewModel;

namespace Spp_sekolah.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        public DAUser user;
        private readonly db_sppContext db;
        public UserController(db_sppContext _db)
        {
            db = _db;
            user = new DAUser(_db);
        }
        [HttpGet("[action]/{email?}")]
        public async Task<ActionResult> GetByEmail(string email)
        {
            try
            {
                VMResponse<VMTbMUser> response = await Task.Run(() => user!.GetByEmail(email));
                return (response.Data != null) ?
                    Ok(response) : throw new Exception(response.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("User Controller.GetByEmail : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("login")]
        public async Task<ActionResult> Login(VMTbMUser data)
        {
            try
            {
                VMResponse<VMTbMUser> response = await Task.Run(() => user.Login(data));
                if (response.Data != null)
                {
                    return Ok(response);
                }
                else
                {
                    Console.WriteLine("LoginController.Update: " + response.Message);
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("UserController.Update: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
