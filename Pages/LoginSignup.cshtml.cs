using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyWebApp.Pages
{
    public class LoginSignupModel : PageModel
    {
    [BindProperty]
    public string? Username { get; set; }

    [BindProperty]
    public string? Password { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        // validate the user credentials and redirect them to a dashboard or show an error.

        return Content($"Username: {Username}, Password: {Password}");  
    }
    }
}
