using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using MyWebApp.Pages;
using System;

namespace MyWebApp.Controllers
{
    public class SoccerController : Controller
    {
        private readonly IConfiguration _configuration;
        public SoccerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index() 
        {
            SoccerModel model = new SoccerModel(_configuration);
            //model.AddPlayer(new Player { Name = "Kylian Mbappé", Position = "ST", Age = 22}); outdated for local purpose

            return View(model); // shows the razor view
        }
        [HttpPost]
        public IActionResult AddPlayer([FromBody] Player p)
        {
            string connectionString = _configuration.GetConnectionString("myDatabase");
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Players (Player_Name, Player_Position, Player_Age) VALUES (@name, @position, @age)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", p.Name);
                    command.Parameters.AddWithValue("@position", p.Position);
                    command.Parameters.AddWithValue("@age", p.Age);
                    command.ExecuteNonQuery();
                }
            }

            return Ok();
        }

        [HttpPost]
        public IActionResult SendMatchRequest([FromBody] MatchRequest request)
        {
            try
            {
                SoccerModel model = new SoccerModel(_configuration);
                model.AddMatchRequest(request);
                return Json(new { message = "Match request sent successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { message = "An error occurred while sending the match request: " + ex.Message });
            }
        }
        [HttpGet("GetPlayerName")]
        public IActionResult GetPlayerName(int requesterId)
        {
            try
            {
                SoccerModel model = new SoccerModel(_configuration);
                string playerName = model.GetPlayerName(requesterId);
                return Ok(new { playerName = playerName });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while getting the player name: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult SubmitTeam(Team team)
        {
            string connectionString = _configuration.GetConnectionString("myDatabase");
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Teams (Team_Name) VALUES (@name)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", team.Team_Name);
                    command.ExecuteNonQuery();
                }
            }

            // Redirect the user to a confirmation page or back to the form
            return RedirectToAction("Soccer");
        }
    } 
    
}