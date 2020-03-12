using Microsoft.AspNetCore.Routing;

namespace PersonalWebsite
{
    /// <summary>
    /// Provides endpoints definitions.
    /// </summary>
    public interface IEndpointsBuilder
    {
        /// <summary>
        /// Build endpoints definitions.
        /// </summary>
        /// <param name="routes">The endpoints builder</param>
        void Build(IEndpointRouteBuilder endpoints);
    }
}