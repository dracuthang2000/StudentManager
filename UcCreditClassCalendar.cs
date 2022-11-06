using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using StudentManagement.Model;
using StudentManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Linq;
using System.ComponentModel;
using StudentManagement.Undo;
using DevExpress.XtraGrid.Views.Base;
using StudentManagement.Factories;
using DevExpress.XtraSplashScreen;

namespace StudentManagement
{
    public partial class UcCreditClassCalendar : DevExpress.XtraEditors.XtraUserControl
    {
        bool isInsert = false;
        private LopTinChiDAL lopTinChiDAL;
        private GiangVienDAL giangVienDAL;
        private MonHocDAL monHocDAL;
        private SUndo undo;
        private bool stateUndo;
        private List<GIANGVIEN> lstGiangVien;
        private List<LOPTINCHI> lstLopTC;
        private List<LICHHOC> lstLichHoc;
        private LichHocDAL lichHocDAL;
        private LICHHOC initNewLichHoc = new LICHHOC();
        public UcCreditClassCalendar()
        {
            InitializeComponent();
            lopTinChiDAL = new LopTinChiDAL();
            giangVienDAL = new GiangVienDAL();
            monHocDAL = new MonHocDAL();
            lichHocDAL = new LichHocDAL();
            undo = new SUndo();
            stateUndo = false;


           

            SupportDAL connectionDAL = new SupportDAL();
            lkFaculty.DataSource = Program.servers;

            lkFaculty.DisplayMember = "TENCN";
            lkFaculty.ValueMember = "TENSERVER";
            lkFaculty.PopulateColumns();
            lkFaculty.Columns["TENSERVER"].Visible = false;
            bEFaculty.EditValue = Program.serverName;

            if (Program.group == Role.KHOA)
                bEFaculty.Enabled = false;


            InitialSchoolYear();
            bESemester.EditValue = 1;


            LoadData(); 
            //lkTeacher.selected
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
            bESchoolYear.EditValue = String.Format("{0}-{1}", now.Year, now.Year + 1);
        }

        private void bESemester_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //this.tASPCreditClass.Connection.ConnectionString = Program.conmStr;
            
        }

        private void bEFaculty_EditValueChanged(object sender, EventArgs e)
        {
            Program.currentServer = bEFaculty.EditValue as string;
            SQLFactory.SetCurrentServer(bEFaculty.EditValue as string);
        }

        private void bEAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gvCreditClass.ClearSelection();
            this.dSSPCreditClass.SuspendBinding();
            gvCreditClass.FocusInvalidRow();
            isInsert = true;
           
        }

        private void bEdelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GetSelelectRow() == -1 || gvCreditClass.GetRowCellValue(GetSelelectRow(), "MALTC") == null)
                return;
            if (gvLichHoc.GetFocusedRowCellValue("MALH") != null && gvLichHoc.GetFocusedRowCellValue("MALTC") != null)
            {
                lstLichHoc.RemoveAll(lh => lh.MALH == (int)gvLichHoc.GetFocusedRowCellValue("MALH") && lh.MALTC == (int)gvLichHoc.GetFocusedRowCellValue("MALTC"));
                gvLichHoc.DeleteSelectedRows();
            }
        }

       
        private void bELoadData_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            LoadData();
        }
        private void LoadData()
        {
            string nienKhoa = bESchoolYear.EditValue.ToString();
            int hocKy = int.Parse(bESemester.EditValue.ToString());
            gcLichHoc.DataSource = null;
            gcLichHoc.Enabled = false;
            var res = lopTinChiDAL.GetListLopTinChiKhongGV(nienKhoa, hocKy);
            lstLopTC = lopTinChiDAL.GetListLopTinChiKhongGV(nienKhoa, hocKy).Data;
            lstLichHoc = lichHocDAL.GetChiTietLichHoc().Data;
            if (res.Response.State == ResponseState.Fail)
            {
                // Notify error
            }

            lstGiangVien = new GiangVienDAL().GetListGiangVienLopTC().Data;
            gcCreditClass.DataSource = new BindingList<LOPTINCHI>(res.Data);
            //gcCreditClass.DataSource = res.Data;
            gvCreditClass.FocusInvalidRow();
            rilkMAMH.DataSource = new MonHocDAL().GetListMonHocKeHoach(nienKhoa,hocKy).Data;
            grkLichHoc.DataSource = new LichHocDAL().GetLichHoc().Data;
        }

        private void bESchoolYear_EditValueChanged(object sender, EventArgs e)
        {
            //tbxSchoolYear.EditValue = bESchoolYear.EditValue;
            
        }

        private void bESemester_EditValueChanged(object sender, EventArgs e)
        {
            //nmuSemester.EditValue = bESemester.EditValue;
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

      

        private void ckCancel_CheckedChanged(object sender, EventArgs e)
        {
            if (isInsert == true)
                return;
            int row = GetSelelectRow();
            if (row == -1)
                return;

          
        }

        private void bESave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            List<UPDATE_CHITIET_LH> listUpdate;
            listUpdate =  lstLichHoc.Select(x => new UPDATE_CHITIET_LH(x)).ToList();
            var res = lichHocDAL.UpdateCTLichHoc(listUpdate);
            if (res.Response.State == ResponseState.Fail)
            {
                // Notify error
            } else
            {
                // notify susscess
                Program.formMain.Notify("Lưu thành công");
                LoadData();
            }


        }

       
        private void rilkMAMH_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            
        }

        private void gcCreditClass_Click(object sender, EventArgs e)
        {
            gcLichHoc.Enabled = true;
            LOPTINCHI ltc = (LOPTINCHI)gvCreditClass.GetRow(GetSelelectRow());
            List<LOPTINCHI> ltcTemp;
            if (ltc != null)
            {
                gcLichHoc.DataSource = new BindingList<LICHHOC>(lstLichHoc.Where(lh => lh.MALTC == ltc.MALTC).ToList());
                initNewLichHoc.MALTC = ltc.MALTC;
            }
            else
            {
                gcLichHoc.DataSource = null;
            }
           
        }

        private void gvCreditClass_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            Console.WriteLine('A');
        }


        private void gvCreditClass_InitNewRow(object sender, InitNewRowEventArgs e)
        {

            if (stateUndo)
                return;
            undo.Push(new ActionUndo(2, gvCreditClass.RowCount, null), new ActionUndo(3, GetSelelectRow(), new LOPTINCHI()));
        }

        private void bEUndo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
            ChangeUndoAction(undo.Before());
        }

        private void beRedo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ChangeUndoAction(undo.After());
        }
        void ChangeUndoAction(ActionUndo action)
        {
            if (action == null)
                return;
            stateUndo = true;
            switch (action.type)
            {
                case 1:
                    GridCell cell = action.obj as GridCell;
                    gvCreditClass.SetRowCellValue(gvCreditClass.GetRowHandle(cell.RowHandle), cell.Column, action.value);
                    Console.WriteLine(gvCreditClass.GetRowCellValue(gvCreditClass.GetRowHandle(cell.RowHandle), cell.Column));
                    gvCreditClass.FocusedRowHandle = cell.RowHandle;
                    break;
                case 2:
                    int row = (int)action.obj;
                    gvCreditClass.DeleteRow(row - 1);
                    break;
                case 3:

                    List<LOPTINCHI> LOPTINCHIs = (gvCreditClass.DataSource as BindingList<LOPTINCHI>).ToList();
                    LOPTINCHIs.Insert(int.Parse(action.obj.ToString()), action.value as LOPTINCHI);
                   
                    gcCreditClass.DataSource = new BindingList<LOPTINCHI>(LOPTINCHIs);
                    gvCreditClass.FocusedRowHandle = int.Parse(action.obj.ToString());
                    break;
            }
            stateUndo = false;
        }

        private void gvCreditClass_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
        }

        private void gvCreditClass_ValidateRow(object sender, ValidateRowEventArgs e)
        {
        }
        private void gridGV_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {

        }

        private void grkGiangVien_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null)
                return;
            GIANGVIEN gIANGVIEN = ((List<GIANGVIEN>)grkGiangVien.DataSource).FirstOrDefault(x => x.MAGV == e.NewValue.ToString());
            if (gIANGVIEN == null)
                return;
            gvLichHoc.SetRowCellValue(gvLichHoc.FocusedRowHandle, "HOTEN", gIANGVIEN.HOTEN);
        }

        private void grkLichHoc_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null)
                return;
            LICHHOC lichHoc = ((List<LICHHOC>)grkLichHoc.DataSource).FirstOrDefault(x => x.MALH == Int32.Parse(e.NewValue.ToString()));
            if (lichHoc == null)
                return;
            gvLichHoc.SetRowCellValue(gvLichHoc.FocusedRowHandle, "THU", lichHoc.THU);
            gvLichHoc.SetRowCellValue(gvLichHoc.FocusedRowHandle, "TBD", lichHoc.TBD);
            gvLichHoc.SetRowCellValue(gvLichHoc.FocusedRowHandle, "TKT", lichHoc.TKT);
            initNewLichHoc.THU = lichHoc.THU;
            initNewLichHoc.TBD = lichHoc.TBD;
            initNewLichHoc.TKT = lichHoc.TKT;
        }

        private void gvLichHoc_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            view.SetRowCellValue(e.RowHandle, view.Columns["THU"], initNewLichHoc.THU);
            view.SetRowCellValue(e.RowHandle, view.Columns["TBD"], initNewLichHoc.TBD);
            view.SetRowCellValue(e.RowHandle, view.Columns["TKT"], initNewLichHoc.TKT);
            view.SetRowCellValue(e.RowHandle, view.Columns["MALTC"], initNewLichHoc.MALTC);
        }

        private void gvLichHoc_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            GridView gridView = sender as GridView;
            var binding = (BindingList<LICHHOC>)gvLichHoc.DataSource;
            if (binding != null)
            {
                if(binding.ToList().Where(lh=>lh.MALH == (int)gridView.GetRowCellValue(e.RowHandle, "MALH")).Count() >= 2)
                {
                    e.ErrorText = "Lịch học đã đăng ký";
                    e.Valid = false;
                }
                else
                {
                    lstLichHoc.RemoveAll(lh=>binding.ToList()[0].MALTC == lh.MALTC);
                    binding.ToList().ForEach(x => lstLichHoc.Add(x));
                }
            }
        }

        private void gvCreditClass_MasterRowEmpty(object sender, MasterRowEmptyEventArgs e)
        {
            GridView view = sender as GridView;
            LOPTINCHI ltc = view.GetRow(e.RowHandle) as LOPTINCHI;
            if (ltc != null)
            {
                e.IsEmpty = !lstGiangVien.Any(gv => gv.MALTC == ltc.MALTC);
            }
        }

        private void gvCreditClass_MasterRowGetChildList(object sender, MasterRowGetChildListEventArgs e)
        {
            GridView view = sender as GridView;
            LOPTINCHI ltc = view.GetRow(e.RowHandle) as LOPTINCHI;
            if (ltc != null)
            {
                e.ChildList = lstGiangVien.Where(gv => gv.MALTC == ltc.MALTC).ToList();
            }
        }

        private void gvCreditClass_MasterRowGetRelationCount(object sender, MasterRowGetRelationCountEventArgs e)
        {
            e.RelationCount = 1;
        }

        private void gvCreditClass_MasterRowGetRelationName(object sender, MasterRowGetRelationNameEventArgs e)
        {
            e.RelationName = "Giảng viên";
        }
    }
}
