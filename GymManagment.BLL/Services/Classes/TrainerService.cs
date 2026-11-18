using AutoMapper;
using GymManagment.BLL.Common;
using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.Services.ViewModels.TrainerViewModels;
using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Repositorities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TrainerService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }


        public async Task<Result> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct)
        {
            var Emailex = await unitOfWork.GetReposotries<Trainer>().AnyAsync(n => n.Email == model.Email, ct);
            var Phoneix = await unitOfWork.GetReposotries<Trainer>().AnyAsync(n => n.phone == model.Phone, ct);

            if (Emailex || Phoneix)
            {
                return Result.Validation("Email And Phone Is Dublicated");
            }

            var MyModel = mapper.Map<CreateTrainerViewModel, Trainer>(model);

            unitOfWork.GetReposotries<Trainer>().Add(MyModel);

            var result = await unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.OK() : Result.Fail("Can't Create This Trainer");
        }




        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainerAsync(CancellationToken ct)
        {
            var trainer = await unitOfWork.GetReposotries<Trainer>().GetAllAsync(ct: ct);

            var model = mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerViewModel>>(trainer);

            return model;

        }





        public async Task<Result<TrainerViewModel>> GetTrainerDetailsAsync(int id, CancellationToken ct)
        {
            var trainer = await unitOfWork.GetReposotries<Trainer>().GetByIdAsync(id, ct);

            if (trainer is null) return Result<TrainerViewModel>.NotFound("Trainer Not Found");

            var model = mapper.Map<Trainer, TrainerViewModel>(trainer);

            return Result<TrainerViewModel>.OK(model);
        }


        public async Task<Result<TrainerUpdateViewModel>> GetUpdateTrainerAsync(int id, CancellationToken ct)
        {
            var trainer = await unitOfWork.GetReposotries<Trainer>().GetByIdAsync(id, ct);
            if (trainer is null) return Result<TrainerUpdateViewModel>.NotFound("Trainer Not Found");

            var model = mapper.Map<Trainer, TrainerUpdateViewModel>(trainer);

            return Result<TrainerUpdateViewModel>.OK(model);
        }


        public async Task<Result> UpdateTrainerAsync(int id, TrainerUpdateViewModel model, CancellationToken ct)
        {
            var trainer = await unitOfWork.GetReposotries<Trainer>().GetByIdAsync(id, ct);

            if(trainer is null)
            {
                return Result.NotFound("Trainer Not Found");
            }
            var Emailex = await unitOfWork.GetReposotries<Trainer>().AnyAsync(n => n.Email == model.Email && n.Id != id, ct);
            var Phoneix = await unitOfWork.GetReposotries<Trainer>().AnyAsync(n => n.phone == model.Phone && n.Id != id, ct);

            if (Emailex || Phoneix)
            {
                return Result.Validation("Email And Phone Is Dublicated");
            }

            var mappedModel = mapper.Map(model , trainer);



            trainer.updatedAt = DateTime.Now;

            unitOfWork.GetReposotries<Trainer>().Update(mappedModel);
            var result = await unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Can't Update This Trainer");

        }
        public async Task<Result> RemoveTrainerAsync(int id, CancellationToken ct)
        {
            var trainer = await unitOfWork.GetReposotries<Trainer>().GetByIdAsync(id, ct);

            if (trainer is null) return Result.NotFound("Trainer Not Found"); ;

            var Sessions = await unitOfWork.GetReposotries<Session>().AnyAsync(n => n.Id == trainer.Id && n.EndDate > DateTime.Now , ct);

            if(Sessions)
            {
                return Result.Fail("There's A Session Assigned To This Trainer");
            }

            unitOfWork.GetReposotries<Trainer>().Delete(trainer);
            var result = await unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Can't Delete This Trainer");

        }
    }
}
