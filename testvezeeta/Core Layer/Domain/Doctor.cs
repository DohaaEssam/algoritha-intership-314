using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace testvezeeta.Core_Layer.Domain
{
    public class Doctor
    {

        public int Id { get; set; }

        public int Price { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("specialization")]
        public int SpecializationId { get; set; }
        public virtual Specialization Specialization { get; set; }
        public virtual List<Appointment> Appointments { get; set; }

    }
}
