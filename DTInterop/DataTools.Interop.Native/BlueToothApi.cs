// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library 
// '
// ' Module: Bluetooth API
// '         Complete Translation of
// '         BluetoothAPI.h
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CoreCT.Memory;
using CoreCT.Text;

namespace DataTools.Interop.Native
{
    internal static class BluetoothApi
    {
        // TODO !!!

    

        // ' {0850302A-B344-4fda-9BE9-90576B8D46F0}
        public readonly static Guid GUID_BTHPORT_DEVICE_INTERFACE = new Guid(0x850302AU, 0xB344, 0x4FDA, 0x9B, 0xE9, 0x90, 0x57, 0x6B, 0x8D, 0x46, 0xF0);

        // ' RFCOMM device interface GUID for RFCOMM services
        // ' {b142fc3e-fa4e-460b-8abc-072b628b3c70}
        public readonly static Guid GUID_BTH_RFCOMM_SERVICE_DEVICE_INTERFACE = new Guid(0xB142FC3E, 0xFA4E, 0x460B, 0x8A, 0xBC, 0x7, 0x2B, 0x62, 0x8B, 0x3C, 0x70);

        // ' {EA3B5B82-26EE-450E-B0D8-D26FE30A3869}
        public readonly static Guid GUID_BLUETOOTH_RADIO_IN_RANGE = new Guid(0xEA3B5B82, 0x26EE, 0x450E, 0xB0, 0xD8, 0xD2, 0x6F, 0xE3, 0xA, 0x38, 0x69);

        // ' {E28867C9-C2AA-4CED-B969-4570866037C4}
        public readonly static Guid GUID_BLUETOOTH_RADIO_OUT_OF_RANGE = new Guid(0xE28867C9, 0xC2AA, 0x4CED, 0xB9, 0x69, 0x45, 0x70, 0x86, 0x60, 0x37, 0xC4);

        // ' {7EAE4030-B709-4AA8-AC55-E953829C9DAA}
        public readonly static Guid GUID_BLUETOOTH_L2CAP_EVENT = new Guid(0x7EAE4030U, 0xB709, 0x4AA8, 0xAC, 0x55, 0xE9, 0x53, 0x82, 0x9C, 0x9D, 0xAA);

        // ' {FC240062-1541-49BE-B463-84C4DCD7BF7F}
        public readonly static Guid GUID_BLUETOOTH_HCI_EVENT = new Guid(0xFC240062, 0x1541, 0x49BE, 0xB4, 0x63, 0x84, 0xC4, 0xDC, 0xD7, 0xBF, 0x7F);

        // '
        // ' Support added in KB942567

        // ' {5DC9136D-996C-46DB-84F5-32C0A3F47352}
        public readonly static Guid GUID_BLUETOOTH_AUTHENTICATION_REQUEST = new Guid(0x5DC9136DU, 0x996C, 0x46DB, 0x84, 0xF5, 0x32, 0xC0, 0xA3, 0xF4, 0x73, 0x52);

        // ' {D668DFCD-0F4E-4EFC-BFE0-392EEEC5109C}
        public readonly static Guid GUID_BLUETOOTH_KEYPRESS_EVENT = new Guid(0xD668DFCD, 0xF4E, 0x4EFC, 0xBF, 0xE0, 0x39, 0x2E, 0xEE, 0xC5, 0x10, 0x9C);

        // ' {547247e6-45bb-4c33-af8c-c00efe15a71d}
        public readonly static Guid GUID_BLUETOOTH_HCI_VENDOR_EVENT = new Guid(0x547247E6, 0x45BB, 0x4C33, 0xAF, 0x8C, 0xC0, 0xE, 0xFE, 0x15, 0xA7, 0x1D);


        // '
        // ' Bluetooth base UUID for service discovery
        // '
        public readonly static Guid Bluetooth_Base_UUID = new Guid(0x0, 0x0, 0x1000, 0x80, 0x0, 0x0, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        public static Guid DEFINE_BLUETOOTH_UUID128(ushort shortId)
        {

            // public readonly DEFINE_BLUETOOTH_UUID128 As UInteger = (name,shortId) _
            // DEFINE_GUID(name, shortId, &H0000, &H1000, &H80, &H00, &H00, &H80, &H5F, &H9B, &H34, &HFB)

            return new Guid(shortId, 0x0, 0x1000, 0x80, 0x0, 0x0, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        }



        // '
        // ' UUIDs for the Protocol Identifiers, Service Discovery Assigned Numbers
        // '
        public const ushort SDP_PROTOCOL_UUID16 = 0x1;
        public const ushort UDP_PROTOCOL_UUID16 = 0x2;
        public const ushort RFCOMM_PROTOCOL_UUID16 = 0x3;
        public const ushort TCP_PROTOCOL_UUID16 = 0x4;
        public const ushort TCSBIN_PROTOCOL_UUID16 = 0x5;
        public const ushort TCSAT_PROTOCOL_UUID16 = 0x6;
        public const ushort ATT_PROTOCOL_UUID16 = 0x7;
        public const ushort OBEX_PROTOCOL_UUID16 = 0x8;
        public const ushort IP_PROTOCOL_UUID16 = 0x9;
        public const ushort FTP_PROTOCOL_UUID16 = 0xA;
        public const ushort HTTP_PROTOCOL_UUID16 = 0xC;
        public const ushort WSP_PROTOCOL_UUID16 = 0xE;
        public const ushort BNEP_PROTOCOL_UUID16 = 0xF;
        public const ushort UPNP_PROTOCOL_UUID16 = 0x10;
        public const ushort HID_PROTOCOL_UUID16 = 0x11;
        public const ushort HCCC_PROTOCOL_UUID16 = 0x12;
        public const ushort HCDC_PROTOCOL_UUID16 = 0x14;
        public const ushort HCN_PROTOCOL_UUID16 = 0x16;
        public const ushort AVCTP_PROTOCOL_UUID16 = 0x17;
        public const ushort AVDTP_PROTOCOL_UUID16 = 0x19;
        public const ushort CMPT_PROTOCOL_UUID16 = 0x1B;
        public const ushort UDI_C_PLANE_PROTOCOL_UUID16 = 0x1D;
        public const ushort L2CAP_PROTOCOL_UUID16 = 0x100;
        public readonly static Guid SDP_PROTOCOL_UUID = DEFINE_BLUETOOTH_UUID128(SDP_PROTOCOL_UUID16);
        public readonly static Guid UDP_PROTOCOL_UUID = DEFINE_BLUETOOTH_UUID128(UDP_PROTOCOL_UUID16);
        public readonly static Guid RFCOMM_PROTOCOL_UUID = DEFINE_BLUETOOTH_UUID128(RFCOMM_PROTOCOL_UUID16);
        public readonly static Guid TCP_PROTOCOL_UUID = DEFINE_BLUETOOTH_UUID128(TCP_PROTOCOL_UUID16);
        public readonly static Guid TCSBIN_PROTOCOL_UUID = DEFINE_BLUETOOTH_UUID128(TCSBIN_PROTOCOL_UUID16);
        public readonly static Guid TCSAT_PROTOCOL_UUID = DEFINE_BLUETOOTH_UUID128(TCSAT_PROTOCOL_UUID16);
        public readonly static Guid ATT_PROTOCOL_UUID = DEFINE_BLUETOOTH_UUID128(ATT_PROTOCOL_UUID16);
        public readonly static Guid OBEX_PROTOCOL_UUID = DEFINE_BLUETOOTH_UUID128(OBEX_PROTOCOL_UUID16);
        public readonly static Guid IP_PROTOCOL_UUID = DEFINE_BLUETOOTH_UUID128(IP_PROTOCOL_UUID16);
        public readonly static Guid FTP_PROTOCOL_UUID = DEFINE_BLUETOOTH_UUID128(FTP_PROTOCOL_UUID16);
        public readonly static Guid HTTP_PROTOCOL_UUID = DEFINE_BLUETOOTH_UUID128(HTTP_PROTOCOL_UUID16);
        public readonly static Guid WSP_PROTOCOL_UUID = DEFINE_BLUETOOTH_UUID128(WSP_PROTOCOL_UUID16);
        public readonly static Guid BNEP_PROTOCOL_UUID = DEFINE_BLUETOOTH_UUID128(BNEP_PROTOCOL_UUID16);
        public readonly static Guid UPNP_PROTOCOL_UUID = DEFINE_BLUETOOTH_UUID128(UPNP_PROTOCOL_UUID16);
        public readonly static Guid HID_PROTOCOL_UUID = DEFINE_BLUETOOTH_UUID128(HID_PROTOCOL_UUID16);
        public readonly static Guid HCCC_PROTOCOL_UUID = DEFINE_BLUETOOTH_UUID128(HCCC_PROTOCOL_UUID16);
        public readonly static Guid HCDC_PROTOCOL_UUID = DEFINE_BLUETOOTH_UUID128(HCDC_PROTOCOL_UUID16);
        public readonly static Guid HCN_PROTOCOL_UUID = DEFINE_BLUETOOTH_UUID128(HCN_PROTOCOL_UUID16);
        public readonly static Guid AVCTP_PROTOCOL_UUID = DEFINE_BLUETOOTH_UUID128(AVCTP_PROTOCOL_UUID16);
        public readonly static Guid AVDTP_PROTOCOL_UUID = DEFINE_BLUETOOTH_UUID128(AVDTP_PROTOCOL_UUID16);
        public readonly static Guid CMPT_PROTOCOL_UUID = DEFINE_BLUETOOTH_UUID128(CMPT_PROTOCOL_UUID16);
        public readonly static Guid UDI_C_PLANE_PROTOCOL_UUID = DEFINE_BLUETOOTH_UUID128(UDI_C_PLANE_PROTOCOL_UUID16);
        public readonly static Guid L2CAP_PROTOCOL_UUID = DEFINE_BLUETOOTH_UUID128(L2CAP_PROTOCOL_UUID16);

        // '
        // ' UUIDs for Service Class IDs, Service Discovery Assigned Numbers
        // '
        public const ushort ServiceDiscoveryServerServiceClassID_UUID16 = 0x1000;
        public const ushort BrowseGroupDescriptorServiceClassID_UUID16 = 0x1001;
        public const ushort PublicBrowseGroupServiceClassID_UUID16 = 0x1002;
        public const ushort SerialPortServiceClassID_UUID16 = 0x1101;
        public const ushort LANAccessUsingPPPServiceClassID_UUID16 = 0x1102;
        public const ushort DialupNetworkingServiceClassID_UUID16 = 0x1103;
        public const ushort IrMCSyncServiceClassID_UUID16 = 0x1104;
        public const ushort OBEXObjectPushServiceClassID_UUID16 = 0x1105;
        public const ushort OBEXFileTransferServiceClassID_UUID16 = 0x1106;
        public const ushort IrMcSyncCommandServiceClassID_UUID16 = 0x1107;
        public const ushort HeadsetServiceClassID_UUID16 = 0x1108;
        public const ushort CordlessTelephonyServiceClassID_UUID16 = 0x1109;
        public const ushort AudioSourceServiceClassID_UUID16 = 0x110A;
        public const ushort AudioSinkServiceClassID_UUID16 = 0x110B;
        public const ushort AVRemoteControlTargetServiceClassID_UUID16 = 0x110C;
        public const ushort AVRemoteControlServiceClassID_UUID16 = 0x110E;
        public const ushort AVRemoteControlControllerServiceClass_UUID16 = 0x110F;
        public const ushort IntercomServiceClassID_UUID16 = 0x1110;
        public const ushort FaxServiceClassID_UUID16 = 0x1111;
        public const ushort HeadsetAudioGatewayServiceClassID_UUID16 = 0x1112;
        public const ushort WAPServiceClassID_UUID16 = 0x1113;
        public const ushort WAPClientServiceClassID_UUID16 = 0x1114;
        public const ushort PANUServiceClassID_UUID16 = 0x1115;
        public const ushort NAPServiceClassID_UUID16 = 0x1116;
        public const ushort GNServiceClassID_UUID16 = 0x1117;
        public const ushort DirectPrintingServiceClassID_UUID16 = 0x1118;
        public const ushort ReferencePrintingServiceClassID_UUID16 = 0x1119;
        public const ushort ImagingResponderServiceClassID_UUID16 = 0x111B;
        public const ushort ImagingAutomaticArchiveServiceClassID_UUID16 = 0x111C;
        public const ushort ImagingReferenceObjectsServiceClassID_UUID16 = 0x111D;
        public const ushort HandsfreeServiceClassID_UUID16 = 0x111E;
        public const ushort HandsfreeAudioGatewayServiceClassID_UUID16 = 0x111F;
        public const ushort DirectPrintingReferenceObjectsServiceClassID_UUID16 = 0x1120;
        public const ushort ReflectsUIServiceClassID_UUID16 = 0x1121;
        public const ushort PrintingStatusServiceClassID_UUID16 = 0x1123;
        public const ushort HumanInterfaceDeviceServiceClassID_UUID16 = 0x1124;
        public const ushort HCRPrintServiceClassID_UUID16 = 0x1126;
        public const ushort HCRScanServiceClassID_UUID16 = 0x1127;
        public const ushort CommonISDNAccessServiceClassID_UUID16 = 0x1128;
        public const ushort VideoConferencingGWServiceClassID_UUID16 = 0x1129;
        public const ushort UDIMTServiceClassID_UUID16 = 0x112A;
        public const ushort UDITAServiceClassID_UUID16 = 0x112B;
        public const ushort AudioVideoServiceClassID_UUID16 = 0x112C;
        public const ushort SimAccessServiceClassID_UUID16 = 0x112D;
        public const ushort PhonebookAccessPceServiceClassID_UUID16 = 0x112E;
        public const ushort PhonebookAccessPseServiceClassID_UUID16 = 0x112F;
        public const ushort HeadsetHSServiceClassID_UUID16 = 0x1131;
        public const ushort MessageAccessServerServiceClassID_UUID16 = 0x1132;
        public const ushort MessageNotificationServerServiceClassID_UUID16 = 0x1133;
        public const ushort GNSSServerServiceClassID_UUID16 = 0x1136;
        public const ushort ThreeDimensionalDisplayServiceClassID_UUID16 = 0x1137;
        public const ushort ThreeDimensionalGlassesServiceClassID_UUID16 = 0x1138;
        public const ushort MPSServiceClassID_UUID16 = 0x113B;
        public const ushort CTNAccessServiceClassID_UUID16 = 0x113C;
        public const ushort CTNNotificationServiceClassID_UUID16 = 0x113D;
        public const ushort PnPInformationServiceClassID_UUID16 = 0x1200;
        public const ushort GenericNetworkingServiceClassID_UUID16 = 0x1201;
        public const ushort GenericFileTransferServiceClassID_UUID16 = 0x1202;
        public const ushort GenericAudioServiceClassID_UUID16 = 0x1203;
        public const ushort GenericTelephonyServiceClassID_UUID16 = 0x1204;
        public const ushort UPnpServiceClassID_UUID16 = 0x1205;
        public const ushort UPnpIpServiceClassID_UUID16 = 0x1206;
        public const ushort ESdpUpnpIpPanServiceClassID_UUID16 = 0x1300;
        public const ushort ESdpUpnpIpLapServiceClassID_UUID16 = 0x1301;
        public const ushort ESdpUpnpL2capServiceClassID_UUID16 = 0x1302;
        public const ushort VideoSourceServiceClassID_UUID16 = 0x1303;
        public const ushort VideoSinkServiceClassID_UUID16 = 0x1304;
        public const ushort HealthDeviceProfileSourceServiceClassID_UUID16 = 0x1401;
        public const ushort HealthDeviceProfileSinkServiceClassID_UUID16 = 0x1402;
        public readonly static Guid ServiceDiscoveryServerServiceClassID_UUID = DEFINE_BLUETOOTH_UUID128(ServiceDiscoveryServerServiceClassID_UUID16);
        public readonly static Guid BrowseGroupDescriptorServiceClassID_UUID = DEFINE_BLUETOOTH_UUID128(BrowseGroupDescriptorServiceClassID_UUID16);
        public readonly static Guid PublicBrowseGroupServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(PublicBrowseGroupServiceClassID_UUID16);
        public readonly static Guid SerialPortServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(SerialPortServiceClassID_UUID16);
        public readonly static Guid LANAccessUsingPPPServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(LANAccessUsingPPPServiceClassID_UUID16);
        public readonly static Guid DialupNetworkingServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(DialupNetworkingServiceClassID_UUID16);
        public readonly static Guid IrMCSyncServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(IrMCSyncServiceClassID_UUID16);
        public readonly static Guid OBEXObjectPushServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(OBEXObjectPushServiceClassID_UUID16);
        public readonly static Guid OBEXFileTransferServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(OBEXFileTransferServiceClassID_UUID16);
        public readonly static Guid IrMCSyncCommandServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(IrMcSyncCommandServiceClassID_UUID16);
        public readonly static Guid HeadsetServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(HeadsetServiceClassID_UUID16);
        public readonly static Guid CordlessTelephonyServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(CordlessTelephonyServiceClassID_UUID16);
        public readonly static Guid AudioSourceServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(AudioSourceServiceClassID_UUID16);
        public readonly static Guid AudioSinkServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(AudioSinkServiceClassID_UUID16);
        public readonly static Guid AVRemoteControlTargetServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(AVRemoteControlTargetServiceClassID_UUID16);
        public readonly static Guid AVRemoteControlServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(AVRemoteControlServiceClassID_UUID16);
        public readonly static Guid AVRemoteControlControllerServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(AVRemoteControlControllerServiceClass_UUID16);
        public readonly static Guid IntercomServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(IntercomServiceClassID_UUID16);
        public readonly static Guid FaxServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(FaxServiceClassID_UUID16);
        public readonly static Guid HeadsetAudioGatewayServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(HeadsetAudioGatewayServiceClassID_UUID16);
        public readonly static Guid WAPServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(WAPServiceClassID_UUID16);
        public readonly static Guid WAPClientServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(WAPClientServiceClassID_UUID16);
        public readonly static Guid PANUServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(PANUServiceClassID_UUID16);
        public readonly static Guid NAPServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(NAPServiceClassID_UUID16);
        public readonly static Guid GNServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(GNServiceClassID_UUID16);
        public readonly static Guid DirectPrintingServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(DirectPrintingServiceClassID_UUID16);
        public readonly static Guid ReferencePrintingServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(ReferencePrintingServiceClassID_UUID16);
        public readonly static Guid ImagingResponderServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(ImagingResponderServiceClassID_UUID16);
        public readonly static Guid ImagingAutomaticArchiveServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(ImagingAutomaticArchiveServiceClassID_UUID16);
        public readonly static Guid ImagingReferenceObjectsServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(ImagingReferenceObjectsServiceClassID_UUID16);
        public readonly static Guid HandsfreeServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(HandsfreeServiceClassID_UUID16);
        public readonly static Guid HandsfreeAudioGatewayServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(HandsfreeAudioGatewayServiceClassID_UUID16);
        public readonly static Guid DirectPrintingReferenceObjectsServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(DirectPrintingReferenceObjectsServiceClassID_UUID16);
        public readonly static Guid ReflectedUIServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(ReflectsUIServiceClassID_UUID16);
        public readonly static Guid PrintingStatusServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(PrintingStatusServiceClassID_UUID16);
        public readonly static Guid HumanInterfaceDeviceServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(HumanInterfaceDeviceServiceClassID_UUID16);
        public readonly static Guid HCRPrintServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(HCRPrintServiceClassID_UUID16);
        public readonly static Guid HCRScanServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(HCRScanServiceClassID_UUID16);
        public readonly static Guid CommonISDNAccessServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(CommonISDNAccessServiceClassID_UUID16);
        public readonly static Guid VideoConferencingGWServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(VideoConferencingGWServiceClassID_UUID16);
        public readonly static Guid UDIMTServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(UDIMTServiceClassID_UUID16);
        public readonly static Guid UDITAServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(UDITAServiceClassID_UUID16);
        public readonly static Guid AudioVideoServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(AudioVideoServiceClassID_UUID16);
        public readonly static Guid SimAccessServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(SimAccessServiceClassID_UUID16);
        public readonly static Guid PhonebookAccessPceServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(PhonebookAccessPceServiceClassID_UUID16);
        public readonly static Guid PhonebookAccessPseServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(PhonebookAccessPseServiceClassID_UUID16);
        public readonly static Guid HeadsetHSServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(HeadsetHSServiceClassID_UUID16);
        public readonly static Guid MessageAccessServerServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(MessageAccessServerServiceClassID_UUID16);
        public readonly static Guid MessageNotificationServerServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(MessageNotificationServerServiceClassID_UUID16);
        public readonly static Guid GNSSServerServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(GNSSServerServiceClassID_UUID16);
        public readonly static Guid ThreeDimensionalDisplayServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(ThreeDimensionalDisplayServiceClassID_UUID16);
        public readonly static Guid ThreeDimensionalGlassesServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(ThreeDimensionalGlassesServiceClassID_UUID16);
        public readonly static Guid MPSServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(MPSServiceClassID_UUID16);
        public readonly static Guid CTNAccessServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(CTNAccessServiceClassID_UUID16);
        public readonly static Guid CTNNotificationServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(CTNNotificationServiceClassID_UUID16);
        public readonly static Guid PnPInformationServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(PnPInformationServiceClassID_UUID16);
        public readonly static Guid GenericNetworkingServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(GenericNetworkingServiceClassID_UUID16);
        public readonly static Guid GenericFileTransferServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(GenericFileTransferServiceClassID_UUID16);
        public readonly static Guid GenericAudioServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(GenericAudioServiceClassID_UUID16);
        public readonly static Guid GenericTelephonyServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(GenericTelephonyServiceClassID_UUID16);
        public readonly static Guid UPnpServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(UPnpServiceClassID_UUID16);
        public readonly static Guid UPnpIpServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(UPnpIpServiceClassID_UUID16);
        public readonly static Guid ESdpUpnpIpPanServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(ESdpUpnpIpPanServiceClassID_UUID16);
        public readonly static Guid ESdpUpnpIpLapServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(ESdpUpnpIpLapServiceClassID_UUID16);
        public readonly static Guid ESdpUpnpL2capServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(ESdpUpnpL2capServiceClassID_UUID16);
        public readonly static Guid VideoSourceServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(VideoSourceServiceClassID_UUID16);
        public readonly static Guid VideoSinkServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(VideoSinkServiceClassID_UUID16);
        public readonly static Guid HealthDeviceProfileSourceServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(HealthDeviceProfileSourceServiceClassID_UUID16);
        public readonly static Guid HealthDeviceProfileSinkServiceClass_UUID = DEFINE_BLUETOOTH_UUID128(HealthDeviceProfileSinkServiceClassID_UUID16);

        // '
        // ' UUIDs for SIG defined profiles, Service Discovery Assigned Numbers
        // '
        public const ushort AdvancedAudioDistributionProfileID_UUID16 = 0x110D;
        public const ushort ImagingServiceProfileID_UUID16 = 0x111A;
        public const ushort BasicPrintingProfileID_UUID16 = 0x1122;
        public const ushort HardcopyCableReplacementProfileID_UUID16 = 0x1125;
        public const ushort PhonebookAccessProfileID_UUID16 = 0x1130;
        public const ushort MessageAccessProfileID_UUID16 = 0x1134;
        public const ushort GNSSProfileID_UUID16 = 0x1135;
        public const ushort ThreeDimensionalSynchronizationProfileID_UUID16 = 0x1139;
        public const ushort MPSProfileID_UUID16 = 0x113A;
        public const ushort CTNProfileID_UUID16 = 0x113E;
        public const ushort VideoDistributionProfileID_UUID16 = 0x1305;
        public const ushort HealthDeviceProfileID_UUID16 = 0x1400;
        public readonly static Guid AdvancedAudioDistributionProfile_UUID = DEFINE_BLUETOOTH_UUID128(AdvancedAudioDistributionProfileID_UUID16);
        public readonly static Guid ImagingServiceProfile_UUID = DEFINE_BLUETOOTH_UUID128(ImagingServiceProfileID_UUID16);
        public readonly static Guid BasicPrintingProfile_UUID = DEFINE_BLUETOOTH_UUID128(BasicPrintingProfileID_UUID16);
        public readonly static Guid HardcopyCableReplacementProfile_UUID = DEFINE_BLUETOOTH_UUID128(HardcopyCableReplacementProfileID_UUID16);
        public readonly static Guid PhonebookAccessProfile_UUID = DEFINE_BLUETOOTH_UUID128(PhonebookAccessProfileID_UUID16);
        public readonly static Guid MessageAccessProfile_UUID = DEFINE_BLUETOOTH_UUID128(MessageAccessProfileID_UUID16);
        public readonly static Guid GNSSProfile_UUID = DEFINE_BLUETOOTH_UUID128(GNSSProfileID_UUID16);
        public readonly static Guid ThreeDimensionalSynchronizationProfile_UUID = DEFINE_BLUETOOTH_UUID128(ThreeDimensionalSynchronizationProfileID_UUID16);
        public readonly static Guid MPSProfile_UUID = DEFINE_BLUETOOTH_UUID128(MPSProfileID_UUID16);
        public readonly static Guid CTNProfile_UUID = DEFINE_BLUETOOTH_UUID128(CTNProfileID_UUID16);
        public readonly static Guid VideoDistributionProfile_UUID = DEFINE_BLUETOOTH_UUID128(VideoDistributionProfileID_UUID16);
        public readonly static Guid HealthDeviceProfile_UUID = DEFINE_BLUETOOTH_UUID128(HealthDeviceProfileID_UUID16);

        // '
        // ' The SIG renamed the uuid for VideoConferencingServiceClass
        // '
        public readonly static Guid VideoConferencingServiceClass_UUID = AVRemoteControlControllerServiceClass_UUID;
        public readonly static ushort VideoConferencingServiceClassID_UUID16 = AVRemoteControlControllerServiceClass_UUID16;

        // '
        // ' Fixing typos introduced in previous releases
        // '
        public readonly static Guid HN_PROTOCOL_UUID = HCN_PROTOCOL_UUID;
        public readonly static Guid BasicPringingServiceClass_UUID = BasicPrintingProfile_UUID;

        // '
        // ' Fixing naming inconsistencies in UUID16 list
        // '
        public readonly static ushort CommonISDNAccessServiceClass_UUID16 = CommonISDNAccessServiceClassID_UUID16;
        public readonly static ushort VideoConferencingGWServiceClass_UUID16 = VideoConferencingGWServiceClassID_UUID16;
        public readonly static ushort UDIMTServiceClass_UUID16 = UDIMTServiceClassID_UUID16;
        public readonly static ushort UDITAServiceClass_UUID16 = UDITAServiceClassID_UUID16;
        public readonly static ushort AudioVideoServiceClass_UUID16 = AudioVideoServiceClassID_UUID16;

        // '
        // ' Fixing naming inconsistencies in profile list
        // '
        public readonly static ushort CordlessServiceClassID_UUID16 = CordlessTelephonyServiceClassID_UUID16;
        public readonly static ushort AudioSinkSourceServiceClassID_UUID16 = AudioSinkServiceClassID_UUID16;
        public readonly static ushort AdvancedAudioDistributionServiceClassID_UUID16 = AdvancedAudioDistributionProfileID_UUID16;
        public readonly static ushort ImagingServiceClassID_UUID16 = ImagingServiceProfileID_UUID16;
        public readonly static ushort BasicPrintingServiceClassID_UUID16 = BasicPrintingProfileID_UUID16;
        public readonly static ushort HardcopyCableReplacementServiceClassID_UUID16 = HardcopyCableReplacementProfileID_UUID16;
        public readonly static Guid AdvancedAudioDistributionServiceClass_UUID = AdvancedAudioDistributionProfile_UUID;
        public readonly static Guid ImagingServiceClass_UUID = ImagingServiceProfile_UUID;
        public readonly static Guid BasicPrintingServiceClass_UUID = BasicPrintingProfile_UUID;
        public readonly static Guid HardcopyCableReplacementServiceClass_UUID = HardcopyCableReplacementProfile_UUID;
        public readonly static Guid VideoDistributionServiceClass_UUID = VideoDistributionProfile_UUID;


        // '
        // ' max length of device friendly name.
        // '
        public const ushort BTH_MAX_NAME_SIZE = 248;
        public const ushort BTH_MAX_PIN_SIZE = 16;
        public const ushort BTH_LINK_KEY_LENGTH = 16;


        // ' Manufacturers
        public readonly static BTH_MFG_INFO BTH_MFG_ERICSSON = new BTH_MFG_INFO("Ericsson", 0);
        public readonly static BTH_MFG_INFO BTH_MFG_NOKIA = new BTH_MFG_INFO("Nokia", 1);
        public readonly static BTH_MFG_INFO BTH_MFG_INTEL = new BTH_MFG_INFO("Intel", 2);
        public readonly static BTH_MFG_INFO BTH_MFG_IBM = new BTH_MFG_INFO("IBM", 3);
        public readonly static BTH_MFG_INFO BTH_MFG_TOSHIBA = new BTH_MFG_INFO("Toshiba", 4);
        public readonly static BTH_MFG_INFO BTH_MFG_3COM = new BTH_MFG_INFO("3COM", 5);
        public readonly static BTH_MFG_INFO BTH_MFG_MICROSOFT = new BTH_MFG_INFO("Microsoft", 6);
        public readonly static BTH_MFG_INFO BTH_MFG_LUCENT = new BTH_MFG_INFO("Lucent", 7);
        public readonly static BTH_MFG_INFO BTH_MFG_MOTOROLA = new BTH_MFG_INFO("Motorola", 8);
        public readonly static BTH_MFG_INFO BTH_MFG_INFINEON = new BTH_MFG_INFO("Infineon", 9);
        public readonly static BTH_MFG_INFO BTH_MFG_CSR = new BTH_MFG_INFO("CSR", 10);
        public readonly static BTH_MFG_INFO BTH_MFG_SILICONWAVE = new BTH_MFG_INFO("SiliconWave", 11);
        public readonly static BTH_MFG_INFO BTH_MFG_DIGIANSWER = new BTH_MFG_INFO("DigiAnswer", 12);
        public readonly static BTH_MFG_INFO BTH_MFG_TI = new BTH_MFG_INFO("TI", 13);
        public readonly static BTH_MFG_INFO BTH_MFG_PARTHUS = new BTH_MFG_INFO("Parthus", 14);
        public readonly static BTH_MFG_INFO BTH_MFG_BROADCOM = new BTH_MFG_INFO("Broadcom", 15);
        public readonly static BTH_MFG_INFO BTH_MFG_MITEL = new BTH_MFG_INFO("MITEL", 16);
        public readonly static BTH_MFG_INFO BTH_MFG_WIDCOMM = new BTH_MFG_INFO("Widcomm", 17);
        public readonly static BTH_MFG_INFO BTH_MFG_ZEEVO = new BTH_MFG_INFO("Zeevo", 18);
        public readonly static BTH_MFG_INFO BTH_MFG_ATMEL = new BTH_MFG_INFO("ATMEL", 19);
        public readonly static BTH_MFG_INFO BTH_MFG_MITSIBUSHI = new BTH_MFG_INFO("Mitsibushi", 20);
        public readonly static BTH_MFG_INFO BTH_MFG_RTX_TELECOM = new BTH_MFG_INFO("RTX Telecom", 21);
        public readonly static BTH_MFG_INFO BTH_MFG_KC_TECHNOLOGY = new BTH_MFG_INFO("KC Technology", 22);
        public readonly static BTH_MFG_INFO BTH_MFG_NEWLOGIC = new BTH_MFG_INFO("NewLogic", 23);
        public readonly static BTH_MFG_INFO BTH_MFG_TRANSILICA = new BTH_MFG_INFO("Transilica", 24);
        public readonly static BTH_MFG_INFO BTH_MFG_ROHDE_SCHWARZ = new BTH_MFG_INFO("Rohde Schwarz", 25);
        public readonly static BTH_MFG_INFO BTH_MFG_TTPCOM = new BTH_MFG_INFO("TTPCOM", 26);
        public readonly static BTH_MFG_INFO BTH_MFG_SIGNIA = new BTH_MFG_INFO("Signia", 27);
        public readonly static BTH_MFG_INFO BTH_MFG_CONEXANT = new BTH_MFG_INFO("Conexant", 28);
        public readonly static BTH_MFG_INFO BTH_MFG_QUALCOMM = new BTH_MFG_INFO("Qualcomm", 29);
        public readonly static BTH_MFG_INFO BTH_MFG_INVENTEL = new BTH_MFG_INFO("INVENTEL", 30);
        public readonly static BTH_MFG_INFO BTH_MFG_AVM_BERLIN = new BTH_MFG_INFO("AVM Berlin", 31);
        public readonly static BTH_MFG_INFO BTH_MFG_BANDSPEED = new BTH_MFG_INFO("Bandspeed", 32);
        public readonly static BTH_MFG_INFO BTH_MFG_MANSELLA = new BTH_MFG_INFO("Mansella", 33);
        public readonly static BTH_MFG_INFO BTH_MFG_NEC = new BTH_MFG_INFO("NEC", 34);
        public readonly static BTH_MFG_INFO BTH_MFG_WAVEPLUS_TECHNOLOGY_CO = new BTH_MFG_INFO("WavePlus Technology Co", 35);
        public readonly static BTH_MFG_INFO BTH_MFG_ALCATEL = new BTH_MFG_INFO("ALCATEL", 36);
        public readonly static BTH_MFG_INFO BTH_MFG_PHILIPS_SEMICONDUCTOR = new BTH_MFG_INFO("Philips Semiconductor", 37);
        public readonly static BTH_MFG_INFO BTH_MFG_C_TECHNOLOGIES = new BTH_MFG_INFO("C Technologies", 38);
        public readonly static BTH_MFG_INFO BTH_MFG_OPEN_INTERFACE = new BTH_MFG_INFO("Open Interface", 39);
        public readonly static BTH_MFG_INFO BTH_MFG_RF_MICRO_DEVICES = new BTH_MFG_INFO("RF Micro Devices", 40);
        public readonly static BTH_MFG_INFO BTH_MFG_HITACHI = new BTH_MFG_INFO("Hitachi", 41);
        public readonly static BTH_MFG_INFO BTH_MFG_SYMBOL_TECHNOLOGIES = new BTH_MFG_INFO("Symbol Technologies", 42);
        public readonly static BTH_MFG_INFO BTH_MFG_TENOVIS = new BTH_MFG_INFO("Tenovis", 43);
        public readonly static BTH_MFG_INFO BTH_MFG_MACRONIX_INTERNATIONAL = new BTH_MFG_INFO("Macronix International", 44);
        public readonly static BTH_MFG_INFO BTH_MFG_MARVELL = new BTH_MFG_INFO("Marvell", 72);
        public readonly static BTH_MFG_INFO BTH_MFG_APPLE = new BTH_MFG_INFO("Apple", 76);
        public readonly static BTH_MFG_INFO BTH_MFG_NORDIC_SEMICONDUCTORS_ASA = new BTH_MFG_INFO("Nordic Semiconductors ASA", 89);
        public readonly static BTH_MFG_INFO BTH_MFG_ARUBA_NETWORKS = new BTH_MFG_INFO("Aruba Networks", 283);
        public readonly static BTH_MFG_INFO BTH_MFG_INTERNAL_USE = new BTH_MFG_INFO("INTERNAL_USE", 65535);


        // ' COD


        public const ulong BTH_ADDR_NULL = 0UL;
        public const ulong NAP_MASK = 0xFFFF00000000UL;
        public const ulong SAP_MASK = 0xFFFFFFFFUL;
        public const int NAP_BIT_OFFSET = 8 * 4;
        public const uint SAP_BIT_OFFSET = 0U;

        public static ulong GET_NAP(ulong _bth_addr)
        {
            return (_bth_addr & NAP_MASK) >> NAP_BIT_OFFSET;
        }

        public static ulong GET_SAP(ulong _bth_addr)
        {
            return (_bth_addr & NAP_MASK) >> (int)SAP_BIT_OFFSET;
        }

        public static ulong SET_NAP(ulong _nap)
        {
            return _nap << NAP_BIT_OFFSET;
        }

        public static ulong SET_SAP(ulong _sap)
        {
            return _sap << (int)SAP_BIT_OFFSET;
        }

        public static ulong SET_NAP_SAP(ulong _nap, ulong _sap)
        {
            return SET_NAP(_nap) | SET_SAP(_sap);
        }

        public const byte COD_FORMAT_BIT_OFFSET = 0;
        public const byte COD_MINOR_BIT_OFFSET = 2;
        public const byte COD_MAJOR_BIT_OFFSET = 8 * 1;
        public const byte COD_SERVICE_BIT_OFFSET = 8 * 1 + 5;
        public const uint COD_FORMAT_MASK = 0x3;
        public const uint COD_MINOR_MASK = 0xFC;
        public const uint COD_MAJOR_MASK = 0x1F00;
        public const uint COD_SERVICE_MASK = 0xFFE000;
        public const uint COD_VERSION = 0x0;
        public const uint COD_SERVICE_LIMITED = 0x1;
        public const uint COD_SERVICE_POSITIONING = 0x8;
        public const uint COD_SERVICE_NETWORKING = 0x10;
        public const uint COD_SERVICE_RENDERING = 0x20;
        public const uint COD_SERVICE_CAPTURING = 0x40;
        public const uint COD_SERVICE_OBJECT_XFER = 0x80;
        public const uint COD_SERVICE_AUDIO = 0x100;
        public const uint COD_SERVICE_TELEPHONY = 0x200;
        public const uint COD_SERVICE_INFORMATION = 0x400;

        public static uint COD_SERVICE_VALID_MASK()
        {
            return COD_SERVICE_LIMITED | COD_SERVICE_POSITIONING | COD_SERVICE_NETWORKING | COD_SERVICE_RENDERING | COD_SERVICE_CAPTURING | COD_SERVICE_OBJECT_XFER | COD_SERVICE_AUDIO | COD_SERVICE_TELEPHONY | COD_SERVICE_INFORMATION;
        }

        public readonly static int COD_SERVICE_MAX_COUNT = 9;

        // '
        // ' Major class codes
        // '
        public const uint COD_MAJOR_MISCELLANEOUS = 0x0;
        public const uint COD_MAJOR_COMPUTER = 0x1;
        public const uint COD_MAJOR_PHONE = 0x2;
        public const uint COD_MAJOR_LAN_ACCESS = 0x3;
        public const uint COD_MAJOR_AUDIO = 0x4;
        public const uint COD_MAJOR_PERIPHERAL = 0x5;
        public const uint COD_MAJOR_IMAGING = 0x6;
        public const uint COD_MAJOR_WEARABLE = 0x7;
        public const uint COD_MAJOR_TOY = 0x8;
        public const uint COD_MAJOR_HEALTH = 0x9;
        public const uint COD_MAJOR_UNCLASSIFIED = 0x1F;

        // '
        // ' Minor class codes specific to each major class
        // '
        public const uint COD_COMPUTER_MINOR_UNCLASSIFIED = 0x0;
        public const uint COD_COMPUTER_MINOR_DESKTOP = 0x1;
        public const uint COD_COMPUTER_MINOR_SERVER = 0x2;
        public const uint COD_COMPUTER_MINOR_LAPTOP = 0x3;
        public const uint COD_COMPUTER_MINOR_HANDHELD = 0x4;
        public const uint COD_COMPUTER_MINOR_PALM = 0x5;
        public const uint COD_COMPUTER_MINOR_WEARABLE = 0x6;
        public const uint COD_PHONE_MINOR_UNCLASSIFIED = 0x0;
        public const uint COD_PHONE_MINOR_CELLULAR = 0x1;
        public const uint COD_PHONE_MINOR_CORDLESS = 0x2;
        public const uint COD_PHONE_MINOR_SMART = 0x3;
        public const uint COD_PHONE_MINOR_WIRED_MODEM = 0x4;
        public const uint COD_AUDIO_MINOR_UNCLASSIFIED = 0x0;
        public const uint COD_AUDIO_MINOR_HEADSET = 0x1;
        public const uint COD_AUDIO_MINOR_HANDS_FREE = 0x2;
        public const uint COD_AUDIO_MINOR_HEADSET_HANDS_FREE = 0x3;
        public const uint COD_AUDIO_MINOR_MICROPHONE = 0x4;
        public const uint COD_AUDIO_MINOR_LOUDSPEAKER = 0x5;
        public const uint COD_AUDIO_MINOR_HEADPHONES = 0x6;
        public const uint COD_AUDIO_MINOR_PORTABLE_AUDIO = 0x7;
        public const uint COD_AUDIO_MINOR_CAR_AUDIO = 0x8;
        public const uint COD_AUDIO_MINOR_SET_TOP_BOX = 0x9;
        public const uint COD_AUDIO_MINOR_HIFI_AUDIO = 0xA;
        public const uint COD_AUDIO_MINOR_VCR = 0xB;
        public const uint COD_AUDIO_MINOR_VIDEO_CAMERA = 0xC;
        public const uint COD_AUDIO_MINOR_CAMCORDER = 0xD;
        public const uint COD_AUDIO_MINOR_VIDEO_MONITOR = 0xE;
        public const uint COD_AUDIO_MINOR_VIDEO_DISPLAY_LOUDSPEAKER = 0xF;
        public const uint COD_AUDIO_MINOR_VIDEO_DISPLAY_CONFERENCING = 0x10;
        // ' Public Const COD_AUDIO_MINOR_RESERVED As UInteger = (&H11)
        public const uint COD_AUDIO_MINOR_GAMING_TOY = 0x12;
        public const uint COD_PERIPHERAL_MINOR_KEYBOARD_MASK = 0x10;
        public const uint COD_PERIPHERAL_MINOR_POINTER_MASK = 0x20;
        public const uint COD_PERIPHERAL_MINOR_NO_CATEGORY = 0x0;
        public const uint COD_PERIPHERAL_MINOR_JOYSTICK = 0x1;
        public const uint COD_PERIPHERAL_MINOR_GAMEPAD = 0x2;
        public const uint COD_PERIPHERAL_MINOR_REMOTE_CONTROL = 0x3;
        public const uint COD_PERIPHERAL_MINOR_SENSING = 0x4;
        public const uint COD_IMAGING_MINOR_DISPLAY_MASK = 0x4;
        public const uint COD_IMAGING_MINOR_CAMERA_MASK = 0x8;
        public const uint COD_IMAGING_MINOR_SCANNER_MASK = 0x10;
        public const uint COD_IMAGING_MINOR_PRINTER_MASK = 0x20;
        public const uint COD_WEARABLE_MINOR_WRIST_WATCH = 0x1;
        public const uint COD_WEARABLE_MINOR_PAGER = 0x2;
        public const uint COD_WEARABLE_MINOR_JACKET = 0x3;
        public const uint COD_WEARABLE_MINOR_HELMET = 0x4;
        public const uint COD_WEARABLE_MINOR_GLASSES = 0x5;
        public const uint COD_TOY_MINOR_ROBOT = 0x1;
        public const uint COD_TOY_MINOR_VEHICLE = 0x2;
        public const uint COD_TOY_MINOR_DOLL_ACTION_FIGURE = 0x3;
        public const uint COD_TOY_MINOR_CONTROLLER = 0x4;
        public const uint COD_TOY_MINOR_GAME = 0x5;
        public const uint COD_HEALTH_MINOR_BLOOD_PRESSURE_MONITOR = 0x1;
        public const uint COD_HEALTH_MINOR_THERMOMETER = 0x2;
        public const uint COD_HEALTH_MINOR_WEIGHING_SCALE = 0x3;
        public const uint COD_HEALTH_MINOR_GLUCOSE_METER = 0x4;
        public const uint COD_HEALTH_MINOR_PULSE_OXIMETER = 0x5;
        public const uint COD_HEALTH_MINOR_HEART_PULSE_MONITOR = 0x6;
        public const uint COD_HEALTH_MINOR_HEALTH_DATA_DISPLAY = 0x7;
        public const uint COD_HEALTH_MINOR_STEP_COUNTER = 0x8;

        // '
        // ' Cannot use GET_COD_MINOR for this b/c it is embedded in a different manner
        // ' than the rest of the major classes
        // '

        public const byte COD_LAN_ACCESS_BIT_OFFSET = 5;
        public const uint COD_LAN_MINOR_MASK = 0x1C;
        public const uint COD_LAN_ACCESS_MASK = 0xE0;

        public static uint GET_COD_LAN_MINOR(uint _cod)
        {
            return (_cod & COD_LAN_MINOR_MASK) >> COD_MINOR_BIT_OFFSET;
        }

        public static uint GET_COD_LAN_ACCESS(uint _cod)
        {
            return (_cod & COD_LAN_ACCESS_MASK) >> COD_LAN_ACCESS_BIT_OFFSET;
        }

        // '
        // ' LAN access percent usage subcodes
        // '
        public const uint COD_LAN_MINOR_UNCLASSIFIED = 0x0;
        public const uint COD_LAN_ACCESS_0_USED = 0x0;
        public const uint COD_LAN_ACCESS_17_USED = 0x1;
        public const uint COD_LAN_ACCESS_33_USED = 0x2;
        public const uint COD_LAN_ACCESS_50_USED = 0x3;
        public const uint COD_LAN_ACCESS_67_USED = 0x4;
        public const uint COD_LAN_ACCESS_83_USED = 0x5;
        public const uint COD_LAN_ACCESS_99_USED = 0x6;
        public const uint COD_LAN_ACCESS_FULL = 0x7;

        public static void ParseClass(uint classId, ref ushort service, ref ushort major, ref ushort minor)
        {
            minor = (ushort)((classId & COD_MINOR_MASK) >> COD_MINOR_BIT_OFFSET);
            major = (ushort)((classId & COD_MAJOR_MASK) >> COD_MAJOR_BIT_OFFSET);
            service = (ushort)((classId & COD_SERVICE_MASK) >> COD_SERVICE_BIT_OFFSET);
        }

        public static string PrintMinorClass(ushort major, ushort minor)
        {
            var sb = new List<string>();
            if (major == COD_MAJOR_COMPUTER & minor == COD_COMPUTER_MINOR_UNCLASSIFIED)
                sb.Add("UNCLASSIFIED");
            if (major == COD_MAJOR_COMPUTER & minor == COD_COMPUTER_MINOR_DESKTOP)
                sb.Add("DESKTOP");
            if (major == COD_MAJOR_COMPUTER & minor == COD_COMPUTER_MINOR_SERVER)
                sb.Add("SERVER");
            if (major == COD_MAJOR_COMPUTER & minor == COD_COMPUTER_MINOR_LAPTOP)
                sb.Add("LAPTOP");
            if (major == COD_MAJOR_COMPUTER & minor == COD_COMPUTER_MINOR_HANDHELD)
                sb.Add("HANDHELD");
            if (major == COD_MAJOR_COMPUTER & minor == COD_COMPUTER_MINOR_PALM)
                sb.Add("PALM");
            if (major == COD_MAJOR_COMPUTER & minor == COD_COMPUTER_MINOR_WEARABLE)
                sb.Add("WEARABLE");
            if (major == COD_MAJOR_PHONE & minor == COD_PHONE_MINOR_UNCLASSIFIED)
                sb.Add("UNCLASSIFIED");
            if (major == COD_MAJOR_PHONE & minor == COD_PHONE_MINOR_CELLULAR)
                sb.Add("CELLULAR");
            if (major == COD_MAJOR_PHONE & minor == COD_PHONE_MINOR_CORDLESS)
                sb.Add("CORDLESS");
            if (major == COD_MAJOR_PHONE & minor == COD_PHONE_MINOR_SMART)
                sb.Add("SMART");
            if (major == COD_MAJOR_PHONE & minor == COD_PHONE_MINOR_WIRED_MODEM)
                sb.Add("WIRED_MODEM");
            if (major == COD_MAJOR_AUDIO & minor == COD_AUDIO_MINOR_UNCLASSIFIED)
                sb.Add("UNCLASSIFIED");
            if (major == COD_MAJOR_AUDIO & minor == COD_AUDIO_MINOR_HEADSET)
                sb.Add("HEADSET");
            if (major == COD_MAJOR_AUDIO & minor == COD_AUDIO_MINOR_HANDS_FREE)
                sb.Add("HANDS_FREE");
            if (major == COD_MAJOR_AUDIO & minor == COD_AUDIO_MINOR_HEADSET_HANDS_FREE)
                sb.Add("HEADSET_HANDS_FREE");
            if (major == COD_MAJOR_AUDIO & minor == COD_AUDIO_MINOR_MICROPHONE)
                sb.Add("MICROPHONE");
            if (major == COD_MAJOR_AUDIO & minor == COD_AUDIO_MINOR_LOUDSPEAKER)
                sb.Add("LOUDSPEAKER");
            if (major == COD_MAJOR_AUDIO & minor == COD_AUDIO_MINOR_HEADPHONES)
                sb.Add("HEADPHONES");
            if (major == COD_MAJOR_AUDIO & minor == COD_AUDIO_MINOR_PORTABLE_AUDIO)
                sb.Add("PORTABLE_AUDIO");
            if (major == COD_MAJOR_AUDIO & minor == COD_AUDIO_MINOR_CAR_AUDIO)
                sb.Add("CAR_AUDIO");
            if (major == COD_MAJOR_AUDIO & minor == COD_AUDIO_MINOR_SET_TOP_BOX)
                sb.Add("SET_TOP_BOX");
            if (major == COD_MAJOR_AUDIO & minor == COD_AUDIO_MINOR_HIFI_AUDIO)
                sb.Add("HIFI_AUDIO");
            if (major == COD_MAJOR_AUDIO & minor == COD_AUDIO_MINOR_VCR)
                sb.Add("VCR");
            if (major == COD_MAJOR_AUDIO & minor == COD_AUDIO_MINOR_VIDEO_CAMERA)
                sb.Add("VIDEO_CAMERA");
            if (major == COD_MAJOR_AUDIO & minor == COD_AUDIO_MINOR_CAMCORDER)
                sb.Add("CAMCORDER");
            if (major == COD_MAJOR_AUDIO & minor == COD_AUDIO_MINOR_VIDEO_MONITOR)
                sb.Add("VIDEO_MONITOR");
            if (major == COD_MAJOR_AUDIO & minor == COD_AUDIO_MINOR_VIDEO_DISPLAY_LOUDSPEAKER)
                sb.Add("VIDEO_DISPLAY_LOUDSPEAKER");
            if (major == COD_MAJOR_AUDIO & minor == COD_AUDIO_MINOR_VIDEO_DISPLAY_CONFERENCING)
                sb.Add("VIDEO_DISPLAY_CONFERENCING");
            if (major == COD_MAJOR_AUDIO & minor == COD_AUDIO_MINOR_GAMING_TOY)
                sb.Add("GAMING_TOY");
            if (major == COD_MAJOR_PERIPHERAL & minor == COD_PERIPHERAL_MINOR_KEYBOARD_MASK)
                sb.Add("KEYBOARD_MASK");
            if (major == COD_MAJOR_PERIPHERAL & minor == COD_PERIPHERAL_MINOR_POINTER_MASK)
                sb.Add("POINTER_MASK");
            if (major == COD_MAJOR_PERIPHERAL & minor == COD_PERIPHERAL_MINOR_NO_CATEGORY)
                sb.Add("NO_CATEGORY");
            if (major == COD_MAJOR_PERIPHERAL & minor == COD_PERIPHERAL_MINOR_JOYSTICK)
                sb.Add("JOYSTICK");
            if (major == COD_MAJOR_PERIPHERAL & minor == COD_PERIPHERAL_MINOR_GAMEPAD)
                sb.Add("GAMEPAD");
            if (major == COD_MAJOR_PERIPHERAL & minor == COD_PERIPHERAL_MINOR_REMOTE_CONTROL)
                sb.Add("REMOTE_CONTROL");
            if (major == COD_MAJOR_PERIPHERAL & minor == COD_PERIPHERAL_MINOR_SENSING)
                sb.Add("SENSING");
            if (major == COD_MAJOR_IMAGING & minor == COD_IMAGING_MINOR_DISPLAY_MASK)
                sb.Add("DISPLAY_MASK");
            if (major == COD_MAJOR_IMAGING & minor == COD_IMAGING_MINOR_CAMERA_MASK)
                sb.Add("CAMERA_MASK");
            if (major == COD_MAJOR_IMAGING & minor == COD_IMAGING_MINOR_SCANNER_MASK)
                sb.Add("SCANNER_MASK");
            if (major == COD_MAJOR_IMAGING & minor == COD_IMAGING_MINOR_PRINTER_MASK)
                sb.Add("PRINTER_MASK");
            if (major == COD_MAJOR_WEARABLE & minor == COD_WEARABLE_MINOR_WRIST_WATCH)
                sb.Add("WRIST_WATCH");
            if (major == COD_MAJOR_WEARABLE & minor == COD_WEARABLE_MINOR_PAGER)
                sb.Add("PAGER");
            if (major == COD_MAJOR_WEARABLE & minor == COD_WEARABLE_MINOR_JACKET)
                sb.Add("JACKET");
            if (major == COD_MAJOR_WEARABLE & minor == COD_WEARABLE_MINOR_HELMET)
                sb.Add("HELMET");
            if (major == COD_MAJOR_WEARABLE & minor == COD_WEARABLE_MINOR_GLASSES)
                sb.Add("GLASSES");
            if (major == COD_MAJOR_TOY & minor == COD_TOY_MINOR_ROBOT)
                sb.Add("ROBOT");
            if (major == COD_MAJOR_TOY & minor == COD_TOY_MINOR_VEHICLE)
                sb.Add("VEHICLE");
            if (major == COD_MAJOR_TOY & minor == COD_TOY_MINOR_DOLL_ACTION_FIGURE)
                sb.Add("DOLL_ACTION_FIGURE");
            if (major == COD_MAJOR_TOY & minor == COD_TOY_MINOR_CONTROLLER)
                sb.Add("CONTROLLER");
            if (major == COD_MAJOR_TOY & minor == COD_TOY_MINOR_GAME)
                sb.Add("GAME");
            if (major == COD_MAJOR_HEALTH & minor == COD_HEALTH_MINOR_BLOOD_PRESSURE_MONITOR)
                sb.Add("BLOOD_PRESSURE_MONITOR");
            if (major == COD_MAJOR_HEALTH & minor == COD_HEALTH_MINOR_THERMOMETER)
                sb.Add("THERMOMETER");
            if (major == COD_MAJOR_HEALTH & minor == COD_HEALTH_MINOR_WEIGHING_SCALE)
                sb.Add("WEIGHING_SCALE");
            if (major == COD_MAJOR_HEALTH & minor == COD_HEALTH_MINOR_GLUCOSE_METER)
                sb.Add("GLUCOSE_METER");
            if (major == COD_MAJOR_HEALTH & minor == COD_HEALTH_MINOR_PULSE_OXIMETER)
                sb.Add("PULSE_OXIMETER");
            if (major == COD_MAJOR_HEALTH & minor == COD_HEALTH_MINOR_HEART_PULSE_MONITOR)
                sb.Add("HEART_PULSE_MONITOR");
            if (major == COD_MAJOR_HEALTH & minor == COD_HEALTH_MINOR_HEALTH_DATA_DISPLAY)
                sb.Add("HEALTH_DATA_DISPLAY");
            if (major == COD_MAJOR_HEALTH & minor == COD_HEALTH_MINOR_STEP_COUNTER)
                sb.Add("STEP_COUNTER");
            for (int i = 0, loopTo = sb.Count - 1; i <= loopTo; i++)
                sb[i] = TextTools.TitleCase(sb[i]);
            return string.Join(", ", sb);
        }

        public static string PrintMajorClass(ushort major)
        {
            var sb = new List<string>();

            // '
            // ' Major class codes
            // '
            if (major == COD_MAJOR_MISCELLANEOUS)
                sb.Add("MISCELLANEOUS");
            if (major == COD_MAJOR_COMPUTER)
                sb.Add("COMPUTER");
            if (major == COD_MAJOR_PHONE)
                sb.Add("PHONE");
            if (major == COD_MAJOR_LAN_ACCESS)
                sb.Add("LAN_ACCESS");
            if (major == COD_MAJOR_AUDIO)
                sb.Add("AUDIO");
            if (major == COD_MAJOR_PERIPHERAL)
                sb.Add("PERIPHERAL");
            if (major == COD_MAJOR_IMAGING)
                sb.Add("IMAGING");
            if (major == COD_MAJOR_WEARABLE)
                sb.Add("WEARABLE");
            if (major == COD_MAJOR_TOY)
                sb.Add("TOY");
            if (major == COD_MAJOR_HEALTH)
                sb.Add("HEALTH");
            if (major == COD_MAJOR_UNCLASSIFIED)
                sb.Add("UNCLASSIFIED");
            for (int i = 0, loopTo = sb.Count - 1; i <= loopTo; i++)
                sb[i] = TextTools.TitleCase(sb[i]);
            return string.Join(", ", sb);
        }

        public static string PrintServiceClass(ushort service)
        {
            var sb = new List<string>();
            if ((service & COD_SERVICE_LIMITED) == COD_SERVICE_LIMITED)
                sb.Add("LIMITED");
            if ((service & COD_SERVICE_POSITIONING) == COD_SERVICE_POSITIONING)
                sb.Add("POSITIONING");
            if ((service & COD_SERVICE_NETWORKING) == COD_SERVICE_NETWORKING)
                sb.Add("NETWORKING");
            if ((service & COD_SERVICE_RENDERING) == COD_SERVICE_RENDERING)
                sb.Add("RENDERING");
            if ((service & COD_SERVICE_CAPTURING) == COD_SERVICE_CAPTURING)
                sb.Add("CAPTURING");
            if ((service & COD_SERVICE_OBJECT_XFER) == COD_SERVICE_OBJECT_XFER)
                sb.Add("OBJECT_XFER");
            if ((service & COD_SERVICE_AUDIO) == COD_SERVICE_AUDIO)
                sb.Add("AUDIO");
            if ((service & COD_SERVICE_TELEPHONY) == COD_SERVICE_TELEPHONY)
                sb.Add("TELEPHONY");
            if ((service & COD_SERVICE_INFORMATION) == COD_SERVICE_INFORMATION)
                sb.Add("INFORMATION");
            for (int i = 0, loopTo = sb.Count - 1; i <= loopTo; i++)
                sb[i] = TextTools.TitleCase(sb[i]);
            return string.Join(", ", sb);
        }



        // '
        // ' Extended Inquiry Response (EIR) defines.
        // '
        public const byte BTH_EIR_FLAGS_ID = 0x1;
        public const byte BTH_EIR_16_UUIDS_PARTIAL_ID = 0x2;
        public const byte BTH_EIR_16_UUIDS_COMPLETE_ID = 0x3;
        public const byte BTH_EIR_32_UUIDS_PARTIAL_ID = 0x4;
        public const byte BTH_EIR_32_UUIDS_COMPLETE_ID = 0x5;
        public const byte BTH_EIR_128_UUIDS_PARTIAL_ID = 0x6;
        public const byte BTH_EIR_128_UUIDS_COMPLETE_ID = 0x7;
        public const byte BTH_EIR_LOCAL_NAME_PARTIAL_ID = 0x8;
        public const byte BTH_EIR_LOCAL_NAME_COMPLETE_ID = 0x9;
        public const byte BTH_EIR_TX_POWER_LEVEL_ID = 0xA;
        public const byte BTH_EIR_OOB_OPT_DATA_LEN_ID = 0xB; // ' OOB only.
        public const byte BTH_EIR_OOB_BD_ADDR_ID = 0xC; // ' OOB only.
        public const byte BTH_EIR_OOB_COD_ID = 0xD; // ' OOB only.
        public const byte BTH_EIR_OOB_SP_HASH_ID = 0xE; // ' OOB only.
        public const byte BTH_EIR_OOB_SP_RANDOMIZER_ID = 0xF; // ' OOB only.
        public const byte BTH_EIR_MANUFACTURER_ID = 0xFF;

        // '
        // ' Extended Inquiry Response (EIR) size.
        // '
        public const byte BTH_EIR_SIZE = 240;

        // '
        // ' Used as an initializer of LAP_DATA
        // '
        public readonly static byte[] LAP_GIAC_INIT = new byte[] { 0x33, 0x8B, 0x9E };
        public readonly static byte[] LAP_LIAC_INIT = new byte[] { 0x0, 0x8B, 0x9E };

        // '
        // ' General Inquiry Access Code.
        // '
        public const uint LAP_GIAC_VALUE = 0x9E8B33;

        // '
        // ' Limited Inquiry Access Code.
        // '
        public const uint LAP_LIAC_VALUE = 0x9E8B00;
        public const uint BTH_ADDR_IAC_FIRST = 0x9E8B00;
        public const uint BTH_ADDR_IAC_LAST = 0x9E8B3F;
        public const uint BTH_ADDR_LIAC = 0x9E8B00;
        public const uint BTH_ADDR_GIAC = 0x9E8B33;

        public static bool BTH_ERROR(byte _btStatus)
        {
            return _btStatus != BTH_ERROR_SUCCESS;
        }

        public static bool BTH_SUCCESS(byte _btStatus)
        {
            return _btStatus == BTH_ERROR_SUCCESS;
        }

        public const byte BTH_ERROR_SUCCESS = 0x0;
        public const byte BTH_ERROR_UNKNOWN_HCI_COMMAND = 0x1;
        public const byte BTH_ERROR_NO_CONNECTION = 0x2;
        public const byte BTH_ERROR_HARDWARE_FAILURE = 0x3;
        public const byte BTH_ERROR_PAGE_TIMEOUT = 0x4;
        public const byte BTH_ERROR_AUTHENTICATION_FAILURE = 0x5;
        public const byte BTH_ERROR_KEY_MISSING = 0x6;
        public const byte BTH_ERROR_MEMORY_FULL = 0x7;
        public const byte BTH_ERROR_CONNECTION_TIMEOUT = 0x8;
        public const byte BTH_ERROR_MAX_NUMBER_OF_CONNECTIONS = 0x9;
        public const byte BTH_ERROR_MAX_NUMBER_OF_SCO_CONNECTIONS = 0xA;
        public const byte BTH_ERROR_ACL_CONNECTION_ALREADY_EXISTS = 0xB;
        public const byte BTH_ERROR_COMMAND_DISALLOWED = 0xC;
        public const byte BTH_ERROR_HOST_REJECTED_LIMITED_RESOURCES = 0xD;
        public const byte BTH_ERROR_HOST_REJECTED_SECURITY_REASONS = 0xE;
        public const byte BTH_ERROR_HOST_REJECTED_PERSONAL_DEVICE = 0xF;
        public const byte BTH_ERROR_HOST_TIMEOUT = 0x10;
        public const byte BTH_ERROR_UNSUPPORTED_FEATURE_OR_PARAMETER = 0x11;
        public const byte BTH_ERROR_INVALID_HCI_PARAMETER = 0x12;
        public const byte BTH_ERROR_REMOTE_USER_ENDED_CONNECTION = 0x13;
        public const byte BTH_ERROR_REMOTE_LOW_RESOURCES = 0x14;
        public const byte BTH_ERROR_REMOTE_POWERING_OFF = 0x15;
        public const byte BTH_ERROR_LOCAL_HOST_TERMINATED_CONNECTION = 0x16;
        public const byte BTH_ERROR_REPEATED_ATTEMPTS = 0x17;
        public const byte BTH_ERROR_PAIRING_NOT_ALLOWED = 0x18;
        public const byte BTH_ERROR_UKNOWN_LMP_PDU = 0x19;
        public const byte BTH_ERROR_UNSUPPORTED_REMOTE_FEATURE = 0x1A;
        public const byte BTH_ERROR_SCO_OFFSET_REJECTED = 0x1B;
        public const byte BTH_ERROR_SCO_INTERVAL_REJECTED = 0x1C;
        public const byte BTH_ERROR_SCO_AIRMODE_REJECTED = 0x1D;
        public const byte BTH_ERROR_INVALID_LMP_PARAMETERS = 0x1E;
        public const byte BTH_ERROR_UNSPECIFIED_ERROR = 0x1F;
        public const byte BTH_ERROR_UNSUPPORTED_LMP_PARM_VALUE = 0x20;
        public const byte BTH_ERROR_ROLE_CHANGE_NOT_ALLOWED = 0x21;
        public const byte BTH_ERROR_LMP_RESPONSE_TIMEOUT = 0x22;
        public const byte BTH_ERROR_LMP_TRANSACTION_COLLISION = 0x23;
        public const byte BTH_ERROR_LMP_PDU_NOT_ALLOWED = 0x24;
        public const byte BTH_ERROR_ENCRYPTION_MODE_NOT_ACCEPTABLE = 0x25;
        public const byte BTH_ERROR_UNIT_KEY_NOT_USED = 0x26;
        public const byte BTH_ERROR_QOS_IS_NOT_SUPPORTED = 0x27;
        public const byte BTH_ERROR_INSTANT_PASSED = 0x28;
        public const byte BTH_ERROR_PAIRING_WITH_UNIT_KEY_NOT_SUPPORTED = 0x29;
        public const byte BTH_ERROR_DIFFERENT_TRANSACTION_COLLISION = 0x2A;
        public const byte BTH_ERROR_QOS_UNACCEPTABLE_PARAMETER = 0x2C;
        public const byte BTH_ERROR_QOS_REJECTED = 0x2D;
        public const byte BTH_ERROR_CHANNEL_CLASSIFICATION_NOT_SUPPORTED = 0x2E;
        public const byte BTH_ERROR_INSUFFICIENT_SECURITY = 0x2F;
        public const byte BTH_ERROR_PARAMETER_OUT_OF_MANDATORY_RANGE = 0x30;
        public const byte BTH_ERROR_ROLE_SWITCH_PENDING = 0x32;
        public const byte BTH_ERROR_RESERVED_SLOT_VIOLATION = 0x34;
        public const byte BTH_ERROR_ROLE_SWITCH_FAILED = 0x35;
        public const byte BTH_ERROR_EXTENDED_INQUIRY_RESPONSE_TOO_LARGE = 0x36;
        public const byte BTH_ERROR_SECURE_SIMPLE_PAIRING_NOT_SUPPORTED_BY_HOST = 0x37;
        public const byte BTH_ERROR_HOST_BUSY_PAIRING = 0x38;
        public const byte BTH_ERROR_CONNECTION_REJECTED_DUE_TO_NO_SUITABLE_CHANNEL_FOUND = 0x39;
        public const byte BTH_ERROR_CONTROLLER_BUSY = 0x3A;
        public const byte BTH_ERROR_UNACCEPTABLE_CONNECTION_INTERVAL = 0x3B;
        public const byte BTH_ERROR_DIRECTED_ADVERTISING_TIMEOUT = 0x3C;
        public const byte BTH_ERROR_CONNECTION_TERMINATED_DUE_TO_MIC_FAILURE = 0x3D;
        public const byte BTH_ERROR_CONNECTION_FAILED_TO_BE_ESTABLISHED = 0x3E;
        public const byte BTH_ERROR_MAC_CONNECTION_FAILED = 0x3F;
        public const byte BTH_ERROR_UNSPECIFIED = 0xFF;

        // '
        // ' Min, max, and default L2cap MTU.
        // '
        public const uint L2CAP_MIN_MTU = 48U;
        public const uint L2CAP_MAX_MTU = 0xFFFF;
        public const uint L2CAP_DEFAULT_MTU = 672U;

        // '
        // ' Max l2cap signal size (48) - size of signal header (4)
        // '
        public const uint MAX_L2CAP_PING_DATA_LENGTH = 44U;
        public const uint MAX_L2CAP_INFO_DATA_LENGTH = 44U;

        // '
        // ' the following structures provide information about
        // ' discovered remote radios.
        // '

        public const uint BDIF_ADDRESS = 0x1;
        public const uint BDIF_COD = 0x2;
        public const uint BDIF_NAME = 0x4;
        public const uint BDIF_PAIRED = 0x8;
        public const uint BDIF_PERSONAL = 0x10;
        public const uint BDIF_CONNECTED = 0x20;

        // '
        // ' Support added in KB942567
        // '
        // (NTDDI_VERSION > NTDDI_VISTASP1 || _

        public const uint BDIF_SHORT_NAME = 0x40;
        public const uint BDIF_VISIBLE = 0x80;
        public const uint BDIF_SSP_SUPPORTED = 0x100;
        public const uint BDIF_SSP_PAIRED = 0x200;
        public const uint BDIF_SSP_MITM_PROTECTED = 0x400;
        public const uint BDIF_RSSI = 0x1000;
        public const uint BDIF_EIR = 0x2000;

        // (NTDDI_VERSION >= NTDDI_WIN8) '' >= WIN8

        public const uint BDIF_BR = 0x4000;
        public const uint BDIF_LE = 0x8000;
        public const uint BDIF_LE_PAIRED = 0x10000;
        public const uint BDIF_LE_PERSONAL = 0x20000;
        public const uint BDIF_LE_MITM_PROTECTED = 0x40000;
        public const uint BDIF_LE_PRIVACY_ENABLED = 0x80000;
        public const uint BDIF_LE_RANDOM_ADDRESS_TYPE = 0x100000;

        // (NTDDI_VERSION >= NTDDI_WIN10) '' >= WIN10

        public const uint BDIF_LE_DISCOVERABLE = 0x200000;
        public const uint BDIF_LE_NAME = 0x400000;
        public const uint BDIF_LE_VISIBLE = 0x800000;

        // (NTDDI_VERSION >= NTDDI_WIN10_RS2) '' >= WIN10_RS2

        public const uint BDIF_LE_CONNECTED = 0x1000000;
        public const uint BDIF_LE_CONNECTABLE = 0x2000000;
        public const uint BDIF_CONNECTION_INBOUND = 0x4000000;
        public const uint BDIF_BR_SECURE_CONNECTION_PAIRED = 0x8000000;
        public const uint BDIF_LE_SECURE_CONNECTION_PAIRED = 0x10000000;
        public const uint BDIF_DEBUGKEY = 0x20000000;
        public const uint BDIF_LE_DEBUGKEY = 0x40000000;

        public static uint BDIF_VALID_FLAGS()
        {
            return BDIF_ADDRESS | BDIF_COD | BDIF_NAME | BDIF_PAIRED | BDIF_PERSONAL | BDIF_CONNECTED | BDIF_SHORT_NAME | BDIF_VISIBLE | BDIF_RSSI | BDIF_EIR | BDIF_SSP_PAIRED | BDIF_SSP_MITM_PROTECTED | BDIF_BR | BDIF_LE | BDIF_LE_PAIRED | BDIF_LE_PERSONAL | BDIF_LE_MITM_PROTECTED | BDIF_LE_PRIVACY_ENABLED | BDIF_LE_RANDOM_ADDRESS_TYPE | BDIF_LE_DISCOVERABLE | BDIF_LE_NAME | BDIF_LE_VISIBLE | BDIF_LE_CONNECTED | BDIF_LE_CONNECTABLE | BDIF_CONNECTION_INBOUND | BDIF_BR_SECURE_CONNECTION_PAIRED | BDIF_LE_SECURE_CONNECTION_PAIRED | BDIF_DEBUGKEY | BDIF_LE_DEBUGKEY;
        }

        // '' >= SP1+KB942567

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BTH_ADDR
        {
            public ulong Address;

            public static implicit operator BTH_ADDR(ulong var1)
            {
                return new BTH_ADDR(var1);
            }

            public static implicit operator ulong(BTH_ADDR var1)
            {
                return var1.Address;
            }

            public BTH_ADDR(ulong addr)
            {
                Address = addr;
            }

            public override string ToString()
            {
                return Address.ToString();
            }

            public string ToString(string format)
            {
                return Address.ToString(format);
            }

            public string ToString(IFormatProvider formatProvider)
            {
                return Address.ToString(formatProvider);
            }

            public string ToString(string format, IFormatProvider formatProvider)
            {
                return Address.ToString(format, formatProvider);
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BTH_COD
        {
            public ulong Value;

            public static implicit operator BTH_COD(ulong var1)
            {
                return new BTH_COD(var1);
            }

            public static implicit operator ulong(BTH_COD var1)
            {
                return var1.Value;
            }

            public BTH_COD(ulong cod)
            {
                Value = cod;
            }

            public override string ToString()
            {
                return Value.ToString();
            }

            public string ToString(string format)
            {
                return Value.ToString(format);
            }

            public string ToString(IFormatProvider formatProvider)
            {
                return Value.ToString(formatProvider);
            }

            public string ToString(string format, IFormatProvider formatProvider)
            {
                return Value.ToString(format, formatProvider);
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BTH_DEVICE_INFO
        {
            // '
            // ' Combination BDIF_Xxx flags
            // '
            public uint flags;

            // '
            // ' Address of remote device.
            // '
            public BTH_ADDR address;

            // '
            // ' Class Of Device.
            // '
            public BTH_COD classOfDevice;

            // '
            // ' name of the device
            // '
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BTH_MAX_NAME_SIZE)]
            public string name;
        }

        // '
        // ' Buffer associated with GUID_BLUETOOTH_RADIO_IN_RANGE
        // '

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BTH_RADIO_IN_RANGE
        {
            // '
            // ' Information about the remote radio
            // '
            public BTH_DEVICE_INFO deviceInfo;

            // '
            // ' The previous flags value for the BTH_DEVICE_INFO.  The receiver of this
            // ' notification can compare the deviceInfo.flags and previousDeviceFlags
            // ' to determine what has changed about this remote radio.
            // '
            // ' For instance, if BDIF_NAME is set in deviceInfo.flags and not in
            // ' previousDeviceFlags, the remote radio's has just been retrieved.
            // '
            public uint previousDeviceFlags;
        }

        // '
        // ' Buffer associated with GUID_BLUETOOTH_L2CAP_EVENT
        // '
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BTH_L2CAP_EVENT_INFO
        {
            // '
            // ' Remote radio address which the L2CAP event is associated with
            // '
            public BTH_ADDR bthAddress;

            // '
            // ' The PSM that is either being connected to or disconnected from
            // '
            public ushort psm;

            // '
            // ' If != 0, then the channel has just been established.  If = 0, then the
            // ' channel has been destroyed.  Notifications for a destroyed channel will
            // ' only be sent for channels successfully established.
            // '
            public byte connected;

            // '
            // ' If != 0, then the local host iniated the l2cap connection.  If = 0, then
            // ' the remote host initated the connection.  This field is only valid if
            // ' connect is != 0.
            // '
            public byte initiated;
        }

        public readonly static uint HCI_CONNECTION_TYPE_ACL = 1U;
        public readonly static uint HCI_CONNECTION_TYPE_SCO = 2U;
        public readonly static uint HCI_CONNECTION_TYPE_LE = 3U;

        // '
        // ' Fix typos
        // '
        public readonly static uint HCI_CONNNECTION_TYPE_ACL = HCI_CONNECTION_TYPE_ACL;
        public readonly static uint HCI_CONNNECTION_TYPE_SCO = HCI_CONNECTION_TYPE_SCO;



        // '
        // ' Buffer associated with GUID_BLUETOOTH_HCI_EVENT
        // '
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BTH_HCI_EVENT_INFO
        {
            // '
            // ' Remote radio address which the HCI event is associated with
            // '
            public BTH_ADDR bthAddress;

            // '
            // ' HCI_CONNNECTION_TYPE_XXX value
            // '
            public byte connectionType;

            // '
            // ' If != 0, then the underlying connection to the remote radio has just
            // ' been estrablished.  If = 0, then the underlying conneciton has just been
            // ' destroyed.
            // '
            public byte connected;
        }

        public enum IO_CAPABILITY
        {
            IoCaps_DisplayOnly = 0x0,
            IoCaps_DisplayYesNo = 0x1,
            IoCaps_KeyboardOnly = 0x2,
            IoCaps_NoInputNoOutput = 0x3,
            IoCaps_Undefined = 0xFF
        }

        public enum AUTHENTICATION_REQUIREMENTS
        {
            MITMProtectionNotRequired = 0x0,
            MITMProtectionRequired = 0x1,
            MITMProtectionNotRequiredBonding = 0x2,
            MITMProtectionRequiredBonding = 0x3,
            MITMProtectionNotRequiredGeneralBonding = 0x4,
            MITMProtectionRequiredGeneralBonding = 0x5,
            MITMProtectionNotDefined = 0xFF
        }

        public static bool IsMITMProtectionRequired(AUTHENTICATION_REQUIREMENTS requirements)
        {
            return AUTHENTICATION_REQUIREMENTS.MITMProtectionRequired == requirements | AUTHENTICATION_REQUIREMENTS.MITMProtectionRequiredBonding == requirements | AUTHENTICATION_REQUIREMENTS.MITMProtectionRequiredGeneralBonding == requirements;
        }

        // '' >= SP1+KB942567

        // '
        // ' Max length we allow for ServiceName in the remote SDP records
        // '
        public const uint BTH_MAX_SERVICE_NAME_SIZE = 256U;
        public const uint MAX_UUIDS_IN_QUERY = 12U;
        public const uint BTH_VID_DEFAULT_VALUE = 0xFFFF;
        public const uint SDP_ERROR_INVALID_SDP_VERSION = 0x1;
        public const uint SDP_ERROR_INVALID_RECORD_HANDLE = 0x2;
        public const uint SDP_ERROR_INVALID_REQUEST_SYNTAX = 0x3;
        public const uint SDP_ERROR_INVALID_PDU_SIZE = 0x4;
        public const uint SDP_ERROR_INVALID_CONTINUATION_STATE = 0x5;
        public const uint SDP_ERROR_INSUFFICIENT_RESOURCES = 0x6;

        // '
        // ' Defined by windows to handle server errors that are not described by the
        // ' above errors.  Start at &H0100 so we don't go anywhere near the spec
        // ' defined values.
        // '

        // '
        // ' Success, nothing went wrong
        // '
        public const uint SDP_ERROR_SUCCESS = 0x0;

        // '
        // ' The SDP PDU or parameters other than the SDP stream response was not correct
        // '
        public const uint SDP_ERROR_SERVER_INVALID_RESPONSE = 0x100;

        // '
        // ' The SDP response stream did not parse correctly.
        // '
        public const uint SDP_ERROR_SERVER_RESPONSE_DID_NOT_PARSE = 0x200;

        // '
        // ' The SDP response stream was successfully parsed, but did not match the
        // ' required format for the query.
        // '
        public const uint SDP_ERROR_SERVER_BAD_FORMAT = 0x300;

        // '
        // ' SDP was unable to send a continued query back to the server
        // '
        public const uint SDP_ERROR_COULD_NOT_SEND_CONTINUE = 0x400;

        // '
        // ' Server sent a response that was too large to fit in the caller's buffer.
        // '
        public const uint SDP_ERROR_RESPONSE_TOO_LARGE = 0x500;
        public readonly static uint SDP_ATTRIB_RECORD_HANDLE = 0x0;
        public readonly static uint SDP_ATTRIB_CLASS_ID_LIST = 0x1;
        public readonly static uint SDP_ATTRIB_RECORD_STATE = 0x2;
        public readonly static uint SDP_ATTRIB_SERVICE_ID = 0x3;
        public readonly static uint SDP_ATTRIB_PROTOCOL_DESCRIPTOR_LIST = 0x4;
        public readonly static uint SDP_ATTRIB_BROWSE_GROUP_LIST = 0x5;
        public readonly static uint SDP_ATTRIB_LANG_BASE_ATTRIB_ID_LIST = 0x6;
        public readonly static uint SDP_ATTRIB_INFO_TIME_TO_LIVE = 0x7;
        public readonly static uint SDP_ATTRIB_AVAILABILITY = 0x8;
        public readonly static uint SDP_ATTRIB_PROFILE_DESCRIPTOR_LIST = 0x9;
        public readonly static uint SDP_ATTRIB_DOCUMENTATION_URL = 0xA;
        public readonly static uint SDP_ATTRIB_CLIENT_EXECUTABLE_URL = 0xB;
        public readonly static uint SDP_ATTRIB_ICON_URL = 0xC;
        public const int SDP_ATTRIB_ADDITIONAL_PROTOCOL_DESCRIPTOR_LIST = 0xD;

        // '
        // ' Attribute IDs in the range of &H000D - &H01FF are reserved for future use
        // '
        public readonly static uint SDP_ATTRIB_PROFILE_SPECIFIC = 0x200;
        public readonly static uint LANG_BASE_LANGUAGE_INDEX = 0x0;
        public readonly static uint LANG_BASE_ENCODING_INDEX = 0x1;
        public readonly static uint LANG_BASE_OFFSET_INDEX = 0x2;
        public readonly static uint LANG_DEFAULT_ID = 0x100;
        public readonly static uint LANGUAGE_EN_US = 0x656E;
        public readonly static uint ENCODING_UTF_8 = 0x6A;
        public readonly static uint STRING_NAME_OFFSET = 0x0;
        public readonly static uint STRING_DESCRIPTION_OFFSET = 0x1;
        public readonly static uint STRING_PROVIDER_NAME_OFFSET = 0x2;
        public readonly static uint SDP_ATTRIB_SDP_VERSION_NUMBER_LIST = 0x200;
        public readonly static uint SDP_ATTRIB_SDP_DATABASE_STATE = 0x201;
        public readonly static uint SDP_ATTRIB_BROWSE_GROUP_ID = 0x200;
        public readonly static uint SDP_ATTRIB_CORDLESS_EXTERNAL_NETWORK = 0x301;
        public readonly static uint SDP_ATTRIB_FAX_CLASS_1_SUPPORT = 0x302;
        public readonly static uint SDP_ATTRIB_FAX_CLASS_2_0_SUPPORT = 0x303;
        public readonly static uint SDP_ATTRIB_FAX_CLASS_2_SUPPORT = 0x304;
        public readonly static uint SDP_ATTRIB_FAX_AUDIO_FEEDBACK_SUPPORT = 0x305;
        public readonly static uint SDP_ATTRIB_HEADSET_REMOTE_AUDIO_VOLUME_CONTROL = 0x302;
        public readonly static uint SDP_ATTRIB_LAN_LPSUBNET = 0x200;
        public readonly static uint SDP_ATTRIB_OBJECT_PUSH_SUPPORTED_FORMATS_LIST = 0x303;
        public readonly static uint SDP_ATTRIB_SYNCH_SUPPORTED_DATA_STORES_LIST = 0x301;

        // '  this is in the assigned numbers doc, but it does not show up in any profile
        public readonly static uint SDP_ATTRIB_SERVICE_VERSION = 0x300;
        public readonly static uint SDP_ATTRIB_PAN_NETWORK_ADDRESS = 0x306;
        public readonly static uint SDP_ATTRIB_PAN_WAP_GATEWAY = 0x307;
        public readonly static uint SDP_ATTRIB_PAN_HOME_PAGE_URL = 0x308;
        public readonly static uint SDP_ATTRIB_PAN_WAP_STACK_TYPE = 0x309;
        public readonly static uint SDP_ATTRIB_PAN_SECURITY_DESCRIPTION = 0x30A;
        public readonly static uint SDP_ATTRIB_PAN_NET_ACCESS_TYPE = 0x30B;
        public readonly static uint SDP_ATTRIB_PAN_MAX_NET_ACCESS_RATE = 0x30C;
        public readonly static uint SDP_ATTRIB_IMAGING_SUPPORTED_CAPABILITIES = 0x310;
        public readonly static uint SDP_ATTRIB_IMAGING_SUPPORTED_FEATURES = 0x311;
        public readonly static uint SDP_ATTRIB_IMAGING_SUPPORTED_FUNCTIONS = 0x312;
        public readonly static uint SDP_ATTRIB_IMAGING_TOTAL_DATA_CAPACITY = 0x313;
        public readonly static uint SDP_ATTRIB_DI_SPECIFICATION_ID = 0x200;
        public readonly static uint SDP_ATTRIB_DI_VENDOR_ID = 0x201;
        public readonly static uint SDP_ATTRIB_DI_PRODUCT_ID = 0x202;
        public readonly static uint SDP_ATTRIB_DI_VERSION = 0x203;
        public readonly static uint SDP_ATTRIB_DI_PRIMARY_RECORD = 0x204;
        public readonly static uint SDP_ATTRIB_DI_VENDOR_ID_SOURCE = 0x205;
        public readonly static uint SDP_ATTRIB_HID_DEVICE_RELEASE_NUMBER = 0x200;
        public readonly static uint SDP_ATTRIB_HID_PARSER_VERSION = 0x201;
        public readonly static uint SDP_ATTRIB_HID_DEVICE_SUBCLASS = 0x202;
        public readonly static uint SDP_ATTRIB_HID_COUNTRY_CODE = 0x203;
        public readonly static uint SDP_ATTRIB_HID_VIRTUAL_CABLE = 0x204;
        public readonly static uint SDP_ATTRIB_HID_RECONNECT_INITIATE = 0x205;
        public readonly static uint SDP_ATTRIB_HID_DESCRIPTOR_LIST = 0x206;
        public readonly static uint SDP_ATTRIB_HID_LANG_ID_BASE_LIST = 0x207;
        public readonly static uint SDP_ATTRIB_HID_SDP_DISABLE = 0x208;
        public readonly static uint SDP_ATTRIB_HID_BATTERY_POWER = 0x209;
        public readonly static uint SDP_ATTRIB_HID_REMOTE_WAKE = 0x20A;
        public readonly static uint SDP_ATTRIB_HID_PROFILE_VERSION = 0x20B;
        public readonly static uint SDP_ATTRIB_HID_SUPERVISION_TIMEOUT = 0x20C;
        public readonly static uint SDP_ATTRIB_HID_NORMALLY_CONNECTABLE = 0x20D;
        public readonly static uint SDP_ATTRIB_HID_BOOT_DEVICE = 0x20E;
        public readonly static uint SDP_ATTRIB_HID_SSR_HOST_MAX_LATENCY = 0x20F;
        public readonly static uint SDP_ATTRIB_HID_SSR_HOST_MIN_TIMEOUT = 0x210;
        public readonly static uint SDP_ATTRIB_A2DP_SUPPORTED_FEATURES = 0x311;
        public readonly static uint SDP_ATTRIB_AVRCP_SUPPORTED_FEATURES = 0x311;
        public readonly static uint SDP_ATTRIB_HFP_SUPPORTED_FEATURES = 0x311;

        // '
        // ' Profile specific values
        // '
        public readonly static uint AVRCP_SUPPORTED_FEATURES_CATEGORY_1 = 0x1;
        public readonly static uint AVRCP_SUPPORTED_FEATURES_CATEGORY_2 = 0x2;
        public readonly static uint AVRCP_SUPPORTED_FEATURES_CATEGORY_3 = 0x4;
        public readonly static uint AVRCP_SUPPORTED_FEATURES_CATEGORY_4 = 0x8;
        public readonly static uint AVRCP_SUPPORTED_FEATURES_CT_BROWSING = 0x40;
        public readonly static uint AVRCP_SUPPORTED_FEATURES_CT_COVER_ART_IMAGE_PROPERTIES = 0x80;
        public readonly static uint AVRCP_SUPPORTED_FEATURES_CT_COVER_ART_IMAGE = 0x100;
        public readonly static uint AVRCP_SUPPORTED_FEATURES_CT_COVER_ART_LINKED_THUMBNAIL = 0x200;
        public readonly static uint AVRCP_SUPPORTED_FEATURES_TG_PLAYER_APPLICATION_SETTINGS = 0x10;
        public readonly static uint AVRCP_SUPPORTED_FEATURES_TG_GROUP_NAVIGATION = 0x20;
        public readonly static uint AVRCP_SUPPORTED_FEATURES_TG_BROWSING = 0x40;
        public readonly static uint AVRCP_SUPPORTED_FEATURES_TG_MULTIPLE_PLAYER_APPLICATIONS = 0x80;
        public readonly static uint AVRCP_SUPPORTED_FEATURES_TG_COVER_ART = 0x100;
        public readonly static uint CORDLESS_EXTERNAL_NETWORK_PSTN = 0x1;
        public readonly static uint CORDLESS_EXTERNAL_NETWORK_ISDN = 0x2;
        public readonly static uint CORDLESS_EXTERNAL_NETWORK_GSM = 0x3;
        public readonly static uint CORDLESS_EXTERNAL_NETWORK_CDMA = 0x4;
        public readonly static uint CORDLESS_EXTERNAL_NETWORK_ANALOG_CELLULAR = 0x5;
        public readonly static uint CORDLESS_EXTERNAL_NETWORK_PACKET_SWITCHED = 0x6;
        public readonly static uint CORDLESS_EXTERNAL_NETWORK_OTHER = 0x7;
        public readonly static uint OBJECT_PUSH_FORMAT_VCARD_2_1 = 0x1;
        public readonly static uint OBJECT_PUSH_FORMAT_VCARD_3_0 = 0x2;
        public readonly static uint OBJECT_PUSH_FORMAT_VCAL_1_0 = 0x3;
        public readonly static uint OBJECT_PUSH_FORMAT_ICAL_2_0 = 0x4;
        public readonly static uint OBJECT_PUSH_FORMAT_VNOTE = 0x5;
        public readonly static uint OBJECT_PUSH_FORMAT_VMESSAGE = 0x6;
        public readonly static uint OBJECT_PUSH_FORMAT_ANY = 0xFF;
        public readonly static uint SYNCH_DATA_STORE_PHONEBOOK = 0x1;
        public readonly static uint SYNCH_DATA_STORE_CALENDAR = 0x3;
        public readonly static uint SYNCH_DATA_STORE_NOTES = 0x5;
        public readonly static uint SYNCH_DATA_STORE_MESSAGES = 0x6;
        public readonly static uint DI_VENDOR_ID_SOURCE_BLUETOOTH_SIG = 0x1;
        public readonly static uint DI_VENDOR_ID_SOURCE_USB_IF = 0x2;
        public readonly static uint PSM_SDP = 0x1;
        public readonly static uint PSM_RFCOMM = 0x3;
        public readonly static uint PSM_TCS_BIN = 0x5;
        public readonly static uint PSM_TCS_BIN_CORDLESS = 0x7;
        public readonly static uint PSM_BNEP = 0xF;
        public readonly static uint PSM_HID_CONTROL = 0x11;
        public readonly static uint PSM_HID_INTERRUPT = 0x13;
        public readonly static uint PSM_UPNP = 0x15;
        public readonly static uint PSM_AVCTP = 0x17;
        public readonly static uint PSM_AVDTP = 0x19;
        public readonly static uint PSM_AVCTP_BROWSE = 0x1B;
        public readonly static uint PSM_UDI_C_PLANE = 0x1D;
        public readonly static uint PSM_ATT = 0x1F;
        public readonly static uint PSM_3DSP = 0x21;
        public readonly static uint PSM_LE_IPSP = 0x23;

        // '
        // ' Strings
        // '
        public const string STR_ADDR_FMTW = "(%02x:%02x:%02x:%02x:%02x:%02x)";
        public const string STR_ADDR_SHORT_FMTW = "%04x%08x";
        public const string STR_USBHCI_CLASS_HARDWAREIDW = @"USB\\Class_E0&SubClass_01&Prot_01";

        // defined(UNICODE) || defined(BTH_KERN)

        public const string STR_ADDR_FMT = STR_ADDR_FMTW;
        public const string STR_ADDR_SHORT_FMT = STR_ADDR_SHORT_FMTW;
        public const string STR_USBHCI_CLASS_HARDWAREID = STR_USBHCI_CLASS_HARDWAREIDW;

        // '' UNICODE


        public static uint GET_BITS(uint field, uint offset, uint mask)
        {
            return field >> (int)offset & mask;
        }

        public static uint GET_BIT(uint field, uint offset)
        {
            return GET_BITS(field, offset, 0x1U);
        }

        public static uint LMP_3_SLOT_PACKETS(uint x)
        {
            return GET_BIT(x, 0U);
        }

        public static uint LMP_5_SLOT_PACKETS(uint x)
        {
            return GET_BIT(x, 1U);
        }

        public static uint LMP_ENCRYPTION(uint x)
        {
            return GET_BIT(x, 2U);
        }

        public static uint LMP_SLOT_OFFSET(uint x)
        {
            return GET_BIT(x, 3U);
        }

        public static uint LMP_TIMING_ACCURACY(uint x)
        {
            return GET_BIT(x, 4U);
        }

        public static uint LMP_SWITCH(uint x)
        {
            return GET_BIT(x, 5U);
        }

        public static uint LMP_HOLD_MODE(uint x)
        {
            return GET_BIT(x, 6U);
        }

        public static uint LMP_SNIFF_MODE(uint x)
        {
            return GET_BIT(x, 7U);
        }

        public static uint LMP_PARK_MODE(uint x)
        {
            return GET_BIT(x, 8U);
        }

        public static uint LMP_RSSI(uint x)
        {
            return GET_BIT(x, 9U);
        }

        public static uint LMP_CHANNEL_QUALITY_DRIVEN_MODE(uint x)
        {
            return GET_BIT(x, 10U);
        }

        public static uint LMP_SCO_LINK(uint x)
        {
            return GET_BIT(x, 11U);
        }

        public static uint LMP_HV2_PACKETS(uint x)
        {
            return GET_BIT(x, 12U);
        }

        public static uint LMP_HV3_PACKETS(uint x)
        {
            return GET_BIT(x, 13U);
        }

        public static uint LMP_MU_LAW_LOG(uint x)
        {
            return GET_BIT(x, 14U);
        }

        public static uint LMP_A_LAW_LOG(uint x)
        {
            return GET_BIT(x, 15U);
        }

        public static uint LMP_CVSD(uint x)
        {
            return GET_BIT(x, 16U);
        }

        public static uint LMP_PAGING_SCHEME(uint x)
        {
            return GET_BIT(x, 17U);
        }

        public static uint LMP_POWER_CONTROL(uint x)
        {
            return GET_BIT(x, 18U);
        }

        public static uint LMP_TRANSPARENT_SCO_DATA(uint x)
        {
            return GET_BIT(x, 19U);
        }

        public static uint LMP_FLOW_CONTROL_LAG(uint x)
        {
            return GET_BITS(x, 20U, 0x3U);
        }

        public static uint LMP_BROADCAST_ENCRYPTION(uint x)
        {
            return GET_BIT(x, 23U);
        }

        public static uint LMP_ENHANCED_DATA_RATE_ACL_2MBPS_MODE(uint x)
        {
            return GET_BIT(x, 25U);
        }

        public static uint LMP_ENHANCED_DATA_RATE_ACL_3MBPS_MODE(uint x)
        {
            return GET_BIT(x, 26U);
        }

        public static uint LMP_ENHANCED_INQUIRY_SCAN(uint x)
        {
            return GET_BIT(x, 27U);
        }

        public static uint LMP_INTERLACED_INQUIRY_SCAN(uint x)
        {
            return GET_BIT(x, 28U);
        }

        public static uint LMP_INTERLACED_PAGE_SCAN(uint x)
        {
            return GET_BIT(x, 29U);
        }

        public static uint LMP_RSSI_WITH_INQUIRY_RESULTS(uint x)
        {
            return GET_BIT(x, 30U);
        }

        public static uint LMP_ESCO_LINK(uint x)
        {
            return GET_BIT(x, 31U);
        }

        public static uint LMP_EV4_PACKETS(uint x)
        {
            return GET_BIT(x, 32U);
        }

        public static uint LMP_EV5_PACKETS(uint x)
        {
            return GET_BIT(x, 33U);
        }

        public static uint LMP_AFH_CAPABLE_SLAVE(uint x)
        {
            return GET_BIT(x, 35U);
        }

        public static uint LMP_AFH_CLASSIFICATION_SLAVE(uint x)
        {
            return GET_BIT(x, 36U);
        }

        public static uint LMP_BR_EDR_NOT_SUPPORTED(uint x)
        {
            return GET_BIT(x, 37U);
        }

        public static uint LMP_LE_SUPPORTED(uint x)
        {
            return GET_BIT(x, 38U);
        }

        public static uint LMP_3SLOT_EDR_ACL_PACKETS(uint x)
        {
            return GET_BIT(x, 39U);
        }

        public static uint LMP_5SLOT_EDR_ACL_PACKETS(uint x)
        {
            return GET_BIT(x, 40U);
        }

        public static uint LMP_SNIFF_SUBRATING(uint x)
        {
            return GET_BIT(x, 41U);
        }

        public static uint LMP_PAUSE_ENCRYPTION(uint x)
        {
            return GET_BIT(x, 42U);
        }

        public static uint LMP_AFH_CAPABLE_MASTER(uint x)
        {
            return GET_BIT(x, 43U);
        }

        public static uint LMP_AFH_CLASSIFICATION_MASTER(uint x)
        {
            return GET_BIT(x, 44U);
        }

        public static uint LMP_EDR_ESCO_2MBPS_MODE(uint x)
        {
            return GET_BIT(x, 45U);
        }

        public static uint LMP_EDR_ESCO_3MBPS_MODE(uint x)
        {
            return GET_BIT(x, 46U);
        }

        public static uint LMP_3SLOT_EDR_ESCO_PACKETS(uint x)
        {
            return GET_BIT(x, 47U);
        }

        public static uint LMP_EXTENDED_INQUIRY_RESPONSE(uint x)
        {
            return GET_BIT(x, 48U);
        }

        public static uint LMP_SIMULT_LE_BR_TO_SAME_DEV(uint x)
        {
            return GET_BIT(x, 49U);
        }

        public static uint LMP_SECURE_SIMPLE_PAIRING(uint x)
        {
            return GET_BIT(x, 51U);
        }

        public static uint LMP_ENCAPSULATED_PDU(uint x)
        {
            return GET_BIT(x, 52U);
        }

        public static uint LMP_ERRONEOUS_DATA_REPORTING(uint x)
        {
            return GET_BIT(x, 53U);
        }

        public static uint LMP_NON_FLUSHABLE_PACKET_BOUNDARY_FLAG(uint x)
        {
            return GET_BIT(x, 54U);
        }

        public static uint LMP_LINK_SUPERVISION_TIMEOUT_CHANGED_EVENT(uint x)
        {
            return GET_BIT(x, 56U);
        }

        public static uint LMP_INQUIRY_RESPONSE_TX_POWER_LEVEL(uint x)
        {
            return GET_BIT(x, 57U);
        }

        public static uint LMP_EXTENDED_FEATURES(uint x)
        {
            return GET_BIT(x, 63U);
        }



        // '
        // ' IOCTL defines. 
        // '
        public readonly static uint BTH_IOCTL_BASE = 0U;

        public static DiskApi.CTL_CODE BTH_CTL(uint id)
        {
            return new DiskApi.CTL_CODE(IoControl.FILE_DEVICE_BLUETOOTH, id, DiskApi.METHOD_BUFFERED, DiskApi.FILE_ANY_ACCESS);
        }

        public static DiskApi.CTL_CODE BTH_KERNEL_CTL(uint id)
        {
            return new DiskApi.CTL_CODE(IoControl.FILE_DEVICE_BLUETOOTH, id, DiskApi.METHOD_NEITHER, DiskApi.FILE_ANY_ACCESS);
        }


        // '
        // ' kernel-level (internal) IOCTLs
        // '
        public readonly static DiskApi.CTL_CODE IOCTL_INTERNAL_BTH_SUBMIT_BRB = BTH_KERNEL_CTL((uint)(BTH_IOCTL_BASE + 0x0L));

        // '
        // ' Input:  none
        // ' Output:  BTH_ENUMERATOR_INFO
        // '
        public readonly static DiskApi.CTL_CODE IOCTL_INTERNAL_BTHENUM_GET_ENUMINFO = BTH_KERNEL_CTL((uint)(BTH_IOCTL_BASE + 0x1L));

        // '
        // ' Input:  none
        // ' Output:  BTH_DEVICE_INFO
        // '
        public readonly static DiskApi.CTL_CODE IOCTL_INTERNAL_BTHENUM_GET_DEVINFO = BTH_KERNEL_CTL((uint)(BTH_IOCTL_BASE + 0x2L));

        // '
        // ' IOCTLs 
        // '
        // '
        // ' Input:  none
        // ' Output:  BTH_LOCAL_RADIO_INFO
        // '
        public readonly static DiskApi.CTL_CODE IOCTL_BTH_GET_LOCAL_INFO = BTH_CTL((uint)(BTH_IOCTL_BASE + 0x0L));

        // '
        // ' Input:  BTH_ADDR
        // ' Output:  BTH_RADIO_INFO
        // '
        public readonly static DiskApi.CTL_CODE IOCTL_BTH_GET_RADIO_INFO = BTH_CTL((uint)(BTH_IOCTL_BASE + 0x1L));

        // '
        // ' use this ioctl to get a list of cached discovered devices in the port driver.
        // '
        // ' Input: None
        // ' Output: BTH_DEVICE_INFO_LIST
        // '
        public readonly static DiskApi.CTL_CODE IOCTL_BTH_GET_DEVICE_INFO = BTH_CTL((uint)(BTH_IOCTL_BASE + 0x2L));

        // '
        // ' Input:  BTH_ADDR
        // ' Output:  none
        // '
        public readonly static DiskApi.CTL_CODE IOCTL_BTH_DISCONNECT_DEVICE = BTH_CTL((uint)(BTH_IOCTL_BASE + 0x3L));
        // '
        // ' Input:   BTH_VENDOR_SPECIFIC_COMMAND 
        // ' Output:  PVOID
        // '
        public readonly static DiskApi.CTL_CODE IOCTL_BTH_HCI_VENDOR_COMMAND = BTH_CTL((uint)(BTH_IOCTL_BASE + 0x14L));
        // '
        // ' Input:  BTH_SDP_CONNECT
        // ' Output:  BTH_SDP_CONNECT
        // '
        public readonly static DiskApi.CTL_CODE IOCTL_BTH_SDP_CONNECT = BTH_CTL((uint)(BTH_IOCTL_BASE + 0x80L));

        // '
        // ' Input:  HANDLE_SDP
        // ' Output:  none
        // '
        public readonly static DiskApi.CTL_CODE IOCTL_BTH_SDP_DISCONNECT = BTH_CTL((uint)(BTH_IOCTL_BASE + 0x81L));

        // '
        // ' Input:  BTH_SDP_SERVICE_SEARCH_REQUEST
        // ' Output:  ULong * number of handles wanted
        // '
        public readonly static DiskApi.CTL_CODE IOCTL_BTH_SDP_SERVICE_SEARCH = BTH_CTL((uint)(BTH_IOCTL_BASE + 0x82L));

        // '
        // ' Input:  BTH_SDP_ATTRIBUTE_SEARCH_REQUEST
        // ' Output:  BTH_SDP_STREAM_RESPONSE Or bigger
        // '
        public readonly static DiskApi.CTL_CODE IOCTL_BTH_SDP_ATTRIBUTE_SEARCH = BTH_CTL((uint)(BTH_IOCTL_BASE + 0x83L));

        // '
        // ' Input:  BTH_SDP_SERVICE_ATTRIBUTE_SEARCH_REQUEST
        // ' Output:  BTH_SDP_STREAM_RESPONSE Or bigger
        // '
        public readonly static DiskApi.CTL_CODE IOCTL_BTH_SDP_SERVICE_ATTRIBUTE_SEARCH = BTH_CTL((uint)(BTH_IOCTL_BASE + 0x84L));

        // '
        // ' Input:  raw SDP stream (at least 2 bytes)
        // ' Ouptut: HANDLE_SDP
        // '
        public readonly static DiskApi.CTL_CODE IOCTL_BTH_SDP_SUBMIT_RECORD = BTH_CTL((uint)(BTH_IOCTL_BASE + 0x85L));

        // '
        // ' Input:  HANDLE_SDP
        // ' Output:  none
        // '
        public readonly static DiskApi.CTL_CODE IOCTL_BTH_SDP_REMOVE_RECORD = BTH_CTL((uint)(BTH_IOCTL_BASE + 0x86L));

        // '
        // ' Input:  BTH_SDP_RECORD + raw SDP record
        // ' Output:  HANDLE_SDP
        // '
        public readonly static DiskApi.CTL_CODE IOCTL_BTH_SDP_SUBMIT_RECORD_WITH_INFO = BTH_CTL((uint)(BTH_IOCTL_BASE + 0x87L));

        // '
        // ' Input:  NONE
        // ' Output:  BTH_HOST_FEATURE_MASK
        // '
        public readonly static DiskApi.CTL_CODE IOCTL_BTH_GET_HOST_SUPPORTED_FEATURES = BTH_CTL((uint)(BTH_IOCTL_BASE + 0x88L));

        public struct BTH_DEVICE_INFO_LIST
        {

            // 
            // [IN/OUT] minimum of 1 device required
            // 
            public uint numOfDevices;

            // 
            // Open ended array of devices;
            // 
            public BTH_DEVICE_INFO[] deviceList;

            public SafePtr ToPointer()
            {
                var mm = new SafePtr();
                int sz = Marshal.SizeOf<BTH_DEVICE_INFO>();
                int ofs = 4;
                mm.Alloc(4L + sz * numOfDevices);
                numOfDevices = (uint)deviceList.Length;
                mm.UIntAt(0L) = numOfDevices;
                for (int i = 0, loopTo = (int)(numOfDevices - 1L); i <= loopTo; i++)
                {
                    mm.FromStructAt(ofs, deviceList[i]);
                    ofs += sz;
                }

                return mm;
            }

            public static BTH_DEVICE_INFO_LIST FromPointer(IntPtr ptr)
            {
                BTH_DEVICE_INFO_LIST op;
                MemPtr mm = ptr;
                int sz = Marshal.SizeOf<BTH_DEVICE_INFO>();
                int ofs = 4;
                op.numOfDevices = mm.UIntAt(0L);
                op.deviceList = new BTH_DEVICE_INFO[(int)(op.numOfDevices - 1L) + 1];
                for (int i = 0, loopTo = (int)(op.numOfDevices - 1L); i <= loopTo; i++)
                {
                    op.deviceList[i] = mm.ToStructAt<BTH_DEVICE_INFO>(ofs);
                    ofs += sz;
                }

                return op;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct PBTH_DEVICE_INFO_LIST
        {
            internal MemPtr _ptr;

            public PBTH_DEVICE_INFO_LIST(int allocSize)
            {
                _ptr = new MemPtr();
                Alloc(CalcElementsFromSize(allocSize));
            }

            public static int CalcElementsFromSize(int size)
            {
                size -= Marshal.SizeOf<uint>();
                size = (int)(size / (double)Marshal.SizeOf<BTH_DEVICE_INFO>());
                return size;
            }

            public void Alloc(int elements)
            {
                Free();
                int len = Marshal.SizeOf<uint>() + Marshal.SizeOf<BTH_DEVICE_INFO>() * elements;

                _ptr.Alloc(len);
            }

            public void Free()
            {
                if (_ptr != IntPtr.Zero & _ptr != (IntPtr)(-1))
                {
                    try
                    {
                        _ptr.Free();
                    }
                    catch (Exception ex)
                    {
                    }

                    _ptr = IntPtr.Zero;
                }
            }

            // '
            // ' [IN/OUT] minimum of 1 device required
            // '
            // Public numOfDevices As UInteger

            // '
            // ' Open ended array of devices;
            // '
            // Public deviceList As BTH_DEVICE_INFO

            public uint NumberOfDevices
            {
                get
                {
                    return _ptr.UIntAt(0L);
                }
            }

            public BTH_DEVICE_INFO get_Devices(uint index)
            {
                var offset = new IntPtr(Marshal.SizeOf<uint>() + Marshal.SizeOf<BTH_DEVICE_INFO>() * index);
                var mm = _ptr + offset;
                return mm.ToStruct<BTH_DEVICE_INFO>();
            }

            public static explicit operator IntPtr(PBTH_DEVICE_INFO_LIST var1)
            {
                return var1._ptr.Handle;
            }

            public static explicit operator PBTH_DEVICE_INFO_LIST(IntPtr var1)
            {
                var p = new PBTH_DEVICE_INFO_LIST();
                p._ptr = var1;
                return p;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BTH_RADIO_INFO
        {
            // '
            // ' Supported LMP features of the radio.  Use LMP_XXX() to extract
            // ' the desired bits.
            // '
            public ulong lmpSupportedFeatures;

            // '
            // ' Manufacturer ID (possibly BTH_MFG_XXX)
            // '
            public BTH_MFG_INFO mfg;

            // '
            // ' LMP subversion
            // '
            public ushort lmpSubversion;

            // '
            // ' LMP version
            // '
            public byte lmpVersion;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BTH_LOCAL_RADIO_INFO
        {
            // '
            // ' Local BTH_ADDR, class of defice, And radio name
            // '
            public BTH_DEVICE_INFO localInfo;

            // '
            // ' Combo of LOCAL_RADIO_XXX values
            // '
            public uint flags;

            // '
            // ' HCI revision, see core spec
            // '
            public ushort hciRevision;

            // '
            // ' HCI version, see core spec
            // '
            public byte hciVersion;

            // '
            // ' More information about the local radio (LMP, MFG)
            // '
            public BTH_RADIO_INFO radioInfo;
        }

        public const int SDP_CONNECT_CACHE = 0x1;
        public const int SDP_CONNECT_ALLOW_PIN = 0x2;
        public const int SDP_REQUEST_TO_DEFAULT = 0;
        public const int SDP_REQUEST_TO_MIN = 10;
        public const int SDP_REQUEST_TO_MAX = 45;
        public const int SERVICE_OPTION_DO_NOT_PUBLISH = 0x2;
        public const int SERVICE_OPTION_NO_PUBLIC_BROWSE = 0x4;
        public const int SERVICE_OPTION_DO_NOT_PUBLISH_EIR = 0x8;
        public const int SERVICE_SECURITY_USE_DEFAULTS = 0x0;
        public const int SERVICE_SECURITY_NONE = 0x1;
        public const int SERVICE_SECURITY_AUTHORIZE = 0x2;
        public const int SERVICE_SECURITY_AUTHENTICATE = 0x4;
        public const int SERVICE_SECURITY_ENCRYPT_REQUIRED = 0x10;
        public const int SERVICE_SECURITY_ENCRYPT_OPTIONAL = 0x20;
        public const int SERVICE_SECURITY_DISABLED = 0x10000000;
        public const int SERVICE_SECURITY_NO_ASK = 0x20000000;

        // '
        // ' Do Not attempt to validate that the stream can be parsed
        // '
        public const int SDP_SEARCH_NO_PARSE_CHECK = 0x1;

        // '
        // ' Do Not check the format of the results.  This includes suppression of both
        // ' the check for a record patten (SEQ of UINT16 + value) And the validation
        // ' of each universal attribute's accordance to the spec.
        // '
        public const int SDP_SEARCH_NO_FORMAT_CHECK = 0x2;
        public readonly static IntPtr HANDLE_SDP_NULL = IntPtr.Zero;
        public readonly static IntPtr HANDLE_SDP_LOCAL = new IntPtr(-2);

        public struct SDP_LARGE_INTEGER_16
        {
            public ulong LowPart;
            public long Highpart;
        }

        public struct SDP_ULARGE_INTEGER_16
        {
            public ulong LowPart;
            public ulong Highpart;
        }

        public enum NodeContainerType
        {
            NodeContainerTypeSequence,
            NodeContainerTypeAlternative
        }

        public enum SDP_TYPE
        {
            SDP_TYPE_NIL = 0x0,
            SDP_TYPE_UINT = 0x1,
            SDP_TYPE_INT = 0x2,
            SDP_TYPE_UUID = 0x3,
            SDP_TYPE_STRING = 0x4,
            SDP_TYPE_BOOLEAN = 0x5,
            SDP_TYPE_SEQUENCE = 0x6,
            SDP_TYPE_ALTERNATIVE = 0x7,
            SDP_TYPE_URL = 0x8,
            SDP_TYPE_CONTAINER = 0x20
        }
        // '  9 - 31 are reserved


        // ' allow for a little easier type checking / sizing for integers And UUIDs
        // ' ((SDP_ST_XXX & &HF0) >> 4) == SDP_TYPE_XXX
        // ' size of the data (in bytes) Is encoded as ((SDP_ST_XXX & &HF0) >> 8)
        public enum SDP_SPECIFICTYPE
        {
            SDP_ST_NONE = 0x0,
            SDP_ST_UINT8 = 0x10,
            SDP_ST_UINT16 = 0x110,
            SDP_ST_UINT32 = 0x210,
            SDP_ST_UINT64 = 0x310,
            SDP_ST_UINT128 = 0x410,
            SDP_ST_INT8 = 0x20,
            SDP_ST_INT16 = 0x120,
            SDP_ST_INT32 = 0x220,
            SDP_ST_INT64 = 0x320,
            SDP_ST_INT128 = 0x420,
            SDP_ST_UUID16 = 0x130,
            SDP_ST_UUID32 = 0x220,
            SDP_ST_UUID128 = 0x430
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SdpAttributeRange
        {
            public ushort minAttribute;
            public ushort maxAttribute;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BTH_SDP_CONNECT
        {
            // '
            // ' Address of the remote SDP server.  Cannot be the local radio.
            // '
            public BTH_ADDR bthAddress;

            // '
            // ' Combination of SDP_CONNECT_XXX flags
            // '
            public uint fSdpConnect;

            // '
            // ' When the connect request returns, this will specify the handle to the
            // ' SDP connection to the remote server
            // '
            public IntPtr hConnection;

            // '
            // ' Timeout, in seconds, for the requests on ths SDP channel.  If the request
            // ' times out, the SDP connection represented by the HANDLE_SDP must be
            // ' closed.  The values for this field are bound by SDP_REQUEST_TO_MIN And
            // ' SDP_REQUEST_MAX.  If SDP_REQUEST_TO_DEFAULT Is specified, the timeout Is
            // ' 30 seconds.
            // '
            public byte requestTimeout;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BTH_SDP_DISCONNECT
        {
            // '
            // ' hConnection returned by BTH_SDP_CONNECT
            // '
            public IntPtr hConnection;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BTH_SDP_RECORD
        {
            // '
            // ' Combination of SERVICE_SECURITY_XXX flags
            // '
            public uint fSecurity;

            // '
            // ' Combination of SERVICE_OPTION_XXX flags
            // '
            public uint fOptions;

            // '
            // ' combo of COD_SERVICE_XXX flags
            // '
            public uint fCodService;

            // '
            // ' The length of the record array, in bytes.
            // '
            public uint recordLength;

            // '
            // ' The SDP record in its raw format
            // '
            public byte[] record;

            public SafePtr ToPointer()
            {
                var mm = new SafePtr();
                recordLength = (uint)record.Length;
                mm.Alloc(16L + recordLength);
                mm.UIntAt(0L) = fSecurity;
                mm.UIntAt(1L) = fOptions;
                mm.UIntAt(2L) = fCodService;
                mm.UIntAt(3L) = recordLength;
                mm.FromByteArray(record, 16L);
                return mm;
            }

            public static BTH_SDP_RECORD FromPointer(IntPtr ptr)
            {
                BTH_SDP_RECORD op;
                MemPtr mm = ptr;
                op.fSecurity = mm.UIntAt(0L);
                op.fOptions = mm.UIntAt(1L);
                op.fCodService = mm.UIntAt(2L);
                op.recordLength = mm.UIntAt(3L);
                op.record = mm.ToByteArray(16L, op.recordLength);
                return op;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BTH_SDP_SERVICE_SEARCH_REQUEST
        {
            // '
            // ' Handle returned by the connect request Or HANDLE_SDP_LOCAL
            // '
            public IntPtr hConnection;

            // '
            // ' Array of UUIDs.  Each entry can be either a 2 byte, 4 byte Or 16 byte
            // ' UUID. SDP spec mandates that a request can have a maximum of 12 UUIDs.
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)MAX_UUIDS_IN_QUERY)]
            public Guid[] uuids;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BTH_SDP_ATTRIBUTE_SEARCH_REQUEST
        {
            // '
            // ' Handle returned by the connect request Or HANDLE_SDP_LOCAL
            // '
            public IntPtr hConnection;

            // '
            // ' Combo of SDP_SEARCH_Xxx flags
            // '
            public uint searchFlags;

            // '
            // ' Record handle returned by the remote SDP server, most likely from a
            // ' previous BTH_SDP_SERVICE_SEARCH_RESPONSE.
            // '
            public uint recordHandle;

            // '
            // ' Array of attributes to query for.  Each SdpAttributeRange entry can
            // ' specify either a single attribute Or a range.  To specify a single
            // ' attribute, minAttribute should be equal to maxAttribute.   The array must
            // ' be in sorted order, starting with the smallest attribute.  Furthermore,
            // ' if a range Is specified, the minAttribute must be <= maxAttribute.
            // '
            public SdpAttributeRange[] range;

            public SafePtr ToPointer()
            {
                var mm = new SafePtr();
                int sz = Marshal.SizeOf<SdpAttributeRange>();
                int ofs = IntPtr.Size;
                int size = sz * range.Length + IntPtr.Size + 8;
                mm.Alloc(size);
                if (ofs == 4)
                {
                    mm.UIntAt(0L) = (uint)hConnection;
                }
                else
                {
                    mm.ULongAt(0L) = (ulong)hConnection;
                }

                mm.UIntAtAbsolute(ofs) = searchFlags;
                mm.UIntAtAbsolute(ofs + 4) = recordHandle;
                ofs += 8;
                for (int i = 0, loopTo = range.Length - 1; i <= loopTo; i++)
                {
                    mm.FromStructAt(ofs, range[i]);
                    ofs += sz;
                }

                return mm;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BTH_SDP_SERVICE_ATTRIBUTE_SEARCH_REQUEST
        {
            // '
            // ' Handle returned by the connect request Or HANDLE_SDP_LOCAL
            // '
            public IntPtr hConnection;

            // '
            // ' Combo of SDP_SEARCH_Xxx flags
            // '
            public uint searchFlags;

            // '
            // ' See comments in BTH_SDP_SERVICE_SEARCH_REQUEST
            // '
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)MAX_UUIDS_IN_QUERY)]
            public Guid[] uuids;

            // '
            // ' See comments in BTH_SDP_ATTRIBUTE_SEARCH_REQUEST
            // '
            public SdpAttributeRange[] range;

            public SafePtr ToPointer()
            {
                var mm = new SafePtr();
                int sz = Marshal.SizeOf<SdpAttributeRange>();
                int szg = Marshal.SizeOf<Guid>();
                int ofs = IntPtr.Size;
                long size = sz * range.Length + IntPtr.Size + 4 + MAX_UUIDS_IN_QUERY * szg;
                mm.Alloc(size);
                if (ofs == 4)
                {
                    mm.UIntAt(0L) = (uint)hConnection;
                }
                else
                {
                    mm.ULongAt(0L) = (ulong)hConnection;
                }

                mm.UIntAtAbsolute(ofs) = searchFlags;
                ofs += 4;
                for (int j = 0; j <= (int)(MAX_UUIDS_IN_QUERY - 1L); j++)
                {
                    mm.FromStructAt(ofs, uuids[j]);
                    ofs += szg;
                }

                for (int i = 0, loopTo = range.Length - 1; i <= loopTo; i++)
                {
                    mm.FromStructAt(ofs, range[i]);
                    ofs += sz;
                }

                return mm;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BTH_SDP_STREAM_RESPONSE
        {
            // '
            // ' The required buffer size (Not including the first 2 ULONG_PTRs of this
            // ' data structure) needed to contain the response.
            // '
            // ' If the buffer passed was large enough to contain the entire response,
            // ' requiredSize will be equal to responseSize.  Otherwise, the caller should
            // ' resubmit the request with a buffer size equal to
            // ' sizeof(BTH_SDP_STREAM_RESPONSE) + requiredSize - 1.  (The -1 Is because
            // ' the size of this data structure already includes one byte of the
            // ' response.)
            // '
            // ' A response cannot exceed 4GB in size.
            // '
            public uint requiredSize;

            // '
            // ' The number of bytes copied into the response array of this data
            // ' structure.  If there Is Not enough room for the entire response, the
            // ' response will be partially copied into the response array.
            // '
            public uint responseSize;

            // '
            // ' The raw SDP response from the search.
            // '
            public byte[] response;

            public static BTH_SDP_STREAM_RESPONSE FromPointer(IntPtr ptr)
            {
                BTH_SDP_STREAM_RESPONSE op;
                MemPtr mm = ptr;
                op.requiredSize = mm.UIntAt(0L);
                op.responseSize = mm.UIntAt(1L);
                op.response = mm.ToByteArray(8L, op.responseSize);
                return op;
            }
        }

        // '
        // ' Vendor specific HCI command header
        // '
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BTH_COMMAND_HEADER
        {

            // '
            // ' Opcode for the command
            // ' 
            public ushort OpCode;

            // '
            // ' Payload of the command excluding the header.
            // ' TotalParameterLength = TotalCommandLength - sizeof(BTH_COMMAND_HEADER)
            // ' 
            public byte TotalParameterLength;
        }

        // '
        // ' Vendor Specific Command structure
        // '
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BTH_VENDOR_SPECIFIC_COMMAND
        {
            // '
            // ' Manufacturer ID
            // ' 
            public uint ManufacturerId;

            // '
            // ' LMP version. Command Is send to radio only if the radios 
            // ' LMP version Is greater than this value.
            // ' 
            public byte LmpVersion;

            // '
            // ' Should all the patterns match Or just one. If MatchAnySinglePattern == TRUE
            // ' then if a single pattern matches the command, we decide that we have a match.
            [MarshalAs(UnmanagedType.Bool)]
            public bool MatchAnySinglePattern;

            // '
            // ' HCI Command Header
            // '
            public BTH_COMMAND_HEADER HciHeader;

            // '
            // ' Data for the above command including patterns
            // '
            public byte[] Data;

            public SafePtr ToPointer()
            {
                var mm = new SafePtr();
                int size = 6 + Data.Length + Marshal.SizeOf<BTH_COMMAND_HEADER>();
                int dataStart = 6 + Marshal.SizeOf<BTH_COMMAND_HEADER>();
                HciHeader.TotalParameterLength = (byte)Data.Length;
                mm.Alloc(size);
                mm.UIntAt(0L) = ManufacturerId;
                mm.ByteAt(4L) = LmpVersion;
                mm.ByteAt(5L) = MatchAnySinglePattern ? 1 : 0;
                mm.FromStructAt(6L, HciHeader);
                mm.FromByteArray(Data, dataStart);
                return mm;
            }

            public static BTH_VENDOR_SPECIFIC_COMMAND FromPointer(IntPtr ptr)
            {
                BTH_VENDOR_SPECIFIC_COMMAND op;
                MemPtr mm = ptr;
                op.ManufacturerId = mm.UIntAt(0L);
                op.LmpVersion = mm.ByteAt(4L);
                op.MatchAnySinglePattern = mm.ByteAt(5L) == 1;
                op.HciHeader = mm.ToStructAt<BTH_COMMAND_HEADER>(6L);
                op.Data = mm.ToByteArray(6 + Marshal.SizeOf<BTH_COMMAND_HEADER>(), op.HciHeader.TotalParameterLength);
                return op;
            }
        }

        // '
        // ' Structure of patterns
        // '
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BTH_VENDOR_PATTERN
        {
            // '
            // ' Pattern Offset in the event structure excluding EVENT header
            // ' 
            public byte Offset;

            // '
            // ' Size of the Pattern
            // ' 
            public byte Size;

            // '
            // ' Pattern
            // ' 
            public byte[] Pattern;

            public SafePtr ToPointer()
            {
                var mm = new SafePtr();
                int size = 2 + Pattern.Length;
                Size = (byte)Pattern.Length;
                mm.Alloc(size);
                mm.ByteAt(0L) = Offset;
                mm.ByteAt(1L) = (byte)size;
                mm.FromByteArray(Pattern, 2L);
                return mm;
            }

            public static BTH_VENDOR_PATTERN FromPointer(IntPtr ptr)
            {
                MemPtr mm = ptr;
                BTH_VENDOR_PATTERN op;
                op.Offset = mm.ByteAt(0L);
                op.Size = mm.ByteAt(1L);
                op.Pattern = mm.ToByteArray(2L, op.Size);
                return op;
            }
        }

        // '
        // 'The buffer associated with GUID_BLUETOOTH_HCI_VENDOR_EVENT
        // '
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BTH_VENDOR_EVENT_INFO
        {
            // '
            // 'Local radio address with which the event Is associated.
            // '
            public BTH_ADDR BthAddress;

            // '
            // 'Size of the event buffer including Event header
            // '
            public uint EventSize;

            // '
            // 'Information associated with the event
            // '
            public byte[] EventInfo;

            public SafePtr ToPointer()
            {
                var mm = new SafePtr();
                int size = 12 + EventInfo.Length;
                EventSize = (uint)EventInfo.Length;
                mm.Alloc(size);
                mm.ULongAt(0L) = BthAddress;
                mm.UIntAtAbsolute(8L) = EventSize;
                mm.FromByteArray(EventInfo, 12L);
                return mm;
            }

            public static BTH_VENDOR_EVENT_INFO FromPointer(IntPtr ptr)
            {
                BTH_VENDOR_EVENT_INFO op;
                MemPtr mm = ptr;
                op.BthAddress = mm.ULongAt(0L);
                op.EventSize = mm.UIntAtAbsolute(8L);
                op.EventInfo = mm.ToByteArray(12L, op.EventSize);
                return op;
            }
        }



        // '
        // ' Host supported features
        // '
        public const ulong BTH_HOST_FEATURE_ENHANCED_RETRANSMISSION_MODE = 0x1;
        public const ulong BTH_HOST_FEATURE_STREAMING_MODE = 0x2;
        public const ulong BTH_HOST_FEATURE_LOW_ENERGY = 0x4;
        public const ulong BTH_HOST_FEATURE_SCO_HCI = 0x8;
        public const ulong BTH_HOST_FEATURE_SCO_HCIBYPASS = 0x10;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BTH_HOST_FEATURE_MASK
        {
            // '
            // ' Mask of supported features. 
            // '
            public ulong Mask;

            // '
            // ' Reserved for future use.
            // '
            public ulong Reserved1;
            public ulong Reserved2;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BLUETOOTH_DEVICE_SEARCH_PARAMS
        {
            public uint dwSize;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fReturnAuthenticated;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fReturnRemembered;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fReturnUnknown;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fReturnConnected;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fIssueInquiry;
            public byte cTimeoutMultiplier;
            public IntPtr hRadio;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BLUETOOTH_FIND_RADIO_PARAMS
        {
            public uint dwSize;
        }

        // HBLUETOOTH_RADIO_FIND BluetoothFindFirstRadio(
        // const BLUETOOTH_FIND_RADIO_PARAMS *pbtfrp,
        // HANDLE * phRadio
        // );

        public const int BLUETOOTH_MAX_NAME_SIZE = 248;
        public const int BLUETOOTH_MAX_PASSKEY_SIZE = 16;
        public const int BLUETOOTH_MAX_PASSKEY_BUFFER_SIZE = BLUETOOTH_MAX_PASSKEY_SIZE + 1;
        public const int BLUETOOTH_MAX_SERVICE_NAME_SIZE = 256;
        public const int BLUETOOTH_DEVICE_NAME_SIZE = 256;

        [DllImport("bluetoothapis.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr BluetoothFindFirstRadio(ref BLUETOOTH_FIND_RADIO_PARAMS pbtfrp, out IntPtr phradio);
        [DllImport("bluetoothapis.dll", CharSet = CharSet.Unicode)]
        public static extern bool BluetoothFindNextRadio(IntPtr hFind, out IntPtr phRadio);
        [DllImport("bluetoothapis.dll", CharSet = CharSet.Unicode)]
        public static extern bool BluetoothFindRadioClose(IntPtr hFind);
        [DllImport("bluetoothapis.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr BluetoothFindFirstDevice(BLUETOOTH_DEVICE_SEARCH_PARAMS pbtdsp, ref BLUETOOTH_DEVICE_INFO pbtdi);
        [DllImport("bluetoothapis.dll", CharSet = CharSet.Unicode)]
        public static extern bool BluetoothFindNextDevice(IntPtr hFind, ref BLUETOOTH_DEVICE_INFO pbtdi);
        [DllImport("bluetoothapis.dll", CharSet = CharSet.Unicode)]
        public static extern bool BluetoothFindDeviceClose(IntPtr hFind);
        [DllImport("bluetoothapis.dll", CharSet = CharSet.Unicode)]
        public static extern uint BluetoothGetRadioInfo(IntPtr hRadio, ref BLUETOOTH_RADIO_INFO pRadioInfo);

        public static BLUETOOTH_DEVICE_INFO[] _internalEnumBluetoothDevices()
        {
            var bl = new List<BLUETOOTH_DEVICE_INFO>();

            // Dim hDevice As IntPtr
            IntPtr hRadio;
            IntPtr hFind;
            IntPtr radFind;
            BLUETOOTH_FIND_RADIO_PARAMS radParams;
            var @params = default(BLUETOOTH_DEVICE_SEARCH_PARAMS);
            BLUETOOTH_DEVICE_INFO brInfo;
            bool fr;
            bool frad;
            radParams.dwSize = 4U;
            radFind = BluetoothFindFirstRadio(ref radParams, out hRadio);
            if (radFind == IntPtr.Zero)
                return null;
            do
            {
                @params.dwSize = (uint)Marshal.SizeOf<BLUETOOTH_DEVICE_SEARCH_PARAMS>();
                @params.fReturnRemembered = true;
                @params.fReturnAuthenticated = true;
                @params.fReturnConnected = true;
                @params.fReturnUnknown = true;
                @params.cTimeoutMultiplier = 2;
                @params.hRadio = hRadio;
                brInfo = new BLUETOOTH_DEVICE_INFO();
                brInfo.dwSize = Marshal.SizeOf<BLUETOOTH_DEVICE_INFO>();
                hFind = BluetoothFindFirstDevice(@params, ref brInfo);
                if (hFind == IntPtr.Zero)
                {
                    PInvoke.CloseHandle(hRadio);
                    BluetoothFindRadioClose(radFind);
                    bl.ToArray();
                }

                bl.Add(brInfo);
                do
                {
                    brInfo = new BLUETOOTH_DEVICE_INFO();
                    brInfo.dwSize = Marshal.SizeOf<BLUETOOTH_DEVICE_INFO>();
                    fr = BluetoothFindNextDevice(hFind, ref brInfo);
                    if (fr)
                        bl.Add(brInfo);
                }
                while (fr == true);
                BluetoothFindDeviceClose(hFind);
                PInvoke.CloseHandle(hRadio);
                frad = BluetoothFindNextRadio(radFind, out hRadio);
            }
            while (frad == true);
            BluetoothFindRadioClose(radFind);
            return bl.ToArray();
        }

        public static BLUETOOTH_RADIO_INFO[] _internalEnumBluetoothRadios()
        {
            var bl = new List<BLUETOOTH_RADIO_INFO>();
            IntPtr hRadio;
            IntPtr hFind;
            BLUETOOTH_FIND_RADIO_PARAMS @params;
            BLUETOOTH_RADIO_INFO brInfo;
            bool fr;
            @params.dwSize = 4U;
            hFind = BluetoothFindFirstRadio(ref @params, out hRadio);
            if (hFind == IntPtr.Zero)
                return null;
            do
            {
                brInfo = new BLUETOOTH_RADIO_INFO();
                brInfo.dwSize = (ulong)Marshal.SizeOf<BLUETOOTH_RADIO_INFO>();
                uint x = BluetoothGetRadioInfo(hRadio, ref brInfo);
                string s = NativeErrorMethods.FormatLastError(x);
                PInvoke.CloseHandle(hRadio);
                bl.Add(brInfo);
                fr = BluetoothFindNextRadio(hFind, out hRadio);
            }
            while (fr == true);
            BluetoothFindRadioClose(hFind);
            return bl.ToArray();
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct BLUETOOTH_ADDRESS
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] rgBytes;

        public override string ToString()
        {
            string ToStringRet = default;
            ToStringRet = "";
            if (rgBytes is null || rgBytes.Length < 5)
                return "";
            for (int i = 5; i >= 0; i -= 1)
            {
                if (!string.IsNullOrEmpty(ToStringRet))
                    ToStringRet += ":";
                ToStringRet += rgBytes[i].ToString("X2");
            }

            return ToStringRet;
        }

        public static explicit operator BLUETOOTH_ADDRESS(ulong val1)
        {
            BLUETOOTH_ADDRESS bt;
            bt.rgBytes = BitConverter.GetBytes(val1);
            return bt;
        }

        public static explicit operator ulong(BLUETOOTH_ADDRESS val1)
        {
            return BitConverter.ToUInt64(val1.rgBytes, 0);
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct BLUETOOTH_RADIO_INFO
    {
        public ulong dwSize; // ' Size, In bytes, Of this entire data Structure
        public BLUETOOTH_ADDRESS address; // ' Address Of the local radio
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BluetoothApi.BLUETOOTH_MAX_NAME_SIZE)]
        public string szName;
        public uint ulClassofDevice; // ' Class of device for the local radio
        public ushort lmpSubVersion;                    // ' lmpSubversion, manufacturer specifc.
        public BTH_MFG_INFO manufacturer;                        // ' Manufacturer Of the radio, BTH_MFG_Xxx value.  For the most up to date
        // ' list, goto the Bluetooth specification website And get the Bluetooth
        // ' assigned numbers document.
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct BLUETOOTH_DEVICE_INFO
    {
        public long dwSize;
        public BLUETOOTH_ADDRESS Address;
        public uint ulClassofDevice;
        [MarshalAs(UnmanagedType.Bool)]
        public bool fConnected;
        [MarshalAs(UnmanagedType.Bool)]
        public bool fRemembered;
        [MarshalAs(UnmanagedType.Bool)]
        public bool fAuthenticated;
        [MarshalAs(UnmanagedType.Struct)]
        public PInvoke.SYSTEMTIME stLastSeen;
        [MarshalAs(UnmanagedType.Struct)]
        public PInvoke.SYSTEMTIME stLastUsed;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BluetoothApi.BLUETOOTH_MAX_NAME_SIZE)]
        public string szName;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct BTH_MFG_INFO
    {
        private static Dictionary<ushort, string> Manufacturers = new Dictionary<ushort, string>();
        private ushort _val;

        public ushort Value
        {
            get
            {
                return _val;
            }
        }

        public string Name
        {
            get
            {
                return ToString();
            }
        }

        internal BTH_MFG_INFO(string name, ushort value)
        {
            _val = value;
            if (!Manufacturers.ContainsKey(value))
                Manufacturers.Add(value, name);
        }

        public override string ToString()
        {
            if (Manufacturers.ContainsKey(_val))
                return string.Format("{0}: {1}", _val, Manufacturers[_val]);
            else
                return _val.ToString();
        }

        public static implicit operator ushort(BTH_MFG_INFO val1)
        {
            return val1._val;
        }

        public static explicit operator BTH_MFG_INFO(ushort val1)
        {
            BTH_MFG_INFO b;
            b._val = val1;
            return b;
        }
    }
}