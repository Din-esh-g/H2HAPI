using NewProjectAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NewProjectAPI.Models;

namespace NewProjectAPI.Repo
{
  public class Seed
  {
    public static void SeedUser(NewProjectAPIContext context)
    {
      if (!context.Users.Any())
      {
        var userData = System.IO.File.ReadAllText("Repo/UserSeedData.json");
        var users = JsonConvert.DeserializeObject<List<Users>>(userData);
        foreach (var user in users)
        {
          byte[] passwordhash, passwordSalt;
          CreatePasswordHash("password", out passwordhash, out passwordSalt);
          user.HashPassword = passwordhash;
          user.PasswordSalt = passwordSalt;
          user.Username = user.Username.ToLower();
          context.Add(user);
        }

        context.SaveChanges();
      }
    }
    
    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
      using (var hvac = new System.Security.Cryptography.HMACSHA512())
      {
        passwordSalt = hvac.Key;
        passwordHash = hvac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
      }
    }
  }
}
