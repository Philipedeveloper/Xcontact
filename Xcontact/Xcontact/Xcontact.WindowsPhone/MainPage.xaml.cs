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
using Xcontact.Class;
using Xcontact.Model;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;
using Windows.ApplicationModel.Calls;
using Windows.ApplicationModel.Chat;
using Windows.UI.Xaml.Media.Imaging;
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
            this.findcontact.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.Loaded += ReadContactList_Loaded;
            this.Loaded += ReadGrupoList_Loaded;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }
        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                
                return;
            }

            if (frame.Content is MainPage)
            {
                e.Handled = true;
                findcontact.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                // Debug.WriteLine("I'm in HomePage");
            }
            else if (frame.CanGoBack)
            {
                frame.GoBack();
                e.Handled = true;
            }
        }
        

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Addcontact));
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            string x = "";
           Frame.Navigate(typeof(Pages.SmsPage),x);
        }
        private void SmsPagego(object sender, RoutedEventArgs e)
        {
            string SelectedContactId = "0";
            if (lista.SelectedItem != null)
            {
                ContactsDataModel listitem = lista.SelectedItem as ContactsDataModel;
                //MessageBox("valor " + listitem.C_nome);
                SelectedContactId = Convert.ToString(listitem.Model.C_telemovel);
                this.Frame.Navigate(typeof(Pages.SmsPage),SelectedContactId);
            }
           
        }
        //READ db list
        private  void ReadContactList_Loaded(object sender, RoutedEventArgs e)
        {
            
                ReadAllContactsList dbcontacts = new ReadAllContactsList();
                DB_ContactList = dbcontacts.GetAllContacts();//Vai buscar os contactos
                if (DB_ContactList.Count > 0)
                {

                }
                lista.ItemsSource = DB_ContactList.OrderByDescending(i => i.Model.C_nome).ToList();//
          
             
            
        }
        
        private void ReadGrupoList_Loaded(object sender, RoutedEventArgs e)
        {
           
            ReadAllContactsList dbcontacts = new ReadAllContactsList();
            DB_ContactList = dbcontacts.GetAllContacts();//Vai buscar os contactos
            if (DB_ContactList.Count > 0)
            {

            }
            //listagem.ItemsSource = DB_ContactList.OrderByDescending(i => i.C_nome).ToList();//
        }
        private void listcontact_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int SelectedContactId = 0;
            if(lista.SelectedIndex != -1)
            {
                Contactos listitem = lista.SelectedItem as Contactos;// get select item contact id
                //frame para editar contacto
            }
        }
        private void ListViewItem_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }
        private void lista_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            string SelectedContactId ="0";
            string callnome;
            if(lista.SelectedIndex != -1 )
            {
                ContactsDataModel listitem = lista.SelectedItem as ContactsDataModel;
                SelectedContactId = Convert.ToString(listitem.Model.C_telemovel);
                callnome = listitem.Model.C_nome;
                PhoneCallManager.ShowPhoneCallUI("+351" + SelectedContactId, callnome);
            }
       }
        private async void MessageBox(string message)
        {
            var dialog = new MessageDialog(message.ToString());
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await dialog.ShowAsync());
        }
        
        //for search
        public void textsearchOn(object sender, RoutedEventArgs e)
        {
            findcontact.Visibility = Windows.UI.Xaml.Visibility.Visible;
            findcontact.Focus(FocusState.Keyboard);
        }
        private  void SearchContact(object sender, TextChangedEventArgs e)
        {
            
            if(DB_ContactList != null)
            {
                this.lista.ItemsSource = DB_ContactList.Where(x => x.Model.C_nome.ToUpper().Contains(findcontact.Text.ToUpper()));
            }
        }

        private void GoContactUpdate(object sender, RoutedEventArgs e)
        {
            string nome = "";
            string SelectedContactId = "0";
            //string SelectedContactName = "";
            if (lista.SelectedItem != null)
            {
                
                //Contactos listitem = lista.SelectedItem as Contactos;
                //SelectedContactId = listitem.C_nome;
                //nome = Convert.ToString(listitem.C_nome);
                this.Frame.Navigate(typeof(Updatecontact), lista.SelectedItem);
            }
            
            
        }
        private void VerDetalhe(object sender, RoutedEventArgs e)
        {
            
            if (lista.SelectedItem != null)
            {
                ContactsDataModel listitem = lista.SelectedItem as ContactsDataModel;

                this.Frame.Navigate(typeof(DetalheContact), lista.SelectedItem);
            }
        }
        private void shareOnSocialNetwork(object sender, RoutedEventArgs e)
        {
           // ShareStatusTask shareStatusTask = new ShareStatusTask();
        }
    }
}
