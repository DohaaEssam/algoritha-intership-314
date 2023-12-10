using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace testvezeeta.Core_Layer.Domain
{
    public class Specialization
    {
        public int Id { get; set; }
        public string SpecializationName { get; set; }
        public virtual List<Doctor> Doctors { get; set; }

    }
}
