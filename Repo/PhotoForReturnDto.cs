using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewProjectAPI.Repo
{
  public class PhotoForReturnDto
  {
    public int Id { get; set; }
    public string URL { get; set; }
    public string Description { get; set; }
    public DateTime DateAdded { get; set; }
    public bool IsMain { get; set; }

    public string PublicId { get; set; }
  }
}
