using System;
using System.Data; 
using DebugTools.DBSupport;
using DebugTools.Information;
using DebugTools.DataBase;
using Oracle.DataAccess.Client; 

namespace DebugTools.Common.DBSupport.Oracle
{
    public class OracleDBAccessor : IDBAccessor
    {
        private OracleConnection _connection = null;
        private OracleTransaction _transaction = null;

        public OracleDBAccessor(IConnectionSetting setting)
        {
            _connection = new OracleConnection(setting.ConnectionString);
        }

        public ResultInfo<bool> Open()
        {
            try
            {
                if (_connection.State == ConnectionState.Open)
                {
                    return new ResultInfo<bool>(true);
                }
                _connection.Open();
                return new ResultInfo<bool>(true);
            }
            catch  (Exception ex)
            {
                return new ResultInfo<bool>(false,ex.Message );
            }
        }

        public ResultInfo<bool> Test()
        {
            try
            {
                if (_connection.State == ConnectionState.Open)
                {
                    return new ResultInfo<bool>(true);
                }
                _connection.Open();
                _connection.Close();
                return new ResultInfo<bool>(true);
            }
            catch(Exception ex)
            {
                return new ResultInfo<bool>(false, ex.Message);
            }
        }
         
        public ResultInfo<bool> BeginTransaction()
        {
            if (_transaction != null)
            {
                return new ResultInfo<bool>(false, "トランザクションは既に始ります。");
            }
            try
            {
                ResultInfo<bool> result = Open();
                if (!result.Value) { return result; }
                _transaction = _connection.BeginTransaction();
                return new ResultInfo<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResultInfo<bool>(false, ex.Message);
            }
        }

        public ResultInfo<bool> Commit()
        {
            if (_transaction == null)
            {
                return new ResultInfo<bool>(false, "トランザクションは開始されてない");
            }
            try
            {
                _transaction.Commit();
                return new ResultInfo<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResultInfo<bool>(false, ex.Message);
            }
        }

        public void RollBack()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
            } 
        }

        public DataSet GetData(SqlInfo sqlInfo)
        {
            return GetData(new SqlInfo[] { sqlInfo });
        }

        public DataSet GetData(SqlInfo[] sqlInfos)
        {  
            try
            {
                ResultInfo<bool> result = Open();
                if (!result.Value) { return null; }
                DataSet dataset = new DataSet();
                foreach (SqlInfo sqlInfo in sqlInfos)
                {
                    OracleCommand command = CreateCommand(sqlInfo);
                    OracleDataAdapter adapter = new OracleDataAdapter(command);
                    if (string.IsNullOrEmpty(sqlInfo.ResultTableName))
                    {
                        adapter.Fill(dataset);
                    }
                    else
                    {
                        adapter.Fill(dataset, sqlInfo.ResultTableName);
                    }
                }
                return dataset;
            }
            catch
            {
                return null;
            }
        }

        public ResultInfo<int> ExecuteSql(SqlInfo sqlInfo)
        {
            return ExecuteSql(new SqlInfo[] { sqlInfo });
        }

        public ResultInfo<int> ExecuteSql(SqlInfo[] sqlInfos)
        {
            try
            {
                Open();
                int count = 0;
                foreach (SqlInfo sqlInfo in sqlInfos)
                {
                    OracleCommand command = CreateCommand(sqlInfo);
                    count += command.ExecuteNonQuery();
                }
                return new ResultInfo<int>(count);
            }
            catch (Exception ex)
            {
                return new ResultInfo<int>(-1, ex.Message);
            }
        }

        private OracleCommand CreateCommand(SqlInfo sqlInfo)
        {

            OracleCommand command = _connection.CreateCommand();
            command.CommandText = sqlInfo.SQL;
            return command;
        }

        public ResultInfo<bool> Close()
        {
            try
            {
                _connection.Close();
                return new ResultInfo<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResultInfo<bool>(false, ex.Message);
            }
        }

        public ResultInfo<bool> Reset()
        {
            _connection = null;
            return new ResultInfo<bool>(true);
        }
         
    }
}
