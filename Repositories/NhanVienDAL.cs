using Dapper;
using DapperParameters;
using StudentManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Repositories
{
    class NhanVienDAL
    {
        public DataResponse<List<NHANVIEN>> getListNhanVien()
        {
            if (!BaseDAl.Connect())
                return new DataResponeFail<List<NHANVIEN>>("Lỗi kết nối");
            try
            {
                string command = "exec dbo.SP_DS_NHANVIEN";
                DynamicParameters parameters = new DynamicParameters();
                var data = Program.conn.Query<NHANVIEN>(command).ToList();
                return new DataResponeSuccess<List<NHANVIEN>>(data);
            }
            catch (Exception)
            {
                return new DataResponeFail<List<NHANVIEN>>("Lỗi hệ thống");
            }
            finally
            {
                BaseDAl.DisConnect();
            }
        }       
    }
}
