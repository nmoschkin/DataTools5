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
using WizLib;

namespace WizBulb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public delegate void BulbDoer();

        public MainWindow()
        {
            InitializeComponent();

            var loc = Settings.LastWindowLocation;
            var size = Settings.LastWindowSize;

            Left = loc.X;
            Top = loc.Y;

            Width = size.Width;
            Height = size.Height;


            var b = new Bulb("192.168.1.12");
            var b2 = new Bulb("192.168.1.8");

            PredefinedScene p = PredefinedScene.GoldenWhite;

            List<Action> paras = new List<Action>();

            paras.Add(() => { b.SetScene(p, 64); });
            paras.Add(() => { b2.SetScene(p, 64); });
            //paras.Add(() => { b.SetScene(p, System.Drawing.Color.Aqua, 255); });
            //paras.Add(() => { b2.SetScene(p, System.Drawing.Color.Violet, 255); });

            Parallel.Invoke(paras.ToArray());

            this.Loaded += MainWindow_Loaded;
            this.LocationChanged += MainWindow_LocationChanged;
            this.SizeChanged += MainWindow_SizeChanged;
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Settings.LastWindowSize = e.NewSize;
        }

        private void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            Settings.LastWindowLocation = new Point(Left, Top);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            
            var loc = Settings.LastWindowLocation;
            var size = Settings.LastWindowSize;

            Left = loc.X;
            Top = loc.Y;

            Width = size.Width;
            Height = size.Height;

        }
    }
}
