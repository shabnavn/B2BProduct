using b2b_solution.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace b2b_solution.Controllers
{
    [CheckSession]
    public class CustomerServiceController : Controller
    {
        DataModel dm = new DataModel();
        public ActionResult CustomerServiceList()
        {
            
            return View();
        }
    }
}