using System;
using System.Collections.Generic;
using System.Text;

namespace Xcontact.Model
{
    class amigos
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id_amigos { get; set; }
        public int FK_ctami { get; set; }
        public amigos()
        {
            //empty
        }
        public amigos(int FKcontactamigos)
        {
            FK_ctami = FKcontactamigos;
        }
        
    }
}
