using Microsoft.AspNet.Http;
using Microsoft.AspNet.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Mvc.Rendering;

namespace PersonalWebsite.Helpers
{
    [HtmlTargetElement("li", Attributes = listToRun)]
    public class ConditionalCssClassHelper : TagHelper
    {
        private const string listToRun = "asp-controller,asp-action,asp-this-context,if-current-css-class";

        private const string conditionalCssClassAttribute = "if-current-css-class";
        private const string aspControllerAttribute = "asp-controller";
        private const string aspActionAttribute = "asp-action";

        private const string aspThisContextAttribute = "asp-this-context";

        [HtmlAttributeName(conditionalCssClassAttribute)]
        public string ConditionalCssClass { get; set; }

        [HtmlAttributeName(aspControllerAttribute)]
        public string AspController { get; set; }

        [HtmlAttributeName(aspActionAttribute)]
        public string AspAction { get; set; }

        [HtmlAttributeName(aspThisContextAttribute)]
        public ViewContext ThisContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            var routeConstraints = ThisContext.ActionDescriptor.RouteConstraints;

            var currentControllerName = routeConstraints.First(x => x.RouteKey == "controller").RouteValue;
            var currentActionName = routeConstraints.First(x => x.RouteKey == "action").RouteValue;

            if (AspController == currentControllerName && AspAction == currentActionName)
            {
                TagHelperAttribute maybeClass;
                if (output.Attributes.TryGetAttribute("Value", out maybeClass)) {
                    if(maybeClass != null)
                    {
                        output.Attributes["class"] = $"{maybeClass.Value} {ConditionalCssClass}";
                        return;
                    }
                }

                output.Attributes["class"] = ConditionalCssClass;
            }
        }
    }
}
