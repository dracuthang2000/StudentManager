using Dapper;
using DapperParameters;
using StudentManagement.Factories;
using StudentManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Repositories
{
    public class SinhVienDAL
    {
        public DataResponse<List<SINHVIEN>> GetListSINHVIEN_LOPTINHCHI(string nienKhoa, int hocKy, string mamh, int nhom)
        {
            var conn = SQLFactory.GetConnection();
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
            finally
            {
                conn.Close();
            }

        }
        public DataResponse<List<SINHVIEN>> GetListSinhVien()
        {
            var conn = SQLFactory.GetConnection();
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
            finally
            {
                conn.Close();
            }
        }
        public DataResponse<List<SINHVIEN>> GetListSinhVienByLop(string idClass)
        {
            var conn = SQLFactory.GetConnection();
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
            finally
            {
                conn.Close();
            }
        }

        public DataResponse<bool> UpdateSinhVien(List<UPDATESINHVIEN> list)
        {
            var conn = SQLFactory.GetConnection();
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
            finally
            {
                conn.Close();
            }
        }
        public DataResponse<bool> CheckSinhvien(string masv)
        {
            var conn = SQLFactory.GetConnection();
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
            finally
            {
                conn.Close();
            }
        }

        public DataResponse<bool> CheckSinhvienUpdate(string masv)
        {
            var conn = SQLFactory.GetConnection();
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
            finally
            {
                conn.Close();
            }
        }

        public DataResponse<bool> CheckSinhvienExistsByServer(string masv)
        {
            var conn = SQLFactory.GetConnection();
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
            finally
            {
                conn.Close();
            }
        }
        public DataResponse<bool> CheckSinhvienThi(string masv)
        {
            var conn = SQLFactory.GetConnection();
            try
            {
                string command = "select [dbo].[FUNC_CHECK_SV_THI] (@MASV)";
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
            finally
            {
                conn.Close();
            }
        }

        public DataResponse<List<INDIEMSINHVIEN>> inDiemSV(string masv)
        {
            var conn = SQLFactory.GetConnection();
            try
            {
                string command = "exec dbo.SP_DS_DIEM_SINHVIEN_BY_MASV @masv";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@masv", masv);
                var data = conn.Query<INDIEMSINHVIEN>(command,parameters).ToList();
                return new DataResponeSuccess<List<INDIEMSINHVIEN>>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<List<INDIEMSINHVIEN>>("Lỗi hệ thống");
            }
            finally
            {
                conn.Close();
            }
        }

    }
}
