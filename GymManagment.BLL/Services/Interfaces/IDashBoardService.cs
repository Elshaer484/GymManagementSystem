using GymManagment.BLL.Common;
using GymManagment.BLL.Services.ViewModels.DashBoardViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.BLL.Services.Interfaces
{
    public interface IDashBoardService
    {
        public Task<Result<DashboardViewModel>> GetDashBoard(CancellationToken ct);
    }
}
