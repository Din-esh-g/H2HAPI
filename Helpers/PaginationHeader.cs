using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewProjectAPI.Helpers
{
  public class PaginationHeader
  {
    public int CurrentPage{ get; set; }
    public int ItemsPerPage { get; set; }
    public int TotalItems { get; set; }

    public int TotalPage { get; set; }
    public PaginationHeader(int currentPage, int itemsPerPage, int tottalItems, int totalPages)
    {
      this.CurrentPage = currentPage;
      this.TotalItems = tottalItems;
      this.TotalPage = totalPages;
      this.ItemsPerPage = itemsPerPage;
    }
  }
}
