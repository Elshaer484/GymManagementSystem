using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.Services.ViewModels.SessionViewModels;
using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Repositorities.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography.Pkcs;

namespace GymManagment.PL.Controllers
{

    [Authorize]
    public class SessionsController : Controller
    {
        private readonly ISessionService sessionService;

        public SessionsController(ISessionService sessionService)
        {
            this.sessionService = sessionService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var session = await sessionService.GetAllSessionAsync(ct: ct);
            return View(session);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
           await PopulateDownDropListAsync();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model , CancellationToken ct)
        {
            if(!ModelState.IsValid)
            {
               await PopulateDownDropListAsync();
                return View(model);
            }

            var session = await sessionService.CreateSessionAsync(model, ct);
            if(session.success)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            
                TempData["ErrorMessage"] = session.error;
                await PopulateDownDropListAsync();
                return View(model);
            
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id , CancellationToken ct)
        {
            var session = await sessionService.GetSessionDetailsByIdAsync(id , ct);
            if(session.success)
            {
                return View(session.value);
            }
            else
            {
                TempData["ErrorMessage"] = session.error;
                return RedirectToAction(nameof(Index));
            }

        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id, CancellationToken ct)
        {
            var session = await sessionService.RemoveSessionAsync(id, ct);

            if (session.success)
            {
                TempData["SuccessMessage"] = "Session Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = session.error;

            }
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id , CancellationToken ct = default)
        {
            var result = await sessionService.GetUpdateSessionAsync(id, ct);
            if(result.success)
            {
                ViewBag.Trainers = new SelectList(await sessionService.GetTrainer(ct), "Id", "Name");
                return View(result.value);
            }
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));


        }


        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, UpdateSessionViewModel model , CancellationToken ct)
        {

            if(!ModelState.IsValid)
            {
                ViewBag.Trainers = new SelectList(await sessionService.GetTrainer(ct), "Id", "Name");
                return View(model);
            }

            var session = await sessionService.UpdateSessionAsync(id, model, ct);

            if (session.success)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";
                return RedirectToAction(nameof(Index));

            }
            else
            {

                TempData["ErrorMessage"] = session.error;
                ViewBag.Trainers = new SelectList(await sessionService.GetTrainer(ct), "Id", "Name");
                return View(model);
            }
        }



        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var session = await sessionService.GetSessionDetailsByIdAsync(id, ct);

            if(session.success)
            {
                return View(session.value);
            }
            TempData["ErrorMessage"] = session.error;
            return RedirectToAction(nameof(Index));
        }





        private async Task PopulateDownDropListAsync(CancellationToken ct = default)
        {
            ViewBag.Trainers = new SelectList(await sessionService.GetTrainer(ct), "Id", "Name");
            ViewBag.Categories = new SelectList(await sessionService.GetCategory(ct), "Id", "CategoryName");
        }
    }
}
