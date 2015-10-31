using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using CacheAsidePatternWebTemplate.Cache;
using CacheAsidePatternWebTemplate.Models;

namespace CacheAsidePatternWebTemplate
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            PrimeCache();
        }

        void PrimeCache()
        {
            
        }
    }
}