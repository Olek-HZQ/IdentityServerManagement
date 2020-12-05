﻿using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using IdentityServer.Admin.Core.Data;

namespace IdentityServer.Admin.Dapper
{
    public abstract class RepositoryBase<T> : RepositoryDataTypeBase<T> where T : class
    {
        public virtual async Task<T> GetAsync(int id, bool useTransaction = false, int? commandTimeout = null)
        {
            if (id == 0)
                return null;

            IDbSession session = DbSession;

            IDbTransaction transaction = null;
            if (useTransaction)
            {
                session.BeginTrans();
                transaction = session.Transaction;
            }

            try
            {
                var result = await session.Connection.GetAsync<T>(id, transaction, commandTimeout);

                transaction?.Commit();

                return result;
            }
            finally
            {
                session.Dispose();
            }
        }

        public virtual async Task<T> GetFirstOrDefaultAsync(string sql, object param = null, bool useTransaction = false, int? commandTimeout = null)
        {
            IDbSession session = DbSession;

            IDbTransaction transaction = null;
            if (useTransaction)
            {
                session.BeginTrans();
                transaction = session.Transaction;
            }

            try
            {
                var result = await session.Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandTimeout);

                transaction?.Commit();

                return result;
            }
            finally
            {
                session.Dispose();
            }
        }

        public virtual async Task<IEnumerable<T>> GetListAsync(string sql, object param = null, bool useTransaction = false,
            int? commandTimeout = null, CommandType? commandType = null)
        {
            if (string.IsNullOrEmpty(sql))
                return null;

            IDbSession session = DbSession;

            IDbTransaction transaction = null;
            if (useTransaction)
            {
                session.BeginTrans();
                transaction = session.Transaction;
            }

            try
            {
                var result = await session.Connection.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);

                transaction?.Commit();

                return result;
            }
            finally
            {
                session.Dispose();
            }
        }

        public virtual async Task<int> InsertAsync(T entity, bool useTransaction = false, int? commandTimeout = null)
        {
            IDbSession session = DbSession;

            IDbTransaction transaction = null;
            if (useTransaction)
            {
                session.BeginTrans();
                transaction = session.Transaction;
            }

            try
            {
                int result = await session.Connection.InsertAsync(entity, transaction, commandTimeout);

                transaction?.Commit();

                return result;
            }
            catch
            {
                transaction?.Rollback();

                return 0;
            }
            finally
            {
                session.Dispose();
            }
        }

        public virtual async Task<bool> UpdateAsync(T entity, bool useTransaction = false, int? commandTimeout = null)
        {
            IDbSession session = DbSession;

            IDbTransaction transaction = null;
            if (useTransaction)
            {
                session.BeginTrans();
                transaction = session.Transaction;
            }

            try
            {
                bool result = await session.Connection.UpdateAsync(entity, transaction, commandTimeout);

                transaction?.Commit();

                return result;
            }
            catch
            {
                transaction?.Rollback();

                return false;
            }
            finally
            {
                session.Dispose();
            }
        }

        public virtual async Task<bool> DeleteAsync(T entity, bool useTransaction = false, int? commandTimeout = null)
        {
            IDbSession session = DbSession;

            IDbTransaction transaction = null;
            if (useTransaction)
            {
                session.BeginTrans();
                transaction = session.Transaction;
            }

            try
            {
                bool result = await session.Connection.DeleteAsync(entity, transaction, commandTimeout);

                transaction?.Commit();

                return result;
            }
            catch
            {
                transaction?.Rollback();

                return false;
            }
            finally
            {
                session.Dispose();
            }
        }

        public virtual async Task<int> ExecuteAsync(string sql, object param = null, bool useTransaction = false, int? commandTimeout = null,
            CommandType? commandType = null)
        {
            IDbSession session = DbSession;

            IDbTransaction transaction = null;
            if (useTransaction)
            {
                session.BeginTrans();
                transaction = session.Transaction;
            }

            try
            {
                int result = await session.Connection.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);

                transaction?.Commit();

                return result;
            }
            catch
            {
                transaction?.Rollback();

                return 0;
            }
            finally
            {
                session.Dispose();
            }
        }
    }
}