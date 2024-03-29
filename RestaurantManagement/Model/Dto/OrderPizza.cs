using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Api.Model.Dto
{
    public class OrderPizza
    {
        public string OrderId { get; set; }
        public string PizzaId { get; set; }
        public string UserId { get; set; }
        public string AddresId { get; set; }
    }
}
