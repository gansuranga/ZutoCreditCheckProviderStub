﻿using System.Data;
using Application.TestApi.Entities;
using Dapper;

namespace Application.TestApi.DataAccess
{
    public class DapperQueryFactory : IDapperQueryFactory
    {
        public ICommandDefinition<ConfigureModel> GetConfigureModel(IDbTransaction dbTransaction, string provider,
            string env)
        {
            return new CommandDefinition<ConfigureModel>(@"SELECT TOP 1  p.ID as Id,
                         p.Name as Provider,
                         e.Name as ForEnvironment, 
                         cfg.TimeoutMs as TimeOutMilliSeconds,
                         cfg.StackTrace as ThrowStackTrace,
                         cfg.Exception as ThrowFaultException,
                         cfg.NoMatch as NoMatch,
                         cfg.SingleMatch as SingleMatch,
                         cfg.MultipleMatch as MultipleMatch,
                         cfg.NoTrace,
                         cfg.RNILF04Score,
                         cfg.FTILF04Score,
                         cfg.RNILF01Score,
                         cfg.INOSF04Score,
                         cfg.ThrowValidationFaultException
                        FROM [dbo].[Provider] p
                        INNER JOIN [dbo].[Environment] e ON p.EnvID = e.ID
                        INNER JOIN [dbo].[Configuration] cfg ON cfg.ProviderId = p.ID
                         WHERE e.Name = @environment AND p.Name = @provider",
                new {environment = env, provider},
                dbTransaction);
        }

        public ICommandDefinition<int> UpsertConfiguration(IDbTransaction dbTransaction, ConfigureModel provider)
        {
            var p = new DynamicParameters();

            p.Add("Environment", provider.ForEnvironment);
            p.Add("Provider", provider.Provider);
            p.Add("TimeOut", provider.TimeOutMilliSeconds);
            p.Add("Exception", provider.ThrowFaultException);
            p.Add("StackTrace", provider.ThrowStackTrace);
            p.Add("NoTrace", provider.NoTrace);
            p.Add("NoMatch", provider.NoMatch);
            p.Add("SingleMatch", provider.SingleMatch);
            p.Add("MultipleMatch", provider.MultipleMatch);
            p.Add("RNILF04Score", provider.RNILF04Score);
            p.Add("FTILF04Score", provider.FTILF04Score);
            p.Add("RNILF01Score", provider.RNILF01Score);
            p.Add("INOSF04Score", provider.INOSF04Score);
            p.Add("ThrowValidationFaultException", provider.ThrowValidationFaultException);

            return new CommandDefinition<int>("UpsertConfig", p, dbTransaction, CommandType.StoredProcedure);
        }
    }
}