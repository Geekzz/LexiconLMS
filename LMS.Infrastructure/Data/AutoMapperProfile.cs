using AutoMapper;
using Domain.Models.Entities;
using LMS.Shared.DTOs;
namespace LMS.Infrastructure.Data;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UserForRegistrationDto, ApplicationUser>();
        CreateMap<ActivityDto, Activity>();
        CreateMap<ActivityDocumentDto, ActivityDocument>();
        CreateMap<ActivityTypeDto, ActivityType>();
        CreateMap<CourseDto, Course>();
        CreateMap<CourseDocumentDto, CourseDocument>();
        CreateMap<ModuleDocumentDto, ModuleDocument>();
        CreateMap<ModuleDto, Module>();
    }
}
