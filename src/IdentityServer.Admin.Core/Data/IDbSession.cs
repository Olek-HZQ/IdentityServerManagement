﻿using System;
using System.Data;

namespace IdentityServer.Admin.Core.Data
{
    public interface IDbSession : IDisposable
    {
        IDbConnection Connection { get; }

        IDbTransaction Transaction { get; }

        IDbTransaction BeginTrans(IsolationLevel isolation = IsolationLevel.ReadCommitted);

        void Commit();

        void Rollback();
    }
}
