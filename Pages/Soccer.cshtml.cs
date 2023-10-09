using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyWebApp.Pages
{
    public class SoccerModel : PageModel
    {
        public string Name {get; set;}
        public void OnGet()
        {
        }
    }
}
