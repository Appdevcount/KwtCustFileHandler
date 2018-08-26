using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DocumentManagementServices
{
    public partial class Main : System.Web.UI.Page
    {
        RequiredDocumentsToken RequiredAct = new RequiredDocumentsToken();
        public Page basepage = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            string sToken = "";
            string[] queryParamArray = new string[] { };
            string DcryptString = "";
            if (Request.QueryString["TokenValue"] != null)
            {

                sToken = Convert.ToString(Request.QueryString["TokenValue"]);
                DcryptString = RequiredAct.DecryptToken(sToken);
                if (DcryptString.IndexOf("+") == -1)
                    Server.Transfer("~/Errorpage.aspx");
                queryParamArray = DcryptString.Split('+');
                if (queryParamArray.Length != 7)
                    Server.Transfer("~/Errorpage.aspx");
                else
                {
                   // Session["MainViewCnt"] ="1";
                    string sGuid = System.Guid.NewGuid().ToString("N");
                    HttpCookie ViewCookie = new HttpCookie("ViewCookie");
                    ViewCookie.Value = sGuid;// queryParamArray[1]; ;// queryParamArray[1];
                    Response.Cookies.Add(ViewCookie);
                    Session["ViewCookie"] = sGuid;// Guid.NewGuid();// queryParamArray[1];  
                    Server.Transfer("~/RequiredDocumentsUploadToken.aspx");
                }
            }
            //else if (Request.QueryString["Token"] != null && Session["MainViewCnt"] != null && Session["MainViewCnt"].ToString() == "1")
            else if (Request.QueryString["Token"] != null && Request.Cookies["ViewCookie"] != null && Session["ViewCookie"] != null && Request.Cookies["ViewCookie"].Value == Session["ViewCookie"].ToString())
            {
                sToken = Convert.ToString(Request.QueryString["Token"]);
                DcryptString = RequiredAct.DecryptToken(sToken);
                if (DcryptString.IndexOf("+") == -1)
                    Server.Transfer("~/Errorpage.aspx");
                queryParamArray = DcryptString.Split('+');
                if (queryParamArray.Length != 5)
                    Server.Transfer("~/Errorpage.aspx");
                else
                {
                    Session["ViewCookie"] = Request.Cookies["ViewCookie"].Value;
                    Server.Transfer("~/RenderFileToken.aspx");
                }
            }
            else
                Server.Transfer("~/Errorpage.aspx");

            //if (Request.QueryString["TokenValue"] != null)
            //{
            //    Session["MainTokenValue"] = Request.QueryString["TokenValue"].ToString();
            //    Response.Redirect("~/Main.aspx", true);
            //}
            //else
            //{
                //Server.Transfer("~/RequiredDocumentsUploadToken.aspx");
            //}
        }
    }
}