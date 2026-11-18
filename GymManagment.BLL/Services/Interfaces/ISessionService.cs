using GymManagment.BLL.Common;
using GymManagment.BLL.Services.ViewModels.SessionViewModels;
using GymManagment.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionViewModel>> GetAllSessionAsync(CancellationToken ct);
        Task<Result<SessionViewModel>> GetSessionDetailsByIdAsync(int id , CancellationToken ct);

        Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default);
        Task<Result> RemoveSessionAsync(int id, CancellationToken ct = default);
        Task<Result> UpdateSessionAsync(int id , UpdateSessionViewModel model, CancellationToken ct = default);

        Task<Result<UpdateSessionViewModel>> GetUpdateSessionAsync(int id , CancellationToken ct);

        Task< IEnumerable<GetCategoryDropDownListViewModel>> GetCategory(CancellationToken ct);
        Task< IEnumerable<GetTrainerDropDownListViewModel>> GetTrainer(CancellationToken ct);

    }
}
