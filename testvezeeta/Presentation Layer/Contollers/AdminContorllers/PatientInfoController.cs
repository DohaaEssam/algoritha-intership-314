using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using testvezeeta.Core_Layer.Domain;
using testvezeeta.Core_Layer.IServices.IAdminServices;
using testvezeeta.Presentation_Layer.DTO.AdminDTO;

namespace testvezeeta.Presentation_Layer.Contollers.AdminContorllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientInfoController : ControllerBase
    {

        private readonly IPatientInfo _PatientInfoServices;
        private readonly IDoctorInfo _DoctorInfoServices;
        private readonly UserManager<ApplicationUser> usermanager;
        public PatientInfoController(IPatientInfo _PatientInfoServices, UserManager<ApplicationUser> usermanager, IDoctorInfo _DoctorInfoServices)
        {
            this._PatientInfoServices = _PatientInfoServices;
            this.usermanager = usermanager;
            this._DoctorInfoServices = _DoctorInfoServices;
        }

        [HttpGet("AllPatients")]
        public IActionResult GetDoctors(string name, int pageNumber = 1, int pageSize = 10)
        {

            var doctorsInRole = usermanager.GetUsersInRoleAsync("Patient").Result;

            if (doctorsInRole != null && doctorsInRole.Any())
            {
                if (!string.IsNullOrEmpty(name))
                {
                    doctorsInRole = doctorsInRole.Where(user =>
                        user.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                        user.LastName.Contains(name, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

                List<PatientsDTO> patients = new List<PatientsDTO>();

                foreach (var user in doctorsInRole)
                {
                    PatientsDTO doctor = new PatientsDTO()
                    {
                        Email = user.Email,
                        FullName = user.FullName,
                        Gender = user.Gender.ToString(),
                        PhoneNumber = user.PhoneNumber,
                        Image = user.Image,
                    };

                    patients.Add(doctor);
                }

                var paginatedDoctors = patients
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(paginatedDoctors);
            }
            else
            {
                return NotFound("No Patients Found");
            }
        }


        [HttpGet("Patient/{id}")]
        public IActionResult GetPatientById(string id)
        {
            var PatientUser = usermanager.FindByIdAsync(id).Result;
            Booking booking = _PatientInfoServices.GetBooking(id);
            Doctor doctor = _PatientInfoServices.GetDoctor(id);
            Patient patient1 = _PatientInfoServices.GetByID(id);
            var DoctorUser = usermanager.FindByIdAsync(doctor.UserId).Result;

            if (PatientUser != null)
            {
                PatientsDTO patient = new PatientsDTO
                {
                    Email = PatientUser.Email,
                    FullName = PatientUser.FullName,
                    Gender = PatientUser.Gender.ToString(),
                    PhoneNumber = PatientUser.PhoneNumber,
                    Image = PatientUser.Image,
                    DOB = PatientUser.DOB
                };
                PatientBookingsDTO patientBooking = new PatientBookingsDTO
                {
                    Day = _PatientInfoServices.GetDay(id),
                    DoctorName = DoctorUser.FullName,
                    DoctorImage = DoctorUser.Image,
                    DoctorSpecialize = _DoctorInfoServices.GetSpecialization(doctor.UserId),
                    Price = doctor.Price,
                    FinalPrice = booking.TotalPrice,
                    DiscoundCode = patient1.DiscountCodeCoupon,
                    Status = booking.Status.ToString(),
                    Time = booking.time,
                };

                PatientAndBookingsDTO patientAndBookings = new PatientAndBookingsDTO
                {
                    patient = patient,
                    bookingsDTO = patientBooking,
                };

                return Ok(patientAndBookings);
            }
            else
            {
                return NotFound($"Patient with ID '{id}' not found.");
            }
        }
    }
}
