using GymManagment.BLL.Common;
using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.Services.ViewModels.DashBoardViewModels;
using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Repositorities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.BLL.Services.Classes
{
    public class DashBoardService : IDashBoardService
    {
        private readonly IUnitOfWork unitOfWork;

        public DashBoardService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<DashboardViewModel>> GetDashBoard(CancellationToken ct = default)
        {
            var Upsessions = await unitOfWork.GetReposotries<Session>().CountAsync(n => n.StartDate > DateTime.Now); 
            var Onsessions = await unitOfWork.GetReposotries<Session>().CountAsync(n => n.StartDate >= DateTime.Now && DateTime.Now <= n.EndDate); 
            var Completedsessions = await unitOfWork.GetReposotries<Session>().CountAsync(n => DateTime.Now > n.EndDate);

            var TotalMembers = await unitOfWork.GetReposotries<Member>().CountAsync(ct: ct);
            var TotalTrainer = await unitOfWork.GetReposotries<Trainer>().CountAsync(ct: ct);
            var ActiveMember = await unitOfWork.GetReposotries<MemberShip>().CountAsync( n=> n.EndDate > DateTime.Now , ct);


            var model = new DashboardViewModel()
            {
                UpComingSessions = Upsessions,
                OnGoingSessions = Onsessions,
                CompletedSessions = Completedsessions,
                TotalMembers = TotalMembers,
                TrainersCount = TotalTrainer,
                ActiveMember = ActiveMember,


            };

            return Result<DashboardViewModel>.OK(model);
        }
    }
}
