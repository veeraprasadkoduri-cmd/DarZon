using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using DarZon.Models;

namespace DarZon.Controllers
{
    public class UserAuthenticationFilters : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            //Check Session is Empty Then set as Result is HttpUnauthorizedResult 
            if (string.IsNullOrEmpty(Convert.ToString(filterContext.HttpContext.Session["UserName"])))
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {

            string url=  filterContext.HttpContext.Request.Url.ToString();
            string[] refurl = url.Split('/');
   
            if(refurl[3] == "Measurmentdetails")
            {
                refurl[3] = refurl[4] = "SaleOrder";
            }
            string cunurl = "../" + refurl[3] + "/" + refurl[4];
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult )
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "../Login/Login"
                };
            }
         else
            {
                DARZANTESTEntities db = new DARZANTESTEntities();
                string username= filterContext.HttpContext.Session["UserName"].ToString();
                var objlist = (from a in db.C_USER_MASTER.AsEnumerable() where a.U_User == username && a.U_TemPath== cunurl orderby int.Parse(a.Code) select a).FirstOrDefault();
                    if(objlist==null)
                {
                    filterContext.Result = new ViewResult
                    {
                        ViewName = "../Login/Login"
                    };

                }
           
                
             }



        }
    }
}