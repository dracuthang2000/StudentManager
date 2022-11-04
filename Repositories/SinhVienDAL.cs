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
    public class SinhVienDAL: AbsRepository
    {
        public DataResponse<List<SINHVIEN>> GetListSINHVIEN_LOPTINHCHI(string nienKhoa, int hocKy, string mamh, int nhom)
        {

            try
            {
                string command = "exec dbo.SP_DS_SINHVEN_LOPTINCHI @nienKhoa, @hocKy, @mamh, @nhom";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@nienKhoa", nienKhoa);
                parameters.Add("@hocKy", hocKy);
                parameters.Add("@mamh", mamh);
                parameters.Add("@nhom", nhom);
                var data = conn.Query<SINHVIEN>(command, parameters).ToList();
                return new DataResponeSuccess<List<SINHVIEN>>(data);
            }
            catch (Exception)
            {
                return new DataResponeFail<List<SINHVIEN>>("Lỗi hệ thống");
            }

        }
        public DataResponse<List<SINHVIEN>> GetListSinhVien()
        {
            try
            {
                string command = "exec dbo.SP_DS_SINHVIEN";
                var data = conn.Query<SINHVIEN>(command).ToList();
                return new DataResponeSuccess<List<SINHVIEN>>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<List<SINHVIEN>>("Lỗi hệ thống");
            }
        }
        public DataResponse<List<SINHVIEN>> GetListSinhVienByLop(string idClass)
        {
            try
            {
                string command = "exec dbo.SP_DS_SINHVIEN_BY_LOP @MALOP";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MALOP", idClass);
                var data = conn.Query<SINHVIEN>(command, parameters).ToList();
                return new DataResponeSuccess<List<SINHVIEN>>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<List<SINHVIEN>>("Lỗi hệ thống");
            }
        }

        public DataResponse<bool> UpdateSinhVien(List<UPDATESINHVIEN> list)
        {
            try
            {
                string command = "exec [dbo].[SP_UPDATE_SINHVIEN] @SINHVIEN";
                DynamicParameters parameters = new DynamicParameters();
                parameters.AddTable("@SINHVIEN", "TYPE_NEWUPDATE_SINHVIEN", list);
                conn.Execute(command, parameters);
                return new DataResponeSuccess<bool>(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<bool>("Lỗi hệ thống");
            }
        }
        public DataResponse<bool> CheckSinhvien(string masv)
        {
            try
            {
                string command = "select [dbo].[func_KT_SINHVIEN_EXISTS] (@MASV)";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MASV", masv);
                var res = conn.ExecuteScalar<bool>(command, parameters);
                return new DataResponeSuccess<bool>(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<bool>("Lỗi hệ thống");
            }
        }

        public DataResponse<bool> CheckSinhvienUpdate(string masv)
        {
            try
            {
                string command = "select [dbo].[FUNC_KT_MASINHVIEN] (@MASV)";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MASV", masv);
                var res = conn.ExecuteScalar<bool>(command, parameters);
                return new DataResponeSuccess<bool>(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<bool>("Lỗi hệ thống");
            }
        }

        public DataResponse<bool> CheckSinhvienExistsByServer(string masv)
        {
            try
            {
                string command = "select [dbo].[FUNC_KT_MASINHVIEN_EXISTSBYSERVER] (@MASV)";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MASV", masv);
                var res = conn.ExecuteScalar<bool>(command, parameters);
                return new DataResponeSuccess<bool>(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<bool>("Lỗi hệ thống");
            }
        }

        public DataResponse<List<DIEMSINHVIEN>> inDiemSV(string masv)
        {
            try
            {
                string command = "exec dbo.SP_DS_DIEM_SINHVIEN_BY_MASV @masv";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@masv", masv);
                var data = conn.Query<DIEMSINHVIEN>(command,parameters).ToList();
                return new DataResponeSuccess<List<DIEMSINHVIEN>>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<List<DIEMSINHVIEN>>("Lỗi hệ thống");
            }
        }
    }
}
