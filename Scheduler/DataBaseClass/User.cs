using System;
using System.Collections.Generic;

namespace Scheduler.DataBaseClass
{
    public partial class User
    {
        public uint Idusers { get; set; }
        public string UsersLogin { get; set; } = null!;
        public string UsersPassword { get; set; } = null!;
        public string UsersEmail { get; set; } = null!;
        public string? UsersPhone { get; set; }
    }
}
