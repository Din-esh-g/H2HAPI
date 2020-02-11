using Microsoft.EntityFrameworkCore;
using NewProjectAPI.Data;
using NewProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewProjectAPI.Repo
{
  public class MyJobRepocs : IMyjobRepo
  {
    private readonly NewProjectAPIContext _context;
    public MyJobRepocs(NewProjectAPIContext context)
    {
      _context = context;
    }
    public Task<MyJob> DeleteMyJob(int id)
    {
      throw new NotImplementedException();
    }

    public Task<MyJob> GetMyJob()
    {
      throw new NotImplementedException();
    }

    public Task<MyJob> GetMyJob(int id)
    {
      throw new NotImplementedException();
    }

    public Task<bool> MyJobExists(int id)
    {
      throw new NotImplementedException();
    }

    public  Task<MyJob> PostmyJob(MyJobDTO myjob)
    {


      throw new NotImplementedException();



    }

    public Task<MyJob> PostmyJob(MyJob myjob)
    {
      throw new NotImplementedException();
    }

    public Task<MyJob> PutMyJob(int id, MyJob myJob)
    {
      throw new NotImplementedException();
    }



  }
}
