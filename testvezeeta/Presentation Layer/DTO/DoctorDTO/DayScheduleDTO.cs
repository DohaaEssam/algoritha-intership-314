using System.Collections.Generic;
using testvezeeta.Core_Layer.Enums;

namespace testvezeeta.Presentation_Layer.DTO.DoctorDTO
{
    public class DayScheduleDTO
    {
        public Days day { get; set; }
       public List<TimeDTO> times { get; set; }
    }
}