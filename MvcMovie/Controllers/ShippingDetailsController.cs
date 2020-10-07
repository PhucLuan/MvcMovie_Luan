using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class ShippingDetailsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Checkout(ShippingDetail detail)
        {
            //var hoaDon = this.Session["HoaDon"] as HoaDon;
            //if (hoaDon.ChiTietHoaDons.Count() == 0)
            //{
            //    ModelState.AddModelError("", "Sorry, your cart is empty!");
            //}
            //if (ModelState.IsValid)
            //{
            //    StringBuilder body = new StringBuilder()
            //    .AppendLine("A new order has been submitted")
            //    .AppendLine("---")
            //    .AppendLine("Items:");
            //    foreach (var hoaDonChiTiet in hoaDon.ChiTietHoaDons)
            //    {
            //        var subtotal = hoaDonChiTiet.MovieObj.Price *
            //        hoaDonChiTiet.SoLuong;
            //        body.AppendFormat("{0} x {1} (subtotal: {2:c}",
            //        hoaDonChiTiet.SoLuong,
            //        hoaDonChiTiet.MovieObj.Title,
            //        subtotal);
            //    }
            //    body.AppendFormat("Total order value: {0:c}", hoaDon.TongTien)
            //    .AppendLine("---")
            //    .AppendLine("Ship to:")
            //    .AppendLine(detail.Name)
            //    .AppendLine(detail.Address)
            //    .AppendLine(detail.Mobile.ToString());
            //    EmailServiceNew.SendEmail(new IdentityMessage()
            //    {
            //        Destination = detail.Email,
            //        Subject = "New order submitted!",
            //        Body = body.ToString()
            //    });
            //    this.Session["HoaDon"] = null;
            //    return View("CheckoutCompleted");
            //}
            //else
            //{
            //    return View(new ShippingDetail());
            //}
            return View();
        }

        public IActionResult CheckoutCompleted()
        {
            return View();
        }
    }

}