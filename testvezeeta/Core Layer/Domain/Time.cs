using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace testvezeeta.Core_Layer.Domain
{
    public class Time
    {
        public int Id { get; set; }
        public DateTime AvailableTime { get; set; }
        public bool Booked { get; set; }

        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; }

    }
}
