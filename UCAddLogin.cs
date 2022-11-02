using DevExpress.XtraEditors;
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
    public partial class UCAddLogin : DevExpress.XtraEditors.XtraUserControl
    {
        private UserDAL _userDAL;
        private GiangVienDAL _giangVienDAL;
        private NhanVienDAL _nhanVienDAL;
        public UCAddLogin()
        {
            InitializeComponent();
            _userDAL = new UserDAL();
            _giangVienDAL = new GiangVienDAL();
            _nhanVienDAL = new NhanVienDAL();

            if (Program.group.Equals("KHOA"))
            {
                lkGiangVien.Properties.DataSource = _giangVienDAL.GetListCurrentGiangVien().Data;
            }
            else 
            {
                lkGiangVien.Properties.DataSource = _nhanVienDAL.getListNhanVien().Data;
            }

            switch (Program.group)
            {
                case Role.KHOA: cbxRole.Properties.Items.Add(Role.KHOA);
                    break;
                case Role.PGV:
                    cbxRole.Properties.Items.Add(Role.PGV);
                    break;
            }    
        }

        

        private void Tạo_Click(object sender, EventArgs e)
        {
            if(lkGiangVien.EditValue == null)
            {
                MessageBox.Show("User không được để trống");
                return;
            }
            if (cbxRole.EditValue == null)
            {
                MessageBox.Show("Chức vụ không được để trống");
                return;
            }
            if (String.IsNullOrWhiteSpace(teLogin.Text))
            {
                MessageBox.Show("Tên đăng nhập không được để trống");
                return;
            }
            if (String.IsNullOrWhiteSpace(tePW.Text))
            {
                MessageBox.Show("Mật khẩu không được để trống");
                return;
            }


            string login = teLogin.Text.Trim();
            string password = tePW.Text.Trim();
            string user = (lkGiangVien.EditValue as string).Trim();
            string role = cbxRole.EditValue.ToString().Trim();

            var res = _userDAL.CreateLogin(login, password, user, role);
            if (res.Response.State == Model.ResponseState.Fail)
            {
                MessageBox.Show(res.Response.Message);
            }
            else
            {
                MessageBox.Show("Tạo tài khoản thành công");
            }
        }
    }
}
