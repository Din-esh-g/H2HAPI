using NewProjectAPI.Helpers;
using NewProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewProjectAPI.Repo
{
  public interface IDatingRepo
  {
    void Add<T>(T entity) where T : class;
    void Delete<T>(T entity) where T : class;
    Task<bool> SaveAll();
    //Task<IEnumerable<Users>> GetUsers();
    //This is for paginations
    Task<PagedList<Users>> GetUsers(UserParams userParams);
    Task<Users> GetUser(int id);
    Task <Photo>GetPhoto(int id);
    Task<Photo> GetMainPhotoForUser(int userId);

  }
}
