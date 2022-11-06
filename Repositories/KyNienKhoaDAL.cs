using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using StudentManagement.Model;
using DapperParameters;
using StudentManagement.Factories;

namespace StudentManagement.Repositories
{
    public class KyNienKhoaDAL
    {
        public DataResponse<KYNIENKHOA> GetNienKhoaByAny(string nienKhoa, int hocKy)
        {
            var conn = SQLFactory.GetConnection();
            try
            {
                string command = "exec dbo.SP_GET_MANK @nienKhoa, @hocKy";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@nienKhoa", nienKhoa);
                parameters.Add("@hocKy", hocKy);
                var data = conn.Query<KYNIENKHOA>(command,parameters).First();
                return new DataResponeSuccess<KYNIENKHOA>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<KYNIENKHOA>("Lỗi hệ thống");
            }
            finally
            {
                conn.Close();
            }
        }
       
    }

       
}
