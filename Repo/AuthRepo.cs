using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewProjectAPI.Data;
using NewProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewProjectAPI.Repo
{
  public class AuthRepo : IAuthRepo
  {
    private readonly NewProjectAPIContext _context;
    public AuthRepo(NewProjectAPIContext context)
    {
      _context = context;
    }

    public async  Task<Users> Login(string username, string password)
    {
      var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Username == username);
      if (user == null)
        return null;
      if (!VerifyHashPassword(user.HashPassword, user.PasswordSalt, password)) 
      return null;

      return user;
    }

    private bool VerifyHashPassword(byte[] hashPassword, byte[] passwordSalt, string password)
    {
      using (var hvac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
      {

        var newhas = hvac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

        for (int i = 0; i < newhas.Length; i++)
        {
          if (hashPassword[i] != newhas[i])
          {
            return false;
          }

        }

      }
      return true;
    }

    public async Task<Users> Register(Users user, string password)
    {
      byte[] PasswordHash, PasswordSalt;

      CreatePasswordHash(password, out PasswordHash, out PasswordSalt);

      user.HashPassword = PasswordHash;
      user.PasswordSalt = PasswordSalt;

      await _context.Users.AddAsync(user);
      await _context.SaveChangesAsync();
      return user;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
      using(var hvac = new System.Security.Cryptography.HMACSHA512())
      {
        passwordSalt = hvac.Key;
        passwordHash = hvac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
      }
    }

    public async  Task<bool> UserExist(string username)
    {
      if (await _context.Users.AnyAsync(x => x.Username == username))
        return true;
      return false;
    }
  }
}
