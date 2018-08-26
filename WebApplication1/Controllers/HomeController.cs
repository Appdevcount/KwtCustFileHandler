using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using Microsoft.Ajax.Utilities;
using System.Runtime.InteropServices;
using System.CodeDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using WebApplication1.Models;
using PagedList;
using System.Web.UI;
using System.Web.SessionState;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Security.Principal;
using WebApplication1.Models.BAL;
using WebApplication1.Models.DAL;
using DocumentManagementServices;
using MicroClear.EnterpriseSolutions.CryptographyServices;
using MicroClear.EnterpriseSolutions.ServiceFactories;
using CryptographyKGAC;
using System.Text;
using System.Collections;
using System.Web.Caching;
using System.Security.Cryptography;
//using System.Web.HttpContext;
using System.Net.Mail;
using System.Net;
// using System.Net.Http;
// using System.Web.Http;
// using System.Net;

namespace WebApplication1.Controllers
{


    public class HomeController : Controller
    {



        //        SQLHelper sqlHelp = new SQLHelper();
        SQLHelper sqlHelp = null;

        

        #region variables
        //TokenvalueDetails ObjtokenDetails;// = new TokenvalueDetails();

//        BuisnessUploads B_upload = new BuisnessUploads();


        TokenvalueDetails ObjtokenDetails;// = new TokenvalueDetails();
        ErrorLogger Elog;

        BuisnessUploads B_upload; // = new BuisnessUploads();
        FileModel F = new FileModel();
        DataSet ds;
        string ID = string.Empty;
        String strConnString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ToString();
        //private const int FileUploadSizeInKB = 500 * 1024;
        private int FileUploadSizeInKB = 0;
        int count = 0;
        public string ShareLocationPassword = string.Empty;
        public string ShareLocationDomain = string.Empty;
        public string ShareLocationUserName = string.Empty;
        public string ShareFolderPath = string.Empty;
        public string ScanDocRequestId = string.Empty;
        public string ReferenceId = string.Empty;

        public string referredUrl = string.Empty;
        public int documentcount = 0;
        public int doccountconfig = 0;

        public int mcTokendoccount = 0;

        public int AdditionalDocumentId = 0;


        public string isBroker = string.Empty;
        public string RoleId = string.Empty;
        public string DocumentId = string.Empty;
        public string RecCreatedBy = string.Empty;
        public string UploadedFrom = string.Empty;
        public string TablePrimaryKey = string.Empty;
        public string ProfileName = string.Empty;
        public string ReferenceType = string.Empty;
        public string DeclarationId = string.Empty;
        string[] queryParamArray = new string[] { };
        string DcryptString = "";
        public string Ownerlocid = string.Empty;
        public string Ownerorgid = string.Empty;
        public string mytokenvalue = string.Empty;
        public string mysessionId = string.Empty;
        public string Tokensalt = string.Empty;

        public string EncodedToken = string.Empty;
        public string sRefProfName = string.Empty;
        public string sSLUN = string.Empty;
        public string sSLD = string.Empty;
        public string sSLP = string.Empty;
        public string sSFP = string.Empty;
        public string StateId = string.Empty;
        public string FolderStructure = string.Empty;
        public string maqassadocumenttypeForFirstLoad = string.Empty;
        public string ItemAssocitaionFlag = string.Empty;
        //static readonly string PasswordHash = "P@@Sw0rd";
        //static readonly string SaltKey = "S@LT&KEY";
        //static readonly string VIKey = "@1B2c3D4e5F6g7H8";
        //  ItemAssocitaionFlag

         string PasswordHash = "P@@Sw0rd";
         string SaltKey = "S@LT&KEY";
         string VIKey = "@1B2c3D4e5F6g7H8";

       // HttpPostedFileBase[] newfiles;
        int counter = 0;
        public string documenttypeflag = string.Empty;
        public string ThemeId = string.Empty;
        public string hidRefProfile = string.Empty;
        public string sProfileReferenceId = string.Empty;
        public string CommandText = string.Empty;
        ImpersonateUser iU = new ImpersonateUser();
        string sPwd = "";
        SymmetricEncryption CgServices = ServiceFactory.GetSymmetricEncryptionInstance();
        public string Language = string.Empty;

        #endregion
        #region miscellaneous
        public ActionResult Index()
        {
            return View("AuthenticationError");
        }

        public ActionResult CommercialView()
        {
            ViewBag.languageculture = "eng";
            return View("CommercialItems");
        }


        public ActionResult ExpireSession()
        {
            //  string x = "";
            B_upload = new BuisnessUploads();
            mytokenvalue = Session["mytokenvalue"].ToString().Split('|')[0];
            mysessionId = Session["mysessionId"].ToString();

            Tokensalt = Session["mytokenvalue"].ToString().Split('|')[1];
            SqlParameter[] commandParameters1 = new SqlParameter[3];

            commandParameters1[0] = new SqlParameter();
            commandParameters1[0].ParameterName = "@tokenval";
            commandParameters1[0].Value = mytokenvalue;

            commandParameters1[1] = new SqlParameter();
            commandParameters1[1].ParameterName = "@sessionId";
            commandParameters1[1].Value = mysessionId;

            commandParameters1[2] = new SqlParameter();
            commandParameters1[2].ParameterName = "@Tokensalt";
            commandParameters1[2].Value = Tokensalt;

            //  int s = F.SelectedDropdownId;

            ds = B_upload.GetTokenvalue(strConnString, "usp_ExpireSession", commandParameters1);

            //   Response.Cookies.Clear();
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            string[] myCookies = Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }

            B_upload = null;
            return View("AuthenticationError");

        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        #endregion


        #region delete file
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult DeleteFile(string dataItem, string dropdownvalue)
        {
            Elog = new ErrorLogger();
            int InsertResult = 0;
            StringBuilder sb = new StringBuilder();
            string[] encrypteddata = dataItem.Split(',');
            ArrayList decryptedvalue = new ArrayList();
            foreach (var item in encrypteddata)
            {
                dataItem = Decrypt(item);
                if (dataItem.All(char.IsDigit))
                {
                    sb.Append(',' + dataItem);
                }
            }
            dropdownvalue = Decrypt(dropdownvalue);

            if (!dropdownvalue.All(char.IsDigit))
            {
              //  WriteToLogFile("Tampered Dropdown");
                return View("AuthenticationError");

            }

            string s = sb.ToString();
            ModelState.Clear();
            //  dataItem = Decrypt(dataItem);

            setSessionRealtedValues();
            B_upload = new BuisnessUploads();
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[5];
                commandParameters[0] = new SqlParameter("@Id", SqlDbType.VarChar, 8000);
                commandParameters[0].Value = s.TrimStart(',');
                commandParameters[1] = new SqlParameter("@TablePrimaryKey", SqlDbType.VarChar, 100);
                commandParameters[1].Value = TablePrimaryKey;
                
                commandParameters[2] = new SqlParameter("@ProfileName", SqlDbType.VarChar, 50);
                commandParameters[2].Value = ProfileName;
                commandParameters[3] = new SqlParameter();
                commandParameters[3].ParameterName = "@declarationid";
                commandParameters[3].Value = Convert.ToInt32(sProfileReferenceId);
                commandParameters[3].SqlDbType = SqlDbType.Int;

                commandParameters[4] = new SqlParameter();
                commandParameters[4].ParameterName = "@documenttype";
                commandParameters[4].Value = Convert.ToInt32(dropdownvalue);
                commandParameters[4].SqlDbType = SqlDbType.Int;
                
                InsertResult = B_upload.ExecuteNonQuery(strConnString, "Sp_DeleteOperation", commandParameters);
                B_upload = null;
            }
            catch (Exception ex)
            {
                Elog.WriteToLogFile(ex, s, "from Csupload DeleteFile Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");
               // WriteToLogFile(ex, "public ActionResult DeleteFile");

                return View("AuthenticationError");
            }
            var mymsg = Language == "eng" ? ViewBag.Errormessage = InsertResult + "Files Deleted  " : InsertResult + "تم حذف الملفات";

            ViewBag.FriendlyMsg = mymsg;


            SetTablevalue();
            return View("UploadFiles", F);


        }

        #endregion
        #region uploadFile 
        [HttpGet]

        public ActionResult tokenvalue(string tokenvalue)
        {
            //  to be enabled for security testing          
            Elog = new ErrorLogger();
            B_upload = new BuisnessUploads();
            referredUrl = Request.RawUrl.ToString();
            Elog.WriteToLogFile("The First Request From Cs Upload For Url Verification ", "the requested Raw Url'" + referredUrl + "' ");

            Session["referredUrl"] = referredUrl.ToString();

            if (tokenvalue == null)
            {
                Elog.WriteToLogFile(tokenvalue, "from Csupload public ActionResult tokenvalue Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");

                //  WriteToLogFile("Empty Token recd in public ActionResult tokenvalue> '" + tokenvalue + "' ");
                return View("AuthenticationError");

            }

            try


            {
                DcryptString = B_upload.DecryptToken(tokenvalue);
                B_upload = null;
            }
            catch (Exception ex)
            {
                //    WriteToLogFile(ex, "public ActionResult tokenvalue>'" + tokenvalue + "'");
                Elog.WriteToLogFile(ex, "", " from CsUpload public ActionResult tokenvalue Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");

            }

            if (DcryptString != "false")
            {

                queryParamArray = DcryptString.Split('+');
                Session["mysessionId"] = queryParamArray[1];
                Session["mytokenvalue"] = queryParamArray[2];
                return RedirectToAction("UploadFiles");
            }
            else
            {
                Elog = new ErrorLogger();
               Elog.WriteToLogFile(DcryptString, "from Csupload Query String Not Proper Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");

                Elog = null;
                //                WriteToLogFile("Query String Not Proper'" + DcryptString + "'");
                return View("AuthenticationError");
            }
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]

        public ActionResult UploadFiles()
        {
            Elog = new ErrorLogger();
            ObjtokenDetails = new TokenvalueDetails();
            string BrowserDet = Request.Browser.Browser.ToString();
            int version = Request.Browser.MajorVersion;
            var s = Request.Cookies["__RequestVerificationToken"];
            if (version >= 11)
            {
                F.IEVersion = "Supported";
            }
            else
            {
                F.IEVersion = "NotSupported";
            }
            Session["IEVersion"] = F.IEVersion;
            setSessionRealtedValues();
            if (ObjtokenDetails.profileName != null && ObjtokenDetails.profileName != "")
            {
                SetTablevalue();
                F.rowCount = count;
                ViewBag.languageculture = Language;
                return View(F);
            }
            else
            {
                Elog = new ErrorLogger();
                //  WriteToLogFile("Session Doesnot Have ProfileName(token Value>)+'" + mytokenvalue + "' SessionID>'" + mysessionId + "' tokensalt>'" + Tokensalt + "'TokenValue'" + EncodedToken + "'");
                Elog.WriteToLogFile("", "from Csupload Session Doesnot Have ProfileName Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");


                //   Elog.WriteToLogFile("", "from Csupload Session Doesnot Have ProfileName Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");
                Elog = null;
                return View("AuthenticationError");
            }

        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult UploadFiles(HttpPostedFileBase[] files, FormCollection collection, FileModel F)
        {
            Elog = new ErrorLogger();
            try
            {
                
               // B_upload = new BuisnessUploads();
                if (files == null)
                {


                    List<HttpPostedFileBase> postedFiles = new List<HttpPostedFileBase>();

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        postedFiles.Add(Request.Files[i]);
                    }

                    files = postedFiles.ToArray();
                }


                string Desc = F.Description;
                if (F.docsid != null)
                {
                    TempData["selcteddropdwonid"] = Decrypt(F.docsid);
                }

                setSessionRealtedValues();



                if (files.Length != 0)
                {
                    //below added for crf details 
                    if (files[0] != null)
                    {
                        if (ObjtokenDetails != null)
                        {
                            setConfigValues();
                            //  sPwd = "C3M@8a5$D0c";
                            iU.Impersonate(sSLD, sSLUN, sPwd);

                            // testlog("afterImpersonate");
                            List<Filedetails> filedetailsTobesendtoview = new List<Filedetails>();
                            string SelectedFileId = Convert.ToString(collection["SelectedFileId"]);
                            string Description = F.Description;
                            string[] strArray = SelectedFileId.Split('|');
                            counter = strArray.Where(k => k.Contains('.')).Count();

                            //  testlog(counter.ToString());
                            //  testlog(count.ToString());
                            if (counter > count)
                            {
                                bool filesize = true;
                                TempData["fileSize"] = filesize;
                                var mymsg = Language == "eng" ? ViewBag.Errormessage = "You can Upload maximum 5 files at one time " : "يمكنك تحميل 5 ملفات كحد أقصى في وقت واحد";
                                TempData["errormessage"] = ViewBag.Errormessage;
                                return RedirectToAction("afterImpersonatefile");
                            }

                            if (mcTokendoccount >= doccountconfig)
                            {
                                bool filesize = true;
                                TempData["fileSize"] = filesize;
                                var mymsg = Language == "eng" ? ViewBag.Errormessage = "You can Upload maximum 50 files at per session " : "يمكنك تحميل 50 ملف كحد أقصى في كل جلسة";
                                TempData["errormessage"] = ViewBag.Errormessage;
                                return RedirectToAction("afterImpersonatefile");

                            }

                  

                            foreach (string obj in strArray.Where(o => o.Contains('.')))
                            {
 
                                foreach (HttpPostedFileBase file in files.Where(m => Path.GetFileName(m.FileName.ToString()) == obj.Trim()))

                                {
                               if (file != null)
                                    {
                                        // testlog("afterfilecheck");
                                        string fileName = Path.GetFileName(file.FileName);
                                        string GetFileName = Path.GetFileNameWithoutExtension(file.FileName);
                                        string GetExtension = Path.GetExtension(file.FileName);
                                        if (file.ContentLength > FileUploadSizeInKB)
                                        {
                                            bool filesize = true;
                                            TempData["fileSize"] = filesize;
                                            var mymsg = Language == "eng" ? ViewBag.Errormessage = " '" + fileName + "' Exceeds Limit of 500kb " : ViewBag.Errormessage = " '" + fileName + "'يتجاوز الحدkb 500 ميغابايت  ";
                                            TempData["errormessage"] = ViewBag.Errormessage;

                                            return RedirectToAction("afterImpersonatefile");
                                        }
                                        if (file.ContentLength == 0)
                                        {
                                            bool filesize = true;
                                            TempData["fileSize"] = filesize;
                                            var mymsg = Language == "eng" ? ViewBag.Errormessage = " '" + fileName + "' Uploading of 0 bytes files is not allowed " : ViewBag.Errormessage = " '" + fileName + "'لا يمكن تحميل ملف مساحته '0' بايت ";
                                            TempData["errormessage"] = ViewBag.Errormessage;

                                            return RedirectToAction("afterImpersonatefile");
                                        }

                                        //if (file.FileName.Contains("14") || file.FileName.Contains("89"))
                                        //{
                                        //    HttpContext.Current.Response.ContentType = "text/plain";
                                        //    var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                        //    var result = new { name = file.FileName };

                                        //    HttpContext.Current.Response.Write(serializer.Serialize(result));
                                        //    HttpContext.Current.Response.StatusCode = 404;

                                        //   // return new HttpResponseMessage(HttpStatusCode.Forbidden);
                                        //}

                                        string filetobeprocesssed = fileName;


                                        if (filetobeprocesssed.Trim() == obj.Trim())
                                        {
                                            #region changes for database entry
                                            ID = GetNewID(ProfileName);
                                            var s = GetExtension.ToUpper().ToString();
                                            if (!GetExtension.ToUpper().ToString().Contains(".PDF") && !GetExtension.ToUpper().ToString().Contains(".JPEG") && !GetExtension.ToUpper().ToString().Contains(".JPG"))
                                            {
                                                bool filesize = true;
                                                TempData["fileSize"] = filesize;
                                                var mymsg = Language == "eng" ? ViewBag.Errormessage = " '" + fileName + "' Not of Pdf,JPG,JPEG Format " : ViewBag.Errormessage = " '" + fileName + "'نوع الملف غير مسموح بتحميله، الأنواع المسموحة فقط pdf,JPG,JPEG ";
                                                TempData["errormessage"] = ViewBag.Errormessage;
                                                return RedirectToAction("afterImpersonatefile");
                                            }

                                            string Date = DateTime.Now.ToString(FolderStructure); //+ "\\" + DeclarationId;
                                            string year = DateTime.Now.ToString(FolderStructure.Split('-')[0]);
                                            string month = DateTime.Now.ToString(FolderStructure.Split('-')[1]);
                                            string day = DateTime.Now.ToString(FolderStructure.Split('-')[2]);
                                            string FullfileName = GetFileName + ID + GetExtension;
                                            string ShareFolderPath1 = hidRefProfile + "\\" + year + "\\" + month + "\\" + day + "\\" + sProfileReferenceId;// +DeclarationId;
                                            if (!Directory.Exists(Path.Combine(ShareFolderPath, ShareFolderPath1))) Directory.CreateDirectory(Path.Combine(ShareFolderPath, ShareFolderPath1));
                                            string strpath = ShareFolderPath1 + "\\" + FullfileName;
                                            file.SaveAs(Path.Combine(ShareFolderPath, strpath));
                                            // var ServerSavePath = strpath;
                                            if (ScanDocRequestId == "")
                                            {
                                                if (sProfileReferenceId != null && sProfileReferenceId.Trim() != "")
                                                {
                                                    sqlHelp = new SQLHelper();
                                                    SqlParameter[] commandParameters2 = new SqlParameter[1];
                                                    commandParameters2[0] = new SqlParameter("@Declarationid", SqlDbType.VarChar, 100);
                                                    commandParameters2[0].Value = sProfileReferenceId;
                                                    ds = sqlHelp.Returndataset(strConnString, CommandType.StoredProcedure, "Sp_GetScanDocRequestId", commandParameters2);
                                                    sqlHelp = null;
                                                    if (ds.Tables[0].Rows.Count > 0)
                                                        ScanDocRequestId = ds.Tables[0].Rows[0]["ScanDocRequestId"].ToString();
                                                }
                                                SqlParameter[] commandParameters = new SqlParameter[20];
                                                commandParameters[0] = new SqlParameter();
                                                commandParameters[0].ParameterName = "@DocumentId";
                                                commandParameters[0].Value = ID;
                                                commandParameters[1] = new SqlParameter();
                                                commandParameters[1].ParameterName = "@DocumentName";
                                                commandParameters[1].Value = fileName;
                                                commandParameters[2] = new SqlParameter();
                                                commandParameters[2].ParameterName = "@ReferenceId";
                                                commandParameters[2].Value = ReferenceId == "" ? "0" : ReferenceId;
                                                commandParameters[3] = new SqlParameter();
                                                commandParameters[3].ParameterName = "@NewFileName";
                                                commandParameters[3].Value = strpath;
                                                commandParameters[4] = new SqlParameter();
                                                commandParameters[4].ParameterName = "@StateId";
                                                commandParameters[4].Value = StateId;
                                                commandParameters[5] = new SqlParameter();
                                                commandParameters[5].ParameterName = "@Description";
                                                commandParameters[5].Value = F.Description;
                                                commandParameters[6] = new SqlParameter();
                                                commandParameters[6].ParameterName = "@DateCreated";
                                                commandParameters[6].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                                                commandParameters[7] = new SqlParameter();
                                                commandParameters[7].ParameterName = "@ReferenceType";
                                                commandParameters[7].Value = ObjtokenDetails.ReferenceType;
                                                commandParameters[8] = new SqlParameter();
                                                commandParameters[8].ParameterName = "@DeclarationDocumentType";



                                                //  commandParameters[8].Value = Decrypt(F.docsid);
                                                // TempData["selcteddropdwonid"] = Decrypt(F.docsid);


                                                commandParameters[8].Value = Decrypt(F.SelectedDropdownId);
                                                TempData["selcteddropdwonid"] = Decrypt(F.SelectedDropdownId);

                                                commandParameters[9] = new SqlParameter();
                                                commandParameters[9].ParameterName = "@DeclarationId";
                                                commandParameters[9].Value = sProfileReferenceId;
                                                commandParameters[10] = new SqlParameter();
                                                commandParameters[10].ParameterName = "@ReqCreatedRole";
                                                commandParameters[10].Value = RoleId;
                                                commandParameters[11] = new SqlParameter();
                                                commandParameters[11].ParameterName = "@ScanDocRequestId";
                                                commandParameters[11].Value = ObjtokenDetails.ScanDocRequestId;
                                                commandParameters[12] = new SqlParameter();
                                                commandParameters[12].ParameterName = "@RecCreatedBy";
                                                commandParameters[12].Value = ObjtokenDetails.RecCreatedBy;
                                                commandParameters[13] = new SqlParameter();
                                                commandParameters[13].ParameterName = "@UploadedFrom";
                                                commandParameters[13].Value = UploadedFrom;
                                                commandParameters[14] = new SqlParameter();
                                                commandParameters[14].ParameterName = "@ProfileName";
                                                commandParameters[14].Value = ProfileName;
                                                commandParameters[15] = new SqlParameter();
                                                commandParameters[15].ParameterName = "@TablePrimaryKey";
                                                commandParameters[15].Value = TablePrimaryKey;
                                                commandParameters[16] = new SqlParameter();
                                                commandParameters[16].ParameterName = "@Ownerlocid";
                                                commandParameters[16].Value = Ownerlocid;
                                                commandParameters[17] = new SqlParameter();
                                                commandParameters[17].ParameterName = "@ownerorgid";
                                                commandParameters[17].Value = Ownerorgid;

                                                commandParameters[18] = new SqlParameter();
                                                commandParameters[18].ParameterName = "@sessionvalue";
                                                commandParameters[18].Value = mytokenvalue;
                                                // for additional docs request
                                                commandParameters[19] = new SqlParameter();
                                                commandParameters[19].ParameterName = "@AdditionalDocumentId";
                                                commandParameters[19].Value = AdditionalDocumentId;
                                                


                                                B_upload = new BuisnessUploads();
                                                int InsertResult = B_upload.ExecuteNonQuery(strConnString, "usp_UploadInsert", commandParameters);

                                                F.UploadStatus = InsertResult + "file Uploaded Successfully";

                                            }
                                            #endregion
                                        }
                                    }
                                }

                            }
                        }

                        else
                        {
                          //  WriteToLogFile("Token Details Not Found While Upload");
                            return RedirectToAction("Authentication");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                //   WriteToLogFile(ex);
               Elog = new ErrorLogger();
                Elog.WriteToLogFile(ex, "", "from CsUploadInsert Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");

                Elog = null;
                return RedirectToAction("Authentication");
                //  WriteToLogFile(e);
                
            }
            TempData["documentcount"] = documentcount + mcTokendoccount;
            SetTablevalue();

            Array.Clear(files, 0, files.Length);
            return RedirectToAction("afterImpersonatefile", F);

        }



        public ActionResult Authentication()
        {
            return View("AuthenticationError");
        }


        public ActionResult afterImpersonatefile(FileModel F)
        {
            Elog = new ErrorLogger();
            FileModel MyDetails = (FileModel)TempData["Fmodel"];
            bool size = Convert.ToBoolean(TempData["fileSize"]);
            if (size)
            {
                ViewBag.Errormessage = TempData["errormessage"];
                //   WriteToLogFile("from public ActionResult afterImpersonatefile");
                Elog.WriteToLogFile( "", " from CsUpload public ActionResult afterImpersonatefile Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");
                Elog = null;

                return View("Error");
            }
            ViewBag.languageculture = MyDetails.languagesession;

            MyDetails.IEVersion = Session["IEVersion"].ToString();
            if (Session["languageid"].ToString() == "eng")
            {
                ViewBag.ThemeId = Session["SThemeId"].ToString() + ".css";
            }
            else
            {
                ViewBag.ThemeId = Session["SThemeId"].ToString() + "_ara.css";
            }

            ViewBag.UploadStatus = MyDetails.UploadStatus;


            TempData.Remove("Fmodel");

            return View("UploadFiles", MyDetails);
        }

        #endregion Upload
        

        #region download File 
        public ActionResult DownloadFile(string fileName)
        {
            Elog = new ErrorLogger();
            string NewFileName = string.Empty;
            string MyfileName = string.Empty; string strpath = string.Empty;
            string id = Decrypt(fileName);
            if (id != "")
            {
                try
                {
                    setSessionRealtedValues();


                    SqlParameter[] commandParameters = new SqlParameter[4];
                    commandParameters[0] = new SqlParameter();
                    commandParameters[0].ParameterName = "@Id";
                    commandParameters[0].Value = id;
                    commandParameters[0].SqlDbType = SqlDbType.Int;

                    commandParameters[1] = new SqlParameter();
                    commandParameters[1].ParameterName = "@TablePrimaryKey";
                    commandParameters[1].Value = TablePrimaryKey;
                    commandParameters[2] = new SqlParameter();
                    commandParameters[2].ParameterName = "@ProfileName";
                    commandParameters[2].Value = ProfileName;
                    commandParameters[3] = new SqlParameter();
                    commandParameters[3].ParameterName = "@declarationid";
                    commandParameters[3].Value = Convert.ToInt32(sProfileReferenceId);
                    commandParameters[3].SqlDbType = SqlDbType.Int;


                    B_upload = new BuisnessUploads();


                    // ds = B_upload.GetTokenvalue(strConnString, "Sp_DwonloadFile", commandParameters);
                    ds = B_upload.GetTokenvalue(strConnString, "Sp_DwonloadFile", commandParameters);
                    B_upload = null;

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        NewFileName = ds.Tables[0].Rows[0]["NewFileName"].ToString();
                        MyfileName = ds.Tables[0].Rows[0]["DocumentName"].ToString();
                    }
                    ImpersonateUser iU = new ImpersonateUser();
                    SymmetricEncryption CgServices = ServiceFactory.GetSymmetricEncryptionInstance(); ;
                    setConfigValues();
                    iU.Impersonate(sSLD, sSLUN, sPwd);
                    strpath = Path.Combine(ShareFolderPath, NewFileName);

                    // fro arabic handlers 
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlPathEncode(MyfileName));
                    // path =\\10.10.65.3\kgac_upload_dd_test\Declarations\2017\11\21\9167559\
                    //   return File(path, "application/pdf", fileName + ".pdf");
                    if (MyfileName.ToLower().ToString().Contains(".pdf"))
                    {
                        return File(strpath, "application/pdf", ".pdf");
                    }
                    else
                    {
                        return File(strpath, "image/jpeg");
                    }


                }
                catch (Exception ex)
                {
                    //  WriteToLogFile(ex);

                    Elog.WriteToLogFile(ex, "", " from CsUploadpublic ActionResult download file  Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");
                    Elog = null;
                }

            }
            ViewBag.Errormessage = "Error In downloading File";
            return View("Error");



        }
        #endregion
        #region page related functions 






        public void SetTablevalue()
        {
           // Elog = new ErrorLogger();
            setConfigValues();
            B_upload = new BuisnessUploads();
            //SqlParameter[] commandParameters1 = new SqlParameter[3];
            //commandParameters1[0] = new SqlParameter();
            //commandParameters1[0].ParameterName = "@DeclarationId";
            //commandParameters1[0].Value = DeclarationId;
            //commandParameters1[1] = new SqlParameter();
            //commandParameters1[1].ParameterName = "@LanguageId";
            //commandParameters1[1].Value = Language;

            //commandParameters1[2] = new SqlParameter();
            //commandParameters1[2].ParameterName = "@RefProfile";
            //commandParameters1[2].Value = hidRefProfile;


            SqlParameter[] commandParameters1 = new SqlParameter[4];
            commandParameters1[0] = new SqlParameter();
            commandParameters1[0].ParameterName = "@DeclarationId";
            commandParameters1[0].Value = DeclarationId;
            commandParameters1[1] = new SqlParameter();
            commandParameters1[1].ParameterName = "@LanguageId";
            commandParameters1[1].Value = Language;

            commandParameters1[2] = new SqlParameter();
            commandParameters1[2].ParameterName = "@RefProfile";
            commandParameters1[2].Value = hidRefProfile;


            commandParameters1[3] = new SqlParameter();
            commandParameters1[3].ParameterName = "@AdditionalDocRequestId";
            commandParameters1[3].Value = AdditionalDocumentId;

            ds = B_upload.GetTokenvalue(strConnString, "getreqdocsddldata", commandParameters1);
            //   var maqassadocumenttypeForFirstLoad = ds.Tables[0].Rows[0]["DeclarationDocumentId"];
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                maqassadocumenttypeForFirstLoad = ds.Tables[0].Rows[0]["DeclarationDocumentId"].ToString();

                int selectedindex = -1;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (DocumentId != "" && ds.Tables[0].Rows[i]["DeclarationDocumentId"].ToString() == DocumentId)
                    {
                        selectedindex = i + 1;
                    }
                }

                var DropdownBind = from s in ds.Tables[0].AsEnumerable()
                                   select new SelectListItem
                                   {
                                       Text = s["Name"].ToString() + " (" + s["Cnt"] + ") ",
                                       Value = Encrypt(s["DeclarationDocumentId"].ToString()),
                                       // Value = s["DeclarationDocumentId"].ToString(),

                                       Selected = (s["DeclarationDocumentId"].ToString() == DocumentId ? true : false)

                                   };




                ViewBag.TableList = DropdownBind;
                F.text = DocumentId;

                F.disabled = documenttypeflag;

                F.ddlDocumentTypesitems = DropdownBind.ToList();

                F.languagesession = Language;
                CommandText = string.Empty;
                SqlParameter[] commandParameters = new SqlParameter[8];
                commandParameters[0] = new SqlParameter("@LanguageId", SqlDbType.VarChar, 5);
                commandParameters[0].Value = Language;
                commandParameters[1] = new SqlParameter("@TablePrimaryKey", SqlDbType.VarChar, 300);
                commandParameters[1].Value = TablePrimaryKey;
                commandParameters[2] = new SqlParameter("@ProfileName", SqlDbType.VarChar, 300);
                commandParameters[2].Value = ProfileName;
                commandParameters[3] = new SqlParameter("@ReferenceType", SqlDbType.VarChar, 12);
                commandParameters[3].Value = ReferenceType;
                commandParameters[4] = new SqlParameter("@ReferenceId", SqlDbType.VarChar, 12);
                commandParameters[4].Value = sProfileReferenceId;
                commandParameters[5] = new SqlParameter("@DocumentId", SqlDbType.Int, 12);
                int selecteddropdwonid = 0;
                if (Convert.ToString(TempData["selcteddropdwonid"]) != "")
                {

                    selecteddropdwonid = Convert.ToInt32(TempData["selcteddropdwonid"]);
                    TempData.Remove("selcteddropdwonid");
                }
                if (selecteddropdwonid != 0)
                {
                    commandParameters[5].Value = selecteddropdwonid;
                }
                else
                {
                    if (DocumentId != "")

                    {
                        commandParameters[5].Value = DocumentId;
                    }
                    else
                    {
                        commandParameters[5].Value = ds.Tables[0].Rows[0]["DeclarationDocumentId"].ToString();

                    }
                }
                if (!commandParameters[5].Value.ToString().All(char.IsDigit))
                {
                    Decrypt(commandParameters[5].Value.ToString());
                }
                selecteddropdwonid = 0;

                //  maqassadocumenttypeForFirstLoad = "";
                var m = maqassadocumenttypeForFirstLoad;
                var D = DocumentId;
                this.DocumentId = string.Empty;
                this.maqassadocumenttypeForFirstLoad = string.Empty;

                maqassadocumenttypeForFirstLoad = "";
                commandParameters[6] = new SqlParameter("@UploadedFrom", SqlDbType.VarChar, 300);
                commandParameters[6].Value = UploadedFrom;
           


                commandParameters[7] = new SqlParameter();
                commandParameters[7].ParameterName = "@AdditionalDocRequestId";
                commandParameters[7].Value = AdditionalDocumentId;
                // string tok = "mttoke";
                // for grid 
                ds = B_upload.GetTokenvalue(strConnString, "usp_GetUploadedDocumentsInfo", commandParameters);
                commandParameters[5].Value = "";
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {


                    TempData["TablePrimaryKey"] = TablePrimaryKey;
                    TempData["ProfileName"] = ProfileName;
                                              var docsUploadedlist = from s in ds.Tables[0].AsEnumerable()
                                         select new Filedetails
                                           {

                                               //  }),
                                               Fileid = Encrypt(s["Documentid"].ToString()),
                                               shortFileName = s["documentname"].ToString(),
                                               Filename = s["documentname"].ToString(),
                                               createdBy = s["createdby"].ToString(),
                                               description = s["description"].ToString(),
                                               DocumentType = s["name"].ToString(),
                                               Uploadeddate = s["DateCreated"].ToString()
                                           };

                    F.listofuploadedFiles = docsUploadedlist.ToList();
                    //  F.UploadStatus = "file uploaded successfully";
                }


                else
                {
                    Elog = new ErrorLogger();
                    // WriteToLogFile("No data Found For usp_GetUploadedDocumentsInfo Parametere used '" + Language + "''" + TablePrimaryKey + "' '" + ProfileName + "''" + ReferenceType + "'documentid>>'" + D + "'maqassaDocumnettype>>'" + m + "','" + sProfileReferenceId + "''" + UploadedFrom + "'");
                    Elog.WriteToLogFile(" From Csupload No data Found For usp_GetUploadedDocumentsInfo Parametere used '" + Language + "''" + TablePrimaryKey + "' '" + ProfileName + "''" + ReferenceType + "'documentid>>'" + D + "'maqassaDocumnettype>>'" + m + "','" + sProfileReferenceId + "''" + UploadedFrom + "'", " and   Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");
                    Elog = null;

                    //   WriteToLogFile("No data Found For usp_GetUploadedDocumentsInfo Parametere used ");
                }

                TempData["Fmodel"] = F;
            }
            else
            {
                //  WriteToLogFile("No data Found For getreqdocsddldata parameter used '" + DeclarationId + "' '" + hidRefProfile + "' '" + Language + "'");

                Elog = new ErrorLogger();
                Elog.WriteToLogFile( "", " From Csupload No data Found For getreqdocsddldata parameter used '" + DeclarationId + "' '" + hidRefProfile + "' '" + Language + "' Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");

                Elog = null;
            }





        }
        public void setSessionRealtedValues()
        {

            int x = documentcount;
            ds = new DataSet();
       //     Elog = new ErrorLogger();
            B_upload = new BuisnessUploads();
            try
            {
                mytokenvalue = Session["mytokenvalue"].ToString().Split('|')[0];
                mysessionId = Session["mysessionId"].ToString();

                Tokensalt = Session["mytokenvalue"].ToString().Split('|')[1];
                SqlParameter[] commandParameters1 = new SqlParameter[3];

                commandParameters1[0] = new SqlParameter();
                commandParameters1[0].ParameterName = "@tokenval";
                commandParameters1[0].Value = mytokenvalue;

                commandParameters1[1] = new SqlParameter();
                commandParameters1[1].ParameterName = "@sessionId";
                commandParameters1[1].Value = mysessionId;

                commandParameters1[2] = new SqlParameter();
                commandParameters1[2].ParameterName = "@Tokensalt";
                commandParameters1[2].Value = Tokensalt;


                //  int s = F.SelectedDropdownId;
                ds = B_upload.GetTokenvalue(strConnString, "usp_GetTokenInfo_DocUpload", commandParameters1);

                if (ds.Tables.Count != 0)
                {
                    if (ds != null && ds.Tables["FileAndTokenInfo"].Rows.Count > 0)
                    {
                        if (ObjtokenDetails == null)
                            ObjtokenDetails = new TokenvalueDetails();
                        // session refernece profile only 
                        // ObjtokenDetails.ReferenceProfile = ds.Tables[0].Rows[0]["ReferenceProfile"].ToString();
                        F.tokencreatedby = ds.Tables[0].Rows[0]["ReqCreatedBy"].ToString();
                        F.isbroker = ds.Tables[0].Rows[0]["isbroker"].ToString();
                        UploadedFrom = ds.Tables[0].Rows[0]["ReferenceProfile"].ToString();
                        DocumentId = ds.Tables[0].Rows[0]["DocumentId"].ToString();
                        hidRefProfile = ds.Tables[0].Rows[0]["ReferenceProfile"].ToString();
                       // ObjtokenDetails.ProfileReferenceId = Convert.ToInt32(ds.Tables[0].Rows[0]["ReferenceId"]);
                        sProfileReferenceId = ds.Tables[0].Rows[0]["ReferenceId"].ToString();
                        DeclarationId = ds.Tables[0].Rows[0]["ReferenceId"].ToString();
                        F.DeclarationId = ds.Tables[0].Rows[0]["ReferenceId"].ToString();
                        ObjtokenDetails.profileName = ds.Tables[0].Rows[0]["profileName"].ToString();
                        ProfileName = ds.Tables[0].Rows[0]["profileName"].ToString();
                        TablePrimaryKey = B_upload.GetPrimaryKey(ProfileName, strConnString);
                       // ObjtokenDetails.RoleID = ds.Tables[0].Rows[0]["Roleid"].ToString();
                        RoleId = ds.Tables[0].Rows[0]["Roleid"].ToString();
                       // ObjtokenDetails.LanguageId = ds.Tables[0].Rows[0]["LanguageId"].ToString();
                        Language = ds.Tables[0].Rows[0]["LanguageId"].ToString();
                       // ObjtokenDetails.DeclarationId = ds.Tables[0].Rows[0]["ReferenceId"].ToString();
                        ObjtokenDetails.ScanDocRequestId = ds.Tables[0].Rows[0]["ScanDocRequestId"].ToString();
                        ObjtokenDetails.ReferenceType = ds.Tables[0].Rows[0]["ReferenceType"].ToString();
                        ReferenceType = ds.Tables[0].Rows[0]["ReferenceType"].ToString();
                        Ownerlocid = ds.Tables[0].Rows[0]["ownerlocid"].ToString();
                        Ownerorgid = ds.Tables[0].Rows[0]["OwnerOrgId"].ToString();
                        ObjtokenDetails.RecCreatedBy = ds.Tables[0].Rows[0]["ReqCreatedBy"].ToString();
                        ThemeId = ds.Tables[0].Rows[0]["ThemeId"].ToString();
                        F.islocked = ds.Tables[0].Rows[0]["Islocked"].ToString();
                        F.IsUploadLocked = ds.Tables[0].Rows[0]["IsUploadLocked"].ToString();
                        F.ItemAssocitaionFlag = ItemAssocitaionFlag;

                        ViewBag.languageculture = ds.Tables[0].Rows[0]["LanguageId"].ToString();
                        Session["languageid"] = ds.Tables[0].Rows[0]["LanguageId"].ToString();
                        Session["SThemeId"] = ThemeId.ToString();
                        F.languagesession = Language;
                        F.ReferenceCaption = ds.Tables[0].Rows[0]["ReferenceCaption"].ToString();
                        F.ReferenceNo = ds.Tables[0].Rows[0]["ReferenceNo"].ToString();
                        mcTokendoccount = Convert.ToInt32(ds.Tables[0].Rows[0]["countfordocuments"]);
                        AdditionalDocumentId = Convert.ToInt32(ds.Tables[0].Rows[0]["AdditionalDocumentId"]);

                        B_upload = null;
                        //   Language = "eng";

                        if (Language == "eng")
                        {
                            ViewBag.ThemeId = ThemeId + ".css";
                        }
                        else
                        {
                            ViewBag.ThemeId = ThemeId + "_ara.css";
                        }
                    }
                }
                else
                {
                    Elog = new ErrorLogger();
                    Elog.WriteToLogFile(" from CsUpload token Value Is null DataBase Returned no records ", "Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");
                    //    Elog.WriteToLogFile(ex, "", "tokenValueIsnull page refreshed without any token  Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");
                    Elog = null;
                }


            }

            catch (Exception ex)
            {
                if (mytokenvalue == "" || mysessionId == "" || Tokensalt == "")
                {
                    //    WriteToLogFile("tokenValueIsnull page refreshed without any token");
                    Elog = new ErrorLogger();
                    // Elog.WriteToLogFile(ex, "", "   from CsUpload   tokenValueIsnull page refreshed without any token  Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");
                    Elog.WriteToLogFile(ex, "tokenValueIsnull page refreshed without any token", "");

                    Elog = null;


                }
                else
                {
                    Elog = new ErrorLogger();
                    //    WriteToLogFile(ex, " From public void setSessionRealtedValues())");
                    //                    Elog.WriteToLogFile("token Value Is null DataBase Returned no records ", "Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");

                    Elog.WriteToLogFile(ex, "", "  from CsUpload Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");
                    Elog = null;

                }

            }
        }
        public void setConfigValues()
        {
            ds = new DataSet();
        
            SqlParameter[] commandParameters1 = new SqlParameter[2];
            commandParameters1[0] = new SqlParameter();
            commandParameters1[0].ParameterName = "@ProfileName";
            commandParameters1[0].Value = ProfileName;
            commandParameters1[1] = new SqlParameter();
            commandParameters1[1].ParameterName = "@hidrefprofile";
            commandParameters1[1].Value = hidRefProfile;

            B_upload = new BuisnessUploads();

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
                    FileUploadSizeInKB = Convert.ToInt32(ds.Tables[0].Rows[0]["MaxSize"]);
                    FolderStructure = ds.Tables[0].Rows[0]["FolderStructure"].ToString();
                    StateId = ds.Tables[0].Rows[0]["ProfileStateId"].ToString();
                    sPwd = ds.Tables[0].Rows[0]["SRDocumentsShareFolderPath"].ToString();
                    ShareFolderPath = sSFP;
                    sPwd = CgServices.DecryptData(sSLP);
                    count = Convert.ToInt32(ds.Tables[0].Rows[0]["Count"]);
                    documenttypeflag = ds.Tables[0].Rows[0]["documenttypeflag"].ToString().Trim();

                    ItemAssocitaionFlag = ds.Tables[0].Rows[0]["ItemAssocitaionFlag"].ToString().Trim();
                    F.ItemAssocitaionFlag = ItemAssocitaionFlag;
                    F.ItemAssocitaionprofilename = ProfileName;
                    doccountconfig = Convert.ToInt32(ds.Tables[0].Rows[0]["doccountconfig"]);

                    B_upload = null;


                }
                else
                {
                    //  WriteToLogFile("Configuration Value Not Found For '" + ProfileName + "' and encodedtoken='" + EncodedToken + "' and  tokenvalue='" + mytokenvalue + "'");
                    Elog = new ErrorLogger();
                    Elog.WriteToLogFile( "", " from CsUpload Configuration Value Not Found For '" + ProfileName + "' and encodedtoken='" + EncodedToken + "' Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");
                    Elog = null;


                }

            }
            catch (Exception ex)
            {
                // WriteToLogFile(ex, "public void setConfigValues() ProfileName >'" + ProfileName + "' hidrefProfile>'" + hidRefProfile + "'");
                Elog = new ErrorLogger();
                Elog.WriteToLogFile(ex, "", " from CsUpload public void setConfigValues() ProfileName >'" + ProfileName + "' hidrefProfile>'" + hidRefProfile + "' Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");

                Elog = null;

            }
        }




        public string GetNewID(string profile)
        {
            sqlHelp = new SQLHelper();
            return sqlHelp.GetNewIntKey(profile);

        }
        //private string GetNewIntKey(string sCounterName)
        //{
        //    Int64 counterValueStart;
        //    Int64 counterValueEnd;
        //    GetNewIntCounter(sCounterName, 1, out counterValueStart, out counterValueEnd);
        //    return counterValueStart.ToString();
        //}
        //private Int64 GetNewIntCounter(string sCounterName, Int64 seedValue, out Int64 counterValueStart, out Int64 counterValueEnd)
        //{
        //    sqlHelp = new SQLHelper();
        //    string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
        //    SqlParameter[] commandParameters = new SqlParameter[4];
        //    commandParameters[0] = new SqlParameter("@DataSourceName", SqlDbType.VarChar, 50);
        //    commandParameters[1] = new SqlParameter("@SeedValue", SqlDbType.BigInt);
        //    commandParameters[2] = new SqlParameter("@CounterValueStart", SqlDbType.BigInt);
        //    commandParameters[3] = new SqlParameter("@CounterValueEnd", SqlDbType.BigInt);
        //    commandParameters[2].Direction = ParameterDirection.Output;
        //    commandParameters[3].Direction = ParameterDirection.Output;
        //    commandParameters[0].Value = sCounterName;
        //    commandParameters[1].Value = seedValue;



        //    sqlHelp.ExecuteNonQueryWithOutvalue("usp_MCPKCounters", commandParameters);
        //    sqlHelp = null;
        //    counterValueStart = (Int64)commandParameters[2].Value;
        //    counterValueEnd = (Int64)commandParameters[3].Value;
        //    return (Int64)commandParameters[2].Value;

        //}


        #region commentedcodeForLogger

        //public void WriteToLogFile(Exception ex)
        //{
        //    using (EventLog eventLog = new EventLog("Application"))
        //    {
        //        eventLog.Source = "Application";
        //        eventLog.WriteEntry(" from Csupload  (referredUrl='" + Session["referredUrl"].ToString() + "')    and tokenValue='" + Session["mytokenvalue"].ToString() + "' and sessionID='" + Session["mysessionId"].ToString() + "'and tokensalt='" + Tokensalt + "' " + ex.Message + ex.StackTrace, EventLogEntryType.Information, 101, 1);
        //    }
        //}

        //public void WriteToLogFile(Exception ex, string inputvalue)
        //{
        //    using (EventLog eventLog = new EventLog("Application"))
        //    {
        //        eventLog.Source = "Application";
        //        eventLog.WriteEntry(" from Csupload Param Information=>'" + inputvalue + "' (referredUrl='" + Session["referredUrl"].ToString() + "')    and tokenValue='" + Session["mytokenvalue"].ToString() + "' and sessionID='" + Session["mysessionId"].ToString() + "'and tokensalt='" + Tokensalt + "' " + ex.Message + ex.StackTrace, EventLogEntryType.Information, 101, 1);
        //    }
        //}

        //public void WriteToLogFile(string errormessage)
        //{

        //    using (EventLog eventLog = new EventLog("Application"))
        //    {
        //        eventLog.Source = "Application";
        //        if (mytokenvalue.ToString() != "")
        //        {
        //            eventLog.WriteEntry(" from Csupload  (referredUrl='" + Session["referredUrl"].ToString() + "')    and tokenValue='" + Session["mytokenvalue"].ToString() + "' and sessionID='" + Session["mysessionId"].ToString() + "'and tokensalt='" + Tokensalt + "' " + errormessage, EventLogEntryType.Information, 101, 1);

        //        }
        //        else
        //        {
        //            eventLog.WriteEntry(" from Csupload'" + errormessage + "'");                  //  eventLog.WriteEntry(" from Csupload  (referredUrl='" + Session["referredUrl"].ToString() + "')    and tokenValue='" + Session["mytokenvalue"].ToString() + "' and sessionID='" + Session["mysessionId"].ToString() + "'and tokensalt='" + Tokensalt + "' " + errormessage, EventLogEntryType.Information, 101, 1);

        //        }

        //    }
        //}

        #endregion
        public string GetEncriptionString(string prefix, string sToken, string[] suffix)
        {
            string encodedToken = "";

            try
            {
                string DocManagementKey = System.Configuration.ConfigurationSettings.AppSettings["DocManagementSecurityKey"].ToString();
                byte[] DocManagementKeyBytes = Encoding.UTF8.GetBytes(DocManagementKey);

                string sprefix = prefix.Trim() == "" ? prefix : prefix + "+";// +"+"+sToken
                string sEncryptingValue = "";

                string sSuffixVal = "";
                for (int i = 0; i < suffix.Length; i++)
                {
                    if (sSuffixVal == "")
                        sSuffixVal = suffix[i];
                    else
                        sSuffixVal = sSuffixVal + "+" + suffix[i];
                }
                if (sSuffixVal != "")
                    sEncryptingValue = sprefix + sToken + "+" + sSuffixVal;
                else
                    sEncryptingValue = sprefix + sToken;

                string sNewToken = SymmetricEncryption.NewInstance().EncryptStringToBytes(sEncryptingValue, DocManagementKeyBytes, DocManagementKeyBytes);
                encodedToken = HttpUtility.UrlEncode(sNewToken);
            }
            catch (Exception ex)
            {
                Elog = new ErrorLogger();
                //    WriteToLogFile(ex, "public string GetEncriptionString Prefix>'" + prefix + "' and sToken>'" + sToken + "'");
                Elog.WriteToLogFile(ex, "", " from CsUpload public string GetEncriptionString Prefix>'" + prefix + "' and sToken>'" + sToken + "' Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");
                Elog = null;

            }

            return encodedToken;
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
                //     WriteToLogFile(ex, "public string Decrypt(string encryptedText>" + encryptedText);
                //                public string Decrypt(string encryptedText)
                Elog.WriteToLogFile(ex, "", " from CsUpload public string Decrypt  Param Information => (referredUrl = '" + Session["referredUrl"].ToString() + "')    and tokenValue = '" + Session["mytokenvalue"].ToString() + "' and sessionID = '" + Session["mysessionId"].ToString() + "'and tokensalt = '" + Tokensalt + "'");

                Elog = null;

                //  WriteToLogFile(e);
            }
            return x;
        }
        #endregion




    }
}