using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;




// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Xcontact.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Maps : Page
    {
        
        Geolocator geolocator;
        String Morada;
        public Maps()
        {
            this.InitializeComponent();

            geolocator = new Geolocator();

            this.NavigationCacheMode = NavigationCacheMode.Required;

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
            
        {
            
            Mymap.MapServiceToken = "abcdef-abcdefghijklmno";

            
            
            geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;
            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(2),
                    timeout: TimeSpan.FromSeconds(10));
                //MapIcon
                     //MapIcon mapicon = new MapIcon();
                     //mapicon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Images/Maps/Marker-50.png"));
                //msg acima do mapicon
                     //mapicon.Title = "Minha morada";
                //Opçoes do MapIcon
                    //mapicon.Location = new Geopoint(new BasicGeoposition()
                //Create a New Pushpin
                var puspin = CreatePushPin();
                Mymap.Children.Add(puspin);
                var location = new Geopoint(new BasicGeoposition()                
                {
                    Latitude = geoposition.Coordinate.Point.Position.Latitude,
                    Longitude = geoposition.Coordinate.Point.Position.Longitude
                });

                //Posiçao do MapIcon
                    //mapicon.NormalizedAnchorPoint = new Point(0.5, 0.5);
                    //Mymap.MapElements.Add(mapicon);
                //Mostra no mapa
                    //await Mymap.TrySetViewAsync(mapicon.Location, 180, 0, 0, MapAnimationKind.Bow);
                //Position pin
                MapControl.SetLocation(puspin, location);
                MapControl.SetNormalizedAnchorPoint(puspin, new Point(0.0, 1.0));
                await Mymap.TrySetViewAsync(location, 18D, 0, 0, MapAnimationKind.Bow);
            
            }
            catch(UnauthorizedAccessException)
            {
                MessageBox("Localizaçao está desligada!");
            }
            base.OnNavigatedTo(e);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

        }

        private DependencyObject CreatePushPin() 
        {
            // Creating a Polygon Marker
            Polygon polygon = new Polygon();
            polygon.Points.Add(new Point(0, 0));
            polygon.Points.Add(new Point(0, 50));
            polygon.Points.Add(new Point(25, 0));
            polygon.Fill = new SolidColorBrush(Colors.Red);

            return polygon;
        }


        private async void MessageBox(string message)
        {
            var dialog = new MessageDialog(message.ToString());
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await dialog.ShowAsync());
        }

        private async void Mymap_MapTapped(MapControl sender, MapInputEventArgs args)
        {
            Geopoint pointToReverseGeocode = new Geopoint(args.Location.Position);

            //Mostra o local encontrado 
            MapLocationFinderResult result = await MapLocationFinder.FindLocationsAtAsync(pointToReverseGeocode);
            var resultText = new StringBuilder();

            if(result.Status == MapLocationFinderStatus.Success)
            {
                resultText.AppendLine(result.Locations[0].Address.Street + ", " + result.Locations[0].Address.Town + 
                     ", " + result.Locations[0].Address.Country);

            }
            Morada = resultText.ToString();
            MessageBox(resultText.ToString());
            this.Frame.Navigate(typeof(Addcontact), Morada);
           
        }


        private async void LocalizaMe(object sender , RoutedEventArgs e)
        {
            geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;

            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10));
                await Mymap.TrySetViewAsync(geoposition.Coordinate.Point, 18D);

            }
            catch
            {
                MessageBox("Localização desligada!");
            }
        }//LocalizaMe_click
        private void voltarComMorada(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Addcontact),Morada);
        }
        private void viewstreet (Object sender , RoutedEventArgs e)
        {
           
        }
    }
}
