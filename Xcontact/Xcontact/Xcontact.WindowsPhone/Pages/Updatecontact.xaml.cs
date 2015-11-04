using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Chat;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;
using Xcontact.Class;
using Xcontact.Model;

//using SQLite;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Xcontact
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Updatecontact : Page
    {
        ObservableCollection<Contactos> DB_ContactList = new ObservableCollection<Contactos>();
        DataBaseHelperClass DB_Helper = new DataBaseHelperClass();
        int Id_selected = 0;
        public Updatecontact()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            Loaded += Updatecontact_Loaded;
        }

        void Updatecontact_Loaded(object sender, RoutedEventArgs e)
        {
            image.Content = "";
          
            
            }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            
            ContactsDataModel Dbbusca = e.Parameter as ContactsDataModel;
            Unome.Text = Dbbusca.Model.C_nome.ToString();
            Utelemovel.Text = Dbbusca.Model.C_telemovel;
            Utelcasa.Text = Dbbusca.Model.C_telcasa;
            Uteltrabalho.Text = Dbbusca.Model.C_teltrabalho;
            Umorada.Text = Dbbusca.Model.C_morada.ToString();
            Uemail.Text = Dbbusca.Model.C_email.ToString();
            btimg.ImageSource = Dbbusca.Imagem;
            Id_selected = Dbbusca.Model.Id_contacto;
            
        }
        public void DeleteContact(object sender, RoutedEventArgs e)
        {
           
            DB_Helper.DeleteContact(Id_selected);
            Frame.Navigate(typeof(MainPage));
            
        }
        
        
        private async void MessageBox(string message)
        {
            var dialog = new MessageDialog(message.ToString());
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await dialog.ShowAsync());
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Contactos update = new Contactos();
                update.C_nome = Unome.Text;
                update.C_telemovel = Utelemovel.Text;
                update.C_telcasa = Utelcasa.Text;
                update.C_teltrabalho = Uteltrabalho.Text;
                update.C_morada = Umorada.Text;
                update.C_email = Uemail.Text;
                update.Id_contacto = Id_selected;
                DB_Helper.UpdateContact(update);
                Frame.Navigate(typeof(MainPage));
            }
            catch { }
        }
    }
}
