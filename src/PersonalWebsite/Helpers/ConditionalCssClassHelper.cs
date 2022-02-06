using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;

namespace PersonalWebsite.Helpers;

[HtmlTargetElement("li", Attributes = ListToRun)]
public class ConditionalCssClassHelper : TagHelper
{
    private const string ListToRun = "asp-controller,asp-action,asp-this-context,if-current-css-class";

    private const string ConditionalCssClassAttribute = "if-current-css-class";
    private const string AspControllerAttribute = "asp-controller";
    private const string AspActionAttribute = "asp-action";

    private const string AspThisContextAttribute = "asp-this-context";

    [HtmlAttributeName(ConditionalCssClassAttribute)]
    public string ConditionalCssClass { get; set; } = null!;

    [HtmlAttributeName(AspControllerAttribute)]

    public string AspController { get; set; } = null!;

    [HtmlAttributeName(AspActionAttribute)]
    public string AspAction { get; set; } = null!;

    [HtmlAttributeName(AspThisContextAttribute)]
    public ViewContext ThisContext { get; set; } = null!;

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