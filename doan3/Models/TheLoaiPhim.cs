using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doan3.Models
{
    public class TheLoaiPhim
    {
        DatVeXemPhimEntities1 db = new DatVeXemPhimEntities1();
        public TheLoaiPhim()
        {

        }
        public List<TheLoai> GetTheLoai()
        {
            return db.TheLoais.OrderBy(t => t.MaTheLoai).ToList();
        }
    }
}