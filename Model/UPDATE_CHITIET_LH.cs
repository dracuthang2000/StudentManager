using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Model
{
    public class UPDATE_CHITIET_LH
    {
        public int MALH { get; set; }

        public int MALTC { get; set; }

        public UPDATE_CHITIET_LH(LICHHOC model)
        {
            MALH = model.MALH;
            MALTC = model.MALTC;
        }
    }
}
