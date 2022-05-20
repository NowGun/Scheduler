using System;
using System.Collections.Generic;

namespace Scheduler.DataBaseClass
{
    public partial class Casegroup
    {
        public uint Idcasegroup { get; set; }
        public uint GroupsIdgroups { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateOnly? Date { get; set; }
        public sbyte? Done { get; set; }
        public uint CategoryIdcategory { get; set; }

        public virtual Category CategoryIdcategoryNavigation { get; set; } = null!;
        public virtual Group GroupsIdgroupsNavigation { get; set; } = null!;
    }
}
