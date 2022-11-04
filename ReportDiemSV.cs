using DevExpress.XtraReports.UI;
using StudentManagement.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace StudentManagement
{
    public partial class ReportDiemSV : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportDiemSV()
        {
            InitializeComponent();
        }
        public void InitData(List<DIEMSINHVIEN> data)
        {
            objectDataSource1.DataSource = data;
        }
    }
}
