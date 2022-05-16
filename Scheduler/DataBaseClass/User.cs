using System;
using System.Collections.Generic;

#nullable disable

namespace Scheduler.DataBaseClass
{
    public partial class User
    {
        public User()
        {
            Cases = new HashSet<Case>();
        }

        public uint Idusers { get; set; }
        public string UsersPassword { get; set; }
        public string UsersEmail { get; set; }
        public string UsersPhone { get; set; }
        public string UsersName { get; set; }
        public string UsersSurname { get; set; }

        public virtual ICollection<Case> Cases { get; set; }
    }
}
