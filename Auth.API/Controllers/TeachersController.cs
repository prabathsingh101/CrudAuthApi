using Auth.API.Models.Domain;
using Auth.API.Models.DTO;
using Auth.API.Repositories.Abstract;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherRepository teacherRepository;
        private readonly IMapper mapper;

        public TeachersController(ITeacherRepository teacherRepository, IMapper mapper)
        {
            this.teacherRepository = teacherRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]       
        public  async Task<IActionResult> Create([FromBody] AddTeacherRequestDto teacherRequestDto)
        {
            //convert or map dto to domain model
            var teacherModel = mapper.Map<TeacherModel>(teacherRequestDto);

            //use domain model to create region
            teacherModel = await teacherRepository.CreateAsync(teacherModel);

            //map domain model back to dto
            var teacherDto = mapper.Map<TeacherDto>(teacherModel);

            return CreatedAtAction(nameof(GetById), new { id = teacherDto.Id }, teacherDto);
        }

        //GET:/api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 1000)
        {
            //get data from database - domain models
            var teacherDomain = await teacherRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            //returns  dto
            return Ok(mapper.Map<List<TeacherDto>>(teacherDomain));
        }

        [HttpGet]
        [Route("{id:int}")]
        [Authorize(Roles = "User,Admin")]        
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            //get teacher domain from database      

            var teacherDomain = await teacherRepository.GetByIdAsync(id);
            //map domain model to dto

            if (teacherDomain == null)
            {
                return NotFound();
            }
            //convert domain model to dto
            return Ok(mapper.Map<TeacherDto>(teacherDomain));
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]       
        public async Task<IActionResult> Update([FromRoute] int id,
                                                [FromBody] UpdateTeacherRequestDto updateTeacherRequestDto)
        {
            //map dto to domain model
            var teacherDomainModel = mapper.Map<TeacherModel>(updateTeacherRequestDto);

            //check region if exists
            teacherDomainModel = await teacherRepository.UpdateAsync(id, teacherDomainModel);
            if (teacherDomainModel == null)
            {
                return NotFound();
            }

            //convert domain model to dto
            return Ok(mapper.Map<TeacherDto>(teacherDomainModel));
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]    
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            //Delete from database and back to domain
            var teacherDomainModel = await teacherRepository.DeleteAsync(id);
            if (teacherDomainModel == null)
            {
                return NotFound();
            }
            //convert domain model to dto
            return Ok(mapper.Map<TeacherDto>(teacherDomainModel));
        }
    }
}
