using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testvezeeta.Core_Layer.Enums;
using testvezeeta.Core_Layer.IServices.IAdminServices;

namespace testvezeeta.Presentation_Layer.Contollers.AdminContorllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountCodeSettingController : ControllerBase
    {
        private readonly IDiscountCodeSetting _DiscountCodeSettingServices;

        public DiscountCodeSettingController(IDiscountCodeSetting _DiscountCodeSettingServices)
        {
            this._DiscountCodeSettingServices = _DiscountCodeSettingServices;
        }

        [HttpPost("AddDiscountCode")]
        public IActionResult Add(string DiscountCodeCoupon, int requests, DiscountType type, int value)
        {
            if (_DiscountCodeSettingServices.Add(DiscountCodeCoupon, requests, type, value))
            {
                return Ok(true);
            }
            else return BadRequest(ModelState);
        }

        [HttpPut("UpdateDiscountCode/{id}")]
        public IActionResult Update(int id ,string DiscountCodeCoupon, int requests, DiscountType type, int value)
        {
            if (_DiscountCodeSettingServices.Update(id,DiscountCodeCoupon, requests, type, value))
            {
                return Ok(true);
            }
            else return BadRequest(ModelState);
        }

        [HttpPut("DeactivateDiscountCode/{id}")]
        public IActionResult Deactivate(int id)
        {
            if (_DiscountCodeSettingServices.Deactivate(id))
            {
                return Ok(true);
            }
            else return BadRequest(ModelState);
        }

        [HttpDelete("DeleteDiscountCode/{id}")]
        public IActionResult Delete(int id)
        {
            if (_DiscountCodeSettingServices.Delete(id))
            {
                return Ok(true);
            }
            else return BadRequest(ModelState);
        }
    }
}
