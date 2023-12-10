using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testvezeeta.Presentation_Layer.DTO.PatientDTO
{
    public class AllDoctorInfoDTO
    {
        //[{image,fullName,email,phone,specialize,price,gender,appointments:[{day,times:[{id,time}]}]}]

        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Image { get; set; }
        public string SpecializaName { get; set; }
        public int Price { get; set;}
        public List<DoctorAppointmentDTO> doctorAppointments { get; set; }

    }
}
