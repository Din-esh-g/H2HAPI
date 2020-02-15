using AutoMapper;
using NewProjectAPI.Controllers;
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
      CreateMap<UserDTO, Users>();
      CreateMap<MessageForCreationDTO, Message>().ReverseMap();
      CreateMap<Message, MessageToReturnDTO>()
        .ForMember(m => m.SenderPhotoUrl, opt => opt.MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).URL))
     .ForMember(m => m.RecipientPhotoUrl, opt => opt.MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).URL));

    }

  }
}
