using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTools.Text;
using DataTools.Win32Api;

namespace DataTools.Win32Api.PartitionInfo
{

    /// <summary>
    /// Contains the master list of all known partition types as PartitionCodeInfo objects.
    /// </summary>
    /// <remarks></remarks>
    public sealed class PartitionCodeInfo
    {
        private static List<PartitionCodeInfo> _Col = new List<PartitionCodeInfo>();
        private FriendlyPartitionId _PartitionID;
        private PartitionOccurrence _Occurrence;
        private PartitionAccess _Access;
        private PartitionBootability _Bootability;
        private PartitionCharacteristics _Characteristics;
        private string[] _Origins;
        private string[] _OS;
        private string _Description;
        private string _Name;

        /// <summary>
        /// Initialize a new instance of the PartitionCodeInfo object.
        /// </summary>
        /// <param name="s">Parsing data with which to initialize the object.</param>
        /// <remarks></remarks>
        private PartitionCodeInfo(string s = null)
        {
            if (s is object)
                Parse(s);
        }

        /// <summary>
        /// Partition byte Id.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public FriendlyPartitionId PartitionID
        {
            get
            {
                return _PartitionID;
            }

            private set
            {
                _PartitionID = value;
            }
        }

        /// <summary>
        /// Partition occurrence.  Describes where on the disk a partition entry could be found.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public PartitionOccurrence Occurrence
        {
            get
            {
                return _Occurrence;
            }

            private set
            {
                _Occurrence = value;
            }
        }

        /// <summary>
        /// Specifies the kind of hardware access the partition type supports.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public PartitionAccess Access
        {
            get
            {
                return _Access;
            }

            private set
            {
                _Access = value;
            }
        }

        /// <summary>
        /// Specifies the partition type's bootability.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public PartitionBootability Bootability
        {
            get
            {
                return _Bootability;
            }

            private set
            {
                _Bootability = value;
            }
        }

        /// <summary>
        /// Specifies the partition type's characteristics.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public PartitionCharacteristics Characteristics
        {
            get
            {
                return _Characteristics;
            }

            private set
            {
                _Characteristics = value;
            }
        }

        /// <summary>
        /// Specifies the company or companies of origin.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string[] Origins
        {
            get
            {
                return _Origins;
            }

            private set
            {
                _Origins = value;
            }
        }

        /// <summary>
        /// Specifies the supported operating systems.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string[] SupporedOSes
        {
            get
            {
                return _OS;
            }

            private set
            {
                _OS = value;
            }
        }

        /// <summary>
        /// Provides a description of the partition type.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Description
        {
            get
            {
                return _Description;
            }

            private set
            {
                _Description = value;
            }
        }

        /// <summary>
        /// Provides the name of the partition type (if different than description).
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
        /// Converts this object into its string representation.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            if (Name is null)
            {
                if (_OS is null || _OS.Count() == 0)
                {
                    if (Origins is object && Origins.Length > 0)
                    {
                        return Origins[0];
                    }
                    else
                    {
                        return PartitionID.ToString();
                    }
                }
                else
                {
                    return string.Join(", ", _OS);
                }
            }
            else
            {
                return Name;
            }
        }

        /// <summary>
        /// Parse the data into the new instance from one of the entries in the private partition type cache.
        /// </summary>
        /// <param name="s"></param>
        /// <remarks></remarks>
        private void Parse(string s)
        {
            var vs = TextTools.Split(s, ",", true, true, '"', '"', false, false, true);

            int i;
            int c = vs.Length;

            for (i = 0; i < c; i++)
            {
                vs[i] = vs[i].Trim();
            }

            _PartitionID.Value = (byte)TextTools.FVal(vs[0]);

            _Occurrence = (PartitionOccurrence)(int)(DevProp.FlagsParse<PartitionOccurrence>(vs[1]));
            _Access = (PartitionAccess)(int)(DevProp.FlagsParse<PartitionAccess>(vs[2]));

            _Bootability = _internalParseBootFlags(vs[3]);

            _Characteristics = (PartitionCharacteristics)(int)(DevProp.FlagsParse<PartitionCharacteristics>(vs[4]));
            _Origins = TextTools.Split(vs[5], ", ");

            if (vs.Length >= 7)
            {
                _OS = TextTools.Split(vs[6], ", ");
                if (vs.Length >= 8)
                {
                    _Description = vs[7];
                    _Name = vs[7];
                }
            }
        }

        /// <summary>
        /// Returns a list of all known partition types as PartitionInfo objects.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<PartitionCodeInfo> Codes
        {
            get
            {
                return _Col;
            }
        }

        /// <summary>
        /// Finds all partition type information objects that match the specified code.
        /// </summary>
        /// <param name="code">Byte code of the partition type to search.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static PartitionCodeInfo[] FindByCode(byte code)
        {
            var l = new List<PartitionCodeInfo>();
            foreach (var pt in _Col)
            {
                if (pt.PartitionID.Value == code)
                {
                    l.Add(pt);
                }
            }

            return l.ToArray();
        }

        private static string[] _AllOSes;

        /// <summary>
        /// Returns a list of all operating systems listed by the MBR partition types.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string[] AllOSes
        {
            get
            {
                return _AllOSes;
            }
        }

        static PartitionCodeInfo()
        {
            var v = _internalPopulationInfo();
            foreach (var l in v)
                _Col.Add(new PartitionCodeInfo(l));
            _Col.Sort(new PartCodeSorter());
            var vl = new List<string>();
            foreach (var c in _Col)
            {
                if (c.SupporedOSes is null)
                    continue;
                foreach (var s in c.SupporedOSes)
                {
                    if (s is null)
                        continue;
                    if (vl.Contains(s) == false)
                    {
                        vl.Add(s);
                    }
                }
            }

            vl.Sort();
            _AllOSes = vl.ToArray();
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private static PartitionBootability _internalParseBootFlags(string s)
        {
            if (string.IsNullOrEmpty(s))
                return PartitionBootability.NoInfo;
            var vs = TextTools.Split(s, ", ");
            var vl = PartitionBootability.NoInfo;
            var sl = Enum.GetNames(vl.GetType());
            foreach (var x in vs)
            {
                if (sl.Contains(x))
                {
                    vl = vl | (PartitionBootability)(int)(Enum.Parse(vl.GetType(), x));
                }
                else if (sl.Contains("M" + x))
                {
                    vl = vl | (PartitionBootability)(int)(Enum.Parse(vl.GetType(), "M" + x));
                }
                else if (sl.Contains("I" + x))
                {
                    vl = vl | (PartitionBootability)(int)(Enum.Parse(vl.GetType(), "I" + x));
                }
                else if (x == "8080/Z80")
                {
                    vl = vl | PartitionBootability.I8080Z80;
                }
            }

            return vl;
        }

        /// <summary>
        /// Returns a string array of comma-seperated partition information entries.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string[] _internalPopulationInfo()
        {

            // This list was adapted from the Wikipedia article: http://en.wikipedia.org/wiki/Partition_type

            var v = new List<string>();
            v.Add("\"&H00\",\"MBR, EBR\",\"N/A\",\"No\",\"Free\",\"IBM\",\"All\",\"Empty partition entry\"");
            v.Add("\"&H01\",\"MBR, EBR\",\"CHS, LBA\",\"x86, 68000, 8080/Z80\",\"Filesystem\",\"IBM\",\"DOS 2.0+\",\"FAT12 as primary partition in first physical 32 MB of disk or as logical drive anywhere on disk (else use 06h instead)\"");
            v.Add("\"&H02\",\"MBR\",\"CHS\",\"\",\"\",\"Microsoft, SCO\",\"XENIX\",\"XENIX root\"");
            v.Add("\"&H03\",\"MBR\",\"CHS\",\"\",\"\",\"Microsoft, SCO\",\"XENIX\",\"XENIX usr\"");
            v.Add("\"&H04\",\"MBR, EBR\",\"CHS, LBA\",\"x86, 68000, 8080/Z80\",\"Filesystem\",\"Microsoft\",\"DOS 3.0+\",\"FAT16 with less than 65536 sectors (32 MB). As primary partition it must reside in first physical 32 MB of disk, or as logical drive anywhere on disk (else use 06h instead).\"");
            v.Add("\"&H05\",\"MBR, EBR\",\"CHS, (LBA)\",\"No, AAP\",\"Container\",\"IBM\",\"DOS (3.2) 3.3+\",\"Extended partition with CHS addressing. It must reside in first physical 8 GB of disk, else use 0Fh instead\"");
            v.Add("\"&H06\",\"MBR, EBR\",\"CHS, LBA\",\"x86\",\"Filesystem\",\"Compaq\",\"DOS 3.31+\",\"FAT16B with 65536 or more sectors. It must reside in first physical 8 GB of disk, unless used for logical drives in an 0Fh extended partition (else use 0Eh instead). Also used for FAT12 and FAT16 volumes in primary partitions if they are not residing in first physical 32 MB of disk.\"");
            v.Add("\"&H07\",\"MBR, EBR\",\"CHS, LBA\",\"x86\",\"Filesystem\",\"Microsoft, IBM\",\"OS/2\",\"IFS\"");
            v.Add("\"&H07\",\"MBR, EBR\",\"CHS, LBA\",\"286\",\"Filesystem\",\"IBM\",\"OS/2, Windows NT\",\"HPFS\"");
            v.Add("\"&H07\",\"MBR, EBR\",\"CHS, LBA\",\"386\",\"Filesystem\",\"Microsoft\",\"Windows NT\",\"NTFS\"");
            v.Add("\"&H07\",\"MBR, EBR\",\"CHS, LBA\",\"Yes\",\"Filesystem\",\"Microsoft\",\"Windows Embedded CE\",\"exFAT\"");
            v.Add("\"&H07\",\"\",\"\",\"\",\"\",\"\",\"\",\"Advanced Unix\"");
            v.Add("\"&H07\",\"\",\"\",\"\",\"\",\"Quantum Software Systems\",\"QNX 2\",\"QNX 'qnx' (7) (pre-1988 only)\"");
            v.Add("\"&H08\",\"MBR\",\"CHS\",\"x86\",\"Filesystem\",\"Commodore\",\"Commodore MS-DOS 3.x\",\"Logical sectored FAT12 or FAT16\"");
            v.Add("\"&H08\",\"\",\"CHS\",\"x86\",\"Filesystem\",\"IBM\",\"OS/2 1.0-1.3\",\"OS/2\"");
            v.Add("\"&H08\",\"\",\"\",\"\",\"\",\"IBM\",\"AIX\",\"AIX boot/split\"");
            v.Add("\"&H08\",\"\",\"\",\"\",\"\",\"\",\"\",\"SplitDrive\"");
            v.Add("\"&H08\",\"\",\"\",\"\",\"\",\"Quantum Software Systems\",\"QNX 1.x/2.x\",\"QNX 'qny' (8)\"");
            v.Add("\"&H08\",\"\",\"\",\"\",\"\",\"Dell\",\"\",\"partition spanning multiple drives\"");
            v.Add("\"&H09\",\"\",\"\",\"\",\"\",\"IBM\",\"AIX\",\"AIX data/boot\"");
            v.Add("\"&H09\",\"\",\"\",\"\",\"\",\"Quantum Software Systems\",\"QNX 1.x/2.x\",\"QNX 'qnz' (9)\"");
            v.Add("\"&H09\",\"MBR\",\"CHS\",\"286\",\"Filesystem\",\"Mark Williams Company\",\"Coherent\",\"Coherent file system\"");
            v.Add("\"&H09\",\"MBR\",\"\",\"\",\"Filesystem\",\"Microware\",\"OS-9\",\"OS-9 RBF\"");
            v.Add("\"&H0A\",\"\",\"\",\"\",\"\",\"PowerQuest, IBM\",\"OS/2\",\"OS/2 Boot Manager\"");
            v.Add("\"&H0A\",\"\",\"\",\"\",\"\",\"Mark Williams Company\",\"Coherent\",\"Coherent swap partition\"");
            v.Add("\"&H0A\",\"\",\"\",\"\",\"\",\"Unisys\",\"OPUS\",\"Open Parallel Unisys Server\"");
            v.Add("\"&H0B\",\"MBR, EBR\",\"CHS, LBA\",\"x86\",\"Filesystem\",\"Microsoft\",\"DOS 7.1+\",\"FAT32 with CHS addressing\"");
            v.Add("\"&H0C\",\"MBR, EBR\",\"LBA\",\"x86\",\"Filesystem\",\"Microsoft\",\"DOS 7.1+\",\"FAT32X with LBA\"");
            v.Add("\"&H0E\",\"MBR, EBR\",\"LBA\",\"x86\",\"Filesystem\",\"Microsoft\",\"DOS 7.0+\",\"FAT16X with LBA\"");
            v.Add("\"&H0F\",\"MBR, EBR\",\"LBA\",\"No, AAP\",\"Container\",\"Microsoft\",\"DOS 7.0+\",\"Extended partition with LBA\"");
            v.Add("\"&H10\",\"\",\"\",\"\",\"\",\"Unisys\",\"OPUS\",\"\"");
            v.Add("\"&H11\",\"MBR\",\"CHS\",\"x86\",\"Filesystem\",\"Leading Edge\",\"Leading Edge MS-DOS 3.x\",\"Logical sectored FAT12 or FAT16\"");
            v.Add("\"&H11\",\"\",\"\",\"\",\"Hidden, Filesystem\",\"IBM\",\"OS/2 Boot Manager\",\"Hidden FAT12 (corresponds with 01h)\"");
            v.Add("\"&H12\",\"MBR\",\"CHS, LBA\",\"x86\",\"Service, Filesystem\",\"Compaq\",\"\",\"configuration partition (bootable FAT)\"");
            v.Add("\"&H12\",\"\",\"\",\"\",\"Hibernation\",\"Compaq\",\"Compaq Contura\",\"hibernation partition\"");
            v.Add("\"&H12\",\"MBR\",\"\",\"x86\",\"Service, Filesystem\",\"NCR\",\"\",\"diagnostics and firmware partition (bootable FAT)\"");
            v.Add("\"&H12\",\"MBR\",\"\",\"x86\",\"Service, Filesystem\",\"Intel\",\"\",\"service partition (bootable FAT)\"");
            v.Add("\"&H12\",\"\",\"\",\"\",\"Service\",\"IBM\",\"\",\"Rescue and Recovery partition\"");
            v.Add("\"&H14\",\"\",\"\",\"\",\"Filesystem\",\"AST\",\"AST MS-DOS 3.x\",\"Logical sectored FAT12 or FAT16\"");
            v.Add("\"&H14\",\"\",\"\",\"x86, 68000, 8080/Z80\",\"Hidden, Filesystem\",\"IBM\",\"OS/2 Boot Manager\",\"Hidden FAT16 (corresponds with 04h)\"");
            v.Add("\"&H14\",\"\",\"LBA\",\"\",\"Filesystem\",\"\",\"Maverick OS\",\"Omega filesystem\"");
            v.Add("\"&H15\",\"\",\"\",\"No, AAP\",\"Hidden, Container\",\"IBM\",\"OS/2 Boot Manager\",\"Hidden extended partition with CHS addressing (corresponds with 05h)\"");
            v.Add("\"&H15\",\"\",\"LBA\",\"\",\"\",\"\",\"Maverick OS\",\"swap\"");
            v.Add("\"&H16\",\"\",\"\",\"x86, 68000, 8080/Z80\",\"Hidden, Filesystem\",\"IBM\",\"OS/2 Boot Manager\",\"Hidden FAT16B (corresponds with 06h)\"");
            v.Add("\"&H17\",\"\",\"\",\"\",\"Hidden, Filesystem\",\"IBM\",\"OS/2 Boot Manager\",\"Hidden IFS (corresponds with 07h)\"");
            v.Add("\"&H17\",\"\",\"\",\"\",\"\",\"\",\"\",\"Hidden HPFS (corresponds with 07h)\"");
            v.Add("\"&H17\",\"\",\"\",\"\",\"\",\"\",\"\",\"Hidden NTFS (corresponds with 07h)\"");
            v.Add("\"&H17\",\"\",\"\",\"\",\"\",\"\",\"\",\"Hidden exFAT (corresponds with 07h)\"");
            v.Add("\"&H18\",\"\",\"\",\"No\",\"Hibernation\",\"AST\",\"AST Windows\",\"AST Zero Volt Suspend or SmartSleep partition\"");
            v.Add("\"&H19\",\"\",\"\",\"\",\"\",\"Willow Schlanger\",\"Willowtech Photon coS\",\"Willowtech Photon coS\"");
            v.Add("\"&H1B\",\"\",\"\",\"\",\"Hidden, Filesystem\",\"IBM\",\"OS/2 Boot Manager\",\"Hidden FAT32 (corresponds with 0Bh)\"");
            v.Add("\"&H1C\",\"\",\"\",\"\",\"Hidden, Filesystem\",\"IBM\",\"OS/2 Boot Manager\",\"Hidden FAT32X with LBA (corresponds with 0Ch)\"");
            v.Add("\"&H1E\",\"\",\"\",\"\",\"Hidden, Filesystem\",\"IBM\",\"OS/2 Boot Manager\",\"Hidden FAT16X with LBA (corresponds with 0Eh)\"");
            v.Add("\"&H1F\",\"MBR, EBR\",\"LBA\",\"\",\"Hidden, Container\",\"IBM\",\"OS/2 Boot Manager\",\"Hidden extended partition with LBA addressing (corresponds with 0Fh)\"");
            v.Add("\"&H20\",\"\",\"\",\"\",\"\",\"Microsoft\",\"Windows Mobile\",\"Windows Mobile update XIP\"");
            v.Add("\"&H20\",\"\",\"\",\"\",\"\",\"Willow Schlanger\",\"\",\"Willowsoft Overture File System (OFS1)\"");
            v.Add("\"&H21\",\"MBR\",\"\",\"\",\"\",\"Hewlett Packard\",\"\",\"HP Volume Expansion (SpeedStor)\"");
            v.Add("\"&H21\",\"\",\"\",\"\",\"Filesystem\",\"Dave Poirier\",\"Oxygen\",\"FSo2 (Oxygen File System)\"");
            v.Add("\"&H22\",\"\",\"\",\"\",\"Container\",\"Dave Poirier\",\"Oxygen\",\"Oxygen Extended Partition Table\"");
            v.Add("\"&H23\",\"\",\"\",\"\",\"\",\"Microsoft, IBM\",\"\",\"Reserved\"");
            v.Add("\"&H23\",\"\",\"\",\"Yes\",\"\",\"Microsoft\",\"Windows Mobile\",\"Windows Mobile boot XIP\"");
            v.Add("\"&H24\",\"MBR\",\"CHS\",\"x86\",\"Filesystem\",\"NEC\",\"NEC MS-DOS 3.30\",\"Logical sectored FAT12 or FAT16\"");
            v.Add("\"&H25\",\"\",\"\",\"\",\"\",\"Microsoft\",\"Windows Mobile\",\"Windows Mobile IMGFS\"");
            v.Add("\"&H26\",\"\",\"\",\"\",\"\",\"Microsoft, IBM\",\"\",\"Reserved\"");
            v.Add("\"&H27\",\"\",\"\",\"\",\"Service, Filesystem\",\"Microsoft\",\"Windows\",\"Windows recovery environment (RE) partition (hidden NTFS partition type 07h)\"");
            v.Add("\"&H27\",\"MBR\",\"CHS, LBA\",\"Yes\",\"Hidden, Service, Filesystem\",\"Acer\",\"PQservice\",\"FAT32 or NTFS rescue partition\"");
            v.Add("\"&H27\",\"\",\"\",\"\",\"\",\"\",\"MirOS BSD\",\"MirOS partition\"");
            v.Add("\"&H27\",\"\",\"\",\"\",\"\",\"\",\"RooterBOOT\",\"RooterBOOT kernel partition (contains a raw ELF Linux kernel, no filesystem)\"");
            v.Add("\"&H2A\",\"\",\"\",\"\",\"Filesystem\",\"Kurt Skauen\",\"AtheOS\",\"AtheOS file system (AthFS, AFS) (an extension of BFS, see 2Bh and EBh)\"");
            v.Add("\"&H2B\",\"\",\"\",\"\",\"\",\"Kristian van der Vliet\",\"SyllableOS\",\"SyllableSecure (SylStor), a variant of AthFS (an extension of BFS, see 2Ah and EBh)\"");
            v.Add("\"&H31\",\"\",\"\",\"\",\"\",\"Microsoft, IBM\",\"\",\"Reserved\"");
            v.Add("\"&H32\",\"\",\"\",\"\",\"\",\"Alien Internet Services\",\"NOS\"");
            v.Add("\"&H33\",\"\",\"\",\"\",\"\",\"Microsoft, IBM\",\"\",\"Reserved\"");
            v.Add("\"&H34\",\"\",\"\",\"\",\"\",\"Microsoft, IBM\",\"\",\"Reserved\"");
            v.Add("\"&H35\",\"MBR, EBR\",\"CHS, LBA\",\"No\",\"Filesystem\",\"IBM\",\"OS/2 Warp Server / eComStation\",\"JFS (OS/2 implementation of AIX Journaling Filesystem)\"");
            v.Add("\"&H36\",\"\",\"\",\"\",\"\",\"Microsoft, IBM\",\"\",\"Reserved\"");
            v.Add("\"&H38\",\"\",\"\",\"\",\"\",\"Timothy Williams\",\"THEOS\",\"THEOS version 3.2, 2 GB partition\"");
            v.Add("\"&H39\",\"\",\"\",\"\",\"Container\",\"Bell Labs\",\"Plan 9\",\"Plan 9 edition 3 partition (sub-partitions described in second sector of partition)\"");
            v.Add("\"&H39\",\"\",\"\",\"\",\"\",\"Timothy Williams\",\"THEOS\",\"THEOS version 4 spanned partition\"");
            v.Add("\"&H3A\",\"\",\"\",\"\",\"\",\"Timothy Williams\",\"THEOS\",\"THEOS version 4, 4 GB partition\"");
            v.Add("\"&H3B\",\"\",\"\",\"\",\"\",\"Timothy Williams\",\"THEOS\",\"THEOS version 4 extended partition\"");
            v.Add("\"&H3C\",\"\",\"\",\"\",\"\",\"PowerQuest\",\"PartitionMagic\",\"PqRP (PartitionMagic or DriveImage in progress)\"");
            v.Add("\"&H3D\",\"\",\"\",\"\",\"Hidden, Filesystem\",\"PowerQuest\",\"PartitionMagic\",\"Hidden NetWare\"");
            v.Add("\"&H3F\",\"\",\"\",\"\",\"\",\"\",\"OS/32\"");
            v.Add("\"&H40\",\"\",\"\",\"\",\"\",\"PICK Systems\",\"PICK\",\"PICK R83\"");
            v.Add("\"&H40\",\"\",\"\",\"\",\"\",\"VenturCom\",\"Venix\",\"Venix 80286\"");
            v.Add("\"&H41\",\"\",\"\",\"Yes\",\"\",\"\",\"Personal RISC\",\"Personal RISC Boot\"");
            v.Add("\"&H41\",\"\",\"\",\"\",\"\",\"Linux\",\"Linux\",\"Old Linux/Minix (disk shared with DR DOS 6.0) (corresponds with 81h)\"");
            v.Add("\"&H41\",\"\",\"\",\"PowerPC\",\"\",\"PowerPC\",\"PowerPC\",\"PPC PReP (Power PC Reference Platform) Boot\"");
            v.Add("\"&H42\",\"\",\"\",\"\",\"Secured, Filesystem\",\"Peter Gutmann\",\"SFS\",\"Secure Filesystem (SFS)\"");
            v.Add("\"&H42\",\"\",\"\",\"No\",\"\",\"Linux\",\"Linux\",\"Old Linux swap (disk shared with DR DOS 6.0) (corresponds with 82h)\"");
            v.Add("\"&H42\",\"\",\"\",\"\",\"Container\",\"Microsoft\",\"Windows 2000, XP, etc.\",\"Dynamic extended partition marker\"");
            v.Add("\"&H43\",\"\",\"\",\"Yes\",\"Filesystem\",\"Linux\",\"Linux\",\"Old Linux native (disk shared with DR DOS 6.0) (corresponds with 83h)\"");
            v.Add("\"&H44\",\"\",\"\",\"\",\"\",\"Wildfile\",\"GoBack\",\"Norton GoBack, WildFile GoBack, Adaptec GoBack, Roxio GoBack\"");
            v.Add("\"&H45\",\"\",\"\",\"\",\"\",\"Priam\",\"\",\"Priam\"");
            v.Add("\"&H45\",\"MBR\",\"CHS\",\"Yes\",\"\",\"\",\"Boot-US\",\"Boot-US boot manager (1 cylinder)\"");
            v.Add("\"&H45\",\"\",\"\",\"\",\"\",\"Jochen Liedtke, GMD\",\"EUMEL/ELAN\",\"EUMEL/ELAN (L2)\"");
            v.Add("\"&H46\",\"\",\"\",\"\",\"\",\"Jochen Liedtke, GMD\",\"EUMEL/ELAN\",\"EUMEL/ELAN (L2)\"");
            v.Add("\"&H47\",\"\",\"\",\"\",\"\",\"Jochen Liedtke, GMD\",\"EUMEL/ELAN\",\"EUMEL/ELAN (L2)\"");
            v.Add("\"&H48\",\"\",\"\",\"\",\"\",\"Jochen Liedtke, GMD\",\"EUMEL/ELAN\",\"EUMEL/ELAN (L2)\"");
            v.Add("\"&H48\",\"\",\"\",\"\",\"\",\"ERGOS\",\"ERGOS L3\",\"ERGOS L3\"");
            v.Add("\"&H4A\",\"MBR\",\"\",\"Yes\",\"\",\"Nick Roberts\",\"AdaOS\",\"Aquila\"");
            v.Add("\"&H4A\",\"MBR, EBR\",\"CHS, LBA\",\"No\",\"Filesystem\",\"Mark Aitchison\",\"ALFS/THIN\",\"ALFS/THIN advanced lightweight filesystem for DOS\"");
            v.Add("\"&H4C\",\"\",\"\",\"\",\"\",\"ETH Zürich\",\"ETH Oberon\",\"Aos (A2) filesystem (76)\"");
            v.Add("\"&H4D\",\"\",\"\",\"\",\"\",\"Quantum Software Systems\",\"QNX 4.x, Neutrino\",\"Primary QNX POSIX volume on disk (77)\"");
            v.Add("\"&H4E\",\"\",\"\",\"\",\"\",\"Quantum Software Systems\",\"QNX 4.x, Neutrino\",\"Secondary QNX POSIX volume on disk (78)\"");
            v.Add("\"&H4F\",\"\",\"\",\"\",\"\",\"Quantum Software Systems\",\"QNX 4.x, Neutrino\",\"Tertiary QNX POSIX volume on disk (79)\"");
            v.Add("\"&H4F\",\"\",\"\",\"Yes\",\"\",\"ETH Zürich\",\"ETH Oberon\",\"boot / native filesystem (79)\"");
            v.Add("\"&H50\",\"\",\"\",\"\",\"\",\"ETH Zürich\",\"ETH Oberon\",\"Alternative native filesystem (80)\"");
            v.Add("\"&H50\",\"\",\"\",\"No\",\"\",\"OnTrack\",\"Disk Manager 4\",\"Read-only partition (old)\"");
            v.Add("\"&H50\",\"\",\"\",\"\",\"\",\"\",\"LynxOS\",\"Lynx RTOS\"");
            v.Add("\"&H50\",\"\",\"\",\"\",\"\",\"\",\"\",\"Novell\"");
            v.Add("\"&H51\",\"\",\"\",\"\",\"\",\"Novell\",\"\"");
            v.Add("\"&H50\",\"\",\"\",\"No\",\"\",\"OnTrack\",\"Disk Manager 4-6\",\"Read-write partition (Aux 1)\"");
            v.Add("\"&H52\",\"\",\"\",\"\",\"\",\"\",\"CP/M\",\"CP/M\"");
            v.Add("\"&H52\",\"\",\"\",\"\",\"\",\"\",\"Microport\",\"System V/AT, V/386\"");
            v.Add("\"&H53\",\"\",\"\",\"\",\"\",\"OnTrack\",\"Disk Manager 6\",\"Auxiliary 3 (WO)\"");
            v.Add("\"&H54\",\"\",\"\",\"\",\"\",\"OnTrack\",\"Disk Manager 6\",\"Dynamic Drive Overlay (DDO)\"");
            v.Add("\"&H55\",\"\",\"\",\"\",\"\",\"MicroHouse / StorageSoft\",\"EZ-Drive\",\"EZ-Drive, Maxtor, MaxBlast, or DriveGuide INT 13h redirector volume\"");
            v.Add("\"&H56\",\"\",\"\",\"\",\"\",\"AT&T\",\"AT&T MS-DOS 3.x\",\"Logical sectored FAT12 or FAT16\"");
            v.Add("\"&H56\",\"\",\"\",\"\",\"\",\"MicroHouse / StorageSoft\",\"EZ-Drive\",\"Disk Manager partition converted to EZ-BIOS\"");
            v.Add("\"&H56\",\"\",\"\",\"\",\"\",\"Golden Bow\",\"VFeature\",\"VFeature partitionned volume\"");
            v.Add("\"&H57\",\"\",\"\",\"\",\"\",\"MicroHouse / StorageSoft\",\"DrivePro\"");
            v.Add("\"&H56\",\"\",\"\",\"\",\"\",\"Novell\",\"\",\"VNDI partition\"");
            v.Add("\"&H5C\",\"\",\"\",\"\",\"Container\",\"Priam\",\"EDISK\",\"Priam EDisk Partitioned Volume\"");
            v.Add("\"&H5D\",\"MBR, EBR\",\"CHS, LBA\",\"x86\",\"Policy\",\"\",\"APTI (Alternative Partition Table Identification) conformant systems\",\" APTI alternative partition\"");
            v.Add("\"&H5E\",\"MBR, EBR\",\"LBA\",\"No, AAP\",\"Policy, Container\",\"\",\"APTI conformant systems\",\" APTI alternative extended partition (corresponds with 0Fh)\"");
            v.Add("\"&H5F\",\"MBR, EBR\",\"CHS\",\"No, AAP\",\"Policy, Container\",\"\",\"APTI conformant systems\",\" APTI alternative extended partition (< 8 GB) (corresponds with 05h)\"");
            v.Add("\"&H61\",\"\",\"\",\"\",\"\",\"Storage Dimensions\",\"SpeedStor\"");
            v.Add("\"&H63\",\"\",\"CHS\",\"\",\"Filesystem\",\"\",\"Unix\",\"SCO Unix, ISC, UnixWare, AT&T System V/386, ix, MtXinu BSD 4.3 on Mach, GNU HURD\"");
            v.Add("\"&H64\",\"\",\"\",\"\",\"\",\"Storage Dimensions\",\"SpeedStor\"");
            v.Add("\"&H63\",\"\",\"\",\"\",\"Filesystem\",\"Novell\",\"NetWare\",\"NetWare File System 286/2\"");
            v.Add("\"&H63\",\"\",\"\",\"\",\"\",\"Solomon\",\"\",\"PC-ARMOUR\"");
            v.Add("\"&H65\",\"\",\"\",\"\",\"Filesystem\",\"Novell\",\"NetWare\",\"NetWare File System 386\"");
            v.Add("\"&H66\",\"\",\"\",\"\",\"Filesystem\",\"Novell\",\"NetWare\",\"NetWare File System 386\"");
            v.Add("\"&H66\",\"\",\"\",\"\",\"\",\"Novell\",\"NetWare\",\"Storage Management Services (SMS)\"");
            v.Add("\"&H67\",\"\",\"\",\"\",\"\",\"Novell\",\"NetWare\",\"Wolf Mountain\"");
            v.Add("\"&H68\",\"\",\"\",\"\",\"\",\"Novell\",\"NetWare\"");
            v.Add("\"&H69\",\"\",\"\",\"\",\"\",\"Novell\",\"NetWare 5\"");
            v.Add("\"&H67\",\"\",\"\",\"\",\"\",\"Novell\",\"NetWare\",\"Novell Storage Services (NSS)\"");
            v.Add("\"&H70\",\"\",\"\",\"\",\"\",\"\",\"DiskSecure\",\"DiskSecure multiboot\"");
            v.Add("\"&H71\",\"\",\"\",\"\",\"\",\"Microsoft, IBM\",\"\",\"Reserved\"");
            v.Add("\"&H72\",\"MBR, EBR\",\"CHS\",\"x86\",\"Policy, Filesystem\",\"\",\"APTI conformant systems\",\"APTI alternative FAT12 (CHS, SFN) (corresponds with 01h)\"");
            v.Add("\"&H72\",\"\",\"\",\"\",\"\",\"Nordier\",\"Unix V7/x86\",\"V7/x86\"");
            v.Add("\"&H73\",\"\",\"\",\"\",\"\",\"Microsoft, IBM\",\"\",\"Reserved\"");
            v.Add("\"&H74\",\"\",\"\",\"\",\"\",\"Microsoft, IBM\",\"\",\"Reserved\"");
            v.Add("\"&H74\",\"\",\"\",\"\",\"Secured\",\"\",\"\",\"Scramdisk\"");
            v.Add("\"&H75\",\"\",\"\",\"\",\"\",\"IBM\",\"PC/IX\"");
            v.Add("\"&H76\",\"\",\"\",\"\",\"\",\"Microsoft, IBM\",\"\",\"Reserved\"");
            v.Add("\"&H77\",\"\",\"\",\"\",\"Filesystem\",\"Novell\",\"\",\"VNDI, M2FS, M2CS\"");
            v.Add("\"&H78\",\"\",\"\",\"Yes\",\"Filesystem\",\"Geurt Vos\",\"\",\"XOSL bootloader filesystem\"");
            v.Add("\"&H79\",\"MBR, EBR\",\"CHS\",\"x86\",\"Policy, Filesystem\",\"\",\"APTI conformant systems\",\"APTI alternative FAT16 (CHS, SFN) (corresponds with 04h)\"");
            v.Add("\"&H7A\",\"MBR, EBR\",\"LBA\",\"x86\",\"Policy, Filesystem\",\"\",\"APTI conformant systems\",\"APTI alternative FAT16X (LBA, SFN) (corresponds with 0Dh)\"");
            v.Add("\"&H7B\",\"MBR, EBR\",\"CHS\",\"x86\",\"Policy, Filesystem\",\"\",\"APTI conformant systems\",\"APTI alternative FAT16B (CHS, SFN) (corresponds with 06h)\"");
            v.Add("\"&H7C\",\"MBR, EBR\",\"LBA\",\"x86\",\"Policy, Filesystem\",\"\",\"APTI conformant systems\",\"APTI alternative FAT32X (LBA, SFN) (corresponds with 0Ch)\"");
            v.Add("\"&H7D\",\"MBR, EBR\",\"CHS\",\"x86\",\"Policy, Filesystem\",\"\",\"APTI conformant systems\",\"APTI alternative FAT32 (CHS, SFN) (corresponds with 0Bh)\"");
            v.Add("\"&H7E\",\"\",\"\",\"\",\"\",\"\",\"F.I.X.\"");
            v.Add("\"&H7F\",\"MBR, EBR\",\"CHS, LBA\",\"Yes\",\"\",\"AODPS\",\"Varies\",\" Alternative OS Development Partition Standard - reserved for individual or local use and temporary or experimental projects\"");
            v.Add("\"&H80\",\"\",\"\",\"\",\"Filesystem\",\"Andrew Tanenbaum\",\"Minix 1.1-1.4a\",\"Minix file system (old)\"");
            v.Add("\"&H81\",\"\",\"\",\"\",\"Filesystem\",\"Andrew Tanenbaum\",\"Minix 1.4b+\",\"MINIX file system (corresponds with 41h)\"");
            v.Add("\"&H81\",\"\",\"\",\"\",\"\",\"\",\"Linux\",\"Mitac Advanced Disk Manager\"");
            v.Add("\"&H82\",\"\",\"\",\"No\",\"\",\"GNU/Linux\",\"\",\"Linux swap space (corresponds with 42h)\"");
            v.Add("\"&H82\",\"\",\"\",\"x86\",\"Container\",\"Sun Microsystems\",\"\",\"Solaris x86 (for Sun disklabels up to 2005)\"");
            v.Add("\"&H82\",\"\",\"\",\"\",\"\",\"\",\"\",\"Prime\"");
            v.Add("\"&H83\",\"\",\"\",\"\",\"Filesystem\",\"GNU/Linux\",\"\",\"Any native Linux file system\"");
            v.Add("\"&H84\",\"\",\"\",\"No\",\"Hibernation\",\"Microsoft\",\"\",\"APM hibernation (suspend to disk, S2D)\"");
            v.Add("\"&H84\",\"\",\"\",\"\",\"Hidden, Filesystem\",\"IBM\",\"OS/2\",\"Hidden C: (FAT16)\"");
            v.Add("\"&H84\",\"\",\"\",\"\",\"Hibernation\",\"Intel\",\"Windows 7\",\"Rapid Start technology\"");
            v.Add("\"&H85\",\"\",\"\",\"No, AAP\",\"Container\",\"GNU/Linux\",\"\",\"Linux extended (corresponds with 05h)\"");
            v.Add("\"&H86\",\"\",\"\",\"\",\"Filesystem\",\"Microsoft\",\"Windows NT 4 Server\",\"Fault-tolerant FAT16B mirrored volume set\"");
            v.Add("\"&H86\",\"\",\"\",\"\",\"\",\"GNU/Linux\",\"Linux\",\"Linux RAID superblock with auto-detect (old)\"");
            v.Add("\"&H87\",\"\",\"\",\"\",\"Filesystem\",\"Microsoft\",\"Windows NT 4 Server\",\"Fault-tolerant HPFS/NTFS mirrored volume set\"");
            v.Add("\"&H88\",\"\",\"\",\"\",\"\",\"GNU/Linux\",\"\",\"Linux plaintext partition table\"");
            v.Add("\"&H8A\",\"\",\"\",\"\",\"\",\"Martin Kiewitz\",\"AiR-BOOT\",\"Linux kernel image\"");
            v.Add("\"&H8B\",\"\",\"\",\"\",\"Filesystem\",\"Microsoft\",\"Windows NT 4 Server\",\"Legacy fault-tolerant FAT32 mirrored volume set\"");
            v.Add("\"&H8C\",\"\",\"\",\"\",\"Filesystem\",\"Microsoft\",\"Windows NT 4 Server\",\"Legacy fault-tolerant FAT32X mirrored volume set\"");
            v.Add("\"&H8D\",\"MBR, EBR\",\"CHS, LBA\",\"x86, 68000, 8080/Z80\",\"Hidden, Filesystem\",\"FreeDOS\",\"Free FDISK\",\"Hidden FAT12 (corresponds with 01h)\"");
            v.Add("\"&H8E\",\"\",\"\",\"\",\"\",\"GNU/Linux\",\"Linux\",\"Linux LVM\"");
            v.Add("\"&H90\",\"MBR, EBR\",\"CHS, LBA\",\"x86, 68000, 8080/Z80\",\"Hidden, Filesystem\",\"FreeDOS\",\"Free FDISK\",\"Hidden FAT16 (corresponds with 04h)\"");
            v.Add("\"&H91\",\"MBR, EBR\",\"CHS, LBA\",\"No, AAP\",\"Hidden, Container\",\"FreeDOS\",\"Free FDISK\",\"Hidden extended partition with CHS addressing (corresponds with 05h)\"");
            v.Add("\"&H92\",\"MBR, EBR\",\"CHS, LBA\",\"x86\",\"Hidden, Filesystem\",\"FreeDOS\",\"Free FDISK\",\"Hidden FAT16B (corresponds with 06h)\"");
            v.Add("\"&H93\",\"\",\"\",\"\",\"Filesystem\",\"\",\"Amoeba\",\"Amoeba native filesystem\"");
            v.Add("\"&H93\",\"\",\"\",\"\",\"Hidden, Filesystem\",\"\",\"Linux\",\"Hidden Linux filesystem\"");
            v.Add("\"&H94\",\"\",\"\",\"\",\"\",\"\",\"Amoeba\",\"Amoeba bad block table\"");
            v.Add("\"&H95\",\"\",\"\",\"\",\"\",\"MIT\",\"EXOPC\",\"EXOPC native\"");
            v.Add("\"&H96\",\"\",\"\",\"\",\"Filesystem\",\"\",\"CHRP\",\"ISO-9660 filesystem\"");
            v.Add("\"&H97\",\"MBR, EBR\",\"CHS, LBA\",\"x86\",\"Hidden, Filesystem\",\"FreeDOS\",\"Free FDISK\",\"Hidden FAT32 (corresponds with 0Bh)\"");
            v.Add("\"&H98\",\"MBR, EBR\",\"LBA\",\"x86\",\"Hidden, Filesystem\",\"FreeDOS\",\"Free FDISK\",\"Hidden FAT32X (corresponds with 0Ch)\"");
            v.Add("\"&H98\",\"MBR\",\"CHS, LBA\",\"x86\",\"Hidden, Service, Filesystem\",\"Datalight\",\"ROM-DOS\",\"service partition (bootable FAT) ROM-DOS SuperBoot\"");
            v.Add("\"&H98\",\"MBR\",\"CHS, LBA\",\"x86\",\"Hidden, Service, Filesystem\",\"Intel\",\"\",\"service partition (bootable FAT)\"");
            v.Add("\"&H99\",\"\",\"\",\"\",\"Filesystem\",\"\",\"early Unix\"");
            v.Add("\"&H98\",\"\",\"\",\"\",\"Container\",\"Mylex\",\"DCE376\",\"EISA SCSI (> 1024)\"");
            v.Add("\"&H9A\",\"MBR, EBR\",\"LBA\",\"x86\",\"Hidden, Filesystem\",\"FreeDOS\",\"Free FDISK\",\"Hidden FAT16X (corresponds with 0Eh)\"");
            v.Add("\"&H9B\",\"MBR, EBR\",\"LBA\",\"No, AAP\",\"Hidden, Container\",\"FreeDOS\",\"Free FDISK\",\"Hidden extended partition with LBA (corresponds with 0Fh)\"");
            v.Add("\"&H9E\",\"\",\"\",\"\",\"\",\"Andy Valencia\",\"VSTA\"");
            v.Add("\"&H9B\",\"\",\"\",\"\",\"\",\"Andy Valencia\",\"ForthOS\",\"ForthOS (eForth port)\"");
            v.Add("\"&H9F\",\"\",\"\",\"\",\"\",\"\",\"BSD/OS 3.0+, BSDI\",\"\"");
            v.Add("\"&HA0\",\"MBR\",\"\",\"\",\"Service\",\"Hewlett Packard\",\"\",\"Diagnostic partition for HP laptops\"");
            v.Add("\"&HA0\",\"\",\"\",\"\",\"Hibernation\",\"Phoenix, IBM, Toshiba, Sony\",\"\",\"Hibernate partition\"");
            v.Add("\"&HA1\",\"\",\"\",\"\",\"\",\"Hewlett Packard\",\"\",\"HP Volume Expansion (SpeedStor)\"");
            v.Add("\"&HA1\",\"\",\"\",\"\",\"Hibernation\",\"Phoenix, NEC\",\"\",\"Hibernate partition\"");
            v.Add("\"&HA3\",\"\",\"\",\"\",\"\",\"Hewlett Packard\",\"\",\"HP Volume Expansion (SpeedStor)\"");
            v.Add("\"&HA4\",\"\",\"\",\"\",\"\",\"Hewlett Packard\",\"\",\"HP Volume Expansion (SpeedStor)\"");
            v.Add("\"&HA5\",\"MBR\",\"\",\"\",\"Container\",\"FreeBSD\",\"BSD\",\"BSD slice (BSD/386, 386BSD, NetBSD (old), FreeBSD)\"");
            v.Add("\"&HA6\",\"\",\"\",\"\",\"\",\"Hewlett Packard\",\"\",\"HP Volume Expansion (SpeedStor)\"");
            v.Add("\"&HA6\",\"MBR\",\"\",\"\",\"Container\",\"OpenBSD\",\"OpenBSD\",\"OpenBSD slice\"");
            v.Add("\"&HA7\",\"\",\"\",\"386\",\"Filesystem\",\"NeXT\",\"\",\"NeXTSTEP\"");
            v.Add("\"&HA8\",\"\",\"\",\"\",\"Filesystem\",\"Apple\",\"Darwin, Mac OS X\",\"Apple Darwin, Mac OS X UFS\"");
            v.Add("\"&HA9\",\"MBR\",\"\",\"\",\"Container\",\"NetBSD\",\"NetBSD\",\"NetBSD slice\"");
            v.Add("\"&HAA\",\"MBR\",\"CHS\",\"\",\"Service, Image\",\"Olivetti\",\"MS-DOS\",\"Olivetti MS-DOS FAT12 (1.44 MB)\"");
            v.Add("\"&HAB\",\"\",\"\",\"Yes\",\"\",\"Apple\",\"Darwin, Mac OS X\",\"Apple Darwin, Mac OS X boot\"");
            v.Add("\"&HAB\",\"\",\"\",\"\",\"\",\"Stanislav Karchebny\",\"GO! OS\",\"GO!\"");
            v.Add("\"&HAD\",\"\",\"\",\"\",\"Filesystem\",\"Ben Avison, Acorn\",\"RISC OS\",\"ADFS / FileCore format\"");
            v.Add("\"&HAE\",\"\",\"\",\"x86\",\"Filesystem\",\"Frank Barrus\",\"ShagOS\",\"ShagOS file system\"");
            v.Add("\"&HAF\",\"\",\"\",\"\",\"\",\"Apple\",\"\",\"Apple Mac OS X HFS and HFS+\"");
            v.Add("\"&HAF\",\"\",\"\",\"No\",\"\",\"Frank Barrus\",\"ShagOS\",\"ShagOS swap\"");
            v.Add("\"&HB0\",\"MBR\",\"CHS, LBA\",\"x86\",\"Blocker\",\"Star-Tools\",\"Boot-Star\",\"Boot-Star dummy partition\"");
            v.Add("\"&HB1\",\"\",\"\",\"\",\"\",\"Hewlett Packard\",\"\",\"HP Volume Expansion (SpeedStor)\"");
            v.Add("\"&HB1\",\"\",\"\",\"\",\"\",\"QNX Software Systems\",\"QNX 6.x\",\"QNX Neutrino power-safe file system\"");
            v.Add("\"&HB2\",\"\",\"\",\"\",\"\",\"QNX Software Systems\",\"QNX 6.x\",\"QNX Neutrino power-safe file system\"");
            v.Add("\"&HB3\",\"\",\"\",\"\",\"\",\"Hewlett Packard\",\"\",\"HP Volume Expansion (SpeedStor)\"");
            v.Add("\"&HB3\",\"\",\"\",\"\",\"\",\"QNX Software Systems\",\"QNX 6.x\",\"QNX Neutrino power-safe file system\"");
            v.Add("\"&HB4\",\"\",\"\",\"\",\"\",\"Hewlett Packard\",\"\",\"HP Volume Expansion (SpeedStor)\"");
            v.Add("\"&HB6\",\"\",\"\",\"\",\"\",\"Hewlett Packard\",\"\",\"HP Volume Expansion (SpeedStor)\"");
            v.Add("\"&HB6\",\"EBR\",\"\",\"\",\"\",\"Microsoft\",\"Windows NT 4 Server\",\"Corrupted fault-tolerant FAT16B mirrored master volume\"");
            v.Add("\"&HB7\",\"\",\"\",\"\",\"Filesystem\",\"\",\"BSDI (before 3.0)\",\"BSDI native filesystem / swap\"");
            v.Add("\"&HB7\",\"EBR\",\"\",\"\",\"\",\"Microsoft\",\"Windows NT 4 Server\",\"Corrupted fault-tolerant HPFS/NTFS mirrored master volume\"");
            v.Add("\"&HB8\",\"\",\"\",\"\",\"Filesystem\",\"\",\"BSDI (before 3.0)\",\"BSDI swap / native filesystem\"");
            v.Add("\"&HBB\",\"\",\"\",\"\",\"Hidden, (Filesystem)\",\"PhysTechSoft, Acronis, SWsoft\",\"BootWizard, OS Selector\",\"PTS BootWizard 4 / OS Selector 5 for hidden partitions other than 01h, 04h, 06h, 07h, 0Bh, 0Ch, 0Eh and unformatted partitions\"");
            v.Add("\"&HBB\",\"EBR\",\"\",\"\",\"\",\"Microsoft\",\"Windows NT 4 Server\",\"Corrupted fault-tolerant FAT32 mirrored master volume\"");
            v.Add("\"&HBC\",\"EBR\",\"\",\"\",\"\",\"Microsoft\",\"Windows NT 4 Server\",\"Corrupted fault-tolerant FAT32X mirrored master volume\"");
            v.Add("\"&HBC\",\"MBR\",\"LBA\",\"\",\"\",\"Acronis\",\"\",\"Backup / Acronis Secure Zone ('ACRONIS SZ')\"");
            v.Add("\"&HBC\",\"MBR, EBR\",\"\",\"\",\"\",\"Paragon Software Group\",\"Backup Capsule\",\"Backup Capsule\"");
            v.Add("\"&HBD\",\"\",\"\",\"\",\"\",\"\",\"BonnyDOS/286\"");
            v.Add("\"&HBE\",\"\",\"\",\"Yes\",\"\",\"Sun Microsystems\",\"Solaris 8\",\"Solaris 8 boot\"");
            v.Add("\"&HBF\",\"\",\"\",\"x86\",\"Container\",\"Sun Microsystems\",\"Solaris\",\"Solaris x86 (for Sun disklabels, since 2005)\"");
            v.Add("\"&HC0\",\"MBR\",\"CHS, LBA\",\"x86\",\"Secured, (Container)\",\"Novell, IMS\",\"DR-DOS, Multiuser DOS, REAL/32\",\"Secured FAT partition (smaller than 32 MB)\"");
            v.Add("\"&HC0\",\"\",\"\",\"\",\"\",\"Novell\",\"\",\"NTFT\"");
            v.Add("\"&HC1\",\"MBR, EBR\",\"CHS, LBA\",\"x86\",\"Secured, Hidden, Filesystem\",\"Digital Research\",\"DR DOS 6.0+\",\"Secured FAT12 (corresponds with 01h)\"");
            v.Add("\"&HC2\",\"\",\"\",\"Yes\",\"Hidden, Filesystem\",\"BlueSky Innovations\",\"Power Boot\",\"Hidden Linux native filesystem\"");
            v.Add("\"&HC3\",\"\",\"\",\"No\",\"Hidden\",\"BlueSky Innovations\",\"Power Boot\",\"Hidden Linux swap\"");
            v.Add("\"&HC4\",\"MBR, EBR\",\"CHS, LBA\",\"x86\",\"Secured, Hidden, Filesystem\",\"Digital Research\",\"DR DOS 6.0+\",\"Secured FAT16 (corresponds with 04h)\"");
            v.Add("\"&HC5\",\"MBR, EBR\",\"CHS, LBA\",\"No, AAP\",\"Secured, Hidden, Container\",\"Digital Research\",\"DR DOS 6.0+\",\"Secured extended partition with CHS addressing (corresponds with 05h)\"");
            v.Add("\"&HC6\",\"MBR, EBR\",\"CHS, LBA\",\"x86\",\"Secured, Hidden, Filesystem\",\"Digital Research\",\"DR DOS 6.0+\",\"Secured FAT16B (corresponds with 06h)\"");
            v.Add("\"&HC6\",\"EBR\",\"\",\"\",\"\",\"Microsoft\",\"Windows NT 4 Server\",\"Corrupted fault-tolerant FAT16B mirrored slave volume\"");
            v.Add("\"&HC7\",\"MBR\",\"\",\"Yes\",\"\",\"\",\"Syrinx\",\"Syrinx boot\"");
            v.Add("\"&HC7\",\"EBR\",\"\",\"\",\"\",\"Microsoft\",\"Windows NT 4 Server\",\"Corrupted fault-tolerant HPFS/NTFS mirrored slave volume\"");
            v.Add("\"&HC8\",\"\",\"\",\"\",\"\",\"\",\"DR-DOS\",\"Reserved for DR-DOS\"");
            v.Add("\"&HC9\",\"\",\"\",\"\",\"\",\"\",\"DR-DOS\",\"Reserved for DR-DOS\"");
            v.Add("\"&HCA\",\"\",\"\",\"\",\"\",\"\",\"DR-DOS\",\"Reserved for DR-DOS\"");
            v.Add("\"&HCB\",\"MBR, EBR\",\"CHS, LBA\",\"x86\",\"Secured, Hidden, Filesystem\",\"Caldera\",\"DR-DOS 7.0x\",\"Secured FAT32 (corresponds with 0Bh)\"");
            v.Add("\"&HCB\",\"EBR\",\"\",\"\",\"\",\"Microsoft\",\"Windows NT 4 Server\",\"Corrupted fault-tolerant FAT32 mirrored slave volume\"");
            v.Add("\"&HCC\",\"MBR, EBR\",\"LBA\",\"x86\",\"Secured, Hidden, Filesystem\",\"Caldera\",\"DR-DOS 7.0x\",\"Secured FAT32X (corresponds with 0Ch)\"");
            v.Add("\"&HCC\",\"EBR\",\"\",\"\",\"\",\"Microsoft\",\"Windows NT 4 Server\",\"Corrupted fault-tolerant FAT32X mirrored slave volume\"");
            v.Add("\"&HCD\",\"\",\"\",\"No\",\"\",\"Convergent Technologies, Unisys\",\"CTOS\",\"Memory dump\"");
            v.Add("\"&HCE\",\"MBR, EBR\",\"LBA\",\"x86\",\"Secured, Hidden, Filesystem\",\"Caldera\",\"DR-DOS 7.0x\",\"Secured FAT16X (corresponds with 0Eh)\"");
            v.Add("\"&HCF\",\"MBR, EBR\",\"LBA\",\"No, AAP\",\"Secured, Hidden, Container\",\"Caldera\",\"DR-DOS 7.0x\",\"Secured extended partition with LBA (corresponds with 0Fh)\"");
            v.Add("\"&HD0\",\"MBR\",\"CHS, LBA\",\"386\",\"Secured, (Container)\",\"Novell, IMS\",\"Multiuser DOS, REAL/32\",\"Secured FAT partition (larger than 32 MB)\"");
            v.Add("\"&HD1\",\"MBR, EBR\",\"CHS\",\"386\",\"Secured, Hidden, Filesystem\",\"Novell\",\"Multiuser DOS\",\"Secured FAT12 (corresponds with 01h)\"");
            v.Add("\"&HD4\",\"MBR, EBR\",\"CHS\",\"386\",\"Secured, Hidden, Filesystem\",\"Novell\",\"Multiuser DOS\",\"Secured FAT16 (corresponds with 04h)\"");
            v.Add("\"&HD5\",\"MBR, EBR\",\"CHS\",\"No\",\"Secured, Hidden, Container\",\"Novell\",\"Multiuser DOS\",\"Secured extended partition with CHS addressing (corresponds with 05h)\"");
            v.Add("\"&HD6\",\"MBR, EBR\",\"CHS\",\"386\",\"Secured, Hidden, Filesystem\",\"Novell\",\"Multiuser DOS\",\"Secured FAT16B (corresponds with 06h)\"");
            v.Add("\"&HD8\",\"MBR\",\"CHS\",\"\",\"Filesystem\",\"Digital Research\",\"\",\"CP/M-86\"");
            v.Add("\"&HDA\",\"\",\"\",\"No\",\"\",\"John Hardin\",\"\",\"Non-filesystem data\"");
            v.Add("\"&HDA\",\"\",\"\",\"\",\"\",\"DataPower\",\"Powercopy Backup\",\"Shielded disk\"");
            v.Add("\"&HDB\",\"MBR\",\"CHS\",\"x86\",\"Filesystem\",\"Digital Research\",\"CP/M-86, Concurrent CP/M-86, Concurrent DOS\",\"CP/M-86, Concurrent CP/M-86, Concurrent DOS\"");
            v.Add("\"&HDB\",\"\",\"\",\"\",\"\",\"Convergent Technologies, Unisys\",\"CTOS\",\"\"");
            v.Add("\"&HDB\",\"\",\"\",\"x86\",\"\",\"KDG Telemetry\",\"D800\",\"boot image for x86 supervisor CPU (SCPU) module\"");
            v.Add("\"&HDB\",\"MBR\",\"CHS, LBA\",\"x86\",\"Hidden, Service, Filesystem\",\"Dell\",\"DRMK\",\"FAT32 system restore partition (DSR)\"");
            v.Add("\"&HDD\",\"\",\"\",\"No\",\"\",\"Convergent Technologies, Unisys\",\"CTOS\",\"Hidden memory dump\"");
            v.Add("\"&HDE\",\"MBR\",\"CHS, LBA\",\"x86\",\"Hidden, Service, Filesystem\",\"Dell\",\"\",\"FAT16 utility/diagnostic partition\"");
            v.Add("\"&HDF\",\"\",\"\",\"\",\"\",\"Data General\",\"DG/UX\",\"DG/UX virtual disk manager\"");
            v.Add("\"&HDF\",\"MBR\",\"\",\"\",\"Blocker\",\"TeraByte Unlimited\",\"BootIt\",\"EMBRM\"");
            v.Add("\"&HDF\",\"\",\"\",\"\",\"\",\"\",\"\",\"Aviion\"");
            v.Add("\"&HE0\",\"\",\"\",\"\",\"Filesystem\",\"STMicroelectronics\",\"\",\"ST AVFS\"");
            v.Add("\"&HE1\",\"\",\"\",\"\",\"Filesystem\",\"Storage Dimensions\",\"SpeedStor\",\"Extended FAT12 (> 1023 cylinder)\"");
            v.Add("\"&HE2\",\"\",\"\",\"\",\"Filesystem\",\"\",\"\",\"DOS read-only (XFDISK)\"");
            v.Add("\"&HE3\",\"\",\"\",\"\",\"Filesystem\",\"Storage Dimensions\",\"SpeedStor\",\"DOS read-only\"");
            v.Add("\"&HE4\",\"\",\"\",\"\",\"Filesystem\",\"Storage Dimensions\",\"SpeedStor\",\"Extended FAT16 (< 1024 cylinder)\"");
            v.Add("\"&HE5\",\"MBR\",\"CHS\",\"x86\",\"Filesystem\",\"Tandy\",\"Tandy MS-DOS\",\"Logical sectored FAT12 or FAT16\"");
            v.Add("\"&HE6\",\"\",\"\",\"\",\"\",\"Storage Dimensions\",\"SpeedStor\"");
            v.Add("\"&HE8\",\"\",\"\",\"\",\"\",\"Linux\",\"LUKS\",\"Linux Unified Key Setup\"");
            v.Add("\"&HEB\",\"\",\"\",\"386\",\"Filesystem\",\"Be Inc.\",\"BeOS, Haiku\",\"BFS\"");
            v.Add("\"&HEC\",\"\",\"\",\"\",\"Filesystem\",\"Robert Szeleney\",\"SkyOS\",\"SkyFS\"");
            v.Add("\"&HED\",\"MBR, EBR\",\"CHS, LBA\",\"x86\",\"\",\"Matthias Paul\",\"Sprytix\",\"EDC loader\"");
            v.Add("\"&HED\",\"VirtualMBR\",\"CHS, LBA\",\"x86\",\"\",\"Robert Elliott, Hewlett Packard\",\"EDD 4\",\"GPT hybrid MBR\"");
            v.Add("\"&HEE\",\"MBR\",\"\",\"x86\",\"Blocker, Policy, Container\",\"Microsoft\",\"EFI\",\"GPT protective MBR\"");
            v.Add("\"&HEF\",\"MBR\",\"\",\"\",\"\",\"Intel\",\"EFI\",\"EFI system partition can be a FAT12, FAT16, FAT32 (or other) file system\"");
            v.Add("\"&HF0\",\"\",\"CHS\",\"\",\"\",\"\",\"Linux\",\"PA-RISC Linux boot loader. It must reside in first physical 2 GB.\"");
            v.Add("\"&HF0\",\"\",\"\",\"\",\"\",\"\",\"OS/32\",\"floppy\"");
            v.Add("\"&HF1\",\"\",\"\",\"\",\"\",\"Storage Dimensions\",\"SpeedStor\"");
            v.Add("\"&HF2\",\"MBR\",\"CHS\",\"x86\",\"Filesystem\",\"Sperry IT, Unisys, Digital Research\",\"Sperry IT MS-DOS 3.x, Unisys MS-DOS 3.3, Digital Research DOS Plus 2.1\",\"Logical sectored FAT12 or FAT16 secondary partition\"");
            v.Add("\"&HF3\",\"\",\"\",\"\",\"\",\"Storage Dimensions\",\"SpeedStor\"");
            v.Add("\"&HF4\",\"\",\"\",\"\",\"Filesystem\",\"Storage Dimensions\",\"SpeedStor\",\"'large' DOS partition\"");
            v.Add("\"&HF4\",\"\",\"\",\"\",\"Filesystem\",\"\",\"Prologue\",\"single volume partition for NGF or TwinFS\"");
            v.Add("\"&HF5\",\"\",\"\",\"\",\"Container\",\"\",\"Prologue\",\"MD0-MD9 multi volume partition for NGF or TwinFS\"");
            v.Add("\"&HF6\",\"\",\"\",\"\",\"\",\"Storage Dimensions\",\"SpeedStor\"");
            v.Add("\"&HF7\",\"\",\"\",\"\",\"Filesystem\",\"Natalia Portillo\",\"O.S.G.\",\"EFAT\"");
            v.Add("\"&HF7\",\"\",\"\",\"\",\"Filesystem\",\"DDRdrive\",\"X1\",\"Solid State file system\"");
            v.Add("\"&HF9\",\"\",\"\",\"\",\"\",\"ALC Press\",\"Linux\",\"pCache ext2/ext3 persistent cache\"");
            v.Add("\"&HFA\",\"\",\"\",\"\",\"\",\"MandrakeSoft\",\"Bochs\",\"x86 emulator\"");
            v.Add("\"&HFB\",\"\",\"\",\"\",\"Filesystem\",\"VMware\",\"VMware\",\"VMware VMFS filesystem partition\"");
            v.Add("\"&HFC\",\"\",\"\",\"No\",\"\",\"VMware\",\"VMware\",\"VMware swap / VMKCORE kernel dump partition\"");
            v.Add("\"&HFD\",\"\",\"\",\"\",\"\",\"GNU/Linux\",\"Linux\",\"Linux RAID superblock with auto-detect\"");
            v.Add("\"&HFD\",\"MBR, EBR\",\"CHS, LBA\",\"x86\",\"\",\"FreeDOS\",\"FreeDOS\",\"Reserved for FreeDOS\"");
            v.Add("\"&HFE\",\"\",\"\",\"\",\"\",\"Storage Dimensions\",\"SpeedStor\",\"partition > 1024 cylinder\"");
            v.Add("\"&HFE\",\"\",\"\",\"\",\"\",\"\",\"Intel\",\"LANstep\"");
            v.Add("\"&HFE\",\"\",\"\",\"\",\"Hidden, Service\",\"IBM\",\"\",\"PS/2 IML partition\"");
            v.Add("\"&HFE\",\"MBR\",\"CHS, LBA\",\"x86\",\"Hidden, Service, Filesystem\",\"IBM\",\"\",\"PS/2 recovery partition (FAT12 reference disk floppy image), (corresponds with 01h if activated, all other partitions +10h then)\"");
            v.Add("\"&HFE\",\"\",\"\",\"\",\"Hidden\",\"Microsoft\",\"Windows NT\",\"Disk Administration hidden partition\"");
            v.Add("\"&HFE\",\"\",\"\",\"\",\"\",\"\",\"Linux\",\"old Linux LVM\"");
            v.Add("\"&HFF\",\"MBR\",\"CHS\",\"No\",\"\",\"Microsoft\",\"XENIX\",\"XENIX bad block table\"");
            return v.ToArray();
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }

}
