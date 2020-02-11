using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewProjectAPI.Repo;

namespace NewProjectAPI.Controllers
{
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

  }
}
