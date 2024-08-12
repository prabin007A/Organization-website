using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizationWebsite.Data;
using OrganizationWebsite.ViewModels;
using OrganizationWebsite.Models;


namespace OrganizationWebsite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDashBoardController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public AdminDashBoardController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult StudentDetails()
        {
            var list = dbContext.Users.ToList();
            return View(list);
        }

        [HttpGet]
        public IActionResult AdminEdit(string id)
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
                Email= result.Email,
                Phone_number=result.PhoneNumber,
                Picked_Courses=result.Picked_Courses,
                Status = result.Status,
            };

            return View(upd);
        }

        [HttpPost]
        public IActionResult AdminEdit(UpdateVm model)
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
                result.Picked_Courses = model.Picked_Courses;
                result.Status = model.Status;
                dbContext.SaveChanges();
                return RedirectToAction("StudentDetails");
            }
            return RedirectToAction("StudentDetails");
        }

        [HttpPost]
        public IActionResult Delete(UpdateVm stud)
        {
            if (stud == null || string.IsNullOrEmpty(stud.Id))
            {
                return RedirectToAction("StudentDetails", "AdminDashBoard");
            }

            var student = dbContext.Users.Find(stud.Id);
            if (student != null)
            {
                dbContext.Users.Remove(student);
                dbContext.SaveChanges();
            }

            return RedirectToAction("StudentDetails", "AdminDashBoard");
        }

        public IActionResult AddNotification()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNotification(NotificationVm noti)
        {
            if(ModelState.IsValid)
            {
                var notification = new Notification
                {
                    Notifications = noti.Notification,
                    Date=noti.Date,
                };
                dbContext.NotificationTable.Add(notification);
                dbContext.SaveChanges();

                TempData["SuccessMessage"] = "Notification added successfully!";

                return RedirectToAction("AddNotification");
            }
            return View(noti);
        }
    }
}
