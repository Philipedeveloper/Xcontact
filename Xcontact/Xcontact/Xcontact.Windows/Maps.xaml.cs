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
using Windows.UI.Popups;
using Bing.Maps;
using Windows.UI.Popups;
using System.Threading.Tasks;
using Windows.ApplicationModel.Search;
using System.Runtime.Serialization.Json;
using Windows.Devices.Geolocation;
using System.Threading.Tasks;
using Bing.Maps.Search;
using BingMapsRESTService.Common.JSON;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Xcontact
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Maps : Page
    {
        
        public Maps()
        {
            
            this.InitializeComponent();
            var manager = new Windows.ApplicationModel.Search.Core.SearchSuggestionManager();
            //manager.ClearHistory();

        }
        

        private async void searchPane_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            //throw new NotImplementedException();
            myMap.Children.Clear();
            
            //Logic for geaocding the query text
            if (!string.IsNullOrWhiteSpace(args.QueryText))
            {

              await new  MessageDialog(args.QueryText).ShowAsync();
                Uri geocodeUri = new Uri(
                    string.Format("http://dev.virtualearth.net/REST/v1/Locations?q={0}&c={1}&key={2}",
                    Uri.EscapeUriString(args.QueryText), args.Language, myMap.Credentials));

                //Get response from Bing Maps REST services
                Response r = await GetResponse(geocodeUri);

                if(r != null &&
                    r.ResourceSets != null &&
                    r.ResourceSets.Length > 0 &&
                    r.ResourceSets[0].Resources != null &&
                    r.ResourceSets[0].Resources.Length > 0)
                {
                    LocationCollection locations = new LocationCollection();

                    int i = 1;
                    foreach(BingMapsRESTService.Common.JSON.Location l
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
