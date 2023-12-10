using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using testvezeeta.Core_Layer.Enums;

namespace testvezeeta.Core_Layer.Domain
{
    public class DiscountCode
    {
        public int Id { get; set; }
        public string DiscountCodeCoupon{ get; set; }
        public int RequestNum { get; set; }
        public DiscountType Type { get; set; }
        public float DiscountValue { get; set; } 
        public bool IsActive { get; set; }
        public bool Used { get; set; }
    }
}
