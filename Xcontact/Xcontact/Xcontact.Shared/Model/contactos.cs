using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
namespace Xcontact.Model
{
    public class Contactos
    {
        //The id proprety is marked as the Primary key
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id_contacto { get; set; }
        public string C_nome { get; set; }
        public string C_telemovel { get; set; }
        public string C_telcasa { get; set; }
        public string C_teltrabalho { get; set; }
        public string C_morada { get; set; }
        public string C_email { get; set; }
        public string C_foto { get; set; }
        public Contactos()
        {
            //empty construtor
        }
        public Contactos(string Nome, string Telemovel, string Telcasa, string Teltrabalho, string Morada, string Email, string Foto)
        {
            C_nome = Nome;
            C_telemovel = Telemovel;
            C_telcasa = Telcasa;
            C_teltrabalho = Teltrabalho;
            C_morada = Morada;
            C_email = Email;
            C_foto = Foto;
        }



    }
}