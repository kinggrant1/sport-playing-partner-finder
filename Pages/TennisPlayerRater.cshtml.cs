using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace MyWebApp.Pages
{
    public class TennisPlayerRaterModel : PageModel
    {
        // public TennisPlayer TargetTennisPlayer { get; set; }
        public List<TennisPlayer> TennisPlayers { get; set; }
        
        public void OnGet(int Player)
        {
            TennisPlayers = new List<TennisPlayer>();

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
                            // TargetTennisPlayer = tennisPlayer;
                            TennisPlayers.Add(tennisPlayer);
                        }
                    }
                }
            }
        }

        public IActionResult OnPostSubmit(int playerId, int playerRating, String playerComment)
        {
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
                    return RedirectToPage("/Tennis");;
                }

                string query = String.Format("INSERT INTO TennisPlayerRatings (`playerId`, `playerRating`, `playerComment`) VALUES ('{0}', '{1}', '{2}')", playerId, playerRating, playerComment);

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToPage("/Tennis"); // You can redirect to another page or return a different IActionResult as needed.
        }
    }
}
