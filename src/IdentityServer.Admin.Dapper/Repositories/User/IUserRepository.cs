using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos.User;
using IdentityServer.Admin.Core.Entities.Users;

namespace IdentityServer.Admin.Dapper.Repositories.User
{
    public interface IUserRepository : IRepository<Core.Entities.Users.User>
    {
        Task<PagedUserDto> GetPagedAsync(string search, int page, int pageSize);

        Task<List<Core.Entities.Users.User>> GetAllUsersAsync();

        Task<Core.Entities.Users.User> GetUserByNameAsync(string name);

        Task<Core.Entities.Users.User> GetUserBySubjectId(string subjectId);

        Task<PagedUserRoleDto> GetPagedUserRoleAsync(int userId, int page, int pageSize);

        Task<bool> IsUserRoleExistsAsync(int userId, int roleId);

        Task<int> InsertUserRoleAsync(int userId, int roleId);

        Task<UserRoleDto> GetUserRoleByAsync(int userId, int roleId);

        Task<bool> DeleteUserRoleByAsync(int userId, int roleId);

        Task<PagedUserClaimDto> GetPagedUserClaimAsync(int userId, int page, int pageSize);

        Task<UserClaim> GetUserClaimByIdAsync(int id);

        Task<List<UserClaim>> GetUserClaimsByUserIdAsync(int userId);

        Task<bool> IsUserClaimExistsAsync(int userId, string claimType, string claimValue);

        Task<int> InsertUserClaimAsync(UserClaim userClaim);

        Task<bool> DeleteUserClaimByIdAsync(int id);

        Task<UserPassword> GetUserPasswordAsync(int id);

        Task<UserPassword> GetPasswordByUserIdAsync(int userId);

        Task<int> InsertUserPasswordAsync(UserPassword userPassword);

        Task<int> UpdateUserPasswordAsync(UserPassword userPassword);
    }
}
