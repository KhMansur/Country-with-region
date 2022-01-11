using Data.Models;
using Services.DTO;
using AutoMapper;
using Data.Context;

namespace CountryRegion.MapConfig
{
    public class MapConfiguration : Profile
    {
        public MapConfiguration()
        {
            CreateMap<Region, RegionCreationDTO>().ReverseMap();

            CreateMap<Country, CountryCreationDTO>()
                .ForMember(dto => dto.FullName,
                src => src.MapFrom(p => p.Name))
                .ReverseMap();

        }
        /*
         
        CreateMap<CreateOrEditProfileDto, EntityProfile>()
          .ForMember(dest => dest.Routes, opt =>
             opt.MapFrom(src => src.Routes.Select(x=>
             new Route()
             {
                 ProfileId = x.ProfileId,
                 DriverName = x.DriverName,
                 DriverSurname = x.DriverSurname,
                 Phone = x.Phone,
                 Email = x.Email,
             }
             ))
          );

        */
    }
}
