using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using testvezeeta.Core_Layer.Enums;

namespace testvezeeta.Core_Layer.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public override string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public string FullName { get {
                return $"{FirstName} {LastName}";
            } }
        public Gender Gender { get; set; }
        public DateTime DOB { get; set; }
        public string Image { get; set; }
        public AccountType AccountType { get; set; }
        public virtual List<Doctor> Doctors { get; set; }
        public virtual List<Patient> Patients { get; set; }

    }
}
