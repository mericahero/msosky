using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class MSOController : Controller
    {
        //
        // GET: /MSO/

        public ActionResult Index()
        {
            return View();
        }

        public ViewResult List()
        {
            return View();
        }

    }
}
