using GymManagment.DAL.Data.Models.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Data.Models
{
    public abstract class GymUser : BaseEntity
    {
        public string Name { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string phone { get; set; } = default!;

        public DateOnly DateofBirth { get; set; }

        public Gender gender { get; set; }

        public Address address { get; set; } 

    }
    [Owned]
    public class Address
    {
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string BuildingNo { get; set; } = default!;
    }
}
