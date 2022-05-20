using System;
using System.Collections.Generic;

namespace Scheduler.DataBaseClass
{
    public partial class Category
    {
        public Category()
        {
            Casegroups = new HashSet<Casegroup>();
        }

        public uint Idcategory { get; set; }
        public string CategoryName { get; set; } = null!;

        public virtual ICollection<Casegroup> Casegroups { get; set; }
    }
}
