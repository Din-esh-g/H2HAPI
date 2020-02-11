using System;
using System.Collections.Generic;

namespace NewProjectAPI.Models
{
    public class Users
    {
    public int Id { get; set; }
    public string Username { get; set; }
    public byte [] HashPassword { get; set; }
    public byte [] PasswordSalt { get; set; }

    public string  Gender  { get; set; }

    public string KnownAs  { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime Created{ get; set; }
    public DateTime LastActive { get; set; }

    public string Introduction { get; set; }
    public string LookingFor { get; set; }
    public string Interest { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public ICollection<Photo> Photos { get; set; }
  }
}
