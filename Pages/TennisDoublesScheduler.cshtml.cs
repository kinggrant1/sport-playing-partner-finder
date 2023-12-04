using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace MyWebApp.Pages
{
    public class TennisDoublesSchedulerModel : PageModel
    {
        // public TennisPlayer TargetTennisPlayer { get; set; }
        public void OnGet()
        {

        }

        public IActionResult OnPostSubmit(String doublesPlayers, String doublesTime, String doublesDuration, String doublesLocation)
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

                string query = String.Format("INSERT INTO TennisDoublesSessions (`doublesPlayers`, `doublesLocation`, `doublesTime`, `doublesDuration`) VALUES ('{0}', '{1}', '{2}', '{3}')", doublesPlayers, doublesLocation, doublesTime, doublesDuration);

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToPage("/Tennis"); // You can redirect to another page or return a different IActionResult as needed.
        }
    }
}
