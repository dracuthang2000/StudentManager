namespace StudentManagement.Model
{
    public partial class DANGKY
    {
        public int MALTC { get; set; }

        public string MASV { get; set; }

        public int? DIEM_CC { get; set; }

        public double? DIEM_GK { get; set; }

        public double? DIEM_CK { get; set; }

        public bool? HUYDANGKY { get; set; }
        public string TENSV { get; set; }

        public double DIEM_HM { get; set; }

        public void CaculatorDiemHM()
        {
            if(DIEM_CC!=null&& DIEM_GK!=null && DIEM_CK != null)
            {
                DIEM_HM = (double)(DIEM_CC * 0.1 + DIEM_GK * 0.3 + DIEM_CK * 0.6);
            }
        }
        
    }
}
