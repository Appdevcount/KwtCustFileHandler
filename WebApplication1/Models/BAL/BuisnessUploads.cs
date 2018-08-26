using System.Security.Cryptography;
using WebApplication1.Models.DAL;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
namespace WebApplication1.Models.BAL
{

    public class BuisnessUploads
    {
        //   DataSet ds = new DataSet();

        DataSet ds =null;




        //  SQLHelper sqlHelp = new SQLHelper();
        SQLHelper sqlHelp = null;

        public DataSet GetTokenvalue(string ConnectionString, string procedurename, SqlParameter[] parameter)
        {
            ds = new DataSet(); 
            using (sqlHelp = new SQLHelper())
            {
                // sqlHelp= new SQLHelper();
                var command = procedurename.Contains(',') ? CommandType.Text : CommandType.StoredProcedure;
                //   ds = SQLHelper.Returndataset(ConnectionString, CommandType.StoredProcedure, procedurename, parameter);

                ds = sqlHelp.Returndataset(ConnectionString, command, procedurename, parameter);


                sqlHelp
                    = null;
            }
            return ds;


        }

        public string DecryptToken(string EncryptedToken)
        {

            //  sqlHelp = new SQLHelper(); //Use Using
            //   return sqlHelp.DecryptToken(EncryptedToken);
           // string token = "";
            using (sqlHelp = new SQLHelper())
            {
                return sqlHelp.DecryptToken(EncryptedToken);
            }
           
           // return token;

        }

        public string GetPrimaryKey(string TableName, string strConnString)
        {
            //   sqlHelp = new SQLHelper(); //use using
            //  var command = procedurename.Contains(',') ? CommandType.Text : CommandType.StoredProcedure;
            using (sqlHelp = new SQLHelper())
            {
                return sqlHelp.GetPrimaryKey(TableName, strConnString);
            }
            }
        
        public int ExecuteNonQuery(string ConnectionString, string procedurename, SqlParameter[] parameter)
        {
            using (sqlHelp = new SQLHelper())
            {
                // sqlHelp = new SQLHelper();
                var command = procedurename.Contains('(') ? CommandType.Text : CommandType.StoredProcedure;
                int x = sqlHelp.ExecuteNonQuery(ConnectionString, command, procedurename, parameter);

                // int x=SQLHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, procedurename, parameter);
                sqlHelp = null;
                return x;
            }
          
        }



        public DataSet ExecuteDataset(string ConnectionString, string spName, SqlParameter[] parameterValues)
        {
            ds = new DataSet();
            using (sqlHelp = new SQLHelper())
            { 
                ds = sqlHelp.Returndataset(ConnectionString, CommandType.StoredProcedure, spName, parameterValues);
            sqlHelp = null;
            }
            return ds;
        }


    }
}