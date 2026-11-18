using GymManagment.BLL.Common;
using GymManagment.BLL.Services.ViewModels.MemberViewModel;
using GymManagment.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.BLL.Classes.Interfaces
{
    public interface ImemberService
    {
        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default);

        Task<Result> CreateMemberAsync(CreateMemberViewModel member, CancellationToken ct);

        Task<Result<MemberViewModel>> GetMemberDetailsByIdAsync(int id, CancellationToken ct = default);

        Task<Result<HealthRecordViewModel>> GetHealthRecordDetailsByIdAsync(int id, CancellationToken ct = default);

        Task<Result<UpdateMemberViewModel>> UpdateMemberAsync(int id, CancellationToken ct = default);

        Task<Result> UpdateMemberDetalisAsync(int id, UpdateMemberViewModel MyModel, CancellationToken ct = default);

        Task<Result> DeleteMemberAsync(int memberId, CancellationToken ct);

    }
}
