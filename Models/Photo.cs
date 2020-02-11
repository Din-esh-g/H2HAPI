using System;

namespace NewProjectAPI.Models
{
  public class Photo
  {
    public int Id { get; set; }
    public string URL { get; set; }
    public string Description { get; set; }
    public DateTime DateAdded { get; set; }
    public bool IsMain { get; set; }
    public string PublicId { get; set; }
    public Users User { get; set; }
    public int UserId { get; set; }


  }
}
