using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using ConferenceManagementSystem.Controllers;
using ConferenceManagementSystem.Models;
using System.Web;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.Routing;
using System.Linq;
using ConferenceManagementSystem.DataAccessLayer;

namespace ConferenceManagementSystem.Filters
{
    public class Authentication : ActionFilterAttribute
    {

        private ConferenceManagementContext db = new ConferenceManagementContext();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            HttpSessionStateBase session = filterContext.HttpContext.Session;
            Controller controller = filterContext.Controller as Controller;

            //session timeout          
            if (session["sessionLoggedInUserName"] == null)
            {
                controller.TempData["sessionTimeout"] = "Your session has timed out. Please login again.";
                var routeValues = new RouteValueDictionary();
                routeValues["controller"] = "Account";
                routeValues["action"] = "Login";
                filterContext.Result = new RedirectToRouteResult(routeValues);
            }

            //try
            //{
            //    //authentication - make sure user is logged in, in order to access the system modules
            //    var user = db.Users.FirstOrDefault(u => u.Username == System.Web.HttpContext.Current.User.Identity.Name && u.LoggedIn == true);
            //    var admin = db.Admins.FirstOrDefault(u => u.Username == System.Web.HttpContext.Current.User.Identity.Name && u.LoggedIn == true);
            //    var organizer = db.Organizers.FirstOrDefault(u => u.Username == System.Web.HttpContext.Current.User.Identity.Name && u.LoggedIn == true);
            //    //if user is logged in
            //    if (user != null || admin != null || organizer!= null)
            //    {
            //        if (user.LoggedIn != Utilities.getUserCurrentIp())
            //        {
            //            controller.TempData["sessionMultipleLogin"] = "Logged out due to multiple login detected";
            //            var routeValues = new RouteValueDictionary();
            //            routeValues["controller"] = "Account";
            //            routeValues["action"] = "Login";
            //            filterContext.Result = new RedirectToRouteResult(routeValues);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.Write(ex.Message);
            //}

        }
    }
}
