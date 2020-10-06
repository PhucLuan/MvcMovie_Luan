using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Models
{
    public class Genre
    {
        public Genre()
        {
            Movies = new List<Movie>();
        }
        [Key]
        public int IdGenre { get; set; }
        public string Name { get; set; }
        public IList<Movie> Movies { get; set; }
    }
}
