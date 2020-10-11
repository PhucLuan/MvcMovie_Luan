using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Models
{
    public class ShippingDetail
    {
        public int ID { get; set; }
        [Display(Name = "Tên")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Địa Chỉ")]
        [Required]
        public string Address { get; set; }
        [Display(Name = "Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Mobile")]
        [Required]
        public int Mobile { get; set; }
        [Display(Name = "Ngày giao")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }
    }

}
