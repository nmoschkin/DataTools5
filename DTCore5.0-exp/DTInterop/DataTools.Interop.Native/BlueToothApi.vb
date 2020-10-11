'' ************************************************* ''
'' DataTools Visual Basic Utility Library 
''
'' Module: Bluetooth API
''         Complete Translation of
''         BluetoothAPI.h
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Imports System.Runtime.InteropServices
Imports CoreCT.Memory
Imports CoreCT.Text

Namespace Native

    Friend Module BluetoothApi
        'TODO !!!


        '' {0850302A-B344-4fda-9BE9-90576B8D46F0}
        Public ReadOnly GUID_BTHPORT_DEVICE_INTERFACE As Guid = New Guid(&H850302A, &HB344, &H4FDA, &H9B, &HE9, &H90, &H57, &H6B, &H8D, &H46, &HF0)

        '' RFCOMM device interface GUID for RFCOMM services
        '' {b142fc3e-fa4e-460b-8abc-072b628b3c70}
        Public ReadOnly GUID_BTH_RFCOMM_SERVICE_DEVICE_INTERFACE As Guid = New Guid(&HB142FC3EUL, &HFA4E, &H460B, &H8A, &HBC, &H7, &H2B, &H62, &H8B, &H3C, &H70)

        '' {EA3B5B82-26EE-450E-B0D8-D26FE30A3869}
        Public ReadOnly GUID_BLUETOOTH_RADIO_IN_RANGE As Guid = New Guid(&HEA3B5B82, &H26EE, &H450E, &HB0, &HD8, &HD2, &H6F, &HE3, &HA, &H38, &H69)

        '' {E28867C9-C2AA-4CED-B969-4570866037C4}
        Public ReadOnly GUID_BLUETOOTH_RADIO_OUT_OF_RANGE As Guid = New Guid(&HE28867C9UL, &HC2AA, &H4CED, &HB9, &H69, &H45, &H70, &H86, &H60, &H37, &HC4)

        '' {7EAE4030-B709-4AA8-AC55-E953829C9DAA}
        Public ReadOnly GUID_BLUETOOTH_L2CAP_EVENT As Guid = New Guid(&H7EAE4030, &HB709, &H4AA8, &HAC, &H55, &HE9, &H53, &H82, &H9C, &H9D, &HAA)

        '' {FC240062-1541-49BE-B463-84C4DCD7BF7F}
        Public ReadOnly GUID_BLUETOOTH_HCI_EVENT As Guid = New Guid(&HFC240062, &H1541, &H49BE, &HB4, &H63, &H84, &HC4, &HDC, &HD7, &HBF, &H7F)

        ''
        '' Support added in KB942567

        '' {5DC9136D-996C-46DB-84F5-32C0A3F47352}
        Public ReadOnly GUID_BLUETOOTH_AUTHENTICATION_REQUEST As Guid = New Guid(&H5DC9136D, &H996C, &H46DB, &H84, &HF5, &H32, &HC0, &HA3, &HF4, &H73, &H52)

        '' {D668DFCD-0F4E-4EFC-BFE0-392EEEC5109C}
        Public ReadOnly GUID_BLUETOOTH_KEYPRESS_EVENT As Guid = New Guid(&HD668DFCD, &HF4E, &H4EFC, &HBF, &HE0, &H39, &H2E, &HEE, &HC5, &H10, &H9C)

        '' {547247e6-45bb-4c33-af8c-c00efe15a71d}
        Public ReadOnly GUID_BLUETOOTH_HCI_VENDOR_EVENT As Guid = New Guid(&H547247E6, &H45BB, &H4C33, &HAF, &H8C, &HC0, &HE, &HFE, &H15, &HA7, &H1D)


        ''
        '' Bluetooth base UUID for service discovery
        ''
        Public ReadOnly Bluetooth_Base_UUID As Guid = New Guid(&H0, &H0, &H1000, &H80, &H0, &H0, &H80, &H5F, &H9B, &H34, &HFB)


        Public Function DEFINE_BLUETOOTH_UUID128(shortId As UShort) As Guid

            'public readonly DEFINE_BLUETOOTH_UUID128 As UInteger = (name,shortId) _
            '        DEFINE_GUID(name, shortId, &H0000, &H1000, &H80, &H00, &H00, &H80, &H5F, &H9B, &H34, &HFB)

            Return New Guid(shortId, &H0, &H1000, &H80, &H0, &H0, &H80, &H5F, &H9B, &H34, &HFB)

        End Function



        ''
        '' UUIDs for the Protocol Identifiers, Service Discovery Assigned Numbers
        ''
        Public Const SDP_PROTOCOL_UUID16 As UShort = (&H1)
        Public Const UDP_PROTOCOL_UUID16 As UShort = (&H2)
        Public Const RFCOMM_PROTOCOL_UUID16 As UShort = (&H3)
        Public Const TCP_PROTOCOL_UUID16 As UShort = (&H4)
        Public Const TCSBIN_PROTOCOL_UUID16 As UShort = (&H5)
        Public Const TCSAT_PROTOCOL_UUID16 As UShort = (&H6)
        Public Const ATT_PROTOCOL_UUID16 As UShort = (&H7)
        Public Const OBEX_PROTOCOL_UUID16 As UShort = (&H8)
        Public Const IP_PROTOCOL_UUID16 As UShort = (&H9)
        Public Const FTP_PROTOCOL_UUID16 As UShort = (&HA)
        Public Const HTTP_PROTOCOL_UUID16 As UShort = (&HC)
        Public Const WSP_PROTOCOL_UUID16 As UShort = (&HE)
        Public Const BNEP_PROTOCOL_UUID16 As UShort = (&HF)
        Public Const UPNP_PROTOCOL_UUID16 As UShort = (&H10)
        Public Const HID_PROTOCOL_UUID16 As UShort = (&H11)
        Public Const HCCC_PROTOCOL_UUID16 As UShort = (&H12)
        Public Const HCDC_PROTOCOL_UUID16 As UShort = (&H14)
        Public Const HCN_PROTOCOL_UUID16 As UShort = (&H16)
        Public Const AVCTP_PROTOCOL_UUID16 As UShort = (&H17)
        Public Const AVDTP_PROTOCOL_UUID16 As UShort = (&H19)
        Public Const CMPT_PROTOCOL_UUID16 As UShort = (&H1B)
        Public Const UDI_C_PLANE_PROTOCOL_UUID16 As UShort = (&H1D)
        Public Const L2CAP_PROTOCOL_UUID16 As UShort = (&H100)

        Public ReadOnly SDP_PROTOCOL_UUID As Guid = DEFINE_BLUETOOTH_UUID128(SDP_PROTOCOL_UUID16)
        Public ReadOnly UDP_PROTOCOL_UUID As Guid = DEFINE_BLUETOOTH_UUID128(UDP_PROTOCOL_UUID16)
        Public ReadOnly RFCOMM_PROTOCOL_UUID As Guid = DEFINE_BLUETOOTH_UUID128(RFCOMM_PROTOCOL_UUID16)
        Public ReadOnly TCP_PROTOCOL_UUID As Guid = DEFINE_BLUETOOTH_UUID128(TCP_PROTOCOL_UUID16)
        Public ReadOnly TCSBIN_PROTOCOL_UUID As Guid = DEFINE_BLUETOOTH_UUID128(TCSBIN_PROTOCOL_UUID16)
        Public ReadOnly TCSAT_PROTOCOL_UUID As Guid = DEFINE_BLUETOOTH_UUID128(TCSAT_PROTOCOL_UUID16)
        Public ReadOnly ATT_PROTOCOL_UUID As Guid = DEFINE_BLUETOOTH_UUID128(ATT_PROTOCOL_UUID16)
        Public ReadOnly OBEX_PROTOCOL_UUID As Guid = DEFINE_BLUETOOTH_UUID128(OBEX_PROTOCOL_UUID16)
        Public ReadOnly IP_PROTOCOL_UUID As Guid = DEFINE_BLUETOOTH_UUID128(IP_PROTOCOL_UUID16)
        Public ReadOnly FTP_PROTOCOL_UUID As Guid = DEFINE_BLUETOOTH_UUID128(FTP_PROTOCOL_UUID16)
        Public ReadOnly HTTP_PROTOCOL_UUID As Guid = DEFINE_BLUETOOTH_UUID128(HTTP_PROTOCOL_UUID16)
        Public ReadOnly WSP_PROTOCOL_UUID As Guid = DEFINE_BLUETOOTH_UUID128(WSP_PROTOCOL_UUID16)
        Public ReadOnly BNEP_PROTOCOL_UUID As Guid = DEFINE_BLUETOOTH_UUID128(BNEP_PROTOCOL_UUID16)
        Public ReadOnly UPNP_PROTOCOL_UUID As Guid = DEFINE_BLUETOOTH_UUID128(UPNP_PROTOCOL_UUID16)
        Public ReadOnly HID_PROTOCOL_UUID As Guid = DEFINE_BLUETOOTH_UUID128(HID_PROTOCOL_UUID16)
        Public ReadOnly HCCC_PROTOCOL_UUID As Guid = DEFINE_BLUETOOTH_UUID128(HCCC_PROTOCOL_UUID16)
        Public ReadOnly HCDC_PROTOCOL_UUID As Guid = DEFINE_BLUETOOTH_UUID128(HCDC_PROTOCOL_UUID16)
        Public ReadOnly HCN_PROTOCOL_UUID As Guid = DEFINE_BLUETOOTH_UUID128(HCN_PROTOCOL_UUID16)
        Public ReadOnly AVCTP_PROTOCOL_UUID As Guid = DEFINE_BLUETOOTH_UUID128(AVCTP_PROTOCOL_UUID16)
        Public ReadOnly AVDTP_PROTOCOL_UUID As Guid = DEFINE_BLUETOOTH_UUID128(AVDTP_PROTOCOL_UUID16)
        Public ReadOnly CMPT_PROTOCOL_UUID As Guid = DEFINE_BLUETOOTH_UUID128(CMPT_PROTOCOL_UUID16)
        Public ReadOnly UDI_C_PLANE_PROTOCOL_UUID As Guid = DEFINE_BLUETOOTH_UUID128(UDI_C_PLANE_PROTOCOL_UUID16)
        Public ReadOnly L2CAP_PROTOCOL_UUID As Guid = DEFINE_BLUETOOTH_UUID128(L2CAP_PROTOCOL_UUID16)

        ''
        '' UUIDs for Service Class IDs, Service Discovery Assigned Numbers
        ''
        Public Const ServiceDiscoveryServerServiceClassID_UUID16 As UShort = (&H1000)
        Public Const BrowseGroupDescriptorServiceClassID_UUID16 As UShort = (&H1001)
        Public Const PublicBrowseGroupServiceClassID_UUID16 As UShort = (&H1002)

        Public Const SerialPortServiceClassID_UUID16 As UShort = (&H1101)
        Public Const LANAccessUsingPPPServiceClassID_UUID16 As UShort = (&H1102)
        Public Const DialupNetworkingServiceClassID_UUID16 As UShort = (&H1103)
        Public Const IrMCSyncServiceClassID_UUID16 As UShort = (&H1104)
        Public Const OBEXObjectPushServiceClassID_UUID16 As UShort = (&H1105)
        Public Const OBEXFileTransferServiceClassID_UUID16 As UShort = (&H1106)
        Public Const IrMcSyncCommandServiceClassID_UUID16 As UShort = (&H1107)
        Public Const HeadsetServiceClassID_UUID16 As UShort = (&H1108)
        Public Const CordlessTelephonyServiceClassID_UUID16 As UShort = (&H1109)
        Public Const AudioSourceServiceClassID_UUID16 As UShort = (&H110A)
        Public Const AudioSinkServiceClassID_UUID16 As UShort = (&H110B)
        Public Const AVRemoteControlTargetServiceClassID_UUID16 As UShort = (&H110C)
        Public Const AVRemoteControlServiceClassID_UUID16 As UShort = (&H110E)
        Public Const AVRemoteControlControllerServiceClass_UUID16 As UShort = (&H110F)
        Public Const IntercomServiceClassID_UUID16 As UShort = (&H1110)
        Public Const FaxServiceClassID_UUID16 As UShort = (&H1111)
        Public Const HeadsetAudioGatewayServiceClassID_UUID16 As UShort = (&H1112)
        Public Const WAPServiceClassID_UUID16 As UShort = (&H1113)
        Public Const WAPClientServiceClassID_UUID16 As UShort = (&H1114)
        Public Const PANUServiceClassID_UUID16 As UShort = (&H1115)
        Public Const NAPServiceClassID_UUID16 As UShort = (&H1116)
        Public Const GNServiceClassID_UUID16 As UShort = (&H1117)
        Public Const DirectPrintingServiceClassID_UUID16 As UShort = (&H1118)
        Public Const ReferencePrintingServiceClassID_UUID16 As UShort = (&H1119)
        Public Const ImagingResponderServiceClassID_UUID16 As UShort = (&H111B)
        Public Const ImagingAutomaticArchiveServiceClassID_UUID16 As UShort = (&H111C)
        Public Const ImagingReferenceObjectsServiceClassID_UUID16 As UShort = (&H111D)
        Public Const HandsfreeServiceClassID_UUID16 As UShort = (&H111E)
        Public Const HandsfreeAudioGatewayServiceClassID_UUID16 As UShort = (&H111F)
        Public Const DirectPrintingReferenceObjectsServiceClassID_UUID16 As UShort = (&H1120)
        Public Const ReflectsUIServiceClassID_UUID16 As UShort = (&H1121)
        Public Const PrintingStatusServiceClassID_UUID16 As UShort = (&H1123)
        Public Const HumanInterfaceDeviceServiceClassID_UUID16 As UShort = (&H1124)
        Public Const HCRPrintServiceClassID_UUID16 As UShort = (&H1126)
        Public Const HCRScanServiceClassID_UUID16 As UShort = (&H1127)
        Public Const CommonISDNAccessServiceClassID_UUID16 As UShort = (&H1128)
        Public Const VideoConferencingGWServiceClassID_UUID16 As UShort = (&H1129)
        Public Const UDIMTServiceClassID_UUID16 As UShort = (&H112A)
        Public Const UDITAServiceClassID_UUID16 As UShort = (&H112B)
        Public Const AudioVideoServiceClassID_UUID16 As UShort = (&H112C)
        Public Const SimAccessServiceClassID_UUID16 As UShort = (&H112D)
        Public Const PhonebookAccessPceServiceClassID_UUID16 As UShort = (&H112E)
        Public Const PhonebookAccessPseServiceClassID_UUID16 As UShort = (&H112F)

        Public Const HeadsetHSServiceClassID_UUID16 As UShort = (&H1131)
        Public Const MessageAccessServerServiceClassID_UUID16 As UShort = (&H1132)
        Public Const MessageNotificationServerServiceClassID_UUID16 As UShort = (&H1133)

        Public Const GNSSServerServiceClassID_UUID16 As UShort = (&H1136)
        Public Const ThreeDimensionalDisplayServiceClassID_UUID16 As UShort = (&H1137)
        Public Const ThreeDimensionalGlassesServiceClassID_UUID16 As UShort = (&H1138)

        Public Const MPSServiceClassID_UUID16 As UShort = (&H113B)
        Public Const CTNAccessServiceClassID_UUID16 As UShort = (&H113C)
        Public Const CTNNotificationServiceClassID_UUID16 As UShort = (&H113D)

        Public Const PnPInformationServiceClassID_UUID16 As UShort = (&H1200)
        Public Const GenericNetworkingServiceClassID_UUID16 As UShort = (&H1201)
        Public Const GenericFileTransferServiceClassID_UUID16 As UShort = (&H1202)
        Public Const GenericAudioServiceClassID_UUID16 As UShort = (&H1203)
        Public Const GenericTelephonyServiceClassID_UUID16 As UShort = (&H1204)
        Public Const UPnpServiceClassID_UUID16 As UShort = (&H1205)
        Public Const UPnpIpServiceClassID_UUID16 As UShort = (&H1206)

        Public Const ESdpUpnpIpPanServiceClassID_UUID16 As UShort = (&H1300)
        Public Const ESdpUpnpIpLapServiceClassID_UUID16 As UShort = (&H1301)
        Public Const ESdpUpnpL2capServiceClassID_UUID16 As UShort = (&H1302)
        Public Const VideoSourceServiceClassID_UUID16 As UShort = (&H1303)
        Public Const VideoSinkServiceClassID_UUID16 As UShort = (&H1304)

        Public Const HealthDeviceProfileSourceServiceClassID_UUID16 As UShort = (&H1401)
        Public Const HealthDeviceProfileSinkServiceClassID_UUID16 As UShort = (&H1402)

        Public ReadOnly ServiceDiscoveryServerServiceClassID_UUID As Guid = DEFINE_BLUETOOTH_UUID128(ServiceDiscoveryServerServiceClassID_UUID16)
        Public ReadOnly BrowseGroupDescriptorServiceClassID_UUID As Guid = DEFINE_BLUETOOTH_UUID128(BrowseGroupDescriptorServiceClassID_UUID16)
        Public ReadOnly PublicBrowseGroupServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(PublicBrowseGroupServiceClassID_UUID16)

        Public ReadOnly SerialPortServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(SerialPortServiceClassID_UUID16)
        Public ReadOnly LANAccessUsingPPPServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(LANAccessUsingPPPServiceClassID_UUID16)
        Public ReadOnly DialupNetworkingServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(DialupNetworkingServiceClassID_UUID16)
        Public ReadOnly IrMCSyncServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(IrMCSyncServiceClassID_UUID16)
        Public ReadOnly OBEXObjectPushServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(OBEXObjectPushServiceClassID_UUID16)
        Public ReadOnly OBEXFileTransferServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(OBEXFileTransferServiceClassID_UUID16)
        Public ReadOnly IrMCSyncCommandServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(IrMcSyncCommandServiceClassID_UUID16)
        Public ReadOnly HeadsetServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(HeadsetServiceClassID_UUID16)
        Public ReadOnly CordlessTelephonyServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(CordlessTelephonyServiceClassID_UUID16)
        Public ReadOnly AudioSourceServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(AudioSourceServiceClassID_UUID16)
        Public ReadOnly AudioSinkServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(AudioSinkServiceClassID_UUID16)
        Public ReadOnly AVRemoteControlTargetServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(AVRemoteControlTargetServiceClassID_UUID16)
        Public ReadOnly AVRemoteControlServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(AVRemoteControlServiceClassID_UUID16)
        Public ReadOnly AVRemoteControlControllerServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(AVRemoteControlControllerServiceClass_UUID16)
        Public ReadOnly IntercomServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(IntercomServiceClassID_UUID16)
        Public ReadOnly FaxServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(FaxServiceClassID_UUID16)
        Public ReadOnly HeadsetAudioGatewayServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(HeadsetAudioGatewayServiceClassID_UUID16)
        Public ReadOnly WAPServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(WAPServiceClassID_UUID16)
        Public ReadOnly WAPClientServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(WAPClientServiceClassID_UUID16)
        Public ReadOnly PANUServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(PANUServiceClassID_UUID16)
        Public ReadOnly NAPServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(NAPServiceClassID_UUID16)
        Public ReadOnly GNServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(GNServiceClassID_UUID16)
        Public ReadOnly DirectPrintingServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(DirectPrintingServiceClassID_UUID16)
        Public ReadOnly ReferencePrintingServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(ReferencePrintingServiceClassID_UUID16)
        Public ReadOnly ImagingResponderServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(ImagingResponderServiceClassID_UUID16)
        Public ReadOnly ImagingAutomaticArchiveServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(ImagingAutomaticArchiveServiceClassID_UUID16)
        Public ReadOnly ImagingReferenceObjectsServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(ImagingReferenceObjectsServiceClassID_UUID16)
        Public ReadOnly HandsfreeServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(HandsfreeServiceClassID_UUID16)
        Public ReadOnly HandsfreeAudioGatewayServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(HandsfreeAudioGatewayServiceClassID_UUID16)
        Public ReadOnly DirectPrintingReferenceObjectsServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(DirectPrintingReferenceObjectsServiceClassID_UUID16)
        Public ReadOnly ReflectedUIServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(ReflectsUIServiceClassID_UUID16)
        Public ReadOnly PrintingStatusServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(PrintingStatusServiceClassID_UUID16)
        Public ReadOnly HumanInterfaceDeviceServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(HumanInterfaceDeviceServiceClassID_UUID16)
        Public ReadOnly HCRPrintServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(HCRPrintServiceClassID_UUID16)
        Public ReadOnly HCRScanServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(HCRScanServiceClassID_UUID16)
        Public ReadOnly CommonISDNAccessServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(CommonISDNAccessServiceClassID_UUID16)
        Public ReadOnly VideoConferencingGWServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(VideoConferencingGWServiceClassID_UUID16)
        Public ReadOnly UDIMTServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(UDIMTServiceClassID_UUID16)
        Public ReadOnly UDITAServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(UDITAServiceClassID_UUID16)
        Public ReadOnly AudioVideoServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(AudioVideoServiceClassID_UUID16)
        Public ReadOnly SimAccessServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(SimAccessServiceClassID_UUID16)
        Public ReadOnly PhonebookAccessPceServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(PhonebookAccessPceServiceClassID_UUID16)
        Public ReadOnly PhonebookAccessPseServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(PhonebookAccessPseServiceClassID_UUID16)

        Public ReadOnly HeadsetHSServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(HeadsetHSServiceClassID_UUID16)
        Public ReadOnly MessageAccessServerServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(MessageAccessServerServiceClassID_UUID16)
        Public ReadOnly MessageNotificationServerServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(MessageNotificationServerServiceClassID_UUID16)

        Public ReadOnly GNSSServerServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(GNSSServerServiceClassID_UUID16)
        Public ReadOnly ThreeDimensionalDisplayServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(ThreeDimensionalDisplayServiceClassID_UUID16)
        Public ReadOnly ThreeDimensionalGlassesServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(ThreeDimensionalGlassesServiceClassID_UUID16)

        Public ReadOnly MPSServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(MPSServiceClassID_UUID16)
        Public ReadOnly CTNAccessServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(CTNAccessServiceClassID_UUID16)
        Public ReadOnly CTNNotificationServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(CTNNotificationServiceClassID_UUID16)

        Public ReadOnly PnPInformationServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(PnPInformationServiceClassID_UUID16)
        Public ReadOnly GenericNetworkingServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(GenericNetworkingServiceClassID_UUID16)
        Public ReadOnly GenericFileTransferServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(GenericFileTransferServiceClassID_UUID16)
        Public ReadOnly GenericAudioServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(GenericAudioServiceClassID_UUID16)
        Public ReadOnly GenericTelephonyServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(GenericTelephonyServiceClassID_UUID16)
        Public ReadOnly UPnpServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(UPnpServiceClassID_UUID16)
        Public ReadOnly UPnpIpServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(UPnpIpServiceClassID_UUID16)

        Public ReadOnly ESdpUpnpIpPanServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(ESdpUpnpIpPanServiceClassID_UUID16)
        Public ReadOnly ESdpUpnpIpLapServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(ESdpUpnpIpLapServiceClassID_UUID16)
        Public ReadOnly ESdpUpnpL2capServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(ESdpUpnpL2capServiceClassID_UUID16)
        Public ReadOnly VideoSourceServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(VideoSourceServiceClassID_UUID16)
        Public ReadOnly VideoSinkServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(VideoSinkServiceClassID_UUID16)

        Public ReadOnly HealthDeviceProfileSourceServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(HealthDeviceProfileSourceServiceClassID_UUID16)
        Public ReadOnly HealthDeviceProfileSinkServiceClass_UUID As Guid = DEFINE_BLUETOOTH_UUID128(HealthDeviceProfileSinkServiceClassID_UUID16)

        ''
        '' UUIDs for SIG defined profiles, Service Discovery Assigned Numbers
        ''
        Public Const AdvancedAudioDistributionProfileID_UUID16 As UShort = (&H110D)
        Public Const ImagingServiceProfileID_UUID16 As UShort = (&H111A)
        Public Const BasicPrintingProfileID_UUID16 As UShort = (&H1122)
        Public Const HardcopyCableReplacementProfileID_UUID16 As UShort = (&H1125)
        Public Const PhonebookAccessProfileID_UUID16 As UShort = (&H1130)
        Public Const MessageAccessProfileID_UUID16 As UShort = (&H1134)
        Public Const GNSSProfileID_UUID16 As UShort = (&H1135)
        Public Const ThreeDimensionalSynchronizationProfileID_UUID16 As UShort = (&H1139)
        Public Const MPSProfileID_UUID16 As UShort = (&H113A)
        Public Const CTNProfileID_UUID16 As UShort = (&H113E)
        Public Const VideoDistributionProfileID_UUID16 As UShort = (&H1305)
        Public Const HealthDeviceProfileID_UUID16 As UShort = (&H1400)

        Public ReadOnly AdvancedAudioDistributionProfile_UUID As Guid = DEFINE_BLUETOOTH_UUID128(AdvancedAudioDistributionProfileID_UUID16)
        Public ReadOnly ImagingServiceProfile_UUID As Guid = DEFINE_BLUETOOTH_UUID128(ImagingServiceProfileID_UUID16)
        Public ReadOnly BasicPrintingProfile_UUID As Guid = DEFINE_BLUETOOTH_UUID128(BasicPrintingProfileID_UUID16)
        Public ReadOnly HardcopyCableReplacementProfile_UUID As Guid = DEFINE_BLUETOOTH_UUID128(HardcopyCableReplacementProfileID_UUID16)
        Public ReadOnly PhonebookAccessProfile_UUID As Guid = DEFINE_BLUETOOTH_UUID128(PhonebookAccessProfileID_UUID16)
        Public ReadOnly MessageAccessProfile_UUID As Guid = DEFINE_BLUETOOTH_UUID128(MessageAccessProfileID_UUID16)
        Public ReadOnly GNSSProfile_UUID As Guid = DEFINE_BLUETOOTH_UUID128(GNSSProfileID_UUID16)
        Public ReadOnly ThreeDimensionalSynchronizationProfile_UUID As Guid = DEFINE_BLUETOOTH_UUID128(ThreeDimensionalSynchronizationProfileID_UUID16)
        Public ReadOnly MPSProfile_UUID As Guid = DEFINE_BLUETOOTH_UUID128(MPSProfileID_UUID16)
        Public ReadOnly CTNProfile_UUID As Guid = DEFINE_BLUETOOTH_UUID128(CTNProfileID_UUID16)
        Public ReadOnly VideoDistributionProfile_UUID As Guid = DEFINE_BLUETOOTH_UUID128(VideoDistributionProfileID_UUID16)
        Public ReadOnly HealthDeviceProfile_UUID As Guid = DEFINE_BLUETOOTH_UUID128(HealthDeviceProfileID_UUID16)

        ''
        '' The SIG renamed the uuid for VideoConferencingServiceClass
        ''
        Public ReadOnly VideoConferencingServiceClass_UUID As Guid = AVRemoteControlControllerServiceClass_UUID
        Public ReadOnly VideoConferencingServiceClassID_UUID16 As UShort = AVRemoteControlControllerServiceClass_UUID16

        ''
        '' Fixing typos introduced in previous releases
        ''
        Public ReadOnly HN_PROTOCOL_UUID As Guid = HCN_PROTOCOL_UUID
        Public ReadOnly BasicPringingServiceClass_UUID As Guid = BasicPrintingProfile_UUID

        ''
        '' Fixing naming inconsistencies in UUID16 list
        ''
        Public ReadOnly CommonISDNAccessServiceClass_UUID16 As UShort = CommonISDNAccessServiceClassID_UUID16
        Public ReadOnly VideoConferencingGWServiceClass_UUID16 As UShort = VideoConferencingGWServiceClassID_UUID16
        Public ReadOnly UDIMTServiceClass_UUID16 As UShort = UDIMTServiceClassID_UUID16
        Public ReadOnly UDITAServiceClass_UUID16 As UShort = UDITAServiceClassID_UUID16
        Public ReadOnly AudioVideoServiceClass_UUID16 As UShort = AudioVideoServiceClassID_UUID16

        ''
        '' Fixing naming inconsistencies in profile list
        ''
        Public ReadOnly CordlessServiceClassID_UUID16 As UShort = CordlessTelephonyServiceClassID_UUID16
        Public ReadOnly AudioSinkSourceServiceClassID_UUID16 As UShort = AudioSinkServiceClassID_UUID16
        Public ReadOnly AdvancedAudioDistributionServiceClassID_UUID16 As UShort = AdvancedAudioDistributionProfileID_UUID16
        Public ReadOnly ImagingServiceClassID_UUID16 As UShort = ImagingServiceProfileID_UUID16
        Public ReadOnly BasicPrintingServiceClassID_UUID16 As UShort = BasicPrintingProfileID_UUID16
        Public ReadOnly HardcopyCableReplacementServiceClassID_UUID16 As UShort = HardcopyCableReplacementProfileID_UUID16

        Public ReadOnly AdvancedAudioDistributionServiceClass_UUID As Guid = AdvancedAudioDistributionProfile_UUID
        Public ReadOnly ImagingServiceClass_UUID As Guid = ImagingServiceProfile_UUID
        Public ReadOnly BasicPrintingServiceClass_UUID As Guid = BasicPrintingProfile_UUID
        Public ReadOnly HardcopyCableReplacementServiceClass_UUID As Guid = HardcopyCableReplacementProfile_UUID
        Public ReadOnly VideoDistributionServiceClass_UUID As Guid = VideoDistributionProfile_UUID


        ''
        '' max length of device friendly name.
        ''
        Public Const BTH_MAX_NAME_SIZE As UShort = (248)

        Public Const BTH_MAX_PIN_SIZE As UShort = (16)
        Public Const BTH_LINK_KEY_LENGTH As UShort = (16)


        '' Manufacturers
        Public ReadOnly BTH_MFG_ERICSSON As BTH_MFG_INFO = New BTH_MFG_INFO("Ericsson", (0))
        Public ReadOnly BTH_MFG_NOKIA As BTH_MFG_INFO = New BTH_MFG_INFO("Nokia", (1))
        Public ReadOnly BTH_MFG_INTEL As BTH_MFG_INFO = New BTH_MFG_INFO("Intel", (2))
        Public ReadOnly BTH_MFG_IBM As BTH_MFG_INFO = New BTH_MFG_INFO("IBM", (3))
        Public ReadOnly BTH_MFG_TOSHIBA As BTH_MFG_INFO = New BTH_MFG_INFO("Toshiba", (4))
        Public ReadOnly BTH_MFG_3COM As BTH_MFG_INFO = New BTH_MFG_INFO("3COM", (5))
        Public ReadOnly BTH_MFG_MICROSOFT As BTH_MFG_INFO = New BTH_MFG_INFO("Microsoft", (6))
        Public ReadOnly BTH_MFG_LUCENT As BTH_MFG_INFO = New BTH_MFG_INFO("Lucent", (7))
        Public ReadOnly BTH_MFG_MOTOROLA As BTH_MFG_INFO = New BTH_MFG_INFO("Motorola", (8))
        Public ReadOnly BTH_MFG_INFINEON As BTH_MFG_INFO = New BTH_MFG_INFO("Infineon", (9))
        Public ReadOnly BTH_MFG_CSR As BTH_MFG_INFO = New BTH_MFG_INFO("CSR", (10))
        Public ReadOnly BTH_MFG_SILICONWAVE As BTH_MFG_INFO = New BTH_MFG_INFO("SiliconWave", (11))
        Public ReadOnly BTH_MFG_DIGIANSWER As BTH_MFG_INFO = New BTH_MFG_INFO("DigiAnswer", (12))
        Public ReadOnly BTH_MFG_TI As BTH_MFG_INFO = New BTH_MFG_INFO("TI", (13))
        Public ReadOnly BTH_MFG_PARTHUS As BTH_MFG_INFO = New BTH_MFG_INFO("Parthus", (14))
        Public ReadOnly BTH_MFG_BROADCOM As BTH_MFG_INFO = New BTH_MFG_INFO("Broadcom", (15))
        Public ReadOnly BTH_MFG_MITEL As BTH_MFG_INFO = New BTH_MFG_INFO("MITEL", (16))
        Public ReadOnly BTH_MFG_WIDCOMM As BTH_MFG_INFO = New BTH_MFG_INFO("Widcomm", (17))
        Public ReadOnly BTH_MFG_ZEEVO As BTH_MFG_INFO = New BTH_MFG_INFO("Zeevo", (18))
        Public ReadOnly BTH_MFG_ATMEL As BTH_MFG_INFO = New BTH_MFG_INFO("ATMEL", (19))
        Public ReadOnly BTH_MFG_MITSIBUSHI As BTH_MFG_INFO = New BTH_MFG_INFO("Mitsibushi", (20))
        Public ReadOnly BTH_MFG_RTX_TELECOM As BTH_MFG_INFO = New BTH_MFG_INFO("RTX Telecom", (21))
        Public ReadOnly BTH_MFG_KC_TECHNOLOGY As BTH_MFG_INFO = New BTH_MFG_INFO("KC Technology", (22))
        Public ReadOnly BTH_MFG_NEWLOGIC As BTH_MFG_INFO = New BTH_MFG_INFO("NewLogic", (23))
        Public ReadOnly BTH_MFG_TRANSILICA As BTH_MFG_INFO = New BTH_MFG_INFO("Transilica", (24))
        Public ReadOnly BTH_MFG_ROHDE_SCHWARZ As BTH_MFG_INFO = New BTH_MFG_INFO("Rohde Schwarz", (25))
        Public ReadOnly BTH_MFG_TTPCOM As BTH_MFG_INFO = New BTH_MFG_INFO("TTPCOM", (26))
        Public ReadOnly BTH_MFG_SIGNIA As BTH_MFG_INFO = New BTH_MFG_INFO("Signia", (27))
        Public ReadOnly BTH_MFG_CONEXANT As BTH_MFG_INFO = New BTH_MFG_INFO("Conexant", (28))
        Public ReadOnly BTH_MFG_QUALCOMM As BTH_MFG_INFO = New BTH_MFG_INFO("Qualcomm", (29))
        Public ReadOnly BTH_MFG_INVENTEL As BTH_MFG_INFO = New BTH_MFG_INFO("INVENTEL", (30))
        Public ReadOnly BTH_MFG_AVM_BERLIN As BTH_MFG_INFO = New BTH_MFG_INFO("AVM Berlin", (31))
        Public ReadOnly BTH_MFG_BANDSPEED As BTH_MFG_INFO = New BTH_MFG_INFO("Bandspeed", (32))
        Public ReadOnly BTH_MFG_MANSELLA As BTH_MFG_INFO = New BTH_MFG_INFO("Mansella", (33))
        Public ReadOnly BTH_MFG_NEC As BTH_MFG_INFO = New BTH_MFG_INFO("NEC", (34))
        Public ReadOnly BTH_MFG_WAVEPLUS_TECHNOLOGY_CO As BTH_MFG_INFO = New BTH_MFG_INFO("WavePlus Technology Co", (35))
        Public ReadOnly BTH_MFG_ALCATEL As BTH_MFG_INFO = New BTH_MFG_INFO("ALCATEL", (36))
        Public ReadOnly BTH_MFG_PHILIPS_SEMICONDUCTOR As BTH_MFG_INFO = New BTH_MFG_INFO("Philips Semiconductor", (37))
        Public ReadOnly BTH_MFG_C_TECHNOLOGIES As BTH_MFG_INFO = New BTH_MFG_INFO("C Technologies", (38))
        Public ReadOnly BTH_MFG_OPEN_INTERFACE As BTH_MFG_INFO = New BTH_MFG_INFO("Open Interface", (39))
        Public ReadOnly BTH_MFG_RF_MICRO_DEVICES As BTH_MFG_INFO = New BTH_MFG_INFO("RF Micro Devices", (40))
        Public ReadOnly BTH_MFG_HITACHI As BTH_MFG_INFO = New BTH_MFG_INFO("Hitachi", (41))
        Public ReadOnly BTH_MFG_SYMBOL_TECHNOLOGIES As BTH_MFG_INFO = New BTH_MFG_INFO("Symbol Technologies", (42))
        Public ReadOnly BTH_MFG_TENOVIS As BTH_MFG_INFO = New BTH_MFG_INFO("Tenovis", (43))
        Public ReadOnly BTH_MFG_MACRONIX_INTERNATIONAL As BTH_MFG_INFO = New BTH_MFG_INFO("Macronix International", (44))
        Public ReadOnly BTH_MFG_MARVELL As BTH_MFG_INFO = New BTH_MFG_INFO("Marvell", (72))
        Public ReadOnly BTH_MFG_APPLE As BTH_MFG_INFO = New BTH_MFG_INFO("Apple", (76))
        Public ReadOnly BTH_MFG_NORDIC_SEMICONDUCTORS_ASA As BTH_MFG_INFO = New BTH_MFG_INFO("Nordic Semiconductors ASA", (89))
        Public ReadOnly BTH_MFG_ARUBA_NETWORKS As BTH_MFG_INFO = New BTH_MFG_INFO("Aruba Networks", (283))
        Public ReadOnly BTH_MFG_INTERNAL_USE As BTH_MFG_INFO = New BTH_MFG_INFO("INTERNAL_USE", (65535))


        '' COD


        Public Const BTH_ADDR_NULL As ULong = 0UL

        Public Const NAP_MASK As ULong = &HFFFF00000000UL
        Public Const SAP_MASK As ULong = &HFFFFFFFFUL

        Public Const NAP_BIT_OFFSET = (8 * 4)
        Public Const SAP_BIT_OFFSET As UInteger = 0


        Public Function GET_NAP(_bth_addr As ULong) As ULong
            Return (_bth_addr And NAP_MASK) >> NAP_BIT_OFFSET
        End Function

        Public Function GET_SAP(_bth_addr As ULong) As ULong
            Return (_bth_addr And NAP_MASK) >> SAP_BIT_OFFSET
        End Function

        Public Function SET_NAP(_nap As ULong) As ULong
            Return (_nap << NAP_BIT_OFFSET)
        End Function

        Public Function SET_SAP(_sap As ULong) As ULong
            Return (_sap << SAP_BIT_OFFSET)
        End Function


        Public Function SET_NAP_SAP(_nap As ULong, _sap As ULong) As ULong
            Return SET_NAP(_nap) Or SET_SAP(_sap)
        End Function

        Public Const COD_FORMAT_BIT_OFFSET As Byte = 0
        Public Const COD_MINOR_BIT_OFFSET As Byte = 2
        Public Const COD_MAJOR_BIT_OFFSET As Byte = (8 * 1)
        Public Const COD_SERVICE_BIT_OFFSET As Byte = (8 * 1 + 5)


        Public Const COD_FORMAT_MASK As UInteger = (&H3)
        Public Const COD_MINOR_MASK As UInteger = (&HFC)
        Public Const COD_MAJOR_MASK As UInteger = (&H1F00)
        Public Const COD_SERVICE_MASK As UInteger = (&HFFE000)

        Public Const COD_VERSION As UInteger = (&H0)

        Public Const COD_SERVICE_LIMITED As UInteger = (&H1)
        Public Const COD_SERVICE_POSITIONING As UInteger = (&H8)
        Public Const COD_SERVICE_NETWORKING As UInteger = (&H10)
        Public Const COD_SERVICE_RENDERING As UInteger = (&H20)
        Public Const COD_SERVICE_CAPTURING As UInteger = (&H40)
        Public Const COD_SERVICE_OBJECT_XFER As UInteger = (&H80)
        Public Const COD_SERVICE_AUDIO As UInteger = (&H100)
        Public Const COD_SERVICE_TELEPHONY As UInteger = (&H200)
        Public Const COD_SERVICE_INFORMATION As UInteger = (&H400)



        Public Function COD_SERVICE_VALID_MASK() As UInteger

            Return (COD_SERVICE_LIMITED Or
                COD_SERVICE_POSITIONING Or
                COD_SERVICE_NETWORKING Or
                COD_SERVICE_RENDERING Or
                COD_SERVICE_CAPTURING Or
                COD_SERVICE_OBJECT_XFER Or
                COD_SERVICE_AUDIO Or
                COD_SERVICE_TELEPHONY Or
                COD_SERVICE_INFORMATION)


        End Function


        Public ReadOnly COD_SERVICE_MAX_COUNT As Integer = 9

        ''
        '' Major class codes
        ''
        Public Const COD_MAJOR_MISCELLANEOUS As UInteger = (&H0)
        Public Const COD_MAJOR_COMPUTER As UInteger = (&H1)
        Public Const COD_MAJOR_PHONE As UInteger = (&H2)
        Public Const COD_MAJOR_LAN_ACCESS As UInteger = (&H3)
        Public Const COD_MAJOR_AUDIO As UInteger = (&H4)
        Public Const COD_MAJOR_PERIPHERAL As UInteger = (&H5)
        Public Const COD_MAJOR_IMAGING As UInteger = (&H6)
        Public Const COD_MAJOR_WEARABLE As UInteger = (&H7)
        Public Const COD_MAJOR_TOY As UInteger = (&H8)
        Public Const COD_MAJOR_HEALTH As UInteger = (&H9)
        Public Const COD_MAJOR_UNCLASSIFIED As UInteger = (&H1F)

        ''
        '' Minor class codes specific to each major class
        ''
        Public Const COD_COMPUTER_MINOR_UNCLASSIFIED As UInteger = (&H0)
        Public Const COD_COMPUTER_MINOR_DESKTOP As UInteger = (&H1)
        Public Const COD_COMPUTER_MINOR_SERVER As UInteger = (&H2)
        Public Const COD_COMPUTER_MINOR_LAPTOP As UInteger = (&H3)
        Public Const COD_COMPUTER_MINOR_HANDHELD As UInteger = (&H4)
        Public Const COD_COMPUTER_MINOR_PALM As UInteger = (&H5)
        Public Const COD_COMPUTER_MINOR_WEARABLE As UInteger = (&H6)

        Public Const COD_PHONE_MINOR_UNCLASSIFIED As UInteger = (&H0)
        Public Const COD_PHONE_MINOR_CELLULAR As UInteger = (&H1)
        Public Const COD_PHONE_MINOR_CORDLESS As UInteger = (&H2)
        Public Const COD_PHONE_MINOR_SMART As UInteger = (&H3)
        Public Const COD_PHONE_MINOR_WIRED_MODEM As UInteger = (&H4)

        Public Const COD_AUDIO_MINOR_UNCLASSIFIED As UInteger = (&H0)
        Public Const COD_AUDIO_MINOR_HEADSET As UInteger = (&H1)
        Public Const COD_AUDIO_MINOR_HANDS_FREE As UInteger = (&H2)
        Public Const COD_AUDIO_MINOR_HEADSET_HANDS_FREE As UInteger = (&H3)
        Public Const COD_AUDIO_MINOR_MICROPHONE As UInteger = (&H4)
        Public Const COD_AUDIO_MINOR_LOUDSPEAKER As UInteger = (&H5)
        Public Const COD_AUDIO_MINOR_HEADPHONES As UInteger = (&H6)
        Public Const COD_AUDIO_MINOR_PORTABLE_AUDIO As UInteger = (&H7)
        Public Const COD_AUDIO_MINOR_CAR_AUDIO As UInteger = (&H8)
        Public Const COD_AUDIO_MINOR_SET_TOP_BOX As UInteger = (&H9)
        Public Const COD_AUDIO_MINOR_HIFI_AUDIO As UInteger = (&HA)
        Public Const COD_AUDIO_MINOR_VCR As UInteger = (&HB)
        Public Const COD_AUDIO_MINOR_VIDEO_CAMERA As UInteger = (&HC)
        Public Const COD_AUDIO_MINOR_CAMCORDER As UInteger = (&HD)
        Public Const COD_AUDIO_MINOR_VIDEO_MONITOR As UInteger = (&HE)
        Public Const COD_AUDIO_MINOR_VIDEO_DISPLAY_LOUDSPEAKER As UInteger = (&HF)
        Public Const COD_AUDIO_MINOR_VIDEO_DISPLAY_CONFERENCING As UInteger = (&H10)
        '' Public Const COD_AUDIO_MINOR_RESERVED As UInteger = (&H11)
        Public Const COD_AUDIO_MINOR_GAMING_TOY As UInteger = (&H12)

        Public Const COD_PERIPHERAL_MINOR_KEYBOARD_MASK As UInteger = (&H10)
        Public Const COD_PERIPHERAL_MINOR_POINTER_MASK As UInteger = (&H20)

        Public Const COD_PERIPHERAL_MINOR_NO_CATEGORY As UInteger = (&H0)
        Public Const COD_PERIPHERAL_MINOR_JOYSTICK As UInteger = (&H1)
        Public Const COD_PERIPHERAL_MINOR_GAMEPAD As UInteger = (&H2)
        Public Const COD_PERIPHERAL_MINOR_REMOTE_CONTROL As UInteger = (&H3)
        Public Const COD_PERIPHERAL_MINOR_SENSING As UInteger = (&H4)

        Public Const COD_IMAGING_MINOR_DISPLAY_MASK As UInteger = (&H4)
        Public Const COD_IMAGING_MINOR_CAMERA_MASK As UInteger = (&H8)
        Public Const COD_IMAGING_MINOR_SCANNER_MASK As UInteger = (&H10)
        Public Const COD_IMAGING_MINOR_PRINTER_MASK As UInteger = (&H20)

        Public Const COD_WEARABLE_MINOR_WRIST_WATCH As UInteger = (&H1)
        Public Const COD_WEARABLE_MINOR_PAGER As UInteger = (&H2)
        Public Const COD_WEARABLE_MINOR_JACKET As UInteger = (&H3)
        Public Const COD_WEARABLE_MINOR_HELMET As UInteger = (&H4)
        Public Const COD_WEARABLE_MINOR_GLASSES As UInteger = (&H5)

        Public Const COD_TOY_MINOR_ROBOT As UInteger = (&H1)
        Public Const COD_TOY_MINOR_VEHICLE As UInteger = (&H2)
        Public Const COD_TOY_MINOR_DOLL_ACTION_FIGURE As UInteger = (&H3)
        Public Const COD_TOY_MINOR_CONTROLLER As UInteger = (&H4)
        Public Const COD_TOY_MINOR_GAME As UInteger = (&H5)

        Public Const COD_HEALTH_MINOR_BLOOD_PRESSURE_MONITOR As UInteger = (&H1)
        Public Const COD_HEALTH_MINOR_THERMOMETER As UInteger = (&H2)
        Public Const COD_HEALTH_MINOR_WEIGHING_SCALE As UInteger = (&H3)
        Public Const COD_HEALTH_MINOR_GLUCOSE_METER As UInteger = (&H4)
        Public Const COD_HEALTH_MINOR_PULSE_OXIMETER As UInteger = (&H5)
        Public Const COD_HEALTH_MINOR_HEART_PULSE_MONITOR As UInteger = (&H6)
        Public Const COD_HEALTH_MINOR_HEALTH_DATA_DISPLAY As UInteger = (&H7)
        Public Const COD_HEALTH_MINOR_STEP_COUNTER As UInteger = (&H8)

        ''
        '' Cannot use GET_COD_MINOR for this b/c it is embedded in a different manner
        '' than the rest of the major classes
        ''

        Public Const COD_LAN_ACCESS_BIT_OFFSET As Byte = 5

        Public Const COD_LAN_MINOR_MASK As UInteger = (&H1C)
        Public Const COD_LAN_ACCESS_MASK As UInteger = (&HE0)

        Public Function GET_COD_LAN_MINOR(_cod As UInteger) As UInteger
            Return (((_cod) And COD_LAN_MINOR_MASK) >> COD_MINOR_BIT_OFFSET)
        End Function

        Public Function GET_COD_LAN_ACCESS(_cod As UInteger) As UInteger
            Return (((_cod) And COD_LAN_ACCESS_MASK) >> COD_LAN_ACCESS_BIT_OFFSET)
        End Function

        ''
        '' LAN access percent usage subcodes
        ''
        Public Const COD_LAN_MINOR_UNCLASSIFIED As UInteger = (&H0)

        Public Const COD_LAN_ACCESS_0_USED As UInteger = (&H0)
        Public Const COD_LAN_ACCESS_17_USED As UInteger = (&H1)
        Public Const COD_LAN_ACCESS_33_USED As UInteger = (&H2)
        Public Const COD_LAN_ACCESS_50_USED As UInteger = (&H3)
        Public Const COD_LAN_ACCESS_67_USED As UInteger = (&H4)
        Public Const COD_LAN_ACCESS_83_USED As UInteger = (&H5)
        Public Const COD_LAN_ACCESS_99_USED As UInteger = (&H6)
        Public Const COD_LAN_ACCESS_FULL As UInteger = (&H7)

        Public Sub ParseClass(classId As UInteger, ByRef service As UShort, ByRef major As UShort, ByRef minor As UShort)


            minor = CUShort((classId And COD_MINOR_MASK) >> COD_MINOR_BIT_OFFSET)
            major = CUShort((classId And COD_MAJOR_MASK) >> COD_MAJOR_BIT_OFFSET)

            service = CUShort((classId And COD_SERVICE_MASK) >> COD_SERVICE_BIT_OFFSET)

        End Sub


        Public Function PrintMinorClass(major As UShort, minor As UShort) As String


            Dim sb As New List(Of String)

            If ((major = COD_MAJOR_COMPUTER) And (minor = COD_COMPUTER_MINOR_UNCLASSIFIED)) Then sb.Add("UNCLASSIFIED")
            If ((major = COD_MAJOR_COMPUTER) And (minor = COD_COMPUTER_MINOR_DESKTOP)) Then sb.Add("DESKTOP")
            If ((major = COD_MAJOR_COMPUTER) And (minor = COD_COMPUTER_MINOR_SERVER)) Then sb.Add("SERVER")
            If ((major = COD_MAJOR_COMPUTER) And (minor = COD_COMPUTER_MINOR_LAPTOP)) Then sb.Add("LAPTOP")
            If ((major = COD_MAJOR_COMPUTER) And (minor = COD_COMPUTER_MINOR_HANDHELD)) Then sb.Add("HANDHELD")
            If ((major = COD_MAJOR_COMPUTER) And (minor = COD_COMPUTER_MINOR_PALM)) Then sb.Add("PALM")
            If ((major = COD_MAJOR_COMPUTER) And (minor = COD_COMPUTER_MINOR_WEARABLE)) Then sb.Add("WEARABLE")

            If ((major = COD_MAJOR_PHONE) And (minor = COD_PHONE_MINOR_UNCLASSIFIED)) Then sb.Add("UNCLASSIFIED")
            If ((major = COD_MAJOR_PHONE) And (minor = COD_PHONE_MINOR_CELLULAR)) Then sb.Add("CELLULAR")
            If ((major = COD_MAJOR_PHONE) And (minor = COD_PHONE_MINOR_CORDLESS)) Then sb.Add("CORDLESS")
            If ((major = COD_MAJOR_PHONE) And (minor = COD_PHONE_MINOR_SMART)) Then sb.Add("SMART")
            If ((major = COD_MAJOR_PHONE) And (minor = COD_PHONE_MINOR_WIRED_MODEM)) Then sb.Add("WIRED_MODEM")

            If ((major = COD_MAJOR_AUDIO) And (minor = COD_AUDIO_MINOR_UNCLASSIFIED)) Then sb.Add("UNCLASSIFIED")
            If ((major = COD_MAJOR_AUDIO) And (minor = COD_AUDIO_MINOR_HEADSET)) Then sb.Add("HEADSET")
            If ((major = COD_MAJOR_AUDIO) And (minor = COD_AUDIO_MINOR_HANDS_FREE)) Then sb.Add("HANDS_FREE")
            If ((major = COD_MAJOR_AUDIO) And (minor = COD_AUDIO_MINOR_HEADSET_HANDS_FREE)) Then sb.Add("HEADSET_HANDS_FREE")
            If ((major = COD_MAJOR_AUDIO) And (minor = COD_AUDIO_MINOR_MICROPHONE)) Then sb.Add("MICROPHONE")
            If ((major = COD_MAJOR_AUDIO) And (minor = COD_AUDIO_MINOR_LOUDSPEAKER)) Then sb.Add("LOUDSPEAKER")
            If ((major = COD_MAJOR_AUDIO) And (minor = COD_AUDIO_MINOR_HEADPHONES)) Then sb.Add("HEADPHONES")
            If ((major = COD_MAJOR_AUDIO) And (minor = COD_AUDIO_MINOR_PORTABLE_AUDIO)) Then sb.Add("PORTABLE_AUDIO")
            If ((major = COD_MAJOR_AUDIO) And (minor = COD_AUDIO_MINOR_CAR_AUDIO)) Then sb.Add("CAR_AUDIO")
            If ((major = COD_MAJOR_AUDIO) And (minor = COD_AUDIO_MINOR_SET_TOP_BOX)) Then sb.Add("SET_TOP_BOX")
            If ((major = COD_MAJOR_AUDIO) And (minor = COD_AUDIO_MINOR_HIFI_AUDIO)) Then sb.Add("HIFI_AUDIO")
            If ((major = COD_MAJOR_AUDIO) And (minor = COD_AUDIO_MINOR_VCR)) Then sb.Add("VCR")
            If ((major = COD_MAJOR_AUDIO) And (minor = COD_AUDIO_MINOR_VIDEO_CAMERA)) Then sb.Add("VIDEO_CAMERA")
            If ((major = COD_MAJOR_AUDIO) And (minor = COD_AUDIO_MINOR_CAMCORDER)) Then sb.Add("CAMCORDER")
            If ((major = COD_MAJOR_AUDIO) And (minor = COD_AUDIO_MINOR_VIDEO_MONITOR)) Then sb.Add("VIDEO_MONITOR")
            If ((major = COD_MAJOR_AUDIO) And (minor = COD_AUDIO_MINOR_VIDEO_DISPLAY_LOUDSPEAKER)) Then sb.Add("VIDEO_DISPLAY_LOUDSPEAKER")
            If ((major = COD_MAJOR_AUDIO) And (minor = COD_AUDIO_MINOR_VIDEO_DISPLAY_CONFERENCING)) Then sb.Add("VIDEO_DISPLAY_CONFERENCING")
            If ((major = COD_MAJOR_AUDIO) And (minor = COD_AUDIO_MINOR_GAMING_TOY)) Then sb.Add("GAMING_TOY")

            If ((major = COD_MAJOR_PERIPHERAL) And (minor = COD_PERIPHERAL_MINOR_KEYBOARD_MASK)) Then sb.Add("KEYBOARD_MASK")
            If ((major = COD_MAJOR_PERIPHERAL) And (minor = COD_PERIPHERAL_MINOR_POINTER_MASK)) Then sb.Add("POINTER_MASK")

            If ((major = COD_MAJOR_PERIPHERAL) And (minor = COD_PERIPHERAL_MINOR_NO_CATEGORY)) Then sb.Add("NO_CATEGORY")
            If ((major = COD_MAJOR_PERIPHERAL) And (minor = COD_PERIPHERAL_MINOR_JOYSTICK)) Then sb.Add("JOYSTICK")
            If ((major = COD_MAJOR_PERIPHERAL) And (minor = COD_PERIPHERAL_MINOR_GAMEPAD)) Then sb.Add("GAMEPAD")
            If ((major = COD_MAJOR_PERIPHERAL) And (minor = COD_PERIPHERAL_MINOR_REMOTE_CONTROL)) Then sb.Add("REMOTE_CONTROL")
            If ((major = COD_MAJOR_PERIPHERAL) And (minor = COD_PERIPHERAL_MINOR_SENSING)) Then sb.Add("SENSING")

            If ((major = COD_MAJOR_IMAGING) And (minor = COD_IMAGING_MINOR_DISPLAY_MASK)) Then sb.Add("DISPLAY_MASK")
            If ((major = COD_MAJOR_IMAGING) And (minor = COD_IMAGING_MINOR_CAMERA_MASK)) Then sb.Add("CAMERA_MASK")
            If ((major = COD_MAJOR_IMAGING) And (minor = COD_IMAGING_MINOR_SCANNER_MASK)) Then sb.Add("SCANNER_MASK")
            If ((major = COD_MAJOR_IMAGING) And (minor = COD_IMAGING_MINOR_PRINTER_MASK)) Then sb.Add("PRINTER_MASK")

            If ((major = COD_MAJOR_WEARABLE) And (minor = COD_WEARABLE_MINOR_WRIST_WATCH)) Then sb.Add("WRIST_WATCH")
            If ((major = COD_MAJOR_WEARABLE) And (minor = COD_WEARABLE_MINOR_PAGER)) Then sb.Add("PAGER")
            If ((major = COD_MAJOR_WEARABLE) And (minor = COD_WEARABLE_MINOR_JACKET)) Then sb.Add("JACKET")
            If ((major = COD_MAJOR_WEARABLE) And (minor = COD_WEARABLE_MINOR_HELMET)) Then sb.Add("HELMET")
            If ((major = COD_MAJOR_WEARABLE) And (minor = COD_WEARABLE_MINOR_GLASSES)) Then sb.Add("GLASSES")

            If ((major = COD_MAJOR_TOY) And (minor = COD_TOY_MINOR_ROBOT)) Then sb.Add("ROBOT")
            If ((major = COD_MAJOR_TOY) And (minor = COD_TOY_MINOR_VEHICLE)) Then sb.Add("VEHICLE")
            If ((major = COD_MAJOR_TOY) And (minor = COD_TOY_MINOR_DOLL_ACTION_FIGURE)) Then sb.Add("DOLL_ACTION_FIGURE")
            If ((major = COD_MAJOR_TOY) And (minor = COD_TOY_MINOR_CONTROLLER)) Then sb.Add("CONTROLLER")
            If ((major = COD_MAJOR_TOY) And (minor = COD_TOY_MINOR_GAME)) Then sb.Add("GAME")

            If ((major = COD_MAJOR_HEALTH) And (minor = COD_HEALTH_MINOR_BLOOD_PRESSURE_MONITOR)) Then sb.Add("BLOOD_PRESSURE_MONITOR")
            If ((major = COD_MAJOR_HEALTH) And (minor = COD_HEALTH_MINOR_THERMOMETER)) Then sb.Add("THERMOMETER")
            If ((major = COD_MAJOR_HEALTH) And (minor = COD_HEALTH_MINOR_WEIGHING_SCALE)) Then sb.Add("WEIGHING_SCALE")
            If ((major = COD_MAJOR_HEALTH) And (minor = COD_HEALTH_MINOR_GLUCOSE_METER)) Then sb.Add("GLUCOSE_METER")
            If ((major = COD_MAJOR_HEALTH) And (minor = COD_HEALTH_MINOR_PULSE_OXIMETER)) Then sb.Add("PULSE_OXIMETER")
            If ((major = COD_MAJOR_HEALTH) And (minor = COD_HEALTH_MINOR_HEART_PULSE_MONITOR)) Then sb.Add("HEART_PULSE_MONITOR")
            If ((major = COD_MAJOR_HEALTH) And (minor = COD_HEALTH_MINOR_HEALTH_DATA_DISPLAY)) Then sb.Add("HEALTH_DATA_DISPLAY")
            If ((major = COD_MAJOR_HEALTH) And (minor = COD_HEALTH_MINOR_STEP_COUNTER)) Then sb.Add("STEP_COUNTER")

            For i = 0 To sb.Count - 1
                sb(i) = TextTools.TitleCase(sb(i))
            Next

            Return String.Join(", ", sb)

        End Function

        Public Function PrintMajorClass(major As UShort) As String

            Dim sb As New List(Of String)

            ''
            '' Major class codes
            ''
            If (major = COD_MAJOR_MISCELLANEOUS) Then sb.Add("MISCELLANEOUS")
            If (major = COD_MAJOR_COMPUTER) Then sb.Add("COMPUTER")
            If (major = COD_MAJOR_PHONE) Then sb.Add("PHONE")
            If (major = COD_MAJOR_LAN_ACCESS) Then sb.Add("LAN_ACCESS")
            If (major = COD_MAJOR_AUDIO) Then sb.Add("AUDIO")
            If (major = COD_MAJOR_PERIPHERAL) Then sb.Add("PERIPHERAL")
            If (major = COD_MAJOR_IMAGING) Then sb.Add("IMAGING")
            If (major = COD_MAJOR_WEARABLE) Then sb.Add("WEARABLE")
            If (major = COD_MAJOR_TOY) Then sb.Add("TOY")
            If (major = COD_MAJOR_HEALTH) Then sb.Add("HEALTH")
            If (major = COD_MAJOR_UNCLASSIFIED) Then sb.Add("UNCLASSIFIED")

            For i = 0 To sb.Count - 1
                sb(i) = TextTools.TitleCase(sb(i))
            Next

            Return String.Join(", ", sb)

        End Function

        Public Function PrintServiceClass(service As UShort) As String

            Dim sb As New List(Of String)

            If (service And COD_SERVICE_LIMITED) = COD_SERVICE_LIMITED Then sb.Add("LIMITED")
            If (service And COD_SERVICE_POSITIONING) = COD_SERVICE_POSITIONING Then sb.Add("POSITIONING")
            If (service And COD_SERVICE_NETWORKING) = COD_SERVICE_NETWORKING Then sb.Add("NETWORKING")
            If (service And COD_SERVICE_RENDERING) = COD_SERVICE_RENDERING Then sb.Add("RENDERING")
            If (service And COD_SERVICE_CAPTURING) = COD_SERVICE_CAPTURING Then sb.Add("CAPTURING")
            If (service And COD_SERVICE_OBJECT_XFER) = COD_SERVICE_OBJECT_XFER Then sb.Add("OBJECT_XFER")
            If (service And COD_SERVICE_AUDIO) = COD_SERVICE_AUDIO Then sb.Add("AUDIO")
            If (service And COD_SERVICE_TELEPHONY) = COD_SERVICE_TELEPHONY Then sb.Add("TELEPHONY")
            If (service And COD_SERVICE_INFORMATION) = COD_SERVICE_INFORMATION Then sb.Add("INFORMATION")

            For i = 0 To sb.Count - 1
                sb(i) = TextTools.TitleCase(sb(i))
            Next

            Return String.Join(", ", sb)

        End Function



        ''
        '' Extended Inquiry Response (EIR) defines.
        ''
        Public Const BTH_EIR_FLAGS_ID As Byte = (&H1)
        Public Const BTH_EIR_16_UUIDS_PARTIAL_ID As Byte = (&H2)
        Public Const BTH_EIR_16_UUIDS_COMPLETE_ID As Byte = (&H3)
        Public Const BTH_EIR_32_UUIDS_PARTIAL_ID As Byte = (&H4)
        Public Const BTH_EIR_32_UUIDS_COMPLETE_ID As Byte = (&H5)
        Public Const BTH_EIR_128_UUIDS_PARTIAL_ID As Byte = (&H6)
        Public Const BTH_EIR_128_UUIDS_COMPLETE_ID As Byte = (&H7)
        Public Const BTH_EIR_LOCAL_NAME_PARTIAL_ID As Byte = (&H8)
        Public Const BTH_EIR_LOCAL_NAME_COMPLETE_ID As Byte = (&H9)
        Public Const BTH_EIR_TX_POWER_LEVEL_ID As Byte = (&HA)
        Public Const BTH_EIR_OOB_OPT_DATA_LEN_ID As Byte = (&HB) '' OOB only.
        Public Const BTH_EIR_OOB_BD_ADDR_ID As Byte = (&HC) '' OOB only.
        Public Const BTH_EIR_OOB_COD_ID As Byte = (&HD) '' OOB only.
        Public Const BTH_EIR_OOB_SP_HASH_ID As Byte = (&HE) '' OOB only.
        Public Const BTH_EIR_OOB_SP_RANDOMIZER_ID As Byte = (&HF) '' OOB only.
        Public Const BTH_EIR_MANUFACTURER_ID As Byte = (&HFF)

        ''
        '' Extended Inquiry Response (EIR) size.
        ''
        Public Const BTH_EIR_SIZE As Byte = 240

        ''
        '' Used as an initializer of LAP_DATA
        ''
        Public ReadOnly LAP_GIAC_INIT As Byte() = {&H33, &H8B, &H9E}

        Public ReadOnly LAP_LIAC_INIT As Byte() = {&H0, &H8B, &H9E}

        ''
        '' General Inquiry Access Code.
        ''
        Public Const LAP_GIAC_VALUE As UInteger = (&H9E8B33)

        ''
        '' Limited Inquiry Access Code.
        ''
        Public Const LAP_LIAC_VALUE As UInteger = (&H9E8B00)

        Public Const BTH_ADDR_IAC_FIRST As UInteger = (&H9E8B00)
        Public Const BTH_ADDR_IAC_LAST As UInteger = (&H9E8B3F)
        Public Const BTH_ADDR_LIAC As UInteger = (&H9E8B00)
        Public Const BTH_ADDR_GIAC As UInteger = (&H9E8B33)

        Public Function BTH_ERROR(_btStatus As Byte) As Boolean
            Return ((_btStatus) <> BTH_ERROR_SUCCESS)
        End Function

        Public Function BTH_SUCCESS(_btStatus As Byte) As Boolean
            Return ((_btStatus) = BTH_ERROR_SUCCESS)
        End Function

        Public Const BTH_ERROR_SUCCESS As Byte = (&H0)
        Public Const BTH_ERROR_UNKNOWN_HCI_COMMAND As Byte = (&H1)
        Public Const BTH_ERROR_NO_CONNECTION As Byte = (&H2)
        Public Const BTH_ERROR_HARDWARE_FAILURE As Byte = (&H3)
        Public Const BTH_ERROR_PAGE_TIMEOUT As Byte = (&H4)
        Public Const BTH_ERROR_AUTHENTICATION_FAILURE As Byte = (&H5)
        Public Const BTH_ERROR_KEY_MISSING As Byte = (&H6)
        Public Const BTH_ERROR_MEMORY_FULL As Byte = (&H7)
        Public Const BTH_ERROR_CONNECTION_TIMEOUT As Byte = (&H8)
        Public Const BTH_ERROR_MAX_NUMBER_OF_CONNECTIONS As Byte = (&H9)
        Public Const BTH_ERROR_MAX_NUMBER_OF_SCO_CONNECTIONS As Byte = (&HA)
        Public Const BTH_ERROR_ACL_CONNECTION_ALREADY_EXISTS As Byte = (&HB)
        Public Const BTH_ERROR_COMMAND_DISALLOWED As Byte = (&HC)
        Public Const BTH_ERROR_HOST_REJECTED_LIMITED_RESOURCES As Byte = (&HD)
        Public Const BTH_ERROR_HOST_REJECTED_SECURITY_REASONS As Byte = (&HE)
        Public Const BTH_ERROR_HOST_REJECTED_PERSONAL_DEVICE As Byte = (&HF)
        Public Const BTH_ERROR_HOST_TIMEOUT As Byte = (&H10)
        Public Const BTH_ERROR_UNSUPPORTED_FEATURE_OR_PARAMETER As Byte = (&H11)
        Public Const BTH_ERROR_INVALID_HCI_PARAMETER As Byte = (&H12)
        Public Const BTH_ERROR_REMOTE_USER_ENDED_CONNECTION As Byte = (&H13)
        Public Const BTH_ERROR_REMOTE_LOW_RESOURCES As Byte = (&H14)
        Public Const BTH_ERROR_REMOTE_POWERING_OFF As Byte = (&H15)
        Public Const BTH_ERROR_LOCAL_HOST_TERMINATED_CONNECTION As Byte = (&H16)
        Public Const BTH_ERROR_REPEATED_ATTEMPTS As Byte = (&H17)
        Public Const BTH_ERROR_PAIRING_NOT_ALLOWED As Byte = (&H18)
        Public Const BTH_ERROR_UKNOWN_LMP_PDU As Byte = (&H19)
        Public Const BTH_ERROR_UNSUPPORTED_REMOTE_FEATURE As Byte = (&H1A)
        Public Const BTH_ERROR_SCO_OFFSET_REJECTED As Byte = (&H1B)
        Public Const BTH_ERROR_SCO_INTERVAL_REJECTED As Byte = (&H1C)
        Public Const BTH_ERROR_SCO_AIRMODE_REJECTED As Byte = (&H1D)
        Public Const BTH_ERROR_INVALID_LMP_PARAMETERS As Byte = (&H1E)
        Public Const BTH_ERROR_UNSPECIFIED_ERROR As Byte = (&H1F)
        Public Const BTH_ERROR_UNSUPPORTED_LMP_PARM_VALUE As Byte = (&H20)
        Public Const BTH_ERROR_ROLE_CHANGE_NOT_ALLOWED As Byte = (&H21)
        Public Const BTH_ERROR_LMP_RESPONSE_TIMEOUT As Byte = (&H22)
        Public Const BTH_ERROR_LMP_TRANSACTION_COLLISION As Byte = (&H23)
        Public Const BTH_ERROR_LMP_PDU_NOT_ALLOWED As Byte = (&H24)
        Public Const BTH_ERROR_ENCRYPTION_MODE_NOT_ACCEPTABLE As Byte = (&H25)
        Public Const BTH_ERROR_UNIT_KEY_NOT_USED As Byte = (&H26)
        Public Const BTH_ERROR_QOS_IS_NOT_SUPPORTED As Byte = (&H27)
        Public Const BTH_ERROR_INSTANT_PASSED As Byte = (&H28)
        Public Const BTH_ERROR_PAIRING_WITH_UNIT_KEY_NOT_SUPPORTED As Byte = (&H29)
        Public Const BTH_ERROR_DIFFERENT_TRANSACTION_COLLISION As Byte = (&H2A)
        Public Const BTH_ERROR_QOS_UNACCEPTABLE_PARAMETER As Byte = (&H2C)
        Public Const BTH_ERROR_QOS_REJECTED As Byte = (&H2D)
        Public Const BTH_ERROR_CHANNEL_CLASSIFICATION_NOT_SUPPORTED As Byte = (&H2E)
        Public Const BTH_ERROR_INSUFFICIENT_SECURITY As Byte = (&H2F)
        Public Const BTH_ERROR_PARAMETER_OUT_OF_MANDATORY_RANGE As Byte = (&H30)
        Public Const BTH_ERROR_ROLE_SWITCH_PENDING As Byte = (&H32)
        Public Const BTH_ERROR_RESERVED_SLOT_VIOLATION As Byte = (&H34)
        Public Const BTH_ERROR_ROLE_SWITCH_FAILED As Byte = (&H35)
        Public Const BTH_ERROR_EXTENDED_INQUIRY_RESPONSE_TOO_LARGE As Byte = (&H36)
        Public Const BTH_ERROR_SECURE_SIMPLE_PAIRING_NOT_SUPPORTED_BY_HOST As Byte = (&H37)
        Public Const BTH_ERROR_HOST_BUSY_PAIRING As Byte = (&H38)
        Public Const BTH_ERROR_CONNECTION_REJECTED_DUE_TO_NO_SUITABLE_CHANNEL_FOUND As Byte = (&H39)
        Public Const BTH_ERROR_CONTROLLER_BUSY As Byte = (&H3A)
        Public Const BTH_ERROR_UNACCEPTABLE_CONNECTION_INTERVAL As Byte = (&H3B)
        Public Const BTH_ERROR_DIRECTED_ADVERTISING_TIMEOUT As Byte = (&H3C)
        Public Const BTH_ERROR_CONNECTION_TERMINATED_DUE_TO_MIC_FAILURE As Byte = (&H3D)
        Public Const BTH_ERROR_CONNECTION_FAILED_TO_BE_ESTABLISHED As Byte = (&H3E)
        Public Const BTH_ERROR_MAC_CONNECTION_FAILED As Byte = (&H3F)

        Public Const BTH_ERROR_UNSPECIFIED As Byte = (&HFF)

        ''
        '' Min, max, and default L2cap MTU.
        ''
        Public Const L2CAP_MIN_MTU As UInteger = 48
        Public Const L2CAP_MAX_MTU As UInteger = (&HFFFF)
        Public Const L2CAP_DEFAULT_MTU As UInteger = 672

        ''
        '' Max l2cap signal size (48) - size of signal header (4)
        ''
        Public Const MAX_L2CAP_PING_DATA_LENGTH As UInteger = 44
        Public Const MAX_L2CAP_INFO_DATA_LENGTH As UInteger = 44

        ''
        '' the following structures provide information about
        '' discovered remote radios.
        ''

        Public Const BDIF_ADDRESS As UInteger = (&H1)
        Public Const BDIF_COD As UInteger = (&H2)
        Public Const BDIF_NAME As UInteger = (&H4)
        Public Const BDIF_PAIRED As UInteger = (&H8)
        Public Const BDIF_PERSONAL As UInteger = (&H10)
        Public Const BDIF_CONNECTED As UInteger = (&H20)

        ''
        '' Support added in KB942567
        ''
        ' (NTDDI_VERSION > NTDDI_VISTASP1 || _

        Public Const BDIF_SHORT_NAME As UInteger = (&H40)
        Public Const BDIF_VISIBLE As UInteger = (&H80)
        Public Const BDIF_SSP_SUPPORTED As UInteger = (&H100)
        Public Const BDIF_SSP_PAIRED As UInteger = (&H200)
        Public Const BDIF_SSP_MITM_PROTECTED As UInteger = (&H400)
        Public Const BDIF_RSSI As UInteger = (&H1000)
        Public Const BDIF_EIR As UInteger = (&H2000)

        ' (NTDDI_VERSION >= NTDDI_WIN8) '' >= WIN8

        Public Const BDIF_BR As UInteger = (&H4000)
        Public Const BDIF_LE As UInteger = (&H8000)
        Public Const BDIF_LE_PAIRED As UInteger = (&H10000)
        Public Const BDIF_LE_PERSONAL As UInteger = (&H20000)
        Public Const BDIF_LE_MITM_PROTECTED As UInteger = (&H40000)
        Public Const BDIF_LE_PRIVACY_ENABLED As UInteger = (&H80000)
        Public Const BDIF_LE_RANDOM_ADDRESS_TYPE As UInteger = (&H100000)

        ' (NTDDI_VERSION >= NTDDI_WIN10) '' >= WIN10

        Public Const BDIF_LE_DISCOVERABLE As UInteger = (&H200000)
        Public Const BDIF_LE_NAME As UInteger = (&H400000)
        Public Const BDIF_LE_VISIBLE As UInteger = (&H800000)

        ' (NTDDI_VERSION >= NTDDI_WIN10_RS2) '' >= WIN10_RS2

        Public Const BDIF_LE_CONNECTED As UInteger = (&H1000000)
        Public Const BDIF_LE_CONNECTABLE As UInteger = (&H2000000)
        Public Const BDIF_CONNECTION_INBOUND As UInteger = (&H4000000)
        Public Const BDIF_BR_SECURE_CONNECTION_PAIRED As UInteger = (&H8000000)
        Public Const BDIF_LE_SECURE_CONNECTION_PAIRED As UInteger = (&H10000000)

        Public Const BDIF_DEBUGKEY As UInteger = (&H20000000)
        Public Const BDIF_LE_DEBUGKEY As UInteger = (&H40000000)

        Public Function BDIF_VALID_FLAGS() As UInteger
            Return (BDIF_ADDRESS Or BDIF_COD Or BDIF_NAME Or BDIF_PAIRED Or BDIF_PERSONAL Or
         BDIF_CONNECTED Or BDIF_SHORT_NAME Or BDIF_VISIBLE Or BDIF_RSSI Or BDIF_EIR Or BDIF_SSP_PAIRED Or BDIF_SSP_MITM_PROTECTED Or
         BDIF_BR Or BDIF_LE Or BDIF_LE_PAIRED Or BDIF_LE_PERSONAL Or BDIF_LE_MITM_PROTECTED Or BDIF_LE_PRIVACY_ENABLED Or BDIF_LE_RANDOM_ADDRESS_TYPE Or
         BDIF_LE_DISCOVERABLE Or BDIF_LE_NAME Or BDIF_LE_VISIBLE Or BDIF_LE_CONNECTED Or BDIF_LE_CONNECTABLE Or BDIF_CONNECTION_INBOUND Or
         BDIF_BR_SECURE_CONNECTION_PAIRED Or BDIF_LE_SECURE_CONNECTION_PAIRED Or BDIF_DEBUGKEY Or BDIF_LE_DEBUGKEY)
        End Function

        ' '' >= SP1+KB942567

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BTH_ADDR
            Public Address As ULong


            Public Shared Widening Operator CType(var1 As ULong) As BTH_ADDR
                Return New BTH_ADDR(var1)
            End Operator

            Public Shared Widening Operator CType(var1 As BTH_ADDR) As ULong
                Return var1.Address
            End Operator

            Public Sub New(addr As ULong)
                Address = addr
            End Sub

            Public Overrides Function ToString() As String
                Return Address.ToString()
            End Function

            Public Overloads Function ToString(format As String) As String
                Return Address.ToString(format)
            End Function

            Public Overloads Function ToString(formatProvider As IFormatProvider) As String
                Return Address.ToString(formatProvider)
            End Function

            Public Overloads Function ToString(format As String, formatProvider As IFormatProvider) As String
                Return Address.ToString(format, formatProvider)
            End Function


        End Structure



        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BTH_COD
            Public Value As ULong

            Public Shared Widening Operator CType(var1 As ULong) As BTH_COD
                Return New BTH_COD(var1)
            End Operator

            Public Shared Widening Operator CType(var1 As BTH_COD) As ULong
                Return var1.Value
            End Operator

            Public Sub New(cod As ULong)
                Me.Value = cod
            End Sub

            Public Overrides Function ToString() As String
                Return Me.Value.ToString()
            End Function

            Public Overloads Function ToString(format As String) As String
                Return Me.Value.ToString(format)
            End Function

            Public Overloads Function ToString(formatProvider As IFormatProvider) As String
                Return Me.Value.ToString(formatProvider)
            End Function

            Public Overloads Function ToString(format As String, formatProvider As IFormatProvider) As String
                Return Me.Value.ToString(format, formatProvider)
            End Function

        End Structure


        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BTH_DEVICE_INFO
            ''
            '' Combination BDIF_Xxx flags
            ''
            Public flags As UInteger

            ''
            '' Address of remote device.
            ''
            Public address As BTH_ADDR

            ''
            '' Class Of Device.
            ''
            Public classOfDevice As BTH_COD

            ''
            '' name of the device
            ''
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=BTH_MAX_NAME_SIZE)>
            Public name As String

        End Structure

        ''
        '' Buffer associated with GUID_BLUETOOTH_RADIO_IN_RANGE
        ''

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BTH_RADIO_IN_RANGE
            ''
            '' Information about the remote radio
            ''
            Public deviceInfo As BTH_DEVICE_INFO

            ''
            '' The previous flags value for the BTH_DEVICE_INFO.  The receiver of this
            '' notification can compare the deviceInfo.flags and previousDeviceFlags
            '' to determine what has changed about this remote radio.
            ''
            '' For instance, if BDIF_NAME is set in deviceInfo.flags and not in
            '' previousDeviceFlags, the remote radio's has just been retrieved.
            ''
            Public previousDeviceFlags As UInteger

        End Structure

        ''
        '' Buffer associated with GUID_BLUETOOTH_L2CAP_EVENT
        ''
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BTH_L2CAP_EVENT_INFO
            ''
            '' Remote radio address which the L2CAP event is associated with
            ''
            Public bthAddress As BTH_ADDR

            ''
            '' The PSM that is either being connected to or disconnected from
            ''
            Public psm As UShort

            ''
            '' If != 0, then the channel has just been established.  If = 0, then the
            '' channel has been destroyed.  Notifications for a destroyed channel will
            '' only be sent for channels successfully established.
            ''
            Public connected As Byte

            ''
            '' If != 0, then the local host iniated the l2cap connection.  If = 0, then
            '' the remote host initated the connection.  This field is only valid if
            '' connect is != 0.
            ''
            Public initiated As Byte

        End Structure

        Public ReadOnly HCI_CONNECTION_TYPE_ACL As UInteger = 1
        Public ReadOnly HCI_CONNECTION_TYPE_SCO As UInteger = 2
        Public ReadOnly HCI_CONNECTION_TYPE_LE As UInteger = 3

        ''
        '' Fix typos
        ''
        Public ReadOnly HCI_CONNNECTION_TYPE_ACL As UInteger = HCI_CONNECTION_TYPE_ACL
        Public ReadOnly HCI_CONNNECTION_TYPE_SCO As UInteger = HCI_CONNECTION_TYPE_SCO



        ''
        '' Buffer associated with GUID_BLUETOOTH_HCI_EVENT
        ''
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BTH_HCI_EVENT_INFO
            ''
            '' Remote radio address which the HCI event is associated with
            ''
            Public bthAddress As BTH_ADDR

            ''
            '' HCI_CONNNECTION_TYPE_XXX value
            ''
            Public connectionType As Byte

            ''
            '' If != 0, then the underlying connection to the remote radio has just
            '' been estrablished.  If = 0, then the underlying conneciton has just been
            '' destroyed.
            ''
            Public connected As Byte

        End Structure

        Public Enum IO_CAPABILITY
            IoCaps_DisplayOnly = &H0
            IoCaps_DisplayYesNo = &H1
            IoCaps_KeyboardOnly = &H2
            IoCaps_NoInputNoOutput = &H3
            IoCaps_Undefined = &HFF
        End Enum

        Public Enum AUTHENTICATION_REQUIREMENTS
            MITMProtectionNotRequired = &H0
            MITMProtectionRequired = &H1
            MITMProtectionNotRequiredBonding = &H2
            MITMProtectionRequiredBonding = &H3
            MITMProtectionNotRequiredGeneralBonding = &H4
            MITMProtectionRequiredGeneralBonding = &H5
            MITMProtectionNotDefined = &HFF
        End Enum

        Public Function IsMITMProtectionRequired(requirements As AUTHENTICATION_REQUIREMENTS) As Boolean
            Return ((AUTHENTICATION_REQUIREMENTS.MITMProtectionRequired = requirements) Or (AUTHENTICATION_REQUIREMENTS.MITMProtectionRequiredBonding = requirements) Or (AUTHENTICATION_REQUIREMENTS.MITMProtectionRequiredGeneralBonding = requirements))
        End Function

        ' '' >= SP1+KB942567

        ''
        '' Max length we allow for ServiceName in the remote SDP records
        ''
        Public Const BTH_MAX_SERVICE_NAME_SIZE As UInteger = 256

        Public Const MAX_UUIDS_IN_QUERY As UInteger = 12

        Public Const BTH_VID_DEFAULT_VALUE As UInteger = (&HFFFF)

        Public Const SDP_ERROR_INVALID_SDP_VERSION As UInteger = (&H1)
        Public Const SDP_ERROR_INVALID_RECORD_HANDLE As UInteger = (&H2)
        Public Const SDP_ERROR_INVALID_REQUEST_SYNTAX As UInteger = (&H3)
        Public Const SDP_ERROR_INVALID_PDU_SIZE As UInteger = (&H4)
        Public Const SDP_ERROR_INVALID_CONTINUATION_STATE As UInteger = (&H5)
        Public Const SDP_ERROR_INSUFFICIENT_RESOURCES As UInteger = (&H6)

        ''
        '' Defined by windows to handle server errors that are not described by the
        '' above errors.  Start at &H0100 so we don't go anywhere near the spec
        '' defined values.
        ''

        ''
        '' Success, nothing went wrong
        ''
        Public Const SDP_ERROR_SUCCESS As UInteger = (&H0)

        ''
        '' The SDP PDU or parameters other than the SDP stream response was not correct
        ''
        Public Const SDP_ERROR_SERVER_INVALID_RESPONSE As UInteger = (&H100)

        ''
        '' The SDP response stream did not parse correctly.
        ''
        Public Const SDP_ERROR_SERVER_RESPONSE_DID_NOT_PARSE As UInteger = (&H200)

        ''
        '' The SDP response stream was successfully parsed, but did not match the
        '' required format for the query.
        ''
        Public Const SDP_ERROR_SERVER_BAD_FORMAT As UInteger = (&H300)

        ''
        '' SDP was unable to send a continued query back to the server
        ''
        Public Const SDP_ERROR_COULD_NOT_SEND_CONTINUE As UInteger = (&H400)

        ''
        '' Server sent a response that was too large to fit in the caller's buffer.
        ''
        Public Const SDP_ERROR_RESPONSE_TOO_LARGE As UInteger = (&H500)

        Public ReadOnly SDP_ATTRIB_RECORD_HANDLE As UInteger = (&H0)
        Public ReadOnly SDP_ATTRIB_CLASS_ID_LIST As UInteger = (&H1)
        Public ReadOnly SDP_ATTRIB_RECORD_STATE As UInteger = (&H2)
        Public ReadOnly SDP_ATTRIB_SERVICE_ID As UInteger = (&H3)
        Public ReadOnly SDP_ATTRIB_PROTOCOL_DESCRIPTOR_LIST As UInteger = (&H4)
        Public ReadOnly SDP_ATTRIB_BROWSE_GROUP_LIST As UInteger = (&H5)
        Public ReadOnly SDP_ATTRIB_LANG_BASE_ATTRIB_ID_LIST As UInteger = (&H6)
        Public ReadOnly SDP_ATTRIB_INFO_TIME_TO_LIVE As UInteger = (&H7)
        Public ReadOnly SDP_ATTRIB_AVAILABILITY As UInteger = (&H8)
        Public ReadOnly SDP_ATTRIB_PROFILE_DESCRIPTOR_LIST As UInteger = (&H9)
        Public ReadOnly SDP_ATTRIB_DOCUMENTATION_URL As UInteger = (&HA)
        Public ReadOnly SDP_ATTRIB_CLIENT_EXECUTABLE_URL As UInteger = (&HB)
        Public ReadOnly SDP_ATTRIB_ICON_URL As UInteger = (&HC)
        Public Const SDP_ATTRIB_ADDITIONAL_PROTOCOL_DESCRIPTOR_LIST = (&HD)

        ''
        '' Attribute IDs in the range of &H000D - &H01FF are reserved for future use
        ''
        Public ReadOnly SDP_ATTRIB_PROFILE_SPECIFIC As UInteger = (&H200)

        Public ReadOnly LANG_BASE_LANGUAGE_INDEX As UInteger = (&H0)
        Public ReadOnly LANG_BASE_ENCODING_INDEX As UInteger = (&H1)
        Public ReadOnly LANG_BASE_OFFSET_INDEX As UInteger = (&H2)
        Public ReadOnly LANG_DEFAULT_ID As UInteger = (&H100)

        Public ReadOnly LANGUAGE_EN_US As UInteger = (&H656E)
        Public ReadOnly ENCODING_UTF_8 As UInteger = (&H6A)

        Public ReadOnly STRING_NAME_OFFSET As UInteger = (&H0)
        Public ReadOnly STRING_DESCRIPTION_OFFSET As UInteger = (&H1)
        Public ReadOnly STRING_PROVIDER_NAME_OFFSET As UInteger = (&H2)

        Public ReadOnly SDP_ATTRIB_SDP_VERSION_NUMBER_LIST As UInteger = (&H200)
        Public ReadOnly SDP_ATTRIB_SDP_DATABASE_STATE As UInteger = (&H201)

        Public ReadOnly SDP_ATTRIB_BROWSE_GROUP_ID As UInteger = (&H200)

        Public ReadOnly SDP_ATTRIB_CORDLESS_EXTERNAL_NETWORK As UInteger = (&H301)

        Public ReadOnly SDP_ATTRIB_FAX_CLASS_1_SUPPORT As UInteger = (&H302)
        Public ReadOnly SDP_ATTRIB_FAX_CLASS_2_0_SUPPORT As UInteger = (&H303)
        Public ReadOnly SDP_ATTRIB_FAX_CLASS_2_SUPPORT As UInteger = (&H304)
        Public ReadOnly SDP_ATTRIB_FAX_AUDIO_FEEDBACK_SUPPORT As UInteger = (&H305)

        Public ReadOnly SDP_ATTRIB_HEADSET_REMOTE_AUDIO_VOLUME_CONTROL As UInteger = (&H302)

        Public ReadOnly SDP_ATTRIB_LAN_LPSUBNET As UInteger = (&H200)

        Public ReadOnly SDP_ATTRIB_OBJECT_PUSH_SUPPORTED_FORMATS_LIST As UInteger = (&H303)

        Public ReadOnly SDP_ATTRIB_SYNCH_SUPPORTED_DATA_STORES_LIST As UInteger = (&H301)

        ''  this is in the assigned numbers doc, but it does not show up in any profile
        Public ReadOnly SDP_ATTRIB_SERVICE_VERSION As UInteger = (&H300)

        Public ReadOnly SDP_ATTRIB_PAN_NETWORK_ADDRESS As UInteger = (&H306)
        Public ReadOnly SDP_ATTRIB_PAN_WAP_GATEWAY As UInteger = (&H307)
        Public ReadOnly SDP_ATTRIB_PAN_HOME_PAGE_URL As UInteger = (&H308)
        Public ReadOnly SDP_ATTRIB_PAN_WAP_STACK_TYPE As UInteger = (&H309)
        Public ReadOnly SDP_ATTRIB_PAN_SECURITY_DESCRIPTION As UInteger = (&H30A)
        Public ReadOnly SDP_ATTRIB_PAN_NET_ACCESS_TYPE As UInteger = (&H30B)
        Public ReadOnly SDP_ATTRIB_PAN_MAX_NET_ACCESS_RATE As UInteger = (&H30C)

        Public ReadOnly SDP_ATTRIB_IMAGING_SUPPORTED_CAPABILITIES As UInteger = (&H310)
        Public ReadOnly SDP_ATTRIB_IMAGING_SUPPORTED_FEATURES As UInteger = (&H311)
        Public ReadOnly SDP_ATTRIB_IMAGING_SUPPORTED_FUNCTIONS As UInteger = (&H312)
        Public ReadOnly SDP_ATTRIB_IMAGING_TOTAL_DATA_CAPACITY As UInteger = (&H313)

        Public ReadOnly SDP_ATTRIB_DI_SPECIFICATION_ID As UInteger = (&H200)
        Public ReadOnly SDP_ATTRIB_DI_VENDOR_ID As UInteger = (&H201)
        Public ReadOnly SDP_ATTRIB_DI_PRODUCT_ID As UInteger = (&H202)
        Public ReadOnly SDP_ATTRIB_DI_VERSION As UInteger = (&H203)
        Public ReadOnly SDP_ATTRIB_DI_PRIMARY_RECORD As UInteger = (&H204)
        Public ReadOnly SDP_ATTRIB_DI_VENDOR_ID_SOURCE As UInteger = (&H205)

        Public ReadOnly SDP_ATTRIB_HID_DEVICE_RELEASE_NUMBER As UInteger = (&H200)
        Public ReadOnly SDP_ATTRIB_HID_PARSER_VERSION As UInteger = (&H201)
        Public ReadOnly SDP_ATTRIB_HID_DEVICE_SUBCLASS As UInteger = (&H202)
        Public ReadOnly SDP_ATTRIB_HID_COUNTRY_CODE As UInteger = (&H203)
        Public ReadOnly SDP_ATTRIB_HID_VIRTUAL_CABLE As UInteger = (&H204)
        Public ReadOnly SDP_ATTRIB_HID_RECONNECT_INITIATE As UInteger = (&H205)
        Public ReadOnly SDP_ATTRIB_HID_DESCRIPTOR_LIST As UInteger = (&H206)
        Public ReadOnly SDP_ATTRIB_HID_LANG_ID_BASE_LIST As UInteger = (&H207)
        Public ReadOnly SDP_ATTRIB_HID_SDP_DISABLE As UInteger = (&H208)
        Public ReadOnly SDP_ATTRIB_HID_BATTERY_POWER As UInteger = (&H209)
        Public ReadOnly SDP_ATTRIB_HID_REMOTE_WAKE As UInteger = (&H20A)
        Public ReadOnly SDP_ATTRIB_HID_PROFILE_VERSION As UInteger = (&H20B)
        Public ReadOnly SDP_ATTRIB_HID_SUPERVISION_TIMEOUT As UInteger = (&H20C)
        Public ReadOnly SDP_ATTRIB_HID_NORMALLY_CONNECTABLE As UInteger = (&H20D)
        Public ReadOnly SDP_ATTRIB_HID_BOOT_DEVICE As UInteger = (&H20E)
        Public ReadOnly SDP_ATTRIB_HID_SSR_HOST_MAX_LATENCY As UInteger = (&H20F)
        Public ReadOnly SDP_ATTRIB_HID_SSR_HOST_MIN_TIMEOUT As UInteger = (&H210)

        Public ReadOnly SDP_ATTRIB_A2DP_SUPPORTED_FEATURES As UInteger = (&H311)

        Public ReadOnly SDP_ATTRIB_AVRCP_SUPPORTED_FEATURES As UInteger = (&H311)

        Public ReadOnly SDP_ATTRIB_HFP_SUPPORTED_FEATURES As UInteger = (&H311)

        ''
        '' Profile specific values
        ''
        Public ReadOnly AVRCP_SUPPORTED_FEATURES_CATEGORY_1 As UInteger = (&H1)
        Public ReadOnly AVRCP_SUPPORTED_FEATURES_CATEGORY_2 As UInteger = (&H2)
        Public ReadOnly AVRCP_SUPPORTED_FEATURES_CATEGORY_3 As UInteger = (&H4)
        Public ReadOnly AVRCP_SUPPORTED_FEATURES_CATEGORY_4 As UInteger = (&H8)
        Public ReadOnly AVRCP_SUPPORTED_FEATURES_CT_BROWSING As UInteger = (&H40)
        Public ReadOnly AVRCP_SUPPORTED_FEATURES_CT_COVER_ART_IMAGE_PROPERTIES As UInteger = (&H80)
        Public ReadOnly AVRCP_SUPPORTED_FEATURES_CT_COVER_ART_IMAGE As UInteger = (&H100)
        Public ReadOnly AVRCP_SUPPORTED_FEATURES_CT_COVER_ART_LINKED_THUMBNAIL As UInteger = (&H200)
        Public ReadOnly AVRCP_SUPPORTED_FEATURES_TG_PLAYER_APPLICATION_SETTINGS As UInteger = (&H10)
        Public ReadOnly AVRCP_SUPPORTED_FEATURES_TG_GROUP_NAVIGATION As UInteger = (&H20)
        Public ReadOnly AVRCP_SUPPORTED_FEATURES_TG_BROWSING As UInteger = (&H40)
        Public ReadOnly AVRCP_SUPPORTED_FEATURES_TG_MULTIPLE_PLAYER_APPLICATIONS As UInteger = (&H80)
        Public ReadOnly AVRCP_SUPPORTED_FEATURES_TG_COVER_ART As UInteger = (&H100)

        Public ReadOnly CORDLESS_EXTERNAL_NETWORK_PSTN As UInteger = (&H1)
        Public ReadOnly CORDLESS_EXTERNAL_NETWORK_ISDN As UInteger = (&H2)
        Public ReadOnly CORDLESS_EXTERNAL_NETWORK_GSM As UInteger = (&H3)
        Public ReadOnly CORDLESS_EXTERNAL_NETWORK_CDMA As UInteger = (&H4)
        Public ReadOnly CORDLESS_EXTERNAL_NETWORK_ANALOG_CELLULAR As UInteger = (&H5)
        Public ReadOnly CORDLESS_EXTERNAL_NETWORK_PACKET_SWITCHED As UInteger = (&H6)
        Public ReadOnly CORDLESS_EXTERNAL_NETWORK_OTHER As UInteger = (&H7)

        Public ReadOnly OBJECT_PUSH_FORMAT_VCARD_2_1 As UInteger = (&H1)
        Public ReadOnly OBJECT_PUSH_FORMAT_VCARD_3_0 As UInteger = (&H2)
        Public ReadOnly OBJECT_PUSH_FORMAT_VCAL_1_0 As UInteger = (&H3)
        Public ReadOnly OBJECT_PUSH_FORMAT_ICAL_2_0 As UInteger = (&H4)
        Public ReadOnly OBJECT_PUSH_FORMAT_VNOTE As UInteger = (&H5)
        Public ReadOnly OBJECT_PUSH_FORMAT_VMESSAGE As UInteger = (&H6)
        Public ReadOnly OBJECT_PUSH_FORMAT_ANY As UInteger = (&HFF)

        Public ReadOnly SYNCH_DATA_STORE_PHONEBOOK As UInteger = (&H1)
        Public ReadOnly SYNCH_DATA_STORE_CALENDAR As UInteger = (&H3)
        Public ReadOnly SYNCH_DATA_STORE_NOTES As UInteger = (&H5)
        Public ReadOnly SYNCH_DATA_STORE_MESSAGES As UInteger = (&H6)

        Public ReadOnly DI_VENDOR_ID_SOURCE_BLUETOOTH_SIG As UInteger = (&H1)
        Public ReadOnly DI_VENDOR_ID_SOURCE_USB_IF As UInteger = (&H2)

        Public ReadOnly PSM_SDP As UInteger = (&H1)
        Public ReadOnly PSM_RFCOMM As UInteger = (&H3)
        Public ReadOnly PSM_TCS_BIN As UInteger = (&H5)
        Public ReadOnly PSM_TCS_BIN_CORDLESS As UInteger = (&H7)
        Public ReadOnly PSM_BNEP As UInteger = (&HF)
        Public ReadOnly PSM_HID_CONTROL As UInteger = (&H11)
        Public ReadOnly PSM_HID_INTERRUPT As UInteger = (&H13)
        Public ReadOnly PSM_UPNP As UInteger = (&H15)
        Public ReadOnly PSM_AVCTP As UInteger = (&H17)
        Public ReadOnly PSM_AVDTP As UInteger = (&H19)
        Public ReadOnly PSM_AVCTP_BROWSE As UInteger = (&H1B)
        Public ReadOnly PSM_UDI_C_PLANE As UInteger = (&H1D)
        Public ReadOnly PSM_ATT As UInteger = (&H1F)
        Public ReadOnly PSM_3DSP As UInteger = (&H21)
        Public ReadOnly PSM_LE_IPSP As UInteger = (&H23)

        ''
        '' Strings
        ''
        Public Const STR_ADDR_FMTW As String = "(%02x:%02x:%02x:%02x:%02x:%02x)"

        Public Const STR_ADDR_SHORT_FMTW As String = "%04x%08x"

        Public Const STR_USBHCI_CLASS_HARDWAREIDW As String = "USB\\Class_E0&SubClass_01&Prot_01"

        ' defined(UNICODE) || defined(BTH_KERN)

        Public Const STR_ADDR_FMT = STR_ADDR_FMTW
        Public Const STR_ADDR_SHORT_FMT = STR_ADDR_SHORT_FMTW

        Public Const STR_USBHCI_CLASS_HARDWAREID = STR_USBHCI_CLASS_HARDWAREIDW

        ' '' UNICODE


        Public Function GET_BITS(field As UInteger, offset As UInteger, mask As UInteger) As UInteger
            Return ((field) >> CInt(offset)) And (mask)
        End Function


        Public Function GET_BIT(field As UInteger, offset As UInteger) As UInteger
            Return GET_BITS(field, offset, &H1)
        End Function


        Public Function LMP_3_SLOT_PACKETS(x As UInteger) As UInteger
            Return (GET_BIT(x, 0))
        End Function


        Public Function LMP_5_SLOT_PACKETS(x As UInteger) As UInteger
            Return (GET_BIT(x, 1))
        End Function


        Public Function LMP_ENCRYPTION(x As UInteger) As UInteger
            Return (GET_BIT(x, 2))
        End Function


        Public Function LMP_SLOT_OFFSET(x As UInteger) As UInteger
            Return (GET_BIT(x, 3))
        End Function


        Public Function LMP_TIMING_ACCURACY(x As UInteger) As UInteger
            Return (GET_BIT(x, 4))
        End Function


        Public Function LMP_SWITCH(x As UInteger) As UInteger
            Return (GET_BIT(x, 5))
        End Function


        Public Function LMP_HOLD_MODE(x As UInteger) As UInteger
            Return (GET_BIT(x, 6))
        End Function


        Public Function LMP_SNIFF_MODE(x As UInteger) As UInteger
            Return (GET_BIT(x, 7))
        End Function


        Public Function LMP_PARK_MODE(x As UInteger) As UInteger
            Return (GET_BIT(x, 8))
        End Function


        Public Function LMP_RSSI(x As UInteger) As UInteger
            Return (GET_BIT(x, 9))
        End Function


        Public Function LMP_CHANNEL_QUALITY_DRIVEN_MODE(x As UInteger) As UInteger
            Return (GET_BIT(x, 10))
        End Function


        Public Function LMP_SCO_LINK(x As UInteger) As UInteger
            Return (GET_BIT(x, 11))
        End Function


        Public Function LMP_HV2_PACKETS(x As UInteger) As UInteger
            Return (GET_BIT(x, 12))
        End Function


        Public Function LMP_HV3_PACKETS(x As UInteger) As UInteger
            Return (GET_BIT(x, 13))
        End Function


        Public Function LMP_MU_LAW_LOG(x As UInteger) As UInteger
            Return (GET_BIT(x, 14))
        End Function


        Public Function LMP_A_LAW_LOG(x As UInteger) As UInteger
            Return (GET_BIT(x, 15))
        End Function


        Public Function LMP_CVSD(x As UInteger) As UInteger
            Return (GET_BIT(x, 16))
        End Function


        Public Function LMP_PAGING_SCHEME(x As UInteger) As UInteger
            Return (GET_BIT(x, 17))
        End Function


        Public Function LMP_POWER_CONTROL(x As UInteger) As UInteger
            Return (GET_BIT(x, 18))
        End Function


        Public Function LMP_TRANSPARENT_SCO_DATA(x As UInteger) As UInteger
            Return (GET_BIT(x, 19))
        End Function


        Public Function LMP_FLOW_CONTROL_LAG(x As UInteger) As UInteger
            Return (GET_BITS(x, 20, &H3))
        End Function


        Public Function LMP_BROADCAST_ENCRYPTION(x As UInteger) As UInteger
            Return (GET_BIT(x, 23))
        End Function


        Public Function LMP_ENHANCED_DATA_RATE_ACL_2MBPS_MODE(x As UInteger) As UInteger
            Return (GET_BIT(x, 25))
        End Function


        Public Function LMP_ENHANCED_DATA_RATE_ACL_3MBPS_MODE(x As UInteger) As UInteger
            Return (GET_BIT(x, 26))
        End Function


        Public Function LMP_ENHANCED_INQUIRY_SCAN(x As UInteger) As UInteger
            Return (GET_BIT(x, 27))
        End Function


        Public Function LMP_INTERLACED_INQUIRY_SCAN(x As UInteger) As UInteger
            Return (GET_BIT(x, 28))
        End Function


        Public Function LMP_INTERLACED_PAGE_SCAN(x As UInteger) As UInteger
            Return (GET_BIT(x, 29))
        End Function


        Public Function LMP_RSSI_WITH_INQUIRY_RESULTS(x As UInteger) As UInteger
            Return (GET_BIT(x, 30))
        End Function


        Public Function LMP_ESCO_LINK(x As UInteger) As UInteger
            Return (GET_BIT(x, 31))
        End Function


        Public Function LMP_EV4_PACKETS(x As UInteger) As UInteger
            Return (GET_BIT(x, 32))
        End Function


        Public Function LMP_EV5_PACKETS(x As UInteger) As UInteger
            Return (GET_BIT(x, 33))
        End Function


        Public Function LMP_AFH_CAPABLE_SLAVE(x As UInteger) As UInteger
            Return (GET_BIT(x, 35))
        End Function


        Public Function LMP_AFH_CLASSIFICATION_SLAVE(x As UInteger) As UInteger
            Return (GET_BIT(x, 36))
        End Function


        Public Function LMP_BR_EDR_NOT_SUPPORTED(x As UInteger) As UInteger
            Return (GET_BIT(x, 37))
        End Function


        Public Function LMP_LE_SUPPORTED(x As UInteger) As UInteger
            Return (GET_BIT(x, 38))
        End Function


        Public Function LMP_3SLOT_EDR_ACL_PACKETS(x As UInteger) As UInteger
            Return (GET_BIT(x, 39))
        End Function


        Public Function LMP_5SLOT_EDR_ACL_PACKETS(x As UInteger) As UInteger
            Return (GET_BIT(x, 40))
        End Function


        Public Function LMP_SNIFF_SUBRATING(x As UInteger) As UInteger
            Return (GET_BIT(x, 41))
        End Function


        Public Function LMP_PAUSE_ENCRYPTION(x As UInteger) As UInteger
            Return (GET_BIT(x, 42))
        End Function


        Public Function LMP_AFH_CAPABLE_MASTER(x As UInteger) As UInteger
            Return (GET_BIT(x, 43))
        End Function


        Public Function LMP_AFH_CLASSIFICATION_MASTER(x As UInteger) As UInteger
            Return (GET_BIT(x, 44))
        End Function


        Public Function LMP_EDR_ESCO_2MBPS_MODE(x As UInteger) As UInteger
            Return (GET_BIT(x, 45))
        End Function


        Public Function LMP_EDR_ESCO_3MBPS_MODE(x As UInteger) As UInteger
            Return (GET_BIT(x, 46))
        End Function


        Public Function LMP_3SLOT_EDR_ESCO_PACKETS(x As UInteger) As UInteger
            Return (GET_BIT(x, 47))
        End Function


        Public Function LMP_EXTENDED_INQUIRY_RESPONSE(x As UInteger) As UInteger
            Return (GET_BIT(x, 48))
        End Function


        Public Function LMP_SIMULT_LE_BR_TO_SAME_DEV(x As UInteger) As UInteger
            Return (GET_BIT(x, 49))
        End Function


        Public Function LMP_SECURE_SIMPLE_PAIRING(x As UInteger) As UInteger
            Return (GET_BIT(x, 51))
        End Function


        Public Function LMP_ENCAPSULATED_PDU(x As UInteger) As UInteger
            Return (GET_BIT(x, 52))
        End Function


        Public Function LMP_ERRONEOUS_DATA_REPORTING(x As UInteger) As UInteger
            Return (GET_BIT(x, 53))
        End Function


        Public Function LMP_NON_FLUSHABLE_PACKET_BOUNDARY_FLAG(x As UInteger) As UInteger
            Return (GET_BIT(x, 54))
        End Function


        Public Function LMP_LINK_SUPERVISION_TIMEOUT_CHANGED_EVENT(x As UInteger) As UInteger
            Return (GET_BIT(x, 56))
        End Function


        Public Function LMP_INQUIRY_RESPONSE_TX_POWER_LEVEL(x As UInteger) As UInteger
            Return (GET_BIT(x, 57))
        End Function


        Public Function LMP_EXTENDED_FEATURES(x As UInteger) As UInteger
            Return (GET_BIT(x, 63))
        End Function



        ''
        '' IOCTL defines. 
        ''
        Public ReadOnly BTH_IOCTL_BASE As UInteger = 0

        Public Function BTH_CTL(id As UInteger) As CTL_CODE
            Return New CTL_CODE(FILE_DEVICE_BLUETOOTH,
                                (id),
                                METHOD_BUFFERED,
                                FILE_ANY_ACCESS)
        End Function


        Public Function BTH_KERNEL_CTL(id As UInteger) As CTL_CODE
            Return New CTL_CODE(FILE_DEVICE_BLUETOOTH,
                                (id),
                                METHOD_NEITHER,
                                FILE_ANY_ACCESS)
        End Function


        ''
        '' kernel-level (internal) IOCTLs
        ''
        Public ReadOnly IOCTL_INTERNAL_BTH_SUBMIT_BRB As CTL_CODE = BTH_KERNEL_CTL(CUInt(BTH_IOCTL_BASE + &H0))

        ''
        '' Input:  none
        '' Output:  BTH_ENUMERATOR_INFO
        ''
        Public ReadOnly IOCTL_INTERNAL_BTHENUM_GET_ENUMINFO As CTL_CODE = BTH_KERNEL_CTL(CUInt(BTH_IOCTL_BASE + &H1))

        ''
        '' Input:  none
        '' Output:  BTH_DEVICE_INFO
        ''
        Public ReadOnly IOCTL_INTERNAL_BTHENUM_GET_DEVINFO As CTL_CODE = BTH_KERNEL_CTL(CUInt(BTH_IOCTL_BASE + &H2))

        ''
        '' IOCTLs 
        ''
        ''
        '' Input:  none
        '' Output:  BTH_LOCAL_RADIO_INFO
        ''
        Public ReadOnly IOCTL_BTH_GET_LOCAL_INFO As CTL_CODE = BTH_CTL(CUInt(BTH_IOCTL_BASE + &H0))

        ''
        '' Input:  BTH_ADDR
        '' Output:  BTH_RADIO_INFO
        ''
        Public ReadOnly IOCTL_BTH_GET_RADIO_INFO As CTL_CODE = BTH_CTL(CUInt(BTH_IOCTL_BASE + &H1))

        ''
        '' use this ioctl to get a list of cached discovered devices in the port driver.
        ''
        '' Input: None
        '' Output: BTH_DEVICE_INFO_LIST
        ''
        Public ReadOnly IOCTL_BTH_GET_DEVICE_INFO As CTL_CODE = BTH_CTL(CUInt(BTH_IOCTL_BASE + &H2))

        ''
        '' Input:  BTH_ADDR
        '' Output:  none
        ''
        Public ReadOnly IOCTL_BTH_DISCONNECT_DEVICE As CTL_CODE = BTH_CTL(CUInt(BTH_IOCTL_BASE + &H3))
        ''
        '' Input:   BTH_VENDOR_SPECIFIC_COMMAND 
        '' Output:  PVOID
        ''
        Public ReadOnly IOCTL_BTH_HCI_VENDOR_COMMAND As CTL_CODE = BTH_CTL(CUInt(BTH_IOCTL_BASE + &H14))
        ''
        '' Input:  BTH_SDP_CONNECT
        '' Output:  BTH_SDP_CONNECT
        ''
        Public ReadOnly IOCTL_BTH_SDP_CONNECT As CTL_CODE = BTH_CTL(CUInt(BTH_IOCTL_BASE + &H80))

        ''
        '' Input:  HANDLE_SDP
        '' Output:  none
        ''
        Public ReadOnly IOCTL_BTH_SDP_DISCONNECT As CTL_CODE = BTH_CTL(CUInt(BTH_IOCTL_BASE + &H81))

        ''
        '' Input:  BTH_SDP_SERVICE_SEARCH_REQUEST
        '' Output:  ULong * number of handles wanted
        ''
        Public ReadOnly IOCTL_BTH_SDP_SERVICE_SEARCH As CTL_CODE = BTH_CTL(CUInt(BTH_IOCTL_BASE + &H82))

        ''
        '' Input:  BTH_SDP_ATTRIBUTE_SEARCH_REQUEST
        '' Output:  BTH_SDP_STREAM_RESPONSE Or bigger
        ''
        Public ReadOnly IOCTL_BTH_SDP_ATTRIBUTE_SEARCH As CTL_CODE = BTH_CTL(CUInt(BTH_IOCTL_BASE + &H83))

        ''
        '' Input:  BTH_SDP_SERVICE_ATTRIBUTE_SEARCH_REQUEST
        '' Output:  BTH_SDP_STREAM_RESPONSE Or bigger
        ''
        Public ReadOnly IOCTL_BTH_SDP_SERVICE_ATTRIBUTE_SEARCH As CTL_CODE = BTH_CTL(CUInt(BTH_IOCTL_BASE + &H84))

        ''
        '' Input:  raw SDP stream (at least 2 bytes)
        '' Ouptut: HANDLE_SDP
        ''
        Public ReadOnly IOCTL_BTH_SDP_SUBMIT_RECORD As CTL_CODE = BTH_CTL(CUInt(BTH_IOCTL_BASE + &H85))

        ''
        '' Input:  HANDLE_SDP
        '' Output:  none
        ''
        Public ReadOnly IOCTL_BTH_SDP_REMOVE_RECORD As CTL_CODE = BTH_CTL(CUInt(BTH_IOCTL_BASE + &H86))

        ''
        '' Input:  BTH_SDP_RECORD + raw SDP record
        '' Output:  HANDLE_SDP
        ''
        Public ReadOnly IOCTL_BTH_SDP_SUBMIT_RECORD_WITH_INFO As CTL_CODE = BTH_CTL(CUInt(BTH_IOCTL_BASE + &H87))

        ''
        '' Input:  NONE
        '' Output:  BTH_HOST_FEATURE_MASK
        ''
        Public ReadOnly IOCTL_BTH_GET_HOST_SUPPORTED_FEATURES As CTL_CODE = BTH_CTL(CUInt(BTH_IOCTL_BASE + &H88))


        Public Structure BTH_DEVICE_INFO_LIST

            '
            ' [IN/OUT] minimum of 1 device required
            '
            Public numOfDevices As UInteger

            '
            ' Open ended array of devices;
            '
            Public deviceList As BTH_DEVICE_INFO()


            Public Function ToPointer() As SafePtr
                Dim mm As New SafePtr
                Dim sz As Integer = Marshal.SizeOf(Of BTH_DEVICE_INFO)
                Dim ofs As Integer = 4

                mm.Alloc(4 + (sz * numOfDevices))

                numOfDevices = CUInt(deviceList.Length)
                mm.UIntAt(0) = numOfDevices

                For i = 0 To CInt(numOfDevices - 1)
                    mm.FromStructAt(Of BTH_DEVICE_INFO)(ofs, deviceList(i))
                    ofs += sz
                Next

                Return mm
            End Function

            Public Shared Function FromPointer(ptr As IntPtr) As BTH_DEVICE_INFO_LIST
                Dim op As BTH_DEVICE_INFO_LIST
                Dim mm As MemPtr = ptr
                Dim sz As Integer = Marshal.SizeOf(Of BTH_DEVICE_INFO)
                Dim ofs As Integer = 4

                op.numOfDevices = mm.UIntAt(0)

                ReDim op.deviceList(0 To CInt(op.numOfDevices - 1))

                For i = 0 To CInt(op.numOfDevices - 1)
                    op.deviceList(i) = mm.ToStructAt(Of BTH_DEVICE_INFO)(ofs)
                    ofs += sz
                Next

                Return op
            End Function



        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure PBTH_DEVICE_INFO_LIST
            Friend _ptr As MemPtr

            Public Sub New(allocSize As Integer)
                Alloc(CalcElementsFromSize(allocSize))
            End Sub

            Shared Function CalcElementsFromSize(size As Integer) As Integer
                size -= Marshal.SizeOf(Of UInteger)
                size = CInt(size / Marshal.SizeOf(Of BTH_DEVICE_INFO))

                Return size
            End Function

            Public Sub Alloc(elements As Integer)
                Free()
                Dim len As Integer = Marshal.SizeOf(Of UInteger) + (Marshal.SizeOf(Of BTH_DEVICE_INFO) * elements)
            End Sub

            Public Sub Free()
                If (_ptr <> IntPtr.Zero And _ptr <> CType(-1, IntPtr)) Then
                    Try
                        _ptr.Free()
                    Catch ex As Exception

                    End Try

                    _ptr = IntPtr.Zero
                End If
            End Sub

            ''
            '' [IN/OUT] minimum of 1 device required
            ''
            'Public numOfDevices As UInteger

            ''
            '' Open ended array of devices;
            ''
            'Public deviceList As BTH_DEVICE_INFO

            Public ReadOnly Property NumberOfDevices As UInteger
                Get
                    Return _ptr.UIntAt(0)
                End Get
            End Property

            Public ReadOnly Property Devices(index As UInteger) As BTH_DEVICE_INFO
                Get

                    Dim offset As IntPtr = New IntPtr(Marshal.SizeOf(Of UInteger) + (Marshal.SizeOf(Of BTH_DEVICE_INFO) * index))
                    Dim mm As MemPtr = _ptr + offset

                    Return mm.ToStruct(Of BTH_DEVICE_INFO)()
                End Get
            End Property

            Public Shared Narrowing Operator CType(var1 As PBTH_DEVICE_INFO_LIST) As IntPtr
                Return var1._ptr.Handle
            End Operator

            Public Shared Narrowing Operator CType(var1 As IntPtr) As PBTH_DEVICE_INFO_LIST
                Dim p = New PBTH_DEVICE_INFO_LIST()
                p._ptr = var1
                Return p
            End Operator

        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BTH_RADIO_INFO
            ''
            '' Supported LMP features of the radio.  Use LMP_XXX() to extract
            '' the desired bits.
            ''
            Public lmpSupportedFeatures As ULong

            ''
            '' Manufacturer ID (possibly BTH_MFG_XXX)
            ''
            Public mfg As BTH_MFG_INFO

            ''
            '' LMP subversion
            ''
            Public lmpSubversion As UShort

            ''
            '' LMP version
            ''
            Public lmpVersion As Byte

        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BTH_LOCAL_RADIO_INFO
            ''
            '' Local BTH_ADDR, class of defice, And radio name
            ''
            Public localInfo As BTH_DEVICE_INFO

            ''
            '' Combo of LOCAL_RADIO_XXX values
            ''
            Public flags As UInteger

            ''
            '' HCI revision, see core spec
            ''
            Public hciRevision As UShort

            ''
            '' HCI version, see core spec
            ''
            Public hciVersion As Byte

            ''
            '' More information about the local radio (LMP, MFG)
            ''
            Public radioInfo As BTH_RADIO_INFO

        End Structure




        Public Const SDP_CONNECT_CACHE = (&H1)
        Public Const SDP_CONNECT_ALLOW_PIN = (&H2)


        Public Const SDP_REQUEST_TO_DEFAULT = (0)
        Public Const SDP_REQUEST_TO_MIN = (10)
        Public Const SDP_REQUEST_TO_MAX = (45)

        Public Const SERVICE_OPTION_DO_NOT_PUBLISH = (&H2)
        Public Const SERVICE_OPTION_NO_PUBLIC_BROWSE = (&H4)
        Public Const SERVICE_OPTION_DO_NOT_PUBLISH_EIR = (&H8)

        Public Const SERVICE_SECURITY_USE_DEFAULTS = (&H0)
        Public Const SERVICE_SECURITY_NONE = (&H1)
        Public Const SERVICE_SECURITY_AUTHORIZE = (&H2)
        Public Const SERVICE_SECURITY_AUTHENTICATE = (&H4)
        Public Const SERVICE_SECURITY_ENCRYPT_REQUIRED = (&H10)
        Public Const SERVICE_SECURITY_ENCRYPT_OPTIONAL = (&H20)
        Public Const SERVICE_SECURITY_DISABLED = (&H10000000)
        Public Const SERVICE_SECURITY_NO_ASK = (&H20000000)

        ''
        '' Do Not attempt to validate that the stream can be parsed
        ''
        Public Const SDP_SEARCH_NO_PARSE_CHECK = (&H1)

        ''
        '' Do Not check the format of the results.  This includes suppression of both
        '' the check for a record patten (SEQ of UINT16 + value) And the validation
        '' of each universal attribute's accordance to the spec.
        ''
        Public Const SDP_SEARCH_NO_FORMAT_CHECK = (&H2)

        Public ReadOnly HANDLE_SDP_NULL As IntPtr = IntPtr.Zero

        Public ReadOnly HANDLE_SDP_LOCAL As IntPtr = New IntPtr(-2)




        Public Structure SDP_LARGE_INTEGER_16
            Public LowPart As ULong
            Public Highpart As Long
        End Structure
        Public Structure SDP_ULARGE_INTEGER_16
            Public LowPart As ULong
            Public Highpart As ULong
        End Structure


        Public Enum NodeContainerType
            NodeContainerTypeSequence
            NodeContainerTypeAlternative
        End Enum


        Public Enum SDP_TYPE
            SDP_TYPE_NIL = &H0
            SDP_TYPE_UINT = &H1
            SDP_TYPE_INT = &H2
            SDP_TYPE_UUID = &H3
            SDP_TYPE_STRING = &H4
            SDP_TYPE_BOOLEAN = &H5
            SDP_TYPE_SEQUENCE = &H6
            SDP_TYPE_ALTERNATIVE = &H7
            SDP_TYPE_URL = &H8
            SDP_TYPE_CONTAINER = &H20
        End Enum
        ''  9 - 31 are reserved


        '' allow for a little easier type checking / sizing for integers And UUIDs
        '' ((SDP_ST_XXX & &HF0) >> 4) == SDP_TYPE_XXX
        '' size of the data (in bytes) Is encoded as ((SDP_ST_XXX & &HF0) >> 8)
        Public Enum SDP_SPECIFICTYPE
            SDP_ST_NONE = &H0

            SDP_ST_UINT8 = &H10
            SDP_ST_UINT16 = &H110
            SDP_ST_UINT32 = &H210
            SDP_ST_UINT64 = &H310
            SDP_ST_UINT128 = &H410

            SDP_ST_INT8 = &H20
            SDP_ST_INT16 = &H120
            SDP_ST_INT32 = &H220
            SDP_ST_INT64 = &H320
            SDP_ST_INT128 = &H420

            SDP_ST_UUID16 = &H130
            SDP_ST_UUID32 = &H220
            SDP_ST_UUID128 = &H430
        End Enum

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure SdpAttributeRange
            Public minAttribute As UShort
            Public maxAttribute As UShort
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BTH_SDP_CONNECT
            ''
            '' Address of the remote SDP server.  Cannot be the local radio.
            ''
            Public bthAddress As BTH_ADDR

            ''
            '' Combination of SDP_CONNECT_XXX flags
            ''
            Public fSdpConnect As UInteger

            ''
            '' When the connect request returns, this will specify the handle to the
            '' SDP connection to the remote server
            ''
            Public hConnection As IntPtr

            ''
            '' Timeout, in seconds, for the requests on ths SDP channel.  If the request
            '' times out, the SDP connection represented by the HANDLE_SDP must be
            '' closed.  The values for this field are bound by SDP_REQUEST_TO_MIN And
            '' SDP_REQUEST_MAX.  If SDP_REQUEST_TO_DEFAULT Is specified, the timeout Is
            '' 30 seconds.
            ''
            Public requestTimeout As Byte

        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BTH_SDP_DISCONNECT
            ''
            '' hConnection returned by BTH_SDP_CONNECT
            ''
            Public hConnection As IntPtr

        End Structure


        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BTH_SDP_RECORD
            ''
            '' Combination of SERVICE_SECURITY_XXX flags
            ''
            Public fSecurity As UInteger

            ''
            '' Combination of SERVICE_OPTION_XXX flags
            ''
            Public fOptions As UInteger

            ''
            '' combo of COD_SERVICE_XXX flags
            ''
            Public fCodService As UInteger

            ''
            '' The length of the record array, in bytes.
            ''
            Public recordLength As UInteger

            ''
            '' The SDP record in its raw format
            ''
            Public record As Byte()


            Public Function ToPointer() As SafePtr
                Dim mm As New SafePtr

                recordLength = CUInt(record.Length)
                mm.Alloc(16 + recordLength)

                mm.UIntAt(0) = fSecurity
                mm.UIntAt(1) = fOptions
                mm.UIntAt(2) = fCodService

                mm.UIntAt(3) = recordLength

                mm.FromByteArray(record, 16)

                Return mm
            End Function

            Public Shared Function FromPointer(ptr As IntPtr) As BTH_SDP_RECORD
                Dim op As BTH_SDP_RECORD
                Dim mm As MemPtr = ptr

                op.fSecurity = mm.UIntAt(0)
                op.fOptions = mm.UIntAt(1)
                op.fCodService = mm.UIntAt(2)
                op.recordLength = mm.UIntAt(3)

                op.record = mm.ToByteArray(16, op.recordLength)

                Return op
            End Function

        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BTH_SDP_SERVICE_SEARCH_REQUEST
            ''
            '' Handle returned by the connect request Or HANDLE_SDP_LOCAL
            ''
            Public hConnection As IntPtr

            ''
            '' Array of UUIDs.  Each entry can be either a 2 byte, 4 byte Or 16 byte
            '' UUID. SDP spec mandates that a request can have a maximum of 12 UUIDs.
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=MAX_UUIDS_IN_QUERY)>
            Public uuids As Guid()

        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BTH_SDP_ATTRIBUTE_SEARCH_REQUEST
            ''
            '' Handle returned by the connect request Or HANDLE_SDP_LOCAL
            ''
            Public hConnection As IntPtr

            ''
            '' Combo of SDP_SEARCH_Xxx flags
            ''
            Public searchFlags As UInteger

            ''
            '' Record handle returned by the remote SDP server, most likely from a
            '' previous BTH_SDP_SERVICE_SEARCH_RESPONSE.
            ''
            Public recordHandle As UInteger

            ''
            '' Array of attributes to query for.  Each SdpAttributeRange entry can
            '' specify either a single attribute Or a range.  To specify a single
            '' attribute, minAttribute should be equal to maxAttribute.   The array must
            '' be in sorted order, starting with the smallest attribute.  Furthermore,
            '' if a range Is specified, the minAttribute must be <= maxAttribute.
            ''
            Public range As SdpAttributeRange()

            Public Function ToPointer() As SafePtr
                Dim mm As New SafePtr
                Dim sz = Marshal.SizeOf(Of SdpAttributeRange)
                Dim ofs As Integer = IntPtr.Size

                Dim size = (sz * range.Length) + IntPtr.Size + 8

                mm.Alloc(size)

                If (ofs = 4) Then
                    mm.UIntAt(0) = CUInt(hConnection)
                Else
                    mm.ULongAt(0) = CULng(hConnection)
                End If

                mm.UIntAtAbsolute(ofs) = searchFlags
                mm.UIntAtAbsolute(ofs + 4) = recordHandle

                ofs += 8

                For i = 0 To range.Length - 1

                    mm.FromStructAt(Of SdpAttributeRange)(ofs, range(i))
                    ofs += sz
                Next

                Return mm
            End Function


        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BTH_SDP_SERVICE_ATTRIBUTE_SEARCH_REQUEST
            ''
            '' Handle returned by the connect request Or HANDLE_SDP_LOCAL
            ''
            Public hConnection As IntPtr

            ''
            '' Combo of SDP_SEARCH_Xxx flags
            ''
            Public searchFlags As UInteger

            ''
            '' See comments in BTH_SDP_SERVICE_SEARCH_REQUEST
            ''
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=MAX_UUIDS_IN_QUERY)>
            Public uuids As Guid()

            ''
            '' See comments in BTH_SDP_ATTRIBUTE_SEARCH_REQUEST
            ''
            Public range As SdpAttributeRange()


            Public Function ToPointer() As SafePtr
                Dim mm As New SafePtr
                Dim sz = Marshal.SizeOf(Of SdpAttributeRange)
                Dim szg = Marshal.SizeOf(Of Guid)
                Dim ofs As Integer = IntPtr.Size

                Dim size = (sz * range.Length) + IntPtr.Size + 4 + (MAX_UUIDS_IN_QUERY * szg)

                mm.Alloc(size)

                If (ofs = 4) Then
                    mm.UIntAt(0) = CUInt(hConnection)
                Else
                    mm.ULongAt(0) = CULng(hConnection)
                End If

                mm.UIntAtAbsolute(ofs) = searchFlags

                ofs += 4

                For j = 0 To CInt(MAX_UUIDS_IN_QUERY - 1)
                    mm.FromStructAt(Of Guid)(ofs, uuids(j))
                    ofs += szg
                Next

                For i = 0 To range.Length - 1

                    mm.FromStructAt(Of SdpAttributeRange)(ofs, range(i))
                    ofs += sz
                Next

                Return mm
            End Function


        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BTH_SDP_STREAM_RESPONSE
            ''
            '' The required buffer size (Not including the first 2 ULONG_PTRs of this
            '' data structure) needed to contain the response.
            ''
            '' If the buffer passed was large enough to contain the entire response,
            '' requiredSize will be equal to responseSize.  Otherwise, the caller should
            '' resubmit the request with a buffer size equal to
            '' sizeof(BTH_SDP_STREAM_RESPONSE) + requiredSize - 1.  (The -1 Is because
            '' the size of this data structure already includes one byte of the
            '' response.)
            ''
            '' A response cannot exceed 4GB in size.
            ''
            Public requiredSize As UInteger

            ''
            '' The number of bytes copied into the response array of this data
            '' structure.  If there Is Not enough room for the entire response, the
            '' response will be partially copied into the response array.
            ''
            Public responseSize As UInteger

            ''
            '' The raw SDP response from the search.
            ''
            Public response As Byte()

            Public Shared Function FromPointer(ptr As IntPtr) As BTH_SDP_STREAM_RESPONSE
                Dim op As BTH_SDP_STREAM_RESPONSE
                Dim mm As MemPtr = ptr

                op.requiredSize = mm.UIntAt(0)
                op.responseSize = mm.UIntAt(1)
                op.response = mm.ToByteArray(8, op.responseSize)

                Return op
            End Function

        End Structure

        ''
        '' Vendor specific HCI command header
        ''
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BTH_COMMAND_HEADER

            ''
            '' Opcode for the command
            '' 
            Public OpCode As UShort

            ''
            '' Payload of the command excluding the header.
            '' TotalParameterLength = TotalCommandLength - sizeof(BTH_COMMAND_HEADER)
            '' 
            Public TotalParameterLength As Byte

        End Structure

        ''
        '' Vendor Specific Command structure
        ''
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BTH_VENDOR_SPECIFIC_COMMAND
            ''
            '' Manufacturer ID
            '' 
            Public ManufacturerId As UInteger

            ''
            '' LMP version. Command Is send to radio only if the radios 
            '' LMP version Is greater than this value.
            '' 
            Public LmpVersion As Byte

            ''
            '' Should all the patterns match Or just one. If MatchAnySinglePattern == TRUE
            '' then if a single pattern matches the command, we decide that we have a match.
            <MarshalAs(UnmanagedType.Bool)>
            Public MatchAnySinglePattern As Boolean

            ''
            '' HCI Command Header
            ''
            Public HciHeader As BTH_COMMAND_HEADER

            ''
            '' Data for the above command including patterns
            ''
            Public Data As Byte()

            Public Function ToPointer() As SafePtr

                Dim mm As New SafePtr

                Dim size As Integer = 6 + Data.Length + Marshal.SizeOf(Of BTH_COMMAND_HEADER)
                Dim dataStart = 6 + Marshal.SizeOf(Of BTH_COMMAND_HEADER)

                HciHeader.TotalParameterLength = CByte(Data.Length)

                mm.Alloc(size)

                mm.UIntAt(0) = ManufacturerId
                mm.ByteAt(4) = LmpVersion
                mm.ByteAt(5) = If(MatchAnySinglePattern, CByte(1), CByte(0))

                mm.FromStructAt(Of BTH_COMMAND_HEADER)(6, HciHeader)
                mm.FromByteArray(Data, dataStart)

                Return mm

            End Function

            Public Shared Function FromPointer(ptr As IntPtr) As BTH_VENDOR_SPECIFIC_COMMAND
                Dim op As BTH_VENDOR_SPECIFIC_COMMAND
                Dim mm As MemPtr = ptr

                op.ManufacturerId = mm.UIntAt(0)
                op.LmpVersion = mm.ByteAt(4)
                op.MatchAnySinglePattern = mm.ByteAt(5) = 1
                op.HciHeader = mm.ToStructAt(Of BTH_COMMAND_HEADER)(6)
                op.Data = mm.ToByteArray(6 + Marshal.SizeOf(Of BTH_COMMAND_HEADER), op.HciHeader.TotalParameterLength)

                Return op
            End Function

        End Structure

        ''
        '' Structure of patterns
        ''
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BTH_VENDOR_PATTERN
            ''
            '' Pattern Offset in the event structure excluding EVENT header
            '' 
            Public Offset As Byte

            ''
            '' Size of the Pattern
            '' 
            Public Size As Byte

            ''
            '' Pattern
            '' 
            Public Pattern As Byte()

            Public Function ToPointer() As SafePtr
                Dim mm As New SafePtr

                Dim size As Integer = 2 + Pattern.Length
                Me.Size = CByte(Pattern.Length)

                mm.Alloc(size)

                mm.ByteAt(0) = Offset
                mm.ByteAt(1) = CByte(size)

                mm.FromByteArray(Pattern, 2)

                Return mm
            End Function

            Public Shared Function FromPointer(ptr As IntPtr) As BTH_VENDOR_PATTERN
                Dim mm As MemPtr = ptr
                Dim op As BTH_VENDOR_PATTERN


                op.Offset = mm.ByteAt(0)
                op.Size = mm.ByteAt(1)

                op.Pattern = mm.ToByteArray(2, op.Size)

                Return op
            End Function

        End Structure

        ''
        ''The buffer associated with GUID_BLUETOOTH_HCI_VENDOR_EVENT
        ''
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BTH_VENDOR_EVENT_INFO
            ''
            ''Local radio address with which the event Is associated.
            ''
            Public BthAddress As BTH_ADDR

            ''
            ''Size of the event buffer including Event header
            ''
            Public EventSize As UInteger

            ''
            ''Information associated with the event
            ''
            Public EventInfo As Byte()

            Public Function ToPointer() As SafePtr
                Dim mm As New SafePtr
                Dim size = 12 + EventInfo.Length

                EventSize = CUInt(EventInfo.Length)

                mm.Alloc(size)

                mm.ULongAt(0) = BthAddress
                mm.UIntAtAbsolute(8) = EventSize

                mm.FromByteArray(EventInfo, 12)

                Return mm
            End Function

            Public Shared Function FromPointer(ptr As IntPtr) As BTH_VENDOR_EVENT_INFO
                Dim op As BTH_VENDOR_EVENT_INFO
                Dim mm As MemPtr = ptr

                op.BthAddress = mm.ULongAt(0)

                op.EventSize = mm.UIntAtAbsolute(8)
                op.EventInfo = mm.ToByteArray(12, op.EventSize)

                Return op
            End Function

        End Structure



        ''
        '' Host supported features
        ''
        Public Const BTH_HOST_FEATURE_ENHANCED_RETRANSMISSION_MODE As ULong = (&H1)
        Public Const BTH_HOST_FEATURE_STREAMING_MODE As ULong = (&H2)
        Public Const BTH_HOST_FEATURE_LOW_ENERGY As ULong = (&H4)
        Public Const BTH_HOST_FEATURE_SCO_HCI As ULong = (&H8)
        Public Const BTH_HOST_FEATURE_SCO_HCIBYPASS As ULong = (&H10)

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BTH_HOST_FEATURE_MASK
            ''
            '' Mask of supported features. 
            ''
            Public Mask As ULong

            ''
            '' Reserved for future use.
            ''
            Public Reserved1 As ULong
            Public Reserved2 As ULong
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BLUETOOTH_DEVICE_SEARCH_PARAMS
            Public dwSize As UInteger

            <MarshalAs(UnmanagedType.Bool)>
            Public fReturnAuthenticated As Boolean

            <MarshalAs(UnmanagedType.Bool)>
            Public fReturnRemembered As Boolean

            <MarshalAs(UnmanagedType.Bool)>
            Public fReturnUnknown As Boolean

            <MarshalAs(UnmanagedType.Bool)>
            Public fReturnConnected As Boolean

            <MarshalAs(UnmanagedType.Bool)>
            Public fIssueInquiry As Boolean

            Public cTimeoutMultiplier As Byte

            Public hRadio As IntPtr
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure BLUETOOTH_FIND_RADIO_PARAMS
            Public dwSize As UInteger
        End Structure

        '        HBLUETOOTH_RADIO_FIND BluetoothFindFirstRadio(
        '  const BLUETOOTH_FIND_RADIO_PARAMS *pbtfrp,
        '  HANDLE * phRadio
        ');

        Public Const BLUETOOTH_MAX_NAME_SIZE = (248)
        Public Const BLUETOOTH_MAX_PASSKEY_SIZE = (16)
        Public Const BLUETOOTH_MAX_PASSKEY_BUFFER_SIZE = (BLUETOOTH_MAX_PASSKEY_SIZE + 1)
        Public Const BLUETOOTH_MAX_SERVICE_NAME_SIZE = (256)
        Public Const BLUETOOTH_DEVICE_NAME_SIZE = (256)


        Public Declare Unicode Function BluetoothFindFirstRadio Lib "bluetoothapis.dll" _
            (ByRef pbtfrp As BLUETOOTH_FIND_RADIO_PARAMS, <Out> ByRef phradio As IntPtr) As IntPtr

        Public Declare Unicode Function BluetoothFindNextRadio Lib "bluetoothapis.dll" _
            (hFind As IntPtr, <Out> ByRef phRadio As IntPtr) As Boolean

        Public Declare Unicode Function BluetoothFindRadioClose Lib "bluetoothapis.dll" _
            (hFind As IntPtr) As Boolean


        Public Declare Unicode Function BluetoothFindFirstDevice Lib "bluetoothapis.dll" _
            (pbtdsp As BLUETOOTH_DEVICE_SEARCH_PARAMS, ByRef pbtdi As BLUETOOTH_DEVICE_INFO) As IntPtr

        Public Declare Unicode Function BluetoothFindNextDevice Lib "bluetoothapis.dll" _
            (hFind As IntPtr, ByRef pbtdi As BLUETOOTH_DEVICE_INFO) As Boolean

        Public Declare Unicode Function BluetoothFindDeviceClose Lib "bluetoothapis.dll" _
            (hFind As IntPtr) As Boolean


        Public Declare Unicode Function BluetoothGetRadioInfo Lib "bluetoothapis.dll" _
            (hRadio As IntPtr, ByRef pRadioInfo As BLUETOOTH_RADIO_INFO) As UInteger


        Public Function _internalEnumBluetoothDevices() As BLUETOOTH_DEVICE_INFO()
            Dim bl As New List(Of BLUETOOTH_DEVICE_INFO)

            'Dim hDevice As IntPtr
            Dim hRadio As IntPtr
            Dim hFind As IntPtr

            Dim radFind As IntPtr
            Dim radParams As BLUETOOTH_FIND_RADIO_PARAMS

            Dim params As BLUETOOTH_DEVICE_SEARCH_PARAMS
            Dim brInfo As BLUETOOTH_DEVICE_INFO

            Dim fr As Boolean
            Dim frad As Boolean

            radParams.dwSize = 4
            radFind = BluetoothFindFirstRadio(radParams, hRadio)

            If (radFind = IntPtr.Zero) Then Return Nothing

            Do

                params.dwSize = CUInt(Marshal.SizeOf(Of BLUETOOTH_DEVICE_SEARCH_PARAMS))

                params.fReturnRemembered = True
                params.fReturnAuthenticated = True
                params.fReturnConnected = True
                params.fReturnUnknown = True
                params.cTimeoutMultiplier = 2
                params.hRadio = hRadio

                brInfo = New BLUETOOTH_DEVICE_INFO()
                brInfo.dwSize = Marshal.SizeOf(Of BLUETOOTH_DEVICE_INFO)

                hFind = BluetoothFindFirstDevice(params, brInfo)

                If (hFind = IntPtr.Zero) Then
                    CloseHandle(hRadio)
                    BluetoothFindRadioClose(radFind)

                    bl.ToArray()
                End If

                bl.Add(brInfo)

                Do

                    brInfo = New BLUETOOTH_DEVICE_INFO()
                    brInfo.dwSize = Marshal.SizeOf(Of BLUETOOTH_DEVICE_INFO)

                    fr = BluetoothFindNextDevice(hFind, brInfo)
                    If (fr) Then bl.Add(brInfo)

                Loop While (fr = True)

                BluetoothFindDeviceClose(hFind)

                CloseHandle(hRadio)
                frad = BluetoothFindNextRadio(radFind, hRadio)

            Loop While (frad = True)

            BluetoothFindRadioClose(radFind)

            Return bl.ToArray()
        End Function

        Public Function _internalEnumBluetoothRadios() As BLUETOOTH_RADIO_INFO()
            Dim bl As New List(Of BLUETOOTH_RADIO_INFO)

            Dim hRadio As IntPtr
            Dim hFind As IntPtr
            Dim params As BLUETOOTH_FIND_RADIO_PARAMS
            Dim brInfo As BLUETOOTH_RADIO_INFO
            Dim fr As Boolean

            params.dwSize = 4
            hFind = BluetoothFindFirstRadio(params, hRadio)

            If (hFind = IntPtr.Zero) Then Return Nothing

            Do
                brInfo = New BLUETOOTH_RADIO_INFO()
                brInfo.dwSize = CULng(Marshal.SizeOf(Of BLUETOOTH_RADIO_INFO))

                Dim x = BluetoothGetRadioInfo(hRadio, brInfo)
                Dim s = NativeErrorMethods.FormatLastError(x)

                CloseHandle(hRadio)

                bl.Add(brInfo)

                fr = BluetoothFindNextRadio(hFind, hRadio)
            Loop While (fr = True)

            BluetoothFindRadioClose(hFind)

            Return bl.ToArray()
        End Function


    End Module

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Public Structure BLUETOOTH_ADDRESS
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=8)>
        Public rgBytes As Byte()

        Public Overrides Function ToString() As String

            ToString = ""
            If rgBytes Is Nothing OrElse rgBytes.Length < 5 Then Return ""
            For i = 5 To 0 Step -1
                If (ToString <> "") Then ToString &= ":"
                ToString &= rgBytes(i).ToString("X2")
            Next

        End Function

        Public Shared Narrowing Operator CType(val1 As ULong) As BLUETOOTH_ADDRESS
            Dim bt As BLUETOOTH_ADDRESS
            bt.rgBytes = BitConverter.GetBytes(val1)

            Return bt
        End Operator

        Public Shared Narrowing Operator CType(val1 As BLUETOOTH_ADDRESS) As ULong
            Return BitConverter.ToUInt64(val1.rgBytes, 0)
        End Operator

    End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Public Structure BLUETOOTH_RADIO_INFO
        Public dwSize As ULong '' Size, In bytes, Of this entire data Structure

        Public address As BLUETOOTH_ADDRESS '' Address Of the local radio

        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=BLUETOOTH_MAX_NAME_SIZE)>
        Public szName As String

        Public ulClassofDevice As UInteger '' Class of device for the local radio

        Public lmpSubVersion As UShort                    '' lmpSubversion, manufacturer specifc.
        Public manufacturer As BTH_MFG_INFO                        '' Manufacturer Of the radio, BTH_MFG_Xxx value.  For the most up to date
        '' list, goto the Bluetooth specification website And get the Bluetooth
        '' assigned numbers document.
    End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Public Structure BLUETOOTH_DEVICE_INFO
        Public dwSize As Long

        Public Address As BLUETOOTH_ADDRESS

        Public ulClassofDevice As UInteger

        <MarshalAs(UnmanagedType.Bool)>
        Public fConnected As Boolean

        <MarshalAs(UnmanagedType.Bool)>
        Public fRemembered As Boolean

        <MarshalAs(UnmanagedType.Bool)>
        Public fAuthenticated As Boolean

        <MarshalAs(UnmanagedType.Struct)>
        Public stLastSeen As SYSTEMTIME

        <MarshalAs(UnmanagedType.Struct)>
        Public stLastUsed As SYSTEMTIME

        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=BLUETOOTH_MAX_NAME_SIZE)>
        Public szName As String
    End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Public Structure BTH_MFG_INFO

        Private Shared Manufacturers As Dictionary(Of UShort, String) = New Dictionary(Of UShort, String)

        Private _val As UShort

        Public ReadOnly Property Value As UShort
            Get
                Return _val
            End Get
        End Property

        Public ReadOnly Property Name As String
            Get
                Return ToString()
            End Get
        End Property

        Friend Sub New(name As String, value As UShort)
            _val = value
            If Not Manufacturers.ContainsKey(value) Then Manufacturers.Add(value, name)
        End Sub

        Public Overrides Function ToString() As String
            If Manufacturers.ContainsKey(_val) Then Return String.Format("{0}: {1}", _val, Manufacturers(_val)) Else Return _val.ToString()
        End Function

        Public Shared Widening Operator CType(val1 As BTH_MFG_INFO) As UShort
            Return val1._val
        End Operator

        Public Shared Narrowing Operator CType(val1 As UShort) As BTH_MFG_INFO
            Dim b As BTH_MFG_INFO
            b._val = val1

            Return b
        End Operator

    End Structure


End Namespace
