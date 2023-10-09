using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyWebApp.Pages
{
    public class SoccerModel : PageModel
    {
        public List<Player> Players {get; set;}
        public SoccerModel()
        {
            Players = new List<Player>
            {
                new Player {Name = "Ronaldo", Position = "ST", Country = "Portugal"},
                new Player {Name = "Messi", Position = "ST", Country = "Argentina"},
                new Player {Name = "Van dijk", Position = "CB", Country = "Netherlands"},
            };
        }
        public void OnGet()
        {
        }

        public void AddPlayer(Player p) {
            Players.Add(p);
        }
    }
    public class Player
    {
        public string Name {get; set;}
        public string Position {get; set;}
        public string Country {get; set;}
    }
}
