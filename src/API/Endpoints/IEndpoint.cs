
using Microsoft.AspNetCore.Mvc;

namespace AVPermitSystemV2.API.Endpoints;

/// <summary>
/// Interface for defining API endpoints with versioning support
/// </summary>
public interface IEndpoint
{
    void RegisterEndpoints(IEndpointRouteBuilder app);
}
