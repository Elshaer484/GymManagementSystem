using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Data.Models.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.BLL.Services.ViewModels.TrainerViewModels
{
    public class TrainerViewModel
    {
        public int Id { get; set; }

        public string? Photo { get; set; }

        public string Name { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string Phone { get; set; } = default!;

        public string Date0fBirth { get; set; } = default!;

        public string Gender { get; set; } = default!;

        public Address Myaddress { get; set; } = default!;
        public string specialization { get; set; }
    }
}
