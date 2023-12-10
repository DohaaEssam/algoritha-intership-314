using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testvezeeta.Presentation_Layer.DTO.PatientDTO
{
    public class AllPatientBookingsDTO
    {
        //[{image,doctorName,specialize,day,time,price,discoundCode,finalPrice,status

        public string Image { get; set; }
        public string DoctorName { get; set; }
        public string specialize { get; set; }
        public string day { get; set; }
        public string discoundCode { get; set; }
        public string status { get; set; }
        public int Price { get; set; }
        public float FinalPrice { get; set; }
        public DateTime time { get; set; }

    }
}
