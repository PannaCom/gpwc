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
                "gioi thieu",
                "gioi-thieu",
                new { controller = "Home", action = "GioiThieu" }
            );
            routes.MapRoute(
               "thiet ke website",
               "thiet-ke-website-gia-pha-dong-ho",
               new { controller = "Home", action = "ThietKeWebsite" }
           );
            routes.MapRoute(
               "phan mem gia pha",
               "phan-mem-gia-pha",
               new { controller = "Home", action = "PhanMemGiaPha" }
           );
            routes.MapRoute(
                "thiet ke gia pha",
                "thiet-ke-gia-pha",
                new { controller = "gallery", action = "list" }
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
