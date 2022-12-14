using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using StudentManagement.Model;
using StudentManagement.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManagement
{
    public partial class ucUpdateGrade : DevExpress.XtraEditors.XtraUserControl
    {
        private LopTinChiDAL _lopTinChiDAL;
        private DangKyDAL _dangKyDAL;
        private SupportDAL _supportDAL;
        private bool isChange = false;

        public ucUpdateGrade()
        {
            InitializeComponent();
            InitialSchoolYear();
            _lopTinChiDAL = new LopTinChiDAL();
            _dangKyDAL = new DangKyDAL();
            beSemester.EditValue = 1;



            _supportDAL = new SupportDAL();
            lkFaculty.DataSource = Program.servers;

            lkFaculty.DisplayMember = "TENCN";
            lkFaculty.ValueMember = "TENSERVER";
            lkFaculty.PopulateColumns();
            lkFaculty.Columns["TENSERVER"].Visible = false;
            beFaculty.EditValue = Program.serverName;


            if (Program.group == Role.KHOA)
                beFaculty.Enabled = false;
        }
        public void InitialSchoolYear()
        {
            DateTime now = DateTime.Now;
            int start = now.AddYears(-15).Year;
            int end = now.AddYears(15).Year;
            for (int year = start; year <= end; year++)
            {
                string schoolYear = String.Format("{0}-{1}", year, year + 1);
                cbxSchoolYear.Items.Add(schoolYear);
            }

            if (now.Month < 9)
                now.AddYears(-1);
            beSchoolYear.EditValue = String.Format("{0}-{1}", now.Year, now.Year + 1);
        }

        private void beLoadData_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string nienKhoa = beSchoolYear.EditValue as string;
            int hocKy = Convert.ToInt32(beSemester.EditValue);

            var res = _lopTinChiDAL.GetListLopTinChiActive(nienKhoa, hocKy);
            if (res.Response.State == ResponseState.Fail)
            {
                return;
            }
            var index1 = res.Data.Count;
            var temp = new List<LOPTINCHI>();
            for (int i = 0; i < index1; i++)
            {
                for (int j = i + 1; j < index1; j++)
                {
                    if (res.Data[j].MALTC.Equals(0) || res.Data[i].MALTC.Equals(0))
                    {
                        break;
                    }
                    if (res.Data[j].MALTC == res.Data[i].MALTC)
                    {
                        res.Data[i].TENGV = res.Data[i].TENGV + "," + res.Data[j].TENGV;
                        res.Data[i].MAGV = res.Data[i].MAGV + ", " + res.Data[j].MAGV;
                        res.Data[j] = new LOPTINCHI();
                    }
                }
                if (!(res.Data[i].MALTC.Equals(0)))
                {
                    temp.Add(res.Data[i]);
                }
            }
            res.Data = temp;
            gcCreditClass.DataSource = res.Data;

        }

        private void gvUpdateGrade_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
           
            

        }

        private void gvCreditClass_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
           

          
        }
        private int GetSelelectRow()
        {
            int[] rows = gvCreditClass.GetSelectedRows();
            if (rows.Length == 0)
                return -1;
            return rows[0];
        }

        private void gvCreditClass_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {

        }

        private void gvCreditClass_FocusedRowChanged_1(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gvCreditClass.IsValidRowHandle(e.FocusedRowHandle) && gvCreditClass.GetFocusedRow() != null)
            {
                // DANGKY dANGKY = gvCreditClass.get
                int maltc = (int)gvCreditClass.GetRowCellValue(e.FocusedRowHandle, "MALTC");
                var res = _dangKyDAL.GetListDKByMALTC(maltc);
                if (res.Response.State == ResponseState.Fail)
                {
                    return;
                }
                res.Data.ForEach(x => x.CaculatorDiemHM());
                gcUpdateGrade.DataSource = res.Data;
            }
        }

        private void beSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gvUpdateGrade.RowCount > 0)
            {
                gvUpdateGrade.SetRowCellValue(0, "DIEM_CK", gvUpdateGrade.GetRowCellValue(0, "DIEM_CK"));
            }

            List<DANGKY> listDangky = (List<DANGKY>)gvUpdateGrade.DataSource;
            if (listDangky == null)
                return;
            List<UpdateGrade> listUpdateGrade = listDangky.Select(x => new UpdateGrade(x)).ToList();
            var res = _dangKyDAL.UpdateDiem(listUpdateGrade);
            if(res.Response.State == ResponseState.Success)
            {
                Program.formMain.Notify("Lưu thành công");
            }else
            {
                MessageBox.Show("Lưu thông tin thất bại");
            }    

        }

        private void beFaculty_EditValueChanged(object sender, EventArgs e)
        {
            Program.currentServer = (string)beFaculty.EditValue;
        }

        private void gvUpdateGrade_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (gvUpdateGrade.GetRowCellValue(e.RowHandle, e.Column) == null)
                return;
            if(isChange == true)
            {
                isChange = false;
                return;
            }
            if(e.Column.FieldName ==  "DIEM_GK" || e.Column.FieldName == "DIEM_CK")
            {
                float score;
                if (!float.TryParse(gvUpdateGrade.GetRowCellValue(e.RowHandle, e.Column).ToString(), out score))
                    return;
                

                float odd = score - (float)Math.Floor(score);
                if(odd < 0.25)
                {
                    score += 0;
                }else
                {
                    if (odd < 0.75)
                        score = (float)Math.Floor(score)+ (float)0.5;
                    else
                        score = (float)Math.Floor(score) + 1;
                }

                isChange = true;
                gvUpdateGrade.SetRowCellValue(e.RowHandle, e.Column, score);
                
            }
            if (e.Column.FieldName == "DIEM_GK" || e.Column.FieldName == "DIEM_CK" || e.Column.FieldName == "DIEM_CC")
            {
                if (gvUpdateGrade.GetRowCellValue(e.RowHandle, "DIEM_CC") != null
                          && gvUpdateGrade.GetRowCellValue(e.RowHandle, "DIEM_CC") != null
                          && gvUpdateGrade.GetRowCellValue(e.RowHandle, "DIEM_CC") != null)
                {
                    float diem_cc = float.Parse(gvUpdateGrade.GetRowCellValue(e.RowHandle, "DIEM_CC").ToString());
                    float diem_gk = float.Parse(gvUpdateGrade.GetRowCellValue(e.RowHandle, "DIEM_GK").ToString());
                    float diem_ck = float.Parse(gvUpdateGrade.GetRowCellValue(e.RowHandle, "DIEM_CK").ToString());
                    gvUpdateGrade.SetRowCellValue(e.RowHandle, "DIEM_HM", diem_cc * 0.1 + diem_gk * 0.3 + diem_ck * 0.6);
                }
            }
          

        }

        private void gvUpdateGrade_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            GridView gridView = sender as GridView;
            if (gridView.GetRowCellValue(e.RowHandle, "DIEM_GK") !=null)
            {
                if(float.Parse(gridView.GetRowCellValue(e.RowHandle, "DIEM_GK").ToString()) < 0 || float.Parse(gridView.GetRowCellValue(e.RowHandle, "DIEM_GK").ToString()) > 10)
                {
                    e.ErrorText = "Điểm giữa kì >=0 và <=10";
                    e.Valid = false;
                }                  
            }

            if (gridView.GetRowCellValue(e.RowHandle, "DIEM_CC") != null)
            {
                if (float.Parse(gridView.GetRowCellValue(e.RowHandle, "DIEM_CC").ToString()) < 0 || float.Parse(gridView.GetRowCellValue(e.RowHandle, "DIEM_CC").ToString()) > 10)
                {
                    e.ErrorText = "Điểm chuyên cần >=0 và <=10";
                    e.Valid = false;
                }
            }

            if (gridView.GetRowCellValue(e.RowHandle, "DIEM_CK") != null)
            {
                if (float.Parse(gridView.GetRowCellValue(e.RowHandle, "DIEM_CK").ToString()) < 0 || float.Parse(gridView.GetRowCellValue(e.RowHandle, "DIEM_CK").ToString()) > 10)
                {
                    e.ErrorText = "Điểm cuối kì >=0 và <=10";
                    e.Valid = false;
                }
            }
        }
    }
}
