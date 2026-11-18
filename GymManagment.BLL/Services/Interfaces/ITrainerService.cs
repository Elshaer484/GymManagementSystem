using GymManagment.BLL.Common;
using GymManagment.BLL.Services.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.BLL.Services.Interfaces
{
    public interface ITrainerService
    {
        Task<IEnumerable<TrainerViewModel>> GetAllTrainerAsync(CancellationToken ct);

        Task<Result<TrainerViewModel>> GetTrainerDetailsAsync(int id, CancellationToken ct);

        Task<Result> UpdateTrainerAsync(int id, TrainerUpdateViewModel model, CancellationToken ct);

        Task<Result<TrainerUpdateViewModel>> GetUpdateTrainerAsync(int id, CancellationToken ct);

        Task<Result> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct);

        Task<Result> RemoveTrainerAsync(int id, CancellationToken ct);



    }
}
