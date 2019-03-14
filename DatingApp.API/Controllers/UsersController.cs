using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTO;
using DatingApp.API.Helper;
using DatingApp.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController: ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name="GetUser")]
        public async Task<IActionResult> GetUser(int id){
           var user = await _repo.GetUser(id);
           var userDetails = _mapper.Map<UserForDetailDTO>(user);
           return Ok(userDetails);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams){
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser(currentUserId);
            userParams.UserId = currentUserId;
            if(userParams.Gender == null)
                userParams.Gender = userFromRepo.gender == "male" ?"female": "male";
            var users = await _repo.GetUsers(userParams);
            var userLists = _mapper.Map<IEnumerable<UserForListDTO>>(users);
            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(userLists);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto) {
            if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) ) {
                return Unauthorized();
            }

            var userFromRepo = await _repo.GetUser(id);

            _mapper.Map(userForUpdateDto, userFromRepo);

            if(await _repo.SaveAll())
               return NoContent();

            throw new Exception($"Fail to update user id : {id}");
        }

        [HttpPost("{userId}/like/{receipientId}")]
        public async Task<IActionResult> LikeUser(int userId, int receipientId) {
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) ) {
                return Unauthorized();
            }
            var like = await _repo.GetLike(userId, receipientId);
            if(like != null)
                return BadRequest("You have already like this user");
            
            if(await _repo.GetUser(receipientId) == null)
                return NotFound();
            
            like = new Like {
                LikerId= userId,
                LikeeId= receipientId
            };

            _repo.Add<Like>(like);

            if(await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to like user");
        }
    }
}