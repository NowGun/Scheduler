using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.DataBaseClass
{
    public class UserHasGroup
    {
        public uint UsersIdusers { get; set; }
        public uint GroupsIdgroups { get; set; }

        public virtual Group GroupsIdgroupsNavigation { get; set; }
        public virtual User UsersIdusersNavigation { get; set; }
    }
}
