using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewProjectAPI.Helpers
{
  public static class Extensions
  {
    //This is the extension method for the statrt method
    public static void AddApplicationError(this HttpResponse response, string message)
    {
      response.Headers.Add("Appliaction-Error", message);
      response.Headers.Add("Access-Control-Expose-Headers","Application-Error");
      response.Headers.Add("Access-Control-Allow-Origin","*");

    }
    public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int tottalItems, int totalPages)
    {
      var pagiantaionHeader = new PaginationHeader(currentPage, itemsPerPage, tottalItems, totalPages);
      var camelCaseFormatter = new JsonSerializerSettings();
      camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
      response.Headers.Add("Pagination", JsonConvert.SerializeObject(pagiantaionHeader, camelCaseFormatter));
      response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
    }

    public static int CalculateAge(this DateTime thedateTime)
    {
      var age = DateTime.Today.Year - thedateTime.Year;
      if (thedateTime.AddYears(age) > DateTime.Today)
        age--;
      return age;

    }
  }
}
