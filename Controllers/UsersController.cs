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
  [Route("api/[controller]")]
    [ApiController]
    
    public class UsersController : ControllerBase
    {
    private readonly IDatingRepo _repo;
    private readonly IMapper _mapper;
    public UsersController(IDatingRepo repo, IMapper mapper)
    {
      _repo = repo;
      _mapper = mapper;
    }
    //This method is not implemented pagination 
    //[HttpGet]
    //public async Task<IActionResult> GetUser()
    //{
    //  var users = await _repo.GetUsers();
    //  var userToReturn = _mapper.Map<IEnumerable<UserListDTO>>(users);
    //  return Ok(userToReturn);
    //}

    [HttpGet]
    public async Task<IActionResult> GetUser([FromQuery]UserParams userParams)
    {
      //Filter
     
      var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
      var userFromRepo = await _repo.GetUser(currentUserId);
      userParams.UserId = currentUserId;
      
     if (string.IsNullOrEmpty(userParams.Gender))
     {
       userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
     }
     
           //end filter
      var users = await _repo.GetUsers(userParams);
      var userToReturn = _mapper.Map<IEnumerable<UserListDTO>>(users);
      Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPage);
      return Ok(userToReturn);
    }


    [HttpGet("{id}", Name ="GetUser")]
    public async Task<IActionResult> GetUser(int id)
    {
      var user = await _repo.GetUser(id);
      var userToReturn = _mapper.Map<UserForDetails>(user);
      return Ok(user);
    }
    [HttpPut("{id}")]

    public async Task<IActionResult>UpdateUser(int id, UserForUpdateDTO userForUpdateDTO)
    {

      if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        return Unauthorized();

      var userFromRepo =  await _repo.GetUser(id);
      _mapper.Map(userForUpdateDTO, userFromRepo);
      if (await _repo.SaveAll())
        return NoContent();
      throw new Exception($"Updating user {id} failed on save");

    }


    [HttpPost ("{id}/like/{recipientId}")]
    public async Task<IActionResult>LikeUser(int id , int recipientId)
    {
      //Checking user is authorized or not
      if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        return Unauthorized();

      var like = await _repo.GetLike(id, recipientId);
      if(like != null)
      {
        return BadRequest("You already like this user");
      }

      if(await _repo.GetUser(recipientId) == null)
      {
        return NotFound();
      }
      like = new Models.Like
      {
        LikerId = id,
        LikeeId = recipientId

      };
      _repo.Add<Like>(like);

      if (await _repo.SaveAll())
        return Ok();

      return BadRequest("Failed to like user.");
    }


  }
}
