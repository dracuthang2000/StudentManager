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
    class MonHocDAL: AbsRepository
    {
        public DataResponse<bool> UpdateMonHoc(List<UPDATEMONHOC> list, List<KHANANGGIANG> listKNG)
        {
            try
            {
                string command = "exec [dbo].[SP_UPDATE_MONHOC] @MONHOC, @KNG";
                DynamicParameters parameters = new DynamicParameters();
                parameters.AddTable("@MONHOC", "TYPE_NEWUPDATE_MONHOC", list);
                parameters.AddTable("@KNG", "TYPE_NEWUPDATE_KHANANGGIANG", listKNG);
                conn.Execute(command, parameters);
                return new DataResponeSuccess<bool>(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<bool>("Lỗi hệ thống");
            }
        }
        public DataResponse<List<MONHOC>> GetListMonHoc()
        {
            try
            {
                string command = "exec dbo.SP_DS_MonHoc";
                var data = conn.Query<MONHOC>(command).ToList();
                return new DataResponeSuccess<List<MONHOC>>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<List<MONHOC>>("Lỗi hệ thống");
            }
        }

        public DataResponse<List<MONHOC>> GetListMonHocKeHoach(string nienKhoa,int hocky)
        {
            try
            {
                string command = "exec [dbo].[SP_DS_MONHOC_KEHOACH] @nienKhoa, @hocky";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@nienKhoa", nienKhoa);
                parameters.Add("@hocKy", hocky);
                var data = conn.Query<MONHOC>(command, parameters).ToList();
                return new DataResponeSuccess<List<MONHOC>>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<List<MONHOC>>("Lỗi hệ thống");
            }
 
        }
        public DataResponse<bool> CheckMonHoc(string mamh)
        {
            try
            {
                string command = "select [dbo].[func_KT_MONHOC] (@MAMH)";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MAMH", mamh);
                var res = conn.ExecuteScalar<bool>(command, parameters);
                return new DataResponeSuccess<bool>(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<bool>("Lỗi hệ thống");
            }
        }

        public DataResponse<bool> CheckMaMonHoc(string mamh)
        {
            try
            {
                string command = "select [dbo].[FUNC_KT_MAMONHOC] (@MAMH)";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MAMH", mamh);
                var res = conn.ExecuteScalar<bool>(command, parameters);
                return new DataResponeSuccess<bool>(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<bool>("Lỗi hệ thống");
            }

        }
    }
}
