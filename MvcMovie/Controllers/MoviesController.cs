using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MimeKit;
using MvcMovie.Data;
using MvcMovie.Models;
using MvcMovie.Utils;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartId = "CartId";

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index(HoaDon hoaDon)
        {
            var applicationDbContext = _context.Movies.Include(m => m.GenreObj);
            //Ghi cookie xuong DB
            bool isExisted = await _context.HoaDons.AnyAsync(x => x.MaHoaDonTam == Request.Cookies[CartId]);
            if (!isExisted)
            {
                var cookie = new CookieOptions
                {
                    Expires = DateTime.Now.AddMonths(3)
                };
                var cartId = Guid.NewGuid();
                hoaDon = new HoaDon
                {
                    MaHoaDonTam = cartId.ToString()
                };

                _context.HoaDons.Add(hoaDon);
                await _context.SaveChangesAsync();

                Response.Cookies.Append(CartId, cartId.ToString());
            }
            else
            {
                string cookie = Request.Cookies[CartId];

                hoaDon = await _context.HoaDons.Include(x => x.ChiTietHoaDons).ThenInclude(x => x.MovieObj).FirstOrDefaultAsync(x => x.MaHoaDonTam == cookie);
            }
            ViewBag.HoaDon = hoaDon;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.GenreObj)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewBag.ListGenre = _context.Genres.Select(x => new SelectListItem(){ Text = x.Name, Value = x.IdGenre.ToString() })
            .Distinct().ToList();
            ViewData["IdGenre"] = new SelectList(_context.Genres, "IdGenre", "IdGenre");
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Summary,ReleaseDate,Genre,Price,Rated,PicturePath,IdGenre,PictureUpload")] Movie movie)
        {
            if (movie.PictureUpload != null)
            {
                string path = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images"),
                Path.GetFileName(movie.PictureUpload.FileName));
                using (var stream = System.IO.File.Create(path))
                {
                    movie.PictureUpload.CopyTo(stream);
                }
                string pathInDb = "/images/" + Path.GetFileName(movie.PictureUpload.FileName);
                movie.PicturePath = pathInDb;
            }
            else
            {
                //Kiem bat ky hinh No Image tren internet
                movie.PicturePath = "/images/no-image.jpg";
            }

            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdGenre"] = new SelectList(_context.Genres, "IdGenre", "IdGenre", movie.IdGenre);
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewBag.ListGenre = _context.Genres.Select(x => new SelectListItem() { Text = x.Name, Value = x.IdGenre.ToString() })
            .Distinct().ToList();
            //ViewData["IdGenre"] = new SelectList(_context.Genres, "IdGenre", "IdGenre", movie.IdGenre);
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Summary,ReleaseDate,Genre,Price,Rated,PicturePath,IdGenre")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (movie.PictureUpload != null)
                    {
                        string path = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images"),
                        Path.GetFileName(movie.PictureUpload.FileName));
                        using (var stream = System.IO.File.Create(path))
                        {
                            movie.PictureUpload.CopyTo(stream);
                        }
                        string pathInDb = "/images/" + Path.GetFileName(movie.PictureUpload.FileName);
                        movie.PicturePath = pathInDb;
                    }
                    else
                    {
                        //Kiem bat ky hinh No Image tren internet
                        movie.PicturePath = "/images/no-image.jpg";
                    }
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdGenre"] = new SelectList(_context.Genres, "IdGenre", "IdGenre", movie.IdGenre);
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.GenreObj)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddToCart(int id, HoaDon hoaDon)
        {
            //Kiem tra Id movie ton tai hay khong
            var movie = _context.Movies.FirstOrDefault(x => x.Id == id);
            if (movie == null)
            {
                return RedirectToAction("Index");
            }

            string cookie = Request.Cookies[CartId];

            hoaDon = await _context.HoaDons.Include(x => x.ChiTietHoaDons).ThenInclude(x => x.MovieObj).FirstOrDefaultAsync(x => x.MaHoaDonTam == cookie);

            //Kiem tra don hang da co truoc do
            var chiTietHoaDon = hoaDon.ChiTietHoaDons.FirstOrDefault(x => x.MaMovie == id);
            if (chiTietHoaDon == null)
            {
                chiTietHoaDon = new ChiTietHoaDon
                {
                    MaMovie = id,
                    MovieObj = movie,
                    HoaDonObj = hoaDon,
                    SoLuong = 1
                };

                hoaDon.ChiTietHoaDons.Add(chiTietHoaDon);
            }
            else
            {
                chiTietHoaDon.SoLuong++;
            }

            hoaDon.TongTien = hoaDon.ChiTietHoaDons
                .Select(x => x.SoLuong * (double)x.MovieObj.Price)
                .Aggregate(0D, (cur, next) => (cur + next));

            await _context.SaveChangesAsync();
            return View(hoaDon);
        }

        public async Task<IActionResult> RemoveFromCart(int maMovies, HoaDon hoaDon)
        {
            //var hoaDon = HttpContext.Session.Get<HoaDon>("HoaDon");
            string cookie = Request.Cookies[CartId];

            hoaDon = await _context.HoaDons.Include(x => x.ChiTietHoaDons).ThenInclude(x => x.MovieObj).FirstOrDefaultAsync(x => x.MaHoaDonTam == cookie);
            var chiTietHoaDon = hoaDon.ChiTietHoaDons.Where(x =>
            x.MovieObj.Id == maMovies).FirstOrDefault();
            if (chiTietHoaDon.SoLuong >1)
            {
                chiTietHoaDon.SoLuong--;
            }
            else
            {
                hoaDon.ChiTietHoaDons.Remove(chiTietHoaDon);
            }
            await _context.SaveChangesAsync();
            return View("AddToCart", hoaDon);
        }
        public async Task<IActionResult> Cart(HoaDon hoaDon)
        {
            string cookie = Request.Cookies[CartId];

            hoaDon = await _context.HoaDons.Include(x => x.ChiTietHoaDons)
                .ThenInclude(x => x.MovieObj)
                .FirstOrDefaultAsync(x => x.MaHoaDonTam == cookie);
            return View("AddToCart", hoaDon);
        }
        public async Task<PartialViewResult> Summary()
        {
            string cookie = Request.Cookies[CartId];
            HoaDon hoaDon;
            hoaDon = await _context.HoaDons.Include(x => x.ChiTietHoaDons).ThenInclude(x => x.MovieObj).FirstOrDefaultAsync(x => x.MaHoaDonTam == cookie);
            //var hoaDon = this.Session["HoaDon"] as HoaDon;
            if (hoaDon == null)
            {
                return null;
            }
            return PartialView(hoaDon);
        }

        //[HttpPost]
        public async Task<IActionResult> Checkout(ShippingDetail detail, HoaDon hoaDon)
        {
            string cookie = Request.Cookies[CartId];
            hoaDon = _context.HoaDons.Include(x => x.ChiTietHoaDons).ThenInclude(x => x.MovieObj).FirstOrDefault(x => x.MaHoaDonTam == cookie);
            //var hoaDon = HttpContext.Session.Get<HoaDon>("HoaDon");
            if (hoaDon.ChiTietHoaDons.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                StringBuilder body = new StringBuilder()
                .AppendLine("A new order has been submitted")
                .AppendLine("---")
                .AppendLine("Items:")
                .AppendLine("");
                foreach (var hoaDonChiTiet in hoaDon.ChiTietHoaDons)
                {
                    var subtotal = hoaDonChiTiet.MovieObj.Price * hoaDonChiTiet.SoLuong;
                    body.AppendFormat("{0} x {1} (subtotal: {2:c})", hoaDonChiTiet.SoLuong,
                    hoaDonChiTiet.MovieObj.Title,
                    subtotal);
                    body.AppendLine("");
                }
                body.AppendFormat("Total order value: {0:c}", hoaDon.TongTien)
                .AppendLine("---")
                .AppendLine("Ship to:")
                .AppendLine(detail.Name)
                .AppendLine(detail.Address)
                .AppendLine(detail.Mobile.ToString())
                .AppendLine("");
                body.AppendFormat("Ngày giao hàng: {0}", detail.ReleaseDate);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("nguyen luan", "phucluan6052000@gmail.com"));
                message.To.Add(new MailboxAddress("NGUYEN PHUC LUAN", detail.Email));
                message.Subject = "Test email shipping detail";
                message.Body = new TextPart("plain") { Text = body.ToString()};
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com",587, false);
                    client.Authenticate("phucluan6052000@gmail.com", "NguyenPhucLuan@0988004673");
                    client.Send(message);
                    client.Disconnect(true);
                }
                _context.ShippingDetails.Add(detail);
                await _context.SaveChangesAsync();
                return View("CheckoutCompleted", detail);
            }
            else
            {
                return View(new ShippingDetail());
            }
        }

        public IActionResult CheckoutCompleted()
        {
            var applicationDbContext = _context.ShippingDetails;
            return View(applicationDbContext.ToList());
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
