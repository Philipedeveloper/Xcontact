using System;
using System.Collections.Generic;
using System.Text;

namespace Xcontact.Model
{
    class favoritos
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id_favoritos { get; set; }
        public int FK_ctfav { get; set; }
        public favoritos()
        {
            //empty
        }
        public favoritos(int FK_contactosfavoritos)
        {
            FK_ctfav = FK_contactosfavoritos;
        }
    }
}
