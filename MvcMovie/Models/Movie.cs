using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MvcMovie.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MvcMovie.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Display(Name = "Tiêu Đề")]
        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Title { get; set; }

        [Display(Name = "Tóm tắc")]
        [StringLength(int.MaxValue, MinimumLength = 3)]
        public string Summary { get; set; }

        [Display(Name = "Ngày phát hành")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [CheckDateGreaterThanTodayAttribute]
        public DateTime ReleaseDate { get; set; }

        [StringLength(10)]
        [Display(Name = "Thể loại")]
        //[GenreAttribute]
        public string Genre { get; set; }

        [Display(Name = "Giá")]
        [Range(5000, double.MaxValue)]
        public decimal Price { get; set; }

        [Display(Name = "Xếp hạng")]
        [Range(1, 5)]
        public double Rated { get; set; }

        public String PicturePath { get; set; }
        [NotMapped]
        [Display(Name = "Hình ảnh")]
        public IFormFile PictureUpload { get; set; }

        [Display(Name = "Thể Loại")]
        //[GenreAttribute]
        [ForeignKey("GenreObj")]
        public int IdGenre { get; set; }

        public virtual Genre GenreObj { get; set; }
    }
    //Release Date: lớn hơn ngày hiện tại, hiện thị kiểu ngày tháng năm
    public class CheckDateGreaterThanTodayAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime dt = (DateTime)value;
            if (dt >= DateTime.UtcNow)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage ?? "Dữ liệu ngày phải lớn hơn ngày hôm nay");
        }
    }
    //Genre: chỉ cho phép nhập từ danh sách có sẳn: Kiếm Hiệp, XHĐ, Thiếu nhi...

    public class GenreAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int genreID = int.Parse(value.ToString());
            ApplicationDbContext context = validationContext.GetService<ApplicationDbContext>();
            if (context.Genres.Any(x => x.IdGenre == genreID))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(
            ErrorMessage ?? "Genre khong ton tai");
        }
    }
}
