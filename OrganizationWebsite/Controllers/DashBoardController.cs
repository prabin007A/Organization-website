using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganizationWebsite.Data;
using OrganizationWebsite.Models;
using OrganizationWebsite.ViewModels;
using System.Security.Claims;

namespace OrganizationWebsite.Controllers
{
    [Authorize]
    public class DashBoardController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ApplicationDbContext dbContext) : Controller
    {
        private readonly SignInManager<AppUser> signInManager = signInManager;
        private readonly UserManager<AppUser> userManager = userManager;
        private readonly ApplicationDbContext dbContext = dbContext;

        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not logged in.");
            }

            // Fetch the student's details based on the user ID
            var student = dbContext.Users.SingleOrDefault(u => u.Id == userId);

            if (student == null)
            {
                return NotFound("Student not found.");
            }

            return View(student);
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var result = dbContext.Users.FirstOrDefault(x => x.Id == id);
            if (result == null)
            {
                return NotFound();
            }

            var upd = new UpdateVm()
            {
                FirstName = result.FirstName,
                LastName = result.LastName,
                Gender = result.Gender,
                DateOfBirth = result.DateOfBirth,
                Email = result.Email,
                Phone_number = result.PhoneNumber,
                Country = result.Country,
                State = result.State,
                City = result.City,
                Hobbie = result.Hobbies,
                Avatar = result.Avatar,
            };

            return View(upd);
        }

        [HttpPost]
        public IActionResult Edit(UpdateVm model)
        {
            var result = dbContext.Users.Find(model.Id);
            if (result != null)
            {
                result.FirstName = model.FirstName;
                result.LastName = model.LastName;
                result.Gender = model.Gender;
                result.DateOfBirth = model.DateOfBirth;
                result.Email = model.Email;
                result.PhoneNumber = model.Phone_number;
                result.Country = model.Country;
                result.State = model.State;
                result.City = model.City;
                dbContext.SaveChanges();
                return RedirectToAction("Profile");

            }
            return RedirectToAction("Profile");
        }

        public IActionResult Notifications()
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (userId == null)
            //{
            //    return RedirectToAction("Login", "Account");
            //}

            var notifications = dbContext.NotificationTable.ToList();
            return View(notifications);
        }
        public IActionResult MyCourses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = dbContext.Users.FirstOrDefault(u => u.Id == userId);

            return View(user);
        }


        public IActionResult SearchResult()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SearchResult(string SearchString)
        {
            var result = new List<Course>();
            if (!String.IsNullOrEmpty(SearchString))
            {
                result = dbContext.CourseTable.Where(
                    i => i.Course_Name!.Contains(SearchString)).ToList();
            }
            ViewBag.SearchString = SearchString;
            return View(result);
        }

        [HttpPost]
        public IActionResult Join(int courseId, string courseName)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                if (string.IsNullOrEmpty(user.Picked_Courses))
                {
                    user.Picked_Courses = courseName;
                    dbContext.SaveChanges();
                    TempData["Message"] = "You have successfully joined the course.";
                }
                else
                {
                    TempData["Message"] = "You can only join one course at a time.";
                }
            }

            return RedirectToAction("MyCourses");
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string currPass, string newPass)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = dbContext.Users.FirstOrDefault(x => x.Id == userId);

            if (user != null)
            {
                var passwordHasher = new PasswordHasher<AppUser>();
                var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, currPass);

                if (verificationResult == PasswordVerificationResult.Success)
                {
                    user.PasswordHash = passwordHasher.HashPassword(user, newPass);
                    dbContext.SaveChanges();
                    TempData["Msg"] = "You have successfully changed your password.";
                }
                else
                {
                    TempData["Msg"] = "Current password is incorrect.";
                }
            }
            else
            {
                TempData["Msg"] = "Can't change the password at the moment. Please try again later.";
            }

            return RedirectToAction("ChangePassword");
        }
    }
}
