using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication2.Controllers
{
    public class TreeviewController : Controller
    {
        // GET: Treeview
        public ActionResult OnDemand()
        {

            List<SiteMenu> all;
            using (var dc = new StevenCorreia_dbEntities())
            {
                all = dc.SiteMenus.Where(a => a.ParentMenuID== 0).ToList();
            }
            return View(all);
        }

        public JsonResult GetSubMenu(string pid)
        {
            var subMenus = new List<SiteMenu>();
            int pID = 0;
            int.TryParse(pid, out pID);
            using (var dc = new StevenCorreia_dbEntities())
            {
                subMenus = dc.SiteMenus.Where(a => a.ParentMenuID == pID ).OrderBy(a => a.MenuName).ToList();
            }

            return new JsonResult { Data = subMenus, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}