using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewProjectAPI.Helpers;
using NewProjectAPI.Models;
using NewProjectAPI.Repo;

namespace NewProjectAPI.Controllers
{
  [ServiceFilter(typeof(LogUserActivity))]
  [Authorize]
  [Route("api/users/{userId}/[controller]")]
  [ApiController]
  public class MessagesController : ControllerBase
  {
    private readonly IDatingRepo _repo;
    private readonly IMapper _mapper;

    public MessagesController(IDatingRepo repo, IMapper mapper)
    {
      _repo = repo;
      _mapper = mapper;
    }

    [HttpGet("{id}", Name = "GetMessage")]
    public async Task<IActionResult> GetMessage(int userId, int id)
    {
      if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        return Unauthorized();

      var massageFromRepo = await _repo.GetMessage(id);

      if (massageFromRepo == null)

        return NotFound();

      return Ok(massageFromRepo);

    }

    [HttpGet]

    public async Task<IActionResult> GetMessageForUser(int userId, [FromQuery] MessageParams messageParams)
    {
      if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        return Unauthorized();

      messageParams.UserId = userId;
      var messageFromRepo = await _repo.GetMessageForUser(messageParams);

      var messages = _mapper.Map<IEnumerable<MessageToReturnDTO>>(messageFromRepo);
      Response.AddPagination(messageFromRepo.CurrentPage, messageFromRepo.PageSize, messageFromRepo.TotalCount, messageFromRepo.TotalPage);
      return Ok(messages);


    }

    [HttpGet("thread/{recipientId}")]

    public async Task <IActionResult> GetMessageThread(int userId, int recipientId)
    {
      if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        return Unauthorized();
      var messageFromRepo = await _repo.GetMessageThread(userId, recipientId);
      var messageThread = _mapper.Map<IEnumerable<MessageToReturnDTO>>(messageFromRepo);
      return Ok(messageThread);

    }




    [HttpPost]
    public async Task<IActionResult> CreateMessage(int userId, MessageForCreationDTO messageForCreationDTO)
    {
      //this line of code is added for the message photo display
      var sender = await _repo.GetUser(userId);

      if (sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        return Unauthorized();

      messageForCreationDTO.SenderId = userId;

      var recipient = await _repo.GetUser(messageForCreationDTO.RecipientId);
      if (recipient == null)
        return BadRequest("Colud not Found the user.");


      var message = _mapper.Map<Message>(messageForCreationDTO);

      _repo.Add(message);

      if(await _repo.SaveAll())
      {
        var messageToReturn = _mapper.Map<MessageToReturnDTO>(message);
        return CreatedAtRoute("GetMessage", new { userId, id = message.Id }, messageToReturn);
      }
      throw new Exception("Creating the message failed. ");

    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult>DeleteMessage(int id, int userId)
    {
      if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        return Unauthorized();

      var messageFromRepo = await _repo.GetMessage(id);

      if (messageFromRepo.SenderId == userId)
        messageFromRepo.SenderDeleted = true;
      if(messageFromRepo.RecipientId ==userId)
        messageFromRepo.RecipientDeleted = true;

      if (messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted)
        _repo.Delete(messageFromRepo);
      if (await _repo.SaveAll())
        return NoContent();
      throw new Exception("Error deleting message. ");

    }



  }
}
