using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Classes
{
    public class Calendar : OrderedListItemEntity
    {
        public Calendar(string name, int order) : base(name, order) { }

        public Calendar() { }

        public ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();

        public ICollection<TaskStatus> TaskStatuses { get; set; } = new List<TaskStatus>();

        public ICollection<TaskType> TaskTypes { get; set; } = new List<TaskType>();
    }
}
