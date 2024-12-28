using AutoMapper;
using DEMO.PL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace DEMO.PL.MappingProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile() 
        {
            CreateMap<IdentityRole , RoleViewModel>().ForMember(R =>R.RoleName , O=>O.MapFrom(S => S.Name)).ReverseMap();
        }
    }
}
