using System;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web;

namespace DocumentManagementServices
{
    internal class SQLDataAccessHelper
    {
        
        private static SQLDataAccessHelper instance;
        private static readonly object padlock = new object();
        private string connectionString = string.Empty;

        private SQLDataAccessHelper()
        {
        

            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

            }
            catch (Exception e)
            {
               
                throw e;
            }

        }

        public static SQLDataAccessHelper Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new SQLDataAccessHelper();
                    }
                    return instance;
                }
            }
        }

        #region ExecuteReader

        public   SqlDataReader ExecuteReader(CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteReader(connectionString, commandType, commandText);
        }
        public  SqlDataReader ExecuteReader(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            return SqlHelper.ExecuteReader(connectionString, commandType, commandText, commandParameters);
        }
        public  SqlDataReader ExecuteReader(string spName, params object[] parameterValues)
        {
            return SqlHelper.ExecuteReader(connectionString, spName, parameterValues);
        }


        #endregion ExecuteReader

        //#region ExecuteDataSet

        public  DataSet ExecuteDataset(CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteDataset(connectionString, commandType, commandText);
        }
        public  DataSet ExecuteDataset(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            return SqlHelper.ExecuteDataset(connectionString, commandType, commandText, commandParameters);
        }
        public  DataSet ExecuteDataset(string spName, params object[] parameterValues)
        {
            return SqlHelper.ExecuteDataset(connectionString, spName, parameterValues);
        }

        //#endregion ExecuteDataSet

        //#region ExecuteNonQuery

        public  int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteNonQuery(connectionString, commandType, commandText);
        }
        public  int ExecuteNonQuery(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            return SqlHelper.ExecuteNonQuery(connectionString, commandType, commandText, commandParameters);
        }
        public   int ExecuteNonQuery(string spName, params object[] parameterValues)
        {
            return SqlHelper.ExecuteNonQuery(connectionString, spName, parameterValues);
        }

        //#endregion ExecuteNonQuery

        //#region ExecuteScalar
        public  object ExecuteScalar(CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteScalar(connectionString, commandType, commandText);
        }
        public  object ExecuteScalar(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            return SqlHelper.ExecuteScalar(connectionString, commandType, commandText, commandParameters);
        }
        public  object ExecuteScalar(string spName, params object[] parameterValues)
        {
            return SqlHelper.ExecuteScalar(connectionString, spName, parameterValues);
        }

        //#endregion ExecuteScalar

        public  int ExecuteNonQueryWithOutvalue(string spName, SqlParameter [] parameterValues)
        {

            return SqlHelper.ExecuteNonQueryWithOutvalue(connectionString, spName, parameterValues);

        }


    }
}
