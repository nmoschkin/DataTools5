using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Win32Api.Disk.Partition
{

    /// <summary>
    /// Partition bootability flags
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    public enum PartitionBootability
    {

        /// <summary>
        /// No information.
        /// </summary>
        /// <remarks></remarks>
        [Description("No Information")]
        NoInfo = 0,

        /// <summary>
        /// Generally not bootable.
        /// </summary>
        /// <remarks></remarks>
        [Description("Generally Not Bootable")]
        No = 1,

        /// <summary>
        /// Generally bootable.
        /// </summary>
        /// <remarks></remarks>
        [Description("Generally Bootable")]
        Yes = 2,

        /// <summary>
        /// Bootable on x86 systems.
        /// </summary>
        /// <remarks></remarks>
        [Description("Bootable on x86 systems")]
        x86 = 4,

        /// <summary>
        /// Bootable on 286 or greater systems.
        /// </summary>
        /// <remarks></remarks>
        [Description("Bootable on i286 or greater systems")]
        I286 = 8,

        /// <summary>
        /// Bootable on 386 or greater systems.
        /// </summary>
        /// <remarks></remarks>
        [Description("Bootable on i386 or greater systems")]
        I386 = 0x10,

        /// <summary>
        /// Bootable on 486 or greater systems.
        /// </summary>
        /// <remarks></remarks>
        [Description("Bootable on i486 or greater systems")]
        I486 = 0x20,

        /// <summary>
        /// Bootable on 586/Pentium or greater systems.
        /// </summary>
        /// <remarks></remarks>
        [Description("Bootable on i586/Pentium or greater systems")]
        I586 = 0x40,

        /// <summary>
        /// Bootable on Motorola 68000 systems.
        /// </summary>
        /// <remarks></remarks>
        [Description("Bootable on Motorola 68000 systems")]
        M68000 = 0x100,

        /// <summary>
        /// Bootable on Intel 8080 / Zilog Z80 systems.
        /// </summary>
        /// <remarks></remarks>
        [Description("Bootable on Intel 8080/Zilog Z80 systems")]
        I8080Z80 = 0x200,

        /// <summary>
        /// Bootable as an Adavanced Active Partition.
        /// </summary>
        /// <remarks></remarks>
        [Description("Bootable as an Advanced Active Partition")]
        AAP = 0x400,

        /// <summary>
        /// Bootable on PowerPC systems.
        /// </summary>
        /// <remarks></remarks>
        [Description("Bootable on PowerPC systems")]
        PowerPC = 0x800
    }

}
