using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;
using Xcontact.Class;
using Xcontact.Model;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Xcontact
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetalheContact : Page
    {
        ObservableCollection<Contactos> DB_ContactList = new ObservableCollection<Contactos>();
        DataBaseHelperClass DB_Helper = new DataBaseHelperClass();
        int Id_selected = 0;
        public DetalheContact()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try 
            {
                ContactsDataModel Dbbusca = e.Parameter as ContactsDataModel;
                FotoUser.ImageSource = Dbbusca.Imagem;
                Tnome.Text = Dbbusca.Model.C_nome;
                Ttelemovel.Text = Dbbusca.Model.C_telemovel;
                Ttelcasa.Text = Dbbusca.Model.C_telcasa;
                Tteltrabalho.Text = Dbbusca.Model.C_teltrabalho;
                Tmorada.Text = Dbbusca.Model.C_morada;
                Temail.Text = Dbbusca.Model.C_email;
                Id_selected = Dbbusca.Model.Id_contacto;
            }
            catch { }

        }

        private void Temail_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Pages.EnviarEmail), Temail.Text);
        }

        
    }
}
