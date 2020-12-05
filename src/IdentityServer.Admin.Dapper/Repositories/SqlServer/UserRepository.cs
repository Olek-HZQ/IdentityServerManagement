using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using IdentityServer.Admin.Core.Configuration;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;
using IdentityServer.Admin.Core.Extensions;
using IdentityServer.Admin.Dapper.Repositories.CommonInterfaces;
using Serilog;
using SqlKata;

namespace IdentityServer.Admin.Dapper.Repositories.SqlServer
{
    public class UserRepository : MssqlRepositoryBase<User>, IUserRepository
    {
        public UserRepository(DbConnectionConfiguration dbConnectionConfig) : base(dbConnectionConfig)
        {
        }

        public async Task<PagedUserDto> GetPagedAsync(string search, int page, int pageSize)
        {
            var result = new PagedUserDto();

            IDbSession session = DbSession;

            try
            {
                var totalCountQuery = new Query(TableName).AsCount();
                var resultQuery = new Query(TableName).Select("*").WhereTrue("Active").WhereFalse("Deleted");

                if (!string.IsNullOrEmpty(search))
                {
                    totalCountQuery = totalCountQuery.WhereContains("Name", search);
                    resultQuery = resultQuery.WhereContains("Name", search);
                }

                var totalCountSqlResult = GetSqlResult(totalCountQuery);

                resultQuery = resultQuery.OrderByDesc("Id").Offset((page - 1) * pageSize).Limit(pageSize);
                var clientSecretSqlResult = GetSqlResult(resultQuery);

                result.TotalCount = await session.Connection.QueryFirstOrDefaultAsync<int>(totalCountSqlResult.Sql, totalCountSqlResult.NamedBindings);
                result.DataPagedList = (await session.Connection.QueryAsync<User>(clientSecretSqlResult.Sql, clientSecretSqlResult.NamedBindings)).ToList();

                result.PageSize = pageSize;

                return result;
            }
            finally
            {
                session.Dispose();
            }
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var query = new Query(TableName).Select("*");
            var sqlResult = GetSqlResult(query);
            return (await GetListAsync(sqlResult.Sql, sqlResult.NamedBindings)).ToList();
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            IDbSession session = DbSession;

            var query = new Query(TableName).Select("*").Where("Name", "=", name);
            var sqlResult = GetSqlResult(query);

            var user = await session.Connection.QueryFirstOrDefaultAsync<User>(sqlResult.Sql, sqlResult.NamedBindings);

            session.Dispose();

            return user;
        }

        public async Task<User> GetUserBySubjectId(string subjectId)
        {
            IDbSession session = DbSession;

            var query = new Query(AttributeExtension.GetTableAttributeName<User>()).Select("*").Where("SubjectId", "=", subjectId);
            var sqlResult = GetSqlResult(query);

            var user = await session.Connection.QueryFirstOrDefaultAsync<User>(sqlResult.Sql, sqlResult.NamedBindings);

            session.Dispose();

            return user;
        }

        public override async Task<bool> UpdateAsync(User entity, bool useTransaction = false, int? commandTimeout = null)
        {
            var query = new Query(AttributeExtension.GetTableAttributeName<User>()).AsUpdate(new
            {
                entity.Name,
                entity.Email,
                entity.Active,
                entity.Deleted,
                entity.CreationTime
            }).Where("Id", "=", entity.Id);
            var sqlResult = GetSqlResult(query);

            try
            {
                await ExecuteAsync(sqlResult.Sql, sqlResult.NamedBindings);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"UserRepository >> UpdateAsync Error: {ex.Message}");

                return false;
            }
        }

        public async Task<PagedUserRoleDto> GetPagedUserRoleAsync(int userId, int page, int pageSize)
        {
            var result = new PagedUserRoleDto();

            IDbSession session = DbSession;

            var userQuery = new Query(TableName).Select("Id", "Name").Where("Id", "=", userId);
            var userSqlResult = GetSqlResult(userQuery);
            var user = await session.Connection.QueryFirstOrDefaultAsync<User>(userSqlResult.Sql, userSqlResult.NamedBindings);

            try
            {
                if (user != null)
                {
                    result.UserId = user.Id;
                    result.UserName = user.Name;

                    var totalCountQuery = new Query($"{TableName} as u")
                        .Join($"{AttributeExtension.GetTableAttributeName<UserRoleMap>()} as urm", "u.Id", "urm.UserId")
                        .Join($"{AttributeExtension.GetTableAttributeName<Role>()} as r", "urm.RoleId", "r.Id")
                        .Where("u.Id", "=", userId).AsCount();
                    var resultQuery = new Query($"{TableName} as u")
                        .Join($"{AttributeExtension.GetTableAttributeName<UserRoleMap>()} as urm", "u.Id", "urm.UserId")
                        .Join($"{AttributeExtension.GetTableAttributeName<Role>()} as r", "urm.RoleId", "r.Id")
                        .Select("u.Id as UserId", "u.Name as UserName", "r.Id as RoleId", "r.Name as RoleName").Where("u.Id", "=", userId);

                    var totalCountSqlResult = GetSqlResult(totalCountQuery);

                    resultQuery = resultQuery.OrderByDesc("u.Id").Offset((page - 1) * pageSize).Limit(pageSize);
                    var clientSecretSqlResult = GetSqlResult(resultQuery);

                    result.TotalCount = await session.Connection.QueryFirstOrDefaultAsync<int>(totalCountSqlResult.Sql, totalCountSqlResult.NamedBindings);
                    result.DataPagedList = (await session.Connection.QueryAsync<UserRoleForPage>(clientSecretSqlResult.Sql, clientSecretSqlResult.NamedBindings)).ToList();

                    result.PageSize = pageSize;
                }


                return result;
            }
            finally
            {
                session.Dispose();
            }
        }

        public async Task<bool> IsUserRoleExistsAsync(int userId, int roleId)
        {
            IDbSession session = DbSession;

            var query = new Query(AttributeExtension.GetTableAttributeName<UserRoleMap>()).Select("*").Where("UserId", "=", userId).Where("RoleId", "=", roleId);
            var sqlResult = GetSqlResult(query);

            var result = (await session.Connection.QueryFirstOrDefaultAsync<UserRoleMap>(sqlResult.Sql, sqlResult.NamedBindings)) != null;

            session.Dispose();

            return result;
        }

        public async Task<int> InsertUserRoleAsync(int userId, int roleId)
        {
            var query = new Query(AttributeExtension.GetTableAttributeName<UserRoleMap>()).AsInsert(new
            {
                UserId = userId,
                RoleId = roleId
            });
            var sqlResult = GetSqlResult(query);

            try
            {
                var insertedResult = await ExecuteAsync(sqlResult.Sql, sqlResult.NamedBindings);

                return insertedResult;
            }
            catch (Exception ex)
            {
                Log.Error($"UserRepository >> InsertUserRoleAsync Error: {ex.Message}");

                return 0;
            }
        }

        public async Task<UserRoleDto> GetUserRoleByAsync(int userId, int roleId)
        {
            IDbSession session = DbSession;

            var query = new Query($"{TableName} as u")
                .Join($"{AttributeExtension.GetTableAttributeName<UserRoleMap>()} as urm", "u.Id", "urm.UserId")
                .Join($"{AttributeExtension.GetTableAttributeName<Role>()} as r", "urm.RoleId", "r.Id")
                .Select("u.Id as UserId", "u.Name as UserName", "r.Id as RoleId", "r.Name as RoleName")
                .Where("u.Id", "=", userId).Where("r.Id", "=", roleId);

            var sqlResult = GetSqlResult(query);

            var result = await session.Connection.QueryFirstOrDefaultAsync<UserRoleDto>(sqlResult.Sql, sqlResult.NamedBindings);

            session.Dispose();

            return result;
        }

        public async Task<bool> DeleteUserRoleByAsync(int userId, int roleId)
        {
            var query = new Query(AttributeExtension.GetTableAttributeName<UserRoleMap>()).Where("UserId", "=", userId).Where("RoleId", "=", roleId).AsDelete();
            var sqlResult = GetSqlResult(query);

            var result = await ExecuteAsync(sqlResult.Sql, sqlResult.NamedBindings);

            return result > 0;
        }

        public async Task<PagedUserClaimDto> GetPagedUserClaimAsync(int userId, int page, int pageSize)
        {
            var result = new PagedUserClaimDto();

            IDbSession session = DbSession;

            var userQuery = new Query(TableName).Select("Id", "Name").Where("Id", "=", userId);
            var userSqlResult = GetSqlResult(userQuery);
            var user = await session.Connection.QueryFirstOrDefaultAsync<User>(userSqlResult.Sql, userSqlResult.NamedBindings);

            try
            {
                if (user != null)
                {
                    result.UserId = user.Id;
                    result.UserName = user.Name;

                    var totalCountQuery = new Query(AttributeExtension.GetTableAttributeName<UserClaim>()).Where("UserId", "=", userId).AsCount();
                    var resultQuery = new Query(AttributeExtension.GetTableAttributeName<UserClaim>()).Where("UserId", "=", userId).Select("*");

                    var totalCountSqlResult = GetSqlResult(totalCountQuery);

                    resultQuery = resultQuery.OrderByDesc("Id").Offset((page - 1) * pageSize).Limit(pageSize);
                    var clientSecretSqlResult = GetSqlResult(resultQuery);

                    result.TotalCount = await session.Connection.QueryFirstOrDefaultAsync<int>(totalCountSqlResult.Sql, totalCountSqlResult.NamedBindings);
                    result.DataPagedList = (await session.Connection.QueryAsync<UserClaimForPage>(clientSecretSqlResult.Sql, clientSecretSqlResult.NamedBindings)).ToList();

                    result.PageSize = pageSize;
                }

                return result;
            }
            finally
            {
                session.Dispose();
            }
        }

        public async Task<UserClaim> GetUserClaimByIdAsync(int id)
        {
            IDbSession session = DbSession;

            var query = new Query(AttributeExtension.GetTableAttributeName<UserClaim>()).Select("*").Where("Id", "=", id);
            var sqlResult = GetSqlResult(query);

            var userClaim = await session.Connection.QueryFirstOrDefaultAsync<UserClaim>(sqlResult.Sql, sqlResult.NamedBindings);

            session.Dispose();

            return userClaim;
        }

        public async Task<List<UserClaim>> GetUserClaimsByUserIdAsync(int userId)
        {
            IDbSession session = DbSession;

            var query = new Query(AttributeExtension.GetTableAttributeName<UserClaim>()).Select("*").Where("UserId", "=", userId);
            var sqlResult = GetSqlResult(query);

            var userClaims = (await session.Connection.QueryAsync<UserClaim>(sqlResult.Sql, sqlResult.NamedBindings)).ToList();

            session.Dispose();

            return userClaims;
        }

        public async Task<bool> IsUserClaimExistsAsync(int userId, string claimType, string claimValue)
        {
            IDbSession session = DbSession;

            var query = new Query(AttributeExtension.GetTableAttributeName<UserClaim>()).Select("*")
                .Where("UserId", "=", userId)
                .Where("ClaimType", "=", claimType)
                .Where("ClaimValue", "=", claimValue);
            var sqlResult = GetSqlResult(query);

            var result = (await session.Connection.QueryFirstOrDefaultAsync<UserClaim>(sqlResult.Sql, sqlResult.NamedBindings)) != null;

            session.Dispose();

            return result;
        }

        public async Task<int> InsertUserClaimAsync(UserClaim userClaim)
        {
            var query = new Query(AttributeExtension.GetTableAttributeName<UserClaim>()).AsInsert(new
            {
                userClaim.UserId,
                userClaim.ClaimType,
                userClaim.ClaimValue
            });
            var sqlResult = GetSqlResult(query);

            var insertedResult = await ExecuteAsync(sqlResult.Sql, sqlResult.NamedBindings);

            return insertedResult;
        }

        public async Task<bool> DeleteUserClaimByIdAsync(int id)
        {
            var query = new Query(AttributeExtension.GetTableAttributeName<UserClaim>()).Where("Id", "=", id).AsDelete();
            var sqlResult = GetSqlResult(query);

            var deletedResult = await ExecuteAsync(sqlResult.Sql, sqlResult.NamedBindings);

            return deletedResult > 0;
        }

        public async Task<UserPassword> GetUserPasswordAsync(int id)
        {
            IDbSession session = DbSession;

            var query = new Query(AttributeExtension.GetTableAttributeName<UserPassword>()).Select("*").Where("Id", "=", id);
            var sqlResult = GetSqlResult(query);

            var userPassword = await session.Connection.QueryFirstOrDefaultAsync<UserPassword>(sqlResult.Sql, sqlResult.NamedBindings);

            session.Dispose();

            return userPassword;
        }

        public async Task<UserPassword> GetPasswordByUserIdAsync(int userId)
        {
            IDbSession session = DbSession;

            var query = new Query(AttributeExtension.GetTableAttributeName<UserPassword>()).Select("*").Where("UserId", "=", userId).OrderByDesc("CreationTime");
            var sqlResult = GetSqlResult(query);

            var userPassword = await session.Connection.QueryFirstOrDefaultAsync<UserPassword>(sqlResult.Sql, sqlResult.NamedBindings);

            session.Dispose();

            return userPassword;
        }

        public async Task<int> InsertUserPasswordAsync(UserPassword userPassword)
        {
            var query = new Query(AttributeExtension.GetTableAttributeName<UserPassword>()).AsInsert(new
            {
                userPassword.UserId,
                userPassword.PasswordSalt,
                userPassword.Password,
                userPassword.CreationTime
            });
            var sqlResult = GetSqlResult(query);

            var insertedResult = await ExecuteAsync(sqlResult.Sql, sqlResult.NamedBindings);

            return insertedResult;
        }

        public async Task<int> UpdateUserPasswordAsync(UserPassword userPassword)
        {
            var query = new Query(AttributeExtension.GetTableAttributeName<UserPassword>()).AsUpdate(new
            {
                userPassword.UserId,
                userPassword.PasswordSalt,
                userPassword.Password,
                userPassword.CreationTime
            }).Where("Id", "=", userPassword.Id);
            var sqlResult = GetSqlResult(query);

            var updatedResult = await ExecuteAsync(sqlResult.Sql, sqlResult.NamedBindings);

            return updatedResult;
        }
    }
}
