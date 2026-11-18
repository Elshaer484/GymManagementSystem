using GymManagment.BLL.Common;
using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.Services.ViewModels.MemberViewModel;
using GymManagment.BLL.Services.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Identity.Client;

namespace GymManagment.PL.Controllers
{

    [Authorize]
    public class TrainersController : Controller
    {
        private readonly ITrainerService ts;

        public TrainersController(ITrainerService TrainerSer)
        {
            ts = TrainerSer;
        }


        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var record = await ts.GetAllTrainerAsync(ct);

            return View(record);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainerViewModel model , CancellationToken ct)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await ts.CreateTrainerAsync(model, ct);

            if (result.success)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                return View(model);
            }

           
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id , CancellationToken ct)
        {
            var trainer = await ts.GetUpdateTrainerAsync(id, ct);
            if(trainer.success)
            {
                return View(trainer.value);
            }
            TempData["ErrorMessage"] = trainer.error;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id , TrainerUpdateViewModel model ,  CancellationToken ct)
        {

            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await ts.UpdateTrainerAsync(id, model, ct);

            if (result.success)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id , CancellationToken ct)
        {
            var trainer = await ts.GetTrainerDetailsAsync(id, ct);
            if (!trainer.success)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer.value);
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id , CancellationToken ct)
        {
            var trainer = await ts.GetTrainerDetailsAsync(id, ct);
            if(trainer is null)
            {
                TempData["SuccessMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id , CancellationToken ct)
        {
            var trainer = await ts.RemoveTrainerAsync(id, ct);

            if (trainer.success)
                TempData["SuccessMessage"] = "Trainer deleted successfully.";
            else
                TempData["ErrorMessage"] = trainer.error;

            return RedirectToAction(nameof(Index));
        }

    }
}
