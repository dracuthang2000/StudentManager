using DevExpress.XtraEditors;
using StudentManagement.Factories;
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
    public partial class FormReportListStudentByLop : DevExpress.XtraEditors.XtraForm
    {
        ReportListStudentByClass report = new ReportListStudentByClass();
        SinhVienDAL sinhVienDAL = new SinhVienDAL();
        LopDAL lopDAL = new LopDAL();
        SupportDAL supportDAL = new SupportDAL();
        public FormReportListStudentByLop()
        {
            InitializeComponent();
            rilkKhoa2.DataSource = Program.servers;
            beKhoa.EditValue = Program.serverName;

            if (Program.group == Role.KHOA)
                beKhoa.Enabled = false;

        }
        

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string malop =beLop.EditValue as string ;
            var lstLop = (List<LOP>)rilkLop.DataSource;
            string tenlop = lstLop.Single(l => l.MALOP.Trim() == malop.Trim()).TENLOP;
            string tenKhoa = Program.servers.FirstOrDefault(x => x.TENSERVER == Program.currentServer).TENCN;
            if (String.IsNullOrWhiteSpace(malop))
            {
                MessageBox.Show("Mã lớp không được để trống");
                return ;
            }    
            foreach (DevExpress.XtraReports.Parameters.Parameter p in report.Parameters)
                p.Visible = false;

            var res = sinhVienDAL.GetListSinhVienByLop(malop);
            report.InitData(tenKhoa, res.Data,malop, tenlop);
            docView.DocumentSource = report;
            report.CreateDocument();
        }

        private void beKhoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void beKhoa_EditValueChanged(object sender, EventArgs e)
        {
            Program.currentServer = beKhoa.EditValue as string;
            SQLFactory.SetCurrentServer(beKhoa.EditValue as string);
            var res = lopDAL.GetListLop();
            rilkLop.DataSource = res.Data;
            beLop.EditValue = "";
        }
    }
}