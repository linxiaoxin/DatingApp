using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController: ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public AdminController(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("usersWithRoles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var userList = await (from user in _context.Users orderby user.UserName
                                    select new {
                                        Id = user.Id,
                                        UserName = user.UserName,
                                        Roles = (from userRole in _context.UserRoles 
                                                    join role in _context.Roles
                                                        on userRole.RoleId equals role.Id 
                                                    where userRole.UserId == user.Id
                                                        select role.Name
                                                        ).ToList()
                                    }).ToListAsync();

            return Ok(userList);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("editRoles/{userName}")]
        public async Task<IActionResult> EditRoles(string userName, RoleEditDto roleEditDto)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var userRole = await _userManager.GetRolesAsync(user);

            var selectedRoles = roleEditDto.rolesName?? new string[] {};

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRole));
            if(!result.Succeeded)
                BadRequest("Fail to add user roles.");

            result = await _userManager.RemoveFromRolesAsync(user, userRole.Except(selectedRoles));

            if(!result.Succeeded)
                BadRequest("Fail to remove user role");

            return Ok(await _userManager.GetRolesAsync(user));
        }   

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photosForModeration")]
        public IActionResult GetPhotosForModeration()
        {
            return Ok("Admin or Moderator can see this");
        }
    }
}