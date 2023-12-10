using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testvezeeta.Presentation_Layer.DTO.AdminDTO
{
    public class PatientBookingsDTO
    {
        //requests:[{image,doctorName,specialize,day,time,price,discoundCode,finalPrice,status}]]
        public string DoctorImage { get; set; }
        public string DoctorName { get; set; }
        public string DoctorSpecialize { get; set; }
        public string Day { get; set; }
        public DateTime Time { get; set; }
        public int Price { get; set; }
        public string DiscoundCode { get; set; }
        public float FinalPrice { get; set; }
        public string Status { get; set; }

    }
}
