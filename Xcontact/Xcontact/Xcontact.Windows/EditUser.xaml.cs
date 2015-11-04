using Xcontact.Common;
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
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;
using Xcontact.Class;
using Xcontact.Model;
using System.Windows;
using Bing.Maps;
using Windows.UI.Popups;
using System.Threading.Tasks;
using Windows.ApplicationModel.Search;
using System.Runtime.Serialization.Json;
using Windows.Devices.Geolocation;
using System.Threading.Tasks;
using Bing.Maps.Search;
using BingMapsRESTService.Common.JSON;
// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Xcontact
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class EditUser : Page
    {
        ObservableCollection<Contactos> DB_ContactList = new ObservableCollection<Contactos>();
        DataBaseHelperClass DB_Helper = new DataBaseHelperClass();
        int Id_selected = 0;
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public EditUser()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ContactsDataModel Dbbusca = e.Parameter as ContactsDataModel;
            Tnome.Text = Dbbusca.Model.C_nome.ToString();
            Ttelemovel.Text = Dbbusca.Model.C_telemovel;
            Ttelcasa.Text = Dbbusca.Model.C_telcasa;
            Tteltrabalho.Text = Dbbusca.Model.C_teltrabalho;
            //Tmorada.Text = Dbbusca.Model.C_morada.ToString();
            SearchMorada.QueryText = Dbbusca.Model.C_morada.ToString();
            Temail.Text = Dbbusca.Model.C_email.ToString();
            Id_selected = Dbbusca.Model.Id_contacto;
        }
        public void DeleteContact(object sender, RoutedEventArgs e)
        {

            DB_Helper.DeleteContact(Id_selected);
            Frame.Navigate(typeof(MainPage));

        }
       
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Contactos update = new Contactos();
                update.C_nome = Tnome.Text;
                update.C_telemovel = Ttelemovel.Text;
                update.C_telcasa = Ttelcasa.Text;
                update.C_teltrabalho = Tteltrabalho.Text;
                update.C_morada = SearchMorada.QueryText;
                update.C_email = Temail.Text;
                update.Id_contacto = Id_selected;
                DB_Helper.UpdateContact(update);
                
                Frame.Navigate(typeof(MainPage));




            }
            catch { }
        }

        private void DelContact(object sender, RoutedEventArgs e)
        {

            DB_Helper.DeleteContact(Id_selected);
            Frame.Navigate(typeof(MainPage));

        }
        private async void searchPane_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            //throw new NotImplementedException();
            myMap.Children.Clear();

            //Logic for geaocding the query text
            if (!string.IsNullOrWhiteSpace(args.QueryText))
            {

                await new MessageDialog(args.QueryText).ShowAsync();
                Uri geocodeUri = new Uri(
                    string.Format("http://dev.virtualearth.net/REST/v1/Locations?q={0}&c={1}&key={2}",
                    Uri.EscapeUriString(args.QueryText), args.Language, myMap.Credentials));

                //Get response from Bing Maps REST services
                Response r = await GetResponse(geocodeUri);

                if (r != null &&
                    r.ResourceSets != null &&
                    r.ResourceSets.Length > 0 &&
                    r.ResourceSets[0].Resources != null &&
                    r.ResourceSets[0].Resources.Length > 0)
                {
                    LocationCollection locations = new LocationCollection();

                    int i = 1;
                    foreach (BingMapsRESTService.Common.JSON.Location l
                                in r.ResourceSets[0].Resources)
                    {
                        //Get the Location of each result 
                        Bing.Maps.Location location = new Bing.Maps.Location(l.Point.Coordinates[0], l.Point.Coordinates[1]);
                        //Create a pushpin each location 
                        Pushpin pin = new Pushpin()
                        {
                            Tag = l.Name,
                            Text = i.ToString()
                        };
                        i++;

                        //Add a tapped event that will displau the name of the location
                        pin.Tapped += async (s, a) =>
                        {
                            var p = s as Pushpin;
                            await new MessageDialog(p.Tag as string).ShowAsync();
                        };
                        //Set the location of the pushpin
                        MapLayer.SetPosition(pin, location);

                        //add th pushpin to the map
                        myMap.Children.Add(pin);

                        //Add the coordinates of the location to a location collection
                        locations.Add(location);
                    }//End foreach

                    //set the map view based on the location collection
                    myMap.SetView(new LocationRect(locations));
                }//End IF
                else
                {
                    await new MessageDialog("Nenhum resultado encontrado..").ShowAsync();
                }

                //Tmorada.Text = SearchMorada.QueryText;
            }
        }

        private async Task<Response> GetResponse(Uri uri)
        {
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            var response = await client.GetAsync(uri);

            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Response));
                return ser.ReadObject(stream) as Response;
            }
        }

    }
}
