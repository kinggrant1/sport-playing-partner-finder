
// using System;
// using System.Linq;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.RazorPages;
// using System.Collections.Generic;
// using System.Data;
// using MySql.Data.MySqlClient;
// using System.Security.Cryptography;
// using System.Text;
// using System.Security.Claims;
// using Microsoft.AspNetCore.Authentication;

// namespace MyWebApp.Pages
// {
//     public class LoginSignupModel : PageModel
//     {
//         // Assuming you need these properties for binding in the POST method
//         [BindProperty]
//         public LoginInput Login {get; set;}

//         [BindProperty]
//         public SignupInput Signup {get; set;}

//         public class SignupInput {
//             public string firstName {get; set;}
//             public string lastName {get; set;}
//             public string email {get; set;}
//             public string password { get; set; }
//         }
//         public class LoginInput {
//             public string email {get; set;}
//             public string password {get; set;}
//         }
//         public void OnGet() {

//        }

//     public async Task<IActionResult> OnPostLoginAsync() {
//         foreach (var key in ModelState.Keys.Where(k => k.StartsWith("Signup")).ToList())
//         {
//         ModelState.Remove(key);
//         }
//         if (!ModelState.IsValid) {
//         // Handle the situation when the model state is not valid.
//             return Page();
//         }

//     // Hash the password entered by the user to compare with the database
//         var hashedPassword = HashPassword(Login.password);

//     // Your connection string
//         string connectionString = "server=sql9.freesqldatabase.com;port=3306;database=sql9658005;user=sql9658005;password=jnZR1jkii2;";

//         using(MySqlConnection connection = new MySqlConnection(connectionString)) {
//             connection.Open();
//             string sql = "SELECT * FROM users WHERE email = @Email LIMIT 1";
//             using (MySqlCommand command = new MySqlCommand(sql, connection)) {
//                 command.Parameters.AddWithValue("@Email", Login.email);
//             using (MySqlDataReader reader = command.ExecuteReader()) {
//                 if (reader.Read()) {
//                     // Assuming your user table has a 'password' column that stores hashed passwords
//                     string dbHashedPassword = reader["password"].ToString();
//                     if (string.IsNullOrEmpty(dbHashedPassword)) {
//     // Handle the case where the password is null or empty
//                         ModelState.AddModelError("", "Invalid login attempt.");
//                             return Page();
//                         }
//                     if (hashedPassword == dbHashedPassword) {
//                         var claims = new List<Claim> {
//                             new Claim(ClaimTypes.Email, Login.email),
//         // Add other claims as needed
//                                 };

//                         var claimsIdentity = new ClaimsIdentity(claims, "cookieAuth");
//                         await HttpContext.SignInAsync("cookieAuth", new ClaimsPrincipal(claimsIdentity));

//                         return RedirectToPage("/Index");
                        
//                     } else {
//                         // The password is incorrect
//                         ModelState.AddModelError("", "Invalid login attempt.");
//                         return Page();
//                     }
//                 } else {
//                     // No user was found with the provided email.
//                     ModelState.AddModelError("", "Invalid login attempt.");
//                     return Page();
//                 }
//             }
//         }
//     }
// }

// // Your existing HashPassword method remains the same.

//         public IActionResult OnPostSignup() {
//             foreach (var key in ModelState.Keys.Where(k => k.StartsWith("Login")).ToList())
//             {
//             ModelState.Remove(key);
//             }
//             if (!ModelState.IsValid)
//             {
//                 // Handle the situation when the model state is not valid.
//                 return Page();
//             }

//             // Assuming you have a method to hash passwords.
//             var hashedPassword = HashPassword(Signup.password);

//             // Here, convert the sports array into a string, e.g., by joining with a comma.
//             //string selectedSports = string.Join(",", Input.Sports);

//             string connectionString = "server=sql9.freesqldatabase.com;port=3306;database=sql9658005;user=sql9658005;password=jnZR1jkii2;";
//             using(MySqlConnection connection = new MySqlConnection(connectionString))
//             {
//                 string sql = "INSERT INTO users (firstName, lastName, email, password) VALUES (@FirstName, @LastName, @Email, @Password)";
//                 using (MySqlCommand command = new MySqlCommand(sql, connection))
//                 {
//                     command.Parameters.AddWithValue("@FirstName", Signup.firstName);
//                     command.Parameters.AddWithValue("@LastName", Signup.lastName);
//                     command.Parameters.AddWithValue("@Email", Signup.email);
//                     //command.Parameters.AddWithValue("@Username", Input.Username);
//                     command.Parameters.AddWithValue("@Password", hashedPassword);
//                     //command.Parameters.AddWithValue("@Sports", selectedSports);

//                     connection.Open();
//                     command.ExecuteNonQuery();
//                 }
                
//             }

//             // Redirect to a confirmation page or login page after successful registration
//             return RedirectToPage("/Index");
//         }

//         private string HashPassword(string password)
//         {
//             // It's important to use a secure method to hash passwords
//             using (var sha256 = SHA256.Create()) 
//             {
//                 var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
//                 var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
//                 return hash;
//             }
//         }
//     }
// }




