using Auth.API.Models.Domain;
using Auth.API.Models.DTO;
using Auth.API.Repositories.Abstract;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    //testing
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly IUserService userService;

        public UserController(UserManager<ApplicationUser> userManager, IMapper mapper,
            IUserService userService)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult GetData()
        {
            var status = new StatusDto();
            status.StatusCode = 1;
            status.Message = "Data from protected controller";
            return Ok(status);
        }
        //[HttpGet]
        //[Authorize(Roles ="Admin")]
        //[Route("GetAll")]
        //public async  Task<IActionResult> getAllUser()
        //{
        //    var model = await userManager.Users.ToListAsync();
        //    if (model == null)
        //        return BadRequest();
        //    return Ok(model);
        //}
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("getAllUser")]
        public async Task<IActionResult> getAllUser()
        {
            var model = await  userService.GetAllUsersAsync();
            if (model == null)
                return BadRequest();
            //return Ok(mapper.Map<UserModelDto>(model));
            return Ok(model);
        }
    }
}
