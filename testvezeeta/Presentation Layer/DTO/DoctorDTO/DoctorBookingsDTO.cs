using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testvezeeta.Presentation_Layer.DTO.DoctorDTO
{
    public class DoctorBookingsDTO
    {
        //List Of(PatientName/Image/age/Gender/Phone/Email/Appointment)
        public string PatientName { get; set; }
        public string Image { get; set; }
        public int age { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime Appointment { get; set; }
    }
}
