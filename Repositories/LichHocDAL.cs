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
    public class LichHocDAL
    {
        public DataResponse<List<LICHHOC>> GetLichHoc()
        {
            var conn = SQLFactory.GetConnection();
            try
            {
                string command = "exec dbo.SP_GET_LICHHOC";
                DynamicParameters parameters = new DynamicParameters();
                var data = conn.Query<LICHHOC>(command).ToList();
                return new DataResponeSuccess<List<LICHHOC>>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<List<LICHHOC>>("Lỗi hệ thống");
            }
            finally
            {
                conn.Close();
            }
        }

        public DataResponse<List<LICHHOC>> GetChiTietLichHocByNienKhoa(string nienKhoa, int hocKy)
        {
            var conn = SQLFactory.GetConnection();
            try
            {
                string command = "exec dbo.[SP_GET_CT_LICHHOC_BY_NK] @nienKhoa, @hocky";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@nienKhoa", nienKhoa);
                parameters.Add("@hocky", hocKy);
                var data = conn.Query<LICHHOC>(command,parameters).ToList();
                return new DataResponeSuccess<List<LICHHOC>>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<List<LICHHOC>>("Lỗi hệ thống");
            }
            finally
            {
                conn.Close();
            }
        }

        public DataResponse<List<LICHHOC>> GetChiTietLichHoc()
        {
            var conn = SQLFactory.GetConnection();
            try
            {
                string command = "exec dbo.[SP_GET_CHI_TIET_LICHHOC]";
                DynamicParameters parameters = new DynamicParameters();
                var data = conn.Query<LICHHOC>(command).ToList();
                return new DataResponeSuccess<List<LICHHOC>>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<List<LICHHOC>>("Lỗi hệ thống");
            }
            finally
            {
                conn.Close();
            }
        }

        public DataResponse<List<LICHHOC>> GetChiTietLichHocByMaLTC(int maltc)
        {
            var conn = SQLFactory.GetConnection();
            try
            {
                string command = "exec dbo.[SP_GET_CHI_TIET_LICHHOC_LTC] @maltc";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@maltc", maltc);
                var data = conn.Query<LICHHOC>(command,parameters).ToList();
                return new DataResponeSuccess<List<LICHHOC>>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<List<LICHHOC>>("Lỗi hệ thống");
            }
            finally
            {
                conn.Close();
            }
        }

        public DataResponse<bool> UpdateCTLichHoc(List<UPDATE_CHITIET_LH> list)
        {
            var conn = SQLFactory.GetConnection();
            try
            {
                string command = "exec [dbo].[SP_UPDATE_CT_LICHHOC] @CTLH";
                DynamicParameters parameters = new DynamicParameters();
                parameters.AddTable("@CTLH", "TYPE_NEWUPDATE_CT_LICHHOC", list);
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
