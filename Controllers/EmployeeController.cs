using Microsoft.AspNetCore.Mvc;
using Northwind.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Northwind.Controllers
{
public class EmployeeController : Controller
{
private NorthwindContext _northwindContext;
private UserManager<AppUser> _userManager;
public EmployeeController(NorthwindContext db, UserManager<AppUser> usrMgr)
    {
        _northwindContext = db;
        _userManager = usrMgr;
    }
[Authorize(Roles = "northwind-employee")]
 public IActionResult EditDiscount() => View();
[Authorize(Roles = "northwind-employee"), HttpPost, ValidateAntiForgeryToken]
public IActionResult EditDiscount(Discount discount)
{
    // Edit customer info
    _northwindContext.EditDiscount(discount);
    return RedirectToAction("Index", "Home");
}
private void AddErrorsFromResult(IdentityResult result)
    {
        foreach (IdentityError error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }
    }
}
}