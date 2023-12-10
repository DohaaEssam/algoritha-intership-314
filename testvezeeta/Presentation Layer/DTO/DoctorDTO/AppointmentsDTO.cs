using System.Collections.Generic;

namespace testvezeeta.Presentation_Layer.DTO.DoctorDTO
{
    public class AppointmentsDTO
    {
        public string Day { get; set; }
        public List<TimeDTO> Time { get; set; }
    }
}