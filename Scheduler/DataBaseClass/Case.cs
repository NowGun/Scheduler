using System;
using System.Collections.Generic;

#nullable disable

namespace Scheduler.DataBaseClass
{
    public partial class Case
    {
        public uint Idcase { get; set; }
        public uint UsersIdusers { get; set; }
        public string CaseTitle { get; set; }
        public string CaseDescription { get; set; }
        public sbyte? CaseBookmark { get; set; }
        public DateOnly? CaseDate { get; set; }
        public TimeOnly? CaseTimestart { get; set; }
        public TimeOnly? CaseTimeend { get; set; }
        public sbyte? CaseDone { get; set; }

        public virtual User UsersIdusersNavigation { get; set; }
    }
}
