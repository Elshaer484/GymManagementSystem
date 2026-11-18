using AutoMapper;
using GymManagment.BLL.Attachments;
using GymManagment.BLL.Classes.Interfaces;
using GymManagment.BLL.Common;
using GymManagment.BLL.Services.ViewModels.MemberViewModel;
using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Repositorities.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace GymManagment.BLL.ViewModels.Classes
{
    public class MemberService : ImemberService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IAttachmentService attachmentService;

        public MemberService(IUnitOfWork unitOfWork , IMapper mapper , IAttachmentService attachmentService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.attachmentService = attachmentService;
        }

        public async Task<Result> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct)
        {
            var Emailex = await unitOfWork.GetReposotries<Member>().AnyAsync(x => x.Email == model.Email, ct);
            var Phoneix = await unitOfWork.GetReposotries<Member>().AnyAsync(x => x.phone == model.Phone, ct);

            if (Emailex || Phoneix)
            {
                return Result.Validation("Email and Phone Is Dublicated");
            }
            else
            {
                var storedPhotoName = await attachmentService.UploadAsync(model.PhotoFile.OpenReadStream(), model.PhotoFile.FileName, "Members");
                if (string.IsNullOrWhiteSpace(storedPhotoName.value)) return Result.NotFound("Photo Not Found");


                var Mymember = mapper.Map<CreateMemberViewModel, Member>(model);

                Mymember.Photo = storedPhotoName.value;

                 unitOfWork.GetReposotries<Member>().Add(Mymember);


                var result = await unitOfWork.SaveChangesAsync(ct);
                if(result > 0)
                {
                    return Result.OK();
                }
                else
                {
                    attachmentService.Delete(storedPhotoName.value, "Members");
                    return Result.Fail("Failed To Delte");
                }    

            }
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await unitOfWork.GetReposotries<Member>().GetAllAsync(ct: ct);

            if(!members.Any())
            {
                return [];
            }
            var MemberViewModel = mapper.Map<IEnumerable<Member> , IEnumerable<MemberViewModel>>(members);

            return MemberViewModel;
            
        }

        public async Task<Result<HealthRecordViewModel>?> GetHealthRecordDetailsByIdAsync(int MemberId, CancellationToken ct = default)
        {
            var loc = await unitOfWork.GetReposotries<HealthRecord>().FirstOrDefaultAsync(x => x.memberId == MemberId , ct: ct);
            if (loc is null)
            {
                return Result<HealthRecordViewModel>.NotFound("Health Record Not Found");
            }

            var model = mapper.Map<HealthRecord, HealthRecordViewModel>(loc);

            return Result<HealthRecordViewModel>.OK(model);
        }

        public async Task<Result<MemberViewModel>> GetMemberDetailsByIdAsync(int MemberId, CancellationToken ct = default)
        {

            var member = await unitOfWork.GetReposotries<Member>().GetByIdAsync(MemberId, ct);


            if (member is null)
            {
               return Result<MemberViewModel>.NotFound("Member Not Found");
            }

            var model = mapper.Map<Member, MemberViewModel>(member);


            var activeMember = await unitOfWork.GetReposotries<MemberShip>().FirstOrDefaultAsync(x => x.memberId == MemberId && x.EndDate > DateTime.Now);

            if(activeMember is not null)
            {
                var plans = await unitOfWork.GetReposotries<Plan>().GetByIdAsync(activeMember.planId, ct);
                model.PlanName = plans?.Name;
                model.MembershipStartDate = activeMember.createdAt.ToString();
                model.MembershipEndDate = activeMember.EndDate.ToString();
            }

            return Result<MemberViewModel>.OK(model);
        }

        public async Task<Result> UpdateMemberDetalisAsync(int id, UpdateMemberViewModel MyModel, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetReposotries<Member>().GetByIdAsync(id, ct);

            if(member is null) return Result.NotFound("Member Not Found");

            var emailEx = await unitOfWork.GetReposotries<Member>().AnyAsync(x => x.Email == member.Email && x.Id != member.Id);
            var PhoneEx = await unitOfWork.GetReposotries<Member>().AnyAsync(x => x.phone == member.phone && x.Id != member.Id);

            if (emailEx || PhoneEx) return Result.Validation("Email and Phone Is Dublicated");


            mapper.Map(MyModel, member);
            member.updatedAt = DateTime.Now;



            unitOfWork.GetReposotries<Member>().Update(member);

            var result = await unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.OK() : Result.Fail("Can't Update This Member");
        }

        public async Task<Result<UpdateMemberViewModel>?> UpdateMemberAsync(int MemberId, CancellationToken ct = default)
        {
            var record = await unitOfWork.GetReposotries<Member>().GetByIdAsync(MemberId, ct);

            if (record is null) return Result<UpdateMemberViewModel>.NotFound("Member Not Found");


            var model = mapper.Map<Member, UpdateMemberViewModel>(record);
            return Result<UpdateMemberViewModel>.OK(model);
        }

        public async Task<Result> DeleteMemberAsync(int MemberId, CancellationToken ct)
        {
            var member = await unitOfWork.GetReposotries<Member>().GetByIdAsync(MemberId, ct);
            if (member is null) return Result.NotFound("Member Not Found");


            var BookingMember = await unitOfWork.GetReposotries<Booking>().AnyAsync(x => x.memberId == MemberId && x.session.EndDate > DateTime.Now, ct);

            unitOfWork.GetReposotries<Member>().Delete(member);

            var result = await unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.OK() : Result.Fail("Can't Delete This Member");

        }
    }
}
