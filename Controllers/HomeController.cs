using Microsoft.AspNetCore.Mvc;

namespace FunktsUurimine.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
    public IActionResult Theory() => View();
    public IActionResult Error() => View();
}
