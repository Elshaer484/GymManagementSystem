using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.BLL.Services.ViewModels.DashBoardViewModels
{
    public class DashboardViewModel
    {
        public int TotalMembers { get; set; }
        public int ActiveMember { get; set; }
        public int TrainersCount { get; set; }
        public int UpComingSessions { get; set; }
        public int OnGoingSessions { get; set; }
        public int CompletedSessions { get; set; }
    }
}
