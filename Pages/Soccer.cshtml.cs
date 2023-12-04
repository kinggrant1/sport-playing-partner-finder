using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

// appSettings.json needs to be updated for the new database connection string. 

namespace MyWebApp.Pages
{
    public class SoccerModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<Player> Players { get; set; }
        [BindProperty]
        public Team Team { get; set; } = new Team();
        public SoccerModel(IConfiguration configuration) // constructor for private data in appsettings.json
        {
            _configuration = configuration;
            Players = new List<Player>(); 
        }

        public IActionResult OnPostSubmitTeam(Team? team) // CHANGED DATABASE TO AUTOINCREMENT, FIXES TRY CATCH ERROR. update: try catch has been removed. 
        {
            string connectionString = _configuration.GetConnectionString("myDatabase");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Teams (Team_Name) VALUES (@teamName)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@teamName", team.Team_Name);
                    command.ExecuteNonQuery();
                }
            }

            // Redirect the user to a confirmation page or back to the form
            return RedirectToPage("/Soccer"); // Update the page path accordingly
        }

        public void OnGet()
        {
            Players = new List<Player>();

            string connectionString = _configuration.GetConnectionString("MyDatabase");
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {  //shows if connection is working
                    connection.Open();
                    Console.WriteLine("Connection opened successfully.");
                }
                catch (Exception exc)
                {
                    //Handles error
                    Console.WriteLine(exc.Message);
                    return;
                }

                string query = "SELECT * FROM Players";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                { 
                    try { //handles sql command error
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Player player = new Player();
                                player.Name = reader.GetString("Player_Name");
                                player.Position = reader.GetString("Player_Position");
                                player.Age = reader.GetInt32("Player_Age");
                                Players.Add(player); //redundant
                            }
                        }
                    }
                    catch (Exception exc) { // handles reader error
                        Console.WriteLine(exc.Message);
                    }
                }
            }
        }
        public void AddPlayerToDatabase(Player p) // not redundant interacts with database
        {
            string connectionString = _configuration.GetConnectionString("myDatabase");
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Players (Player_Name, Player_Position, Player_Age) VALUES (name, position, age)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("name", p.Name); 
                    command.Parameters.AddWithValue("position", p.Position);
                    command.Parameters.AddWithValue("age", p.Age);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddMatchRequest(MatchRequest request)
        {
            string connectionString = _configuration.GetConnectionString("myDatabase");
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO MatchRequests (requesterId, requesteeId) VALUES (@requesterId, @requesteeId)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@requesterId", request.RequesterId);
                    command.Parameters.AddWithValue("@requesteeId", request.RequesteeId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public string GetPlayerName(int requesterId) 
        {
            string connectionString = _configuration.GetConnectionString("myDatabase");
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Testing");
                string query = "SELECT Player_Name FROM Players WHERE Id = @requesterId";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@requesterId", requesterId);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetString(0);
                        }
                        else
                        {
                            throw new Exception("Player not found");
                        }
                    }
                }
            }
        }
    }

    public class Player
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public int Age { get; set; }
    }

    public class MatchRequest // Todo: need to add a way to get the player name from the requesterId, foreign keys likely
    {
        public int RequesterId { get; set; }
        public int RequesteeId { get; set; }
    }
    public class Team {
        public string Team_Name {get; set;}
    }
};