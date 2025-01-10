using AutoMapper;
using Domain.Models.Entities;
using LMS.Shared.DTOs.Create;
using LMS.Shared.DTOs.Read;
namespace LMS.Infrastructure.Data;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UserForRegistrationDto, ApplicationUser>().ReverseMap();
        CreateMap<ActivityDto, Activity>().ReverseMap();
        CreateMap<ActivityDocumentDto, ActivityDocument>().ReverseMap();
        CreateMap<ActivityTypeDto, ActivityType>().ReverseMap();
        CreateMap<CourseDto, Course>().ReverseMap();
        CreateMap<CourseDocumentDto, CourseDocument>().ReverseMap();
        CreateMap<ModuleDocumentDto, ModuleDocument>().ReverseMap();
        CreateMap<ModuleDto, Module>().ReverseMap();

        CreateMap<CourseCreateDto, Course>().ReverseMap();
        CreateMap<ModuleCreateDto, Module>();
        CreateMap<ActivityCreateDto, Activity>();

        CreateMap<ActivityTypeCreateDto, ActivityType>();
        //CreateMap<CourseDto, CourseCreateDto>().ReverseMap();   

 
    }
}
