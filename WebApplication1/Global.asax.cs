using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebApplication1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

          

        }
    

        protected void Application_BeginRequest()
        {

            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.AddHeader("pragma", "no-cache");
            Response.AddHeader("Cache-Control", "no-cache");
            Response.CacheControl = "no-cache";
            Response.Expires = -1;
            Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            Response.Cache.SetNoStore();
            Response.Cache.SetAllowResponseInBrowserHistory(false);



        }

        void Application_EndRequest(object sender, EventArgs e)
        {
            // if login failed then display user friendly error page.
            if (Response.StatusCode == 403|| Response.StatusCode == 404)
            {
                Response.ClearContent();
                Server.Transfer("~/DocumentError.aspx");
                Response.StatusCode = 200;
            }
        }
        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");

            Response.Headers.Remove("X-AspNet-Version");

            Response.Headers.Remove("X-AspNetMvc-Version");

            MvcHandler.DisableMvcResponseHeader = true;
        }

    


    }
}
