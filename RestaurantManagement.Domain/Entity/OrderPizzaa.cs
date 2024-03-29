using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Domain.Entity
{
    public class OrderPizzaa : BaseClass
    {
        public Guid OrderId { get; set; }
        public Guid PizzaId { get; set; }
        public Guid UserId { get; set; }
        public Guid AddresId { get; set; }

        public  Order Order { get; set; }    
        public  Pizza Pizza { get; set; }
        public  User User { get; set; }
        public  Address Address { get; set; }


    }
}
