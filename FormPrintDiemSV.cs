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
    public partial class FormPrintDiemSV : Form
    {
        ReportDiemSV report = new ReportDiemSV();
        public FormPrintDiemSV()
        {
            InitializeComponent();
            var res = new SinhVienDAL().inDiemSV(Program.username);

            report.InitData(res.Data);
            //report.xR
            documentViewer1.DocumentSource = report;
            report.CreateDocument();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if(textMaSV.EditValue is null)
            {
                MessageBox.Show("Bạn chưa nhập mã sinh viên", "", MessageBoxButtons.OK);
                return;
            }


            if (res.Response.State == ResponseState.Fail)
            {
                MessageBox.Show(res.Response.Message, "", MessageBoxButtons.OK);
                return;
            }

           
        }
    }
}
