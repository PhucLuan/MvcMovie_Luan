using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Movies.Include(m => m.GenreObj);
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
            ViewData["IdGenre"] = new SelectList(_context.Genres, "IdGenre", "IdGenre", movie.IdGenre);
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

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
