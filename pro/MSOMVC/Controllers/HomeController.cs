using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MSOSKY.BL;
using MSOMVC.Models;

namespace MSOMVC.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /MSO/

        public ActionResult Index()
        {
            return View(Recommend.GetAll());
        }

        public ActionResult List(string keyword,int pageindex, int type)
        {
            int totalCount=0;
            int pageCount=0;
            int mecs=0;
            var list = ATS.DoSearch(keyword, pageindex, out totalCount, out pageCount, out mecs);

            return View(new HashListView {
                SearchParam=new SearchUnit(type,keyword),
                PageIndex=pageindex,
                TotalCount=totalCount,
                PageCount=pageCount,
                Mecs=mecs,
                List=list});
        }

        public ActionResult Detail(long hashid)
        {
            var model = Models.HashItem.GetOne(hashid);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            model.SearchParam = new SearchUnit(0,"");
            return View(model);
        }


        public ActionResult Help()
        {
            return View();
        }

    }
}
