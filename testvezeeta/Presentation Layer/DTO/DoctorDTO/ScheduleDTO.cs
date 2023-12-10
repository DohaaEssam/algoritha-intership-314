using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testvezeeta.Core_Layer.Enums;

namespace testvezeeta.Presentation_Layer.DTO.DoctorDTO
{
    public class ScheduleDTO
    {
        //Object Of (Price & List Of Days(enum) Each Day have List Of Time)
        public int Price { get; set; }

        public List<DayScheduleDTO> Days { get; set; }
    }
}
