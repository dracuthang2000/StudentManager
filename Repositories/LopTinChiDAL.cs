﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using StudentManagement.Model;
using DapperParameters;
using StudentManagement.Factories;
using System.Data.SqlClient;

namespace StudentManagement.Repositories
{
    public class LopTinChiDAL
    {
        public DataResponse<List<LOPTINCHI>> GetListLopTinChi(string nienKhoa, int hocKy)
        {
            var conn = SQLFactory.GetConnection();
            try
            {
                
                string command = "exec dbo.SP_DS_LopTinChi @nienKhoa , @hocKy";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@nienKhoa", nienKhoa);
                parameters.Add("@hocKy", hocKy);
                var data = conn.Query<LOPTINCHI>(command, parameters).ToList();
                return new DataResponeSuccess<List<LOPTINCHI>>(data);
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

        public DataResponse<List<LOPTINCHI>> GetListLopTinChiKhongGV(string nienKhoa, int hocKy)
        {
            var conn = SQLFactory.GetConnection();
            try
            {
                string command = "exec dbo.SP_DS_LopTinChi_Khong_GiangVien @nienKhoa , @hocKy";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@nienKhoa", nienKhoa);
                parameters.Add("@hocKy", hocKy);
                var data = conn.Query<LOPTINCHI>(command, parameters).ToList();
                return new DataResponeSuccess<List<LOPTINCHI>>(data);
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

        public DataResponse<List<INBANGDIEM>> InBangDiemSV(string maSV, string role)
        {
            var conn = SQLFactory.GetConnection();
            try
            {
                string command = "exec dbo.SP_InBangDiemSV @maSV, @role";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@maSV", maSV);
                parameters.Add("@role", role);
                var data = conn.Query<INBANGDIEM>(command, parameters).ToList();
                return new DataResponeSuccess<List<INBANGDIEM>>(data);
            }
            catch (Exception e)
            {
                return new DataResponeFail<List<INBANGDIEM>>(e.ToString().Substring(0, 128));
            }
            finally
            {
                conn.Close();
            }
        }
        public DataResponse<List<LOPTINCHI>> GetListLopTinChiActive(string nienKhoa, int hocKy)
        {
            var conn = SQLFactory.GetConnection();
            try
            {
                string command = "exec dbo.SP_DS_LopTinChi_Active @nienKhoa , @hocKy";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@nienKhoa", nienKhoa);
                parameters.Add("@hocKy", hocKy);
                var data = conn.Query<LOPTINCHI>(command, parameters).ToList();
                return new DataResponeSuccess<List<LOPTINCHI>>(data);
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

        public DataResponse<bool> UpdateLopTinChi(List<UpdateLopTinChi> list,List<GIANGVIENLTC> lstGvLTC, string nienKhoa, int hocKy)
        {
            var conn = SQLFactory.GetConnection();
            try
            {
                string command = "exec [dbo].[SP_UPDATE_LopTinChi] @LTC ,@DAYTEMP, @nienkhoa , @hocky";
                DynamicParameters parameters = new DynamicParameters();
                parameters.AddTable("@LTC","Type_NEWUpdate_LopTinChi",list);
                parameters.AddTable("@DAYTEMP", "TYPE_NEWUPDATE_DAY", lstGvLTC);
                parameters.Add("@nienkhoa", nienKhoa);
                parameters.Add("@hocky", hocKy);
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
        public DataResponse<bool> CheckLopTinChi(int maltc)
        {
            var conn = SQLFactory.GetConnection();
            try
            {
                string command = "select [dbo].[FUNC_KT_DK_LopTinChi] (@MALTC)";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MALTC", maltc);
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

        public DataResponse<List<REPORTDIEMLTC>> ReportDiemLTC(string nienKhoa, int hocKy,string mamh,int nhom)
        {
            var conn = SQLFactory.GetConnection();
            try
            {
                string command = "exec dbo.SP_REPORT_LOPTINHCHI @nienKhoa,@hocKy,@mamh,@nhom";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@nienKhoa", nienKhoa);
                parameters.Add("@hocKy", hocKy);
                parameters.Add("@mamh", mamh);
                parameters.Add("@nhom", nhom);
                var data = conn.Query<REPORTDIEMLTC>(command, parameters).ToList();
                return new DataResponeSuccess<List<REPORTDIEMLTC>>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataResponeFail<List<REPORTDIEMLTC>>("Lỗi hệ thống");
            }
            finally
            {
                conn.Close();
            }
        }
        public void updateHuyLop()
        {
            var conn = SQLFactory.GetConnection();
            try
            {
                string command = "exec dbo.SP_UPDATE_HUYLOP";
                DynamicParameters parameters = new DynamicParameters();
                var data = conn.Query<LOPTINCHI>(command).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
