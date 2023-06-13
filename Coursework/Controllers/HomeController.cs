using Coursework.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Coursework.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Data;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyDbContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(MyDbContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        static public int GetRole(int userID, MyDbContext _context)
        {
            var role = _context.Roles.FirstOrDefault(r => r.UserInfoID == userID);

            if (role != null)
            {
                if (role.Driver)
                {
                    return 1; // Driver role
                }
                else if (role.Admin)
                {
                    return 2; // Admin role
                }
            }

            return 0; // User role (default)
        }

        public IActionResult Index()
        {
            // Get the logged-in user's ID
            int userID = GetUserID();
            // Get the logged-in user's Role
            ViewData["Role"] = HomeController.GetRole(userID, _context);
            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                
                // Retrieve the user-specific data based on the user ID
                var userData = _context.Users.Where(u => u.ID == userID).ToList();

                if (userID >= 0)
                {
                    ViewData["Data"] = userData[0];
                }
                else { ViewData["Data"] = null; }
            }
            else
            {
                // Handle the case when the user is not authenticated
                // For example, display a message or redirect to the login page
                ViewData["Data"] = null;
            }
            return View();
        }
        public IActionResult MakeOrder()
        {
            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                // Get the logged-in user's ID
                int userID = GetUserID();
                // Get the logged-in user's Role
                ViewData["Role"] = HomeController.GetRole(userID, _context);


                // Retrieve the user-specific data based on the user ID
                var userData = _context.Users.Where(u => u.ID == userID).ToList();

                if (userID >= 0)
                {
                    ViewData["Data"] = userData[0];
                }
                else { ViewData["Data"] = null; }
            }
            else
            {
                // Handle the case when the user is not authenticated
                // For example, display a message or redirect to the login page
                ViewData["Data"] = null;
            }

            ViewData["OrderTypes"] = _context.OrderTypes.ToList();

            return View("~/Views/Home/User/MakeOrder.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> MakeOrder(string fromCity, string fromAddress, string toCity, string toAddress, string cargoWeight, int typeOrder)
        {
            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                // Get the logged-in user's ID
                int userID = GetUserID();
                // Get the logged-in user's Role
                ViewData["Role"] = HomeController.GetRole(userID, _context);


                // Create a new instance of the AddressInfoModel for the 'from' address
                var fromAddressInfo = new AddressInfoModel
                {
                    City = fromCity,
                    Address = fromAddress,
                    CompanyAddress = false // Assuming it's not a company address, modify as needed
                };

                // Add the 'from' address to the database
                _context.AdressInfo.Add(fromAddressInfo);
                await _context.SaveChangesAsync();

                // Create a new instance of the AddressInfoModel for the 'to' address
                var toAddressInfo = new AddressInfoModel
                {
                    City = toCity,
                    Address = toAddress,
                    CompanyAddress = false // Assuming it's not a company address, modify as needed
                };

                // Add the 'to' address to the database
                _context.AdressInfo.Add(toAddressInfo);
                await _context.SaveChangesAsync();

                // Create a new instance of the OrderInfoModel and set its properties
                var orderInfo = new OrderInfoModel
                {
                    UserInfoID = userID,
                    Date = DateTime.Now, // Set the current date and time

                    FromCity = fromAddressInfo,
                    ToCity = toAddressInfo,

                    DriverID = 27, // Replace with the appropriate driver ID

                    Capacity_kg = decimal.Parse(cargoWeight),
                    OrderTypeID = typeOrder
                };

                // Add the orderInfo to the database
                _context.OrderInfo.Add(orderInfo);
                await _context.SaveChangesAsync();

                // You can redirect the user to another page or perform additional actions
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Handle the case when the user is not authenticated
                // For example, display a message or redirect to the login page
                ViewData["Data"] = null;
                return RedirectToAction("Login", "Home");
            }
        }
        public IActionResult MyOrders()
        {
            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                // Get the logged-in user's ID
                int userID = GetUserID();
                // Get the logged-in user's Role
                ViewData["Role"] = HomeController.GetRole(userID, _context);

                // Retrieve the user-specific data based on the user ID
                var userData = _context.Users.Where(u => u.ID == userID).ToList();
                if (userID >= 0)
                {
                    ViewData["Data"] = userData[0];
                }
                else { ViewData["Data"] = null; }

                var myOrderInfos = _context.OrderInfo
                .Include(o => o.UserInfo)
                .Include(o => o.FromCity)
                .Include(o => o.ToCity)
                .Include(o => o.Driver)
                    .ThenInclude(d => d.Vehicle)
                .Include(o => o.Driver)
                    .ThenInclude(d => d.UserInfo)
                .Include(o => o.OrderType)
                .Where(u => u.UserInfoID == userID)
                .ToList();

                ViewData["MyOrderInfos"] = myOrderInfos;
            }
            else
            {
                // Handle the case when the user is not authenticated
                // For example, display a message or redirect to the login page
                ViewData["Data"] = null;
                return RedirectToAction("Login", "Home");
            }
            return View("~/Views/Home/User/MyOrders.cshtml");
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Cabinet()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Get the logged-in user's ID
                int userID = GetUserID();
                // Get the logged-in user's Role
                ViewData["Role"] = HomeController.GetRole(userID, _context);

                // Retrieve the user-specific data based on the user ID
                var userData = _context.Users.Where(u => u.ID == userID).ToList();

                if (userID >= 0)
                {
                    ViewData["Data"] = userData[0];
                }
                else { ViewData["Data"] = null; }
            }
            else
            {
                // Handle the case when the user is not authenticated
                // For example, display a message or redirect to the login page
                ViewData["Data"] = null;
            }

            return View();
        }
        

        private int GetUserID()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId.HasValue)
            {
                return userId.Value;
            }

            // Return a default or error value if the user ID is not found
            return -1;
        }
      
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        public async Task<IActionResult> UpdateUserInfo(UserTable updatedUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == updatedUser.Email);

                    if (user != null)
                    {
                        user.Name = updatedUser.Name;
                        user.Surname = updatedUser.Surname;
                        user.Birthdate = updatedUser.Birthdate;

                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    // Handle the exception
                }
            }

            return RedirectToAction("Cabinet", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserPassword(string email, string oldPassword, string newPassword)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(oldPassword) && !string.IsNullOrEmpty(newPassword))
            {
                try
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == oldPassword);

                    if (user != null)
                    {
                        user.Password = newPassword;

                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception)
                {
                    // Handle the exception
                }
            }

            return RedirectToAction("Cabinet", "Home");
        }
    }
}