using NewProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewProjectAPI.Repo
{
  public interface IMyjobRepo
  {

  
    Task<MyJob> GetMyJob();
    Task<MyJob> GetMyJob(int id);
    Task<MyJob> PutMyJob(int id, MyJob myJob);
    Task <MyJob> PostmyJob(MyJob myjob);
    Task<MyJob> DeleteMyJob(int id);
    Task<bool> MyJobExists(int id);
  }
}
