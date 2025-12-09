using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doan3.Models;
using System.Data.Entity;

namespace doan3.Controllers
{
    public class HomeController : Controller
    {
        DatVeXemPhimEntities1 db = new DatVeXemPhimEntities1();
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult PhimDangChieu()
        {
            List<Phim> dsphim = db.Phims
                          .Include(p => p.Lich_Chieu) 
                          .OrderByDescending(t => t.NgayKhoiChieu)
                          .ToList();
            return View(dsphim);
        }
        public ActionResult PhimTheoTheLoai(int MATHELOAI)
        {
            // Lấy thông tin thể loại
            TheLoai theloai = db.TheLoais
                .SingleOrDefault(t => t.MaTheLoai == MATHELOAI);
            if (theloai == null)    
                return HttpNotFound();

            // Lấy danh sách phim thuộc thể loại đó
            List<Phim> dsPhim = db.Phims
                                 .Where(p => p.MaTheLoai == MATHELOAI)
                                 .OrderBy(p => p.ThoiLuong)
                                 .ToList();

            // Gửi tên thể loại sang View
            ViewBag.TenTheLoai = theloai.TenTheLoai;

            return View(dsPhim);
        } 
        public ActionResult PhimTheo_Rap(long IDRap)
        {
            Rap_Chieu rapchieu = db.Rap_Chieu.SingleOrDefault(t => t.RapID == IDRap);
            if(rapchieu == null)
            {
                return HttpNotFound();
            }
            var phim = db.Lich_Chieu
                                    .Where(lc => lc.Phong_Chieu.RapID == IDRap)
                                    .Select(lc => lc.Phim)
                                    .Distinct()
                                    .ToList();
            return View(phim);
        }
        public ActionResult ChiTietPhim(int id)
        {
            var phim = db.Phims
                         .Include(p => p.TheLoai)
                         .SingleOrDefault(p => p.PhimID == id);

            if (phim == null)
            {
                Response.StatusCode = 404;
                return View("Error");
            }

            return View(phim);
        }
      
        public ActionResult TimKiemPhim()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TimKiemPhim(string tenphim)
        {
            
            if (string.IsNullOrEmpty(tenphim))
            {
                ViewBag.Message = "Vui lòng nhập tên phim!";
                ViewBag.Result = null;
                return View("TimKiemPhim");
            }

         
            var ketQua = db.Phims
                           .Where(p => p.TenPhim.Contains(tenphim))
                           .ToList();

        
            if (ketQua == null || ketQua.Count == 0)
            {
                ViewBag.Message = "Không tìm thấy phim nào có tên: " + tenphim;
                ViewBag.Result = null;
                return View("SearchMovie");
            }

           
            ViewBag.Message = "Bạn vừa tìm: " + tenphim;
            ViewBag.Result = ketQua;

            return View("TimKiemPhim");
        }





        




    }
}
