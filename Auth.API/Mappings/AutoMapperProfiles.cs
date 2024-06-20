using Auth.API.Models.Domain;
using Auth.API.Models.DTO;
using AutoMapper;

namespace Auth.API.Mappings
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<TeacherModel, TeacherDto>().ReverseMap();
            CreateMap<AddTeacherRequestDto, TeacherModel>().ReverseMap();
            CreateMap<UpdateTeacherRequestDto, TeacherModel>().ReverseMap();


            CreateMap<ImageModel, ImageDto>().ReverseMap();

            CreateMap<UserModel, UserModelDto>().ReverseMap();
        }
    }
}
