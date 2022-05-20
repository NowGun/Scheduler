using System;
using System.Collections.Generic;

namespace Scheduler.DataBaseClass
{
    public partial class User
    {
        public User()
        {
            Cases = new HashSet<Case>();
            Groups = new HashSet<Group>();
            GroupsIdgroups = new HashSet<Group>();
            UserHasGroups = new HashSet<UserHasGroup>();
        }

        public uint Idusers { get; set; }
        public string UsersPassword { get; set; } = null!;
        public string UsersEmail { get; set; } = null!;
        public string? UsersPhone { get; set; }
        public string UsersName { get; set; } = null!;
        public string? UsersSurname { get; set; }

        public virtual ICollection<Case> Cases { get; set; }
        public virtual ICollection<Group> Groups { get; set; }

        public virtual ICollection<Group> GroupsIdgroups { get; set; }
        public virtual ICollection<UserHasGroup> UserHasGroups { get; set; }
    }
}
