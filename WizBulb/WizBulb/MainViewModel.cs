using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using DataTools.Hardware.Network;

using WizBulb.Localization.Resources;
using WizLib;

namespace WizBulb
{

    public delegate void ScanCompleteEvent(object sender, EventArgs e);


    public class MainViewModel : ViewModelBase
    {

        public event ScanCompleteEvent ScanComplete;

        private AdaptersCollection adapters;


        private bool btnsEnabled = true;

        public bool ButtonsEnabled
        {
            get => btnsEnabled;
            set
            {
                SetProperty(ref btnsEnabled, value);
            }
        }

        private int timeout = 10;

        public int Timeout
        {
            get => timeout;
            set
            {
                SetProperty(ref timeout, value);
            }
        }

        private string timeoutStatus;

        public string TimeoutStatus
        {
            get => timeoutStatus;
            set
            {
                SetProperty(ref timeoutStatus, value);
            }
        }

        private Visibility showts = Visibility.Hidden;

        public Visibility ShowTimeoutStatus
        {
            get => showts;
            set
            {
                SetProperty(ref showts, value);
            }
        }

        private string networkStatus;

        public string NetworkStatus
        {
            get => networkStatus;
            set
            {
                SetProperty(ref networkStatus, value);
            }
        }

        private Visibility showns = Visibility.Hidden;

        public Visibility ShowNetworkStatus
        {
            get => showns;
            set
            {
                SetProperty(ref showns, value);
            }
        }


        private string statusMessage;

        public string StatusMessage
        {
            get => statusMessage;
            set
            {
                SetProperty(ref statusMessage, value);
            }
        }

        public ObservableCollection<NetworkAdapter> Adapters
        {
            get => adapters.Adapters;
        }

        private NetworkAdapter selAdapter;
        public NetworkAdapter SelectedAdapter
        {
            get => selAdapter;
            set
            {
                SetProperty(ref selAdapter, value);
            }
        }


        private ObservableCollection<Bulb> bulbs = new ObservableCollection<Bulb>();

        public ObservableCollection<Bulb> Bulbs
        {
            get => bulbs;
            protected set
            {
                SetProperty(ref bulbs, value);
            }
        }


        private ObservableCollection<Room> rooms = new ObservableCollection<Room>();

        public ObservableCollection<Room> Rooms
        {
            get => rooms;
            set
            {
                SetProperty(ref rooms, value);
            }
        }

        public virtual bool CheckTimeout()
        {
            if (Timeout < 5 || Timeout > 360)
            {
                StatusMessage = string.Format(AppResources.WarnEnterNumberRange, 5, 360);
                return false;
            }

            return true;
        }

        public virtual bool ScanForBulbs()
        {
            var disp = Dispatcher.CurrentDispatcher;

            if (ButtonsEnabled == false) return false;

            ButtonsEnabled = false;

            if (selAdapter == null)
            {
                StatusMessage = AppResources.WarnSelectAdapter;
                ButtonsEnabled = true;
                return false;
            }

            if (!CheckTimeout())
            {
                ButtonsEnabled = true;
                return false;
            }

            if (Bulbs == null)
            {
                Bulbs = new ObservableCollection<Bulb>();
            }
            else
            {
                Bulbs.Clear();
            }

            string prevStat = ""; // StatusMessage;
            StatusMessage = AppResources.ScanningBulbs;

            _ = Task.Run(async () =>
            {
                await Bulb.ScanForBulbs(selAdapter.IPV4Address.ToString(), ScanModes.GetSystemConfig, Timeout,
                (b) =>
                {
                    disp.Invoke(() =>
                    {
                        Bulbs.Add(b);
                        StatusMessage = string.Format(AppResources.ScanningBulbsXBulbsFound, Bulbs.Count);
                    });
                });


                foreach (var bulb in Bulbs)
                {
                    GC.Collect(0);
                    
                    StatusMessage = string.Format(AppResources.GettingBulbInfoForX, bulb.ToString());

                    for (int re = 0; re < 3; re++)
                    {
                        if (await bulb.GetPilot()) break;
                        StatusMessage = string.Format(AppResources.RetryingX, re);
                        await Task.Delay(1000);
                    }
                }

                StatusMessage = AppResources.ScanComplete;

                ScanComplete?.Invoke(this, new EventArgs());

                ButtonsEnabled = true;

                _ = Task.Run(async () =>
                {

                    await Task.Delay(5000);
                    
                    if (ButtonsEnabled)
                        StatusMessage = prevStat;

                });

            });

            return true;
        }

        public virtual void RefreshNetworks()
        {
            var disp = App.Current.Dispatcher;

            disp.Invoke(() =>
            {
                adapters = new AdaptersCollection();
                OnPropertyChanged("Adapters");

                SelectedAdapter = null;
            });

            _ = Task.Run(() =>
            {
                foreach (var net in Adapters)
                {
                    if (net.HasInternet == InternetStatus.HasInternet)
                    {
                        disp.Invoke(() => SelectedAdapter = net);
                        return;
                    }
                }
            });
        }

        public MainViewModel()
        {
            RefreshNetworks();
        }

    }
}
