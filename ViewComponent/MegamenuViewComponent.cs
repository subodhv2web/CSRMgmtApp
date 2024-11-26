using CSRMGMT.Models;
using Microsoft.AspNetCore.Mvc;
using CSRMGMT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSRMGMT.Component
{
    public class MegamenuViewComponent:ViewComponent
    {
        private readonly AppdbContext _context;

        public MegamenuViewComponent(AppdbContext context)
        {
            _context = context;
        }
        
        public IViewComponentResult Invoke()
        {
            List<MenuSubMenu> myList = new List<MenuSubMenu>();
            var li = _context.tblLinksInfo.Where(x => x.ParentLinkID == 0).ToList();

            foreach (var itm in li)
            {
                string slug = AppHelper.GenerateSlug(itm.sLinkName);
                var submenu = _context.tblLinksInfo.Where(x => x.ParentLinkID == itm.sLinkID);
                if (submenu.Count() > 0)
                {
                    myList.Add(new MenuSubMenu {slug=slug, sLinkName = itm.sLinkName, UseURL = itm.UseURL, ExURL=itm.ExURL, sLinkID =itm.sLinkID, hasSubmenu = 1, Submenu = submenu.ToList() });
                }
                else
                {
                    myList.Add(new MenuSubMenu { slug = slug, sLinkName = itm.sLinkName, UseURL=itm.UseURL, ExURL = itm.ExURL, sLinkID = itm.sLinkID, hasSubmenu = 0, Submenu = submenu.ToList() });
                }
            }
            return View("_Menubar.cshtml", myList);
        }
    }
}
