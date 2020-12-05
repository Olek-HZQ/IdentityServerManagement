using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;

namespace IdentityServer.Admin.Services.CommonInterfaces
{
    public interface IUserService
    {
        Task<PagedUserDto> GetPagedAsync(string search, int page, int pageSize = PageConstant.PageSize);

        Task<User> GetUserByIdAsync(int id);

        Task<List<User>> GetAllUsersAsync();

        Task<User> GetUserByNameAsync(string name);

        Task<bool> ValidateCustomerAsync(string name, string password);

        Task<int> InsertUserAsync(User user);

        Task<bool> UpdateUserAsync(User user);

        Task<bool> DeleteUserAsync(User user);

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
