using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using BCrypt.Net;

namespace WebApplication4.Controllers
{
    public class LoginController : Controller
    {
        CompanyManageDbContext db;
        public LoginController (CompanyManageDbContext DB)
        {
            db = DB;
        }
        //private readonly IHtmlLocalizer<LoginController> _localizer;

        //public LoginController(IHtmlLocalizer<LoginController> localizer)
        //{
        //    _localizer = localizer;
        //}
        public IActionResult Index()
        {
            //ViewData["Message"] = _localizer["<b>Hello</b><i></i>"];
            return View();
        }
        //[HttpPost]
        //public IActionResult Authentication(string Username, string Userpass)
        //{
        //    var userDetail = db.Accounts.Where(x => x.UserName == Username && x.UserPass == Userpass).FirstOrDefault();
        //    if (userDetail != null)
        //    {
        //        if (userDetail.UserRole == "Admin")
        //        {
        //            HttpContext.Session.SetString("username", Username);
        //            return RedirectToAction("Index", "Admin");
        //        }
        //        else
        //        {
        //            HttpContext.Session.SetString("username", Username);
        //            return RedirectToAction("Index", "User");
        //        }
        //    }
        //    else
        //    {
        //        ViewData["Error"] = "Username or Password incorrect";
        //        return View("Index");
        //    }
        //}
        // Test autorize Login
        [Authorize]
        public IActionResult Test()
        {
            return View();
        }
        public async Task<IActionResult> Authentication(AddEmploy Acc)
        {
            var userDetail = db.Accounts.Where(x => x.UserName == Acc.UserName).FirstOrDefault();
            if (userDetail != null)
            {
                bool validPassword = BCrypt.Net.BCrypt.Verify(Acc.UserPass, userDetail.UserPass);
                if (validPassword == true)
                {
                        // Obj represent User information
                    var identity = new ClaimsIdentity(new[] 
                    {
                    //
                        new Claim(ClaimTypes.Name, userDetail.UserName),
                        new Claim(ClaimTypes.Role,userDetail.UserRole)
                    }, CookieAuthenticationDefaults.AuthenticationScheme);
                        //this identity tied to with the cookie scheme
                    var principal = new ClaimsPrincipal(identity);//represnet the User
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("Index", userDetail.UserRole);
                    //}
                    //    else
                    //    {
                    //        var identity = new ClaimsIdentity(new[] {
                    //new Claim(ClaimTypes.Name, userDetail.UserName),
                    //        new Claim(ClaimTypes.Role,"User")
                    //    }, CookieAuthenticationDefaults.AuthenticationScheme);
                    //        var principal = new ClaimsPrincipal(identity);
                    //        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    //        return RedirectToAction("Index", "User");
                    //    }
                }
                else
                {
                    ViewData["Error"] = "Username or Password incorrect";
                    return View("Index");
                }
            }
            return View("Index");
        }
        public IActionResult ErrorForbidden()
        {
            return View ();
        }
        public IActionResult ErrorNotLogIn()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}