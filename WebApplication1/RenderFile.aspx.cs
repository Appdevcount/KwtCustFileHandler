using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using MicroClear.EnterpriseSolutions.CryptographyServices;
using MicroClear.EnterpriseSolutions.ServiceFactories;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using WebApplication1.Models.BAL;
using System.Security.Cryptography;
using WebApplication1.Models.DAL;
using System.Diagnostics;
using WebApplication1.Models;
namespace DocumentManagementServices
{
    public partial class RenderFile : System.Web.UI.Page
    {
        #region variables
        // BuisnessUploads B_upload = new BuisnessUploads();
        BuisnessUploads B_upload;

        public string ShareFolderPath = ConfigurationManager.AppSettings["SRDocumentsShareFolderPath"].ToString();//ShareFolderPath
        static string constr = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
        String strConnString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ToString();
        DataSet ds = new DataSet();
        static readonly string PasswordHash = "P@@Sw0rd";
        static readonly string SaltKey = "S@LT&KEY";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";
        public string sRefProfName = string.Empty;
        public string profilename = string.Empty;
        public string sSLUN = string.Empty;
        public string sSLD = string.Empty;
        public string sSLP = string.Empty;
        public string sSFP = string.Empty;

        ImpersonateUser iU = new ImpersonateUser();
        string sPwd = "";
        SymmetricEncryption CgServices = ServiceFactory.GetSymmetricEncryptionInstance();
        #endregion
        ErrorLogger Elog;
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
                Elog = new ErrorLogger();

                // WriteToLogFile(ex, "public string Decrypt(string encryptedText) In renderFile'"+encryptedText+"'");
                Elog.WriteToLogFile(ex, "public string Decrypt(string encryptedText) In renderFile'" + encryptedText + "'", "tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString()+"'  ");
                //  WriteToLogFile(e);
                Elog = null;
            }
            return x;
        }
        public void setConfigValues()
        {
            ds = new DataSet();
            B_upload = new BuisnessUploads();
            SqlParameter[] commandParameters1 = new SqlParameter[2];
            commandParameters1[0] = new SqlParameter();
            commandParameters1[0].ParameterName = "@ProfileName";
            commandParameters1[0].Value = Session["profileName"].ToString(); 
            commandParameters1[1] = new SqlParameter();
            commandParameters1[1].ParameterName = "@hidrefprofile";
            commandParameters1[1].Value = Session["hidrefprofile"].ToString(); 
            
            try
            { 
            ds = B_upload.GetTokenvalue(strConnString, "usp_GetUploadConfigvalue", commandParameters1);
            if (ds != null && ds.Tables["FileAndTokenInfo"].Rows.Count > 0)
            {
                sRefProfName = ds.Tables[0].Rows[0]["RefProfileName"].ToString();
                sSLUN = ds.Tables[0].Rows[0]["ShareLocationUserName"].ToString();
                sSLD = ds.Tables[0].Rows[0]["ShareLocationDomain"].ToString();
                sSLP = ds.Tables[0].Rows[0]["ShareLocationPassword"].ToString();
                sSFP = ds.Tables[0].Rows[0]["SRDocumentsShareFolderPath"].ToString();
                sPwd = ds.Tables[0].Rows[0]["SRDocumentsShareFolderPath"].ToString();
                ShareFolderPath = sSFP;
                sPwd = CgServices.DecryptData(sSLP);
            }
            }
            catch(Exception ex)
            {
                Elog = new ErrorLogger();
                Elog.WriteToLogFile(ex, "public setConfigValues In renderFile Profile name '" + Session["profileName"].ToString() + "'and hidprofile'" + Session["hidrefprofile"].ToString() + "'", "tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'  ");
                Elog = null;
            }
            B_upload = null;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            string totalpath = "";
            B_upload = new BuisnessUploads();
            if (Request.QueryString["UploadedFrom"] != null && Request.QueryString["UploadedFrom"].ToString() == "OrganizationRequests")
              ShareFolderPath = ConfigurationManager.AppSettings["ORDocumentsShareFolderPath"].ToString();
            if (Request.QueryString["Documentid"] != null)
            {
                try
                {
                    // Read the file and convert it to Byte Array
                    string filePath = ShareFolderPath;
                    string profilename = Session["profileName"].ToString();
                      string filename = "";
                 //   string red = System.Web.HttpUtility.UrlDecode(Request.QueryString["Documentid"].ToString());
                 //   string filenameid = Request.QueryString["Documentid"].ToString().Replace("'", string.Empty).Replace(" ","+");
                    string Documentid = Decrypt(Request.QueryString["Documentid"].ToString());
                    SqlParameter[] commandParameters = new SqlParameter[4];
                    commandParameters[0] = new SqlParameter();
                    commandParameters[0].ParameterName = "@Id";
                    commandParameters[0].Value = Documentid;
                    commandParameters[0].SqlDbType = SqlDbType.Int;
                    commandParameters[1] = new SqlParameter();
                    commandParameters[1].ParameterName = "@TablePrimaryKey";
                    commandParameters[1].Value = B_upload.GetPrimaryKey(profilename, strConnString);
                    commandParameters[2] = new SqlParameter();
                    commandParameters[2].ParameterName = "@ProfileName";
                    commandParameters[2].Value = profilename;
                    commandParameters[3] = new SqlParameter();
                    commandParameters[3].ParameterName = "@declarationid";
                    commandParameters[3].Value = Convert.ToInt32(Session["DeclarationId"]);
                    commandParameters[3].SqlDbType = SqlDbType.Int;
             
                    ds = B_upload.GetTokenvalue(strConnString, "Sp_DwonloadFile", commandParameters);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        filename = ds.Tables[0].Rows[0]["NewFileName"].ToString();
                    }
                    filename = filename.Replace("//", "\\");
                    string contenttype ="";
                    if (filename.ToLower().EndsWith("pdf"))
                        contenttype = "application/pdf";
                    else
                        // in prod 
                        // contenttype = "image/" + Path.GetExtension(filename.Replace(".", ""));
                        if( filename.ToLower().EndsWith("jpeg")|| filename.ToLower().EndsWith("jpg"))
                    { 
                        contenttype = "image/jpeg" + Path.GetExtension(filename.Replace(".", ""));// Request.QueryString["FileName"].ToString().Split('.')[Request.QueryString["FileName"].ToString().Split('.').Length - 1];//+ Path.GetExtension(Request.QueryString["FileName"].Replace(".", ""));
                    }
                    else
                    {
                        contenttype = "image/png" + Path.GetExtension(filename.Replace(".", ""));// Request.QueryString["FileName"].ToString().Split('.')[Request.QueryString["FileName"].ToString().Split('.').Length - 1];//+ Path.GetExtension(Request.QueryString["FileName"].Replace(".", ""));
                    }
                    // image/png
                    setConfigValues();
                    iU.Impersonate(sSLD, sSLUN, sPwd);
                    filePath = ShareFolderPath;
                   // filePath = @"\\10.10.65.3\kgac_upload_dd_test";
                     totalpath = Path.Combine(filePath, filename);
            
                    FileStream fs = new FileStream(Path.Combine(filePath , filename),
                    FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    br.Close();
                    fs.Close();
                    iU.Undo();
                    //Write the file to response Stream
                    Response.Buffer = false;
                    Response.Clear();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = contenttype;
                    Response.AddHeader("content-disposition", "inline;filename=" + filename);
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                   //  Response.End();
                    B_upload = null ;
                }
                catch (Exception ex)
                {
                    // WriteToLogFile(ex ,"From RenderFile'"+ totalpath+"'");
                    //   Response.Redirect("DocumentError.aspx");
                    Elog = new ErrorLogger();
                    Elog.WriteToLogFile(ex, "public Page_Load In renderFile Profile name '" + Session["profileName"].ToString() + "'and hidprofile'" + Session["hidrefprofile"].ToString() + "'", "tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'  ");
                    Elog = null;
                    Response.Redirect("DocumentRenderError.aspx");

                }
            }
        }
    }
}