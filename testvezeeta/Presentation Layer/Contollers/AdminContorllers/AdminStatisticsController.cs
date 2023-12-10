using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class AdminStatisticsController : ControllerBase
    {

        private readonly IAdminStatistics _AdminStatisticsServices;
        private readonly UserManager<ApplicationUser> usermanager;
        public AdminStatisticsController(IAdminStatistics _AdminStatisticsServices, UserManager<ApplicationUser> usermanager)
        {
            this._AdminStatisticsServices = _AdminStatisticsServices;
            this.usermanager = usermanager;

        }

        [HttpGet("NumOfDoctors")]
        public IActionResult NumOfDoctors()
        {
            var doctorsInRole = usermanager.GetUsersInRoleAsync("Doctor").Result;

            if (doctorsInRole != null && doctorsInRole.Any())
            {
                return Ok(doctorsInRole.Count);
            }
            else return BadRequest(ModelState);
        }

        [HttpGet("NumOfPatients")]
        public IActionResult NumOfPatients()
        {
            var PatientsInRole = usermanager.GetUsersInRoleAsync("Patient").Result;

            if (PatientsInRole != null && PatientsInRole.Any())
            {
                return Ok(PatientsInRole.Count);
            }
            else return BadRequest(ModelState);
        }


        [HttpGet("NumOfRequests")]
        public IActionResult NumOfRequests()
        {

            NumRequestsDTO requests = new NumRequestsDTO
            {
                Requests = _AdminStatisticsServices.NumOfRequests(),
                PendingRequests = _AdminStatisticsServices.PendingRequests(),
                CancelledRequests = _AdminStatisticsServices.CanceledRequests(),
                completedRequests = _AdminStatisticsServices.CompletedRequests(),
            };
            return Ok(requests);
        }

        [HttpGet("Top5Specializations")]
        public IActionResult Top5Specializations()
        {

            List<(Specialization specialization, int totalBookings)> SpecializationsWithBookings
                = _AdminStatisticsServices.TopFiveSpecilization();
            List<TopSpecializationDoctorDTO> doctorsDTO = new List<TopSpecializationDoctorDTO>();
            TopSpecializationDoctorDTO doctorDTO = new TopSpecializationDoctorDTO();

            if (SpecializationsWithBookings != null)
            {
                foreach (var item in SpecializationsWithBookings)
                {
                    doctorDTO.FullName = item.specialization.SpecializationName;
                    doctorDTO.Requests = item.totalBookings;
                    doctorsDTO.Add(doctorDTO);
                }
                return Ok(doctorsDTO);
            }
            else return BadRequest(ModelState);
        }


        [HttpGet("Top10Doctors")]
        public IActionResult Top10Doctors()
        {
            List<Doctor> doctors = _AdminStatisticsServices.TopTenDoctors();
            List<Top10DoctorsDTO> top10DoctorsDTO = new List<Top10DoctorsDTO>();
            Top10DoctorsDTO user = new Top10DoctorsDTO();
            if (doctors != null)
            {
                foreach (var doctor in doctors)
                {
                    user.FullName = doctor.User.FullName;
                    user.Requests = doctor.Appointments.Select(a => a.Bookings.Count).Sum();
                    user.image = doctor.User.Image;
                    user.SpecializaName = doctor.Specialization.SpecializationName;
                    top10DoctorsDTO.Add(user);
                }
                return Ok(top10DoctorsDTO);
            }
            else return BadRequest(ModelState);
        }
    }
}
