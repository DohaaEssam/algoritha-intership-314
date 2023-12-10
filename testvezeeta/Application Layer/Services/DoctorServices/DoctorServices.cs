using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testvezeeta.Core_Layer.Domain;
using testvezeeta.Core_Layer.Enums;
using testvezeeta.Core_Layer.IRepository;
using testvezeeta.Core_Layer.IServices.IDoctorServices;

namespace testvezeeta.Application_Layer.Services.DoctorServices
{
    public class DoctorServices : IDoctorServices
    {
        private readonly IRepository<Booking> _BookingRepository;
        private readonly IRepository<ApplicationUser> _ApplicationUserRepository;
        private readonly IRepository<Appointment> _AppointmentRepository;
        private readonly IRepository<Doctor> doctorRepository;
        private readonly IRepository<Time> _TimeRepository;


        public DoctorServices(IRepository<Doctor> _DoctorRepository, IRepository<Time> TimeRepository, IRepository<Appointment> AppointmentRepository, IRepository<ApplicationUser> ApplicationUser_Repository, IRepository<Booking> book_repository)
        {

            _BookingRepository = book_repository;
            _ApplicationUserRepository = ApplicationUser_Repository;
            _AppointmentRepository = AppointmentRepository;
            doctorRepository = _DoctorRepository;
            _TimeRepository = TimeRepository;


        }
        public bool Add(int price,Appointment appointment)
        {
            Doctor doctor = doctorRepository.GetById(appointment.Doctor_Id);
            Appointment newAppointment = new Appointment();
            if(doctor != null)
            {
                doctor.Price = price;
                newAppointment.Doctor_Id = appointment.Doctor_Id;
                newAppointment.Day = appointment.Day;
                foreach(var time in appointment.AppointmentTime)
                {
                    Time t = new Time { Booked = false,AvailableTime = time.AvailableTime };
                    _TimeRepository.Add(t);
                }
                _AppointmentRepository.Add(newAppointment);
                doctorRepository.Update(doctor);
                return true;  
            }
            else return false;
        }

        public bool ConfirmCheckUp(int BookingId)
        {
            Booking booking = _BookingRepository.GetAll().SingleOrDefault(b => b.Id == BookingId);
            if (booking != null)
            {
                booking.Status = AppointmentStatus.Completed;
                _BookingRepository.Update(booking);
                return true;
            }
            else return false;
            
        }

        public bool Delete(int id)
        {
            Time deletedTime = _TimeRepository.GetAll().SingleOrDefault(t => t.Id == id && t.Booked == false);
            if (deletedTime != null)
            {
                _TimeRepository.Delete(deletedTime);
                return true;
            }
            else return false;
            
        }

        public List<Patient> GetAll(int DoctorId, string date, int PageSize, int PageNumber)
        {
            int SkipCount = (PageNumber - 1) * PageSize;
            List<Patient> FilteredPatients = new List<Patient>();
            List<Appointment> appointments = _AppointmentRepository.GetAll().ToList();

            List<Booking> bookings = _BookingRepository.GetAll().ToList();
            FilteredPatients = appointments.Where(appointment => appointment.Doctor_Id == DoctorId)
                .Join(bookings,
                appointment => appointment.Id,
                book => book.AppointmentId,(appointment, book) => new { Appointment = appointment, Book = book })
                .Where(joinResult => joinResult.Appointment.Day.ToString().Equals(date, StringComparison.OrdinalIgnoreCase))
                .Select(joinResult => joinResult.Book.Patient)
                .ToList();
            if(FilteredPatients != null)
            {
                if (PageSize >= 0)
                {
                    FilteredPatients = FilteredPatients.Skip(SkipCount).Take(PageSize).ToList();
                }
               
            }

            return FilteredPatients;
        }

        public bool Update(Time time)
        {
            Time UpdatedTime = _TimeRepository.GetById(time.Id);
            if (time.Booked == true)
            {
                return false;
            }
            else
            {
                UpdatedTime.AppointmentId = time.AppointmentId;
                UpdatedTime.AvailableTime = time.AvailableTime;
                UpdatedTime.Booked = false;
                _TimeRepository.Update(UpdatedTime);
            }
            return true;
        }

        public Time GeyById(int Id)
        {
            return _TimeRepository.GetById(Id);
        }
    }
}
