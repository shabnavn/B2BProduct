using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace b2b_solution.Models
{
    public class CheckSessionTimeOut
    {
    }

    
    public class CheckSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
          
            if ( HttpContext.Current.Session["UserID"] == null)
            {
                filterContext.Result = new RedirectResult("/Landing/Login");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}