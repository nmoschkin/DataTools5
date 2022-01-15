using DataTools.Hardware.Disk;

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

using static DataTools.Text.TextTools;

namespace SysInfoTool.Controls
{
    /// <summary>
    /// Interaction logic for PartitionMap.xaml
    /// </summary>
    public partial class PartitionMap : UserControl
    {

        static readonly Color[] colors = new Color[] { Colors.Green, Colors.Blue, Colors.Red, Colors.Yellow, Colors.Purple, Colors.Orange };

        private void RenderLayouts()
        {
            thisGrid.ColumnDefinitions.Clear();

            if (Disk == null) { return; }

            Color[] cp;
            if (ColorPalette == null || ColorPalette.Length == 0) cp = colors;
            else cp = ColorPalette;

            if (Disk.IsVolume || Disk.Size == 0)
            {
                thisGrid.ColumnDefinitions.Add(new ColumnDefinition());
                thisGrid.Children.Add(new ProgressBar()
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Foreground = new SolidColorBrush(cp[0]),
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Minimum = 0,
                    Maximum = Disk.Size,
                    Value = Disk.SizeUsed,
                    IsIndeterminate = false
                });
            }
            else
            {
                var w = this.ActualWidth;
                if (w == 0) { return; }

                int i, cc = cp.Length;
                int j = 0;
                int c = Disk.DiskLayout.Count;

                long ds = Disk.Size;

                for (i = 0; i < c; i++)
                {
                    var newP = new ProgressBar()
                    {
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Foreground = new SolidColorBrush(cp[j]),
                        ToolTip = PrintFriendlySize(Disk.DiskLayout[i].Size),
                        BorderThickness = new Thickness(i == 0 ? 1 : 0, 1, i == c - 1 ? 1 : 0, 1),
                        Minimum = 0,
                        Maximum = Disk.DiskLayout[i].Size,
                        Value = Disk.DiskLayout[i].Size
                    };

                    thisGrid.ColumnDefinitions.Add(new ColumnDefinition()
                    {
                        Width = new GridLength(w * ((double)Disk.DiskLayout[i].Size / ds))
                    });

                    thisGrid.Children.Add(newP);
                    newP.SetValue(Grid.ColumnProperty, i);

                    j++;
                    if (j >= cc)
                    {
                        j = 0;
                    }
                }

            }


        }



        public Color[] ColorPalette
        {
            get { return (Color[])GetValue(ColorPaletteProperty); }
            set { SetValue(ColorPaletteProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorPalette.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorPaletteProperty =
            DependencyProperty.Register("ColorPalette", typeof(Color[]), typeof(PartitionMap), new PropertyMetadata(colors));



        public DiskDeviceInfo Disk
        {
            get { return (DiskDeviceInfo)GetValue(DiskProperty); }
            set { SetValue(DiskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DiskProperty =
            DependencyProperty.Register("Disk", typeof(DiskDeviceInfo), typeof(PartitionMap), new PropertyMetadata((DiskDeviceInfo)null, OnDiskChanged));

        private static void OnDiskChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is PartitionMap map)
            {
                map.RenderLayouts();
            }
        }

        public PartitionMap()
        {
            InitializeComponent();
            this.SizeChanged += PartitionMap_SizeChanged;
        }

        private void PartitionMap_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RenderLayouts();
        }
    }
}
