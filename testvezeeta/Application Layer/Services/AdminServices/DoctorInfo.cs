using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testvezeeta.Core_Layer.Domain;
using testvezeeta.Core_Layer.Enums;
using testvezeeta.Core_Layer.IRepository;
using testvezeeta.Core_Layer.IServices.IAdminServices;

namespace testvezeeta.Application_Layer.Services.AdminServices
{
    public class DoctorInfo : IDoctorInfo
    {
        private readonly IRepository<ApplicationUser> _ApplicationUserRepository;
        private readonly IRepository<Specialization> _SpecializationRepository;
        private readonly IRepository<Doctor> _DoctorRepository;
        private readonly IRepository<Appointment> _AppointmentRepository;
        public DoctorInfo(IRepository<Specialization> _SpecializationRepository, IRepository<Appointment> AppointmentRepository, IRepository<ApplicationUser> ApplicationUserRepository, IRepository<Doctor> DoctorRepository)
        {
            _ApplicationUserRepository = ApplicationUserRepository;
            _DoctorRepository = DoctorRepository;
            _AppointmentRepository = AppointmentRepository;
            this._SpecializationRepository = _SpecializationRepository;
        }
        public bool Add(string UserId, int Specialization)
        {
            Doctor _doctor = new Doctor();
            _doctor.UserId = UserId;
            _doctor.SpecializationId = Specialization;
            _DoctorRepository.Add(_doctor);
            return true;
        }

        public bool Delete(string Id)
        {
            Doctor doctor = (Doctor)_DoctorRepository.GetAll().Where(d => d.Appointments.Select(a=>a.Bookings.Count() == 0 && d.UserId == Id).FirstOrDefault());
            if (doctor != null)
            {
                var appointments = _AppointmentRepository.GetAll().Where(a => a.Doctor_Id == doctor.Id).ToList();
                _AppointmentRepository.DeleteMany(appointments);
                _DoctorRepository.Delete(doctor);
                return true;
            }
            else return false;
        }

        public bool Edit(string Id, int SpecializationId)
        {
            Doctor _doctor = _DoctorRepository.GetAll().FirstOrDefault(d => d.UserId == Id);
            if (_doctor != null)
            {
                _doctor.SpecializationId = SpecializationId;
                _DoctorRepository.Update(_doctor);
                return true;
            }
            else return false;
        }

        public List<ApplicationUser> GetAll(string search, int pageSize, int page)
        {
            List<ApplicationUser> doctors = _ApplicationUserRepository.GetAll(page, pageSize,
                d => d.AccountType == AccountType.Doctor &&
                  (d.FirstName.ToLower().Contains(search.ToLower()) ||
                   d.LastName.ToLower().Contains(search.ToLower()) ||
                   d.FullName.ToLower().Contains(search.ToLower())));
            return doctors;
        }

        public Doctor GetByID(string Id)
        {
            Doctor doctor = _DoctorRepository.GetAll().FirstOrDefault(d => d.UserId == Id);
            return doctor;
        }

        public string GetSpecialization(string Id)
        {
            Doctor doctor = _DoctorRepository.GetAll().FirstOrDefault(d => d.UserId == Id);
            Specialization specialize =_SpecializationRepository.GetById(doctor.SpecializationId);
            return specialize.SpecializationName;
        }
    }
}
