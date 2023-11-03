using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;


namespace MyWebApp.Pages
{
    public class SignupModel : PageModel
    {
        [BindProperty]
        public SignupInput Signup {get; set;}

        public class SignupInput {
            public string firstName {get; set;}
            public string lastName {get; set;}
            public string email {get; set;}
            public string password { get; set; }
        }
        private string HashPassword(string password)
        {
            // It's important to use a secure method to hash passwords
            using (var sha256 = SHA256.Create()) 
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hash;
            }
        }
    

        public void OnGet() {

       }

       // signup db connection method
        public IActionResult OnPostSignup() {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //hash the password entered
            var hashedPassword = HashPassword(Signup.password);

            //db connection
            string connectionString = "server=sql9.freesqldatabase.com;port=3306;database=sql9658005;user=sql9658005;password=jnZR1jkii2;";
            using(MySqlConnection connection = new MySqlConnection(connectionString)) {
                connection.Open();
                string checkEmailSql = "SELECT COUNT(1) FROM users WHERE email = @Email";
                using (MySqlCommand checkEmailCmd = new MySqlCommand(checkEmailSql, connection)) {
                    checkEmailCmd.Parameters.AddWithValue("@Email", Signup.email);
                    int count = Convert.ToInt32(checkEmailCmd.ExecuteScalar());
                    if (count > 0) {
                // If the email already exists, add a model error and return to the page
                        ModelState.AddModelError("Signup.email", "The email is already in use.");
                        return Page();
            }
            
                }
    
                //insert user input into db
                string sql = "INSERT INTO users (firstName, lastName, email, password) VALUES (@FirstName, @LastName, @Email, @Password)";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", Signup.firstName);
                    command.Parameters.AddWithValue("@LastName", Signup.lastName);
                    command.Parameters.AddWithValue("@Email", Signup.email);
                    command.Parameters.AddWithValue("@Password", hashedPassword);

                    // connection.Open();
                    command.ExecuteNonQuery();
                }
                
            }

            // go to the home page, and see a welcome message. 
            TempData["SuccessMessage"] = $"You have successfully signed up, {Signup.firstName}. Enjoy your time here!";
            return RedirectToPage("/Index");
        }
    }
}