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
using Windows.ApplicationModel.Email;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Xcontact.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EnviarEmail : Page
    {
        String mail;
        public EnviarEmail()
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

              Tto.Text = (string)e.Parameter;
             
            }
             catch { }
        }


        private async void enviarEmail(object sender, RoutedEventArgs e)
        {
            
            Windows.ApplicationModel.Email.EmailMessage mail = new Windows.ApplicationModel.Email.EmailMessage();
            
            mail.Subject = TSubject.Text;
            mail.Body = Tbody.Text;
            mail.To.Add(new Windows.ApplicationModel.Email.EmailRecipient(Tto.Text, TNome.Text));
            await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(mail);
            Frame.Navigate(typeof(MainPage));

           

        }
    
    
    
    }


}
