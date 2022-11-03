using StudentManagement.Factories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Repositories
{
    public abstract class AbsRepository : IDisposable
    {
        protected SqlConnection conn;
        public AbsRepository(bool lazy = false)
        {
            if(!lazy)
                conn = SQLFactory.GetConnection();
            SQLFactory.RegisterSub(this);
        }
        public void Dispose()
        {
            if (conn.State == System.Data.ConnectionState.Open) conn.Close();
            SQLFactory.UnRegisterSub(this);
        }

        public virtual void OnDbChange() {
            if (conn.State == System.Data.ConnectionState.Open) conn.Close();
            conn = SQLFactory.GetConnection();
        }
    }
}
