using AutoMapper;
using Entities.Dto_DataTransferObjects__;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Helpers
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForMember(dest => dest.FullAddress, from => from
                .MapFrom(x=> string.Join(',', x.Address, x.Country))).ReverseMap();// первым свойсвтом указывается КУДА мапим, вторым ОТКУДА

            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<CompanyForCreationDto, Company>().ReverseMap();
            CreateMap<EmployeeForCreationDto, Employee>().ReverseMap();
            CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();
            CreateMap<CompanyForUpdateDto, Company>().ReverseMap();
            CreateMap<UserForCreationDto, User>().ReverseMap();
        }
    }
}
