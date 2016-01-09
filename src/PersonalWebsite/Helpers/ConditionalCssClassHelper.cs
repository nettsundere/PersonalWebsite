using Microsoft.AspNet.Http;
using Microsoft.AspNet.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Razor;

namespace PersonalWebsite.Helpers
{
    [HtmlTargetElement("a", Attributes = listToRun)]
    public class ConditionalCssClassHelper : TagHelper
    {
        private const string listToRun = "asp-controller, asp-action, asp-this-page";
        private const string conditionalCssClassAttribute = "if-current-css-class";

        private const string aspControllerAttribute = "asp-controller";
        private const string aspActionAttribute = "asp-action";

        private const string aspThisPageAttribute = "asp-this-page";

        [HtmlAttributeName(conditionalCssClassAttribute)]
        public string ConditionalCssClass { get; set; }

        [HtmlAttributeName(aspControllerAttribute)]
        public string AspController { get; set; }

        [HtmlAttributeName(aspActionAttribute)]
        public string AspAction { get; set; }

        [HtmlAttributeName(aspThisPageAttribute)]
        public RazorPage ThisPage { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            var currentControllerName = "";
            var currentActionName = "";

            if (AspController.ToString() == currentControllerName.ToString()
               && AspAction.ToString() == currentActionName.ToString())
            {
                var currentClassAttribute = (string)output.Attributes["class"].Value;
                output.Attributes["class"] = $"{currentClassAttribute} {ConditionalCssClass}";
            }
        }
    }
}
