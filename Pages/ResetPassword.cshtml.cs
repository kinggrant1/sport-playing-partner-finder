// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.RazorPages;
// using MySql.Data.MySqlClient;
// using System;
// using System.ComponentModel.DataAnnotations;
// using System.Security.Cryptography;
// using System.Text;

// namespace MyWebApp.Pages
// {
//     public class ResetPasswordModel : PageModel
//     {
//         [BindProperty]
//         public required string email { get; set; }

//         [BindProperty, Required]
//         public required string ResetCode { get; set; }

//         [BindProperty, Required]
//         public required string NewPassword { get; set; }
//         public required string ErrorMessage { get; set; }

//         public IActionResult OnPost()
//         {
//             if (!ModelState.IsValid)
//             {
//                 return Page();
//             }

//             string connectionString = "server=sql5.freesqldatabase.com;port=3306;database=sql5666497;user=sql5666497;password=EgmsYQ9bGa;";
//             using (MySqlConnection connection = new MySqlConnection(connectionString))
//             {
//                 connection.Open();

//                 // Check reset code and expiration
//                 string checkCodeSql = "SELECT email FROM users WHERE reset_code = @ResetCode AND reset_code_expiration > NOW()";
//                 using (MySqlCommand checkCodeCmd = new MySqlCommand(checkCodeSql, connection))
//                 {
//                     checkCodeCmd.Parameters.AddWithValue("@ResetCode", ResetCode);
//                     using (var reader = checkCodeCmd.ExecuteReader())
//                     {
//                         if (!reader.Read())
//                         {
//                             ErrorMessage = "Invalid or expired reset code.";
//                             return Page();
//                         }
//                         email = reader["email"].ToString();
//                     }
//                 }

//                 // Update password
//                 string updatePasswordSql = "UPDATE users SET password = @NewPassword, reset_code = NULL, reset_code_expiration = NULL WHERE email = @Email";
//                 using (MySqlCommand updatePasswordCmd = new MySqlCommand(updatePasswordSql, connection))
//                 {
//                     updatePasswordCmd.Parameters.AddWithValue("@NewPassword", HashPassword(NewPassword));
//                     updatePasswordCmd.Parameters.AddWithValue("@Email", email);
//                     updatePasswordCmd.ExecuteNonQuery();
//                 }
//             }

//             TempData["SuccessMessage"] = "Your password has been reset successfully.";
//             return RedirectToPage("/Index"); // Or wherever you want to redirect after successful password reset
//         }

//         private string HashPassword(string password)
//         {
//             // Hash the new password
//             using (var sha256 = SHA256.Create())
//             {
//                 var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
//                 return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
//             }
//         }
//     }
// }
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Text;

namespace MyWebApp.Pages
{
    public class ResetPasswordModel : PageModel
    {
        [BindProperty]
        public required string ResetCode { get; set; }

        [BindProperty]
        public required string NewPassword { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string connectionString = "server=sql5.freesqldatabase.com;port=3306;database=sql5666497;user=sql5666497;password=EgmsYQ9bGa;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                
                // Validate reset code and expiration
                string validateCodeSql = "SELECT COUNT(1) FROM users WHERE reset_code = @ResetCode AND reset_code_expiration > NOW()";
                using (MySqlCommand validateCodeCmd = new MySqlCommand(validateCodeSql, connection))
                {
                    validateCodeCmd.Parameters.AddWithValue("@ResetCode", ResetCode);
                    int count = Convert.ToInt32(validateCodeCmd.ExecuteScalar());
                    if (count == 0)
                    {
                        ModelState.AddModelError("", "Invalid or expired reset code.");
                        return Page();
                    }
                }

                // Hash the new password
                string hashedPassword = HashPassword(NewPassword);

                // Update password in database
                string updatePasswordSql = "UPDATE users SET password = @NewPassword, reset_code = NULL, reset_code_expiration = NULL WHERE reset_code = @ResetCode";
                using (MySqlCommand updatePasswordCmd = new MySqlCommand(updatePasswordSql, connection))
                {
                    updatePasswordCmd.Parameters.AddWithValue("@NewPassword", hashedPassword);
                    updatePasswordCmd.Parameters.AddWithValue("@ResetCode", ResetCode);
                    updatePasswordCmd.ExecuteNonQuery();
                }
            }

            TempData["SuccessMessage"] = "Your password has been reset successfully.";
            return RedirectToPage("/Index");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hash;
            }
        }
    }
}

