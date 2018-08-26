using WebApplication1.Models.BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Text;

namespace WebApplication1.Models.DAL
{
    public class  SQLHelper : IDisposable
    {
        private SqlConnection con;
        private SqlCommand cmd;
        //private SqlDataAdapter da;
        // private  SQLHelper instance;
        private readonly object padlock = new object();
        private string connectionString;
        ErrorLogger elog;
        public void Dispose()
        {

        }


        public SQLHelper()
        {

            connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

            //
            // TODO: Add constructor logic here
            //
        }
        //public SQLHelper Instance
        //{
        //    get
        //    {
        //        lock (padlock)
        //        {
        //            if (instance == null)
        //            {
        //                instance = new SQLHelper();
        //            }
        //            return instance;
        //        }
        //    }
        //}

        //dataset Helper with Sql helper 
        public DataSet Returndataset(string connectionString, CommandType commandType, string commandText, SqlParameter[] parameters)
        {
            StringBuilder sb = new StringBuilder();
            DataSet ds = new DataSet();
            try
            {
              
                using (con = new SqlConnection(connectionString))
                {
                    cmd = new SqlCommand(commandText, con);
                    cmd.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (SqlParameter p in parameters)
                        {
                            if (p.Value == null)
                            {
                            }
                            cmd.Parameters.Add(p);
                            sb.Append(',' + p.ParameterName+">" +p.Value.ToString());
                        }
                    }
                    con.Open();
                    SqlDataAdapter sda1 = new SqlDataAdapter(cmd);

                    sda1.Fill(ds, "FileAndTokenInfo");
                    cmd.Parameters.Clear();
                 
                }
           
            }
          
            catch (Exception ex)

            {
                elog = new ErrorLogger();
                // WriteToLogFile(ex, connectionString + sb);
                //  throw new ArgumentException(ex.Message);
                elog.WriteToLogFile(ex, connectionString, "from sql helper"+sb.ToString());
                elog = null;
            }
            finally
            {
                con.Close();
            }
            return ds;
        }



        public int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, SqlParameter[] parameters)
        {
            int x = 0;
            try
            {
                DataSet ds = new DataSet();
                using (con = new SqlConnection(connectionString))
                {
                    cmd = new SqlCommand(commandText, con);
                    cmd.CommandType = commandType;
                    cmd.Parameters.Clear();
                    foreach (SqlParameter ParamValue in parameters)
                    {
                        if (ParamValue.Value == null || ParamValue == null)
                        {
                        }
                        cmd.Parameters.Add(ParamValue);
                    }
                    con.Open();
                    x = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                }
            }
            catch (Exception ex)
            {
                elog = new ErrorLogger();
                elog.WriteToLogFile(ex, connectionString, "from sql helper");
                elog = null;
            }

            finally
            {
                con.Close();
            }
            return x;
        }

        private int ExecuteNonQueryWithOutvalue(string connectionString, string spName, SqlParameter[] parameterValues)
        {
            int result = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_MCPKCounters", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (parameterValues != null)
                        {
                            foreach (SqlParameter p in parameterValues)
                            {
                                cmd.Parameters.Add(p);
                            }
                        }

                        connection.Open();
                        result = cmd.ExecuteNonQuery();
                        connection.Close();
                    }

                }
            }
            catch(Exception ex)
            {
                elog = new ErrorLogger();
                elog.WriteToLogFile(ex, connectionString, "from sql helper");
                elog = null;
            }

            return result;
        }
        public int ExecuteNonQueryWithOutvalue(string spName, SqlParameter[] parameterValues)
        {
            return ExecuteNonQueryWithOutvalue(connectionString, spName, parameterValues);
        }
        //new keys
        public string GetNewIntKey(string sCounterName)
        {
            Int64 counterValueStart;
            Int64 counterValueEnd;
            GetNewIntCounter(sCounterName, 1, out counterValueStart, out counterValueEnd);
            return counterValueStart.ToString();
        }

        public Int64 GetNewIntCounter(string sCounterName, Int64 seedValue, out Int64 counterValueStart, out Int64 counterValueEnd)
        {
            //  sqlHelp = new SQLHelper();
         
                string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
                SqlParameter[] commandParameters = new SqlParameter[4];

                commandParameters[0] = new SqlParameter("@DataSourceName", SqlDbType.VarChar, 50);
                commandParameters[1] = new SqlParameter("@SeedValue", SqlDbType.BigInt);
                commandParameters[2] = new SqlParameter("@CounterValueStart", SqlDbType.BigInt);
                commandParameters[3] = new SqlParameter("@CounterValueEnd", SqlDbType.BigInt);
                commandParameters[2].Direction = ParameterDirection.Output;
                commandParameters[3].Direction = ParameterDirection.Output;
                commandParameters[0].Value = sCounterName;
                commandParameters[1].Value = seedValue;


            try { 
                ExecuteNonQueryWithOutvalue("usp_MCPKCounters", commandParameters);
            }
            catch(Exception ex)
            {
                elog = new ErrorLogger();
                elog.WriteToLogFile(ex," from counter input param='"+ sCounterName+"'","");
                elog = null;

            }
            //   sqlHelp = null;
            counterValueStart = (Int64)commandParameters[2].Value;
                counterValueEnd = (Int64)commandParameters[3].Value;
                return (Int64)commandParameters[2].Value;
            }
          
       // }














        public string DecryptToken(string EncryptedToken)
        {
            string DcryptString = "";
            try
            {
                CryptographyKGAC.CryptographyKGAC cryptographyKGACObj = new CryptographyKGAC.CryptographyKGAC();
                string queryParam = EncryptedToken;
                string DocManagementKey = System.Configuration.ConfigurationSettings.AppSettings["DocManagementSecurityKey"].ToString();
                byte[] DocManagementKeyBytes = Encoding.UTF8.GetBytes(DocManagementKey);
                byte[] sNewTokenBytes = Convert.FromBase64String(queryParam);
                DcryptString = cryptographyKGACObj.DecryptStringFromBytes(sNewTokenBytes, DocManagementKeyBytes, DocManagementKeyBytes);

            }
            catch (Exception ex)
            {
                elog = new ErrorLogger();
                elog.WriteToLogFile(ex, " from DecryptToken sql helper Encrypted Token='" + EncryptedToken + "'", "");
                elog = null;
                // WriteToLogFile(ex);
                return "false";
            }

            return DcryptString;
        }
        public string GetPrimaryKey(string TableName, string connectionString)
        {
            string primarykey = string.Empty;
            try
            { 
        
            SqlParameter[] commandParameters = new SqlParameter[1];

            commandParameters[0] = new SqlParameter("@Tablename", SqlDbType.VarChar, 150);
            commandParameters[0].Value = TableName;
            DataSet ds = new DataSet();


            ds = Returndataset(connectionString, CommandType.StoredProcedure, "Sp_GetPrimaryKey", commandParameters);


            if (ds.Tables[0].Rows.Count > 0)
                primarykey = ds.Tables[0].Rows[0]["column_name"].ToString();
            }
            catch(Exception ex)
            {
                elog = new ErrorLogger();
                //    WriteToLogFile(ex, TableName);
                elog.WriteToLogFile(ex, " from GetPrimaryKey sql helper TableName  ='" + TableName + "'", "");
                elog = null;
            }
            return primarykey;
        }
    


    }
}