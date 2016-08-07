using Microsoft.AspNetCore.Routing;

namespace PersonalWebsite
{
    /// <summary>
    /// Provides route definitions.
    /// </summary>
    public interface IRoutesBuilder
    {
        /// <summary>
        /// Build route definitions.
        /// </summary>
        /// <param name="routes">The route builder</param>
        void Build(IRouteBuilder routes);
    }
}