using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MvcMovie.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Models
{
    public class HoaDonModelBinder : IModelBinder
    {
        private const string sessionKey = "HoaDon";
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // get the Cart from the session
            HoaDon cart = null;
            if (bindingContext.HttpContext.Session != null)
            {
                cart = (HoaDon)bindingContext.HttpContext.Session.Get<HoaDon>(sessionKey);
            }
            // create the Cart if there wasn't one in the session data
            if (cart == null)
            {
                cart = new HoaDon();
                cart.NgayLap = DateTime.Now;
                cart.ChiTietHoaDons = new List<ChiTietHoaDon>();
                if (bindingContext.HttpContext.Session != null)
                {
                    bindingContext.HttpContext.Session.Set<HoaDon>(sessionKey, cart);
                }
            }
            // return the cart
            bindingContext.Result = ModelBindingResult.Success(cart);
            return Task.CompletedTask;
        }
    }
}
