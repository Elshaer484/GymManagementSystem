using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymManagment.DAL.Data.Models
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; }

        public ICollection<Session> Sessions { get; set; } = default!;


    }
}
