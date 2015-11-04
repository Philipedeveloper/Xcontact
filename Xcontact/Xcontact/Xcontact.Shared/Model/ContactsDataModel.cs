using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xcontact.Model;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
namespace Xcontact
{
    public class ContactsDataModel : ViewModelBase 
    {
        private Contactos _model;
        private BitmapImage _image;
        public ContactsDataModel(Contactos model)
        {
            _model = model;
            LoadImage();
        }

        public BitmapImage Imagem { get { return _image; } set { SetProperty(ref _image, value); } }
        public Contactos Model { get { return _model; } }
        public async void LoadImage() {
            if (string.IsNullOrEmpty(_model.C_foto)) return;
            try
            {
                var destino = ApplicationData.Current.LocalFolder;
                var file = await destino.GetFileAsync(_model.C_foto);
                using (IRandomAccessStream filestream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite))
                {

                    //set image
                    Imagem = new BitmapImage();
                    await Imagem.SetSourceAsync(filestream);

                }
            }
            catch
            {

            }
        }
    }
}
