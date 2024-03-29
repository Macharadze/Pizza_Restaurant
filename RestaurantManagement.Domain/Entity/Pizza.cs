using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Domain.Entity
{
    public class Pizza : BaseClass
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int CaloryCount { get; set; }
        public Guid? ImageId { get; set; }

        public  Image Image { get; set; }
    }
}
