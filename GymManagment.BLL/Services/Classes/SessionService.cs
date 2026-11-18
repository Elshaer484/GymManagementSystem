using AutoMapper;
using GymManagment.BLL.Common;
using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.Services.ViewModels.SessionViewModels;
using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Data.Models.Enum;
using GymManagment.DAL.Repositorities.Classes;
using GymManagment.DAL.Repositorities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GymManagment.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SessionService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate) return Result.Validation("EndDate Must be After StartDate");
            if (model.StartDate.Hour < DateTime.Now.Hour) return Result.Validation("StartDate Must be In The Future");
            if (model.Capacity < 1 || model.Capacity > 25) Result.Validation("Capacity Must Be Between 1 and 25");

            var trainer = await unitOfWork.GetReposotries<Trainer>().GetByIdAsync(model.TrainerId , ct);

            var category = await unitOfWork.GetReposotries<Category>().GetByIdAsync(model.CategoryId , ct);

            if(trainer is null)
            {
                return Result.NotFound("Trainer Is Not Found");
            }
            if(category is null)
            {
                return Result.NotFound("Category Is Not Found");
            }

            var hasConflict = await unitOfWork.GetReposotries<Session>().AnyAsync(s =>
              s.TrainerId == model.TrainerId &&
              model.StartDate < s.EndDate &&
              model.EndDate > s.StartDate, ct);

            if (hasConflict)
                return Result.Validation("This trainer already has a session in this time");


            var isValid = Enum.TryParse<Special>(category.CategoryName, true, out var value);

            

            

            if (!isValid || trainer.specialization != value) return Result.Validation("Can Not Create This Session For This Trainer");

           var Mapped =  mapper.Map<CreateSessionViewModel, Session>(model);

            unitOfWork.GetReposotries<Session>().Add(Mapped);

            var result = await unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.OK() : Result.Fail("Failed To Create Session");



        }

        public async Task<IEnumerable<SessionViewModel>?> GetAllSessionAsync(CancellationToken ct )
        {
            var session = await unitOfWork.SessionRepository.GetAllSessionsWithTrainersAndCatergryAsync(ct);
            if (session is null || !session.Any())
            {
                return null;
            }

            var MappesSessions = session.Select(s => new SessionViewModel()
            {

                Id = s.Id,
                Capacity = s.Capacity,
                CategoryName = s.category.CategoryName,
                TrainerName = s.trainer.Name,
                Description = s.Description,
                EndDate = s.EndDate,
                StartDate = s.StartDate,
            });

            foreach(var Mysession in MappesSessions)
            {
                Mysession.AvailableSlots = Mysession.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(Mysession.Id,ct);
            }
            return MappesSessions;
        }


        public async Task<Result<UpdateSessionViewModel>> GetUpdateSessionAsync(int id, CancellationToken ct)
        {
            var session = await unitOfWork.GetReposotries<Session>().GetByIdAsync(id, ct);

            if(session is null)
            {
                Result<UpdateSessionViewModel>.NotFound("Session Not Found");
            }

            if(session.StartDate < DateTime.Now)
            {
                return Result<UpdateSessionViewModel>.Fail("Can't Update A Session Has Already Started");
            }

            var Booking = await unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(id, ct);
            if(Booking > 0)
            {
                return Result<UpdateSessionViewModel>.Fail("Can't Edit A Session That Has Bookings");
            }
            var mappedSession = mapper.Map<Session, UpdateSessionViewModel>(session);

            return Result<UpdateSessionViewModel>.OK(mappedSession);
        }




        public async Task<Result> RemoveSessionAsync(int id, CancellationToken ct = default)
        {
            var session = await unitOfWork.GetReposotries<Session>().GetByIdAsync(id, ct);

            if(session is null)
            {
                return Result.NotFound("Session Not Found");
            }


            if(session.EndDate >= DateTime.Now)
            {
                return Result.Fail("Can't Delete Session Has Not Ended Yet");

            }

            var booking = await unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(id, ct);

            if(booking > 0)
            {
                return Result.Fail("Can't Delete A Session That Has Bookings");
            }

            unitOfWork.GetReposotries<Session>().Delete(session);

            var res = await unitOfWork.SaveChangesAsync(ct);

            return res > 0 ? Result.OK() : Result.Fail("Can't Delete This Session");
        }





        public async Task< IEnumerable<GetCategoryDropDownListViewModel>> GetCategory(CancellationToken ct)
        {
            var category = await unitOfWork.GetReposotries<Category>().GetAllAsync(ct: ct);
            return mapper.Map<IEnumerable<GetCategoryDropDownListViewModel>>(category);
        }

        public async Task< IEnumerable<GetTrainerDropDownListViewModel>> GetTrainer(CancellationToken ct)
        {
            var trainer = await unitOfWork.GetReposotries<Trainer>().GetAllAsync(ct: ct);
            return mapper.Map<IEnumerable<GetTrainerDropDownListViewModel>>(trainer);
        }

        public async Task<Result<SessionViewModel>> GetSessionDetailsByIdAsync(int id, CancellationToken ct = default)
        {
            var session = await unitOfWork.SessionRepository.GetByIdSessionWithTrainerAndCategoryAsync(id, ct);


            if(session is null)
            {
                return Result<SessionViewModel>.NotFound("Session Not Found");
            }


            var Mapped = mapper.Map<Session, SessionViewModel>(session);
            var slots = Mapped.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(id, ct);


            return Result<SessionViewModel>.OK(Mapped);
        }

        public async Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var session = await unitOfWork.GetReposotries<Session>().GetByIdAsync(id, ct);

            if (session is null)
            {
                Result.NotFound("Session Not Found");
            }
            if (session.StartDate <= DateTime.Now)
                return Result.Fail("Can Not Edit Session That Has Already Started");

            if (model.EndDate <= model.StartDate)
                return Result.Validation("End Date Must Be After StartDate");

            var BookedCount = await unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(id);
            if (BookedCount > 0)
                return Result.Fail("Can Not Update Session that Has Already has Bookings");

            if (model.StartDate <= DateTime.Now)
                return Result.Validation("Start Date Must Be In Future");


            var trainer = await unitOfWork.GetReposotries<Trainer>().GetByIdAsync(model.TrainerId , ct);
            var category = await unitOfWork.GetReposotries<Category>().GetByIdAsync(session.CategoryId , ct);

            var isValid = Enum.TryParse<Special>(category.CategoryName, true, out var CategorySpeciality);

            if(!isValid || trainer.specialization != CategorySpeciality)
            {
                return Result.Validation("Can't Create This Sessoin For This Trainer");
            }
            var mapped =  mapper.Map(model,session);
            unitOfWork.GetReposotries<Session>().Update(mapped);

            var result = await unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.OK() : Result.Fail("Failed To Update Session");
        }
    }
}
