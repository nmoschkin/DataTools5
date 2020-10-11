
namespace InteropTest
{
    public partial class SysInfoWindow
    {
        public SysInfoWindow()
        {

            // This call is required by the designer.
            this.InitializeComponent();

            // Add any initialization after the InitializeComponent() call.


            this._props.SelectedObject = CoreCT.SystemInformation.SysInfo.LogicalProcessors;
            long mcache = 0L;
            var lcache = new int[4];
            foreach (var fp in CoreCT.SystemInformation.SysInfo.LogicalProcessors)
            {
                if (fp.Relationship == CoreCT.SystemInformation.LOGICAL_PROCESSOR_RELATIONSHIP.RelationCache)
                {
                    if (fp.CacheDescriptor.Size > 1)
                    {
                        mcache += fp.CacheDescriptor.Size;
                        lcache[fp.CacheDescriptor.Level] += 1;
                    }

                    fp.ToString();
                }
            }
        }
    }
}