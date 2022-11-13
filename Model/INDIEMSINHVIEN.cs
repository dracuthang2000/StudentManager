using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Model
{
    public class INDIEMSINHVIEN
    {
        public string MAMH { get; set; }
        public string TENMH { get; set; }

        public int? DIEM_CC { get; set; }

        public double? DIEM_GK { get; set; }

        public double? DIEM_CK { get; set; }

        public double ?DIEM_HM { get; set; }
        
        public string HOTEN { get; set; }
        public string MALOP { get; set; }
        public string TENLOP { get; set; }

        public string NIENKHOA { get; set; }

        public int HOCKY { get; set; }
    }
}
