using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.Services.ViewModels.PlanViewModels;
using GymManagment.DAL.Data.DbContexts;
using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Repositorities.Classes;
using GymManagment.DAL.Repositorities.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace GymManagment.Controllers
{
    [Authorize]
    public class PlanController : Controller
    {
        private readonly IPlanService ps;

        public PlanController(IPlanService _planService)
        {
            ps = _planService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plan = await ps.GetAllPlansAsync(ct);

            return View(plan);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var plans = await ps.GetPlanDetailsByIdAsync(id, ct);

            if (!plans.success)
            {
                TempData["ErrorMessage"] = plans.error;
                return RedirectToAction(nameof(Index));
            }
            return View(plans.value);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var plans = await ps.GetUpdatePlanAsync(id, ct);
            if (!plans.success)
            {
                TempData["ErrorMessage"] = plans.error;
                return RedirectToAction(nameof(Index));
            }

            return View(plans.value);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdatePlanViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var plans = await ps.UpdatePlanAsync(id, model, ct);

            if (plans.success)
            {
                TempData["SuccessMessage"] = "plan Updated Successfully";
                return RedirectToAction(nameof(Index));


            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Update Plan";
                return View(model);
            }



        }

        [HttpPost]
        public async Task<IActionResult> Activate(int id , CancellationToken ct)
        {
            var plans = await ps.ToggleActivationAsync(id, ct);

            if (plans.success)
            {
                TempData["SuccessMessage"] = "Plan Status changed Successfully";

            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Toggle Plan Status";
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
// ctrl + k + f