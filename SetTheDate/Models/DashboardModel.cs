using System.Collections.Generic;

namespace SetTheDate.Models
{
    public class DashboardModel
    {
        public DashboardModel()
        {
            UserEventModelList = new List<UserEventModel>();
        }
        public int DraftEventCount { get; set; }
        public int ActiveEventCount { get; set; }
        public int CompletedEventCount { get; set; }
        public string Name { get; set; }
        public List<UserEventModel> UserEventModelList { get; set; }

    }
}
