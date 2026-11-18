using AutoMapper;
using AutoMapper.Execution;
using GymManagment.BLL.Services.ViewModels.MemberViewModel;
using GymManagment.BLL.Services.ViewModels.PlanViewModels;
using GymManagment.BLL.Services.ViewModels.SessionViewModels;
using GymManagment.BLL.Services.ViewModels.TrainerViewModels;
using GymManagment.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.BLL
{
    public class MappingProfile : Profile 
    {
        public MappingProfile()
        {


            MapMember();
            MapSession();
            MapPlan();
            MapTrainer();

        }

        private void MapTrainer()
        {
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.address, opt => opt.MapFrom(src => new Address()
                {
                    BuildingNo = src.BuildingNumber,
                    City = src.City,
                    Street = src.Street
                })).ReverseMap();


            CreateMap<Trainer, TrainerViewModel>()
           .ForMember(dest => dest.Myaddress,
            opt => opt.MapFrom(src => src.address))
           .ForMember(dest => dest.Date0fBirth, opt => opt.MapFrom(src => src.DateofBirth))
           .ForMember(dest => dest.specialization, opt => opt.MapFrom(src => src.specialization.ToString()));


            CreateMap<Trainer, TrainerUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.address.BuildingNo))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.address.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.address.Street))
                .ForMember(dest => dest.Specialties, opt => opt.MapFrom(src => src.specialization));



            CreateMap<TrainerUpdateViewModel, Trainer>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.address, opt => opt.MapFrom(src => new Address()
                {
                    BuildingNo = src.BuildingNumber,
                    City = src.City,
                    Street = src.Street
                }))
                .ForMember(dest => dest.specialization, opt => opt.MapFrom(src => src.Specialties.ToString()));




        }


        private void MapPlan()
        {
            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan, UpdatePlanViewModel>()
                .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Name)).ReverseMap();

        }


        private void MapMember()
        {
            CreateMap<GymManagment.DAL.Data.Models.Member, MemberViewModel>()
                .ForMember(m => m.Address, opt => opt.MapFrom(src => $"{src.address.BuildingNo} - {src.address.Street} - {src.address.City}"))
                .ForMember(m => m.DateOfBirth, opt => opt.MapFrom(src => $"{src.DateofBirth.ToShortDateString()}"));

            CreateMap<HealthRecord, HealthRecordViewModel>().ReverseMap();

            CreateMap<GymManagment.DAL.Data.Models.Member, UpdateMemberViewModel>()
                .ForMember(m => m.BuildingNumber, opt => opt.MapFrom(src => $"{src.address.BuildingNo}"))
                .ForMember(m => m.City, opt => opt.MapFrom(src => $"{src.address.City}"))
                .ForMember(m => m.Street, opt => opt.MapFrom(src => $"{src.address.Street}"));

            CreateMap<UpdateMemberViewModel, GymManagment.DAL.Data.Models.Member>()
                .ForMember(dest => dest.phone, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.address.BuildingNo = src.BuildingNumber;
                    dest.address.Street = src.Street;
                    dest.address.City = src.City;
                });

            CreateMap<CreateMemberViewModel, GymManagment.DAL.Data.Models.Member>()
                .ForMember(dest => dest.address, opt => opt.MapFrom(src => new Address
                {
                    BuildingNo = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City
                }))
                .ForMember(dest => dest.healthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));


        }
        private void MapSession()
        {
            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Category, GetCategoryDropDownListViewModel>();
            CreateMap<Trainer, GetTrainerDropDownListViewModel>();
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.trainer.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.category.CategoryName));

            CreateMap<SessionViewModel, Session>().ReverseMap();

        }
    }
}
