using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;

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

            var routeConstraints = ThisContext.ActionDescriptor.RouteValues;

            var currentControllerName = routeConstraints.First(x => x.Key == "controller").Value;
            var currentActionName = routeConstraints.First(x => x.Key == "action").Value;

            if (AspController == currentControllerName && AspAction == currentActionName)
            {
                if (output.Attributes.TryGetAttribute("Value", out TagHelperAttribute maybeClass))
                {
                    if (maybeClass != null)
                    {
                        output.Attributes.SetAttribute("class", $"{maybeClass.Value} {ConditionalCssClass}");
                        return;
                    }
                }

                output.Attributes.SetAttribute("class", ConditionalCssClass);
            }
        }
    }
}
