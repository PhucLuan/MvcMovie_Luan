using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using MvcMovie.Data;
using MvcMovie.Models;
using Newtonsoft.Json;

namespace MvcMovie.Controllers
{
    public class HoaDonsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HoaDonsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult AddToCart(int id)
        {
            //Kiem tra Id movie ton tai hay khong
            var movie = _context.Movies.Where(x => x.Id == id).FirstOrDefault();
            if (movie == null)
            {
                return RedirectToAction("Index");
            }
            HttpContext.Session.SetString("Test", "TestValue");

            HoaDon hoaDon; 
            //hoaDon = this.Session["HoaDon"] as HoaDon;

            hoaDon = JsonConvert.DeserializeObject<HoaDon>(HttpContext.Session.GetString("SessionHoaDon"));
            if (hoaDon == null)
            {
                hoaDon = new HoaDon();
                hoaDon.NgayLap = DateTime.Now;
                hoaDon.ChiTietHoaDons = new List<ChiTietHoaDon>();
                hoaDon = JsonConvert.DeserializeObject<HoaDon>(HttpContext.Session.GetString("SessionHoaDon"));
                //this.Session["HoaDon"] = hoaDon;
                _context.HoaDons.Add(hoaDon);
            }
            //Kiem tra don hang da co truoc do
            var chiTietHoaDon =
            hoaDon.ChiTietHoaDons.Where(x =>
            x.MovieObj.Id == id).FirstOrDefault();
            if (chiTietHoaDon == null)
            {
                chiTietHoaDon = new
                ChiTietHoaDon();
                chiTietHoaDon.MaMovie = id;
                chiTietHoaDon.MovieObj = movie;
                chiTietHoaDon.HoaDonObj =
                hoaDon;
                chiTietHoaDon.SoLuong = 1;
                hoaDon.ChiTietHoaDons.Add(chiTietHoaDon);
            }
            else
            {
                chiTietHoaDon.SoLuong++;
            }
            _context.SaveChanges();
            return View(hoaDon);
            //return View();
        }

        public ActionResult RemoveFromCart(int maMovies)
        {
            //var hoaDon = this.Session["HoaDon"] as HoaDon;
            //var chiTietHoaDon = hoaDon.ChiTietHoaDons.Where(x => x.MovieObj.Id == maMovies).FirstOrDefault();
            //hoaDon.ChiTietHoaDons.Remove(chiTietHoaDon);
            //return View("AddToCart", hoaDon);
            return View();
        }

        public PartialViewResult Summary()
        {
            //var hoaDon = this.Session["HoaDon"] as HoaDon;
            //if (hoaDon == null)
            //{
            //    return null;
            //}
            //return PartialView(hoaDon);
            return PartialView();
        }
    }
}