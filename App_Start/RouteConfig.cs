using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace b2b_solution
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Landing", action = "Landing", id = UrlParameter.Optional }
            );

            //routes.MapRoute("Searching", "Search/{*params}", new { controller = "Home", action = "Search" });

        }
    }
}
