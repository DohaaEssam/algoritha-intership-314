using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace testvezeeta.Core_Layer.Domain
{
    public class Patient
    {
        public int Id { get; set; }
        public string DiscountCodeCoupon { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public virtual List<Booking> Bookings { get; set; }

    }
}
