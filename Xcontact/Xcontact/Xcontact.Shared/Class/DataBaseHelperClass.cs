using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xcontact.Model;
namespace Xcontact.Class
{
    class DataBaseHelperClass
    {
        //connect db
        SQLiteConnection dbConn;

        //Create Table
        public async Task<bool> onCreate(string DB_PATH)
        {
            try
            { 
                if(!CheckFileExists(DB_PATH).Result)
                {
                    using (dbConn = new SQLiteConnection(DB_PATH)) 
                    {
                        dbConn.CreateTable<Contactos>();
                        dbConn.CreateTable<amigos>();
                        dbConn.CreateTable<colegas>();
                        dbConn.CreateTable<favoritos>();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        //ChekFileExists
        private async Task<bool> CheckFileExists(string filename)
        {
            try 
            {
                var store = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(filename);
                return true;
            }
            catch { return false; }
        }
        //search the specific contact from the db 
        public Contactos ReadContact (int contactid)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingContact = dbConn.Query<Contactos>("select * from contactos where Id_contacto =" + contactid).FirstOrDefault();
                return existingContact;
            }
        }
        //Retrieve the all contact list from th db
        public ObservableCollection<Contactos> ReadContact()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<Contactos> myCollection = dbConn.Table<Contactos>().ToList<Contactos>();
                ObservableCollection<Contactos> ContactList = new ObservableCollection<Contactos>(myCollection);
                return ContactList;
            }
        }
        //Update existing contact
        public void UpdateContact(Contactos contact)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingcontact = dbConn.Query<Contactos>("Select * from contactos where Id_contacto=" + contact.Id_contacto).FirstOrDefault();
                if(existingcontact != null)
                {
                    existingcontact.C_nome = contact.C_nome;
                    existingcontact.C_telemovel = contact.C_telemovel;
                    existingcontact.C_telcasa = contact.C_telcasa;
                    existingcontact.C_teltrabalho = contact.C_teltrabalho;
                    existingcontact.C_morada = contact.C_morada;
                    existingcontact.C_email = contact.C_email;
                    //existingcontact.C_foto = contact.C_foto;
                    dbConn.RunInTransaction(() =>
                        {
                            dbConn.Update(existingcontact);
                        });
                }
            }
        }//UPDATECONTACT
        //Insert the new contact
        public void InsertContact(Contactos newcontact)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                    {
                        dbConn.Insert(newcontact);
                    });
            }
        }//NEWCONTACT

        public void InsertContactToGroupA(amigos newamigos)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Insert(newamigos);
                });
            }
        }//NEWCONTACT
        //Delete Contact
        public void DeleteContact(int Id)
        {
            using ( var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingcontact = dbConn.Query<Contactos>("Select * from contactos where Id_contacto=" + Id).FirstOrDefault();
                if(existingcontact != null)
                {
                    dbConn.RunInTransaction(() =>
                        {
                            dbConn.Delete(existingcontact);
                        });
                }
            }
        }//DELETE CONTACT THAT I CHOSE
        //DELETE ALL
        public void DeleteAllContact()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.DropTable<Contactos>();
                dbConn.CreateTable<Contactos>();
                dbConn.Dispose();
                dbConn.Close();
            }
        }

       
    }
}
