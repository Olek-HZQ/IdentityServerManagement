using AutoMapper;
using IdentityServer.Admin.Core.Dtos.User;
using IdentityServer.Admin.Core.Entities;
using IdentityServer.Admin.Core.Entities.ApiResource;
using IdentityServer.Admin.Core.Entities.ApiScope;
using IdentityServer.Admin.Core.Entities.IdentityResource;
using IdentityServer.Admin.Core.Entities.Users;
using IdentityServer.Admin.Models.ApiResource;
using IdentityServer.Admin.Models.ApiScope;
using IdentityServer.Admin.Models.IdentityResource;
using IdentityServer.Admin.Models.PersistedGrant;
using IdentityServer.Admin.Models.Role;
using IdentityServer.Admin.Models.User;

namespace IdentityServer.Admin.Infrastructure.Mappers
{
    public class CommonMapperProfile : Profile
    {
        public CommonMapperProfile()
        {
            CreateMap<IdentityResource, IdentityResourceModel>().ReverseMap();

            CreateMap<IdentityResourceClaim, IdentityResourceClaimModel>().ReverseMap();

            CreateMap<IdentityResourceProperty, IdentityResourcePropertyModel>().ReverseMap();

            CreateMap<ApiResource, ApiResourceModel>().ReverseMap();

            CreateMap<ApiResourceSecret, ApiResourceSecretModel>()
                .ForMember(x => x.HashType, y => y.Ignore())
                .ForMember(x => x.HashTypeEnum, y => y.Ignore())
                .ReverseMap();

            CreateMap<ApiResourceScope, ApiResourceScopeModel>().ReverseMap();

            CreateMap<ApiResourceClaim, ApiResourceClaimModel>().ReverseMap();

            CreateMap<ApiResourceProperty, ApiResourcePropertyModel>().ReverseMap();
            CreateMap<ApiResourceScope, ApiResourceScopeModel>().ReverseMap();

            CreateMap<ApiScope, ApiScopeModel>().ForMember(x => x.UserClaimsItems, y => y.Ignore()).ReverseMap();

            CreateMap<ApiScopeClaim, ApiScopeClaimModel>().ReverseMap();

            CreateMap<ApiScopeProperty, ApiScopePropertyModel>().ReverseMap();

            CreateMap<PersistedGrant, PersistedGrantModel>().ReverseMap();

            CreateMap<User, UserModel>().ReverseMap();

            CreateMap<Role, RoleModel>().ReverseMap();

            CreateMap<UserPassword, UserPasswordModel>().ForMember(x => x.ConfirmPassword, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<UserRoleDto, DeleteUserRoleModel>().ReverseMap();

            CreateMap<UserClaim, UserClaimModel>().ForMember(x=>x.UserName,opt=>opt.Ignore()).ReverseMap();
        }
    }
}
