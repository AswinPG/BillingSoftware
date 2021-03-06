﻿using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using System.Linq;

namespace BillingSoftware.DapperService
{
    public class DapperDAO
    {
        IDbConnection dbConnection;

        private string _sqlconstring;
        private Int32 _commandtimeout;

        public DapperDAO(string sqlconstring, Int32 commandtimeout)
        {
            _sqlconstring = sqlconstring;
            _commandtimeout = commandtimeout;
        }

        public Boolean ExecuteQueery(string query)
        {
            using (dbConnection = new SqlConnection(_sqlconstring))
            {
                return (dbConnection.Execute(query, CommandType.Text) != 0);
            }
        }

        public Int32 Insert<T>(string query,Object obj, CommandType commandType=CommandType.Text)
        {
            return Execute<T>(query, obj, commandType: commandType);
        }

        public Int32 Update<T>(string query, Object obj, CommandType commandType = CommandType.Text)
        {
            return Execute<T>(query, obj, commandType: commandType);
        }

        public T Get<T>(string query, Object obj, CommandType commandType = CommandType.Text)
        {
            using (dbConnection = new SqlConnection(_sqlconstring))
            {
                return dbConnection.Query<T>(query, obj, commandTimeout: _commandtimeout, commandType: commandType).SingleOrDefault();
            }
        }

        public IEnumerable<T> GetAll<T>(string query, CommandType commandType = CommandType.Text)
        {
            using (dbConnection = new SqlConnection(_sqlconstring))
            {   
                return dbConnection.Query<T>(query, commandTimeout: _commandtimeout, commandType: commandType);
            }
        }

        public Int32 Delete<T>(string query, Object obj, CommandType commandType = CommandType.Text)
        {
            return Execute<T>(query, obj, commandType: commandType);
        }

        private Int32 Execute<T>(string query, Object obj, CommandType commandType = CommandType.Text)
        {
            using (dbConnection = new SqlConnection(_sqlconstring))
            {
                return dbConnection.Execute(query, obj, commandTimeout: _commandtimeout, commandType: commandType);
            }
        }

        public SqlMapper.GridReader QueryMultiple(string sql, dynamic param = null, IDbTransaction transaction = null, int? CommandTimeout = null, CommandType? CommandType = null)
        {
            try
            {
                IDbConnection Sqlcon = new SqlConnection(_sqlconstring);
                return SqlMapper.QueryMultiple(Sqlcon, sql, param, transaction, CommandTimeout, CommandType);
            }
            catch (Exception ex)
            {
                //WriteLog(ex.ToString());
                throw ex;
            }
        }
    }
}
