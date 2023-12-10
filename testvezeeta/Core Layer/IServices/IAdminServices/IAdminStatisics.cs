using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testvezeeta.Core_Layer.Domain;

namespace testvezeeta.Core_Layer.IServices.IAdminServices
{
    public interface IAdminStatistics
    {
        int NumOfDoctors();
        int NumOfPatients();
        int NumOfRequests();
        int PendingRequests();
        int CompletedRequests();

        int CanceledRequests();
        List<(Specialization specialization, int totalBookings)> TopFiveSpecilization();
        List<Doctor> TopTenDoctors();

    }
}
