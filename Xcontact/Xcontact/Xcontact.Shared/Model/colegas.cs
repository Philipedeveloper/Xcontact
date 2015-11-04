using System;
using System.Collections.Generic;
using System.Text;

namespace Xcontact.Model
{
    class colegas
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id_colegas { get; set; }
        public int FK_ctcol { get; set; }
        public colegas()
        {
            //empty
        }
        public colegas(int FK_contactoscolegas) 
        {
            FK_ctcol = FK_contactoscolegas;
        }
    }
}
