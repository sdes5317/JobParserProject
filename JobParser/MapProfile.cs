using AutoMapper;
using JobParser.Core;

namespace JobParser
{
    public class MapProfile: Profile
    {
        public MapProfile()
        {
            CreateMap<JobElement, JobDto>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.innerText))
                .ForMember(x => x.Url, y => y.MapFrom(s => s.innerHTML));
            CreateMap<CompanyElement, JobDto>()
                .ForMember(x => x.Company, y => y.MapFrom(s => s.innerText));
            CreateMap<AreaElement, JobDto>()
                .ForMember(x => x.Address, y => y.MapFrom(s => s.innerText));
        }
    }
}
