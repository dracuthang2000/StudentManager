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
    class GiangVienDAL : AbsRepository
    {
        public DataResponse<List<GIANGVIEN>> GetListGiangVien()
        {

            try
            {
                string command = "exec dbo.SP_DS_All_GiangVien";
                DynamicParameters parameters = new DynamicParameters();
                var data = conn.Query<GIANGVIEN>(command).ToList();
                return new DataResponeSuccess<List<GIANGVIEN>>(data);
            }
            catch (Exception)
            {
                return new DataResponeFail<List<GIANGVIEN>>("Lỗi hệ thống");
            }
        }

        public DataResponse<List<GIANGVIEN>> GetListGiangVienKhaNangGiangMonHoc(string mamh)
        {
            try
            {
                string command = "exec [dbo].[SP_GIANGVIEN_KHANANGGIANG] @mamh";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@mamh", mamh);
                var data = conn.Query<GIANGVIEN>(command, parameters).ToList();
                return new DataResponeSuccess<List<GIANGVIEN>>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<List<GIANGVIEN>>("Lỗi hệ thống");
            }
        }

        public DataResponse<List<GIANGVIEN>> GetListGiangVienKhaNangGiangMonHoc()
        {
            try
            {
                string command = "exec [dbo].[SP_DS_GIANGVIEN_KHANANGGIANG]";
                DynamicParameters parameters = new DynamicParameters();
                var data = conn.Query<GIANGVIEN>(command).ToList();
                return new DataResponeSuccess<List<GIANGVIEN>>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<List<GIANGVIEN>>("Lỗi hệ thống");
            }
        }

        public DataResponse<List<GIANGVIEN>> GetListGiangVienLopTC()
        {
            try
            {
                string command = "exec [dbo].[SP_DS_GIANGVIEN_LOPTINCHI]";
                DynamicParameters parameters = new DynamicParameters();
                var data = conn.Query<GIANGVIEN>(command).ToList();
                return new DataResponeSuccess<List<GIANGVIEN>>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<List<GIANGVIEN>>("Lỗi hệ thống");
            }
        }
        public DataResponse<List<GIANGVIEN>> GetListCurrentGiangVien()
        {
            try
            {
                string command = "exec dbo.SP_DS_GiangVien_CURRENT";
                DynamicParameters parameters = new DynamicParameters();
                var data = conn.Query<GIANGVIEN>(command).ToList();
                return new DataResponeSuccess<List<GIANGVIEN>>(data);
            }
            catch (Exception)
            {
                return new DataResponeFail<List<GIANGVIEN>>("Lỗi hệ thống");
            }

        }

        public DataResponse<List<GIANGVIEN>> GetListAllGiangVien()
        {
            try
            {
                string command = "exec dbo.SP_DS_All_GiangVien";
                DynamicParameters parameters = new DynamicParameters();
                var data = conn.Query<GIANGVIEN>(command).ToList();
                return new DataResponeSuccess<List<GIANGVIEN>>(data);
            }
            catch (Exception)
            {
                return new DataResponeFail<List<GIANGVIEN>>("Lỗi hệ thống");
            }
        }
        public DataResponse<List<GIANGVIEN>> GetListGiangVienByKhoa(string maKhoa)
        {
            try
            {
                string command = "exec dbo.SP_GET_DS_GIANGVIEN_BY_KHOA @MAKHOA";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MAKHOA", maKhoa);
                var data = conn.Query<GIANGVIEN>(command,parameters).ToList();
                return new DataResponeSuccess<List<GIANGVIEN>>(data);
            }
            catch (Exception)
            {
                return new DataResponeFail<List<GIANGVIEN>>("Lỗi hệ thống");
            }
        }

        public DataResponse<bool> UpdateGiangVien(List<UPDATEGIANGVIEN> list,string maKhoa)
        {
            try
            {
                string command = "exec [dbo].[SP_UPDATE_GIANGVIEN] @GIANGVIEN, @MAKHOA";
                DynamicParameters parameters = new DynamicParameters();
                parameters.AddTable("@GIANGVIEN", "TYPE_NEWUPDATE_GIANGVIEN", list);
                parameters.Add("@MAKHOA", maKhoa);

                conn.Execute(command, parameters);
                return new DataResponeSuccess<bool>(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<bool>("Lỗi hệ thống");
            }
        }

        public DataResponse<bool> CheckGiangVien(string magv)
        {
            try
            {
                string command = "select [dbo].[FUNC_KT_MAGV_KHOANGOAI] (@MAGV)";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MAGV", magv);
                var res = conn.ExecuteScalar<bool>(command, parameters);
                return new DataResponeSuccess<bool>(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<bool>("Lỗi hệ thống");
            }
        }

        public DataResponse<bool> CheckGiangVienDayLTC(string magv,string mamh)
        {
            try
            {
                string command = "select [dbo].[FUNC_KT_MAGV_EXIST_DAY_LTC] (@MAGV,@mamh)";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MAGV", magv);
                parameters.Add("@mamh", mamh);
                var res = conn.ExecuteScalar<bool>(command, parameters);
                return new DataResponeSuccess<bool>(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<bool>("Lỗi hệ thống");
            }
        }

        public DataResponse<List<KHOA>> GetListKhoa()
        {
            try
            {
                string command = "exec dbo.SP_GET_DS_KHOA";
                DynamicParameters parameters = new DynamicParameters();
                var data = conn.Query<KHOA>(command).ToList();
                return new DataResponeSuccess<List<KHOA>>(data);
            }
            catch (Exception)
            {
                return new DataResponeFail<List<KHOA>>("Lỗi hệ thống");
            }
        }
    }
}
