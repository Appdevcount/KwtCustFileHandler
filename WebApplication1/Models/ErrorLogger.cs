using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
namespace WebApplication1.Models
{
    public class ErrorLogger
    {


        public void WriteToLogFile(Exception ex, string inputvalue, string sessionDetails)
        {
            //using (EventLog eventLog = new EventLog("Application"))
            //{
            //    eventLog.Source = "Application";
            // ///   eventLog.WriteEntry(sessionDetails + "InputValue>" + inputvalue+ ex.Message + ex.StackTrace, EventLogEntryType.Information, 101, 1);
            //}
        }


        public void WriteToLogFile(string inputvalue, string sessionDetails)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry(sessionDetails +"Inpu" +
                    "tValue>"+inputvalue, EventLogEntryType.Information, 101, 1);
            }
        }

    }
}


    //    public void WriteToLogFile(Exception ex)
    //    {
    //        using (EventLog eventLog = new EventLog("Application"))
    //        {
    //            eventLog.Source = "Application";
    //            eventLog.WriteEntry(" from Csupload  (referredUrl='" + Session["referredUrl"].ToString() + "')    and tokenValue='" + Session["mytokenvalue"].ToString() + "' and sessionID='" + Session["mysessionId"].ToString() + "'and tokensalt='" + Tokensalt + "' " + ex.Message + ex.StackTrace, EventLogEntryType.Information, 101, 1);
    //        }
    //    }

    //    public void WriteToLogFile(Exception ex, string inputvalue)
    //    {
    //        using (EventLog eventLog = new EventLog("Application"))
    //        {
    //            eventLog.Source = "Application";
    //            eventLog.WriteEntry(" from Csupload Param Information=>'" + inputvalue + "' (referredUrl='" + Session["referredUrl"].ToString() + "')    and tokenValue='" + Session["mytokenvalue"].ToString() + "' and sessionID='" + Session["mysessionId"].ToString() + "'and tokensalt='" + Tokensalt + "' " + ex.Message + ex.StackTrace, EventLogEntryType.Information, 101, 1);
    //        }
    //    }

    //    public void WriteToLogFile(string errormessage)
    //    {

    //        using (EventLog eventLog = new EventLog("Application"))
    //        {
    //            eventLog.Source = "Application";
    //            if (mytokenvalue.ToString() != "")
    //            {
    //                eventLog.WriteEntry(" from Csupload  (referredUrl='" + Session["referredUrl"].ToString() + "')    and tokenValue='" + Session["mytokenvalue"].ToString() + "' and sessionID='" + Session["mysessionId"].ToString() + "'and tokensalt='" + Tokensalt + "' " + errormessage, EventLogEntryType.Information, 101, 1);

    //            }
    //            else
    //            {
    //                eventLog.WriteEntry(" from Csupload'" + errormessage + "'");                  //  eventLog.WriteEntry(" from Csupload  (referredUrl='" + Session["referredUrl"].ToString() + "')    and tokenValue='" + Session["mytokenvalue"].ToString() + "' and sessionID='" + Session["mysessionId"].ToString() + "'and tokensalt='" + Tokensalt + "' " + errormessage, EventLogEntryType.Information, 101, 1);

    //            }

    //        }
    //    }

    //}
