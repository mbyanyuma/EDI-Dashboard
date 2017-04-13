using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EDIDashboard.Controllers
{
    public class SchedulesController : Controller
    {
        // GET: Schedules
        public ActionResult SchedulesTablePage()
        {
            return View();
        }

        public ActionResult SchedulesTablejson()
        {
            using (IGS_TransactionsEntities context = new IGS_TransactionsEntities())
            {
                var data = context.Schedules.OrderBy(x => x.ScheduleName).ToList();
                
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}