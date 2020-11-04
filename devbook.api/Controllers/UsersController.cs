using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using devbook.api.Data;
using devbook.api.dtos;
using devbook.api.Helpers;
using devbook.api.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace devbook.api.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IDeveloperRepository _repo;

         private readonly IMapper _mapper;
        public UsersController(IDeveloperRepository repo, IMapper mapper)
        {
            _repo = repo;
             _mapper = mapper;
          
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {


            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser(currentUserId);

            userParams.UserId = currentUserId;





            var users = await _repo.GetUsers(userParams);

             var userToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

                  Response.AddPagination(users.CurrentPage, users.PageSize,
                users.TotalCount, users.TotalPages);

            return Ok(userToReturn);
        }

       [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        } 


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(id);

            _mapper.Map(userForUpdateDto, userFromRepo);

            if(await _repo.SaveAll())
                return NoContent();

            throw new Exception($"Updating user {id} failed on save");
        }


        [HttpDelete("skill/{id}")]
         public async Task<IActionResult> DeleteSkill(int id)
         {
            var skillFromRepo = await _repo.GetSkill(id);
            if(skillFromRepo.Id != null){
                 _repo.Delete(skillFromRepo);
            }
            if( await _repo.SaveAll()){
                return Ok();
            }
            return BadRequest("Failed to delete Skill");
         } 


        [HttpPost("skill")]
        public async Task<IActionResult> addSkill(SkillForAdditionDTO skill)
        {
            var skillToCreate = new Skills{
                 UserId = skill.UserId,
                 Skill = skill.Skill
                 };
             var createdUser = await _repo.addSkill(skillToCreate);

            return StatusCode(204);
        

         

            
        }




 
    }
}