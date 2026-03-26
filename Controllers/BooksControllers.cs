using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Library.User.Api.Controllers;

[Route("Books")]
[Authorize]
public class BooksController : Controller
{
    [HttpGet("")]
    public IActionResult Index() => View();
}