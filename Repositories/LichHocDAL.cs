﻿using System;
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

        public DataResponse<bool> CheckSamePlan(string magv,int malh, string mank)
        {
            var conn = SQLFactory.GetConnection();
            try
            {
                string command = "select [dbo].[CHECK_SAME_PLAN_WITH_NK] (@MAGV,@ID_LH,@MANK)";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MAGV", magv);
                parameters.Add("@ID_LH", malh);
                parameters.Add("@MANK", mank);
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

        public DataResponse<Boolean> CheckGVPLanLTC(string magv, int maltc)
        {
            var conn = SQLFactory.GetConnection();
            try
            {
                string command = "exec [dbo].[SP_CALL_CHECK_PLAN_GV_LTC] @maltc,@MAGV";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@maltc", maltc);
                parameters.Add("@MAGV", magv);
                var res = conn.ExecuteScalar<Boolean>(command, parameters);
                return new DataResponeSuccess<Boolean>(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<Boolean>("Lỗi hệ thống");
            }
            finally
            {
                conn.Close();
            }
        }
    }

       
}
