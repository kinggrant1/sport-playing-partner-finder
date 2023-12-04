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
        public List<TennisSinglesSession> TennisSinglesSessions { get; set; }
        public List<TennisDoublesSession> TennisDoublesSessions { get; set; }

        public void OnGet()
        {
            TennisPlayers = new List<TennisPlayer>();
            TennisSinglesSessions = new List<TennisSinglesSession>();
            TennisDoublesSessions = new List<TennisDoublesSession>();

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

                query = "SELECT * FROM TennisSinglesSessions";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TennisSinglesSession tennisSinglesSession = new TennisSinglesSession();
                            tennisSinglesSession.SinglesPlayer = reader.GetString("singlesPlayer");
                            tennisSinglesSession.SinglesLocation = reader.GetString("singlesLocation");
                            tennisSinglesSession.SinglesTime = reader.GetString("singlesTime");
                            tennisSinglesSession.SinglesDuration = reader.GetString("singlesDuration");
                            TennisSinglesSessions.Add(tennisSinglesSession);
                        }
                    }
                }

                query = "SELECT * FROM TennisDoublesSessions";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TennisDoublesSession tennisDoublesSession = new TennisDoublesSession();
                            tennisDoublesSession.DoublesPlayers = reader.GetString("doublesPlayers");
                            tennisDoublesSession.DoublesLocation = reader.GetString("doublesLocation");
                            tennisDoublesSession.DoublesTime = reader.GetString("doublesTime");
                            tennisDoublesSession.DoublesDuration = reader.GetString("doublesDuration");
                            TennisDoublesSessions.Add(tennisDoublesSession);
                        }
                    }
                }
            }
        }
    }

    public class TennisPlayer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public double NTRP { get; set; }
    }

    public class TennisSinglesSession
    {
        public string SinglesPlayer { get; set; }
        public string SinglesLocation { get; set; }
        public string SinglesTime { get; set; }
        public string SinglesDuration { get; set; }
    }

    public class TennisDoublesSession
    {
        public string DoublesPlayers { get; set; }
        public string DoublesLocation { get; set; }
        public string DoublesTime { get; set; }
        public string DoublesDuration { get; set; }
    }
}
