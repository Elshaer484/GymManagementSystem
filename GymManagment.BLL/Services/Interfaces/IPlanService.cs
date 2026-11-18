using GymManagment.BLL.Common;
using GymManagment.BLL.Services.ViewModels.MemberViewModel;
using GymManagment.BLL.Services.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.BLL.Services.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default);
        Task<Result<PlanViewModel>> GetPlanDetailsByIdAsync(int PlanId, CancellationToken ct = default);

        Task<Result> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct);

        Task<Result<UpdatePlanViewModel>> GetUpdatePlanAsync(int PlanId, CancellationToken ct);

        Task<Result> ToggleActivationAsync(int PlanId, CancellationToken ct);
    }
}
