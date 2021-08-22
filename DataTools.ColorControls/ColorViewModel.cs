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

        public ColorViewModel(UniColor source)
        {
               this.source = source;
        }

        public UniColor Source
        {
            get {  return source; }
        }

        public System.Windows.Media.Color Color
        {
            get => source.GetWPFColor();
            set
            {
                if (Color != value)
                {
                    source.SetValue(value);

                    OnPropertyChanged();
                    RaiseARGBChange();
                    RaiseHSVChange(false);
                }
            }
        }
      
        private void RaiseARGBChange(bool raiseSource = true)
        {
            OnPropertyChanged(nameof(A));
            OnPropertyChanged(nameof(R));
            OnPropertyChanged(nameof(G));
            OnPropertyChanged(nameof(B));
            if (raiseSource) OnPropertyChanged(nameof(Source));
        }

        private void RaiseHSVChange(bool raiseSource = true)
        {
            OnPropertyChanged(nameof(H));
            OnPropertyChanged(nameof(S));
            OnPropertyChanged(nameof(V));
            if (raiseSource) OnPropertyChanged(nameof(Source));
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
            get => source.H;
            set
            {
                if (source.H != value)
                {
                    source.H = value;
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
