using System.Web.Mvc;

using System.Reflection;

namespace InventoryPizzaExpress.Filters
{
    public class InitializeSimpleMembershipAttribute
    {
    }
    public class OnActionAttribute : ActionMethodSelectorAttribute
    {
        public string ButtonName { get; set; }

        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            var req = controllerContext.RequestContext.HttpContext.Request;
            return !string.IsNullOrEmpty(req.Form[this.ButtonName]);
        }
    }
}