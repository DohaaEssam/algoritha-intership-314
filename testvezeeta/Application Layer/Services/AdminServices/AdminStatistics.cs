using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testvezeeta.Core_Layer.Domain;
using testvezeeta.Core_Layer.IRepository;
using testvezeeta.Core_Layer.IServices.IAdminServices;

namespace testvezeeta.Application_Layer.Services.AdminServices
{
    public class AdminStatistics : IAdminStatistics
    {
        private readonly IRepository<Booking> _BookingRepository;
        private readonly IRepository<ApplicationUser> _ApplicationUserRepository;
        private readonly IRepository<Doctor> _DoctorRepository;


        public AdminStatistics(IRepository<Doctor> DoctorRepository, IRepository<ApplicationUser> ApplicationUser_Repository, IRepository<Booking> book_repository)
        {

            _BookingRepository = book_repository;
            _ApplicationUserRepository = ApplicationUser_Repository;
            _DoctorRepository = DoctorRepository;


        }
        public int NumOfDoctors()
        {
            int doctor_num = _ApplicationUserRepository.GetAll().ToList()
               .Where(p => p.AccountType.ToString().Equals("Doctor", StringComparison.Ordinal)).Count();
            return doctor_num;
        }

        public int NumOfPatients()
        {
            int patients_num = _ApplicationUserRepository.GetAll().ToList()
             .Where(p => p.AccountType.ToString().Equals("Patient", StringComparison.Ordinal)).Count();
            return patients_num;
        }

        public int NumOfRequests()
        {
            List<Booking> bookings = _BookingRepository.GetAll().ToList();
            return bookings.Count;
        }

        public int PendingRequests()
        {
            List<Booking> bookings = _BookingRepository.GetAll().Where(b => b.Status.ToString().Equals("Pending", StringComparison.OrdinalIgnoreCase)).ToList();
            return bookings.Count;
        }
        public int CompletedRequests()
        {
            List<Booking> bookings = _BookingRepository.GetAll().Where(b => b.Status.ToString().Equals("Completed", StringComparison.OrdinalIgnoreCase)).ToList();
            return bookings.Count;
        }
        public int CanceledRequests()
        {
            List<Booking> bookings = _BookingRepository.GetAll().Where(b => b.Status.ToString().Equals("Canceled", StringComparison.OrdinalIgnoreCase)).ToList();
            return bookings.Count;
        }

        public List<(Specialization specialization, int totalBookings)> TopFiveSpecilization()
        {
            List<Doctor> doctors = _DoctorRepository.GetAll().ToList();
            var filteredSpecializations = doctors
                .GroupBy(d => d.Specialization)
                .Select(group => new
                {
                    Specialization = group.Key,
                    TotalBookings = group.Sum(d =>
                        d.Appointments.Select(a =>
                            a.Bookings.Count(b =>
                                b.Status.ToString().Equals("Completed", StringComparison.OrdinalIgnoreCase)
                            )
                        ).Sum()
                    )
                })
                .OrderByDescending(x => x.TotalBookings)
                .Take(5)
                .ToList();


            var result = filteredSpecializations.Select(item => (item.Specialization, item.TotalBookings)).ToList();
            return result;
        }

        public List<Doctor> TopTenDoctors()
        {
            List<Doctor> doctors = _DoctorRepository.GetAll().ToList();
            List<Doctor> FilteredDoctors = new List<Doctor>();
            FilteredDoctors = doctors.OrderByDescending(d => d.Appointments.Select(a =>a.Bookings).Count()).Take(10).ToList();
            return FilteredDoctors;
        }
    }
}
