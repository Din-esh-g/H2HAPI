using NewProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewProjectAPI.Repo
{
  public interface IAuthRepo
  {
    Task<Users> Register(Users user, string password);
    Task<Users> Login(string username, string password);
    Task<bool> UserExist(string username);

  }
}
