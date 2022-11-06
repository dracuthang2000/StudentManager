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
    public class DangKyDAL
    {
        public DataResponse<bool> UpdateDangKy(List<UpdateDangKy> list, string masv)
        {
            var conn = SQLFactory.GetConnection();
            try
            {
                string command = "exec [dbo].[SP_UPDATE_DangKy] @DK, @masv";
                DynamicParameters parameters = new DynamicParameters();
                parameters.AddTable("@DK", "[dbo].[TYPE_UPDATE_DANGKY]", list);
                parameters.Add("@masv", masv);
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
        public DataResponse<List<LOPTINCHI>> GetListDangKyBySinhVien(string nienKhoa, int hocKy, string maSinhVien)
        {
            var conn = SQLFactory.GetConnection();
            try
            {
                string command = "exec [dbo].[SP_DS_DANGKY_SINHVIEN] @nienKhoa , @hocKy , @maSV";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@nienKhoa", nienKhoa);
                parameters.Add("@hocKy", hocKy);
                parameters.Add("maSV", maSinhVien);
                var res = conn.Query<LOPTINCHI>(command, parameters).ToList();
                return new DataResponeSuccess<List<LOPTINCHI>>(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<List<LOPTINCHI>>("Lỗi hệ thống");
            }
            finally
            {
                conn.Close();
            }
        }
        public DataResponse<List<DANGKY>> GetListDKByMALTC(int maltc)
        {
            var conn = SQLFactory.GetConnection();
            try
            {
                string command = "exec [dbo].[SP_DS_DANGKY_BY_MALTC] @maltc";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@maltc", maltc);
                var res = conn.Query<DANGKY>(command, parameters).ToList();
                return new DataResponeSuccess<List<DANGKY>>(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<List<DANGKY>>("Lỗi hệ thống");
            }
            finally
            {
                conn.Close();
            }
        }
        public DataResponse<bool> UpdateDiem(List<UpdateGrade> list)
        {
            var conn = SQLFactory.GetConnection();
            try
            {
                
                string command = "exec [dbo].[SP_UPDATE_DIEM] @DK";
                DynamicParameters parameters = new DynamicParameters();
                parameters.AddTable("@DK", "[dbo].[TYPE_UPDATE_DIEM]", list);
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
    }
}
