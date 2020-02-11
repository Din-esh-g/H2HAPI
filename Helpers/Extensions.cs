using Microsoft.AspNetCore.Http;
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

    public static int CalculateAge(this DateTime thedate)
    {
      var age = DateTime.Today.Year - thedate.Year;
      if (thedate.AddYears(age) > DateTime.Today)
        age--;
      return age;

    }
  }
}
