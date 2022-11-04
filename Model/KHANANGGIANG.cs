using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Model
{
    public class KHANANGGIANG
    {
        string MAMH { get; set; }
        string MAGV { get; set; }

        public KHANANGGIANG(GIANGVIEN model)
        {
            MAMH = model.MAMH;
            MAGV = model.MAGV;
        }
    }
}
