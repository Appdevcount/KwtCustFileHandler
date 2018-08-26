using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.SessionState;
//using TokenGeneration;
using System.Data.SqlClient;
using System.Data;
using WebApplication1.Models.BAL;
using WebApplication1.Models.DAL;
using System.Configuration;
using System.Diagnostics;
using TokenGeneration;
namespace WebApplication1
{
    public partial class TestSession : System.Web.UI.Page
    {
        String strConnString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ToString();
        BuisnessUploads B_upload = new BuisnessUploads();
        int InsertResult = 22;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)



        {

            Response.Redirect("http://10.10.26.226/ScanUploadDocument/home/tokenvalue?TokenValue=dlpih2aa%2bL%2fInOuXvwU6HbXdq%2bPvrCtwutZNHWpxCwZa1eD5s7tq7C2IrnAml5yKPugfLhrEEbAeW6yIdTSVD3mUoyUbWD8xafwm1pLUBlQ%3d");
            //String strConnString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ToString();


            //GenerateToken T = new GenerateToken();
        
            //string encodedToken = T.InsertTokenDetails(2133432313, 8352265, "CRFDetails", "2018-01-02 16:55:30.500", "Form",
            //     "MCDocumentUploadTokensCreatedState", "broker.kwi", "broker.kwi", 8998, 44676, "", "ScanRequestUploadDocs",
            //     "yes", "ClearingAgent", 332293951, 0, "CurrencyReportFormFrPg", "2018-01-02 17:15:30.500", "yes", "cntg3die22zdx535mxcskuos", "", "AzharReciept", "8454", strConnString);



        }
        //public int InsertTokenDetails(long tokenid, int referenceid, string referenceProfile, string tokendate, string mode, string StateId,
        //   string createdby, string modifiedby, int ownerlocid, int ownerorgid, string datemodified, string profilename
        //   , string isbroker, string roleid, int documentid, int ScanDocRequestId, string PageId)
        //{

        //    try
        //    {
        //        string token = System.Guid.NewGuid().ToString("N");

        //        SqlParameter[] commandParameters = new SqlParameter[18];
        //        commandParameters[0] = new SqlParameter("@tokenid", SqlDbType.BigInt, 8000);
        //        commandParameters[0].Value = tokenid;

        //        commandParameters[1] = new SqlParameter("@referenceid", SqlDbType.Int, 100);
        //        commandParameters[1].Value = referenceid;

        //        commandParameters[2] = new SqlParameter("@referenceProfile", SqlDbType.VarChar, 50);
        //        commandParameters[2].Value = referenceProfile;


        //        commandParameters[3] = new SqlParameter("@tokendate", SqlDbType.VarChar, 50);
        //        commandParameters[3].Value = tokendate;

        //        commandParameters[4] = new SqlParameter("@mode", SqlDbType.VarChar, 50);
        //        commandParameters[4].Value = mode;


        //        commandParameters[5] = new SqlParameter("@StateId", SqlDbType.VarChar, 50);
        //        commandParameters[5].Value = StateId;
        //        commandParameters[6] = new SqlParameter("@createdby", SqlDbType.VarChar, 50);
        //        commandParameters[6].Value = createdby;



        //        commandParameters[7] = new SqlParameter("@modifiedby", SqlDbType.VarChar, 50);
        //        commandParameters[7].Value = modifiedby;

        //        commandParameters[8] = new SqlParameter("@ownerlocid", SqlDbType.Int, 50);
        //        commandParameters[8].Value = ownerlocid;

        //        commandParameters[9] = new SqlParameter("@ownerorgid", SqlDbType.Int, 50);
        //        commandParameters[9].Value = ownerorgid;

        //        commandParameters[10] = new SqlParameter("@datemodified", SqlDbType.VarChar, 50);
        //        commandParameters[10].Value = datemodified;





        //        commandParameters[11] = new SqlParameter("@isbroker", SqlDbType.VarChar, 50);
        //        commandParameters[11].Value = isbroker;

        //        commandParameters[12] = new SqlParameter("@roleid", SqlDbType.VarChar, 50);
        //        commandParameters[12].Value = roleid;

        //        commandParameters[13] = new SqlParameter("@documentid", SqlDbType.Int, 50);
        //        commandParameters[13].Value = documentid;

        //        commandParameters[14] = new SqlParameter("@ScanDocRequestId", SqlDbType.Int, 50);
        //        commandParameters[14].Value = ScanDocRequestId;


        //        commandParameters[15] = new SqlParameter("@PageId", SqlDbType.VarChar, 50);
        //        commandParameters[15].Value = isbroker;

        //        commandParameters[16] = new SqlParameter("@token", SqlDbType.VarChar, 50);
        //        commandParameters[16].Value = token;


        //        commandParameters[17] = new SqlParameter("@profilename", SqlDbType.VarChar, 50);
        //        commandParameters[17].Value = profilename;






        //        int InsertResult = B_upload.ExecuteNonQuery(strConnString, "USP_InsertintoMcDocUploadToken", commandParameters);
        //    }

        //    catch (Exception ex)
        //    {




        //    }


        //    return InsertResult;


        //}


    }
}