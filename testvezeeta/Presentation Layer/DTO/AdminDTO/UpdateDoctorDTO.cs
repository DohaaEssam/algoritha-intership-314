using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using testvezeeta.Core_Layer.Enums;

namespace testvezeeta.Presentation_Layer.DTO.AdminDTO
{
    public class UpdateDoctorDTO
    {
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmedPassword { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int SpecializationId { get; set; }
        public string PhoneNumber { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender Gender { get; set; }
        [Required]
        public DateTime DOB { get; set; }

        [SwaggerParameter("Select an image file.")]
        public string Image { get; set; }
    }
}
