using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Model
{
    public class UpdateLopTinChi
    {    
        public bool HUYLOP { get; set; }
        public string MAGV { get; set; }
        public string MAKHOA { get; set; }
        public int MALTC { get; set; }
        public string MAMH { get; set; }       
        public int NHOM { get; set; }
        public string MANK { get; set; }
        public int SOSVTOITHIEU { get; set; }
        public int SOSVTOIDA { get; set; }

        public string MANV { get; set; }
        
        public UpdateLopTinChi()
        {

        }
        public UpdateLopTinChi(LOPTINCHI model)
        {
            MALTC = model.MALTC;
            MANK = model.MANK;
            MAMH = model.MAMH;
            NHOM = model.NHOM;
            MAKHOA = model.MAKHOA;
            SOSVTOITHIEU = model.SOSVTOITHIEU;
            HUYLOP = model.HUYLOP;
            MANV = model.MANV;
            SOSVTOIDA = model.SOSVTOIDA;
        }
    }
}
