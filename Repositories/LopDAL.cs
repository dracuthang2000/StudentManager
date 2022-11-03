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
    class LopDAL : AbsRepository
    {
        public DataResponse<List<LOP>> GetListLopByNienKhoa(string nienKhoa)
        {
            try
            {
                string command = "exec dbo.DS_LOP @nienKhoa";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@nienKhoa", nienKhoa);
                var data = conn.Query<LOP>(command, parameters).ToList();
                return new DataResponeSuccess<List<LOP>>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<List<LOP>>("Lỗi hệ thống");
            }
        }
        public DataResponse<List<KHOAHOC>> GetListKhoaHoc()
        {
            try
            {
                string command = "exec dbo.DS_KHOAHOC_LOP";
                var data = conn.Query<KHOAHOC>(command).ToList();
                return new DataResponeSuccess<List<KHOAHOC>>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<List<KHOAHOC>>("Lỗi hệ thống");
            }
        }

        public DataResponse<List<LOP>> GetListLop()
        {
            try
            {
                string command = "exec dbo.SP_DS_LOPHOC";
                var data = conn.Query<LOP>(command).ToList();
                return new DataResponeSuccess<List<LOP>>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<List<LOP>>("Lỗi hệ thống");
            }
        }

        public DataResponse<bool> UpdateLop(List<UPDATELOP> list)
        {
            try
            {
                string command = "exec [dbo].[SP_UPDATE_Lop] @LOP";
                DynamicParameters parameters = new DynamicParameters();
                parameters.AddTable("@LOP", "TYPE_NEWUPDATE_LOP", list);
                conn.Execute(command, parameters);
                return new DataResponeSuccess<bool>(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<bool>("Lỗi hệ thống");
            }
        }

        public DataResponse<bool> CheckLop(string malop)
        {
            try
            {
                string command = "select [dbo].[KT_KHOANGOAI_LOP] (@MALOP)";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MALOP", malop);
                var res = conn.ExecuteScalar<bool>(command, parameters);
                return new DataResponeSuccess<bool>(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<bool>("Lỗi hệ thống");
            }
        }

        public DataResponse<bool> CheckMaLopLop(string malop)
        {
            try
            {
                string command = "select [dbo].[FUNC_KT_MALOP] (@MALOP)";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MALOP", malop);
                var res = conn.ExecuteScalar<bool>(command, parameters);
                return new DataResponeSuccess<bool>(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<bool>("Lỗi hệ thống");
            }
        }

        public DataResponse<bool> CheckMaLopLopExistsByServer(string malop)
        {
            try
            {
                string command = "select [dbo].[FUNC_KT_MALOP_EXISTSBYSERVER] (@MALOP)";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MALOP", malop);
                var res = conn.ExecuteScalar<bool>(command, parameters);
                return new DataResponeSuccess<bool>(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<bool>("Lỗi hệ thống");
            }
        }
        public DataResponse<List<TongKetCuoiKhoa>> GetListTongKetCuoiKhoa(string malop)
        {
            try
            {
                string command = "exec dbo.SP_DS_TONGKETCUOIKHOA @malop";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@malop", malop);

                var data = conn.Query<TongKetCuoiKhoa>(command,parameters).ToList();
                return new DataResponeSuccess<List<TongKetCuoiKhoa>>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<List<TongKetCuoiKhoa>>("Lỗi hệ thống");
            }
        }
    }
}
