using AutoMapper;
using GymManagment.BLL.Common;
using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.Services.ViewModels.MemberViewModel;
using GymManagment.BLL.Services.ViewModels.PlanViewModels;
using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Repositorities.Interfaces;
using Microsoft.Identity.Client.NativeInterop;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public PlanService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await unitOfWork.GetReposotries<Plan>().GetAllAsync(ct: ct);

            if(!plans.Any())
            {
                return [];
            }

            var model = mapper.Map< IEnumerable<Plan>, IEnumerable<PlanViewModel>>(plans);

            return model;
          

        }

        public async Task<Result<PlanViewModel>> GetPlanDetailsByIdAsync(int PlanId, CancellationToken ct = default)
        {
            var plan = await unitOfWork.GetReposotries<Plan>().GetByIdAsync(PlanId, ct);

            if (plan is null) return null;
            if (await HasActiveMemberShipAsync(PlanId, ct))
            {
                return null;
            }

            var model = mapper.Map<Plan, PlanViewModel>(plan);

            return Result<PlanViewModel>.OK(model);
        }


        public async Task<Result> ToggleActivationAsync(int PlanId, CancellationToken ct)
        {
            var plan = await unitOfWork.GetReposotries<Plan>().GetByIdAsync(PlanId, ct);

            if (plan is null) return Result.NotFound("Plan Not Found");

            if (plan.IsActive && await HasActiveMemberShipAsync(PlanId , ct))
            {
                return Result.Fail("Can't Toggle Activation That Has Already Bookings");
            }

            plan.IsActive = !plan.IsActive;
            plan.updatedAt = DateTime.Now;

            unitOfWork.GetReposotries<Plan>().Update(plan);

            var result = await unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.OK() : Result.Fail("Can't Toggle This Activation");
        }


        public async Task<Result<UpdatePlanViewModel>> GetUpdatePlanAsync(int PlanId, CancellationToken ct)
        {
            var plans = await unitOfWork.GetReposotries<Plan>().GetByIdAsync(PlanId, ct);

            if (plans is null)
            {
                return Result<UpdatePlanViewModel>.NotFound("Plan Not Found");
            }
            if (!plans.IsActive)
            {
                return Result<UpdatePlanViewModel>.Fail("Can't Update This Plan Due To it's Current Status");
            }

            var model = mapper.Map<Plan, UpdatePlanViewModel>(plans);

            return Result<UpdatePlanViewModel>.OK(model);


        }

        public async Task<Result> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct)
        {
            var plans = await unitOfWork.GetReposotries<Plan>().GetByIdAsync(id, ct);

            if (plans is null)
            {
                return Result.NotFound("Plan Not Found");
            }
            if (await HasActiveMemberShipAsync(id, ct))
            {
                return Result.Fail("Can't Update A Plan That Has Already Bookings");
            }

            mapper.Map(model, plans);


            plans.updatedAt = DateTime.Now;


            unitOfWork.GetReposotries<Plan>().Update(plans);

            var result = await unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Can't Update This Plan") ;
        }

        private async Task<bool> HasActiveMemberShipAsync(int id , CancellationToken ct)
        {
            return await unitOfWork.GetReposotries<MemberShip>().AnyAsync(m => m.planId == id && m.EndDate > DateTime.Now , ct);
        }
    }
}
