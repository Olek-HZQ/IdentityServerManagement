using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.User;
using IdentityServer.Admin.Core.Entities.Users;

namespace IdentityServer.Admin.Services.User
{
    public interface IUserService
    {
        Task<PagedUserDto> GetPagedAsync(string search, int page, int pageSize = PageConstant.PageSize);

        Task<Core.Entities.Users.User> GetUserByIdAsync(int id);

        Task<List<Core.Entities.Users.User>> GetAllUsersAsync();

        Task<Core.Entities.Users.User> GetUserByNameAsync(string name);

        Task<bool> ValidateUserAsync(string name, string password);

        Task<int> InsertUserAsync(Core.Entities.Users.User user);

        Task<bool> UpdateUserAsync(Core.Entities.Users.User user);

        Task<bool> DeleteUserAsync(Core.Entities.Users.User user);

        Task<PagedUserRoleDto> GetPagedUserRoleAsync(int userId, int page, int pageSize = PageConstant.PageSize);

        Task<bool> IsUserRoleExistsAsync(int userId, int roleId);

        Task<int> InsertUserRoleAsync(int userId, int roleId);

        Task<UserRoleDto> GetUserRoleByAsync(int userId, int roleId);

        Task<bool> DeleteUserRoleByAsync(int userId, int roleId);

        Task<PagedUserClaimDto> GetPagedUserClaimAsync(int userId, int page, int pageSize = PageConstant.PageSize);

        Task<UserClaim> GetUserClaimByIdAsync(int id);

        Task<bool> IsUserClaimExistsAsync(int userId, string claimType, string claimValue);

        Task<int> InsertUserClaimAsync(UserClaim userClaim);

        Task<bool> DeleteUserClaimByIdAsync(int id);

        Task<UserPassword> GetUserPasswordAsync(int id);

        Task<UserPassword> GetPasswordByUserIdAsync(int userId);

        Task<int> InsertUserPasswordAsync(UserPassword userPassword);

        Task<int> UpdateUserPasswordAsync(UserPassword userPassword);
    }
}
