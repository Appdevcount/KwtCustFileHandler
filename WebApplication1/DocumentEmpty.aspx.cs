using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class DocumentEmpty : System.Web.UI.Page
    {
        public string TypeOfFilter = string.Empty;
        public string ResponsebackUrl = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
        ResponsebackUrl= Session["referredUrl"].ToString();
            if (Request.QueryString["TypeOfFilter"] != "" && Request.QueryString["TypeOfFilter"] != null)
            {
                TypeOfFilter = (Request.QueryString["TypeOfFilter"]).ToString();
            }
        }
    }
}