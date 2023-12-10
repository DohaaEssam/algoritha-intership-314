using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testvezeeta.Core_Layer.Domain;
using testvezeeta.Core_Layer.Enums;
using testvezeeta.Core_Layer.IRepository;
using testvezeeta.Core_Layer.IServices.IPatientServices;
using testvezeeta.Infrastructure_Layer.DbContext;
using Microsoft.EntityFrameworkCore;

namespace testvezeeta.Application_Layer.Services.PatientServices
{
    public class PatientServices : IPatientServices
    {
        private readonly IRepository<Booking> _BookingRepository;
        private readonly IRepository<ApplicationUser> _ApplicationUserRepository;
        private readonly IRepository<Appointment> _AppointmentRepository;
        private readonly IRepository<Time> _TimeRepository;
        private readonly IRepository<Doctor> _DoctorRepository;
        private readonly IRepository<DiscountCode> _DiscountCodeRepository;
        private readonly IRepository<Patient> _PatientRepository;
        private readonly DbEntities _context;


        public PatientServices(DbEntities _context ,IRepository<Patient> PatientRepository, IRepository<DiscountCode> DiscountCodeRepository, IRepository<Doctor> DoctorRepository, IRepository<Time> TimeRepository, IRepository<Appointment> AppointmentRepository, IRepository<ApplicationUser> ApplicationUser_Repository, IRepository<Booking> book_repository)
        {

            _BookingRepository = book_repository;
            _ApplicationUserRepository = ApplicationUser_Repository;
            _AppointmentRepository = AppointmentRepository;
            _TimeRepository = TimeRepository;
            _DoctorRepository = DoctorRepository;
            _DiscountCodeRepository = DiscountCodeRepository;
            _PatientRepository = PatientRepository;
            this._context = _context;

        }
        public bool Booking(int PatientId, int TimeId, string discoundCodeCoupon = " ")
        {

            Booking booking = new Booking();

            Patient patient = _PatientRepository.GetById(PatientId);

            List<Appointment> appointments = _AppointmentRepository.GetAll().ToList();
            Time time = _TimeRepository.GetById(TimeId);

            if (time == null || time.AppointmentId == null)
            {
                return false;
            }

            int appointmentId = time.AppointmentId;

            Appointment appointment = appointments.FirstOrDefault(a => a.Id == appointmentId);
            if (appointment == null)
            {
                return false;
            }

            Doctor doctor = _DoctorRepository.GetById(appointment.Doctor_Id);
            List<Time> appointmentTimes = appointment.AppointmentTime.ToList();

            Time selectedTime = appointmentTimes.FirstOrDefault(t => t.Id == TimeId && !t.Booked);
            if (selectedTime == null)
            {
                return false;
            }

            selectedTime.Booked = true;
            booking.AppointmentId = appointmentId;
            booking.Status = AppointmentStatus.Pending;
            booking.PatientId = PatientId;
            booking.time = selectedTime.AvailableTime;

            if (patient.DiscountCodeCoupon == discoundCodeCoupon)
            {
                DiscountCode discount = _DiscountCodeRepository.GetAll().FirstOrDefault(d => d.DiscountCodeCoupon.Equals(discoundCodeCoupon, StringComparison.OrdinalIgnoreCase));

                if (discount != null && discount.Used == false)
                {
                    if (discount.Type == DiscountType.Percentage)
                    {
                        booking.TotalPrice = doctor.Price - (doctor.Price * discount.DiscountValue);
                        discount.Used = true;

                    }
                    else if (discount.Type == DiscountType.Value && discount.Used == false)
                    {
                        booking.TotalPrice = doctor.Price - discount.DiscountValue;
                        discount.Used = true;
                    }
                }
                else {
                    booking.TotalPrice = doctor.Price;
                }
            }
            else
            {
                booking.TotalPrice = doctor.Price;
            }

            _BookingRepository.Add(booking);

            return true;
        }

        public bool Cancel(int BookingId)
        {
            Booking OldBooking = _BookingRepository.GetById(BookingId);
            int SelectedTimeId = OldBooking.Appointment.AppointmentTime.Select(p => p.Id).FirstOrDefault();
            Time SelectedTime = _TimeRepository.GetById(SelectedTimeId);
            SelectedTime.Booked = false;
            if (OldBooking != null)
            {
                OldBooking.Status = AppointmentStatus.Canceled;
                _BookingRepository.Update(OldBooking);
                _TimeRepository.Update(SelectedTime);
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Booking> GetAll(int Id)
        {
            List<Booking> AllBookings = (List<Booking>)_BookingRepository.GetAll().Where(b => b.PatientId == Id);
            return (AllBookings);
        }

        public List<Doctor> GetAllDoctors()
        {
            
            return _DoctorRepository.GetAll();
            //List<ApplicationUser> doctors = _ApplicationUserRepository.GetAll(page, pageSize,
            //    d => d.AccountType == AccountType.Doctor &&
            //      (d.FirstName.ToLower().Contains(search.ToLower()) ||
            //       d.LastName.ToLower().Contains(search.ToLower()) ||
            //       d.FullName.ToLower().Contains(search.ToLower())));
            //return doctors;
            //List<ApplicationUser> allDoctors = _ApplicationUserRepository
            //    .GetAll()
            //    .Where(d => d.AccountType.ToString().Equals("Doctor", StringComparison.OrdinalIgnoreCase))
            //    .ToList();

            //int skipCount = (page - 1) * pageSize;

            //if (pageSize >= 0 && !string.IsNullOrWhiteSpace(search))
            //{
            //    return allDoctors
            //        .Where(d => d.FirstName.ToLower().Contains(search.ToLower()) 
            //        || d.LastName.ToLower().Contains(search.ToLower()) 
            //        || d.FullName.ToLower().Contains(search.ToLower()))
            //        .Skip(skipCount)
            //        .Take(pageSize)
            //        .ToList();
            //}
            //else if (pageSize >= 0)
            //{
            //    return allDoctors
            //        .Skip(skipCount)
            //        .Take(pageSize)
            //        .ToList();
            //}
            //else if (!string.IsNullOrWhiteSpace(search))
            //{
            //    return allDoctors
            //        .Where(d => d.FirstName.ToLower().Contains(search.ToLower())
            //        || d.LastName.ToLower().Contains(search.ToLower())
            //        || d.FullName.ToLower().Contains(search.ToLower()))
            //        .ToList();
            //}
            //else
            //{
            //    return allDoctors;
            //}
        }

        public bool Register(ApplicationUser user)
        {
            Patient patient = new Patient();
            if (user != null)
            {
                patient.UserId = user.Id;
                _PatientRepository.Add(patient);
                return true;
            }
            else return false;
           
        }

        public Patient GetById(int Id)
        {
            return _PatientRepository.GetById(Id);
        }

    }
}
