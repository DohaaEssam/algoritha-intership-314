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
    [Route("api/admin/[controller]")]
    [ApiController]
    public class DoctorInfoController : ControllerBase
    {
        private readonly IDoctorInfo _DoctorInfoServices;
        private readonly UserManager<ApplicationUser> usermanager;
        public DoctorInfoController(IDoctorInfo _DoctorInfoServices, UserManager<ApplicationUser> usermanager )
        {
            this._DoctorInfoServices = _DoctorInfoServices;
            this.usermanager = usermanager;

        }

        [HttpGet("doctors/{id}")]
        public IActionResult GetDoctorById(string id)
        {
            var doctorUser = usermanager.FindByIdAsync(id).Result;

            if (doctorUser != null)
            {
                DoctorsDTO doctor = new DoctorsDTO
                {
                    Email = doctorUser.Email,
                    FullName = doctorUser.FullName,
                    Gender = doctorUser.Gender.ToString(),
                    PhoneNumber = doctorUser.PhoneNumber,
                    Image = doctorUser.Image,
                    Specialization = _DoctorInfoServices.GetSpecialization(doctorUser.Id)
                };

                return Ok(doctor);
            }
            else
            {
                return NotFound($"Doctor with ID '{id}' not found.");
            }
        }

        [HttpGet("ALLDoctors")]
        public IActionResult GetDoctors(string name, int pageNumber = 1, int pageSize = 10)
        {
            
            var doctorsInRole = usermanager.GetUsersInRoleAsync("Doctor").Result;
            
            if (doctorsInRole != null && doctorsInRole.Any())
            {
                if (!string.IsNullOrEmpty(name))
                {
                    doctorsInRole = doctorsInRole.Where(user =>
                        user.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                        user.LastName.Contains(name, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

                List<DoctorsDTO> doctors = new List<DoctorsDTO>();

                foreach (var user in doctorsInRole)
                {
                    DoctorsDTO doctor = new DoctorsDTO
                    {
                        Email = user.Email,
                        FullName = user.FullName,
                        Gender = user.Gender.ToString(),
                        PhoneNumber = user.PhoneNumber,
                        Image = user.Image,
                        Specialization = _DoctorInfoServices.GetSpecialization(user.Id)
                    };

                    doctors.Add(doctor);
                }

                var paginatedDoctors = doctors
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


        [HttpPost("add/doctor")]
        public async Task<IActionResult> Add([FromForm] AddDoctorDTO userDto)
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
                user.AccountType = Core_Layer.Enums.AccountType.Doctor;
                IdentityResult result = await usermanager.CreateAsync(user, userDto.Password);
                string errors = " ";
                foreach (var error in result.Errors)
                {
                    errors += " " + error.Description;
                }
                if (result.Succeeded)
                {
                    if (_DoctorInfoServices.Add(user.Id,userDto.SpecializationId))
                    {
                        await usermanager.AddToRoleAsync(user, "Doctor");
                        return Ok(true);
                    }
                }
                return BadRequest(errors);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("update/doctor/{id}")]
        public async Task<IActionResult> Update(string id, [FromForm] AddDoctorDTO doctorDto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await usermanager.FindByIdAsync(id);

                if (user == null)
                {
                    return NotFound(); 
                }

                user.FirstName = doctorDto.FirstName;
                user.LastName = doctorDto.LastName;
                user.PasswordHash = doctorDto.Password;
                user.PhoneNumber = doctorDto.PhoneNumber;
                user.Gender = doctorDto.Gender;
                user.DOB = doctorDto.DOB;
                user.Image = doctorDto.Image;
                user.Email = doctorDto.Email;

                IdentityResult result = await usermanager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    if (_DoctorInfoServices.Edit(user.Id, doctorDto.SpecializationId))
                    {
                        return Ok(true);
                    }
                }

                string errors = string.Join(" ", result.Errors.Select(error => error.Description));
                return BadRequest(errors); 
            }

            return BadRequest(ModelState); 
        }


        [HttpDelete("delete/doctor/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            ApplicationUser user = await usermanager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(); 
            }

            IdentityResult result = await usermanager.DeleteAsync(user);

            if (result.Succeeded)
            {

                if (_DoctorInfoServices.Delete(id))
                {
                    return Ok(true);
                } 
            }

            string errors = string.Join(" ", result.Errors.Select(error => error.Description));
            return BadRequest(errors);
        }
    }
}
