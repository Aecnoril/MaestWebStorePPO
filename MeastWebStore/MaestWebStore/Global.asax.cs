using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MaestWebStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            if (!Util.DatabaseConnection.Initialize("dbi319888", "Knotwilg117", "192.168.15.50:1521/fhictora"))
            {
                Debug.WriteLine("Database connection is failing!");
                
            }
            else
            {
                Debug.WriteLine("Database connected!");
            }
        }
    }
}
