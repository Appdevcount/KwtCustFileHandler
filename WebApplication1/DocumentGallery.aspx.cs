using System.Runtime.Remoting.Messaging;
using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using Resources;
using WebApplication1.Models.BAL;
using WebApplication1.Models.DAL;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using WebApplication1.Models;

namespace WebApplication1
{
    public partial class DocumentGallery : System.Web.UI.Page
    {
        //mydeclaration
        #region variables
        public string mysessionId = string.Empty;
        public string EncodedToken = string.Empty;
        static readonly string PasswordHash = "P@@Sw0rd";
        static readonly string SaltKey = "S@LT&KEY";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";
        static string additionaldocidforreview ="";
        static string DecIdforreview = "";
        // BuisnessUploads B_upload = new BuisnessUploads();


        BuisnessUploads B_upload;

        DataTable DTJSON = new DataTable();
        DataSet ds = new DataSet();



        string[] queryParamArray = new string[] { };
        string DcryptString = "";
        public string UploadedFrom = string.Empty;
        public string languageid = string.Empty;
        public string declarationDocumenttype = string.Empty;
        public string hidRefProfile = string.Empty;
        public string TablePrimaryKey = string.Empty;
        public string tokenvalue = string.Empty;
        public string sSLUN = string.Empty;
        public string sSLD = string.Empty;
        public string sSLP = string.Empty;
        public string sSFP = string.Empty;
        public string flagForRecords = string.Empty;
        //   public string TypeOfFilter = string.Empty;


        public string ThemeId = string.Empty;

        public string StateId = string.Empty;
        public string ProfileName = string.Empty;

        public string Tokensalt = string.Empty;
        public string mytokenvalue = string.Empty;

        public int Ownerorgid = 0;

        public int Ownerlocid = 0;

        //public string Ownerorgid = string.Empty;
        //public string Ownerlocid = string.Empty;

        public string CreatedBy = string.Empty;

        public string DateCreated = string.Empty;
        public string DateModified = string.Empty;

        public string ModifiedBy = string.Empty;


        public string referredUrl = string.Empty;
        public string pageId = string.Empty;

        //public bool launchFlagFrReview = false;


        //public bool launchFlagFrReject = false;
        //public bool launchFlagFrApprove = false;
        //public bool launchFlagFrItemAssociation = false;


        public string AdditionalDocumentFlag = "";
        public string AdditionalDocumentId = "";


        //switch for enabling/disabling review status
        //  public bool bEnableReviewStatus = false;

        public bool bEnableReviewStatus = false;
        public bool bEnableRejectStatus = false;
        public bool bEnableApproveStatus = false;
        public bool bEnableItemAssociationStatus = false;

        public bool bEnableDeletedRecordsStatus = false;
        string DeclarationId;
        static string constr = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
        String strConnString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ToString();
        public string lang = "en";
        public string uploadedDocumentsSet = "";

        public bool bSaved = false;
        public string errMsg = "";
        private static Hashtable ServedJobs = new Hashtable();
        private static string jobId = "";
    
        string lang1 = "";
        #endregion
        ErrorLogger Elog;
      
        protected ResourceManager LocRM;

        protected override void InitializeCulture()
        {
            Elog = new ErrorLogger();
            //  to be enabled for security testing 
            /*  Uri myReferrer = Request.UrlReferrer;
              if (myReferrer == null)
              {

                  Response.Redirect("DocumentError.aspx");

              }
           */
            B_upload = new BuisnessUploads();
             referredUrl = Request.RawUrl.ToString();

            Elog.WriteToLogFile("The First Request From Documents Gallery  For Url Verification ", "the requested Raw Url'"+referredUrl+"' ");

            Session["referredUrl"] = referredUrl.TrimStart('/').ToString();
            
            tokenvalue = Request.QueryString["tokenvalue"];
            EncodedToken = tokenvalue;
            if(tokenvalue!=null)
            {
                {

                    try
                    { 

                    DcryptString = B_upload.DecryptToken(tokenvalue);
                    queryParamArray = DcryptString.Split('+');
                    Session["mysessionId"] = queryParamArray[1];
                    Session["mytokenvalue"] = queryParamArray[2];
                    mytokenvalue = Session["mytokenvalue"].ToString().Split('|')[0];
                    Tokensalt =    Session["mytokenvalue"].ToString().Split('|')[1];
                    mysessionId =  Session["mysessionId"].ToString();

                    SqlParameter[] commandParameters1 = new SqlParameter[3];
                    commandParameters1[0] = new SqlParameter();
                    commandParameters1[0].ParameterName = "@tokenval";
                    commandParameters1[0].Value = mytokenvalue.ToString().Split('|')[0];
                    // commandParameters1[0].Value = "512a15ed44064189b440a9293096b4c6";
                    commandParameters1[1] = new SqlParameter();
                    commandParameters1[1].ParameterName = "@sessionId";
                    commandParameters1[1].Value = Session["mysessionId"].ToString();
                  //   commandParameters1[1].Value = "bajbgc15c3aa4rxxahnkmm25";
                    commandParameters1[2] = new SqlParameter();
                    commandParameters1[2].ParameterName = "@Tokensalt";
                    commandParameters1[2].Value = Tokensalt;
                    ds = B_upload.GetTokenvalue(strConnString, "usp_GetTokenInfo_DocUpload", commandParameters1);
                    if (ds.Tables.Count != 0)
                    {
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                                try
                                {
                                    languageid = ds.Tables[0].Rows[0]["LanguageId"].ToString();
                                    UploadedFrom = ds.Tables[0].Rows[0]["ReferenceProfile"].ToString();
                                    declarationDocumenttype = ds.Tables[0].Rows[0]["DocumentId"].ToString();
                                    hidRefProfile = ds.Tables[0].Rows[0]["ReferenceProfile"].ToString();
                                    ProfileName = ds.Tables[0].Rows[0]["profileName"].ToString();
                                    DeclarationId = ds.Tables[0].Rows[0]["ReferenceId"].ToString();
                                    pageId = ds.Tables[0].Rows[0]["pageId"].ToString();
                                    TablePrimaryKey = B_upload.GetPrimaryKey(ProfileName, strConnString);
                                    Ownerlocid = Convert.ToInt32(ds.Tables[0].Rows[0]["ownerlocid"]);
                                    Ownerorgid = Convert.ToInt32(ds.Tables[0].Rows[0]["OwnerOrgId"]);
                                    ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                                    CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                                    Session["profileName"] = ProfileName.ToString();
                                    Session["DeclarationId"] = DeclarationId.ToString();
                                    Session["hidRefProfile"] = hidRefProfile.ToString();
                                    lang1 = ds.Tables[0].Rows[0]["LanguageId"].ToString();
                                    AdditionalDocumentFlag= ds.Tables[0].Rows[0]["AdditionalDocumentFlag"].ToString();
                                    AdditionalDocumentId = ds.Tables[0].Rows[0]["AdditionalDocumentId"].ToString();
                                    additionaldocidforreview= ds.Tables[0].Rows[0]["AdditionalDocumentId"].ToString();
                                    DecIdforreview = DeclarationId.ToString();
                                    ThemeId = ds.Tables[0].Rows[0]["ThemeId"].ToString();

                                   
                                }
                                catch (Exception ex)
                                {
                                    Elog = new ErrorLogger();
                                    //     WriteToLogFile(ex, "protected override void InitializeCulture()");
                                    Elog.WriteToLogFile(ex, "", " from documents gallery  protected override void InitializeCulture() in value of usp_GetTokenInfo_DocUpload  Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");
                                    Elog = null;
                                }
                            }
                      
                    }
                        else
                        {
                            Elog = new ErrorLogger();
                            //   WriteToLogFile("No Value Found For in Sp :usp_GetTokenInfo_DocUpload '" + mytokenvalue + "'");
                            Elog.WriteToLogFile( "", " from documents gallery protected override void InitializeCulture() in value of usp_GetTokenInfo_DocUpload  Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");
                            Elog = null;
                            Response.Redirect("DocumentError.aspx");


                        }
                    }


                    catch (Exception ex)
                    {
                        Elog = new ErrorLogger();
                        Elog.WriteToLogFile(ex,"", " from documents gallery protected override void InitializeCulture()   Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");
                        Elog = null;
                        //  WriteToLogFile(ex);
                    }
                }
                B_upload = null;
            }
       
            else
            {
                Elog = new ErrorLogger();
                Elog.WriteToLogFile( "", " from documents gallery Null Token Recived   Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");
                Elog = null;
               // WriteToLogFile("Null Token Recived  '" + tokenvalue + "'");
                Response.Redirect("DocumentError.aspx");
            }
            // bEnableReviewStatus = checkLaunchFlag();

             checkLaunchFlag();
            //bEnableReviewStatus = Convert.ToBoolean(launchFlagFrReview);
            //bEnableRejectStatus = Convert.ToBoolean(launchFlagFrReject);
            //bEnableApproveStatus = Convert.ToBoolean(launchFlagFrApprove);
            //bEnableItemAssociationStatus = Convert.ToBoolean(launchFlagFrItemAssociation);


            //   bEnableReviewStatus = true;
            if (!bEnableReviewStatus)
            { 
          //  Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('Not allowed Save Functionality')",true);
            }
            if (!string.IsNullOrEmpty(lang1))
                lang = lang1.ToString().ToLower().Substring(0, 2);
            this.UICulture = lang;
            LocRM = captions.ResourceManager;
           base.InitializeCulture();
        }
        protected void BilingualSupport()
        {
            //SaveBtn.Text = LocRM.GetString("Save");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(lang=="ar")
            { 

            LabelSelectedFilterDisplay.Text = labelText.Value==""? "جميع المستندات" : labelText.Value;
            }
            else
            {
                LabelSelectedFilterDisplay.Text = labelText.Value == "" ? "All Documents" : labelText.Value;


            }
            if (DeleteFlagTest.Value== "ScanRequestUploadDocsCreated"|| DeleteFlagTest.Value == "")
            { 
            flagForRecords = "ScanRequestUploadDocsCreated";
            }
            else
            {
                flagForRecords = DeleteFlagTest.Value;
                bEnableReviewStatus = false;
                bEnableRejectStatus = false;
                bEnableApproveStatus = false;
                bEnableItemAssociationStatus = false;
            }

            //   LanguageForAsso.Text = languageid.ToString();
            jobId = Guid.NewGuid().ToString();
            BilingualSupport();
            SaveBtn.Visible = bEnableReviewStatus;
           // if (!IsPostBack)
         //   {
              //  InitiatePage();
          //  }
          //  else
          //  {
                // string s = DeleteFlagTest.Value;
                //  var values = Request.Form["chkMarks"].ToString();
                
                        InitiatePage();
           
           // }
        }

        protected void InitiatePage()
        {
            try
            {

                uploadedDocumentsSet = LoadData();

                string s = TypeOfFilter.Value;
                if (uploadedDocumentsSet=="")
                {

                    Response.Redirect("DocumentEmpty.aspx?TypeOfFilter=" + TypeOfFilter.Value + "");
                }
                else
                {


                }

            }
            catch (Exception ex)
            {
                Response.Clear();

               // Response.Write(ex.Message);
                Response.End();
                Elog = new ErrorLogger();
                //  WriteToLogFile(ex);
                Elog.WriteToLogFile(ex,"", " from doc gallery Null Token Recived   Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");
                Elog = null;
                Response.Redirect("DocumentError.aspx");
                //Response.Redirect("~/Errorpage.aspx");


            }
        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            txtJobId.Text = jobId;
        }

     
        private string LoadData()
        {
            B_upload = new BuisnessUploads();

            DataSet dtjson1 = new DataSet();
            DataTable dtjsondata = null;
            try
            {
                // string sx = DeleteFlag.Text;
                //String constr =
                SqlParameter[] commandParameters = new SqlParameter[11];
                commandParameters[0] = new SqlParameter();
                commandParameters[0].ParameterName = "@ReferenceId";
                commandParameters[0].Value = DeclarationId;
                commandParameters[1] = new SqlParameter();
                commandParameters[1].ParameterName = "@pageId";
                commandParameters[1].Value = TablePrimaryKey;
                commandParameters[2] = new SqlParameter();
                commandParameters[2].ParameterName = "@ProfileName";
                commandParameters[2].Value = ProfileName;

                commandParameters[3] = new SqlParameter();
                commandParameters[3].ParameterName = "@langId";
                commandParameters[3].Value = lang;

                GetDirection.Text = lang;

                commandParameters[4] = new SqlParameter();
                commandParameters[4].ParameterName = "@DocumentId";
                commandParameters[4].Value = "";
                // ref profile addition for crf 

                commandParameters[5] = new SqlParameter();
                commandParameters[5].ParameterName = "@RefProfile";
                commandParameters[5].Value = hidRefProfile;
                // for additional documents request 
                commandParameters[6] = new SqlParameter();
                commandParameters[6].ParameterName = "@tokenvalue";
                commandParameters[6].Value = mytokenvalue;

                commandParameters[7] = new SqlParameter();
                commandParameters[7].ParameterName = "@AdditionalDocRequestId";
                commandParameters[7].Value = AdditionalDocumentId;

                commandParameters[8] = new SqlParameter();
                commandParameters[8].ParameterName = "@DeletedRecordsFlag";
                commandParameters[8].Value = flagForRecords;

                commandParameters[9] = new SqlParameter();
                commandParameters[9].ParameterName = "@TypeOfFilter";
                commandParameters[9].Value = TypeOfFilter.Value;


                commandParameters[10] = new SqlParameter();
                commandParameters[10].ParameterName = "@labelText";
                commandParameters[10].Value = labelText.Value;

                var s = declarationDocumenttype;


                ds = B_upload.GetTokenvalue(strConnString, "Sp_GetPathValues", commandParameters);
                 dtjsondata = ds.Tables[0];
                if (ds.Tables[0].Rows.Count != 0)
                {


                    string strFileNameIds = "";
                    string sGuid = "";
                    if (dtjsondata.Rows.Count > 0)
                    {
                        sGuid = System.Guid.NewGuid().ToString("N");
                        for (int k = 0; k < dtjsondata.Rows.Count; k++)
                        {
                            if (strFileNameIds == "")
                                strFileNameIds = Encrypt(dtjsondata.Rows[k]["FileNameId"].ToString());
                            else
                                strFileNameIds = strFileNameIds + "," + Encrypt(dtjsondata.Rows[k]["FileNameId"].ToString());
                        }

                        SqlParameter[] commandParameters1 = new SqlParameter[2];
                        commandParameters1[0] = new SqlParameter("@sGuid", SqlDbType.VarChar);
                        commandParameters1[0].Value = sGuid;
                        commandParameters1[1] = new SqlParameter("@strFileNameIds", SqlDbType.VarChar);
                        commandParameters1[1].Value = strFileNameIds;

                        int InsertResult = B_upload.ExecuteNonQuery(strConnString, "sp_ViewerFileSecurityToken", commandParameters1);
                    }
                }

                else
                {
                    //  Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('No Documents found ')",true);


                }
            }
            catch (Exception ex)
            {
                Elog = new ErrorLogger();
                Elog.WriteToLogFile(ex, "", " from doc gallery LoadData  Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");
                Elog = null;

            }
            B_upload = null;
            return DataTableToJSONWithStringBuilder(dtjsondata);

        }

        private string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            try
            {
                if (table.Rows.Count > 0)
                {
                    JSONString.Append("[\n");
                    string CDocType = "";
                    bool isCtypechnged = true;
                    int isCtypechngedindx = 0;
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        if (isCtypechnged)
                        {
                            isCtypechnged = false;
                            CDocType = table.Rows[i][0].ToString();
                            isCtypechngedindx = i;
                            if (i > 0) JSONString.Append(",");
                            JSONString.Append("\n{");
                            JSONString.Append("\"" + table.Columns[0].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][0].ToString() + "\"");
                            JSONString.Append(",\"uploadedDocuments\":[\n");
                        }
                        if (i != isCtypechngedindx) JSONString.Append(",");
                        JSONString.Append("\n{");
                        for (int j = 1; j < table.Columns.Count; j++)
                        {

                            if (j > 1) JSONString.Append(",\n");
                            if (table.Columns[j].ColumnName.ToString() == "FileNameId")
                            {
                                var s = table.Rows[i][j].ToString().Replace('+', '|').Trim();
                                var y = Encrypt(s);
                                var z = System.Web.HttpUtility.UrlEncode(y);
                                if (y.Contains(" "))
                                {

                                }
                                JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + z.Replace("\\", "//") + "\"");

                            }
                            else
                            {
                                if (table.Columns[j].ColumnName.ToString() == "path")
                                {
                                    JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + Encrypt(table.Rows[i][j].ToString().Replace("\\", "//")) + "\"");

                                }
                                else
                                {
                                    JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString().Replace("\\", "//") + "\"");

                                }

                            }
                        }
                        JSONString.Append("}");
                        if (i + 1 >= table.Rows.Count || CDocType != table.Rows[i + 1][0].ToString())
                        {
                            JSONString.Append("\n]\n}");
                            isCtypechnged = true;
                        }
                    }
                    JSONString.Append("\n]");
                }
            }
            catch(Exception ex)
            {
                Elog = new ErrorLogger();
                Elog.WriteToLogFile(ex, "", " from doc gallery DataTableToJSONWithStringBuilder  Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");
                Elog = null;
            }
            return JSONString.ToString();
        }


        public string Decrypt(string encryptedText)
        {
            string x = "";
            try
            {
                byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
                byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
                var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

                var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
                var memoryStream = new MemoryStream(cipherTextBytes);
                var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                x = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
            }
            catch (Exception ex)
            {
                // WriteToLogFile(ex);
                Elog = new ErrorLogger();
                Elog.WriteToLogFile(ex,"theEncrypted Text"+ encryptedText, " from doc gallery Decrypt  Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");
                Elog = null;
                //  WriteToLogFile(e);
            }
            return x;
        }

        public  string Encrypt(string plainText)
        {

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                 
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }


            return Convert.ToBase64String(cipherTextBytes);
        }

        private void UpdateReviewedStatus()
        {
            ServedJobs.Add(txtJobId.Text, txtJobId.Text);
            StringBuilder sb = new StringBuilder();
            string[] tempdocid = HttpUtility.UrlDecode(txtArrayIdx.Text).Split(',');
            foreach(var item in tempdocid)
            {
                sb.Append(Decrypt(item.ToString()) + ',');
            }


            string[] arrDocIdx = sb.ToString().TrimEnd(',').Split(',') ;
            string[] arrDocReviewStatus = txtArrayStatus.Text.Split(',');
            DataTable dt = new DataTable("DocReviewStatus");
            dt.Columns.Add("DocumentId", typeof(System.Int64));
            dt.Columns.Add("ReviewStatus", typeof(System.Int16));
            dt.Columns.Add("Createddate", typeof(System.String));
            dt.Columns.Add("Profilename", typeof(System.String));
            dt.Columns.Add("StateId", typeof(System.String));
            dt.Columns.Add("CreatedBy", typeof(System.String));
            dt.Columns.Add("OwnerLocId", typeof(System.Int64));
            dt.Columns.Add("OwnerOrgId", typeof(System.Int64));
            dt.Columns.Add("DateModified", typeof(System.String));
            dt.Columns.Add("ModifiedBy", typeof(System.String));


            //   dt.Columns.Add("CreatedBy", typeof(System.Int16));

       
            

            for (int i = 0; i < arrDocIdx.Length; i++)
            {
                dt.Rows.Add(new object[10] { arrDocIdx[i], arrDocReviewStatus[i],DateCreated,ProfileName, StateId, CreatedBy, Ownerlocid, Ownerorgid,"","" });
            }

            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dt);

            //store / update table

            int Success = Save(dt);
            if (Success > 0)
            {
                txtArrayIdx.Text = "";
                txtArrayStatus.Text = "";
                bSaved = true;
            }
            else
            {
                bSaved = false;
                errMsg = "Retry";
            }
        }

        private static int Save(DataTable dataTable)
        {
            int success = 0;
        
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Sp_DocReviewStatus"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@tableDocuments", dataTable);
                    cmd.Parameters.AddWithValue("@AdditionalDocRequestId", additionaldocidforreview);
                    cmd.Parameters.AddWithValue("@Declarationid", DecIdforreview);

                    con.Open();
                    success = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
      
           
            return success;
        }

     //public  Boolean checkLaunchFlag()
     //   {

     //       Boolean launchFlag = false;
     //       B_upload = new BuisnessUploads();

     //           SqlParameter[] commandParameters1 = new SqlParameter[2];
     //       commandParameters1[0] = new SqlParameter();
     //       commandParameters1[0].ParameterName = "@ProfileName";
     //       commandParameters1[0].Value = ProfileName;

     //       commandParameters1[1] = new SqlParameter();
     //       commandParameters1[1].ParameterName = "@OwnerLocId";
     //       commandParameters1[1].Value = Ownerlocid;

     //       ds = B_upload.GetTokenvalue(strConnString, "usp_GetUploadConfigvalue", commandParameters1);
     //       if (ds != null && ds.Tables[0].Rows.Count > 0)
     //       {


     //           //  DocumentId,ReviewStatus,ProfileName,StateId,CreatedBy,OwnerLocId,OwnerOrgId,DateCreated,DateModified,ModifiedBy)
     //           launchFlag = Convert.ToBoolean(ds.Tables[0].Rows[0]["ReviewFunctionality"]);


     //       }
     //       return launchFlag;


     //   }


public void  checkLaunchFlag()
{


    B_upload = new BuisnessUploads();
    SqlParameter[] commandParameters1 = new SqlParameter[3];
    commandParameters1[0] = new SqlParameter();
    commandParameters1[0].ParameterName = "@ProfileName";
    commandParameters1[0].Value = ProfileName;

    commandParameters1[1] = new SqlParameter();
    commandParameters1[1].ParameterName = "@OwnerLocId";
    commandParameters1[1].Value = Ownerlocid;

    commandParameters1[2] = new SqlParameter();
    commandParameters1[2].ParameterName = "@hidrefprofile";
    commandParameters1[2].Value = hidRefProfile;

            ds = B_upload.GetTokenvalue(strConnString, "usp_GetUploadConfigvalue", commandParameters1);
    if (ds != null && ds.Tables[0].Rows.Count > 0)
    {
                bEnableReviewStatus = Convert.ToBoolean(ds.Tables[0].Rows[0]["ReviewFunctionality"]);
                bEnableApproveStatus = Convert.ToBoolean(ds.Tables[0].Rows[0]["ApproveFunctionality"]);
                bEnableRejectStatus = Convert.ToBoolean(ds.Tables[0].Rows[0]["RejectFunctionality"]);
                bEnableItemAssociationStatus = Convert.ToBoolean(ds.Tables[0].Rows[0]["ItemAssocitaionFlag"]);
                bEnableDeletedRecordsStatus = Convert.ToBoolean(ds.Tables[0].Rows[0]["ViewDeletedFunctionality"]);
            }
            // return launchFlag;
        }

        protected void SaveBtn_Click(object sender, ImageClickEventArgs e)
        {
            if (!ServedJobs.Contains(txtJobId.Text))
            {
                try
                {
                    UpdateReviewedStatus();

                }
                catch (Exception ex)
                {
                    bSaved = false;
                    errMsg = Server.UrlEncode(ex.Message);
                    Elog = new ErrorLogger();
                    //   WriteToLogFile( ex);
                    Elog.WriteToLogFile(ex, "", " from doc gallery SaveButton  Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");
                    Elog = null;
                    Response.Redirect("DocumentError.aspx");
                }
            }

            InitiatePage();

        }
        #region commented code 
        //public void WriteToLogFile(Exception ex)
        //{
        //    using (EventLog eventLog = new EventLog("Application"))
        //    {
        //        eventLog.Source = "Application";
        //        eventLog.WriteEntry(" from DocumentsGallery  (referredUrl='" + referredUrl + "') encodedToken= '" + EncodedToken + "' and tokenValue='" + mytokenvalue + "' and sessionID='" + mysessionId + "'and tokensalt='" + Tokensalt + "' " + ex.Message + ex.StackTrace, EventLogEntryType.Information, 101, 1);
        //    }
        //}


        //public void WriteToLogFile(string errormessage)
        //{
        //    using (EventLog eventLog = new EventLog("Application"))
        //    {
        //        eventLog.Source = "Application";
        //        eventLog.WriteEntry(" from DocumentsGallery  (referredUrl='" + referredUrl + "')   encodedToken= '" + EncodedToken + "' and tokenValue='" + mytokenvalue + "' and sessionID='" + mysessionId + "'and tokensalt='" + Tokensalt + "' " + errormessage, EventLogEntryType.Information, 101, 1);
        //    }
        //}
#endregion

        //public void WriteToLogFile(Exception ex)
        //{
        //    using (EventLog eventLog = new EventLog("Application"))
        //    {
        //        eventLog.Source = "Application";
        //        eventLog.WriteEntry(" from DocumentsGallery  (referredUrl='" + Session["referredUrl"].ToString() + "')    and tokenValue='" + Session["mytokenvalue"].ToString() + "' and sessionID='" + Session["mysessionId"].ToString() + "'and tokensalt='" + Tokensalt + "' " + ex.Message + ex.StackTrace, EventLogEntryType.Information, 101, 1);
        //    }
        //}


        //public void WriteToLogFile(string errormessage)
        //{
        //    using (EventLog eventLog = new EventLog("Application"))
        //    {
        //        eventLog.Source = "Application";
        //        eventLog.WriteEntry(" from DocumentsGallery  (referredUrl='" + Session["referredUrl"].ToString() + "')    and tokenValue='" + Session["mytokenvalue"].ToString() + "' and sessionID='" + Session["mysessionId"].ToString() + "'and tokensalt='" + Tokensalt + "' " + errormessage, EventLogEntryType.Information, 101, 1);
        //    }
        //}
        //public void WriteToLogFile(Exception ex, string inputvalue)
        //{
        //    using (EventLog eventLog = new EventLog("Application"))
        //    {
        //        eventLog.Source = "Application";
        //        eventLog.WriteEntry(" from DocumentsGallery Param Information=>'" + inputvalue + "' (referredUrl='" + Session["referredUrl"].ToString() + "')    and tokenValue='" + Session["mytokenvalue"].ToString() + "' and sessionID='" + Session["mysessionId"].ToString() + "'and tokensalt='" + Tokensalt + "' " + ex.Message + ex.StackTrace, EventLogEntryType.Information, 101, 1);
        //    }

        //}
        protected void ButtonDetails_Click(object sender, EventArgs e)
        {

        }

       

    }

}