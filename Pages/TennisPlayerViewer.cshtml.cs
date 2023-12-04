using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace MyWebApp.Pages
{
    public class TennisPlayerViewerModel : PageModel
    {
        public List<TennisPlayer> TennisPlayers { get; set; }
        public List<TennisPlayerRating> TennisPlayerRatings { get; set; }
        
        public void OnGet(int Player)
        {
            TennisPlayers = new List<TennisPlayer>();
            TennisPlayerRatings = new List<TennisPlayerRating>();

            string connectionString = "server=sql5.freesqldatabase.com;port=3306;database=sql5667044;user=sql5667044;password=wTkJzLah69;";

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

                string query = String.Format("SELECT * FROM TennisPlayers WHERE id='{0}'", Player);
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TennisPlayer tennisPlayer = new TennisPlayer();
                            tennisPlayer.Id = reader.GetInt32("id");
                            tennisPlayer.FirstName = reader.GetString("firstName");
                            tennisPlayer.LastName = reader.GetString("lastName");
                            tennisPlayer.Sex = reader.GetString("sex");
                            tennisPlayer.Age = reader.GetInt32("age");
                            tennisPlayer.NTRP = reader.GetDouble("ntrp");
                            TennisPlayers.Add(tennisPlayer);
                        }
                    }
                }

                query = String.Format("SELECT * FROM TennisPlayerRatings WHERE playerId='{0}'", Player);
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TennisPlayerRating tennisPlayerRating = new TennisPlayerRating();
                            tennisPlayerRating.PlayerRating = reader.GetInt32("playerRating");
                            tennisPlayerRating.PlayerComment = reader.GetString("playerComment");
                            TennisPlayerRatings.Add(tennisPlayerRating);
                        }
                    }
                }
            }
        }
    }

    public class TennisPlayerRating
    {
        public int PlayerRating { get; set; }
        public string PlayerComment { get; set; }
    }
}
