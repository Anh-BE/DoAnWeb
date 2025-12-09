using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doan3.Models;
using System.IO;
using System.Data.SqlClient;

namespace doan3.Controllers
{
    public class ChucNang_AdminController : Controller
    {
        DatVeXemPhimEntities1 db = new DatVeXemPhimEntities1();
        // GET: /ChucNang_Admin/

        // GET: PhimAdmin/ThemPhim
        public ActionResult ThemPhim()
        {
            // Dropdown thể loại phim
            ViewBag.MaTheLoai = new SelectList(
                db.TheLoais
                  .OrderBy(t => t.TenTheLoai)
                  .ToList(),
                "MaTheLoai",
                "TenTheLoai"
            );

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemPhim(
            [Bind(Include = "TenPhim,MoTa,NgayKhoiChieu,ThoiLuong,DaoDien,PhanLoaiDoTuoi,TrangThai,MaTheLoai")]
    Phim phim,
            HttpPostedFileBase fileUpload
        )
        {
            // Load lại dropdown thể loại (trường hợp ModelState lỗi trả về View)
            ViewBag.MaTheLoai = new SelectList(
                db.TheLoais
                  .OrderBy(t => t.TenTheLoai)
                  .ToList(),
                "MaTheLoai",
                "TenTheLoai",
                phim.MaTheLoai
            );

            if (ModelState.IsValid)
            {
                // Xử lý upload poster
                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);

                    // Đặt thư mục lưu poster (bạn chỉnh theo project thật, ví dụ: ~/Images/Poster/)
                    var path = Path.Combine(Server.MapPath("~/Images/Phim/"), fileName);

                    if (System.IO.File.Exists(path))
                    {
                        // Nếu file trùng tên – báo cho admin biết
                        ViewBag.Thongbao = "Poster đã tồn tại trên hệ thống, vui lòng đổi tên file hoặc chọn ảnh khác.";
                        return View(phim);
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }

                    // Lưu tên file vào cột Poster trong bảng Phim
                    phim.Poster = fileName;
                }

                // Nếu admin không nhập trạng thái thì mặc định 'Dang Chieu'
                if (string.IsNullOrEmpty(phim.TrangThai))
                {
                    phim.TrangThai = "Dang Chieu";
                }

                // THÊM PHIM MỚI
                db.Phims.Add(phim);

                // LƯU XUỐNG DATABASE
                db.SaveChanges();

                return RedirectToAction("Index"); // Index = danh sách Phim cho admin
            }

            return View(phim);
        }



    }
}
