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

    [HttpGet]
    public async Task<IActionResult> GetUser()
    {
      var users = await _repo.GetUsers();
      var userToReturn = _mapper.Map<IEnumerable<UserListDTO>>(users);
      return Ok(userToReturn);
    }
    [HttpGet("{id}")]
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

  }
}
