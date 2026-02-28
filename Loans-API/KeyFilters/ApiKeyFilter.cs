using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Loans_API.KeyFilters;

public class ApiKeyFilter : IActionFilter
{
    private readonly IConfiguration _config;

    public ApiKeyFilter(IConfiguration config)
    {  _config = config;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Kontrollera om API-nyckeln finns med i headern på anropet
        
        if (!context.HttpContext.Request.Headers.TryGetValue("API-Key", out var key))
       
        {
            // Om ingen nyckel hittas, neka tillträde
            context.Result = new UnauthorizedResult();
            return;
        }

        // Kontrollera att det är rätt nyckel
        if (key != _config["ApiKey"])
        {
            // Om det är fel nyckel
            context.Result = new UnauthorizedResult();
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {

    }
}