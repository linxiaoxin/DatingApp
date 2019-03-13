using System.Linq;
using AutoMapper;
using DatingApp.API.DTO;
using DatingApp.API.Model;

namespace DatingApp.API.Helper
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForDetailDTO>()
            .ForMember(dest => dest.PhotoUrl, opt => {
                opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.isMain).Url);
            })
            .ForMember(dest => dest.Age, opt => {
                   opt.MapFrom(src => src.DateOfBirth.CalculateAge());  
            });
            CreateMap<User, UserForListDTO>()
                .ForMember(dest => dest.PhotoUrl, opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.isMain).Url);
                })
                .ForMember(dest => dest.Age, opt => {
                   opt.MapFrom(src => src.DateOfBirth.CalculateAge());  
                });
            CreateMap<Photo, PhotoForDetailsDto>();
            CreateMap<UserForUpdateDto, User>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<UserForRegisterDto, User>();
        }
    }
}