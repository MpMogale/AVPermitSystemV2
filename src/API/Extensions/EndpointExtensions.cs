
using System.Reflection;
using AVPermitSystemV2.API.Endpoints;

namespace AVPermitSystemV2.API.Extensions;

public static class EndpointExtensions
{
    /// <summary>
    /// Registers all endpoints that implement IEndpoint interface
    /// </summary>
    /// <param name="app">The web application</param>
    /// <returns>The configured web application</returns>
    public static WebApplication RegisterEndpoints(this WebApplication app)
    {
        var endpointTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IEndpoint)) && 
                       !t.IsInterface && 
                       !t.IsAbstract)
            .ToList();

        foreach (var endpointType in endpointTypes)
        {
            var endpoint = Activator.CreateInstance(endpointType) as IEndpoint;
            if (endpoint != null)
            {
                endpoint.RegisterEndpoints(app);
            }
        }

        return app;
    }
}
