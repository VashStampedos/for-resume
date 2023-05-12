using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebWithEntity.Models;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using MailKit.Net.Smtp;

namespace WebWithEntity.Controllers
{
    public class AccountController : Controller
    {
        private Tshop2Context db;
        public AccountController(Tshop2Context context)
        {
            db = context;
            
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                Users user = await db.Users.FirstOrDefaultAsync(x => x.Login == model.Email);
                if (user == null)
                {
                    var newUser = new Users();
                    newUser.Login = model.Email;
                    newUser.Password = model.Password;
                    newUser.ConfirmedEmail = false;
                    var emailMessage = new MimeMessage();
                    emailMessage.From.Add(new MailboxAddress("Администрация сайта", "vladpashik2000@gmail.com"));
                    emailMessage.To.Add(new MailboxAddress("", newUser.Login));
                    emailMessage.Subject = "Email confirmation";
                    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = string.Format("Для завершения регистрации перейдите по ссылке:" +
                                "<a href=\"{0}\" title=\"Подтвердить регистрацию\">{0}</a>", Url.Action("ConfirmEmail", "Account", new { Token = newUser.Id, Email = newUser.Login, Password = newUser.Password }, Request.Scheme))
                        
                    };
                    using (var client = new SmtpClient())
                    {
                        await client.ConnectAsync("smtp.gmail.com", 465, true);
                        await client.AuthenticateAsync("vladpashik2000@gmail.com", "w1w2w3w4");
                        await client.SendAsync(emailMessage);
                        await client.DisconnectAsync(true);
                    }
                    return RedirectToAction("Confirm", "Account", new { Email = newUser.Login });
              
                }
                ModelState.AddModelError("", "Такой пользователь уже существует");
            }
            
            return View(model);
        }

        public IActionResult Confirm(string Email)
        {
            return View("Confirm","На почтовый адрес " + Email + " высланы дальнейшие " +
                    "инструкции по завершению регистрации");
        }

        public async Task<ActionResult> ConfirmEmail(string Token, string Email, string Password)
        { 
            var user = await db.Users.FirstOrDefaultAsync(x => x.Login == Email);
            if (user == null)
            {
                    var newUser = new Users();
                    newUser.Login = Email;
                    newUser.Password = Password;
                    newUser.ConfirmedEmail = true;
                    db.Users.Add(newUser);
                    await db.SaveChangesAsync();
                    await Authenticate(newUser.Login);
                    return RedirectToAction("Index", "Home");
                    //return RedirectToAction("Index", "Home", new { ConfirmedEmail = user.Email });
            }
            else
            {
                return RedirectToAction("Confirm", "Account", new { Email = "" });
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Users user =await db.Users.FirstOrDefaultAsync(x => x.Login == model.Email && x.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(model.Email);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        private async Task Authenticate(string username)
        {
            // создаем один claim
            var claims = new List<Claim>
            { 
                new Claim(ClaimsIdentity.DefaultNameClaimType, username)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
