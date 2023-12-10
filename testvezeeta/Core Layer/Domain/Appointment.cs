using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using testvezeeta.Core_Layer.Enums;

namespace testvezeeta.Core_Layer.Domain
{
    public class Appointment
    {
        public int Id { get; set; }
        public Days Day { get; set; }

        public List<Time> AppointmentTime;

        [ForeignKey("Doctor")]
        public int Doctor_Id { get; set; }
        public virtual Doctor Doctor { get; set;}
        public virtual List<Booking> Bookings { get; set; }
    }
}
