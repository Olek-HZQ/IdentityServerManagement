using AutoMapper;
using IdentityServer.Admin.Core.Entities.Localization;
using IdentityServer.Admin.Models.Localization;

namespace IdentityServer.Admin.Infrastructure.Mappers
{
    public class LanguageMapperProfile : Profile
    {
        public LanguageMapperProfile()
        {
            CreateMap<Language, LanguageModel>().ReverseMap();
            CreateMap<LocaleStringResource, LocaleStringResourceModel>().ReverseMap();
        }
    }
}
