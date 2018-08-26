using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Specialized;
using System.Net;
using System.Collections;
using System.Reflection;
using System.Web.UI.HtmlControls;
using MicroClear.EnterpriseSolutions.CryptographyServices;
using MicroClear.EnterpriseSolutions.ServiceFactories;

namespace DocumentManagementServices
{
    public class Common : System.Web.UI.Page
    {

        private int PageSize = 10;

        //change here to change the upload file size bytes
        private const int FileUploadSizeInKB = 500 * 1024;

        public string ReferenceId = string.Empty;
        public string ProfileName = string.Empty;
        public string Color = string.Empty;
        public string Language = string.Empty;
        public string ParentId = string.Empty;
        public string TablePrimaryKey;
        public string ReferenceType = string.Empty;


        public string ShareLocationPassword = ConfigurationManager.AppSettings["ShareLocationPassword"].ToString();
        public string ShareLocationDomain = ConfigurationManager.AppSettings["ShareLocationDomain"].ToString();
        public string ShareLocationUserName = ConfigurationManager.AppSettings["ShareLocationUserName"].ToString();
        public string ShareFolderPath = ConfigurationManager.AppSettings["ShareFolderPath"].ToString();

        public Page basepage = null;
        public global::System.Web.UI.WebControls.DropDownList ddlDocumentTypes;
        public global::System.Web.UI.WebControls.GridView GridView1;
        public global::System.Web.UI.WebControls.Label lblMessage;
        public global::System.Web.UI.HtmlControls.HtmlLink TEST;
        public global::System.Web.UI.WebControls.HiddenField HiddenField1;
        public global::System.Web.UI.WebControls.TextBox txtDescription;
        public global::System.Web.UI.WebControls.HiddenField HiddenField3;

        

        public void Onload(Page page, object sender)
        {


            //if (basepage.Session["ReferenceId"] == null)
            //{
            //    // Get the NameValueCollection 
            //    PropertyInfo isreadonly1 = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            //    // make collection editable 
            //    isreadonly1.SetValue(basepage.Request.QueryString, false, null);
            //    // Clear 
            //    basepage.Request.QueryString.Add("ReferenceId", "242");
            //    basepage.Request.QueryString.Add("ProfileName", "MaqasaDocuments");
            //    basepage.Request.QueryString.Add("Language", "ara");
            //    basepage.Request.QueryString.Add("Color", "blue");
            //    basepage.Request.QueryString.Add("FormName", "Form");
            //    basepage.Request.QueryString.Add("ParentId", "4414234");
            //    basepage.Request.QueryString.Add("TablePrimaryKey", "DocumentId");
            //    basepage.Request.QueryString.Add("ReferenceType", "M");
            //    // make collection readonly again
            //    isreadonly1.SetValue(basepage.Request.QueryString, true, null);
            //}



            if (basepage.Request.QueryString.Count == 0)
            {
                if (!basepage.IsPostBack)
                {
                    SetSessionCount();
                }
                bool Countallowed = false;
                if (basepage.Session["SessionCount"].ToString() == "0" || basepage.Session["SessionCount"].ToString() == "1")
                    Countallowed = true;
                if (basepage.Session["ReferenceId"] == null || basepage.Session["SessionCount"] == null || !Countallowed)
                {
                    ResetSessionCount();
                    basepage.Response.Redirect("~/Errorpage.aspx");
                }
                lblMessage.Text = "";

                ReferenceId = basepage.Session["ReferenceId"].ToString();
                ProfileName = basepage.Session["ProfileName"].ToString();
                Language = basepage.Session["Language"].ToString();
                ParentId = basepage.Session["ParentId"].ToString();
                ReferenceType = basepage.Session["ReferenceType"].ToString();

                Color = basepage.Session["Color"].ToString();
                if (Language == "eng")
                {
                    TEST.Attributes["href"] = "~/Styles/" + Color + ".css";
                }
                else if (Language == "ara")
                {
                    TEST.Attributes["href"] = "~/Styles/" + Color + "_ara.css";
                }
                HiddenField1.Value = basepage.Session["FormName"].ToString();
                TablePrimaryKey = basepage.Session["TablePrimaryKey"].ToString();

                if (!basepage.IsPostBack)
                {
                    BindGrid(GridView1);
                    BindDropdownDocumentTypes(ddlDocumentTypes);
                }

            }

            else
            {

                if (basepage.Request.QueryString["ReferenceId"] != null)
                {
                    Session["ReferenceId"] = basepage.Request.QueryString["ReferenceId"].ToString();
                    ReferenceId = basepage.Request.QueryString["ReferenceId"].ToString();

                }
                if (basepage.Request.QueryString["ProfileName"] != null)
                {
                    Session["ProfileName"] = basepage.Request.QueryString["ProfileName"].ToString();
                    ProfileName = basepage.Request.QueryString["ProfileName"].ToString();

                }
                if (basepage.Request.QueryString["Language"] != null)
                {
                    Session["Language"] = basepage.Request.QueryString["Language"].ToString();
                    Language = basepage.Request.QueryString["Language"].ToString();

                }
                if (basepage.Request.QueryString["Color"] != null)
                {
                    if (basepage.Request.QueryString["Color"].ToString() == "")
                    {
                        Session["Color"] = "Blue";
                        Color = "Blue";

                    }
                    else
                    {
                        Session["Color"] = basepage.Request.QueryString["Color"].ToString();
                        Color = basepage.Request.QueryString["Color"].ToString();
                    }
                    if (Language == "eng")
                    {
                        TEST.Attributes["href"] = "~/Styles/" + Color + ".css";
                    }
                    else if (Language == "ara")
                    {
                        TEST.Attributes["href"] = "~/Styles/" + Color + "_ara.css";
                    }

                }
                if (basepage.Request.QueryString["FormName"] != null)
                {
                    Session["FormName"] = basepage.Request.QueryString["FormName"].ToString();
                    HiddenField1.Value = basepage.Request.QueryString["FormName"].ToString();

                }
                if (basepage.Request.QueryString["ParentId"] != null)
                {
                    Session["ParentId"] = basepage.Request.QueryString["ParentId"].ToString();
                    ParentId = basepage.Request.QueryString["ParentId"].ToString();

                }

                if (basepage.Request.QueryString["ReferenceType"] != null)
                {
                    Session["ReferenceType"] = basepage.Request.QueryString["ReferenceType"].ToString();
                    ReferenceType = basepage.Request.QueryString["ReferenceType"].ToString();

                }

                if (ProfileName != string.Empty)
                {
                    TablePrimaryKey = Common.GetPrimaryKey(ProfileName);
                    Session["TablePrimaryKey"] = TablePrimaryKey;

                }

                Session.Timeout = GetSessionTime();

                //        HtmlLink canonical = new HtmlLink();
                //        canonical.Href = url;
                //        canonical.Attributes["rel"] = "canonical";
                //        basepage.Page.Header.Controls.Add(canonical);


                //// Get the NameValueCollection 
                //PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                //// make collection editable 
                //isreadonly.SetValue(basepage.Request.QueryString, false, null);
                //// Clear 
                //basepage.Request.QueryString.Clear();
                //// make collection readonly again
                //isreadonly.SetValue(basepage.Request.QueryString, true, null);







                if (ReferenceId == string.Empty && ProfileName == string.Empty && Color == string.Empty && Language == string.Empty)
                {
                    ResetSessionCount();
                    basepage.Response.Redirect("~/Errorpage.aspx");
                }
                ResetSessionCount();
               
                if (Language == "ara")
                {
                    // SetSessionCount();
                    basepage.Response.Redirect("~/DocumentTransferAra.aspx", true);
                }
                else
                {
                    // SetSessionCount();
                    basepage.Response.Redirect("~/DocumentTransfer.aspx", true);
                }

            }

        }

        public void SetSessionCount()
        {
            if (basepage.Session["SessionCount"] == null || basepage.Session["SessionCount"].ToString() == "")
            {
                basepage.Session["SessionCount"] = "1";
            }
            else
            {
                basepage.Session["SessionCount"] = (Convert.ToInt32(basepage.Session["SessionCount"]) + 1).ToString();

            }
        }

        public void ResetSessionCount()
        {
            if (basepage.Session["SessionCount"] != null)
            {
                int count = Convert.ToInt32(basepage.Session["SessionCount"].ToString());
                if (count > 1)
                {
                    count = count - 1;
                    basepage.Session["SessionCount"] = count.ToString();
                }
                else
                {
                    basepage.Session["SessionCount"] = "0";
                }


            }
            else
            {
                basepage.Session["SessionCount"] = "0";
            }

        }

        public static string GetNewID(string profile)
        {
            //return this.GetNewKey();
            return GetNewIntKey(profile);
        }
        private static string GetNewIntKey(string sCounterName)
        {
            //				int iCounter = ServiceFactory.GetBusinessManagerInstance().GetNextCounterValue(sCounterName);
            //				return iCounter.ToString();

            Int64 counterValueStart;
            Int64 counterValueEnd;
            GetNewIntCounter(sCounterName, 1, out counterValueStart, out counterValueEnd);
            return counterValueStart.ToString();
        }
        private static Int64 GetNewIntCounter(string sCounterName, Int64 seedValue, out Int64 counterValueStart, out Int64 counterValueEnd)
        {

            SqlParameter[] commandParameters = new SqlParameter[4];
              
            commandParameters[0] = new SqlParameter("@DataSourceName", SqlDbType.VarChar, 50);
            commandParameters[1] = new SqlParameter("@SeedValue", SqlDbType.BigInt);
            commandParameters[2] = new SqlParameter("@CounterValueStart", SqlDbType.BigInt);
            commandParameters[3] = new SqlParameter("@CounterValueEnd", SqlDbType.BigInt);
           
            commandParameters[2].Direction =ParameterDirection.Output;
            commandParameters[3].Direction = ParameterDirection.Output;

            commandParameters[0].Value = sCounterName;
            commandParameters[1].Value = seedValue;

            SQLDataAccessHelper.Instance.ExecuteNonQuery("usp_MCPKCounters", commandParameters);

            counterValueStart = (Int64)commandParameters[2].Value;
            counterValueEnd = (Int64)commandParameters[3].Value;

            return (Int64)commandParameters[2].Value;


          
            //string getSqlConnString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            //SqlConnection connection = new SqlConnection(getSqlConnString);
            //SqlCommand cmd = new SqlCommand("usp_MCPKCounters", connection);
            //cmd.CommandType = CommandType.StoredProcedure;

            //cmd.Parameters.Add("@DataSourceName", SqlDbType.VarChar, 50);
            //cmd.Parameters.Add("@SeedValue", SqlDbType.BigInt);
            //cmd.Parameters.Add("@CounterValueStart", SqlDbType.BigInt);
            //cmd.Parameters.Add("@CounterValueEnd", SqlDbType.BigInt);
            //cmd.Parameters["@CounterValueStart"].Direction = ParameterDirection.Output;
            //cmd.Parameters["@CounterValueEnd"].Direction = ParameterDirection.Output;

            //cmd.Parameters["@DataSourceName"].Value = sCounterName;
            //cmd.Parameters["@SeedValue"].Value = seedValue;

            //connection.Open();
            //cmd.ExecuteNonQuery();
            //connection.Close();

            //counterValueStart = (Int64)cmd.Parameters["@CounterValueStart"].Value;
            //counterValueEnd = (Int64)cmd.Parameters["@CounterValueEnd"].Value;

            //return (Int64)cmd.Parameters["@CounterValueStart"].Value;
        }

        public static string GetPrimaryKey(string TableName)
        {
            string primarykey = string.Empty;
            string CommandText ="SELECT column_name FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') = 1 AND table_name = '" + TableName + "'" ;

            using (SqlDataReader sdr = SQLDataAccessHelper.Instance.ExecuteReader(CommandType.Text, CommandText))
            {
                sdr.Read();
                if (sdr.HasRows)
                    primarykey = sdr["column_name"].ToString();
            }
            //string constr = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            //using (SqlConnection con = new SqlConnection(constr))
            //{
            //    using (SqlCommand cmd = new SqlCommand())
            //    {
            //        cmd.CommandText = "SELECT column_name FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') = 1 AND table_name = '" + TableName + "'";
            //        cmd.Connection = con;
            //        con.Open();
            //        using (SqlDataReader sdr = cmd.ExecuteReader())
            //        {
            //            sdr.Read();
            //            if (sdr.HasRows)
            //                primarykey = sdr["column_name"].ToString();
            //        }
            //    }
            //    con.Close();

            //}
            return primarykey;
        }

        public static int GetSessionTime()
        {
            string SessionTime = string.Empty;
            string CommandText = "select ConfigName,ConfigValue from Configurations where ConfigName='SessionTimeOut'";
            using (SqlDataReader sdr = SQLDataAccessHelper.Instance.ExecuteReader(CommandType.Text, CommandText))
            {
                sdr.Read();
                if (sdr.HasRows)
                    SessionTime = sdr["ConfigValue"].ToString();
            }

            //string constr = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            //using (SqlConnection con = new SqlConnection(constr))
            //{
            //    using (SqlCommand cmd = new SqlCommand())
            //    {
            //        cmd.CommandText = "select ConfigName,ConfigValue from Configurations where ConfigName='SessionTimeOut'";
            //        cmd.Connection = con;
            //        con.Open();
            //        using (SqlDataReader sdr = cmd.ExecuteReader())
            //        {
            //            sdr.Read();
            //            if (sdr.HasRows)
            //                SessionTime = sdr["ConfigValue"].ToString();
            //        }
            //    }
            //    con.Close();

            //}
            return ConvertToInteger(SessionTime);
        }

        public static int ConvertToInteger(string stringValue)
        {
            int returnvalue = 5;
            try
            {
                returnvalue = stringValue == null ? 0 : stringValue == "" ? 0 : Convert.ToInt32(stringValue);
            }
            catch (Exception ex)
            {
                string s = ex.Message;

                returnvalue = 0;
            }
            return returnvalue;
        }

        public static void DeleteOperation(StringCollection Id, string TablePrimaryKey)
        {
            string IDs = "";
            foreach (string id in Id)
            {
                IDs += id.ToString() + ",";
            }
            string ID = IDs.Substring(0, IDs.LastIndexOf(","));
            string query = "Delete from MaqasaDocuments where " + TablePrimaryKey + " IN(" + ID + ")";
            SQLDataAccessHelper.Instance.ExecuteNonQuery(CommandType.Text, query);

            //string constr = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            //SqlConnection con = new SqlConnection(constr);
            //string IDs = "";
            //foreach (string id in Id)
            //{
            //    IDs += id.ToString() + ",";
            //}
            //string ID = IDs.Substring(0, IDs.LastIndexOf(","));
            //string query = "Delete from MaqasaDocuments where " + TablePrimaryKey + " IN(" + ID + ")";

            //SqlCommand cmd = new SqlCommand(query);
            //cmd.Connection = con;
            //con.Open();
            //cmd.ExecuteNonQuery();
            //con.Close();
        }

        public void DownloadFiles(Page page, object sender)
        {
            try
            {
                byte[] bytes; string NewFileName = string.Empty;
                string fileName = string.Empty; string strpath = string.Empty;
                int id = int.Parse((sender as LinkButton).CommandArgument);

                string CommandText ="select " + TablePrimaryKey + ",DocumentName, NewFileName from  " + ProfileName + "  where " + TablePrimaryKey + " =@Id";
                 SqlParameter[] commandParameters = new SqlParameter[1];
                 commandParameters[0] = new SqlParameter();
                 commandParameters[0].ParameterName = "@Id";
                 commandParameters[0].Value = id;

                using (SqlDataReader sdr = SQLDataAccessHelper.Instance.ExecuteReader(CommandType.Text, CommandText, commandParameters))
                {
                    sdr.Read();
                    NewFileName = sdr["NewFileName"].ToString();
                    fileName = sdr["DocumentName"].ToString();

                    ImpersonateUser iU = new ImpersonateUser();
                    string sPwd = "";
                    SymmetricEncryption CgServices = ServiceFactory.GetSymmetricEncryptionInstance(); ;
                    sPwd = CgServices.DecryptData(ConfigurationSettings.AppSettings["ShareLocationPassword"].ToString());
                    iU.Impersonate(ConfigurationSettings.AppSettings["ShareLocationDomain"].ToString(), ConfigurationSettings.AppSettings["ShareLocationUserName"], sPwd);

                    strpath = Path.Combine(ShareFolderPath, NewFileName); ;
                    System.IO.FileStream fs = null;
                    fs = System.IO.File.Open(strpath, System.IO.FileMode.Open);
                    bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                    //}

                    iU.Undo();
                }
                    
                //string constr = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
                //using (SqlConnection con = new SqlConnection(constr))
                //{
                //    using (SqlCommand cmd = new SqlCommand())
                //    {
                //        cmd.CommandText = "select " + TablePrimaryKey + ",DocumentName, NewFileName from  " + ProfileName + "  where " + TablePrimaryKey + " =@Id";
                //        cmd.Parameters.AddWithValue("@Id", id);
                //        cmd.Connection = con;
                //        con.Open();
                //        using (SqlDataReader sdr = cmd.ExecuteReader())
                //        {
                //            sdr.Read();
                //            NewFileName = sdr["NewFileName"].ToString();
                //            fileName = sdr["DocumentName"].ToString();

                //            //NetworkCredential NCredentials = new NetworkCredential(ShareLocationUserName, ShareLocationPassword, ShareLocationDomain);

                //            //using (new NetworkConnection(ShareFolderPath, NCredentials))
                //            //{

                //            ImpersonateUser iU = new ImpersonateUser();
                //            string sPwd = "";
                //            SymmetricEncryption CgServices = ServiceFactory.GetSymmetricEncryptionInstance(); ;
                //            sPwd = CgServices.DecryptData(ConfigurationSettings.AppSettings["ShareLocationPassword"].ToString());
                //            iU.Impersonate(ConfigurationSettings.AppSettings["ShareLocationDomain"].ToString(), ConfigurationSettings.AppSettings["ShareLocationUserName"], sPwd);

                //            strpath = Path.Combine(ShareFolderPath, NewFileName); ;
                //            System.IO.FileStream fs = null;
                //            fs = System.IO.File.Open(strpath, System.IO.FileMode.Open);
                //            bytes = new byte[fs.Length];
                //            fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                //            fs.Close();
                //            //}

                //            iU.Undo();
                //        }
                //    }
                //    con.Close();

                //}

                page.Response.Clear();
                page.Response.Buffer = true;
                page.Response.ContentType = "application/octet-stream";
                page.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                page.Response.Charset = "";
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                if (bytes.Length > 0)
                    page.Response.OutputStream.Write(bytes, 0, (int)bytes.Length);

                page.Response.Flush();
                //bytes=new Byte[test];
                page.Response.End();

            }

            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }
        public void UploadFiles(Page page)
        {


            string filename = string.Empty; string lastItem = string.Empty;
            string[] arr = null; string[] separators = { "\\" }; string ID = string.Empty;



            HttpFileCollection fileCollection = basepage.Request.Files;

            //NetworkCredential NCredentials = new NetworkCredential(ShareLocationUserName, ShareLocationPassword, ShareLocationDomain);

            //using (new NetworkConnection(ShareFolderPath, NCredentials))

            ////using (new ImpersonatedUser(ShareLocationUserName, ShareLocationDomain, ShareLocationPassword))

            //{

            //  DriveMapping.DelDrive("i:", 0, true);
            //DriveMapping.MapDrive(@"\\10.138.74.233\c$\MaqasaAttachmentDocumentUpload", "Y:", @"agilityindia\tmurugesan", "mugil#2302");

            /// using (new NetworkConnection(@"\\server2\write", writeCredentials))

            ImpersonateUser iU = new ImpersonateUser();
            string sPwd = "";
            SymmetricEncryption CgServices = ServiceFactory.GetSymmetricEncryptionInstance(); ;
            sPwd = CgServices.DecryptData(ConfigurationSettings.AppSettings["ShareLocationPassword"].ToString());
            iU.Impersonate(ConfigurationSettings.AppSettings["ShareLocationDomain"].ToString(), ConfigurationSettings.AppSettings["ShareLocationUserName"], sPwd);

            for (int i = 0; i < fileCollection.Count; i++)
            {
                HttpPostedFile uploadfile = fileCollection[i];

                if (uploadfile.ContentLength > 0)
                {
                    if (uploadfile.ContentLength >= FileUploadSizeInKB)
                    {
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "alert('you cannot upload files more than 1MB');", true);
                        //return;
                        filename = uploadfile.FileName;
                        arr = filename.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        lastItem = arr[arr.Length - 1];
                        if (Language == "ara")
                        {
                            lblMessage.Text = "أقصى حجم للملفات التي يمكن تحميلها هو 500 كيلوبايت. حجم الملف أكبر من 500 كيلوبايت." + " " + lastItem;
                        }
                        else
                        {
                            lblMessage.Text = lblMessage.Text + lastItem + " " + "Size is too large. Maximum file size of 500 KB is allowed." + " <br/>";

                        }
                        // return;
                    }
                    else
                    {
                        ID = GetNewID("MaqasaDocuments");
                        string fileName = Path.GetFileName(uploadfile.FileName);
                        string GetFileName = Path.GetFileNameWithoutExtension(uploadfile.FileName);
                        string GetExtension = Path.GetExtension(uploadfile.FileName);

                        string Date = DateTime.Now.ToString("yyyy-MM-dd") + "\\" + ParentId;
                        string FullfileName = GetFileName + ID + GetExtension;
                        string FullPathFileName = Date + "\\" + FullfileName;

                        ShareFolderPath = ShareFolderPath + "\\" + Date;
                        if (!Directory.Exists(ShareFolderPath)) Directory.CreateDirectory(ShareFolderPath);
                        string strpath = ShareFolderPath + "\\" + FullfileName;
                        uploadfile.SaveAs(strpath);


                        //byte[] buffer = new byte[uploadfile.ContentLength];
                        //uploadfile.InputStream.Read(buffer, 0, uploadfile.ContentLength);





                        //string contentType = uploadfile.ContentType;
                        //Stream FileStream = uploadfile.InputStream;
                        //BinaryReader br = new BinaryReader(FileStream);
                        //byte[] myData = br.ReadBytes((Int32)FileStream.Length);
                        //// byte[] myData = new byte[uploadfile.ContentLength];

                        string query = "insert into " + ProfileName + " (" + TablePrimaryKey + ",DocumentName,ReferenceId,NewFileName,StateId,Description,DateCreated,ReferenceType,MaqasaDocumentType) values (@DocumentId,@DocumentName, @ReferenceId, @NewFileName,@StateId,@Description,@DateCreated,@ReferenceType,@MaqasaDocumentType)";

                        SqlParameter[] commandParameters = new SqlParameter[9];
                        commandParameters[0] = new SqlParameter();
                        commandParameters[0].ParameterName = TablePrimaryKey;
                        commandParameters[0].Value = ID;

                        commandParameters[1] = new SqlParameter();
                        commandParameters[1].ParameterName = "@DocumentName";
                        commandParameters[1].Value = fileName;

                        commandParameters[2] = new SqlParameter();
                        commandParameters[2].ParameterName = "@ReferenceId";
                        commandParameters[2].Value = ReferenceId;

                        commandParameters[3] = new SqlParameter();
                        commandParameters[3].ParameterName = "@NewFileName";
                        commandParameters[3].Value = FullPathFileName;

                        commandParameters[4] = new SqlParameter();
                        commandParameters[4].ParameterName = "@StateId";
                        commandParameters[4].Value = ProfileName + "CreatedState";

                        commandParameters[5] = new SqlParameter();
                        commandParameters[5].ParameterName = "@Description";
                        commandParameters[5].Value = ((TextBox)page.FindControl("txtDescription")).Text;

                        commandParameters[6] = new SqlParameter();
                        commandParameters[6].ParameterName = "@DateCreated";
                        commandParameters[6].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") ;

                        commandParameters[7] = new SqlParameter();
                        commandParameters[7].ParameterName = "@ReferenceType";
                        commandParameters[7].Value = ReferenceType;

                        commandParameters[8] = new SqlParameter();
                        commandParameters[8].ParameterName = "@MaqasaDocumentType";
                        commandParameters[8].Value = Convert.ToInt32(ddlDocumentTypes.SelectedValue);

                        SQLDataAccessHelper.Instance.ExecuteNonQuery(CommandType.Text, query, commandParameters);

                        //string constr = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
                        //using (SqlConnection con = new SqlConnection(constr))
                        //{
                        //    //Commented by subramanyam for adding MaqasaDocumentType column
                        //    //string query = "insert into " + ProfileName + " (" + TablePrimaryKey + ",DocumentName,ReferenceId,NewFileName,StateId,Description,DateCreated,ReferenceType) values (@DocumentId,@DocumentName, @ReferenceId, @NewFileName,@StateId,@Description,@DateCreated,@ReferenceType)";
                        //string query = "insert into " + ProfileName + " (" + TablePrimaryKey + ",DocumentName,ReferenceId,NewFileName,StateId,Description,DateCreated,ReferenceType,MaqasaDocumentType) values (@DocumentId,@DocumentName, @ReferenceId, @NewFileName,@StateId,@Description,@DateCreated,@ReferenceType,@MaqasaDocumentType)";
                        //using (SqlCommand cmd = new SqlCommand(query))
                        //{
                        //    cmd.Connection = con;
                        //    cmd.Parameters.AddWithValue(TablePrimaryKey, ID);
                        //    cmd.Parameters.AddWithValue("@DocumentName", fileName);
                        //    cmd.Parameters.AddWithValue("@ReferenceId", ReferenceId);
                        //    cmd.Parameters.AddWithValue("@NewFileName", FullPathFileName);
                        //    cmd.Parameters.AddWithValue("@StateId", ProfileName + "CreatedState");
                        //    cmd.Parameters.AddWithValue("@Description", ((TextBox)page.FindControl("txtDescription")).Text);
                        //    cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                        //    cmd.Parameters.AddWithValue("@ReferenceType", ReferenceType);
                        //    //Added by subramanyam
                        //    cmd.Parameters.AddWithValue("@MaqasaDocumentType", Convert.ToInt32(ddlDocumentTypes.SelectedValue));
                        //    con.Open();
                        //    cmd.ExecuteNonQuery();
                        //    con.Close();
                        //}
                        //}
                    }
                }
                else
                {
                    filename = uploadfile.FileName;
                    arr = filename.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    lastItem = arr[arr.Length - 1];
                    if (Language == "ara")
                    {
                        lblMessage.Text = lblMessage.Text + lastItem + " " + "الملف فارغ" + " <br/>";
                    }
                    else
                    {
                        lblMessage.Text = lblMessage.Text + lastItem + " " + "file is empty" + " <br/>";

                    }
                    //return;
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "alert('you did not specify a file to upload?');", true);
                    //return;
                }

            }

            iU.Undo();

            //}
            TextBox stxtDescription = ((TextBox)page.FindControl("txtDescription"));
            stxtDescription.Text = "";
            ddlDocumentTypes.SelectedIndex = 0;
            BindGrid(GridView1);
        }
        public void DeleteFiles(Page page, object sender)
        {

            try
            {
                StringCollection IdCollection = new StringCollection();
                foreach (GridViewRow gvrow in GridView1.Rows)
                {
                    CheckBox chkdelete = (CheckBox)gvrow.FindControl("CheckBox1");

                    if (chkdelete.Checked)
                    {
                        string usrid = Convert.ToString(GridView1.DataKeys[gvrow.RowIndex].Value);
                        IdCollection.Add(usrid);
                    }
                }
                Common.DeleteOperation(IdCollection, TablePrimaryKey);

                BindGrid(GridView1);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }


        }

        public void saveDescription(Page page, object sender)
        {

            //try
            //{
            //    int ID = Convert.ToInt32(((HiddenField)page.FindControl("HiddenField3")).Value);
            //    string constr = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            //    SqlConnection con = new SqlConnection(constr);
            //    // string query = "UPDATE " + ProfileName + " SET Description=@Description" + "  where " + TablePrimaryKey + " =@Id";
            //    //Added by Subramanyam
            //    string query = "UPDATE " + ProfileName + " SET Description=@Description ,MaqasaDocumentType = @MaqasaDocumentType  where " + TablePrimaryKey + " =@Id";
            //    SqlCommand cmd = new SqlCommand(query);
            //    cmd.Connection = con;
            //    cmd.Parameters.AddWithValue("@Id", ID);
            //    cmd.Parameters.AddWithValue("@Description", ((TextBox)page.FindControl("txtDescription")).Text);
            //    //Added by subramanyam  
            //    cmd.Parameters.AddWithValue("@MaqasaDocumentType", ddlDocumentTypes.SelectedValue);
            //    con.Open();
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}

            try
            {
                int ID = Convert.ToInt32(((HiddenField)page.FindControl("HiddenField3")).Value);
                string query = "UPDATE " + ProfileName + " SET Description=@Description ,MaqasaDocumentType = @MaqasaDocumentType  where " + TablePrimaryKey + " =@Id";
                SqlParameter[] commandParameters = new SqlParameter[3];
                
                commandParameters[0] = new SqlParameter();
                commandParameters[0].ParameterName = "@Id";
                commandParameters[0].Value = ID;
               

                commandParameters[1] = new SqlParameter();
                commandParameters[1].ParameterName = "@Description";
                commandParameters[1].Value = ((TextBox)page.FindControl("txtDescription")).Text;

                commandParameters[2] = new SqlParameter();
                commandParameters[2].ParameterName = "@MaqasaDocumentType";
                commandParameters[2].Value = ddlDocumentTypes.SelectedValue;

                 SQLDataAccessHelper.Instance.ExecuteNonQuery(CommandType.Text, query, commandParameters);

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            TextBox stxtDescription = ((TextBox)page.FindControl("txtDescription"));
            stxtDescription.Text = "";
            BindGrid(GridView1);

        }


        public void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
            BindGrid(GridView1);

        }

        public SortDirection dir
        {
            get
            {
                if (ViewState["dirState"] == null)
                {
                    ViewState["dirState"] = SortDirection.Ascending;
                }
                return (SortDirection)ViewState["dirState"];
            }
            set
            {
                ViewState["dirState"] = value;
            }
        }

        public void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {

            BindGrid(GridView1);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            {
                string SortDir = string.Empty;
                if (dir == SortDirection.Ascending)
                {
                    dir = SortDirection.Descending;
                    SortDir = "Desc";
                }
                else
                {
                    dir = SortDirection.Ascending;
                    SortDir = "Asc";
                }
                DataView sortedView = new DataView(dt);
                sortedView.Sort = e.SortExpression + " " + SortDir;
                GridView1.DataSource = sortedView;
                GridView1.DataBind();
            }
        }


        public DataSet ds = new DataSet();

        public void BindGrid(GridView GridViewControl)
        {

            //string constr = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            GridViewControl.DataSource = null;
            GridViewControl.DataBind();
            string CommandText = string.Empty;
            //cmd.CommandText = "select " + TablePrimaryKey + ", DocumentName,Description,DateCreated from " + ProfileName + " where  ReferenceId ='" + ReferenceId + "'" + "AND ReferenceType = '" + ReferenceType + "'";
            if (Language == "eng")
                CommandText = "select " + TablePrimaryKey + ", DocumentName,MD.[Description],MD.DateCreated ,T.Name [DocumentType] from " + ProfileName + " MD ,[Types] T where MaqasaDocumentType=T.[TypeId] And ReferenceId ='" + ReferenceId + "'" + "AND ReferenceType = '" + ReferenceType + "'";
            else if (Language == "ara")
                CommandText = "select " + TablePrimaryKey + ", DocumentName,MD.[Description],MD.DateCreated ,T.Name [DocumentType] from " + ProfileName + " MD ,[Types_ara] T where MaqasaDocumentType=T.[TypeId] And ReferenceId ='" + ReferenceId + "'" + "AND ReferenceType = '" + ReferenceType + "'";

            ds = SQLDataAccessHelper.Instance.ExecuteDataset(CommandType.Text, CommandText);
            GridViewControl.DataSource = ds;
            GridViewControl.DataBind();

            //using (SqlConnection con = new SqlConnection(constr))
            //{
            //    using (SqlCommand cmd = new SqlCommand())
            //    {
            //        //cmd.CommandText = "select " + TablePrimaryKey + ", DocumentName,Description,DateCreated from " + ProfileName + " where  ReferenceId ='" + ReferenceId + "'" + "AND ReferenceType = '" + ReferenceType + "'";
            //        if (Language == "eng")
            //            cmd.CommandText = "select " + TablePrimaryKey + ", DocumentName,MD.[Description],MD.DateCreated ,T.Name [DocumentType] from " + ProfileName + " MD ,[Types] T where MaqasaDocumentType=T.[TypeId] And ReferenceId ='" + ReferenceId + "'" + "AND ReferenceType = '" + ReferenceType + "'";
            //        else if (Language == "ara")
            //            cmd.CommandText = "select " + TablePrimaryKey + ", DocumentName,MD.[Description],MD.DateCreated ,T.Name [DocumentType] from " + ProfileName + " MD ,[Types_ara] T where MaqasaDocumentType=T.[TypeId] And ReferenceId ='" + ReferenceId + "'" + "AND ReferenceType = '" + ReferenceType + "'";

            //        cmd.Connection = con;
            //        con.Open();
            //        SqlDataAdapter da = new SqlDataAdapter(cmd);
            //        da.Fill(ds);
            //        GridViewControl.DataSource = ds;
            //        GridViewControl.DataBind();
            //        con.Close();
            //    }
            //}
        }

        public void BindDropdownDocumentTypes(DropDownList ddlDocumentTypes)
        {
            //string constr = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            ddlDocumentTypes.DataSource = null;
            ddlDocumentTypes.DataBind();

            string CommandText = string.Empty;
                    if (Language == "eng")
                        CommandText = " select [TypeId],[Name] FROM [Types] where [TypeTypeId]  = dbo.KWConstantFn('GBL_Types.TYPE.MaqasaDocumentType')";
                    else if (Language == "ara")
                        CommandText = " select TypeId,Name from Types_ara where TypeId in ( select [TypeId] FROM [Types] where [TypeTypeId] = dbo.KWConstantFn('GBL_Types.TYPE.MaqasaDocumentType') )";

                    DataTable dt = SQLDataAccessHelper.Instance.ExecuteDataset(CommandType.Text, CommandText).Tables[0];
         
                    if (Language == "eng")
                        ddlDocumentTypes.Items.Add(new ListItem("Please Select Document Type", "0"));
                    else if (Language == "ara")
                        ddlDocumentTypes.Items.Add(new ListItem("يرجى إختيار نوع المستند", "0"));

                    string CommandText1 = "select [TypeId] from  [Types] where Name like 'Statistical Declaration + Maqassa Stamp'";
                    string result = SQLDataAccessHelper.Instance.ExecuteScalar(CommandType.Text, CommandText1).ToString();
                    // ReferenceId = "4406741";//For Statistical
                    bool isStatistical = IsStatisticalBayan(ReferenceId);
                    foreach (DataRow item in dt.Rows)
                    {

                        if (result == item["TypeId"].ToString() && isStatistical)
                            continue;

                        ddlDocumentTypes.Items.Add(new ListItem(item["Name"].ToString(), item["TypeId"].ToString()));
                    }
                    ddlDocumentTypes.DataBind();
        


            //using (SqlConnection con = new SqlConnection(constr))
            //{
            //    using (SqlCommand cmd = new SqlCommand())
            //    {
            //        if (Language == "eng")
            //            cmd.CommandText = " select [TypeId],[Name] FROM [Types] where [TypeTypeId]  = dbo.KWConstantFn('GBL_Types.TYPE.MaqasaDocumentType')";
            //        else if (Language == "ara")
            //            cmd.CommandText = " select TypeId,Name from Types_ara where TypeId in ( select [TypeId] FROM [Types] where [TypeTypeId] = dbo.KWConstantFn('GBL_Types.TYPE.MaqasaDocumentType') )";

            //        cmd.Connection = con;
            //        con.Open();
            //        SqlDataAdapter da = new SqlDataAdapter(cmd);
            //        DataTable dt = new DataTable();
            //        da.Fill(dt);

            //        if (Language == "eng")
            //            ddlDocumentTypes.Items.Add(new ListItem("Please Select Document Type", "0"));
            //        else if (Language == "ara")
            //            ddlDocumentTypes.Items.Add(new ListItem("يرجى إختيار نوع المستند", "0"));

            //        cmd.CommandText = "select [TypeId] from  [Types] where Name like 'Statistical Declaration + Maqassa Stamp'";
            //        string result = cmd.ExecuteScalar().ToString();
            //        // ReferenceId = "4406741";//For Statistical
            //        bool isStatistical = IsStatisticalBayan(ReferenceId);
            //        foreach (DataRow item in dt.Rows)
            //        {

            //            if (result == item["TypeId"].ToString() && isStatistical)
            //                continue;

            //            ddlDocumentTypes.Items.Add(new ListItem(item["Name"].ToString(), item["TypeId"].ToString()));
            //        }
            //        ddlDocumentTypes.DataBind();
            //    }
            //}
        }


        public bool IsStatisticalBayan(string DecId)
        {
            bool result = false;
            string CommandText = string.Empty;
            CommandText = "select 1 from Declarations where Declarationid ='" + DecId + "' and CustomsControlProcedureId in (dbo.KWConstantfn('GBL_CustomsControlProcedures.AIRPORT.EXPORT_BILL.STATISTICAL'),dbo.KWConstantfn('GBL_CustomsControlProcedures.LANDPORT.EXPORT_BILL.STATISTICAL'),dbo.KWConstantfn('GBL_CustomsControlProcedures.SEAPORT.EXPORT_BILL.STATISTICAL'))" ;
             object oResult = SQLDataAccessHelper.Instance.ExecuteScalar(CommandType.Text, CommandText) ;
             if (oResult != null && oResult.ToString() == "1")
                return true;

            //string constr = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            //using (SqlConnection con = new SqlConnection(constr))
            //{
            //    using (SqlCommand cmd = new SqlCommand("select 1 from Declarations where Declarationid ='" + DecId + "' and CustomsControlProcedureId in (dbo.KWConstantfn('GBL_CustomsControlProcedures.AIRPORT.EXPORT_BILL.STATISTICAL'),dbo.KWConstantfn('GBL_CustomsControlProcedures.LANDPORT.EXPORT_BILL.STATISTICAL'),dbo.KWConstantfn('GBL_CustomsControlProcedures.SEAPORT.EXPORT_BILL.STATISTICAL'))"))
            //    {
            //        cmd.Connection = con;
            //        con.Open();
            //        if (cmd.ExecuteScalar() != null && cmd.ExecuteScalar().ToString() == "1")
            //            return true;
            //    }
            //}
            return result;
        }
    }


}
