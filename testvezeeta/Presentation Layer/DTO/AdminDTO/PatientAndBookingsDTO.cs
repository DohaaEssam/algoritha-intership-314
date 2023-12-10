using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testvezeeta.Presentation_Layer.DTO.AdminDTO
{
    public class PatientAndBookingsDTO
    {
        public PatientBookingsDTO bookingsDTO { get; set; }
        public PatientsDTO patient { get; set; }
    }
}
