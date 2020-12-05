using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer.Admin.Dapper.Repositories.CommonInterfaces;
using IdentityServer4.Extensions;
using IdentityServer4.Models;

namespace IdentityServer.Admin.Services.Stores
{
    public class ProfileService : IdentityServer4.Services.IProfileService
    {
        private readonly IUserRepository _userRepository;

        public ProfileService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userRepository.GetUserBySubjectId(sub);

            var claims = new List<Claim>();

            if (user != null)
            {
                var userClaims = await _userRepository.GetUserClaimsByUserIdAsync(user.Id);
                if (userClaims.Any())
                {
                    userClaims.ForEach(x =>
                    {
                        claims.Add(new Claim(x.ClaimType, x.ClaimValue));
                    });
                }

                //claims.Add(new Claim(ClaimTypes.Name, user.Name));
                //claims.Add(new Claim(ClaimTypes.Email, user.Email));
                //claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            // context.IssuedClaims = claims;
            context.AddRequestedClaims(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userRepository.GetUserBySubjectId(sub);

            context.IsActive = user != null && user.Active && !user.Deleted;
        }
    }
}
