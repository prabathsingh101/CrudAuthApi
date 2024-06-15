using Auth.API.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public IActionResult GetData()
        {
            var status = new StatusDto();
            status.StatusCode = 1;
            status.Message = "Data from admin controller";
            return Ok(status);
        }
    }
}
