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
    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */    // From Microsoft:

    //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    //                                                                          ''
    // Media types                                                              ''
    //                                                                          ''
    // These are enumerated values of the ifType object defined in MIB-II's     ''
    // ifTable.  They are registered with IANA which publishes this list        ''
    // periodically, in either the Assigned Numbers RFC, or some derivative     ''
    // of it specific to Internet Network Management number assignments.        ''
    // See ftp:''ftp.isi.edu/mib/ianaiftype.mib                                 ''
    //                                                                          ''
    //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    public enum IFTYPE
    {

        /// <summary>
        /// Minimum IF_TYPE integer value present in this enumeration.
        /// </summary>
        /// <remarks></remarks>
        MIN_IF_TYPE = 1,

        /// <summary>
        /// None of the below
        /// </summary>
        /// <remarks></remarks>
        OTHER = 1,
        REGULAR_1822 = 2,
        HDH_1822 = 3,
        DDN_X25 = 4,
        RFC877_X25 = 5,

        /// <summary>
        /// Ethernet adapter
        /// </summary>
        /// <remarks></remarks>
        ETHERNET_CSMACD = 6,
        IS088023_CSMACD = 7,
        ISO88024_TOKENBUS = 8,
        ISO88025_TOKENRING = 9,
        ISO88026_MAN = 10,
        STARLAN = 11,
        PROTEON_10MBIT = 12,
        PROTEON_80MBIT = 13,
        HYPERCHANNEL = 14,
        FDDI = 15,
        LAP_B = 16,
        SDLC = 17,

        /// <summary>
        /// DS1-MIB
        /// </summary>
        /// <remarks></remarks>
        DS1 = 18,

        /// <summary>
        /// Obsolete; see DS1-MIB
        /// </summary>
        /// <remarks></remarks>
        E1 = 19,
        BASIC_ISDN = 20,
        PRIMARY_ISDN = 21,
        /// <summary>
        /// proprietary serial
        /// </summary>
        /// <remarks></remarks>
        PROP_POINT2POINT_SERIAL = 22,
        PPP = 23,
        SOFTWARE_LOOPBACK = 24,

        /// <summary>
        /// CLNP over IP
        /// </summary>
        /// <remarks></remarks>
        EON = 25,
        ETHERNET_3MBIT = 26,

        /// <summary>
        /// XNS over IP
        /// </summary>
        /// <remarks></remarks>
        NSIP = 27,

        /// <summary>
        /// Generic Slip
        /// </summary>
        /// <remarks></remarks>
        SLIP = 28,

        /// <summary>
        /// ULTRA Technologies
        /// </summary>
        /// <remarks></remarks>
        ULTRA = 29,

        /// <summary>
        /// DS3-MIB
        /// </summary>
        /// <remarks></remarks>
        DS3 = 30,

        /// <summary>
        /// SMDS, coffee
        /// </summary>
        /// <remarks></remarks>
        SIP = 31,

        /// <summary>
        /// DTE only
        /// </summary>
        /// <remarks></remarks>
        FRAMERELAY = 32,
        RS232 = 33,

        /// <summary>
        /// Parallel port
        /// </summary>
        /// <remarks></remarks>
        PARA = 34,
        ARCNET = 35,
        ARCNET_PLUS = 36,

        /// <summary>
        /// ATM cells
        /// </summary>
        /// <remarks></remarks>
        ATM = 37,
        MIO_X25 = 38,

        /// <summary>
        /// SONET or SDH
        /// </summary>
        /// <remarks></remarks>
        SONET = 39,
        X25_PLE = 40,
        ISO88022_LLC = 41,
        LOCALTALK = 42,
        SMDS_DXI = 43,

        /// <summary>
        /// FRNETSERV-MIB
        /// </summary>
        /// <remarks></remarks>
        FRAMERELAY_SERVICE = 44,
        V35 = 45,
        HSSI = 46,
        HIPPI = 47,

        /// <summary>
        /// Generic Modem
        /// </summary>
        /// <remarks></remarks>
        MODEM = 48,

        /// <summary>
        /// AAL5 over ATM
        /// </summary>
        /// <remarks></remarks>
        AAL5 = 49,
        SONET_PATH = 50,
        SONET_VT = 51,

        /// <summary>
        /// SMDS InterCarrier Interface
        /// </summary>
        /// <remarks></remarks>
        SMDS_ICIP = 52,

        /// <summary>
        /// Proprietary virtual/internal
        /// </summary>
        /// <remarks></remarks>
        PROP_VIRTUAL = 53,

        /// <summary>
        /// Proprietary multiplexing
        /// </summary>
        /// <remarks></remarks>
        PROP_MULTIPLEXOR = 54,

        /// <summary>
        /// 100BaseVG
        /// </summary>
        /// <remarks></remarks>
        IEEE80212 = 55,
        FIBRECHANNEL = 56,
        HIPPIINTERFACE = 57,

        /// <summary>
        /// Obsolete, use 32 or 44
        /// </summary>
        /// <remarks></remarks>
        FRAMERELAY_INTERCONNECT = 58,

        /// <summary>
        /// ATM Emulated LAN for 802.3
        /// </summary>
        /// <remarks></remarks>
        AFLANE_8023 = 59,

        /// <summary>
        /// ATM Emulated LAN for 802.5
        /// </summary>
        /// <remarks></remarks>
        AFLANE_8025 = 60,

        /// <summary>
        /// ATM Emulated circuit
        /// </summary>
        /// <remarks></remarks>
        CCTEMUL = 61,

        /// <summary>
        /// Fast Ethernet (100BaseT)
        /// </summary>
        /// <remarks></remarks>
        FASTETHER = 62,

        /// <summary>
        /// ISDN and X.25
        /// </summary>
        /// <remarks></remarks>
        ISDN = 63,

        /// <summary>
        /// CCITT V.11/X.21
        /// </summary>
        /// <remarks></remarks>
        V11 = 64,

        /// <summary>
        /// CCITT V.36
        /// </summary>
        /// <remarks></remarks>
        V36 = 65,

        /// <summary>
        /// CCITT G703 at 64Kbps
        /// </summary>
        /// <remarks></remarks>
        G703_64K = 66,

        /// <summary>
        /// Obsolete; see DS1-MIB
        /// </summary>
        /// <remarks></remarks>
        G703_2MB = 67,

        /// <summary>
        /// SNA QLLC
        /// </summary>
        /// <remarks></remarks>
        QLLC = 68,

        /// <summary>
        /// Fast Ethernet (100BaseFX)
        /// </summary>
        /// <remarks></remarks>
        FASTETHER_FX = 69,
        CHANNEL = 70,

        /// <summary>
        /// Radio spread spectrum - WiFi
        /// </summary>
        /// <remarks></remarks>
        IEEE80211 = 71,

        /// <summary>
        /// IBM System 360/370 OEMI Channel
        /// </summary>
        /// <remarks></remarks>
        IBM370PARCHAN = 72,

        /// <summary>
        /// IBM Enterprise Systems Connection
        /// </summary>
        /// <remarks></remarks>
        ESCON = 73,

        /// <summary>
        /// Data Link Switching
        /// </summary>
        /// <remarks></remarks>
        DLSW = 74,

        /// <summary>
        /// ISDN S/T interface
        /// </summary>
        /// <remarks></remarks>
        ISDN_S = 75,

        /// <summary>
        /// ISDN U interface
        /// </summary>
        /// <remarks></remarks>
        ISDN_U = 76,

        /// <summary>
        /// Link Access Protocol D
        /// </summary>
        /// <remarks></remarks>
        LAP_D = 77,

        /// <summary>
        /// IP Switching Objects
        /// </summary>
        /// <remarks></remarks>
        IPSWITCH = 78,

        /// <summary>
        /// Remote Source Route Bridging
        /// </summary>
        /// <remarks></remarks>
        RSRB = 79,

        /// <summary>
        /// ATM Logical Port
        /// </summary>
        /// <remarks></remarks>
        ATM_LOGICAL = 80,

        /// <summary>
        /// Digital Signal Level 0
        /// </summary>
        /// <remarks></remarks>
        DS0 = 81,

        /// <summary>
        /// Group of ds0s on the same ds1
        /// </summary>
        /// <remarks></remarks>
        DS0_BUNDLE = 82,

        /// <summary>
        /// Bisynchronous Protocol
        /// </summary>
        /// <remarks></remarks>
        BSC = 83,

        /// <summary>
        /// Asynchronous Protocol
        /// </summary>
        /// <remarks></remarks>
        ASYNC = 84,

        /// <summary>
        /// Combat Net Radio
        /// </summary>
        /// <remarks></remarks>
        CNR = 85,

        /// <summary>
        /// ISO 802.5r DTR
        /// </summary>
        /// <remarks></remarks>
        ISO88025R_DTR = 86,

        /// <summary>
        /// Ext Pos Loc Report Sys
        /// </summary>
        /// <remarks></remarks>
        EPLRS = 87,

        /// <summary>
        /// Appletalk Remote Access Protocol
        /// </summary>
        /// <remarks></remarks>
        ARAP = 88,

        /// <summary>
        /// Proprietary Connectionless Proto
        /// </summary>
        /// <remarks></remarks>
        PROP_CNLS = 89,

        /// <summary>
        /// CCITT-ITU X.29 PAD Protocol
        /// </summary>
        /// <remarks></remarks>
        HOSTPAD = 90,

        /// <summary>
        /// CCITT-ITU X.3 PAD Facility
        /// </summary>
        /// <remarks></remarks>
        TERMPAD = 91,

        /// <summary>
        /// Multiproto Interconnect over FR
        /// </summary>
        /// <remarks></remarks>
        FRAMERELAY_MPI = 92,

        /// <summary>
        /// CCITT-ITU X213
        /// </summary>
        /// <remarks></remarks>
        X213 = 93,

        /// <summary>
        /// Asymmetric Digital Subscrbr Loop
        /// </summary>
        /// <remarks></remarks>
        ADSL = 94,

        /// <summary>
        /// Rate-Adapt Digital Subscrbr Loop
        /// </summary>
        /// <remarks></remarks>
        RADSL = 95,

        /// <summary>
        /// Symmetric Digital Subscriber Loop
        /// </summary>
        /// <remarks></remarks>
        SDSL = 96,

        /// <summary>
        /// Very H-Speed Digital Subscrb Loop
        /// </summary>
        /// <remarks></remarks>
        VDSL = 97,

        /// <summary>
        /// ISO 802.5 CRFP
        /// </summary>
        /// <remarks></remarks>
        ISO88025_CRFPRINT = 98,

        /// <summary>
        /// Myricom Myrinet
        /// </summary>
        /// <remarks></remarks>
        MYRInet = 99,

        /// <summary>
        /// Voice recEive and transMit
        /// </summary>
        /// <remarks></remarks>
        VOICE_EM = 100,

        /// <summary>
        /// Voice Foreign Exchange Office
        /// </summary>
        /// <remarks></remarks>
        VOICE_FXO = 101,

        /// <summary>
        /// Voice Foreign Exchange Station
        /// </summary>
        /// <remarks></remarks>
        VOICE_FXS = 102,

        /// <summary>
        /// Voice encapsulation
        /// </summary>
        /// <remarks></remarks>
        VOICE_ENCAP = 103,

        /// <summary>
        /// Voice over IP encapsulation
        /// </summary>
        /// <remarks></remarks>
        VOICE_OVERIP = 104,

        /// <summary>
        /// ATM DXI
        /// </summary>
        /// <remarks></remarks>
        ATM_DXI = 105,

        /// <summary>
        /// ATM FUNI
        /// </summary>
        /// <remarks></remarks>
        ATM_FUNI = 106,

        /// <summary>
        /// ATM IMA
        /// </summary>
        /// <remarks></remarks>
        ATM_IMA = 107,

        /// <summary>
        /// PPP Multilink Bundle
        /// </summary>
        /// <remarks></remarks>
        PPPMULTILINKBUNDLE = 108,

        /// <summary>
        /// IBM ipOverCdlc
        /// </summary>
        /// <remarks></remarks>
        IPOVER_CDLC = 109,

        /// <summary>
        /// IBM Common Link Access to Workstn
        /// </summary>
        /// <remarks></remarks>
        IPOVER_CLAW = 110,

        /// <summary>
        /// IBM stackToStack
        /// </summary>
        /// <remarks></remarks>
        STACKTOSTACK = 111,

        /// <summary>
        /// IBM VIPA
        /// </summary>
        /// <remarks></remarks>
        VIRTUALIPADDRESS = 112,

        /// <summary>
        /// IBM multi-proto channel support
        /// </summary>
        /// <remarks></remarks>
        MPC = 113,

        /// <summary>
        /// IBM ipOverAtm
        /// </summary>
        /// <remarks></remarks>
        IPOVER_ATM = 114,

        /// <summary>
        /// ISO 802.5j Fiber Token Ring
        /// </summary>
        /// <remarks></remarks>
        ISO88025_FIBER = 115,

        /// <summary>
        /// IBM twinaxial data link control
        /// </summary>
        /// <remarks></remarks>
        TDLC = 116,
        GIGABITETHERNET = 117,
        HDLC = 118,
        LAP_F = 119,
        V37 = 120,

        /// <summary>
        /// Multi-Link Protocol
        /// </summary>
        /// <remarks></remarks>
        X25_MLP = 121,

        /// <summary>
        /// X.25 Hunt Group
        /// </summary>
        /// <remarks></remarks>
        X25_HUNTGROUP = 122,
        TRANSPHDLC = 123,

        /// <summary>
        /// Interleave channel
        /// </summary>
        /// <remarks></remarks>
        INTERLEAVE = 124,

        /// <summary>
        /// Fast channel
        /// </summary>
        /// <remarks></remarks>
        FAST = 125,

        /// <summary>
        /// IP (for APPN HPR in IP networks)
        /// </summary>
        /// <remarks></remarks>
        IP = 126,

        /// <summary>
        /// CATV Mac Layer
        /// </summary>
        /// <remarks></remarks>
        DOCSCABLE_MACLAYER = 127,

        /// <summary>
        /// CATV Downstream interface
        /// </summary>
        /// <remarks></remarks>
        DOCSCABLE_DOWNSTREAM = 128,

        /// <summary>
        /// CATV Upstream interface
        /// </summary>
        /// <remarks></remarks>
        DOCSCABLE_UPSTREAM = 129,

        /// <summary>
        /// Avalon Parallel Processor
        /// </summary>
        /// <remarks></remarks>
        A12MPPSWITCH = 130,

        /// <summary>
        /// Encapsulation interface
        /// </summary>
        /// <remarks></remarks>
        TUNNEL = 131,

        /// <summary>
        /// Coffee pot
        /// </summary>
        /// <remarks></remarks>
        COFFEE = 132,

        /// <summary>
        /// Circuit Emulation Service
        /// </summary>
        /// <remarks></remarks>
        CES = 133,

        /// <summary>
        /// ATM Sub Interface
        /// </summary>
        /// <remarks></remarks>
        ATM_SUBINTERFACE = 134,

        /// <summary>
        /// Layer 2 Virtual LAN using 802.1Q
        /// </summary>
        /// <remarks></remarks>
        L2_VLAN = 135,

        /// <summary>
        /// Layer 3 Virtual LAN using IP
        /// </summary>
        /// <remarks></remarks>
        L3_IPVLAN = 136,

        /// <summary>
        /// Layer 3 Virtual LAN using IPX
        /// </summary>
        /// <remarks></remarks>
        L3_IPXVLAN = 137,

        /// <summary>
        /// IP over Power Lines
        /// </summary>
        /// <remarks></remarks>
        DIGITALPOWERLINE = 138,

        /// <summary>
        /// Multimedia Mail over IP
        /// </summary>
        /// <remarks></remarks>
        MEDIAMAILOVERIP = 139,

        /// <summary>
        /// Dynamic syncronous Transfer Mode
        /// </summary>
        /// <remarks></remarks>
        DTM = 140,

        /// <summary>
        /// Data Communications Network
        /// </summary>
        /// <remarks></remarks>
        DCN = 141,

        /// <summary>
        /// IP Forwarding Interface
        /// </summary>
        /// <remarks></remarks>
        IPFORWARD = 142,

        /// <summary>
        /// Multi-rate Symmetric DSL
        /// </summary>
        /// <remarks></remarks>
        MSDSL = 143,

        /// <summary>
        /// IEEE1394 High Perf Serial Bus
        /// </summary>
        /// <remarks></remarks>
        IEEE1394 = 144,
        IF_GSN = 145,
        DVBRCC_MACLAYER = 146,
        DVBRCC_DOWNSTREAM = 147,
        DVBRCC_UPSTREAM = 148,
        ATM_VIRTUAL = 149,
        MPLS_TUNNEL = 150,
        SRP = 151,
        VOICEOVERATM = 152,
        VOICEOVERFRAMERELAY = 153,
        IDSL = 154,
        COMPOSITELINK = 155,
        SS7_SIGLINK = 156,
        PROP_WIRELESS_P2P = 157,
        FR_FORWARD = 158,
        RFC1483 = 159,
        USB = 160,
        IEEE8023AD_LAG = 161,
        BGP_POLICY_ACCOUNTING = 162,
        FRF16_MFR_BUNDLE = 163,
        H323_GATEKEEPER = 164,
        H323_PROXY = 165,
        MPLS = 166,
        MF_SIGLINK = 167,
        HDSL2 = 168,
        SHDSL = 169,
        DS1_FDL = 170,
        POS = 171,
        DVB_ASI_IN = 172,
        DVB_ASI_OUT = 173,
        PLC = 174,
        NFAS = 175,
        TR008 = 176,
        GR303_RDT = 177,
        GR303_IDT = 178,
        ISUP = 179,
        PROP_DOCS_WIRELESS_MACLAYER = 180,
        PROP_DOCS_WIRELESS_DOWNSTREAM = 181,
        PROP_DOCS_WIRELESS_UPSTREAM = 182,
        HIPERLAN2 = 183,
        PROP_BWA_P2MP = 184,
        SONET_OVERHEAD_CHANNEL = 185,
        DIGITAL_WRAPPER_OVERHEAD_CHANNEL = 186,
        AAL2 = 187,
        RADIO_MAC = 188,
        ATM_RADIO = 189,
        IMT = 190,
        MVL = 191,
        REACH_DSL = 192,
        FR_DLCI_ENDPT = 193,
        ATM_VCI_ENDPT = 194,
        OPTICAL_CHANNEL = 195,
        OPTICAL_TRANSPORT = 196,
        IEEE80216_WMAN = 237,

        /// <summary>
        /// WWAN devices based on GSM technology
        /// </summary>
        /// <remarks></remarks>
        WWANPP = 243,

        /// <summary>
        /// WWAN devices based on CDMA technology
        /// </summary>
        /// <remarks></remarks>
        WWANPP2 = 244,

        /// <summary>
        /// Maximum IF_TYPE integer value present in this enumeration.
        /// </summary>
        /// <remarks></remarks>
        MAX_IF_TYPE = 244
    }
}
