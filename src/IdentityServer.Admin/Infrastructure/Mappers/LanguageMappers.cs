using AutoMapper;
using IdentityServer.Admin.Core.Entities.Localization;
using IdentityServer.Admin.Models.Localization;

namespace IdentityServer.Admin.Infrastructure.Mappers
{
    public static class LanguageMappers
    {
        static LanguageMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<LanguageMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        /// <summary>
        /// Maps an entity to a model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static LanguageModel ToModel(this Language entity)
        {
            return Mapper.Map<LanguageModel>(entity);
        }

        /// <summary>
        /// Maps a model to an entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static Language ToEntity(this LanguageModel model)
        {
            return Mapper.Map<Language>(model);
        }

        public static LocaleStringResourceModel ToLocaleStringResourceModel(this LocaleStringResource resource)
        {
            return Mapper.Map<LocaleStringResourceModel>(resource);
        }

        public static LocaleStringResource ToLocaleStringResource(this LocaleStringResourceModel model)
        {
            return Mapper.Map<LocaleStringResource>(model);
        }
    }
}
