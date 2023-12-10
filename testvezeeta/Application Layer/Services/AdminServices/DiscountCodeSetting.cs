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
    public class DiscountCodeSetting : IDiscountCodeSetting
    {
        private readonly IRepository<DiscountCode> _DiscountCodeRepository;
        public DiscountCodeSetting(IRepository<DiscountCode> DiscountCodeRepository)
        {
            _DiscountCodeRepository = DiscountCodeRepository;
        }
        public bool Add(string DiscountCodeCoupon, int requests, DiscountType type, int value)
        {
            DiscountCode newDiscount = new DiscountCode();
            if (DiscountCodeCoupon != null && requests > 0 && value > 0)
            {
                newDiscount.DiscountCodeCoupon = DiscountCodeCoupon;
                newDiscount.DiscountValue = value;
                newDiscount.IsActive = true;
                newDiscount.Type = type;
                newDiscount.RequestNum = requests;
                _DiscountCodeRepository.Add(newDiscount);
                newDiscount.Used = false;
                return true;
            }
            else return false;
        }

        public bool Deactivate(int Id)
        {
            DiscountCode discountCode = _DiscountCodeRepository.GetById(Id);
            if (discountCode != null)
            {
                discountCode.IsActive = false;
                _DiscountCodeRepository.Update(discountCode);
                return true;
            }
            else return false;
        }

        public bool Delete(int Id)
        {
            DiscountCode discountCode = _DiscountCodeRepository.GetById(Id);
            if (discountCode != null)
            {
                _DiscountCodeRepository.Delete(discountCode);
                return true;
            }
            else return false;
        }

        public bool Update(int Id, string DiscountCodeCoupon, int requests, DiscountType type, int value)
        {
            DiscountCode discountCode = _DiscountCodeRepository.GetById(Id);
            if (discountCode.Used == false)
            {
                if (DiscountCodeCoupon != null && requests > 0 && value > 0)
                {
                    discountCode.DiscountCodeCoupon = DiscountCodeCoupon;
                    discountCode.DiscountValue = value;
                    discountCode.Type = type;
                    discountCode.RequestNum = requests;
                    _DiscountCodeRepository.Update(discountCode);
                    return true;
                }
                else return false;
            }
            else return false;
        }
    }
}
