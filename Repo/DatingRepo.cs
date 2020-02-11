using Microsoft.EntityFrameworkCore;
using NewProjectAPI.Data;
using NewProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewProjectAPI.Repo
{
  public class DatingRepo : IDatingRepo
  {
    private readonly NewProjectAPIContext _context;
    public DatingRepo(NewProjectAPIContext context)
    {
      _context = context;
    }
    public void Add<T>(T entity) where T : class
    {
      _context.Add(entity);
    }

    public void Delete<T>(T entity) where T : class
    {
      _context.Remove(entity);
    }

    public async Task<Photo> GetPhoto(int id)
    {
      var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
      return photo;
    }

    public async Task<Users> GetUser(int id)
    {
      var user =await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
      return user;
    }

    public async Task<IEnumerable<Users>> GetUsers()
    {
      var users = await _context.Users.Include(p => p.Photos).ToListAsync();
      return users;
    }

    public async Task<bool> SaveAll()
    {
      return await _context.SaveChangesAsync() > 0;
    }
  }
}
