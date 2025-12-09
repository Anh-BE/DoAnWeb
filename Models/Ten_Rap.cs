using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doan3.Models
{
    
    public class Ten_Rap
    {
        DatVeXemPhimEntities1 db = new DatVeXemPhimEntities1();
        public Ten_Rap()
        {

        }
        public List<Rap_Chieu> GetTenRap()
        {
            return db.Rap_Chieu.OrderBy(t => t.MaRap).ToList();
        }
    }
}