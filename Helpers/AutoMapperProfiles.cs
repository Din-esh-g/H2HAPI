using AutoMapper;
using NewProjectAPI.Models;
using NewProjectAPI.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewProjectAPI.Helpers
{
  public class AutoMapperProfiles:Profile
  {
    public AutoMapperProfiles()
    {
      CreateMap<Users, UserListDTO>()
        .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(scr =>scr.Photos.FirstOrDefault(prop=>prop.IsMain).URL))
        .ForMember(dest => dest.Age, opt => opt.MapFrom(scr => scr.DateOfBirth.CalculateAge())) ;
      CreateMap<Users, UserForDetails>()
        .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(scr => scr.Photos.FirstOrDefault(prop => prop.IsMain).URL))
        .ForMember(dest => dest.Age, opt => opt.MapFrom(scr => scr.DateOfBirth.CalculateAge()));
      CreateMap<Photo, PhotoForDTO>();
      CreateMap<Photo, PhotoForReturnDto>();
      CreateMap<PhotoForCreationDto, Photo>();
      CreateMap<UserForUpdateDTO, Users>();

    }

  }
}
