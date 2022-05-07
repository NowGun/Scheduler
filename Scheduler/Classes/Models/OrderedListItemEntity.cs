using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Classes
{
    public class OrderedListItemEntity : Entity
    {
        public OrderedListItemEntity(string name, int order) => (Name, Order) = (name, order);

        public OrderedListItemEntity() { }

        public string Name { get; set; }

        public int Order { get; set; }
    }
}
