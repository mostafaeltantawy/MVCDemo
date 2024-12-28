using AutoMapper;
using DEMO.DAL.Models;
using DEMO.PL.ViewModels;

namespace DEMO.PL.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserViewModel , ApplicationUser>().ReverseMap();
        }
    }
}
