// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: IfDefApi
//         The almighty network interface native API.
//         Some enum documentation comes from the MSDN.
//
// (and an exercise in creative problem solving and data-structure marshaling.)
//
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''


using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using DataTools.Memory;
using DataTools.Win32;

namespace DataTools.Win32.Network
{
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Network adapter address families.
    /// </summary>
    /// <remarks></remarks>
    public enum AddressFamily : ushort
    {

        /// <summary>
        /// unspecified
        /// </summary>
        /// <remarks></remarks>
        AfUnspecified = 0,

        /// <summary>
        /// local to host (pipes, portals)
        /// </summary>
        /// <remarks></remarks>
        AfUNIX = 1,

        /// <summary>
        /// internetwork: UDP, TCP, etc.
        /// </summary>
        /// <remarks></remarks>
        AfInet = 2,

        /// <summary>
        /// arpanet imp addresses
        /// </summary>
        /// <remarks></remarks>
        AfIMPLINK = 3,

        /// <summary>
        /// pup protocols: e.g. BSP
        /// </summary>
        /// <remarks></remarks>
        AfPUP = 4,

        /// <summary>
        /// mit CHAOS protocols
        /// </summary>
        /// <remarks></remarks>
        AfCHAOS = 5,

        /// <summary>
        /// XEROX NS protocols
        /// </summary>
        /// <remarks></remarks>
        AfNS = 6,

        /// <summary>
        /// IPX protocols: IPX, SPX, etc.
        /// </summary>
        /// <remarks></remarks>
        AfIPX = AfNS,

        /// <summary>
        /// ISO protocols
        /// </summary>
        /// <remarks></remarks>
        AfISO = 7,

        /// <summary>
        /// OSI is ISO
        /// </summary>
        /// <remarks></remarks>
        AfOSI = AfISO,

        /// <summary>
        /// european computer manufacturers
        /// </summary>
        /// <remarks></remarks>
        AfECMA = 8,

        /// <summary>
        /// datakit protocols
        /// </summary>
        /// <remarks></remarks>
        AfDataKit = 9,

        /// <summary>
        /// CCITT protocols, X.25 etc
        /// </summary>
        /// <remarks></remarks>
        AfCCITT = 10,

        /// <summary>
        /// IBM SNA
        /// </summary>
        /// <remarks></remarks>
        AfSNA = 11,

        /// <summary>
        /// DECnet
        /// </summary>
        /// <remarks></remarks>
        AfDECnet = 12,

        /// <summary>
        /// Direct data link interface
        /// </summary>
        /// <remarks></remarks>
        AfDLI = 13,

        /// <summary>
        /// LAT
        /// </summary>
        /// <remarks></remarks>
        AfLAT = 14,

        /// <summary>
        /// NSC Hyperchannel
        /// </summary>
        /// <remarks></remarks>
        AfHYLINK = 15,

        /// <summary>
        /// AppleTalk
        /// </summary>
        /// <remarks></remarks>
        AfAppleTalk = 16,

        /// <summary>
        /// NetBios-style addresses
        /// </summary>
        /// <remarks></remarks>
        AfNetBios = 17,

        /// <summary>
        /// VoiceView
        /// </summary>
        /// <remarks></remarks>
        AfVoiceView = 18,

        /// <summary>
        /// Protocols from Firefox
        /// </summary>
        /// <remarks></remarks>
        AfFirefox = 19,

        /// <summary>
        /// Somebody is using this!
        /// </summary>
        /// <remarks></remarks>
        AfUnknown1 = 20,

        /// <summary>
        /// Banyan
        /// </summary>
        /// <remarks></remarks>
        AfBAN = 21,

        /// <summary>
        /// Native ATM Services
        /// </summary>
        /// <remarks></remarks>
        AfATM = 22,

        /// <summary>
        /// Internetwork Version 6
        /// </summary>
        /// <remarks></remarks>
        AfInet6 = 23,

        /// <summary>
        /// Microsoft Wolfpack
        /// </summary>
        /// <remarks></remarks>
        AfCLUSTER = 24,

        /// <summary>
        /// IEEE 1284.4 WG AF
        /// </summary>
        /// <remarks></remarks>
        Af12844 = 25,

        /// <summary>
        /// IrDA
        /// </summary>
        /// <remarks></remarks>
        AfIRDA = 26,

        /// <summary>
        /// Network Designers OSI &amp; gateway
        /// </summary>
        /// <remarks></remarks>
        AfNETDES = 28,

        /// <summary>
        /// 
        /// </summary>
        /// <remarks></remarks>
        AfTCNProcess = 29,

        /// <summary>
        /// 
        /// </summary>
        /// <remarks></remarks>
        AfTCNMessage = 30,

        /// <summary>
        /// 
        /// </summary>
        /// <remarks></remarks>
        AfICLFXBM = 31
    }
}
