using AutoMapper;
using Sehirler.Dtos;
using Sehirler.Models;

namespace Sehirler.Helpers
{
	public class AutoMapperProfiles:Profile
	{
        public AutoMapperProfiles()
        {
            CreateMap<City, CtiryForListDTO>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                });
            CreateMap<City, CtiyForDetailDto>();
            CreateMap<Photo, PhotoForCreationDto>();
			CreateMap<PhotoForCreationDto, Photo>();
		}
    }
}
