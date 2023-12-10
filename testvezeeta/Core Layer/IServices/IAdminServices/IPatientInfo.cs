using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testvezeeta.Core_Layer.Domain;

namespace testvezeeta.Core_Layer.IServices.IAdminServices
{
    public interface IPatientInfo
    {
        Patient GetByID(string Id);

        Booking GetBooking(string Id);

        string GetDay(string Id);

        Doctor GetDoctor(string Id);


    }
}
