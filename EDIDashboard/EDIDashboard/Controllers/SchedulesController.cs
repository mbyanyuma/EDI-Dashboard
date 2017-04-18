using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace EDIDashboard.Controllers
{
    public class SchedulesController : Controller
    {
        // GET: Schedules
        public ActionResult SchedulesTablePage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SchedulesTablejson()
        {


            //jQuery DataTables Param
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            //Find paging info
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Find order columns info
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault()
                                    + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            //find search columns info
            var scheduleName = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var timeOfDay = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var schedule = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var direction = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var tradingParter = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 0;
            int recordsTotal = 0;


            //the 'using' - complier automatically creates try/finally block
            using (IGS_TransactionsEntities dc = new IGS_TransactionsEntities())
            {
                // dc.Configuration.LazyLoadingEnabled = false; // if the table is relational, contain foreign key
                var v = (from a in dc.Schedules select a);

                //SEARCHING...
                if (!string.IsNullOrEmpty(scheduleName))
                {
                    v = v.Where(a => a.ScheduleName.Contains(scheduleName));
                }
                if (!string.IsNullOrEmpty(timeOfDay))
                {
                    v = v.Where(a => a.TimeOfDay.Contains(timeOfDay));
                }
                if (!string.IsNullOrEmpty(schedule))
                {
                    v = v.Where(a => a.Schedule1.Contains(schedule));
                }


                if (!string.IsNullOrEmpty(direction))
                {
                    v = v.Where(a => a.Direction == direction);
                }
                if (!string.IsNullOrEmpty(tradingParter))
                {
                    v = v.Where(a => a.TradingPartner.Contains(tradingParter));
                }

                //SORTING...  (For sorting we need to add a reference System.Linq.Dynamic)
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    v = v.OrderBy(sortColumn + " " + sortColumnDir);
                }

                recordsTotal = v.Count();
                var data = v.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data },
                    JsonRequestBehavior.AllowGet);



                /*
                using (IGS_TransactionsEntities context = new IGS_TransactionsEntities())
            {
                var data = context.Schedules.OrderBy(x => x.ScheduleName).ToList();
                
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
            */

            }
        }
    }
}