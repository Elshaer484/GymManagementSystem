using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Data.Models
{
    public class BaseEntity 
    {
        public int Id { get; set; }

        public DateTime createdAt { get; set; }

        public DateTime? updatedAt { get; set; }


    }
}
