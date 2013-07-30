using System.Web.Mvc;
using System.Web.Routing;

namespace ScheduleWidgetSampleProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "CreateSchedule",
                "Home/CreateSchedule/{eventDate}",
                new {controller = "Home", action = "CreateSchedule"}
                );

            routes.MapRoute(
                "ScheduleOccurrence",
                "Home/ScheduleOccurrence/{id}/{eventDate}",
                new {controller = "Home", action = "ScheduleOccurrence"}
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}