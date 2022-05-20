using System;
using System.Collections.Generic;

namespace Scheduler.DataBaseClass
{
    public partial class Group
    {
        public Group()
        {
            Casegroups = new HashSet<Casegroup>();
            UsersIdusers = new HashSet<User>();
            UserHasGroups = new HashSet<UserHasGroup>();
        }

        public uint Idgroups { get; set; }
        public uint UsersCreate { get; set; }
        public string? GroupsName { get; set; }
        public string? GroupsDescription { get; set; }

        public virtual User UsersCreateNavigation { get; set; } = null!;
        public virtual ICollection<Casegroup> Casegroups { get; set; }

        public virtual ICollection<User> UsersIdusers { get; set; }
        public virtual ICollection<UserHasGroup> UserHasGroups { get; set; }
    }
}
