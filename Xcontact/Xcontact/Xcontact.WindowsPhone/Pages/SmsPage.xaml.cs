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


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Xcontact.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SmsPage : Page
    {
        public SmsPage()
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
           number.Text = (string)e.Parameter;
             
           //}
        }

        private async void SendSms_Click(object sender, RoutedEventArgs e)
        {
            ChatMessage sendsms = new ChatMessage();
            sendsms.Body = body.Text;
            sendsms.Recipients.Add(number.Text);

           
            await Windows.ApplicationModel.Chat.ChatMessageManager.ShowComposeSmsMessageAsync(sendsms);
            Frame.Navigate(typeof(MainPage));
        }//sendSMS
        private async void MessageBox(string message)
        {
            var dialog = new MessageDialog(message.ToString());
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await dialog.ShowAsync());
        }
        private void SmsCancel(object sender, RoutedEventArgs e)
        {
            number.Text = "";
           body.Text = "";
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
