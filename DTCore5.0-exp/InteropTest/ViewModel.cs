using System.Collections.ObjectModel;
using System.Windows;
using DataTools.Interop.Disk;

namespace InteropTest
{


    // ' Work in progress.  

    public class HardwareViewModel : DependencyObject
    {
    }

    public class ChildDeviceCollection : ObservableCollection<DiskDeviceInfo>
    {
    }
}