using System.Linq;
using AutoMapper;
using Task.Dtos;
using Task.Models;

namespace Task.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {   
            CreateMap<ToDoForAddDto, ToDo>();
            CreateMap<ToDo, ToDoForListDto>()
                .ForMember(dest => dest.PhotoUrl, opt => {
                    opt.MapFrom(src => src.Images.FirstOrDefault().Url);
                });
                
            CreateMap<ToDo, ToDoForDetailedDto>()
                .ForMember(dest => dest.PhotoUrl, opt => {
                    opt.MapFrom(src => src.Images.FirstOrDefault().Url);
                });
            CreateMap<Image, ImagesForDetailedDto>();
            CreateMap<ToDoForUpdateDto, ToDo>(); 
            CreateMap<Image, ImageForReturnDto>();
            CreateMap<ImageForCreationDto, Image>();
            CreateMap<UserForRegisterDto, User>();
        }
    }
}