using System.ComponentModel;

using Xamarin.Forms;

using XamCtlTest.ViewModels;

namespace XamCtlTest.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}