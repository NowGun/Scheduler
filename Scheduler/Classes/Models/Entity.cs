using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Classes
{
    public class Entity : Entity<int>
    {
    }

    public class Entity<T>
    {
        public T Id { get; set; }
    }
}
