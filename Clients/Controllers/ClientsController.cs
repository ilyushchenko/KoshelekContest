using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Clients.Controllers
{
    public class ClientsController : Controller
    {
        public IActionResult One()
        {
            return View();
        }
        public IActionResult Two()
        {
            return View();
        }
        public IActionResult Three()
        {
            return View();
        }
    }
}