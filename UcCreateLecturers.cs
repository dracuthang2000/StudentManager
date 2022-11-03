﻿using DevExpress.XtraEditors;
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

namespace StudentManagement
{
    public partial class UcCreateLecturers : DevExpress.XtraEditors.XtraUserControl
    {
        bool isInsert = false;
        private LopTinChiDAL lopTinChiDAL;
        private SinhVienDAL sinhVienDAL;
        private GiangVienDAL giangVienDAL;
        private MonHocDAL monHocDAL;
        private SUndo undo;
        private LopDAL lopDAL;
        private bool stateUndo;
        private String MAKHOA;
        public UcCreateLecturers()
        {
            InitializeComponent();
            lopTinChiDAL = new LopTinChiDAL();
            giangVienDAL = new GiangVienDAL();
            lopDAL = new LopDAL();
            monHocDAL = new MonHocDAL();
            sinhVienDAL = new SinhVienDAL();
            undo = new SUndo();
            stateUndo = false;




            SupportDAL connectionDAL = new SupportDAL();
            lkFaculty.DataSource =  Program.servers;
            bEFaculty.EditValue = Program.serverName;

            if (Program.group == Role.KHOA)
                bEFaculty.Enabled = false;

            lkFaculty.DisplayMember = "TENCN";
            lkFaculty.ValueMember = "TENSERVER";
            lkFaculty.PopulateColumns();
            lkFaculty.Columns["TENSERVER"].Visible = false;
            




            InitialSchoolYear();
            bESemester.EditValue = 1;


            LoadData();
            //lkTeacher.selected
        }


        public void InitialSchoolYear()
        {
            cbxSchoolYear.Items.Clear();
            cbxSchoolYear.Items.Add("Tất cả");
            foreach (LOP lop in lopDAL.GetListLop().Data)
            {
                cbxSchoolYear.Items.Add(lop.MALOP.ToString());
            }
            bESchoolYear.EditValue = "Tất cả";
        }

        private void bESemester_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //this.tASPCreditClass.Connection.ConnectionString = Program.conmStr;

        }

        private void bEFaculty_EditValueChanged(object sender, EventArgs e)
        {
            Program.currentServer = bEFaculty.EditValue as string;
            InitialSchoolYear();
        }

        private void bEAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gvLecturers.ClearSelection();
            this.dSSPCreditClass.SuspendBinding();
            gvLecturers.FocusInvalidRow();
            isInsert = true;

        }

        private void bEdelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GetSelelectRow() == -1)
                return;

            string magv = gvLecturers.GetRowCellValue(GetSelelectRow(), "MAGV").ToString();
            var res = giangVienDAL.CheckGiangVien(magv);
            if (res.Response.State == ResponseState.Fail)
            {
                // notify error
            }

            if (res.Data)
            {
                //  notify error
                if (GetSelelectRow() != -1)
                {
                    GIANGVIEN GIANGVIENs = (GIANGVIEN)gvLecturers.GetRow(GetSelelectRow());
                    undo.Push(new ActionUndo(3, GetSelelectRow(), GIANGVIENs), new ActionUndo(2, GetSelelectRow(), null));
                    gvLecturers.DeleteSelectedRows();
                    return;

                }
            }
            else
            {              
            }


        }


        private void bELoadData_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            if (bESchoolYear.EditValue.ToString() != "Tất cả")
            {
                LoadDataByIdClass();
            }
            else
            {
                LoadData();
            }
        }
        private void LoadData()
        {
            List<KHOA> lstKhoa = giangVienDAL.GetListKhoa().Data;
            rilkKHOA.DataSource = lstKhoa;
            var res = giangVienDAL.GetListGiangVienByKhoa(lstKhoa[0].MAKHOA);
            MAKHOA = lstKhoa[0].MAKHOA;
            if (res.Response.State == ResponseState.Fail)
            {
                // Notify error
            }

            gcLecturers.DataSource = new BindingList<GIANGVIEN>(res.Data);
            //gcCreditClass.DataSource = res.Data;
            gvLecturers.FocusInvalidRow();


        }

        private void LoadDataByIdClass()
        {
            rilkKHOA.DataSource = new LopDAL().GetListLop().Data;
            string idClass = bESchoolYear.EditValue.ToString();
            var res = sinhVienDAL.GetListSinhVienByLop(idClass);
            if (res.Response.State == ResponseState.Fail)
            {
                // Notify error
            }

            gcLecturers.DataSource = new BindingList<SINHVIEN>(res.Data);
            //gcCreditClass.DataSource = res.Data;
            gvLecturers.FocusInvalidRow();
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
            int[] rows = gvLecturers.GetSelectedRows();
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
                gvLecturers.FocusInvalidRow();
                List<UPDATEGIANGVIEN> listUpdate;
                var binding = (BindingList<GIANGVIEN>)gvLecturers.DataSource;
                listUpdate = binding.ToList().Select(x => new UPDATEGIANGVIEN(x)).ToList();
                var res = giangVienDAL.UpdateGiangVien(listUpdate,MAKHOA);
                if (res.Response.State == ResponseState.Fail)
                {
                    MessageBox.Show("Lưu thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Program.formMain.Notify("Lưu thành công");
                    InitialSchoolYear();
                }
            }


        }


        private void rilkMAMH_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null)
                return;
            KHOA KHOAs = ((List<KHOA>)rilkKHOA.DataSource).FirstOrDefault(x => x.MAKHOA == e.NewValue.ToString());
            if (KHOAs == null)
                return;
            gvLecturers.SetRowCellValue(gvLecturers.FocusedRowHandle, "TENKHOA", KHOAs.TENKHOA);
        }

        private void gcCreditClass_Click(object sender, EventArgs e)
        {

        }

        private void gvCreditClass_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {

        }
        private void gvCreditClass_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            if (stateUndo)
                return;
            undo.Push(new ActionUndo(2, gvLecturers.RowCount, null), new ActionUndo(3, GetSelelectRow(), new LOPTINCHI()));
            view.SetRowCellValue(e.RowHandle, "MAKHOA", MAKHOA);
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
                    gvLecturers.SetRowCellValue(gvLecturers.GetRowHandle(cell.RowHandle), cell.Column, action.value);
                    Console.WriteLine(gvLecturers.GetRowCellValue(gvLecturers.GetRowHandle(cell.RowHandle), cell.Column));
                    gvLecturers.FocusedRowHandle = cell.RowHandle;
                    break;
                case 2:
                    int row = (int)action.obj;
                    gvLecturers.DeleteRow(row - 1);
                    break;
                case 3:

                    List<GIANGVIEN> gIANGVIENs = (gvLecturers.DataSource as BindingList<GIANGVIEN>).ToList();
                    gIANGVIENs.Insert(int.Parse(action.obj.ToString()), action.value as GIANGVIEN);

                    gcLecturers.DataSource = new BindingList<GIANGVIEN>(gIANGVIENs);
                    gvLecturers.FocusedRowHandle = int.Parse(action.obj.ToString());
                    break;
            }
            stateUndo = false;
        }

        private void gvCreditClass_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {


            if (e.Column.FieldName == "MAGV")
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
            if (gridView.FocusedColumn.FieldName == "MAGV")
            {
                string MASV = (e.Value.ToString());
                if (MASV==null)
                {
                    e.Valid = false;
                    e.ErrorText = "Không để rỗng!";
                }
            }

            if (gridView.FocusedColumn.FieldName == "HO")
            {
                if (e.Value.Equals(""))
                {
                    e.Valid = false;
                    e.ErrorText = "Không để rỗng!";
                }
            }

            if (gridView.FocusedColumn.FieldName == "TEN")
            {
                if (e.Value.Equals(""))
                {
                    e.Valid = false;
                    e.ErrorText = "Không để rỗng!";
                }
            }

            if (gridView.FocusedColumn.FieldName == "MAKHOA")
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
            List<GIANGVIEN> temp = (gvLecturers.DataSource as BindingList<GIANGVIEN>).ToList();
            if (gridView.GetRowCellValue(e.RowHandle, colMAGV) == null)
            {
                e.Valid = false;
                e.ErrorText = "MAGV không để trống";
            }

            if (gridView.GetRowCellValue(e.RowHandle, colHO) == null)
            {
                e.Valid = false;
                e.ErrorText = "HO không để trống";
            }
            if (gridView.GetRowCellValue(e.RowHandle, colTEN) == null)
            {
                e.Valid = false;
                e.ErrorText = "TEN không để trống";
            }
            if (gridView.GetRowCellValue(e.RowHandle, colMAKHOA) == null)
            {
                e.Valid = false;
                e.ErrorText = "MAKHOA không để trống";
            }    
            if(gridView.GetRowCellValue(e.RowHandle, colMAGV) != null)
            {
                if(temp.Where(gv=>gv.MAGV.Trim() == gridView.GetRowCellValue(e.RowHandle, colMAGV).ToString()).Count() >= 2)
                {
                    e.Valid = false;
                    e.ErrorText = "MAGV đã tồn tại";
                }
                if (MAKHOA.Trim().Equals("CNTT"))
                {
                    if(giangVienDAL.GetListGiangVienByKhoa("VT").Data
                        .Where(gv=>gv.MAGV.Trim() == gridView.GetRowCellValue(e.RowHandle, colMAGV).ToString()).Count() > 0)
                    {
                        e.Valid = false;
                        e.ErrorText = "MAGV đã tồn tại";
                    }
                }
                else
                {
                    if (giangVienDAL.GetListGiangVienByKhoa("CNTT").Data
                                            .Where(gv => gv.MAGV.Trim() == gridView.GetRowCellValue(e.RowHandle, colMAGV).ToString()).Count() > 0)
                    {
                        e.Valid = false;
                        e.ErrorText = "MAGV đã tồn tại";
                    }
                }

            }           
        }
    }
}
