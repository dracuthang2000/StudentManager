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
        public FormPrintDiemSV()
        {
            InitializeComponent();
            ReportDiemSV report = new ReportDiemSV();
            var res = new SinhVienDAL().inDiemSV(Program.username);

            report.InitData(res.Data, Program.username);
            textMaSV.EditValue = Program.username;
            textMaSV.Enabled = false;
            //report.xR
            documentViewer1.DocumentSource = report;
            report.CreateDocument();
        }

    }
}
