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


            var b = new Bulb("192.168.1.28");
            var b2 = new Bulb("192.168.1.27");

            PredefinedScene p = PredefinedScene.Nightlight;

            b.SetScene(p);


            List<Action> paras = new List<Action>();

            paras.Add(() => { b.SetScene(p); });
            paras.Add(() => { b2.SetScene(p); });

            Parallel.Invoke(paras.ToArray());

        }
    }
}
