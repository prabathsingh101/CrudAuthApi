using Auth.API.Data;
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
        private readonly DatabaseContext databaseContext;

        public AdminController(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
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
