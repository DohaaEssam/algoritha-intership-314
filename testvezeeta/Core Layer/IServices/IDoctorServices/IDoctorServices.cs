using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testvezeeta.Core_Layer.Domain;

namespace testvezeeta.Core_Layer.IServices.IDoctorServices
{
    public interface IDoctorServices
    {
        bool Add(int price,Appointment appointment);
        bool Update(Time time);
        bool Delete(int Id);
        List<Patient> GetAll(int DoctorId, string date, int PageSize, int PageNumber);
        bool ConfirmCheckUp(int BookingId);
        public Time GeyById(int Id);

    }
}
