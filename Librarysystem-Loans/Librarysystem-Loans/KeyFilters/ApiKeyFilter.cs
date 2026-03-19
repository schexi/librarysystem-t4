using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LibrarysystemLoans.KeyFilters;

// Kontrollerar att API-nyckeln finns med och stämmer i varje anrop
public class ApiKeyFilter : IActionFilter
{
    private readonly IConfiguration _config;

    public ApiKeyFilter(IConfiguration config)
    {
        _config = config;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Kontrollera om API-nyckeln finns med i headern
        if (!context.HttpContext.Request.Headers.TryGetValue("API-Key", out var key))
        {
            context.Result = new UnauthorizedResult(); // Neka om nyckel saknas
            return;
        }

        // Kontrollera att nyckeln stämmer med konfigurationen
        if (key != _config["ApiKey"])
        {
            context.Result = new UnauthorizedResult(); // Neka om fel nyckel
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}