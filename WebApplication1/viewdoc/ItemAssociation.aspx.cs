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
namespace WebApplication1
{
    public partial class ItemAssociation : System.Web.UI.Page
    {
        public string checkflag="true";
        public static string mysessionId = string.Empty;
        public static string Tokensalt = string.Empty;
        public static string mytokenvalue = string.Empty;
        static readonly string PasswordHash = "P@@Sw0rd";
        static readonly string SaltKey = "S@LT&KEY";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";

        BuisnessUploads B_upload = new BuisnessUploads();
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

        public string StateId = string.Empty;
        public string ProfileName = string.Empty;
        
        
        public int Ownerorgid = 0;

        public int Ownerlocid = 0;

        //public string Ownerorgid = string.Empty;
        //public string Ownerlocid = string.Empty;

        public string CreatedBy = string.Empty;
        public string ThemeId = string.Empty;
        public string DateCreated = string.Empty;
        public string DateModified = string.Empty;

        public string ModifiedBy = string.Empty;

        string DeclarationId;

        public string pageId = string.Empty;


        String strConnString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Documentid"] != null)
            {
                StringBuilder sb = new StringBuilder();
                string dataItem = Request.QueryString["Documentid"].ToString();
          
                Session["decryptedDocid"] = System.Web.HttpUtility.UrlEncode(dataItem.TrimStart(','));



                mytokenvalue = Session["mytokenvalue"].ToString().Split('|')[0];
            Tokensalt = Session["mytokenvalue"].ToString().Split('|')[1];
            SqlParameter[] commandParameters1 = new SqlParameter[3];
            commandParameters1[0] = new SqlParameter();
            commandParameters1[0].ParameterName = "@tokenval";
            commandParameters1[0].Value = mytokenvalue.ToString().Split('|')[0];
            commandParameters1[1] = new SqlParameter();
            commandParameters1[1].ParameterName = "@sessionId";
            commandParameters1[1].Value = Session["mysessionId"].ToString();
            commandParameters1[2] = new SqlParameter();
            commandParameters1[2].ParameterName = "@Tokensalt";
            commandParameters1[2].Value = Tokensalt;
            ds = B_upload.GetTokenvalue(strConnString, "usp_GetTokenInfo_DocUpload", commandParameters1);
            if (ds.Tables.Count != 0)
            {
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                    languageid = ds.Tables[0].Rows[0]["LanguageId"].ToString();
                    UploadedFrom = ds.Tables[0].Rows[0]["ReferenceProfile"].ToString();
                    declarationDocumenttype = ds.Tables[0].Rows[0]["DocumentId"].ToString();
                    hidRefProfile = ds.Tables[0].Rows[0]["ReferenceProfile"].ToString();
                    ProfileName = ds.Tables[0].Rows[0]["profileName"].ToString();
                    DeclarationId = ds.Tables[0].Rows[0]["ReferenceId"].ToString();
                        ThemeId = ds.Tables[0].Rows[0]["ThemeId"].ToString();

                        pageId = ds.Tables[0].Rows[0]["pageId"].ToString();
                    TablePrimaryKey = B_upload.GetPrimaryKey(ProfileName, strConnString);
                    Ownerlocid = Convert.ToInt32(ds.Tables[0].Rows[0]["ownerlocid"]);
                    Ownerorgid = Convert.ToInt32(ds.Tables[0].Rows[0]["OwnerOrgId"]);
                    ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                    CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                    Session["profileName"] = ProfileName.ToString();
                    Session["DeclarationId"] = DeclarationId.ToString();


                        GetDirection.Text = languageid;

                        Session["hidRefProfile"] = hidRefProfile.ToString();


                        if (languageid == "eng")
                        {
                            ThemeId = ThemeId + ".css";
                        }
                        else
                        {
                            ThemeId = ThemeId + "_ara.css";
                        }

                    }

                else
                {
             //       WriteToLogFile("No Value Found For in Sp :usp_GetTokenInfo_DocUpload '" + mytokenvalue + "'");
                    Response.Redirect("DocumentError.aspx");
                }
            }



        }
        }
   
        public string Encrypt(string plainText)
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

        [System.Web.Services.WebMethod]
        public static void ValidateNumber(string number)
        {
            int no = Convert.ToInt32(number);
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

                //  WriteToLogFile(e);
            }
            return x;
        }
    }
}