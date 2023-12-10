using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using testvezeeta.Core_Layer.Enums;

namespace testvezeeta.Presentation_Layer.DTO.PatientDTO
{
    public class RegisterDTO
    {
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmedPassword { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender Gender { get; set; }
        [Required]
        public DateTime DOB { get; set; }

        [SwaggerParameter("Select an image file.")]
        public string Image { get; set; }
    }
}
