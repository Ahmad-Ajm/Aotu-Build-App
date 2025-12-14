using AutoMapper;
using CVSystem.CVs;
using CVSystem.CVs.Dtos;
using System.Text.Json;

namespace CVSystem.Application
{
    public class CVSystemApplicationAutoMapperProfile : Profile
    {
        public CVSystemApplicationAutoMapperProfile()
        {
            /* You can define your AutoMapper object mappings here. */
            CreateMap<CV, CvDto>()
                .ForMember(dest => dest.PersonalInfo, opt => opt.MapFrom(src => src.PersonalInfo != null ? JsonSerializer.Deserialize<PersonalInfoForCvDto>(src.PersonalInfo) : null))
                .ForMember(dest => dest.WorkExperience, opt => opt.MapFrom(src => src.WorkExperience != null ? JsonSerializer.Deserialize<System.Collections.Generic.List<WorkExperienceForCvDto>>(src.WorkExperience) : new System.Collections.Generic.List<WorkExperienceForCvDto>()))
                .ForMember(dest => dest.Education, opt => opt.MapFrom(src => src.Education != null ? JsonSerializer.Deserialize<System.Collections.Generic.List<EducationForCvDto>>(src.Education) : new System.Collections.Generic.List<EducationForCvDto>()))
                .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.Skills != null ? JsonSerializer.Deserialize<System.Collections.Generic.List<SkillForCvDto>>(src.Skills) : new System.Collections.Generic.List<SkillForCvDto>()));

            CreateMap<CreateCvDto, CV>()
                .ForMember(dest => dest.PersonalInfo, opt => opt.Ignore())
                .ForMember(dest => dest.WorkExperience, opt => opt.Ignore())
                .ForMember(dest => dest.Education, opt => opt.Ignore())
                .ForMember(dest => dest.Skills, opt => opt.Ignore());

            CreateMap<UpdateCvDto, CV>()
                .ForMember(dest => dest.PersonalInfo, opt => opt.Ignore())
                .ForMember(dest => dest.WorkExperience, opt => opt.Ignore())
                .ForMember(dest => dest.Education, opt => opt.Ignore())
                .ForMember(dest => dest.Skills, opt => opt.Ignore());
        }
    }
}
