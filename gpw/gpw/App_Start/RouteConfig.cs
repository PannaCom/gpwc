using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace gpw
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            // Router SEO
            routes.MapRoute(
                "daily",
                "gioi-thieu",
                new { controller = "Home", action = "GioiThieu" }
            );
            routes.MapRoute(
                "detail tin tuc",
                "{name}-{id}",
                new { controller = "News", action = "Details", id = UrlParameter.Optional, name = UrlParameter.Optional }
            );//, cat = UrlParameter.Optional, domain = UrlParameter.Optional 
            routes.MapRoute(
                "danh muc tin",
                "tin/{url}",
                new { controller = "News", action = "tin", url = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
