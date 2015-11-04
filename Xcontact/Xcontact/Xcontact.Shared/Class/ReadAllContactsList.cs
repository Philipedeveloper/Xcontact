using System;
using System.Collections.Generic;
using System.Text;
using Xcontact.Model;
using System.Collections.ObjectModel;

namespace Xcontact.Class
{
    class ReadAllContactsList
    {
        DataBaseHelperClass DB_Helper = new DataBaseHelperClass();
        public ObservableCollection<ContactsDataModel> GetAllContacts()
        {
            var list = new ObservableCollection<ContactsDataModel>();
            foreach (var item in DB_Helper.ReadContact())
            {
                list.Add(new ContactsDataModel(item));
            }
            return list;
        }
      }
}
