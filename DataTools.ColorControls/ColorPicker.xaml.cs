using DataTools.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataTools.ColorControls
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {

        ColorViewModel vm;

        public ColorViewModel ViewModel => vm;

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorPicker), new PropertyMetadata(Colors.Black, OnColorChanged));

        private static void OnColorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ColorPicker cp)
            {
                if ((Color)e.OldValue != (Color)e.NewValue)
                {
                    cp.vm.SelectedColor = (Color)e.NewValue;
                }
            }
        }


        public ColorPickerMode Mode
        {
            get { return (ColorPickerMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Mode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register(nameof(Mode), typeof(ColorPickerMode), typeof(ColorPicker), new PropertyMetadata(ColorPickerMode.Wheel, ModePropertyChanged));

        private static void ModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorPicker p)
            {
                if ((ColorPickerMode)e.OldValue != (ColorPickerMode)e.NewValue)
                {
                    //p.ColorSwatch.Mode = (ColorPickerMode)e.NewValue;   
                }
            }
        }

        public bool ShowNamedColors
        {
            get { return (bool)GetValue(ShowNamedColorsProperty); }
            set { SetValue(ShowNamedColorsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowNamedColors.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowNamedColorsProperty =
            DependencyProperty.Register("ShowNamedColors", typeof(bool), typeof(ColorPicker), new PropertyMetadata(true, OnElementsVisibleChanged));
        public bool ShowWebHexValue
        {
            get { return (bool)GetValue(ShowWebHexValueProperty); }
            set { SetValue(ShowWebHexValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowWebHexValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowWebHexValueProperty =
            DependencyProperty.Register("ShowWebHexValue", typeof(bool), typeof(ColorPicker), new PropertyMetadata(true, OnElementsVisibleChanged));

        public bool ShowRGB
        {
            get { return (bool)GetValue(ShowRGBProperty); }
            set { SetValue(ShowRGBProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowRGB.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowRGBProperty =
            DependencyProperty.Register("ShowRGB", typeof(bool), typeof(ColorPicker), new PropertyMetadata(true, OnElementsVisibleChanged));


        public bool ShowHSV
        {
            get { return (bool)GetValue(ShowHSVProperty); }
            set { SetValue(ShowHSVProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowHSV.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowHSVProperty =
            DependencyProperty.Register("ShowHSV", typeof(bool), typeof(ColorPicker), new PropertyMetadata(true, OnElementsVisibleChanged));

        public bool ShowAlpha
        {
            get { return (bool)GetValue(ShowAlphaProperty); }
            set { SetValue(ShowAlphaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowAlpha.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowAlphaProperty =
            DependencyProperty.Register("ShowAlpha", typeof(bool), typeof(ColorPicker), new PropertyMetadata(true, OnElementsVisibleChanged));

        public bool ShowAlphaOption
        {
            get { return (bool)GetValue(ShowAlphaOptionProperty); }
            set { SetValue(ShowAlphaOptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowAlphaOption.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowAlphaOptionProperty =
            DependencyProperty.Register("ShowAlphaOption", typeof(bool), typeof(ColorPicker), new PropertyMetadata(true, OnElementsVisibleChanged));


        private static void OnElementsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ColorPicker cp)
            {
                if ((bool)e.OldValue != (bool)e.NewValue)
                {
                    switch (e.Property.Name)
                    {
                        case "ShowAlpha":
                            cp.vm.ShowAlpha = (bool)e.NewValue;
                            break;

                        case "ShowAlphaOption":
                            cp.vm.ShowAlphaOption = (bool)e.NewValue;
                            break;

                        case "ShowNamedColors":
                            cp.vm.ShowNamedColors = (bool)e.NewValue;
                            break;

                        case "ShowWebHexValue":
                            cp.vm.ShowWebHexValue = (bool)e.NewValue;
                            break;

                        case "ShowRGB":
                            cp.vm.ShowRGB = (bool)e.NewValue;
                            break;

                        case "ShowHSV":
                            cp.vm.ShowHSV = (bool)e.NewValue;
                            break;

                    }

                    cp.Rearrange();
                }
            }
        }

        private void Rearrange()
        {
            if (!vm.ShowRGB && !vm.ShowAlpha && !vm.ShowAlphaOption && !vm.ShowHSV && !vm.ShowWebHexValue)
            {
                RHColor.Width = new GridLength(0, GridUnitType.Star);
                SelPanel.SetValue(Grid.RowProperty, 1);
                SelPanel.SetValue(Grid.ColumnProperty, 0);


            }
            else
            {
                RHColor.Width = new GridLength(1, GridUnitType.Star);
                SelPanel.SetValue(Grid.RowProperty, 0);
                SelPanel.SetValue(Grid.ColumnProperty, 1);
            }
        }


        public ColorPicker()
        {
            InitializeComponent();
            vm = new ColorViewModel(SelectedColor.GetUniColor());
            vm.PropertyChanged += Vm_PropertyChanged;

            ControlGrid.DataContext = vm;
            Rearrange();
        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ColorViewModel.SelectedColor))
            {
                if (SelectedColor != vm.SelectedColor)
                {
                    SelectedColor = vm.SelectedColor;
                }
            }
        }
    }
}
