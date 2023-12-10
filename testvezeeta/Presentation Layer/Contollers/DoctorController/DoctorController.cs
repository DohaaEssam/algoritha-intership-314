using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using testvezeeta.Core_Layer.Domain;
using testvezeeta.Core_Layer.IServices.IDoctorServices;
using testvezeeta.Presentation_Layer.DTO.DoctorDTO;

namespace testvezeeta.Presentation_Layer.Contollers.DoctorController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorServices doctorSevices;
        private readonly UserManager<ApplicationUser> usermanager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public DoctorController(IDoctorServices _DoctorSevices, UserManager<ApplicationUser> usermanager
            , SignInManager<ApplicationUser> signInManager)
        {
            doctorSevices = _DoctorSevices;
            this.usermanager = usermanager;
            this.signInManager = signInManager;
        }

        [HttpPost("login/patient")]
        public async Task<IActionResult> Login([FromForm] DoctorLoginDTO loginDto)
        {
            if (ModelState.IsValid)
            {
                var user = await usermanager.FindByEmailAsync(loginDto.Email);

                if (user != null)
                {
                    var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        if (await usermanager.IsInRoleAsync(user, "Doctor"))
                        {
                            await signInManager.SignInAsync(user, isPersistent: false);
                            return Ok(true);
                        }
                        else return BadRequest("You do not have the required role to access this resource");
                        
                    }
                    if (result.IsLockedOut)
                    {
                        return BadRequest("Account locked out due to multiple failed login attempts. Please try again later.");
                    }
                }
                return BadRequest("Invalid login attempt");
            }

            return BadRequest(ModelState);
        }

        [HttpGet("GetAllBookingsOfDoctor")]
        public IActionResult GetAllBookingsOfDoctor(int DoctorId, string date, int PageSize, int PageNumber)
        {
            var Patients = doctorSevices.GetAll(DoctorId,date,PageSize,PageNumber);
            var DoctorBookings = new List<DoctorBookingsDTO>();
            if (Patients != null)
            {
                foreach (var patient in Patients)
                {
                    var doctorBook = new DoctorBookingsDTO
                    {
                        Email = patient.User.Email,
                        Image = patient.User.Image,
                        PatientName = patient.User.FullName,
                        Gender = patient.User.Gender.ToString(),
                        age = DateTime.Now.Year - patient.User.DOB.Year,
                        Phone = patient.User.PhoneNumber,
                        Appointment = patient.Bookings.Where(p => p.PatientId == patient.Id).Select(p => p.time).FirstOrDefault(),
                    };
                    DoctorBookings.Add(doctorBook);
                }
                return Ok(DoctorBookings);
            }
            else return BadRequest(ModelState);
        }

        [HttpPut("ConfirmCheckUp/{bookingId}")]
        public IActionResult ConfirmCheckUp(int bookingId)
        {
            if (doctorSevices.ConfirmCheckUp(bookingId))
            {
                return Ok(true);
            }
            else return BadRequest(ModelState);
        }

        [HttpDelete("DeleteTime/{AppiontmentId}")]
        public IActionResult Delete(int id)
        {
            if (doctorSevices.Delete(id))
            {
                return Ok(true);
            } else return BadRequest(ModelState);
        }

        [HttpPut("UpdateTime/{id}")]
        public IActionResult Update(int id)
        {
            Time time = doctorSevices.GeyById(id);
            if (doctorSevices.Update(time))
            {
                return Ok(true);
            }
            else return BadRequest(ModelState);
        }

        [HttpPost("AddTime/{DoctorId}")]
        public IActionResult Add([FromBody] ScheduleDTO scheduleDTO, int DoctorId)
        {
            if (ModelState.IsValid)
            {
                foreach (var day in scheduleDTO.Days)
                {
                    var appointment = new Appointment
                    {
                        Doctor_Id = DoctorId,
                        Day = day.day,
                        AppointmentTime = day.times.Select(t => new Time
                        {
                            AvailableTime = t.dateTime,
                            Booked = false
                        }).ToList() 
                    };

                    doctorSevices.Add(scheduleDTO.Price, appointment);
                }
                return Ok(true);
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

    }
}
