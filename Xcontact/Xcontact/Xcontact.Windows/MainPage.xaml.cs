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
using Xcontact.Class;
using Xcontact.Model;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Calls;
using Windows.Storage;
using Windows.Storage.Streams;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Xcontact
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ObservableCollection<ContactsDataModel> DB_ContactList = new ObservableCollection<ContactsDataModel>();
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += ReadContactList_Loaded;
            this.Loaded += ReadGrupoList_Loaded;
            var manager = new Windows.ApplicationModel.Search.Core.SearchSuggestionManager();
            manager.ClearHistory();
        }
        private void ReadContactList_Loaded(object sender, RoutedEventArgs e)
        {
            ReadAllContactsList dbcontacts = new ReadAllContactsList();
            DB_ContactList = dbcontacts.GetAllContacts();//Vai buscar os contactos
            if (DB_ContactList.Count > 0)
            {

            }
            lista.ItemsSource = DB_ContactList.OrderBy(i => i.Model.C_nome).ToList();//
        }//ListAll

        private void ReadGrupoList_Loaded(object sender, RoutedEventArgs e)
        {
            ReadAllContactsList dbcontacts = new ReadAllContactsList();
            DB_ContactList = dbcontacts.GetAllContacts();//Vai buscar os contactos
            if (DB_ContactList.Count > 0)
            {

            }
            //listagem.ItemsSource = DB_ContactList.OrderByDescending(i => i.C_nome).ToList();//
        }

        private void ListViewItem_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }

        private void lista_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            string SelectedContactId = "0";
            string callnome;
            if (lista.SelectedIndex != -1)
            {
                Contactos listitem = lista.SelectedItem as Contactos;
                SelectedContactId = Convert.ToString(listitem.C_telemovel);
                callnome = listitem.C_nome;
                //PhoneCallManager.ShowPhoneCallUI("+351" + SelectedContactId, callnome);


            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Addcontact));
        }

        

        //SearchContact

        private void StackPanel_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }

        private async void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
            if (lista.SelectedItem != null)
            {
               
                ContactsDataModel listitem = lista.SelectedItem as ContactsDataModel;
                Tnome.Text = Convert.ToString(listitem.Model.C_nome);
                Tnumero.Text = Convert.ToString(listitem.Model.C_telemovel);
                Temail.Text = Convert.ToString(listitem.Model.C_email);
                FotoUser.ImageSource = listitem.Imagem;
               
                
            }
        }
        

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        private void SearchContact(SearchBox sender, SearchBoxQueryChangedEventArgs args)
        {
            if (DB_ContactList != null)
            {
                this.lista.ItemsSource = DB_ContactList.Where(x => x.Model.C_nome.ToUpper().Contains(findcontact.QueryText.ToUpper()));
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Maps));
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if (lista.SelectedItem != null)
            {
                this.Frame.Navigate(typeof(EditUser), lista.SelectedItem);
            }
        }





    }
   
}
