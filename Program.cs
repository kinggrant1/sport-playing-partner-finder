var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Register your EmailService for dependency injection
builder.Services.AddTransient<MyWebApp.Services.EmailService>();

// Add authentication services with cookie authentication
builder.Services.AddAuthentication("cookieAuth").AddCookie("cookieAuth", options =>
{
    options.LoginPath = "/LoginSignup"; // Your login page path
    // options.AccessDeniedPath = "/AccessDenied";
    //options.LogoutPath = "/Logout"; // Your logout page path
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set cookie expiration
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
