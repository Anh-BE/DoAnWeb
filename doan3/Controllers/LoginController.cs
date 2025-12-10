using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doan3.Models;
namespace doan3.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        DatVeXemPhimEntities1 db = new DatVeXemPhimEntities1();
        public ActionResult Index_DangNhap()
        {
            if(Request.Cookies["Username"] != null && Request.Cookies["Password"] != null) 
            {
                ViewBag.username = Request.Cookies["Username"].Value;
                ViewBag.username = Request.Cookies["Password"].Value;
            }
            return View();
        }
        public void ghinhotaikhoan(string username, string password)
        {
            HttpCookie us = new HttpCookie("username");
            HttpCookie pas = new HttpCookie("password");

            us.Value = username;
            pas.Value = password;

            us.Expires = DateTime.Now.AddDays(1);
            pas.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(us);
            Response.Cookies.Add(pas);

        }

        [HttpPost]
        public ActionResult Login(string username, string password, string ghinho)
        {
            if (Request.Cookies["username"] != null && Request.Cookies["password"] != null)
            {
                username = Request.Cookies["username"].Value;
                password = Request.Cookies["password"].Value;
            }





            if (checkpassword(username, password))
            {
                var userSession = new UserLogin();
                userSession.UserName = username;

                var listGroups = GetListGroupID(username);//Có thể viết dòng lệnh lấy các GroupID từ CSDL, ví dụ gán ="ADMIN", dùng List<string>

                Session.Add("SESSION_GROUP", listGroups);
                Session.Add("USER_SESSION", userSession);

                if (ghinho == "on")//Ghi nhớ
                    ghinhotaikhoan(username, password);
                return Redirect("~/Home/PhimDangChieu");

            }
            return Redirect("~/dang-nhap");
        }
        public List<string> GetListGroupID(string userName)
        {
            // var user = db.User.Single(x => x.UserName == userName);

            var data = (from a in db.NhomNguoiDungs
                        join b in db.NguoiDungs on a.ID equals b.GroupID
                        where b.UserName == userName

                        select new
                        {
                            UserGroupID = b.GroupID,
                            UserGroupName = a.Name
                        });

            return data.Select(x => x.UserGroupName).ToList();

        }
        public bool checkpassword(string username, string password)
        {
            if (db.NguoiDungs.Where(x => x.UserName == username && x.Password == password).Count() > 0)

                return true;
            else
                return false;


        }




        public ActionResult SignOut()
        {

            Session["USER_SESSION"] = null;




            if (Request.Cookies["username"] != null && Request.Cookies["password"] != null)
            {
                HttpCookie us = Request.Cookies["username"];
                HttpCookie ps = Request.Cookies["password"];

                ps.Expires = DateTime.Now.AddDays(-1);
                us.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(us);
                Response.Cookies.Add(ps);
            }

            return Redirect("~/dang-nhap");
        }


        [ChildActionOnly]
        public ActionResult thongtindangnhap()
        {
            return PartialView("ThongTinDangNhap");

        }

    }
}
