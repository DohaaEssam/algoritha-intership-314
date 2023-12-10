using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testvezeeta.Core_Layer.Domain;

namespace testvezeeta.Core_Layer.IServices.IAdminServices
{
    public interface IDoctorInfo
    {
        List<ApplicationUser> GetAll(string search, int PageSize, int PageNumber);

        Doctor GetByID(string Id);

        bool Add(string UserId, int Specialization);

        bool Edit(string Id, int SpecializationId);

        bool Delete(string Id);
        string GetSpecialization(string Id);
    }
}
