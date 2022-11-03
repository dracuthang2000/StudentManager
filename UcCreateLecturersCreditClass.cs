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

namespace StudentManagement
{
    public partial class UcCreateLecturersCreditClass : DevExpress.XtraEditors.XtraUserControl
    {
        bool isInsert = false;
        private LopTinChiDAL lopTinChiDAL;
        private GiangVienDAL giangVienDAL;
        private MonHocDAL monHocDAL;
        private SUndo undo;
        private bool stateUndo;
        private List<GIANGVIEN> lstGiangVien;
        private List<LOPTINCHI> lstLopTC;
        private KYNIENKHOA kyNienKhoa;
        private GIANGVIEN gvInitnew = new GIANGVIEN();
        private bool selectGvDelete = false;
        public UcCreateLecturersCreditClass()
        {
            InitializeComponent();
            lopTinChiDAL = new LopTinChiDAL();
            giangVienDAL = new GiangVienDAL();
            monHocDAL = new MonHocDAL();
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
            if(gvGiangVien1.GetFocusedRowCellValue("MALTC")!=null 
                && gvGiangVien1.GetFocusedRowCellValue("MAMH")!=null
                && gvGiangVien1.GetFocusedRowCellValue("NHOM") != null
                && gvGiangVien1.GetFocusedRowCellValue("MAGV")!=null 
                && gvGiangVien1.GetFocusedRowCellValue("MANK")!=null
                && selectGvDelete)
            {
                lstGiangVien.RemoveAll(gv => gv.MAMH == gvGiangVien1.GetFocusedRowCellValue("MAMH").ToString() 
                && gv.NHOM == (int)gvGiangVien1.GetFocusedRowCellValue("NHOM")
                && gv.MAGV == gvGiangVien1.GetFocusedRowCellValue("MAGV").ToString() 
                && gv.MANK == gvGiangVien1.GetFocusedRowCellValue("MANK").ToString());
                gvGiangVien1.DeleteSelectedRows();
            }
            else if(selectGvDelete==false)
            {

                int maltc = int.Parse(gvCreditClass.GetRowCellValue(GetSelelectRow(), "MALTC").ToString());
                var res = lopTinChiDAL.CheckLopTinChi(maltc);


                if (res.Response.State == ResponseState.Fail)
                {
                    // notify error
                }

                if (res.Data)
                {
                    MessageBox.Show("Không thế xóa lớp tín chỉ","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                else
                {
                    //  notify error
                    if (GetSelelectRow() != -1)
                    {
                        LOPTINCHI lOPTINCHI = (LOPTINCHI)gvCreditClass.GetRow(GetSelelectRow());
                        undo.Push(new ActionUndo(3, GetSelelectRow(), lOPTINCHI), new ActionUndo(2, GetSelelectRow(), null));
                        gvCreditClass.DeleteSelectedRows();
                        lstGiangVien.RemoveAll(x => x.MALTC == maltc);
                        if (gvCreditClass.GetFocusedRowCellValue("MALTC") != null)
                        {
                            gvGiangVien.DataSource = new BindingList<GIANGVIEN>(lstGiangVien.
                            Where(gv => gv.MAMH == gvCreditClass.GetFocusedRowCellValue("MAMH").ToString() 
                            && gv.NHOM == (int)gvCreditClass.GetFocusedRowCellValue("NHOM")
                            && gv.MANK == gvCreditClass.GetFocusedRowCellValue("MANK").ToString()
                            ).ToList());
                        }
                        else
                        {
                            gvGiangVien.DataSource = null;
                        }  
                        return;

                    }
                }
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
            gvGiangVien.DataSource = null;
            gvGiangVien.Enabled = false;
            var res = lopTinChiDAL.GetListLopTinChiKhongGV(nienKhoa, hocKy);
            lstLopTC = lopTinChiDAL.GetListLopTinChiKhongGV(nienKhoa, hocKy).Data;
            kyNienKhoa = new KyNienKhoaDAL().GetNienKhoaByAny(nienKhoa, hocKy).Data;
            if (res.Response.State == ResponseState.Fail)
            {
                // Notify error
            }

            lstGiangVien = new GiangVienDAL().GetListGiangVienLopTC().Data;
            gcCreditClass.DataSource = new BindingList<LOPTINCHI>(res.Data);
            //gcCreditClass.DataSource = res.Data;
            gvCreditClass.FocusInvalidRow();
            rilkMAMH.DataSource = new MonHocDAL().GetListMonHocKeHoach(nienKhoa,hocKy).Data;
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
            if(gvCreditClass.RowCount >0)
            {
                gvCreditClass.SetRowCellValue(0, "MALTC", gvCreditClass.GetRowCellValue(0, "MALTC"));
            }    
            List<UpdateLopTinChi> listUpdate;
            List<GIANGVIENLTC> listUpdateGVLTC = lstGiangVien.Select(x=> new GIANGVIENLTC(x)).ToList();
            var binding = (BindingList<LOPTINCHI>)gvCreditClass.DataSource;
            listUpdate =  binding.ToList().Select(x => new UpdateLopTinChi(x)).ToList();
            string nienKhoa = bESchoolYear.EditValue as string;
            int hocky = Convert.ToInt32( bESemester.EditValue);
            var res = lopTinChiDAL.UpdateLopTinChi(listUpdate,listUpdateGVLTC,nienKhoa,hocky);
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
            if (e.NewValue == null)
                return;
            MONHOC mONHOC = ((List<MONHOC>)rilkMAMH.DataSource).FirstOrDefault(x => x.MAMH == e.NewValue.ToString());
            if (mONHOC == null)
                return;
            var binding = (BindingList<LOPTINCHI>)gvCreditClass.DataSource;
            gvCreditClass.SetRowCellValue(GetSelelectRow(), "TENMH", mONHOC.TENMH);
            //gvCreditClass.SetRowCellValue(GetSelelectRow(), "MANV", Program.login);
            gvCreditClass.SetRowCellValue(GetSelelectRow(), "MANV", "PGV01");
            if (gvCreditClass.GetRowCellValue(GetSelelectRow(), "MALTC") != null && gvCreditClass.GetRowCellValue(GetSelelectRow(), "MALTC").ToString().Equals("0"))
            {
                lstGiangVien.RemoveAll(x => x.MALTC.ToString() == gvCreditClass.GetRowCellValue(GetSelelectRow(), "MALTC").ToString()
                && x.MAMH == mONHOC.MAMH && x.NHOM == Int32.Parse(gvCreditClass.GetRowCellValue(GetSelelectRow(), "NHOM").ToString())
                && x.MANK == gvCreditClass.GetRowCellValue(GetSelelectRow(), "MANK").ToString());
                var gvTemp = new GiangVienDAL().GetListGiangVienKhaNangGiangMonHoc(mONHOC.MAMH).Data;
                grkGiangVien.DataSource = gvTemp;
                List<GIANGVIEN> gvs = new List<GIANGVIEN>();
                gvTemp[0].MALTC = 0;
                gvTemp[0].MAMH = mONHOC.MAMH;
                gvTemp[0].MANK = kyNienKhoa.MANK;
                gvs.Add(gvTemp[0]);
                lstGiangVien.Add(gvTemp[0]);
                gvInitnew.MAMH = mONHOC.MAMH;
                gvTemp[0].NHOM = Int32.Parse(gvCreditClass.GetRowCellValue(GetSelelectRow(), "NHOM").ToString());
                gvInitnew.NHOM = Int32.Parse(gvCreditClass.GetRowCellValue(GetSelelectRow(), "NHOM").ToString());
                gvGiangVien.DataSource = new BindingList<GIANGVIEN>(gvs);
            }
            else
            {
                var gvTemp = new GiangVienDAL().GetListGiangVienKhaNangGiangMonHoc(mONHOC.MAMH).Data;
                grkGiangVien.DataSource = gvTemp;
                if (gvCreditClass.GetRowCellValue(GetSelelectRow(), "MALTC") != null)
                {
                    gvTemp[0].MALTC = Int32.Parse(gvCreditClass.GetRowCellValue(GetSelelectRow(), "MALTC").ToString());
                    lstGiangVien.RemoveAll(x => x.MALTC.ToString() == gvCreditClass.GetRowCellValue(GetSelelectRow(), "MALTC").ToString() 
                    && x.MAMH == mONHOC.MAMH);
                }
                else if (gvCreditClass.GetRowCellValue(GetSelelectRow(), "MALTC") == null)
                {
                    gvTemp[0].MALTC = 0;
                    gvTemp[0].MAMH = mONHOC.MAMH;
                    gvTemp[0].NHOM = Int32.Parse(gvCreditClass.GetRowCellValue(GetSelelectRow(), "NHOM").ToString());
                    lstGiangVien.RemoveAll(x => x.MALTC.ToString() == gvTemp[0].MALTC.ToString() && x.MAMH == mONHOC.MAMH 
                    && x.NHOM == Int32.Parse(gvCreditClass.GetRowCellValue(GetSelelectRow(), "NHOM").ToString())
                    && x.MANK == gvCreditClass.GetRowCellValue(GetSelelectRow(), "MANK").ToString());
                }
                gvTemp[0].MANK = kyNienKhoa.MANK;
                lstGiangVien.Add(gvTemp[0]);
                List<GIANGVIEN> gvs = new List<GIANGVIEN>();
                gvs.Add(gvTemp[0]);
                gvInitnew.MAMH = mONHOC.MAMH;
                gvInitnew.NHOM = Int32.Parse(gvCreditClass.GetRowCellValue(GetSelelectRow(), "NHOM").ToString());
                gvGiangVien.DataSource = new BindingList<GIANGVIEN>(gvs);
            }
        }

        private void gcCreditClass_Click(object sender, EventArgs e)
        {
            gvGiangVien.Enabled = true;
            selectGvDelete = false;
            LOPTINCHI ltc = (LOPTINCHI)gvCreditClass.GetRow(GetSelelectRow());
            if (ltc != null)
            {
                grkGiangVien.DataSource = new GiangVienDAL().GetListGiangVienKhaNangGiangMonHoc(ltc.MAMH).Data;
                if(ltc.MALTC != 0)
                {
                    gvGiangVien.DataSource = new BindingList<GIANGVIEN>(lstGiangVien.Where(x => x.MALTC == ltc.MALTC && x.MAMH == ltc.MAMH).ToList());
                }
                else
                {
                    gvGiangVien.DataSource = new BindingList<GIANGVIEN>(lstGiangVien.Where(x => x.MALTC == ltc.MALTC && x.MAMH == ltc.MAMH && ltc.NHOM == x.NHOM).ToList());
                }
                gvInitnew.MAMH = ltc.MAMH;
                gvInitnew.MALTC = ltc.MALTC;
                gvInitnew.NHOM = ltc.NHOM;
            }
            else
            {
                gvGiangVien.DataSource = null;
            }
        }

        private void gvCreditClass_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            Console.WriteLine('A');
            GridView view = sender as GridView;
            if (e.Column.FieldName == "NHOM")
            {
                var binding = (BindingList<GIANGVIEN>)gvGiangVien1.DataSource;
                if (binding != null)
                {
                    for (int i = 0; i < binding.ToList().Count; i++)
                    {
                        binding.ToList()[i].NHOM = Int32.Parse(view.GetRowCellValue(e.RowHandle, "NHOM").ToString());
                    }
                }
         
            }
        }

        private void rilkMAGV_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
           if (e.NewValue == null)
                return;
            GIANGVIEN gIANGVIEN = ((List<GIANGVIEN>)rilkMAGV.DataSource).FirstOrDefault(x => x.MAGV == e.NewValue.ToString());
            if (gIANGVIEN == null)
                return;
            gvCreditClass.SetRowCellValue(gvCreditClass.FocusedRowHandle, "HOTEN", gIANGVIEN.HOTEN);
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
            if (e.Column.FieldName == "TENGV" || e.Column.FieldName == "TENMH" || e.Column.FieldName == "MALTC")
                return;

            if (stateUndo)
                return;
            GridView view = sender as GridView;
            GridCell gc = new GridCell(view.GetDataSourceRowIndex(e.RowHandle), e.Column);
            var value = (sender as GridView).GetRowCellValue(gc.RowHandle, gc.Column.FieldName);
            ActionUndo action = new ActionUndo(1, gc, value);
            undo.Push(action, new ActionUndo(1, gc, e.Value));
        }

        private void gvCreditClass_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            GridView gridView = sender as GridView;
            if(gridView.GetRowCellValue(e.RowHandle,"NHOM") == null || (int)gridView.GetRowCellValue(e.RowHandle, "NHOM") == 0)
            {
                e.ErrorText = "Nhóm không được để trống hoặc bằng 0";
                e.Valid = false;
            }
            if (gridView.GetRowCellValue(e.RowHandle, "SOSVTOITHIEU") == null || (int)gridView.GetRowCellValue(e.RowHandle, "SOSVTOITHIEU") == 0)
            {
                e.ErrorText = "Số sinh viên tối thiểu không được để trống hoặc bằng 0";
                e.Valid = false;
            }
            if (gridView.GetRowCellValue(e.RowHandle, "MAMH") == null )
            {
                e.ErrorText = "Mã môn học không được để trống";
                e.Valid = false;
            }
            /*if (gridView.GetRowCellValue(e.RowHandle, "MAGV") == null)
            {
                e.ErrorText = "Max giảng viên không được để trống";

                e.Valid = false;
            }*/


            if(gridView.GetRowCellValue(e.RowHandle, "NHOM") != null
                && gridView.GetRowCellValue(e.RowHandle, "MAMH") != null)
            {
                var binding = (BindingList<LOPTINCHI>)gvCreditClass.DataSource;
                var listUpdate = binding.ToList().Select(x => new UpdateLopTinChi(x)).ToList();
                if(listUpdate.Where(x => x.NHOM == (int)(gridView.GetRowCellValue(e.RowHandle, "NHOM") ) &&
                 x.MAMH == gridView.GetRowCellValue(e.RowHandle, "MAMH").ToString()).Count() >=2)
                    {
                    e.ErrorText = "Môn học và nhóm đã được đăng kí";

                    e.Valid = false;
                }
            }    
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
            gvGiangVien1.SetRowCellValue(gvGiangVien1.FocusedRowHandle, "HOTEN", gIANGVIEN.HOTEN);
            gvInitnew.MAGV = gIANGVIEN.MAGV;
            gvInitnew.HOTEN = gIANGVIEN.HOTEN;
        }

        private void gvGiangVien1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            List<GIANGVIEN> lstGVTemp = new GiangVienDAL().GetListAllGiangVien().Data;
            view.SetRowCellValue(e.RowHandle, view.Columns["HOTEN"], gvInitnew.HOTEN);
            view.SetRowCellValue(e.RowHandle, view.Columns["MALTC"], gvInitnew.MALTC);
            view.SetRowCellValue(e.RowHandle, view.Columns["MAMH"], gvInitnew.MAMH);
            view.SetRowCellValue(e.RowHandle, view.Columns["NHOM"], gvInitnew.NHOM);
            view.SetRowCellValue(e.RowHandle, view.Columns["MANK"], kyNienKhoa.MANK);
        }

        private void gvGiangVien1_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            GridView view = sender as GridView;
            var binding = (BindingList<GIANGVIEN>)gvGiangVien1.DataSource;
            if (view.GetRowCellValue(e.RowHandle, "MAGV") != null)
            {
                var listUpdate = binding.ToList();
                if (listUpdate.Where(x =>x.MAGV == view.GetRowCellValue(e.RowHandle, "MAGV").ToString()).Count() >= 2)
                {
                    e.ErrorText = "Giảng viên đã được đăng kí";
                    e.Valid = false;
                }
                else
                {
                    GIANGVIEN gv = (GIANGVIEN)view.GetRow(e.RowHandle);
                    if (gv != null)
                    {
                        lstGiangVien.RemoveAll(x => x.MALTC.ToString() == gv.MALTC.ToString() && x.MAMH.ToString() == gv.MAMH.ToString() && x.MANK == gv.MANK);
                        foreach (var item in binding.ToList())
                        {
                            lstGiangVien.Add(item);
                        }
                    }

                }
            }     
        }

        private void gvGiangVien_Click(object sender, EventArgs e)
        {
            selectGvDelete = true;
        }
    }
}
