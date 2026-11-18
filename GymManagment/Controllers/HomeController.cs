using GymManagment.BLL.Services.Interfaces;
using GymManagment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GymManagment.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IDashBoardService ds;

        public HomeController(IDashBoardService ds)
        {
            this.ds = ds;
        }

        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var model = await ds.GetDashBoard(ct);

            return View(model.value);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[HttpGet]
        //public async Task<IActionResult> GetInfo(CancellationToken ct = default)
        //{
        //    if(!info.success)
        //    {
        //        TempData["ErroeMessage"] = "Unable To Create A DashBoard";
        //    }
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
