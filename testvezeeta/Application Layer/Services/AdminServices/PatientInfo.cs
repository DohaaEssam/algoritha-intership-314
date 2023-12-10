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
    public class PatientInfo : IPatientInfo
    {
        private readonly IRepository<Doctor> _DoctorRepository;
        private readonly IRepository<Patient> _PatientRepository;
        private readonly IRepository<Booking> _BookingRepository;
        private readonly IRepository<Appointment> _AppointmentRepository;
        public PatientInfo( IRepository<Patient> _PatientRepository,
            IRepository<Doctor> _DoctorRepository, IRepository<Booking> _BookingRepository, IRepository<Appointment> _AppointmentRepository)
        {
            this._AppointmentRepository = _AppointmentRepository;
            this._BookingRepository = _BookingRepository;
            this._DoctorRepository = _DoctorRepository;
            this._PatientRepository = _PatientRepository;
        }

        public Patient GetByID(string Id)
        {
            Patient patient = _PatientRepository.GetAll().FirstOrDefault(p=>p.UserId == Id);
            return patient;
        }
        public Booking GetBooking(string Id)
        {
            Patient patient = GetByID(Id);
            Booking booking = _BookingRepository.GetAll().FirstOrDefault(b => b.PatientId == patient.Id);
            return booking;
        }

        public string GetDay(string Id)
        {
            Patient patient = GetByID(Id);
            Appointment appointment = _AppointmentRepository.GetAll().FirstOrDefault(a => a.Bookings.Any(p => p.PatientId== patient.Id));
            return appointment.Day.ToString();
        }
        public Doctor GetDoctor(string Id)
        {
            Patient patient = GetByID(Id);
            Appointment appointment = _AppointmentRepository.GetAll().FirstOrDefault(a => a.Bookings.Any(p => p.PatientId == patient.Id));
            Doctor doctor = _DoctorRepository.GetAll().FirstOrDefault(d => d.Appointments.Any(a =>a.Id == appointment.Id));
            return doctor;
        }

    }
}
