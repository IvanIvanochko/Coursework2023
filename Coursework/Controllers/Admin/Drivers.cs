using Coursework.Data;
using Coursework.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Coursework.Controllers
{
    public class DriversController : Controller
    {
        private readonly MyDbContext _context;

        public DriversController(MyDbContext context)
        {
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
        public IActionResult ManageDrivers()
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
            var drivers = _context.DriverInfo
            .Include(d => d.UserInfo) // Eager load UserInfo
            .Include(d => d.Vehicle) // Eager load Vehicle
            .ToList();

            return View("/Views/Admin/ManageDrivers/ManageDrivers.cshtml", drivers);
        }

        [HttpGet]
        public IActionResult Edit(int id)
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
            var driver = _context.DriverInfo.FirstOrDefault(d => d.ID == id);

            if (driver == null)
            {
                return NotFound();
            }

            var data = new DriverPageViewModel
            {
                DriverInfo = driver,
                Users = _context.Users.ToList(),
                Cars = _context.CarTable.ToList()
            };

            return View("/Views/Admin/ManageDrivers/EditDriver.cshtml", data);
        }

        [HttpPost]
        public IActionResult Edit(DriverPageViewModel data)
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
            if (!ModelState.IsValid)
            {
                // Repopulate the Cars and Users collections
                data.Cars = _context.CarTable.ToList();
                data.Users = _context.Users.ToList();

                return View("/Views/Admin/ManageDrivers/EditDriver.cshtml", data);
            }

            _context.DriverInfo.Update(data.DriverInfo);
            _context.SaveChanges();

            return RedirectToAction("ManageDrivers");
        }

        [HttpGet]
        public IActionResult Delete(int id)
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
            var driver = _context.DriverInfo.FirstOrDefault(d => d.ID == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View("/Views/Admin/ManageDrivers/DeleteDriver.cshtml", driver);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var driver = _context.DriverInfo.FirstOrDefault(d => d.ID == id);
            if (driver == null)
            {
                return NotFound();
            }

            _context.DriverInfo.Remove(driver);
            _context.SaveChanges();

            return RedirectToAction("ManageDrivers");
        }
    }
}
