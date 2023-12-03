using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace MyWebApp.Pages
{
    public class TennisModel : PageModel
    {
        public List<TennisPlayer> TennisPlayers { get; set; }

        public void OnGet()
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

                string query = "SELECT * FROM TennisPlayers";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TennisPlayer tennisPlayer = new TennisPlayer();
                            tennisPlayer.FirstName = reader.GetString("firstName");
                            tennisPlayer.LastName = reader.GetString("lastName");
                            tennisPlayer.Sex = reader.GetString("sex");
                            tennisPlayer.Age = reader.GetInt32("age");
                            tennisPlayer.NTRP = reader.GetDouble("ntrp");
                            TennisPlayers.Add(tennisPlayer);
                        }
                    }
                }
            }
        }
    }

    public class TennisPlayer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public double NTRP { get; set; }
    }
}
