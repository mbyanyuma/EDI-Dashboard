using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace EDIDashboard.Controllers
{
    //previously named it DatatablesController
    public class DatatablesController : Controller
    {
        // GET: Datatables
        public ActionResult DocumentTracking()
        {
            return View();
        }

        //Writing action for return database data 
        [HttpPost]
        public ActionResult DocumentTrackingTable()
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
            var tradingPartnerName = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var direction = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 0;
            int recordsTotal = 0;

            //the 'using' - complier automatically creates try/finally block
            using (IGS_TransactionsEntities dc = new IGS_TransactionsEntities())
            {
                // dc.Configuration.LazyLoadingEnabled = false; // if the table is relational, contain foreign key
                var v = (from a in dc.DocumentTrackings select a);

                //SEARCHING...
                if (!string.IsNullOrEmpty(tradingPartnerName))
                {
                    v = v.Where(a => a.TPName.Contains(tradingPartnerName));
                }
                if (!string.IsNullOrEmpty(direction))
                {
                    v = v.Where(a => a.Direction == direction);
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

            }



            //separation original
            /*
            using (IGS_TransactionsEntities dc = new IGS_TransactionsEntities())
            {
                var data = dc.DocumentTrackings.OrderBy(a => a.TPName).ToList();
                // var data = dc.Customers.OrderBy(a => a.ContactName).ToList();
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
            */
        }

      
    }
}