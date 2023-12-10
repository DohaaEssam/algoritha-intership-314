using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using testvezeeta.Core_Layer.Domain;
using testvezeeta.Core_Layer.IServices.IPatientServices;
using testvezeeta.Presentation_Layer.DTO.PatientDTO;

namespace testvezeeta.Presentation_Layer.Contollers.PatientContoller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientAccountController : ControllerBase
    {
        private readonly IPatientServices _PatientServices;
        private readonly UserManager<ApplicationUser> usermanager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public PatientAccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> usermanger, IPatientServices patientServices)
        {
            this.usermanager = usermanger;
            _PatientServices = patientServices;
            this.signInManager = signInManager;
        }

        [HttpPost("register/patient")]
        public async Task<IActionResult> Registration([FromForm] RegisterDTO userDto)
        {
            if (ModelState.IsValid)
            {
                //save
                ApplicationUser user = new ApplicationUser();
                user.UserName = userDto.FirstName + user.LastName + userDto.Email;
                user.FirstName = userDto.FirstName;
                user.LastName = userDto.LastName;
                user.PasswordHash = userDto.Password;
                user.PhoneNumber = userDto.PhoneNumber;
                user.Gender = userDto.Gender;
                user.DOB = userDto.DOB;
                user.Image = userDto.Image;
                user.Email = userDto.Email;
                user.AccountType = Core_Layer.Enums.AccountType.Patient;
                IdentityResult result = await usermanager.CreateAsync(user, userDto.Password);
                string errors = " ";
                foreach (var error in result.Errors)
                {
                    errors += " " + error.Description;
                }
                if (result.Succeeded)
                {
                    if (_PatientServices.Register(user))
                    {
                        await usermanager.AddToRoleAsync(user, "Patient");
                        return Ok(true);
                    }
                }
                return BadRequest(errors);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("login/patient")]
        public async Task<IActionResult> Login([FromForm] PatientLoginDTO loginDto)
        {
            if (ModelState.IsValid)
            {
                var user = await usermanager.FindByEmailAsync(loginDto.Email);

                if (user != null)
                {
                    var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        if (await usermanager.IsInRoleAsync(user, "Patient"))
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

        [HttpDelete("CancelBooking/{id}")]
        public IActionResult CancelBooking(int id)
        {
            if (_PatientServices.Cancel(id))
                return Ok(true);
            else return BadRequest(ModelState);
        }

        [HttpGet("GetAllDoctors")]
        public async Task<IActionResult> GetAllDoctorsAsync(string search, int pageNumber = 1, int pageSize = 10)
        {

            var doctorsInRole = usermanager.GetUsersInRoleAsync("Doctor").Result;

            if (doctorsInRole != null && doctorsInRole.Any())
            {
                var DoctorsDTO = new List<AllDoctorInfoDTO>();
                var RetrievedDoctors = _PatientServices.GetAllDoctors();

                foreach (var doctor in RetrievedDoctors)
                {
                    var user = await usermanager.FindByIdAsync(doctor.UserId);

                    var doctorDTO = new AllDoctorInfoDTO()
                    {
                        FullName = user.FullName,
                        Image = user.Image,
                        Gender = user.Gender.ToString(),
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email,
                        Price = doctor.Price,
                        SpecializaName = doctor.Specialization.SpecializationName,
                        doctorAppointments = (List<DoctorAppointmentDTO>)doctor.Appointments.Select(appointment => new DoctorAppointmentDTO
                        {
                            Day = appointment.Day.ToString(),
                            Times = (List<DoctorTimeDTO>)appointment.AppointmentTime.Select(time => new DoctorTimeDTO
                            {
                                Id = time.Id,
                                Time = time.AvailableTime
                            })
                        })
                    };

                    DoctorsDTO.Add(doctorDTO);
                }

                var paginatedDoctors = DoctorsDTO.Where(d => d.FullName.ToLower().Contains(search.ToLower()))
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(paginatedDoctors);
            }
            else
            {
                return NotFound("No doctors found in the specified role.");
            }
        }

        [HttpGet("GetAllBookings/{id}")]

        public IActionResult GetAllBookings(int Id)
        {
            var AllBookings = _PatientServices.GetAll(Id);
            var BookingsInfo = new List<AllPatientBookingsDTO>();
            if (AllBookings != null)
            {
                foreach (var book in AllBookings)
                {
                    var bookingInfo = new AllPatientBookingsDTO
                    {
                        Image = book.Appointment.Doctor.User.Image,
                        DoctorName = book.Appointment.Doctor.User.FullName,
                        specialize = book.Appointment.Doctor.Specialization.SpecializationName,
                        day = book.Appointment.Day.ToString(),
                        Price = book.Appointment.Doctor.Price,
                        FinalPrice = book.TotalPrice,
                        discoundCode = book.Patient.DiscountCodeCoupon,
                        status = book.Status.ToString(),
                        time = book.time,
                    };

                    BookingsInfo.Add(bookingInfo);
                }
                return Ok(AllBookings);
            }
            else return BadRequest(ModelState);
        }

        [HttpPost("Patient/{PatientId}/Booking/{TimeId}")]
        public IActionResult Booking(int PatientId, int timeId, string discoundCodeCoupon = " ")
        {
            if (_PatientServices.Booking(PatientId, timeId, discoundCodeCoupon))
            {
                return Ok(true);
            }
            else return BadRequest(ModelState);
        }

    }
}
