using DataTools.Graphics;
using DataTools.Observable;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace DataTools.ColorControls
{
    public class NamedColorViewModel : ObservableBase
    {
        NamedColor source;
        public static ReadOnlyCollection<NamedColorViewModel> GlobalCatalog { get; private set; }

        public ListCollectionView AllNamedColors { get; private set; }

        private string category;

        static NamedColorViewModel()
        {
            var l = new List<NamedColorViewModel>();
            foreach (var clr in NamedColor.Catalog)
            {
                l.Add(new NamedColorViewModel(clr));
            }

            GlobalCatalog = new ReadOnlyCollection<NamedColorViewModel>(l);
        }

        public NamedColorViewModel()
        {
            var l = new List<NamedColorViewModel>();
            foreach (var clr in GlobalCatalog)
            {
                l.Add(clr);
            }

            AllNamedColors = new ListCollectionView(l);
            AllNamedColors.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
        }

        public NamedColorViewModel(NamedColor source)
        {
            Source = source;
        }


        public NamedColor Source
        {
            get => source;
            set
            {
                if (SetProperty(ref source, value))
                {
                    OnPropertyChanged(nameof(Name));
                    OnPropertyChanged(nameof(Color));
                    SetCategory();
                }
            }
        }

        private void SetCategory()
        {
            var hsv = Source.Color.ToHSV();

            if (hsv.Hue == -1)
            {
                Category = "Grayscale";
            }
            else if (hsv.Hue >= 330 || hsv.Hue < 30)
            {
                Category = "Red";
            }
            else if (hsv.Hue >= 30 && hsv.Hue < 90)
            {
                Category = "Yellow";
            }
            else if (hsv.Hue >= 90 && hsv.Hue < 150)
            {
                Category = "Green";
            }
            else if (hsv.Hue >= 150 && hsv.Hue < 210)
            {
                Category = "Cyan";
            }
            else if (hsv.Hue >= 210 && hsv.Hue < 270)
            {
                Category = "Blue";
            }
            else 
            {
                Category = "Purple";
            }

        }

        public string Category
        {
            get => category;
            set
            {
                SetProperty(ref category, value);
            }
        }

        public string Name
        {
            get => source.Name;
        }

        public Color Color
        {
            get => source.Color.GetWPFColor();
        }

    }
}
