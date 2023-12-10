using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testvezeeta.Core_Layer.Domain;

namespace testvezeeta.Core_Layer.IServices.IPatientServices
{
    public interface IPatientServices
    {
        List<Booking> GetAll(int Id);
        bool Register(ApplicationUser patient);
        List<Doctor> GetAllDoctors();
        bool Booking(int PatientId, int TimeId, string discoundCodeCoupon = " ");
        bool Cancel(int BookingId);

        Patient GetById(int Id);

    }
}
