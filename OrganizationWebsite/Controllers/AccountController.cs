using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using OrganizationWebsite.Models;
using OrganizationWebsite.ViewModels;
using System.IO;
using System.Net.Mail;
using System.Net;
using static System.Net.WebRequestMethods;
using Microsoft.Extensions.Caching.Memory;

namespace OrganizationWebsite.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;
        private readonly IMemoryCache memoryCache;


        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,IMemoryCache memoryCache)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.memoryCache = memoryCache;
        }

        [BindProperty]
        public List<string> Hobbies { get; set; }

        private string hobbie;

        public IActionResult Login()
        {
                return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email!);
                if (user != null)
                {
                    var result = await signInManager.PasswordSignInAsync(model.Email!, model.Password!, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        if (await userManager.IsInRoleAsync(user, "Admin"))
                        {
                            return RedirectToAction("Index", "AdminDashBoard");
                        }
                        return RedirectToAction("Index", "DashBoard");
                    }
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View(model);
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm model, IFormFile Avatar)
        {
            if (ModelState.IsValid)
            {
                if (Avatar == null || Avatar.Length == 0)
                {
                    ModelState.AddModelError("myfile", "Please choose an image.");
                    return View(model);
                }

                if (Hobbies != null && Hobbies.Count > 0)
                {
                    hobbie = string.Join(",", Hobbies);
                }

                string fileName = Avatar.FileName;
                fileName = Path.GetFileName(fileName);
                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                string uploadPath = Path.Combine(uploadFolder, fileName);

                if (System.IO.File.Exists(uploadPath))
                {
                    ViewBag.UploadStatus += fileName + " already exists. ";
                    Random fileNumber = new Random();
                    fileName = fileNumber.Next().ToString() + "_" + fileName;
                    uploadPath = Path.Combine(uploadFolder, fileName);
                }

                using (var uploadStream = new FileStream(uploadPath, FileMode.Create))
                {
                    await Avatar.CopyToAsync(uploadStream);
                }
                string path = "/uploads/" + fileName;

                AppUser user = new()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Email,
                    Gender = model.Gender,
                    DateOfBirth = model.DateOfBirth,
                    Email = model.Email,
                    PhoneNumber = model.Phone_number,
                    Country = model.Country,
                    State = model.State,
                    City = model.City,
                    Hobbies = hobbie,
                    Avatar = path
                };

                var result = await userManager.CreateAsync(user, model.Password!);
                if (result.Succeeded)
                {
                    MailMessage mail = new MailMessage
                    {
                        From = new MailAddress("prabinraj48@gmail.com"),
                        Subject = "Registration Confirmation",
                        IsBodyHtml = false,
                        Body = $"Dear {model.FirstName}\n You have successfully registered with Scope India"
                    };
                    mail.To.Add(new MailAddress(model.Email!));

                    using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("prabinraj48@gmail.com", "wwmu libo dyrz fhzt");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }

                    await signInManager.SignInAsync(user, false);
                    TempData["message"] = " you have successfully registered with Scope India.";
                    return RedirectToAction("Index", "DashBoard");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var otp = new Random().Next(100000, 999999).ToString();
                memoryCache.Set(email, otp, TimeSpan.FromMinutes(5));

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("prabinraj48@gmail.com");
                mail.To.Add(new MailAddress(email));
                mail.Subject = "OTP for Password Reset";
                mail.IsBodyHtml = false;
                mail.Body = $"Your OTP for resetting your password is {otp}. It is valid for 5 minutes.";

                using var smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("prabinraj48@gmail.com", "wwmu libo dyrz fhzt");
                smtp.EnableSsl = true;

                try
                {
                    smtp.Send(mail);
                    TempData["Msg"] = "An OTP has been sent to your email.";
                    TempData["OtpRequested"] = true;
                    TempData["Email"] = email;
                    return RedirectToAction("ForgotPassword");
                }
                catch (Exception)
                {

                    TempData["Msg"] = "There was an error sending the OTP. Please try again later.";
                    return RedirectToAction("ForgotPassword");
                }
            }

            TempData["Msg"] = "No user found with the provided email address.";
            return RedirectToAction("ForgotPassword");
        }

        [HttpPost]
        public async Task<IActionResult> ValidateOtp(string email, string otp, string newPassword)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                if (memoryCache.TryGetValue(email, out string? storedOtp) && storedOtp == otp)
                {
                    var passwordHasher = new PasswordHasher<AppUser>();
                    user.PasswordHash = passwordHasher.HashPassword(user, newPassword);
                    await userManager.UpdateAsync(user);

                    TempData["Msg"] = "Password has been successfully reset.";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["Msg"] = "Invalid or expired OTP.";
                    TempData["OtpRequested"] = true;
                    TempData["Email"] = email;
                    return RedirectToAction("ForgotPassword");
                }
            }

            TempData["Msg"] = "No user found with the provided email address.";
            return RedirectToAction("ForgotPassword");
        }

    }
}
