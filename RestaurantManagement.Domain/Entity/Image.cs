using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Domain.Entity
{
    public class Image : BaseClass
    {
        public string OriginalName { get; set; }
        public string Path { get; set; }
        public Guid PizzaId { get; set; }
        public  Pizza Pizza { get; set; }

    }
}
