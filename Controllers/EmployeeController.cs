
using Microsoft.AspNetCore.Mvc;
using Northwind.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Northwind.Controllers
{
    public class EmployeeController : Controller
    {
        // this controller depends on the NorthwindRepository & the UserManager
        private NorthwindContext _northwindContext;
        private UserManager<AppUser> _userManager;
        public EmployeeController(NorthwindContext db, UserManager<AppUser> usrMgr)
        {
            _northwindContext = db;
            _userManager = usrMgr;
        }
       
        [Authorize(Roles = "northwind-employee")]
         public IActionResult ManageDiscounts() => View(_northwindContext.Discounts);

        public ViewResult Create() => View();

        public IActionResult Edit(int id)
        {
            Discount discount = _northwindContext.Discounts.Find(id);
            return View(discount);
        }
        [HttpPost]
        public IActionResult Edit(Discount d)
        {
             _northwindContext.EditDiscount(d); 
            return RedirectToAction("ManageDiscounts");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var itemToDelete = _northwindContext.Discounts.Where(d => d.DiscountId == id);
            if(itemToDelete != null) 
            {
                Discount discount = _northwindContext.Discounts.Find(id);
                _northwindContext.Discounts.Remove(discount);
                _northwindContext.SaveChanges();
            }
            return RedirectToAction("ManageDiscounts");
        }
    
        [HttpPost]
        public IActionResult Create(Discount discount)
        {
            if (ModelState.IsValid)
            {
                if (_northwindContext.Discounts.Any(d => d.Code == discount.Code))
                {
                    ModelState.AddModelError("", "Discount code must be unique");
                    return View(discount);
                }
                else {
                    _northwindContext.AddDiscount(discount);   
                    return RedirectToAction("ManageDiscounts"); 
                }        
            }
            return View(discount);
        }
    
    }
}