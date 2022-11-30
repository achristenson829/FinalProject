using Microsoft.AspNetCore.Mvc;
using Northwind.Models;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Northwind.Controllers
{
    public class ProductController : Controller
    {
        // this controller depends on the NorthwindRepository
        private NorthwindContext _northwindContext;
        public ProductController(NorthwindContext db) => _northwindContext = db;
        public IActionResult Category() => View(_northwindContext.Categories.OrderBy(c => c.CategoryName));
        public IActionResult Index(int id){
            ViewBag.id = id;
            return View(_northwindContext.Categories.OrderBy(c => c.CategoryName));
        }
        public IActionResult Discounts() => View(_northwindContext.Discounts.Where(d => d.StartTime <= DateTime.Now && d.EndTime > DateTime.Now));
        [Authorize(Roles = "northwind-employee")]
        public IActionResult EditDiscount() => View(_northwindContext.Discounts.OrderBy(d => d.DiscountId));
        [Authorize(Roles = "northwind-employee"), HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditDiscount(Discount discount)
            {
                _northwindContext.EditDiscount(discount);
                return RedirectToAction("Index", "Home");
            }
    }
}
