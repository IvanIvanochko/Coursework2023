using Coursework.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace Coursework
{
    public class Program
    {
        public IConfiguration Configuration { get; }
        public Program(IConfiguration configuration) { 
            Configuration = configuration;
        }
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSession();
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            string connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");

            // Register the database context with the connection string.
            builder.Services.AddDbContext<MyDbContext>(options =>
                options.UseSqlServer(connectionString));

            //// Register authentication services
            //builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>
            //    {
            //        options.Cookie.Name = "Cookie";
            //        options.LoginPath = "/Home/Login"; // Customize the login path as needed
            //    });
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();
            app.UseAuthentication(); // Ensure authentication middleware is added before authorization middleware
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.Run();
        }
    }
}