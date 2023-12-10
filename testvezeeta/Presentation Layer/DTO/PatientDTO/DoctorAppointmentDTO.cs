using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testvezeeta.Presentation_Layer.DTO.PatientDTO
{
    public class DoctorAppointmentDTO
    {
        public string Day { get; set; }
        public List<DoctorTimeDTO> Times { get; set; }
    }
}
