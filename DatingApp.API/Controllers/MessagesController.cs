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
    // [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public MessagesController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet("{id}", Name="GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id) {
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) ) {
                return Unauthorized();
            }
            var message = await _repo.GetMessage(id);

            if(message == null)
                return NotFound();

            return Ok(message);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageForCreationDto messageForCreationDto) 
        {
            var sender = await _repo.GetUser(userId);

            if(sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) ) {
                return Unauthorized();
            } 

            messageForCreationDto.SenderId = userId;
            // this recipient object would be in memory and used by auto mapper
            var recipient = await _repo.GetUser(messageForCreationDto.RecipientId);

            if(recipient == null)
                return BadRequest("Could not find user");

            var message = _mapper.Map<Message>(messageForCreationDto);

            _repo.Add<Message>(message);

            if(await _repo.SaveAll()) {
                return CreatedAtRoute("GetMessage", new {id = message.Id}, _mapper.Map<MessageToReturnDto>(message));
            }
            throw new Exception("Creating message failed on save");
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId, [FromQuery]MsgParams msgParams)
        {
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) ) {
                return Unauthorized();
            } 
            msgParams.UserId = userId;
            var msgs = await _repo.GetMessagesForUser(msgParams);

            var messages =  _mapper.Map<IEnumerable<MessageToReturnDto>>(msgs);

            Response.AddPagination(msgs.CurrentPage, msgs.PageSize, msgs.TotalCount, msgs.TotalPages);

            return Ok(messages);
        }

        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int userId, int recipientId) 
        {
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) ) {
                return Unauthorized();
            } 

            var messages = await _repo.GetMessageThread(userId, recipientId);

            var msgThread = _mapper.Map<IEnumerable<MessageToReturnDto>>(messages);

            return Ok(msgThread);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int Id, int userId) {
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) ) {
                return Unauthorized();
            }

            var msgFromRepo = await _repo.GetMessage(Id);

            if(msgFromRepo.SenderId == userId)
                msgFromRepo.senderDelete = true; 
            if(msgFromRepo.RecipientId == userId)
                msgFromRepo.recipientDelete = true; 
            
            if(msgFromRepo.recipientDelete && msgFromRepo.senderDelete)
                _repo.Delete(msgFromRepo);
            if(await _repo.SaveAll())
                return NoContent();
            throw new Exception("Error deleting message");
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int id, int userId) {
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) ) {
                return Unauthorized();
            }
            var msgFromRepo = await _repo.GetMessage(id);
            if(msgFromRepo.RecipientId != userId)
                return Unauthorized();
            
            msgFromRepo.isRead = true;
            msgFromRepo.dateRead = DateTime.Now;

            if(await _repo.SaveAll())
                return NoContent();
            throw new Exception("Error Marking message as read");
        }
    }
}