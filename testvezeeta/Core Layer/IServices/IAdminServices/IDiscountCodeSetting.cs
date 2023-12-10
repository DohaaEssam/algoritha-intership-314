using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testvezeeta.Core_Layer.Enums;

namespace testvezeeta.Core_Layer.IServices.IAdminServices
{
    public interface IDiscountCodeSetting
    {
        bool Add(string DiscountCodeCoupon, int requests, DiscountType type, int value);
        bool Update(int Id, string DiscountCodeCoupon, int requests, DiscountType type, int value);
        bool Delete(int Id);
        bool Deactivate(int Id);
    }
}
