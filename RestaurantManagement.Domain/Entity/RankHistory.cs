using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Domain.Entity
{
    public class RankHistory : BaseClass
    {
        public Guid UserId { get; set; }
        public Guid PizzaId { get; set; }
        public int Rank { get; set; }

        public  User User { get; set; }
        public  Pizza Pizza { get; set; }    
    }
}
