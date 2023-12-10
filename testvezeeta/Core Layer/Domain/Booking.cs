using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using testvezeeta.Core_Layer.Enums;

namespace testvezeeta.Core_Layer.Domain
{
    public class Booking
    {
        public int Id { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public AppointmentStatus Status { get; set; }
        public float TotalPrice { get; set; }

        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; }
        public DateTime time { get; set; }

    }
}
