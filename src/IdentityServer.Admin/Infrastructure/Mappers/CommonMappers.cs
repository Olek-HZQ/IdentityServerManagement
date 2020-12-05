using AutoMapper;

namespace IdentityServer.Admin.Infrastructure.Mappers
{
    public class CommonMappers
    {
        static CommonMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<CommonMapperProfile>()).CreateMapper();
        }

        internal static IMapper Mapper { get; }
    }
}
