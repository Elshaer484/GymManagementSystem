using GymManagment.BLL.Attachments;
using GymManagment.BLL.Classes.Interfaces;
using GymManagment.BLL.Common;
using GymManagment.BLL.Services.ViewModels.MemberViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;

namespace GymManagment.PL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class MemberController : Controller
    {
        private readonly ImemberService ms;
        private readonly IAttachmentService attachmentService;

        public MemberController(ImemberService ms , IAttachmentService attachmentService)
        {
            this.ms = ms;
            this.attachmentService = attachmentService;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
           var member = await ms.GetAllMembersAsync(ct);

            return View(member);
        }

        [HttpGet]
        public async Task<IActionResult> Picture(int id , CancellationToken ct = default)
        {
            var member = await ms.GetMemberDetailsByIdAsync(id, ct);

            if(!member.success || string.IsNullOrWhiteSpace(member.value.Photo))
            {
                return NotFound();
            }

            var result = attachmentService.GetFile(member.value.Photo, "Members");
            if(result is null)
            {
                return NotFound();
            }
            return File(result.Value.stream, result.Value.contentType);
        }


        [HttpGet]
        public IActionResult Create() => View();


        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberViewModel model , CancellationToken ct)
        {
            if(!ModelState.IsValid)
            {
                return View(nameof(Create), model);
            }

            var result = await ms.CreateMemberAsync(model, ct);

            if(result.success)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
                return RedirectToAction(nameof(Index));

            }
            TempData["ErrorMessage"] = result.error;
            return View(model);


        }



        [HttpGet]
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            var mem = await ms.GetMemberDetailsByIdAsync(id, ct);
            if(!mem.success)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(mem.value);

        }

        [HttpGet]
        public async  Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var health = await ms.GetHealthRecordDetailsByIdAsync(id, ct);
            if(!health.success)
            {
                TempData["ErrorMessage"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(health.value);

        }



        [HttpGet]
        public async Task<IActionResult> EditMember(int id , CancellationToken ct)
        {
            var record = await ms.UpdateMemberAsync(id, ct);
            if(!record.success)
            {
                TempData["ErrorMessage"] = "Member Is Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(record.value);
        }

        [HttpPost]
        public async Task<IActionResult> EditMember(int id, UpdateMemberViewModel MyModel , CancellationToken ct)
        {
            if(!ModelState.IsValid)
            {
                return View(MyModel);
            }

            var result = await ms.UpdateMemberDetalisAsync(id, MyModel, ct);
                
            if(result.success)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully";

            }
            else
            {
                TempData["ErrorMessage"] = result.error;
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> DeleteMember(int id , CancellationToken ct)
        {
            var memeber = await ms.GetMemberDetailsByIdAsync(id, ct);

            if(!memeber.success)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(memeber.value);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id, CancellationToken ct)
        {
            var member = await ms.DeleteMemberAsync(id, ct);

            if (member.success)
            {
                TempData["SuccessMessage"] = "Member Removed Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = member.error;

            }

            return RedirectToAction(nameof(Index));

        }
    }
}
