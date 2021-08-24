using DataTools.Graphics;
using DataTools.Observable;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.ColorControls
{
    public class ColorViewModel : ObservableBase
    {

        private UniColor source;
        private NamedColorViewModel namedColor;
        private double colorValue = 1d;

        public double Value
        {
            get => colorValue;
            set
            {
                SetProperty(ref colorValue, value);
            }
        }

        public ColorViewModel(UniColor source)
        {
               this.source = source;
        }

        public UniColor Source
        {
            get {  return source; }
        }

        public System.Windows.Media.Color SelectedColor
        {
            get => source.GetWPFColor();
            set
            {
                if (SelectedColor != value)
                {
                    source = value.GetUniColor();
                    RaiseARGBChange(false, false);
                    RaiseHSVChange();

                    if (namedColor != null)
                    {
                        if (!namedColor.Color.Equals(SelectedColor))
                        {
                            foreach (NamedColorViewModel nc in NamedColorViewModel.AllNamedColors)
                            {
                                if (nc.Color.Equals(SelectedColor))
                                {
                                    SelectedNamedColor = nc;
                                    return;
                                }
                            }
                        }
                    }

                }
            }
        }
      
        private void RaiseARGBChange(bool raiseSource = true, bool raiseSelColor = true)
        {
            OnPropertyChanged(nameof(A));
            OnPropertyChanged(nameof(R));
            OnPropertyChanged(nameof(G));
            OnPropertyChanged(nameof(B));
            if (raiseSource) OnPropertyChanged(nameof(Source));
            if (raiseSelColor) OnPropertyChanged(nameof(SelectedColor));
            //if (raiseSelColor) OnPropertyChanged(nameof(SelectedColor));
        }

        private void RaiseHSVChange(bool raiseSource = true, bool raiseSelColor = true)
        {
            OnPropertyChanged(nameof(H));
            OnPropertyChanged(nameof(S));
            OnPropertyChanged(nameof(V));
            if (raiseSource) OnPropertyChanged(nameof(Source));
            if (raiseSelColor) OnPropertyChanged(nameof(SelectedColor));
        }

        public NamedColorViewModel SelectedNamedColor
        {
            get => namedColor;
            set
            {
                if (SetProperty(ref namedColor, value))
                {
                    if (namedColor != null)
                    {
                        if (namedColor.Color.Equals(SelectedColor)) return;

                        //Task.Run(() =>
                        //{
                        //    Value = namedColor.Color.GetUniColor().V;
                        //}).ContinueWith((t) =>
                        //{
                            SelectedColor = namedColor.Color;
                        //});
                    }
                }
            }
        }

        public byte A
        {
            get => source.A;
            set
            {
                if (source.A != value)
                {
                    source.A = value;
                    OnPropertyChanged(nameof(A));
                    RaiseHSVChange();
                }
            }
        }
        public byte R
        {
            get => source.R;
            set
            {
                if (source.R != value)
                {
                    source.R = value;
                    OnPropertyChanged(nameof(R));
                    RaiseHSVChange();
                }
            }
        }
        public byte G
        {
            get => source.G;
            set
            {
                if (source.G != value)
                {
                    source.G = value;
                    OnPropertyChanged(nameof(G));
                    RaiseHSVChange();
                }
            }
        }
        public byte B
        {
            get => source.B;
            set
            {
                if (source.B != value)
                {
                    source.B = value;
                    OnPropertyChanged(nameof(B));
                    RaiseHSVChange();
                }
            }
        }

        public double H
        {
            get => Math.Round(source.H, 2);
            set
            {
                if (source.H != value)
                {
                    source.H = Math.Round(value, 2);
                    OnPropertyChanged(nameof(H));
                    RaiseARGBChange();
                }
            }
        }
        public double S
        {
            get => source.S;
            set
            {
                if (source.S != value)
                {
                    source.S = value;
                    OnPropertyChanged(nameof(S));
                    RaiseARGBChange();
                }
            }
        }
        public double V
        {
            get => source.V;
            set
            {
                if (source.V != value)
                {
                    source.V = value;
                    OnPropertyChanged(nameof(V));
                    RaiseARGBChange();
                }
            }
        }


    }
}
