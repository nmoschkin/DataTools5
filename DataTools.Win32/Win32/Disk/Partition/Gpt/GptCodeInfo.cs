using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Win32.Disk.Partition.Gpt
{
    /// <summary>
    /// Contains the master list of all known Gpt partition types as GptCodeInfo objects.
    /// </summary>
    /// <remarks></remarks>
    public sealed class GptCodeInfo
    {
        private static List<GptCodeInfo> _Col = new List<GptCodeInfo>();
        private Guid _Guid;
        private string _Name;

        /// <summary>
        /// Returns the Guid for the Gpt partition type.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Guid Guid
        {
            get
            {
                return _Guid;
            }
        }

        /// <summary>
        /// Returns the name of the partition type.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Name
        {
            get
            {
                return _Name;
            }
        }

        /// <summary>
        /// Returns the description (currently returns the name)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Description
        {
            get
            {
                return _Name;
            }
        }

        public override string ToString()
        {
            return Name;
        }

        private GptCodeInfo(string s, string t)
        {
            _Guid = new Guid(t);
            _Name = s;
        }

        public static GptCodeInfo FindByCode(Guid g)
        {
            foreach (var c in _Col)
            {
                if (c.Guid.Equals(g))
                    return c;
            }

            return null;
        }

        

        static GptCodeInfo()
        {
            _Col.Add(new GptCodeInfo("MBR partition scheme", "024DEE41-33E7-11D3-9D69-0008C781F39F"));
            _Col.Add(new GptCodeInfo("EFI System partition", "C12A7328-F81F-11D2-BA4B-00A0C93EC93B"));
            _Col.Add(new GptCodeInfo("BIOS Boot partition", "21686148-6449-6E6F-744E-656564454649"));
            _Col.Add(new GptCodeInfo("Intel Fast Flash (iFFS)) partition (for Intel Rapid Start technology))", "D3BFE2DE-3DAF-11DF-BA40-E3A556D89593"));
            _Col.Add(new GptCodeInfo("Sony boot partition", "F4019732-066E-4E12-8273-346C5641494F"));
            _Col.Add(new GptCodeInfo("Lenovo boot partition", "BFBFAFE7-A34F-448A-9A5B-6213EB736C22"));
            _Col.Add(new GptCodeInfo("Microsoft Reserved Partition (MSR))", "E3C9E316-0B5C-4DB8-817D-F92DF00215AE"));
            _Col.Add(new GptCodeInfo("Basic data partition", "EBD0A0A2-B9E5-4433-87C0-68B6B72699C7"));
            _Col.Add(new GptCodeInfo("Logical Disk Manager (LDM)) metadata partition", "5808C8AA-7E8F-42E0-85D2-E1E90434CFB3"));
            _Col.Add(new GptCodeInfo("Logical Disk Manager data partition", "AF9B60A0-1431-4F62-BC68-3311714A69AD"));
            _Col.Add(new GptCodeInfo("Windows Recovery Environment", "DE94BBA4-06D1-4D40-A16A-BFD50179D6AC"));
            _Col.Add(new GptCodeInfo("IBM General Parallel File System (GPFS)) partition", "37AFFC90-EF7D-4E96-91C3-2D7AE055B174"));
            _Col.Add(new GptCodeInfo("Data partition", "75894C1E-3AEB-11D3-B7C1-7B03A0000000"));
            _Col.Add(new GptCodeInfo("Service Partition", "E2A1E728-32E3-11D6-A682-7B03A0000000"));
            _Col.Add(new GptCodeInfo("Linux filesystem data", "0FC63DAF-8483-4772-8E79-3D69D8477DE4"));
            _Col.Add(new GptCodeInfo("RAID partition", "A19D880F-05FC-4D3B-A006-743F0F84911E"));
            _Col.Add(new GptCodeInfo("Swap partition", "0657FD6D-A4AB-43C4-84E5-0933C84B4F4F"));
            _Col.Add(new GptCodeInfo("Logical Volume Manager (LVM)) partition", "E6D6D379-F507-44C2-A23C-238F2A3DF928"));
            _Col.Add(new GptCodeInfo("/home partition", "933AC7E1-2EB4-4F13-B844-0E14E2AEF915"));
            _Col.Add(new GptCodeInfo("plain dm-crypt partition", "7FFEC5C9-2D00-49B7-8941-3EA10A5586B7"));
            _Col.Add(new GptCodeInfo("LUKS partition", "CA7D7CCB-63ED-4C53-861C-1742536059CC"));
            _Col.Add(new GptCodeInfo("Reserved", "8DA63339-0007-60C0-C436-083AC8230908"));
            _Col.Add(new GptCodeInfo("Boot partition", "83BD6B9D-7F41-11DC-BE0B-001560B84F0F"));
            _Col.Add(new GptCodeInfo("Data partition", "516E7CB4-6ECF-11D6-8FF8-00022D09712B"));
            _Col.Add(new GptCodeInfo("Swap partition", "516E7CB5-6ECF-11D6-8FF8-00022D09712B"));
            _Col.Add(new GptCodeInfo("Unix File System (UFS) partition", "516E7CB6-6ECF-11D6-8FF8-00022D09712B"));
            _Col.Add(new GptCodeInfo("Vinum volume manager partition", "516E7CB8-6ECF-11D6-8FF8-00022D09712B"));
            _Col.Add(new GptCodeInfo("ZFS partition", "516E7CBA-6ECF-11D6-8FF8-00022D09712B"));
            _Col.Add(new GptCodeInfo("Hierarchical File System Plus (HFS+)) partition", "48465300-0000-11AA-AA11-00306543ECAC"));
            _Col.Add(new GptCodeInfo("Apple UFS", "55465300-0000-11AA-AA11-00306543ECAC"));
            _Col.Add(new GptCodeInfo("ZFS", "6A898CC3-1DD2-11B2-99A6-080020736631"));
            _Col.Add(new GptCodeInfo("Apple RAID partition", "52414944-0000-11AA-AA11-00306543ECAC"));
            _Col.Add(new GptCodeInfo("Apple RAID partition, offline", "52414944-5F4F-11AA-AA11-00306543ECAC"));
            _Col.Add(new GptCodeInfo("Apple Boot partition", "426F6F74-0000-11AA-AA11-00306543ECAC"));
            _Col.Add(new GptCodeInfo("Apple Label", "4C616265-6C00-11AA-AA11-00306543ECAC"));
            _Col.Add(new GptCodeInfo("Apple TV Recovery partition", "5265636F-7665-11AA-AA11-00306543ECAC"));
            _Col.Add(new GptCodeInfo("Apple Core Storage (i.e. Lion FileVault)) partition", "53746F72-6167-11AA-AA11-00306543ECAC"));
            _Col.Add(new GptCodeInfo("Boot partition", "6A82CB45-1DD2-11B2-99A6-080020736631"));
            _Col.Add(new GptCodeInfo("Root partition", "6A85CF4D-1DD2-11B2-99A6-080020736631"));
            _Col.Add(new GptCodeInfo("Swap partition", "6A87C46F-1DD2-11B2-99A6-080020736631"));
            _Col.Add(new GptCodeInfo("Backup partition", "6A8B642B-1DD2-11B2-99A6-080020736631"));
            _Col.Add(new GptCodeInfo("/usr partition", "6A898CC3-1DD2-11B2-99A6-080020736631"));
            _Col.Add(new GptCodeInfo("/var partition", "6A8EF2E9-1DD2-11B2-99A6-080020736631"));
            _Col.Add(new GptCodeInfo("/home partition", "6A90BA39-1DD2-11B2-99A6-080020736631"));
            _Col.Add(new GptCodeInfo("Alternate sector", "6A9283A5-1DD2-11B2-99A6-080020736631"));
            _Col.Add(new GptCodeInfo("Reserved partition", "6A945A3B-1DD2-11B2-99A6-080020736631"));
            _Col.Add(new GptCodeInfo("Reserved", "6A9630D1-1DD2-11B2-99A6-080020736631"));
            _Col.Add(new GptCodeInfo("Reserved", "6A980767-1DD2-11B2-99A6-080020736631"));
            _Col.Add(new GptCodeInfo("Reserved", "6A96237F-1DD2-11B2-99A6-080020736631"));
            _Col.Add(new GptCodeInfo("Reserved", "6A8D2AC7-1DD2-11B2-99A6-080020736631"));
            _Col.Add(new GptCodeInfo("Swap partition", "49F48D32-B10E-11DC-B99B-0019D1879648"));
            _Col.Add(new GptCodeInfo("FFS partition", "49F48D5A-B10E-11DC-B99B-0019D1879648"));
            _Col.Add(new GptCodeInfo("LFS partition", "49F48D82-B10E-11DC-B99B-0019D1879648"));
            _Col.Add(new GptCodeInfo("RAID partition", "49F48DAA-B10E-11DC-B99B-0019D1879648"));
            _Col.Add(new GptCodeInfo("Concatenated partition", "2DB519C4-B10F-11DC-B99B-0019D1879648"));
            _Col.Add(new GptCodeInfo("Encrypted partition", "2DB519EC-B10F-11DC-B99B-0019D1879648"));
            _Col.Add(new GptCodeInfo("ChromeOS kernel", "FE3A2A5D-4F32-41A7-B725-ACCC3285A309"));
            _Col.Add(new GptCodeInfo("ChromeOS rootfs", "3CB8E202-3B7E-47DD-8A3C-7FF2A13CFCEC"));
            _Col.Add(new GptCodeInfo("ChromeOS future use", "2E0A753D-9E48-43B0-8337-B15192CB1B5E"));
            _Col.Add(new GptCodeInfo("Haiku BFS", "42465331-3BA3-10F1-802A-4861696B7521"));
            _Col.Add(new GptCodeInfo("Boot partition", "85D5E45E-237C-11E1-B4B3-E89A8F7FC3A7"));
            _Col.Add(new GptCodeInfo("Data partition", "85D5E45A-237C-11E1-B4B3-E89A8F7FC3A7"));
            _Col.Add(new GptCodeInfo("Swap partition", "85D5E45B-237C-11E1-B4B3-E89A8F7FC3A7"));
            _Col.Add(new GptCodeInfo("Unix File System (UFS) partition", "0394EF8B-237E-11E1-B4B3-E89A8F7FC3A7"));
            _Col.Add(new GptCodeInfo("Vinum volume manager partition", "85D5E45C-237C-11E1-B4B3-E89A8F7FC3A7"));
            _Col.Add(new GptCodeInfo("ZFS partition", "85D5E45D-237C-11E1-B4B3-E89A8F7FC3A7"));
        }

        
    }


}
