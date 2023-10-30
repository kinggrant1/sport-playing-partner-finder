using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace MyWebApp.Pages
{
    public class SoccerModel : PageModel
    {
        public List<Player> Players { get; set; }

        public void OnGet()
        {
            Players = new List<Player>();

            string connectionString = "server=sql9.freesqldatabase.com;port=3306;database=sql9657948;user=sql9657948;password=GrJrMM2h3I;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                { // shows if connection is working
                    connection.Open();
                    Console.WriteLine("Connection opened successfully.");
                }
                catch (Exception ex)
                {
                    // Handle the exception here
                    Console.WriteLine(ex.Message);
                    return;
                }

                string query = "SELECT * FROM Players";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Player player = new Player();
                            player.Name = reader.GetString("Player_Name");
                            player.Position = reader.GetString("Player_Position");
                            player.Age = reader.GetInt32("Player_Age");
                            Players.Add(player);
                        }
                    }
                }
            }
        }

        public void AddPlayer(Player p)
        {
            Players.Add(p);
        }
    }

    public class Player
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public int Age { get; set; }
    }
};