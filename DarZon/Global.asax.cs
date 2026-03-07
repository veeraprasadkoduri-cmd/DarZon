using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using SAPbobsCOM;
using DarZon.Models;
namespace DarZon
{
    public class MvcApplication : System.Web.HttpApplication
    {
        string DBType = "", DBserver = "", DBUname = "", DBPwd = "", Sub = "", SAPUSER = "", SAPPWD = "", COMPANYDB = "";
        public static int sapconnectionflga = 1;
        public static SAPbobsCOM.Company oCompany;
        public static string sErrMsg;
        public static int lErrCode;
        public static int lRetCode;
        public static int lRetCode1;
        public static int lRetCode2;
        public static int lRetCode3;
       protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
           RouteConfig.RegisterRoutes(RouteTable.Routes);
           
        }
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
           //usercheck();
           
        }
        public void usercheck()
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null && !authTicket.Expired)
                {
                    var roles = authTicket.UserData.Split(',');
                    HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new FormsIdentity(authTicket), roles);
                }
            }


        }

        //public int SAPConnect()
        //{

        //    DBType = System.Configuration.ConfigurationManager.AppSettings["DBType"];
        //    DBserver = System.Configuration.ConfigurationManager.AppSettings["DBServer"];
        //    DBUname = System.Configuration.ConfigurationManager.AppSettings["DBUname"];
        //    DBPwd = System.Configuration.ConfigurationManager.AppSettings["DBPwd"];
        //    try
        //    {
        //        oCompany = new SAPbobsCOM.Company();
        //        oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2012;
        //        //Program.oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2008;
        //        oCompany.Server = DBserver; // change to your company server                
        //        oCompany.UseTrusted = false;
        //        oCompany.DbUserName = DBUname;
        //        oCompany.DbPassword = DBPwd;

        //        try
        //        {
        //            oRecordSet = oCompany.GetCompanyList(); // get the company list
        //        }
        //        catch (Exception ex)
        //        {
        //            //Logger.LogInfo(ex.Message.ToString());
        //            // Logger.LogInfo(ex);
        //            return 1;
        //        }

        //        int temp_int = lErrCode;
        //        string temp_string = sErrMsg;
        //        oCompany.GetLastError(out temp_int, out temp_string);

        //        if (lErrCode != 0)
        //        {
        //            // Logger.LogInfo(sErrMsg);
        //        }
        //        else
        //        {
        //            if (!(oRecordSet.EoF == true))
        //            {
        //                dbAvail = true;
        //            }
        //        }
        //        if (!dbAvail)
        //        {
        //            //Logger.LogInfo("There was no Database Found...");
        //            return 1;
        //        }
        //        if (oCompany.Connected) // if already connected
        //        {
        //        }
        //        return (Connect());

        //    }
        //    catch (Exception e1)
        //    {
        //        //Logger.LogInfo(e1.Message.ToString());
        //        //Logger.LogInfo(e1);
        //        return 1;
        //    }
        //}
        public int Connect()
        {
            oCompany = new SAPbobsCOM.Company();
            DBType = System.Configuration.ConfigurationManager.AppSettings["DBType"];
            DBserver = System.Configuration.ConfigurationManager.AppSettings["DBServer"];
            DBUname = System.Configuration.ConfigurationManager.AppSettings["DBUname"];
            DBPwd = System.Configuration.ConfigurationManager.AppSettings["DBPwd"];
            SAPUSER = System.Configuration.ConfigurationManager.AppSettings["SAPUSERUname"];
            SAPPWD = System.Configuration.ConfigurationManager.AppSettings["SAPPwd"];
            COMPANYDB = System.Configuration.ConfigurationManager.AppSettings["SAPDBName"];
            //Program.oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2008;
            oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2014;

            oCompany.Server = DBserver;
            oCompany.UseTrusted = false;
            oCompany.DbUserName = DBUname;
            oCompany.DbPassword = DBPwd;
            oCompany.CompanyDB = COMPANYDB;
            oCompany.UserName = SAPUSER;
            oCompany.Password = SAPPWD;


            lRetCode = oCompany.Connect();
            if (lRetCode != 0) // if the connection failed
            {
                //   Cursor = System.Windows.Forms.Cursors.Default; //Change mouse cursor
                //int temp_int = lErrCode;
                //  string temp_string = sErrMsg;
                oCompany.GetLastError(out lErrCode, out sErrMsg);
                //  MessageBox.Show("Connection Failed - " + sErrMsg);
                // Logger.LogInfo("Connection Failed - " + sErrMsg);
            }
            if (oCompany.Connected) // if connected
            {
                //MessageBox.Show("Connected to - " + Program.oCompany.CompanyDB);
                // Logger.LogInfo("Connected to - " + oCompany.CompanyDB);
            }

            if (oCompany.Connected)
            {
                return 0;
            }
            else
            {
                //Logger.LogInfo("Database Not connected.. Try Again");
                return 1;
            }
        }
    }
}
