using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Classes
{
    public class TaskType : OrderedListItemEntity
    {
        public TaskType(string name, int order) : base(name, order) { }

        public TaskType() { }

        public int CalendarId { get; set; }

        public Calendar Calendar { get; set; }

        public ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();
    }
}
