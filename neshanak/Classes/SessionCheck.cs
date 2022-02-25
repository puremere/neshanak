using neshanak.viewModel;
using Newtonsoft.Json;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace neshanak.Classes 
{
    public class SessionCheck : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            if (session["lang"] == null)
            {
                session["lang"] = "en";
            }

            var descriptor = filterContext.ActionDescriptor;
            var actionName = descriptor.ActionName;



            //if (actionName != "Index" && actionName != "CustomerLogin" && actionName != "createUserReport")
            //{

            //    if (session["LogedInUser2"] == null)
            //    {
            //        filterContext.Result = new RedirectToRouteResult(
            //            new RouteValueDictionary {
            //                    { "Controller", "Admin" },
            //                    { "Action", "Index" }
            //                        });
            //    }
            //}

        }
    }

    public class HomeSessionCheck : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            if (session["lang"] == null)
            {
                session["lang"] = "en";
            }
            if (session["LogedInUser"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                                { "Controller", "Home" },
                                { "Action", "login" }
                                });
            }

        }
    }
    public class adminSessionCheck : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            var descriptor = filterContext.ActionDescriptor;
            var actionName = descriptor.ActionName;

            if (session["lang"] == null)
            {
                session["lang"] = "en";
            }

            if (actionName != "Index" && actionName != "CustomerLogin")
            {

                if (session["LogedInUser2"] == null)
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                                { "Controller", "Admin" },
                                { "Action", "Index" }
                                    });
                }
            }

        }
    }
}