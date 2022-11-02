namespace StudentManagement.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GIANGVIENLTC
    {
        public string MAMH { get; set; }
        public int MALTC { get; set; }
        public string MAGV { get; set; }

        public int NHOM { get; set; }

        public string MANK { get; set; }
        public GIANGVIENLTC(GIANGVIEN model)
        {
            MALTC = model.MALTC;
            MAMH = model.MAMH;
            MAGV = model.MAGV;
            NHOM = model.NHOM;
            MANK = model.MANK;
        }
    }
}
