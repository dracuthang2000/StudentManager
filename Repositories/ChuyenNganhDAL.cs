using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using StudentManagement.Model;
using DapperParameters;

namespace StudentManagement.Repositories
{
    public class ChuyenNganhDAL: AbsRepository
    {
        public DataResponse<List<CHUYENNGANH>> getChuyenNganh()
        {
            try
            {
                string command = "exec dbo.SP_GET_CHUYENNGANH";
                DynamicParameters parameters = new DynamicParameters();
                var data = conn.Query<CHUYENNGANH>(command,parameters).ToList();
                return new DataResponeSuccess<List<CHUYENNGANH>>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<List<CHUYENNGANH>>("Lỗi hệ thống");
            }
        }
       
    }

       
}
