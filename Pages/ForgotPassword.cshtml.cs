
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebApp.Services;
using MySql.Data.MySqlClient;



namespace MyWebApp.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly EmailService _emailService;

        [BindProperty]
        public required string email {get; set; }
        public ForgotPasswordModel(EmailService emailService)
        {
            _emailService = emailService;
        }

    public IActionResult OnPost()
{
    if (!ModelState.IsValid)
    {
        return Page();
    }

    string connectionString = "server=sql5.freesqldatabase.com;port=3306;database=sql5666497;user=sql5666497;password=EgmsYQ9bGa;";
    string resetCode = GenerateResetCode();
    DateTime expiration = DateTime.UtcNow.AddHours(24); // 24 hours validity

    using (MySqlConnection connection = new MySqlConnection(connectionString))
    {
        connection.Open();
        string checkEmailSql = "SELECT COUNT(1) FROM users WHERE email = @Email";
        using (MySqlCommand checkEmailCmd = new MySqlCommand(checkEmailSql, connection))
        {
            checkEmailCmd.Parameters.AddWithValue("@Email", email);
            int count = Convert.ToInt32(checkEmailCmd.ExecuteScalar());
            if (count == 0)
            {
                ModelState.AddModelError("email", "No account found with that email address.");
                return Page();
            }
        }

        // Save reset code and expiration in the database
        string saveCodeSql = "UPDATE users SET reset_code = @ResetCode, reset_code_expiration = @Expiration WHERE email = @Email";
        using (MySqlCommand saveCodeCmd = new MySqlCommand(saveCodeSql, connection))
        {
            saveCodeCmd.Parameters.AddWithValue("@ResetCode", resetCode);
            saveCodeCmd.Parameters.AddWithValue("@Expiration", expiration);
            saveCodeCmd.Parameters.AddWithValue("@Email", email);
            saveCodeCmd.ExecuteNonQuery();
        }
    }

    // Send the reset code via email
    string toEmail = email;
    string subject = "Password Reset Code";
    string body = $"Your password reset code is: {resetCode}. This code is valid for 24 hours.";
    _emailService.SendEmail(toEmail, subject, body);

    TempData["SuccessMessage"] = "If an account exists for this email, a password reset code has been sent.";
    return RedirectToPage("/ResetPassword");
}

private string GenerateResetCode()
{
    Random rnd = new Random();
    return rnd.Next(1000, 10000).ToString(); 
}

    }
}
