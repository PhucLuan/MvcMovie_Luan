using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MvcMovie.Models
{
    [ModelBinder(BinderType = typeof(HoaDonModelBinder))]
    public class HoaDon
    {
        public HoaDon()
        {
            ChiTietHoaDons = new List<ChiTietHoaDon>();
        }
        [Key]
        public int MaHoaDon { get; set; }
        public string MaHoaDonTam { get; set; }
        public IList<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public double TongTien { get; set; }
        public DateTime NgayLap { get; set; }
        public int? MaKhachHang { get; set; }
        public bool DaThanhToan { get; set; }
    }
}
