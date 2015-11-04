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
using Windows.UI.Core;
using Windows.UI.Popups;
using Xcontact.Class;
using Xcontact.Model;
using Windows.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml.Resources;
using System.Windows;
using Windows.Storage;
using Windows.ApplicationModel;

using Windows.ApplicationModel.Activation;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Storage.Pickers;

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
    public sealed partial class Addcontact : Page
    {
        CoreApplicationView view;
        String ImageName;
        BitmapImage SaveImg;
        
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


        public Addcontact()
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
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void AddContacts(object sender, RoutedEventArgs e)
        {
            DataBaseHelperClass Db_Helper = new DataBaseHelperClass();
          
            if (Tnome.Text != null)
            {
                Db_Helper.InsertContact(new Contactos(Tnome.Text, Ttelemovel.Text, Ttelcasa.Text, Tteltrabalho.Text, Tmorada.Text, Temail.Text, ImageName));

                Frame.Navigate(typeof(MainPage));
            }
            else
            {

                MessageBox("Erro ao inserir");
            }
        }//Insert contact

        private async void MessageBox(string message)
        {
            var dialog = new MessageDialog(message.ToString());
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await dialog.ShowAsync());
        }//MessageBox

        private async void GetContactPicture(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            

            StorageFile file = await openPicker.PickSingleFileAsync();  
            var destino = ApplicationData.Current.LocalFolder;
            var res = await file.CopyAsync(destino);
            ImageName = file.Name;
            try
            {
                if (file != null)
                {
                    using (IRandomAccessStream filestream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite))
                    {

                        //set image
                        BitmapImage bitmapimga = new BitmapImage();
                        await bitmapimga.SetSourceAsync(filestream);
                        btimg.ImageSource = bitmapimga;
                        
                    }

                }
                else
                {
                    MessageBox("Operation cancelled.");
                }
            }
            catch { }
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

                Tmorada.Text = SearchMorada.QueryText;
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
