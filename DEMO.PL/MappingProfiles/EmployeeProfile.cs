using AutoMapper;
using DEMO.DAL.Models;
using DEMO.PL.ViewModels;

namespace DEMO.PL.MappingProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
            //CreateMap<EmployeeViewModel, Employee>().ForMember(d=>d.Name , O=>O.MapFrom(S=>S.EmpName));
        }
    }
}
