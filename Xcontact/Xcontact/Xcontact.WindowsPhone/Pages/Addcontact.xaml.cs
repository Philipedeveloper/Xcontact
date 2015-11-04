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
//using SQLite;
using Xcontact.Model;
using Xcontact.Class;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage.Pickers;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml.Shapes;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Resources;
using System.Windows;

using Windows.Storage.Streams;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Xcontact
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    
    public sealed partial class Addcontact : Page
    {
        CoreApplicationView view;
        String ImageName;
        BitmapImage SaveImg  ;

        
        public Addcontact()
        {
            this.InitializeComponent();
            view = CoreApplication.GetCurrentView();
            
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if ((string)e.Parameter != null)
            {
                Tmorada.Text = (string)e.Parameter;
            }
        }
        
       
        
        private void TextBox_KeyDown(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Pages.Maps));
        }

        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            
            DataBaseHelperClass Db_Helper = new DataBaseHelperClass();

            if(Tnome.Text != null )
            {
                Db_Helper.InsertContact(new Contactos(Tnome.Text, Ttelemovel.Text, Ttelcasa.Text, Tteltrabalho.Text, Tmorada.Text, Temail.Text, ImageName));
                
                Frame.Navigate(typeof(MainPage));
            }
            else
            {

                MessageBox("Erro ao inserir");
            }
        }
        private void GetContactPicture(Object sender, RoutedEventArgs e)
        {

            
            FileOpenPicker filePicker = new FileOpenPicker();
            filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            filePicker.ViewMode = PickerViewMode.Thumbnail;

            //Filter file type...
            filePicker.FileTypeFilter.Clear();
            filePicker.FileTypeFilter.Add(".png");
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".jpeg");

            filePicker.PickSingleFileAndContinue();
            view.Activated += view_Activated;
            
        }

        public async void view_Activated(CoreApplicationView sender, Windows.ApplicationModel.Activation.IActivatedEventArgs args1)
        {
            FileOpenPickerContinuationEventArgs args = args1 as FileOpenPickerContinuationEventArgs;


            if (args != null)
            {
                if (args.Files.Count == 0) return;

                view.Activated -= view_Activated;
                StorageFile storfile = args.Files[0];
                var stream = await storfile.OpenAsync(Windows.Storage.FileAccessMode.Read);
                var bitmapImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                await bitmapImage.SetSourceAsync(stream);
                var decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(stream);
                ImageName = storfile.Name;
                btimg.ImageSource = bitmapImage;
                image.Content = "";
                
            }
        }
        private async void MessageBox(string message)
        {
            var dialog = new MessageDialog(message.ToString());
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await dialog.ShowAsync());


        }
        private async void ButtonShowContentDialog1_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Windows.UI.Popups.MessageDialog(
                "Deseja inserir a morada por localização gps? " +
                "Se sim, certifique-se que tenha a localização do telefone ligado.",
                "ATENÇÃO!");
 
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Sim") { Id = 0 });
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Não") { Id = 1 });
 
  
 
                 dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;
 
                var result = await dialog.ShowAsync();
                var opc = sender;
                if( result.Label == "Sim"  )
                {
                    Frame.Navigate(typeof(Pages.Maps));
                }
    
        }


    }
}