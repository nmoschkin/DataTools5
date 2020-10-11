// ' Example program.  Copyright (C) 2014 Nathan Moschkin

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Interop;
using DataTools.Interop.Disk;
using Microsoft.VisualBasic;

namespace InteropTest
{

    // ' VB example demonstates FSMonitor object.

    // ' right now we've configured it for adding.  
    // ' you can comment out the If block, in the event, below, 
    // ' or change the condition to see other results.


    public partial class FSMonTestWindow
    {
        private FSMonitor __FSMon;

        private FSMonitor _FSMon
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return __FSMon;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (__FSMon != null)
                {
                    __FSMon.MonitorClosed -= _FSMon_MonitorClosed;
                    __FSMon.MonitorOpened -= _FSMon_MonitorOpened;
                    __FSMon.WatchNotifyChange -= _FSMon_WatchNotifyChange;
                }

                __FSMon = value;
                if (__FSMon != null)
                {
                    __FSMon.MonitorClosed += _FSMon_MonitorClosed;
                    __FSMon.MonitorOpened += _FSMon_MonitorOpened;
                    __FSMon.WatchNotifyChange += _FSMon_WatchNotifyChange;
                }
            }
        }

        private ObservableCollection<FileNotifyInfo> NotifyCol = new ObservableCollection<FileNotifyInfo>();

        public string MonitoredFolder { get; set; } = null;

        internal void CreateMonitor()
        {
            if (_FSMon is object)
            {
                if (_FSMon.IsWatching)
                {
                    if (Interaction.MsgBox("You are currently watching a folder.  Do you want to stop and select another?", Constants.vbYesNo) == MsgBoxResult.No)
                    {
                        return;
                    }
                    else
                    {
                        _FSMon.StopWatching();
                    }
                }
            }

            _FSMon = null;
            var ip = new WindowInteropHelper(this);
            var fb = new System.Windows.Forms.FolderBrowserDialog();
            if (MonitoredFolder is null)
            {
                fb.SelectedPath = FileSystem.CurDir();
            }
            else
            {
                fb.SelectedPath = MonitoredFolder;
            }

            if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MonitoredFolder = fb.SelectedPath;
                _FSMon = new FSMonitor(MonitoredFolder, ip.EnsureHandle());
            }
        }

        public FSMonTestWindow()
        {
            this.InitializeComponent();
            this.ViewingArea.ItemsSource = NotifyCol;
        }

        private void _FSMon_MonitorClosed(object sender, MonitorClosedEventArgs e)
        {
            switch (e.ClosedState)
            {
                case MonitorClosedState.Closed:
                    {
                        this.Status.Text = "Monitor for '" + MonitoredFolder + "' Closed";
                        break;
                    }

                case MonitorClosedState.ClosedOnError:
                    {
                        this.Status.Text = "Monitor for '" + MonitoredFolder + "'  Closed: " + e.ErrorMessage;
                        break;
                    }
            }
        }

        private void _FSMon_MonitorOpened(object sender, EventArgs e)
        {
            this.Status.Text = "Monitor for '" + MonitoredFolder + "' Opened";
        }

        private void _FSMon_WatchNotifyChange(object sender, FSMonitorEventArgs e)
        {
            var inf = e.Info;
            do
            {

                // ' right now we've configured it for adding.  
                // ' you can comment out the If block, or change the 
                // ' condition to see other results.
                if (inf.Action == FileActions.Added)
                {
                    NotifyCol.Add(inf);
                }

                inf = inf.NextEntry;
            }
            while (!(inf is null));
            this.Status.Text = this.ViewingArea.Items.Count + " total items.";
        }

        private void IPWindow_Closing(object sender, CancelEventArgs e)
        {
            if (_FSMon is null)
                return;
            _FSMon.StopWatching();
        }

        private void StartWatching_Click(object sender, RoutedEventArgs e)
        {
            CreateMonitor();
            if (_FSMon is object)
                _FSMon.Watch();
        }

        private void StopWatching_Click(object sender, RoutedEventArgs e)
        {
            if (_FSMon is null)
                return;
            _FSMon.StopWatching();
        }

        private void Quitting_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}