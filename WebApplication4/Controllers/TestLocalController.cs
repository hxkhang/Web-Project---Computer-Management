using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;

namespace WebApplication4.Controllers
{
    public class TestLocalController : Controller
    {
        private readonly IHtmlLocalizer<TestLocalController> _localizer;

        public TestLocalController(IHtmlLocalizer<TestLocalController> localizer)
        {
            _localizer = localizer;
        }
        public IActionResult Index()
        {
            ViewData["Message"] = _localizer["<b>Hello</b>"];

            return View();
        }
    }
}