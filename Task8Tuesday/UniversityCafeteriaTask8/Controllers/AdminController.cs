using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityCafeteriaTask8.Models;
using UniversityCafeteriaTask8.ViewModels;

namespace UniversityCafeteriaTask8.Controllers;

[Authorize(Roles = "SuperAdmin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _users;

    public AdminController(UserManager<ApplicationUser> users) => _users = users;

    public async Task<IActionResult> Index()
    {
        var model = new List<UserRowViewModel>();
        foreach (var user in await _users.Users.OrderBy(u => u.Email).ToListAsync())
        {
            var roles = await _users.GetRolesAsync(user);
            model.Add(new UserRowViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? "",
                Role = roles.FirstOrDefault() ?? "User"
            });
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var user = await _users.FindByIdAsync(id);
        if (user == null) return NotFound();
        var roles = await _users.GetRolesAsync(user);

        return View(new EditUserRoleViewModel
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email ?? "",
            Role = roles.FirstOrDefault() ?? "User"
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditUserRoleViewModel model)
    {
        if (model.Id == _users.GetUserId(User))
        {
            TempData["Error"] = "You cannot change your own role.";
            return RedirectToAction("Index");
        }

        if (model.Role != "User" && model.Role != "Admin" && model.Role != "SuperAdmin")
            return BadRequest();

        var user = await _users.FindByIdAsync(model.Id);
        if (user == null) return NotFound();

        var oldRoles = await _users.GetRolesAsync(user);
        if (oldRoles.Count > 0)
        {
            var removed = await _users.RemoveFromRolesAsync(user, oldRoles);
            if (!removed.Succeeded) return BadRequest("Could not remove old roles.");
        }

        var added = await _users.AddToRoleAsync(user, model.Role);
        if (!added.Succeeded) return BadRequest("Could not add the new role.");

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        if (id == _users.GetUserId(User))
        {
            TempData["Error"] = "You cannot delete your own account.";
            return RedirectToAction("Index");
        }

        var user = await _users.FindByIdAsync(id);
        if (user == null) return NotFound();

        // Related requests/items are deleted by the database's cascade relationship.
        var result = await _users.DeleteAsync(user);
        if (!result.Succeeded) TempData["Error"] = "Could not delete the user.";
        return RedirectToAction("Index");
    }
}
