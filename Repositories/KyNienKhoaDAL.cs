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
    public class KyNienKhoaDAL
    {
        public DataResponse<KYNIENKHOA> GetNienKhoaByAny(string nienKhoa, int hocKy)
        {
            if (!BaseDAl.Connect())
                return new DataResponeFail<KYNIENKHOA>("Lỗi kết nối");
            try
            {
                string command = "exec dbo.SP_GET_MANK @nienKhoa, @hocKy";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@nienKhoa", nienKhoa);
                parameters.Add("@hocKy", hocKy);
                var data = Program.conn.Query<KYNIENKHOA>(command,parameters).First();
                return new DataResponeSuccess<KYNIENKHOA>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<KYNIENKHOA>("Lỗi hệ thống");
            }
            finally
            {
                BaseDAl.DisConnect();
            }
        }
       
    }

       
}
