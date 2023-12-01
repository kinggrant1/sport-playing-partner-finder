using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace MyWebApp.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public required LoginInput Login {get; set;}
        public class LoginInput {
            public required string email {get; set;}
            public required string password {get; set;}
        }
        private string HashPassword(string password)
        {
            //secure method to hash passwords
            using (var sha256 = SHA256.Create()) 
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hash;
            }
        }
    

        public void OnGet() {

       }
//login database method
    public async Task<IActionResult> OnPostLoginAsync() {
        if (!ModelState.IsValid) {
            return Page();
        }

        // hash the password entered to compare with the database
        var hashedPassword = HashPassword(Login.password);
        //connection string
        string connectionString = "server=sql5.freesqldatabase.com;port=3306;database=sql5666497;user=sql5666497;password=EgmsYQ9bGa;";
        using(MySqlConnection connection = new MySqlConnection(connectionString)) {
            connection.Open();
            string sql = "SELECT * FROM users WHERE email = @Email LIMIT 1";
            using (MySqlCommand command = new MySqlCommand(sql, connection)) {
                command.Parameters.AddWithValue("@Email", Login.email);
            using (MySqlDataReader reader = command.ExecuteReader()) {
                if (reader.Read()) {
                    string dbHashedPassword = reader["password"].ToString();
                    if (string.IsNullOrEmpty(dbHashedPassword)) {
                        // case to handle if the password is empty
                        ModelState.AddModelError("", "Invalid login attempt.");
                            return Page();
                        }
                        //case to handle if email is empty
                    if (hashedPassword == dbHashedPassword) {
                        var claims = new List<Claim> {
                            new Claim(ClaimTypes.Email, Login.email),
                                };

                        var claimsIdentity = new ClaimsIdentity(claims, "cookieAuth");
                        await HttpContext.SignInAsync("cookieAuth", new ClaimsPrincipal(claimsIdentity));
                        //if sign in is successful
                        TempData["SuccessMessage"] = $"Welcome back!";
                        return RedirectToPage("/Index");
                        
                    } else {
                        // password is incorrect/ sign in unsuccessful
                        ModelState.AddModelError("", "The password is incorrect, try again.");
                        return Page();
                    }
                } else {
                    // email is incorrect/ sign in unsuccesful
                    ModelState.AddModelError("", "Incorrect email.");
                    return Page();
                }
            }
        }
        }
    }
    }
}
        
    
    


    
