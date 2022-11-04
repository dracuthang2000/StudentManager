using DevExpress.XtraReports.UI;
using StudentManagement.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace StudentManagement
{
    public partial class ReportListStudentByClass : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportListStudentByClass()
        {
            InitializeComponent();
        }
        public void InitData(string tenKhoa, List<SINHVIEN> data, string malop, string tenlop)
        {
            this.malop.Value = malop;
            this.tenlop.Value = tenlop;
            this.tenKhoa.Value = tenKhoa;
            this.objectDataSource1.DataSource = data;
        }
    }
}
