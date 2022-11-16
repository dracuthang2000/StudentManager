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
    public partial class UcCreateSubject : DevExpress.XtraEditors.XtraUserControl
    {
        bool isInsert = false;
        private LopDAL lopDAL;
        private LopTinChiDAL lopTinChiDAL;
        private GiangVienDAL giangVienDAL;
        private MonHocDAL monHocDAL;
        private SUndo undo;
        private List<GIANGVIEN> lstGiangVien;
        private bool stateUndo;
        private string maMhIsChanged;
        private string initHoTen;
        private bool selectGvDelete = false;
        public UcCreateSubject()
        {
            InitializeComponent();
            lopDAL = new LopDAL();
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




            bESemester.EditValue = 1;
            gcGiangVien.Enabled = false;

            LoadData();
            //lkTeacher.selected
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
            gvSubject.ClearSelection();
            this.dSSPCreditClass.SuspendBinding();
            gvSubject.FocusInvalidRow();
            isInsert = true;

        }

        private void bEdelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GetSelelectRow() == -1)
                return;
            if(gvGiangVien.GetFocusedRowCellValue("MAGV") != null
                && gvGiangVien.GetFocusedRowCellValue("MAMH") != null && selectGvDelete)
            {
                var checkGVDay = giangVienDAL.CheckGiangVienDayLTC(gvGiangVien.GetFocusedRowCellValue("MAGV").ToString(), gvGiangVien.GetFocusedRowCellValue("MAMH").ToString()).Data;
                if (!checkGVDay)
                {
                    lstGiangVien.RemoveAll(gv => gv.MAMH.Trim() == gvGiangVien.GetFocusedRowCellValue("MAMH").ToString().Trim()
                     && gv.MAGV.Trim() == gvGiangVien.GetFocusedRowCellValue("MAGV").ToString().Trim());
                    gvGiangVien.DeleteSelectedRows();
                    MessageBox.Show("Xóa giảng viên thành công!", "DELETE SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Xóa thất bại. Mã giảng viên này đã được dùng ở nơi nào đó rồi", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (!selectGvDelete)
            {
                string mamh = gvSubject.GetRowCellValue(GetSelelectRow(), "MAMH").ToString();
                var res = monHocDAL.CheckMonHoc(mamh);
                if (res.Response.State == ResponseState.Fail)
                {
                    // notify error
                }
                if (res.Data)
                {
                    //  notify error
                    if (GetSelelectRow() != -1)
                    {
                        MONHOC monhoc = (MONHOC)gvSubject.GetRow(GetSelelectRow());
                        undo.Push(new ActionUndo(3, GetSelelectRow(), monhoc), new ActionUndo(2, GetSelelectRow(), null));
                        gvSubject.DeleteSelectedRows();
                        lstGiangVien.RemoveAll(gv => gv.MAMH.Trim() == monhoc.MAMH.Trim());
                        gcGiangVien.DataSource = null;
                        MessageBox.Show("Xóa thành công!", "DELETE SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Xóa thất bại. Mã môn học này đã được dùng ở nơi nào đó rồi", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }


        private void bELoadData_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

                LoadData();
        }

        private void LoadData()
        {

            var res = monHocDAL.GetListMonHoc();
            lstGiangVien = giangVienDAL.GetListGiangVienKhaNangGiangMonHoc().Data;
            if (res.Response.State == ResponseState.Fail)
            {
                // Notify error
            }

            gcSubject.DataSource = new BindingList<MONHOC>(res.Data);
            grkGiangVien.DataSource = giangVienDAL.GetListAllGiangVien().Data;
            //gcCreditClass.DataSource = res.Data;
            gvSubject.FocusInvalidRow();
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
            GridView view = sender as GridView;
        }

        private int GetSelelectRow()
        {
            int[] rows = gvSubject.GetSelectedRows();
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
            DialogResult d;
            d = MessageBox.Show("Bạn có chắc là muốn lưu không?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (d == DialogResult.Yes)
            {
                gvSubject.FocusInvalidRow();
                List<UPDATEMONHOC> listUpdate;
                List<KHANANGGIANG> lstKNG = lstGiangVien.Select(gv => new KHANANGGIANG(gv)).ToList();
                var binding = (BindingList<MONHOC>)gvSubject.DataSource;
                listUpdate = binding.ToList().Select(x => new UPDATEMONHOC(x)).ToList();
                var res = monHocDAL.UpdateMonHoc(listUpdate,lstKNG);
                if (res.Response.State == ResponseState.Fail)
                {
                    MessageBox.Show("Lưu thất bại","Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Program.formMain.Notify("Lưu thành công");
                }
            }

        }



        private void gcCreditClass_Click(object sender, EventArgs e)
        {
            GridView view = sender as GridView;
            gcGiangVien.Enabled = true;
            selectGvDelete = false;
            MONHOC mh = (MONHOC)gvSubject.GetRow(GetSelelectRow());
            if (mh == null)
            {
                gcGiangVien.DataSource = null;
                return;
            }
            gcGiangVien.DataSource = new BindingList<GIANGVIEN>(lstGiangVien.Where(gv => gv.MAMH.Trim() == mh.MAMH.Trim()).ToList());
            maMhIsChanged = mh.MAMH;
        }

        private void gvCreditClass_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            if(e.Column.FieldName == "MAMH")
            {
                maMhIsChanged = view.GetRowCellValue(e.RowHandle, "MAMH").ToString();
            }
        }

        private void gvCreditClass_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            if (stateUndo)
                return;
            undo.Push(new ActionUndo(2, gvSubject.RowCount, null), new ActionUndo(3, GetSelelectRow(), new LOPTINCHI()));
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
                    gvSubject.SetRowCellValue(gvSubject.GetRowHandle(cell.RowHandle), cell.Column, action.value);
                    Console.WriteLine(gvSubject.GetRowCellValue(gvSubject.GetRowHandle(cell.RowHandle), cell.Column));
                    gvSubject.FocusedRowHandle = cell.RowHandle;
                    break;
                case 2:
                    int row = (int)action.obj;
                    gvSubject.DeleteRow(row - 1);
                    break;
                case 3:

                    List<MONHOC> mONHOCs = (gvSubject.DataSource as BindingList<MONHOC>).ToList();
                    mONHOCs.Insert(int.Parse(action.obj.ToString()), action.value as MONHOC);

                    gcSubject.DataSource = new BindingList<MONHOC>(mONHOCs);
                    gvSubject.FocusedRowHandle = int.Parse(action.obj.ToString());
                    break;
            }
            stateUndo = false;
        }

        private void gvCreditClass_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "MAMH")
                return;

            if (stateUndo)
                return;
            GridView view = sender as GridView;
            GridCell gc = new GridCell(view.GetDataSourceRowIndex(e.RowHandle), e.Column);
            var value = (sender as GridView).GetRowCellValue(gc.RowHandle, gc.Column.FieldName);
            ActionUndo action = new ActionUndo(1, gc, value);
            undo.Push(action, new ActionUndo(1, gc, e.Value));
        }

        private void gvCreditClass_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            GridView gridView = sender as GridView;
            if (gridView.FocusedColumn.FieldName == "MAMH")
            {
                string MAMH = (e.Value.ToString());
                if (MAMH.Equals(""))
                {
                    e.Valid = false;
                    e.ErrorText = "Không để rỗng!";
                }
            }
            if (gridView.FocusedColumn.FieldName == "SOTIET_LT")
            {
                int SOTIET_LT = 0;
                if (!Int32.TryParse(e.Value as String, out SOTIET_LT))
                {
                    e.Valid = false;
                    e.ErrorText = "Chỉ nhập số";
                }else
                if (Int32.Parse(e.Value as String) < 0)
                {
                    e.Valid = false;
                    e.ErrorText = "Chỉ nhập số dương";
                }
            }

            if (gridView.FocusedColumn.FieldName == "SOTIET_TH")
            {
                int SOTIET_TH = 0;
                if (!Int32.TryParse(e.Value as String, out SOTIET_TH))
                {
                    e.Valid = false;
                    e.ErrorText = "Chỉ nhập số";
                }else
                if(Int32.Parse(e.Value as String) < 0)
                {
                    e.Valid = false;
                    e.ErrorText = "Chỉ nhập số dương";
                }
            }
            if (gridView.FocusedColumn.FieldName == "TENMH")
            {
                if (e.Value.Equals(""))
                {
                    e.Valid = false;
                    e.ErrorText = "Không để rỗng!";
                }
            }
        }

        private void gvCreditClass_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            GridView gridView = sender as GridView;
            if(gridView.GetRowCellValue(e.RowHandle, idSubject) == null)
            {
                gridView.SetColumnError(idSubject, "Mã môn học không được để trống");
                e.Valid = false;
            }
            else
            {
                string mamh = gridView.GetRowCellValue(e.RowHandle, idSubject).ToString();
                var binding = (BindingList<MONHOC>)gvSubject.DataSource;
                var listUpdate = binding.Where(x => x.MAMH.Trim() == mamh).Count();
                if(listUpdate >= 2)
                {
                    gridView.SetColumnError(idSubject, "Mã môn học bị trùng");
                    e.Valid = false;
                    e.ErrorText = "The value is not correct! ";
                }
               
            }

            if (gridView.GetRowCellValue(e.RowHandle, colSubjectName) == null)
            {
                gridView.SetColumnError(colSubjectName, "Tên môn học không được để trống");
                e.Valid = false;
                e.ErrorText = "The value is not correct! ";
            }
            if (gridView.GetRowCellValue(e.RowHandle, colLT) == null)
            {
                gridView.SetColumnError(colLT, "Lớp lý thuyết không được để trống");
                e.Valid = false;
                e.ErrorText = "The value is not correct! ";
            }
            if (gridView.GetRowCellValue(e.RowHandle, colTH) == null)
            {
                gridView.SetColumnError(colLT, "Lớp thực hành không được để trống");
                e.Valid = false;
                e.ErrorText = "The value is not correct! ";
            }

            if(gridView.GetRowCellValue(e.RowHandle, colTH) != null || gridView.GetRowCellValue(e.RowHandle, colLT) != null)
            {
                int LT = Int32.Parse(gridView.GetRowCellValue(e.RowHandle, colLT).ToString());
                int TH = Int32.Parse(gridView.GetRowCellValue(e.RowHandle, colTH).ToString());
                if ((LT + TH) % 15 != 0||(LT+TH)<=0)
                {
                    gridView.SetColumnError(colLT, "Số tiết lý  thuyết + Thực hành chia hết cho 15");
                    gridView.SetColumnError(colTH, "Số tiết lý  thuyết + Thực hành chia hết cho 15");
                    e.Valid = false;
                    e.ErrorText = "The value is not correct! ";
                }

            }
        }

        private void gvCreditClass_RowClick(object sender, RowClickEventArgs e)
        {
        
        }

        private void gvGiangVien_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            if (initHoTen == null || maMhIsChanged == null) return;
            view.SetRowCellValue(e.RowHandle, view.Columns["HOTEN"],initHoTen);
            view.SetRowCellValue(e.RowHandle, view.Columns["MAMH"], maMhIsChanged);
        }

        private void gvGiangVien_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            GridView view = sender as GridView;
            var binding = (BindingList<GIANGVIEN>)gvGiangVien.DataSource;
            if(view.GetRowCellValue(e.RowHandle, "MAGV") != null){
                var listUpdate = binding.ToList();
                if (listUpdate.Where(x => x.MAGV == view.GetRowCellValue(e.RowHandle, "MAGV").ToString()).Count() >= 2)
                {
                    e.ErrorText = "Giảng viên đã được đăng kí";
                    e.Valid = false;
                }
                else
                {
                    GIANGVIEN gv = (GIANGVIEN)view.GetRow(e.RowHandle);
                    if (gv != null)
                    {
                        lstGiangVien.RemoveAll(x=> x.MAMH.Trim() == maMhIsChanged.Trim());
                        foreach (var item in binding.ToList())
                        {
                            lstGiangVien.Add(item);
                        }
                    }

                }
            }
        }

        private void grkGiangVien_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null)
                return;
            GIANGVIEN gIANGVIEN = ((List<GIANGVIEN>)grkGiangVien.DataSource).FirstOrDefault(x => x.MAGV == e.NewValue.ToString());
            if (gIANGVIEN == null)
                return;
            gvGiangVien.SetRowCellValue(gvGiangVien.FocusedRowHandle, "HOTEN", gIANGVIEN.HOTEN);
            initHoTen = gIANGVIEN.HOTEN;
        }

        private void gcGiangVien_Click(object sender, EventArgs e)
        {
            selectGvDelete = true;
        }
    }
}
