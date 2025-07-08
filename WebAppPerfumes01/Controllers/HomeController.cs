using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppPerfumes01.Data;
using WebAppPerfumes01.Models;
using WebAppPerfumes01.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace WebAppPerfumes01.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;
    private UserManager<IdentityUser> _userManager;
    private readonly EmailService _emailService;
    private SignInManager<IdentityUser> _signInManager;

    public HomeController(ILogger<HomeController> logger , AppDbContext context , UserManager<IdentityUser> userManager, EmailService emailService , SignInManager<IdentityUser> signInManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _emailService = emailService;
        _signInManager = signInManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    //RegisterViewModel
    [HttpGet]
    public IActionResult Register()
    {

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // تحقق إذا كان المستخدم موجود مسبقاً في ASP.NET Identity
        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            ModelState.AddModelError(string.Empty, "هذا البريد مسجل بالفعل.");
            return View(model);
        }

        // إنشاء مستخدم جديد في ASP.NET Identity
        var identityUser = new IdentityUser
        {
            UserName = model.Email,
            Email = model.Email
        };

        // تحديد كلمة المرور (يفضل إضافتها للنموذج)
        var result = await _userManager.CreateAsync(identityUser, "Default@123");

        if (result.Succeeded)
        {
            // حفظ المستخدم في جدول User المخصص
            var customUser = new User
            {
                Name = $"{model.FirstName} {model.LastName}",
                Email = model.Email,
                Role = "User"
            };

            _context.users.Add(customUser);
            await _context.SaveChangesAsync();

            // يمكن تسجيل الدخول مباشرة إذا أردت:
            // await _signInManager.SignInAsync(identityUser, isPersistent: false);
            ViewBag.SuccessMessage = "تم التسجيل بنجاح، سيتم تحويلك لتسجيل الدخول بعد ثوانٍ.";

            // عرض نفس الصفحة مع رسالة النجاح (بدون تحويل فوري في السيرفر)
            return View();
            // return RedirectToAction("Index", "CPanel", new { area = "User" });
        }

        // التعامل مع الأخطاء إن وجدت
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    /* [HttpPost]
     public async Task<IActionResult> Register(RegisterViewModel model)
     {
         if (!ModelState.IsValid)
         {
             return View(model);
         }

         var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
         if (existingUser != null)
         {
             ModelState.AddModelError(string.Empty, "This email is already registered.");
             return View(model);
         }

         var user = new ApplicationUser
         {
             UserName = model.Email,
             Email = model.Email,
             FullName = $"{model.FirstName} {model.LastName}"
         };

         _context.Users.Add(user);
         await _context.SaveChangesAsync();

         return RedirectToAction("Index", "CPanel", new { area = "User" });
     }*/



    //LoginViewModel
    // GET: Account/Login
    public IActionResult Login()
    {

        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {

        HttpContext.Session.SetString("UserEmail", model.Email);

        if (ModelState.IsValid)
        {
            HttpContext.Session.SetString("UserEmail", model.Email);


            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles.Contains("ADMIN"))
                    {
                        return RedirectToAction("Index", "CPanel", new { area = "Admin" });
                    }

                    else if (roles.Contains("USER"))
                    {
                        HttpContext.Session.SetString("UserEmail", model.Email);

                        return RedirectToAction("Index", "CPanel", new { area = "User" });
                    }

                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
        }

        return View(model);
    }
       /* if (!ModelState.IsValid) return View(model);

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == model.Email);

        if (user == null)
        {
            ModelState.AddModelError("", "Invalid login");
            return View(model);
        }

        // Save session
        // HttpContext.Session.SetInt32("UserId", user.Id);

        return RedirectToAction("Index", "CPanel", new { area = "User" });*/
    



    //LoginCode
    [HttpPost]
    public async Task<IActionResult> SendCode(string email)
    {


        // 1. ????? ??? ??????
        var code = new Random().Next(100000, 999999).ToString();

        // 2. ??? ????? ?????? ?? ???Session
        HttpContext.Session.SetString("VerificationCode", code);
        HttpContext.Session.SetString("UserEmail", email);

        // 3. ???? ??????? (SMTP ?? Gmail)
        await SendEmailAsync(email, "Your verification code", $"Your code is: {code}");

        // 4. ????? ????? ????? ?????
        return RedirectToAction("VerifyCode");
    }

    [HttpGet]
    public IActionResult VerifyCode()
    {
        if (HttpContext.Session.GetString("UserEmail") == null)
        {
            return RedirectToAction("Login", "Home", new { area = "" });
        }

        var email = HttpContext.Session.GetString("UserEmail");

        var model = new LoginViewModel
        {
            Email = email
        };

        return View(model);
    }
    [HttpPost]
    public IActionResult VerifyCode(LoginViewModel model)
    {
        var storedCode = HttpContext.Session.GetString("VerificationCode");

        if (model.Code == storedCode)
        {


            if (model.Email.Equals("PERFUMESALES2025@gmail.com"))
            {
                return RedirectToAction("Index", "CPanel", new { area = "Admin" });
            }
            else { return RedirectToAction("Index", "CPanel", new { area = "User" }); }
        }


        ModelState.AddModelError("Code", "Incorrect code.");
        return View(model);
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("PERFUMECOLLECTION2025@gmail.com", "hlszuvuddptveuot"),
            EnableSsl = true // Ensures a secure connection
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress("PERFUMECOLLECTION2025@gmail.com"),
            Subject = subject,
            Body = body,
            IsBodyHtml = false
        };
        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }


    public IActionResult Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return View(new List<Product>());
        }

        var products = _context.Products
            .Where(p => p.Name.Contains(query) || p.Description.Contains(query) || p.Name.Contains(query))
            .ToList();

        return View(products);
    }

    public IActionResult Details(int id)
    {

        var product = _context.Products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }


    [HttpPost]
    public async Task<IActionResult> SubscribeEmail(string email)
    {
        if (!string.IsNullOrWhiteSpace(email))
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                TempData["EmailExists"] = true;
                return RedirectToAction("Index");
            }

            var user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                TempData["Success"] = true;
            }
        }

        return RedirectToAction("Index");
    }



    public IActionResult Contact()
    {
        return View();
    }

    public IActionResult ContactSuccess()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SubmitContact(ContactUsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Contact", model);
        }
        if (model.Email != "Not Provided")
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser == null)
            {
                var newUser = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(newUser, "Default@123");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, "User");
                }
            }
        }
        string Subject = "You have received a new message from your website contact form.";
        string Body = $"A new contact message has been received.\n\n" +
                $"Name: {model.Name} \n" +
                $"Email: {model.Email}\n" +
                $"Phone: {model.Phone}\n" +
                $"Subject: {model.Message}\n";

        _emailService.SendOrderConfirmationEmail(Subject, Body);

        return View("ContactSuccess");
    }





}
