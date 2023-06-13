using Coursework.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Coursework.Models;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Controllers.Admin
{
    public class DriverController : Controller
    {
        private readonly MyDbContext _context;

        private readonly ILogger<HomeController> _logger;

        public DriverController(MyDbContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
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

        public IActionResult ChoseOrder()
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
            var orders = _context.OrderInfo.Include(o => o.FromCity)
                              .Include(o => o.ToCity)
                              .Where(o => o.DriverID == 27)
                              .ToList();


            return View("/Views/Driver/ChoseOrder.cshtml", orders);
        }

        public IActionResult TakeOrder(OrderInfoModel orderID)
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

            // Retrieve the city information for the fromCityID and toCityID
            var order = _context.OrderInfo
                    .Include(o => o.FromCity)
                    .Include(o => o.ToCity)
                    .FirstOrDefault(o => o.ID == orderID.ID);

            return View("/Views/Driver/TakeOrder.cshtml", order);
        }

        public IActionResult ConfirmOrder(OrderInfoModel orderID)
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

            // Retrieve the driverInfo based on the userID
            var driverInfo = _context.DriverInfo.FirstOrDefault(d => d.UserInfoID == userID);

            // Retrieve the order to update
            var order = _context.OrderInfo.FirstOrDefault(o => o.ID == orderID.ID);

            if (order != null && driverInfo != null)
            {
                // Update the DriverID in the order
                order.DriverID = driverInfo.ID;

                // Save the changes to the database
                _context.SaveChanges();
            }

            return RedirectToAction("ChoseOrder");
        }

        public IActionResult TakenOrders()
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

            // Retrieve the driverInfo based on the userID
            var driverInfo = _context.DriverInfo.FirstOrDefault(d => d.UserInfoID == userID);
            // Retrieve the orders for the logged-in driver
            var orders = _context.OrderInfo
                .Include(o => o.FromCity)
                .Include(o => o.ToCity)
                .Where(o => o.DriverID == driverInfo.ID)
                .ToList();

            // Pass the orders to the view
            return View("/Views/Driver/TakenOrders.cshtml", orders);
        }

    }
}
