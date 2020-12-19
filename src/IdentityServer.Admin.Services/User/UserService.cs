using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.User;
using IdentityServer.Admin.Core.Entities.Users;
using IdentityServer.Admin.Dapper.Repositories.User;
using IdentityServer.Admin.Services.Security;

namespace IdentityServer.Admin.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IEncryptionService _encryptionService;

        public UserService(IUserRepository repository, IEncryptionService encryptionService)
        {
            _repository = repository;
            _encryptionService = encryptionService;
        }

        public async Task<PagedUserDto> GetPagedAsync(string search, int page, int pageSize = PageConstant.PageSize)
        {
            return await _repository.GetPagedAsync(search, page, pageSize);
        }

        public async Task<Core.Entities.Users.User> GetUserByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            return await _repository.GetAsync(id);
        }

        public async Task<List<Core.Entities.Users.User>> GetAllUsersAsync()
        {
            return await _repository.GetAllUsersAsync();
        }

        public async Task<Core.Entities.Users.User> GetUserByNameAsync(string name)
        {
            return await _repository.GetUserByNameAsync(name);
        }

        public async Task<bool> ValidateUserAsync(string name, string password)
        {
            var user = await GetUserByNameAsync(name);
            if (user == null)
                return false;

            var userPassword = await GetUserPasswordAsync(user.Id);

            if (userPassword == null)
                return false;

            return userPassword.Password.Equals(_encryptionService.CreatePasswordHash(password, userPassword.PasswordSalt));
        }

        public async Task<int> InsertUserAsync(Core.Entities.Users.User user)
        {
            user.Active = true;
            user.Deleted = false;
            user.SubjectId = Guid.NewGuid().ToString();
            user.CreationTime = DateTime.Now;
            return await _repository.InsertAsync(user);
        }

        public async Task<bool> UpdateUserAsync(Core.Entities.Users.User user)
        {
            user.CreationTime = DateTime.Now;
            return await _repository.UpdateAsync(user);
        }

        public async Task<bool> DeleteUserAsync(Core.Entities.Users.User user)
        {
            user.Deleted = true;
            return await _repository.UpdateAsync(user);
        }

        public async Task<PagedUserRoleDto> GetPagedUserRoleAsync(int userId, int page, int pageSize = PageConstant.PageSize)
        {
            return await _repository.GetPagedUserRoleAsync(userId, page, pageSize);
        }

        public async Task<bool> IsUserRoleExistsAsync(int userId, int roleId)
        {
            if (userId <= 0 || roleId <= 0)
                return false;

            return await _repository.IsUserRoleExistsAsync(userId, roleId);
        }

        public async Task<int> InsertUserRoleAsync(int userId, int roleId)
        {
            return await _repository.InsertUserRoleAsync(userId, roleId);
        }

        public async Task<UserRoleDto> GetUserRoleByAsync(int userId, int roleId)
        {
            return await _repository.GetUserRoleByAsync(userId, roleId);
        }

        public async Task<bool> DeleteUserRoleByAsync(int userId, int roleId)
        {
            return await _repository.DeleteUserRoleByAsync(userId, roleId);
        }

        public async Task<PagedUserClaimDto> GetPagedUserClaimAsync(int userId, int page, int pageSize = PageConstant.PageSize)
        {
            return await _repository.GetPagedUserClaimAsync(userId, page, pageSize);
        }

        public async Task<UserClaim> GetUserClaimByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            return await _repository.GetUserClaimByIdAsync(id);
        }

        public async Task<bool> IsUserClaimExistsAsync(int userId, string claimType, string claimValue)
        {
            return await _repository.IsUserClaimExistsAsync(userId, claimType, claimValue);
        }

        public async Task<int> InsertUserClaimAsync(UserClaim userClaim)
        {
            return await _repository.InsertUserClaimAsync(userClaim);
        }

        public async Task<bool> DeleteUserClaimByIdAsync(int id)
        {
            return await _repository.DeleteUserClaimByIdAsync(id);
        }

        public async Task<UserPassword> GetUserPasswordAsync(int id)
        {
            return await _repository.GetPasswordByUserIdAsync(id);
        }

        public async Task<UserPassword> GetPasswordByUserIdAsync(int userId)
        {
            return await _repository.GetPasswordByUserIdAsync(userId);
        }

        public async Task<int> InsertUserPasswordAsync(UserPassword userPassword)
        {
            var saltKey = _encryptionService.CreateSaltKey(6);
            userPassword.PasswordSalt = saltKey;
            userPassword.Password = _encryptionService.CreatePasswordHash(userPassword.Password, saltKey);
            userPassword.CreationTime = DateTime.Now;

            return await _repository.InsertUserPasswordAsync(userPassword);
        }

        public async Task<int> UpdateUserPasswordAsync(UserPassword userPassword)
        {
            var saltKey = _encryptionService.CreateSaltKey(6);
            userPassword.PasswordSalt = saltKey;
            userPassword.Password = _encryptionService.CreatePasswordHash(userPassword.Password, saltKey);
            userPassword.CreationTime = DateTime.Now;

            return await _repository.UpdateUserPasswordAsync(userPassword);
        }
    }
}
