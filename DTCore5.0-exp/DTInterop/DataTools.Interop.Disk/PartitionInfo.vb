'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: Very low level disk partition information
''         Utility library.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Imports System.ComponentModel
Imports DataTools.Interop.Native
Imports CoreCT.Text

Namespace Disk

    ''' <summary>
    ''' Partition styles.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum PartitionStyle As UInteger

        ''' <summary>
        ''' Master Boot Record
        ''' </summary>
        ''' <remarks></remarks>
        <Description("Master Boot Record")>
        Mbr = 0

        ''' <summary>
        ''' Guid Partition Table
        ''' </summary>
        ''' <remarks></remarks>
        <Description("Guid Partition Table")>
        Gpt = 1

        ''' <summary>
        ''' Raw (unconfigured)
        ''' </summary>
        ''' <remarks></remarks>
        <Description("Raw (unconfigured)")>
        Raw = 2

    End Enum

    ''' <summary>
    ''' Partition characteristics.
    ''' </summary>
    ''' <remarks></remarks>
    <Flags>
    Public Enum PartitionCharacteristics

        ''' <summary>
        ''' None or no information.
        ''' </summary>
        ''' <remarks></remarks>
        None = 0

        ''' <summary>
        ''' File System Partition
        ''' </summary>
        ''' <remarks></remarks>
        Filesystem = &H1

        ''' <summary>
        ''' Container Partition
        ''' </summary>
        ''' <remarks></remarks>
        Container = &H2

        ''' <summary>
        ''' Hidden Partition
        ''' </summary>
        ''' <remarks></remarks>
        Hidden = &H4

        ''' <summary>
        ''' Hibernation Partition
        ''' </summary>
        ''' <remarks></remarks>
        Hibernaton = &H8

        ''' <summary>
        ''' Service Partition
        ''' </summary>
        ''' <remarks></remarks>
        Service = &H10

        ''' <summary>
        ''' Secured Partition
        ''' </summary>
        ''' <remarks></remarks>
        Secured = &H20

        ''' <summary>
        ''' Policy Partition
        ''' </summary>
        ''' <remarks></remarks>
        Policy = &H40

        ''' <summary>
        ''' Blocker Partition
        ''' </summary>
        ''' <remarks></remarks>
        Blocker = &H80
    End Enum

    ''' <summary>
    ''' Partition occurrence flags (where on the disk a partition entry may occur).
    ''' </summary>
    ''' <remarks></remarks>
    <Flags>
    Public Enum PartitionOccurrence
        ''' <summary>
        ''' None or no information.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("No Information")>
        None = 0

        ''' <summary>
        ''' Master Boot Record
        ''' </summary>
        ''' <remarks></remarks>
        <Description("Master Boot Record")>
        MBR = 1

        ''' <summary>
        ''' Extended Boot Record
        ''' </summary>
        ''' <remarks></remarks>
        <Description("Extended Boot Record")>
        EBR = 2

        ''' <summary>
        ''' Virtual Master Boot Record
        ''' </summary>
        ''' <remarks></remarks>
        <Description("Virtual Master Boot Record")>
        VirtualMBR = 4

    End Enum

    ''' <summary>
    ''' Partition access feature flags.
    ''' </summary>
    ''' <remarks></remarks>
    <Flags>
    Public Enum PartitionAccess

        ''' <summary>
        ''' Nothing or N/A
        ''' </summary>
        ''' <remarks></remarks>
        <Description("Nothing or N/A")>
        None = 0

        ''' <summary>
        ''' Logical Block Addressing
        ''' </summary>
        ''' <remarks></remarks>
        <Description("Logical Block Addressing")>
        LBA = 1

        ''' <summary>
        ''' Cylinder-Head-Sector
        ''' </summary>
        ''' <remarks></remarks>
        <Description("Cylinder-Head-Sector")>
        CHS = 2

    End Enum

    ''' <summary>
    ''' Partition bootability flags
    ''' </summary>
    ''' <remarks></remarks>
    <Flags>
    Public Enum PartitionBootability

        ''' <summary>
        ''' No information.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("No Information")>
        NoInfo = 0

        ''' <summary>
        ''' Generally not bootable.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("Generally Not Bootable")>
        No = 1

        ''' <summary>
        ''' Generally bootable.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("Generally Bootable")>
        Yes = 2

        ''' <summary>
        ''' Bootable on x86 systems.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("Bootable on x86 systems")>
        x86 = 4

        ''' <summary>
        ''' Bootable on 286 or greater systems.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("Bootable on i286 or greater systems")>
        I286 = 8

        ''' <summary>
        ''' Bootable on 386 or greater systems.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("Bootable on i386 or greater systems")>
        I386 = &H10

        ''' <summary>
        ''' Bootable on 486 or greater systems.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("Bootable on i486 or greater systems")>
        I486 = &H20

        ''' <summary>
        ''' Bootable on 586/Pentium or greater systems.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("Bootable on i586/Pentium or greater systems")>
        I586 = &H40

        ''' <summary>
        ''' Bootable on Motorola 68000 systems.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("Bootable on Motorola 68000 systems")>
        M68000 = &H100

        ''' <summary>
        ''' Bootable on Intel 8080 / Zilog Z80 systems.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("Bootable on Intel 8080/Zilog Z80 systems")>
        I8080Z80 = &H200

        ''' <summary>
        ''' Bootable as an Adavanced Active Partition.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("Bootable as an Advanced Active Partition")>
        AAP = &H400

        ''' <summary>
        ''' Bootable on PowerPC systems.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("Bootable on PowerPC systems")>
        PowerPC = &H800

    End Enum

    ''' <summary>
    ''' Gpt Partition Flags
    ''' </summary>
    ''' <remarks></remarks>
    <Flags>
    Public Enum GptPartitionAttributes As ULong

        ''' <summary>
        ''' The partition is required for the platform.
        ''' </summary>
        ''' <remarks></remarks>
        RequiredPartition = 1

        ''' <summary>
        ''' UEFI does not produce a block IO device for this partition.
        ''' </summary>
        ''' <remarks></remarks>
        NoBlockIoProtocol = 2

        ''' <summary>
        ''' The partition is visible as bootable to legacy software.
        ''' </summary>
        ''' <remarks></remarks>
        LegacyBiosBootable = 4

        ''' <summary>
        ''' UEFI reserved bits 3-47.
        ''' </summary>
        ''' <remarks></remarks>
        UefiReservedMask = &HFFFFFFFFFFF8

        ''' <summary>
        ''' Platform reserved bits 48-63.
        ''' </summary>
        ''' <remarks></remarks>
        PlatformReservedMask = &HFFFF000000000000UL

    End Enum

    ''' <summary>
    ''' Prints a friendly hexadecimal Mbr partition byte code.
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure FriendlyPartitionId
        Public Value As Byte

        Public Sub New(v As Byte)
            Value = v
        End Sub

        Public Overrides Function ToString() As String
            Return Value.ToString("X2") & "h"
        End Function

        Public Shared Widening Operator CType(operand As Byte) As FriendlyPartitionId
            Return New FriendlyPartitionId(operand)
        End Operator

        Public Shared Widening Operator CType(operand As FriendlyPartitionId) As Byte
            Return operand.Value
        End Operator

        Public Shared Narrowing Operator CType(operand As FriendlyPartitionId) As String
            Return operand.ToString
        End Operator

    End Structure

    ''' <summary>
    ''' Contains the master list of all known Gpt partition types as GptCodeInfo objects.
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class GptCodeInfo
        Private Shared _Col As New List(Of GptCodeInfo)

        Private _Guid As Guid
        Private _Name As String

        ''' <summary>
        ''' Returns the Guid for the Gpt partition type.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Guid As System.Guid
            Get
                Return _Guid
            End Get
        End Property

        ''' <summary>
        ''' Returns the name of the partition type.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Name As String
            Get
                Return _Name
            End Get
        End Property

        ''' <summary>
        ''' Returns the description (currently returns the name)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Description As String
            Get
                Return _Name
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return Name
        End Function

        Private Sub New(s As String, t As String)
            _Guid = New Guid(t)
            _Name = s
        End Sub

        Public Shared Function FindByCode(g As Guid) As GptCodeInfo
            For Each c In _Col
                If c.Guid.Equals(g) Then Return c
            Next
            Return Nothing
        End Function

#Region "Code Initialization"


        Shared Sub New()

            _Col.Add(New GptCodeInfo("MBR partition scheme", "024DEE41-33E7-11D3-9D69-0008C781F39F"))
            _Col.Add(New GptCodeInfo("EFI System partition", "C12A7328-F81F-11D2-BA4B-00A0C93EC93B"))
            _Col.Add(New GptCodeInfo("BIOS Boot partition", "21686148-6449-6E6F-744E-656564454649"))
            _Col.Add(New GptCodeInfo("Intel Fast Flash (iFFS)) partition (for Intel Rapid Start technology))", "D3BFE2DE-3DAF-11DF-BA40-E3A556D89593"))
            _Col.Add(New GptCodeInfo("Sony boot partition", "F4019732-066E-4E12-8273-346C5641494F"))
            _Col.Add(New GptCodeInfo("Lenovo boot partition", "BFBFAFE7-A34F-448A-9A5B-6213EB736C22"))
            _Col.Add(New GptCodeInfo("Microsoft Reserved Partition (MSR))", "E3C9E316-0B5C-4DB8-817D-F92DF00215AE"))
            _Col.Add(New GptCodeInfo("Basic data partition", "EBD0A0A2-B9E5-4433-87C0-68B6B72699C7"))
            _Col.Add(New GptCodeInfo("Logical Disk Manager (LDM)) metadata partition", "5808C8AA-7E8F-42E0-85D2-E1E90434CFB3"))
            _Col.Add(New GptCodeInfo("Logical Disk Manager data partition", "AF9B60A0-1431-4F62-BC68-3311714A69AD"))
            _Col.Add(New GptCodeInfo("Windows Recovery Environment", "DE94BBA4-06D1-4D40-A16A-BFD50179D6AC"))
            _Col.Add(New GptCodeInfo("IBM General Parallel File System (GPFS)) partition", "37AFFC90-EF7D-4E96-91C3-2D7AE055B174"))
            _Col.Add(New GptCodeInfo("Data partition", "75894C1E-3AEB-11D3-B7C1-7B03A0000000"))
            _Col.Add(New GptCodeInfo("Service Partition", "E2A1E728-32E3-11D6-A682-7B03A0000000"))
            _Col.Add(New GptCodeInfo("Linux filesystem data", "0FC63DAF-8483-4772-8E79-3D69D8477DE4"))
            _Col.Add(New GptCodeInfo("RAID partition", "A19D880F-05FC-4D3B-A006-743F0F84911E"))
            _Col.Add(New GptCodeInfo("Swap partition", "0657FD6D-A4AB-43C4-84E5-0933C84B4F4F"))
            _Col.Add(New GptCodeInfo("Logical Volume Manager (LVM)) partition", "E6D6D379-F507-44C2-A23C-238F2A3DF928"))
            _Col.Add(New GptCodeInfo("/home partition", "933AC7E1-2EB4-4F13-B844-0E14E2AEF915"))
            _Col.Add(New GptCodeInfo("plain dm-crypt partition", "7FFEC5C9-2D00-49B7-8941-3EA10A5586B7"))
            _Col.Add(New GptCodeInfo("LUKS partition", "CA7D7CCB-63ED-4C53-861C-1742536059CC"))
            _Col.Add(New GptCodeInfo("Reserved", "8DA63339-0007-60C0-C436-083AC8230908"))
            _Col.Add(New GptCodeInfo("Boot partition", "83BD6B9D-7F41-11DC-BE0B-001560B84F0F"))
            _Col.Add(New GptCodeInfo("Data partition", "516E7CB4-6ECF-11D6-8FF8-00022D09712B"))
            _Col.Add(New GptCodeInfo("Swap partition", "516E7CB5-6ECF-11D6-8FF8-00022D09712B"))
            _Col.Add(New GptCodeInfo("Unix File System (UFS) partition", "516E7CB6-6ECF-11D6-8FF8-00022D09712B"))
            _Col.Add(New GptCodeInfo("Vinum volume manager partition", "516E7CB8-6ECF-11D6-8FF8-00022D09712B"))
            _Col.Add(New GptCodeInfo("ZFS partition", "516E7CBA-6ECF-11D6-8FF8-00022D09712B"))
            _Col.Add(New GptCodeInfo("Hierarchical File System Plus (HFS+)) partition", "48465300-0000-11AA-AA11-00306543ECAC"))
            _Col.Add(New GptCodeInfo("Apple UFS", "55465300-0000-11AA-AA11-00306543ECAC"))
            _Col.Add(New GptCodeInfo("ZFS", "6A898CC3-1DD2-11B2-99A6-080020736631"))
            _Col.Add(New GptCodeInfo("Apple RAID partition", "52414944-0000-11AA-AA11-00306543ECAC"))
            _Col.Add(New GptCodeInfo("Apple RAID partition, offline", "52414944-5F4F-11AA-AA11-00306543ECAC"))
            _Col.Add(New GptCodeInfo("Apple Boot partition", "426F6F74-0000-11AA-AA11-00306543ECAC"))
            _Col.Add(New GptCodeInfo("Apple Label", "4C616265-6C00-11AA-AA11-00306543ECAC"))
            _Col.Add(New GptCodeInfo("Apple TV Recovery partition", "5265636F-7665-11AA-AA11-00306543ECAC"))
            _Col.Add(New GptCodeInfo("Apple Core Storage (i.e. Lion FileVault)) partition", "53746F72-6167-11AA-AA11-00306543ECAC"))
            _Col.Add(New GptCodeInfo("Boot partition", "6A82CB45-1DD2-11B2-99A6-080020736631"))
            _Col.Add(New GptCodeInfo("Root partition", "6A85CF4D-1DD2-11B2-99A6-080020736631"))
            _Col.Add(New GptCodeInfo("Swap partition", "6A87C46F-1DD2-11B2-99A6-080020736631"))
            _Col.Add(New GptCodeInfo("Backup partition", "6A8B642B-1DD2-11B2-99A6-080020736631"))
            _Col.Add(New GptCodeInfo("/usr partition", "6A898CC3-1DD2-11B2-99A6-080020736631"))
            _Col.Add(New GptCodeInfo("/var partition", "6A8EF2E9-1DD2-11B2-99A6-080020736631"))
            _Col.Add(New GptCodeInfo("/home partition", "6A90BA39-1DD2-11B2-99A6-080020736631"))
            _Col.Add(New GptCodeInfo("Alternate sector", "6A9283A5-1DD2-11B2-99A6-080020736631"))
            _Col.Add(New GptCodeInfo("Reserved partition", "6A945A3B-1DD2-11B2-99A6-080020736631"))
            _Col.Add(New GptCodeInfo("Reserved", "6A9630D1-1DD2-11B2-99A6-080020736631"))
            _Col.Add(New GptCodeInfo("Reserved", "6A980767-1DD2-11B2-99A6-080020736631"))
            _Col.Add(New GptCodeInfo("Reserved", "6A96237F-1DD2-11B2-99A6-080020736631"))
            _Col.Add(New GptCodeInfo("Reserved", "6A8D2AC7-1DD2-11B2-99A6-080020736631"))
            _Col.Add(New GptCodeInfo("Swap partition", "49F48D32-B10E-11DC-B99B-0019D1879648"))
            _Col.Add(New GptCodeInfo("FFS partition", "49F48D5A-B10E-11DC-B99B-0019D1879648"))
            _Col.Add(New GptCodeInfo("LFS partition", "49F48D82-B10E-11DC-B99B-0019D1879648"))
            _Col.Add(New GptCodeInfo("RAID partition", "49F48DAA-B10E-11DC-B99B-0019D1879648"))
            _Col.Add(New GptCodeInfo("Concatenated partition", "2DB519C4-B10F-11DC-B99B-0019D1879648"))
            _Col.Add(New GptCodeInfo("Encrypted partition", "2DB519EC-B10F-11DC-B99B-0019D1879648"))
            _Col.Add(New GptCodeInfo("ChromeOS kernel", "FE3A2A5D-4F32-41A7-B725-ACCC3285A309"))
            _Col.Add(New GptCodeInfo("ChromeOS rootfs", "3CB8E202-3B7E-47DD-8A3C-7FF2A13CFCEC"))
            _Col.Add(New GptCodeInfo("ChromeOS future use", "2E0A753D-9E48-43B0-8337-B15192CB1B5E"))
            _Col.Add(New GptCodeInfo("Haiku BFS", "42465331-3BA3-10F1-802A-4861696B7521"))
            _Col.Add(New GptCodeInfo("Boot partition", "85D5E45E-237C-11E1-B4B3-E89A8F7FC3A7"))
            _Col.Add(New GptCodeInfo("Data partition", "85D5E45A-237C-11E1-B4B3-E89A8F7FC3A7"))
            _Col.Add(New GptCodeInfo("Swap partition", "85D5E45B-237C-11E1-B4B3-E89A8F7FC3A7"))
            _Col.Add(New GptCodeInfo("Unix File System (UFS) partition", "0394EF8B-237E-11E1-B4B3-E89A8F7FC3A7"))
            _Col.Add(New GptCodeInfo("Vinum volume manager partition", "85D5E45C-237C-11E1-B4B3-E89A8F7FC3A7"))
            _Col.Add(New GptCodeInfo("ZFS partition", "85D5E45D-237C-11E1-B4B3-E89A8F7FC3A7"))

        End Sub

#End Region

    End Class


    ''' <summary>
    ''' Contains the master list of all known partition types as PartitionCodeInfo objects.
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class PartitionCodeInfo

        Private Shared _Col As New List(Of PartitionCodeInfo)

        Private _PartitionID As FriendlyPartitionId
        Private _Occurrence As PartitionOccurrence
        Private _Access As PartitionAccess
        Private _Bootability As PartitionBootability
        Private _Characteristics As PartitionCharacteristics
        Private _Origins As String()
        Private _OS As String()
        Private _Description As String
        Private _Name As String

        ''' <summary>
        ''' Initialize a new instance of the PartitionCodeInfo object.
        ''' </summary>
        ''' <param name="s">Parsing data with which to initialize the object.</param>
        ''' <remarks></remarks>
        Private Sub New(Optional s As String = Nothing)
            If s IsNot Nothing Then Parse(s)
        End Sub

        ''' <summary>
        ''' Partition byte Id.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PartitionID As FriendlyPartitionId
            Get
                Return _PartitionID
            End Get
            Private Set(value As FriendlyPartitionId)
                _PartitionID = value
            End Set
        End Property

        ''' <summary>
        ''' Partition occurrence.  Describes where on the disk a partition entry could be found.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Occurrence As PartitionOccurrence
            Get
                Return _Occurrence
            End Get
            Private Set(value As PartitionOccurrence)
                _Occurrence = value
            End Set
        End Property

        ''' <summary>
        ''' Specifies the kind of hardware access the partition type supports.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Access As PartitionAccess
            Get
                Return _Access
            End Get
            Private Set(value As PartitionAccess)
                _Access = value
            End Set
        End Property

        ''' <summary>
        ''' Specifies the partition type's bootability.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Bootability As PartitionBootability
            Get
                Return _Bootability
            End Get
            Private Set(value As PartitionBootability)
                _Bootability = value
            End Set
        End Property

        ''' <summary>
        ''' Specifies the partition type's characteristics.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Characteristics As PartitionCharacteristics
            Get
                Return _Characteristics
            End Get
            Private Set(value As PartitionCharacteristics)
                _Characteristics = value
            End Set
        End Property

        ''' <summary>
        ''' Specifies the company or companies of origin.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Origins As String()
            Get
                Return _Origins
            End Get
            Private Set(value As String())
                _Origins = value
            End Set
        End Property

        ''' <summary>
        ''' Specifies the supported operating systems.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SupporedOSes As String()
            Get
                Return _OS
            End Get
            Private Set(value As String())
                _OS = value
            End Set
        End Property

        ''' <summary>
        ''' Provides a description of the partition type.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Description As String
            Get
                Return _Description
            End Get
            Private Set(value As String)
                _Description = value
            End Set
        End Property

        ''' <summary>
        ''' Provides the name of the partition type (if different than description).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Name As String
            Get
                Return _Name
            End Get
        End Property

        ''' <summary>
        ''' Converts this object into its string representation.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function ToString() As String

            If Name Is Nothing Then
                If _OS Is Nothing OrElse _OS.Count = 0 Then
                    If Origins IsNot Nothing AndAlso Origins.Length > 0 Then
                        Return Origins(0)
                    Else
                        Return PartitionID.ToString
                    End If
                Else
                    Return String.Join(", ", _OS)
                End If
            Else
                Return Name
            End If

        End Function

        ''' <summary>
        ''' Parse the data into the new instance from one of the entries in the private partition type cache.
        ''' </summary>
        ''' <param name="s"></param>
        ''' <remarks></remarks>
        Private Sub Parse(s As String)

            Dim vs() As String = TextTools.Split(s, ",", True, True, """"c, """"c, False, False, True)

            Dim i As Integer
            Dim c As Integer = vs.Length - 1

            For i = 0 To c
                vs(i) = vs(i).Trim
            Next

            _PartitionID.Value = CByte(Val(vs(0)))
            _Occurrence = CType(FlagsParse(Of PartitionOccurrence)(vs(1)), PartitionOccurrence)
            _Access = CType(FlagsParse(Of PartitionAccess)(vs(2)), PartitionAccess)
            _Bootability = _internalParseBootFlags(vs(3))
            _Characteristics = CType(FlagsParse(Of PartitionCharacteristics)(vs(4)), PartitionCharacteristics)
            _Origins = TextTools.Split(vs(5), ", ")

            If vs.Length >= 7 Then
                _OS = TextTools.Split(vs(6), ", ")

                If vs.Length >= 8 Then
                    _Description = vs(7)
                    _Name = vs(7)
                End If
            End If

        End Sub

        ''' <summary>
        ''' Returns a list of all known partition types as PartitionInfo objects.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Codes As List(Of PartitionCodeInfo)
            Get
                Return _Col
            End Get
        End Property

        ''' <summary>
        ''' Finds all partition type information objects that match the specified code.
        ''' </summary>
        ''' <param name="code">Byte code of the partition type to search.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function FindByCode(code As Byte) As PartitionCodeInfo()

            Dim l As New List(Of PartitionCodeInfo)
            For Each pt In _Col
                If pt.PartitionID.Value = code Then
                    l.Add(pt)
                End If
            Next

            Return l.ToArray
        End Function

        Private Shared _AllOSes() As String

        ''' <summary>
        ''' Returns a list of all operating systems listed by the MBR partition types.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property AllOSes As String()
            Get
                Return _AllOSes
            End Get
        End Property

        Shared Sub New()

            Dim v() As String = _internalPopulationInfo()

            For Each l In v
                _Col.Add(New PartitionCodeInfo(l))
            Next

            _Col.Sort(New PartCodeSorter)

            Dim vl As New List(Of String)

            For Each c In _Col
                If c.SupporedOSes Is Nothing Then Continue For
                For Each s In c.SupporedOSes
                    If s Is Nothing Then Continue For
                    If vl.Contains(s) = False Then
                        vl.Add(s)
                    End If
                Next
            Next

            vl.Sort()
            _AllOSes = vl.ToArray

        End Sub

#Region "Internal Partition List Information Population"

        Private Shared Function _internalParseBootFlags(s As String) As PartitionBootability

            If String.IsNullOrEmpty(s) Then Return PartitionBootability.NoInfo

            Dim vs() As String = TextTools.Split(s, ", ")

            Dim vl As PartitionBootability = PartitionBootability.NoInfo
            Dim sl() As String = [Enum].GetNames(vl.GetType)

            For Each x In vs

                If sl.Contains(x) Then
                    vl = vl Or CType([Enum].Parse(vl.GetType, x), PartitionBootability)
                ElseIf sl.Contains("M" & x) Then
                    vl = vl Or CType([Enum].Parse(vl.GetType, "M" & x), PartitionBootability)

                ElseIf sl.Contains("I" & x) Then
                    vl = vl Or CType([Enum].Parse(vl.GetType, "I" & x), PartitionBootability)
                ElseIf x = "8080/Z80" Then
                    vl = vl Or PartitionBootability.I8080Z80
                End If
            Next

            Return vl

        End Function

        ''' <summary>
        ''' Returns a string array of comma-seperated partition information entries.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function _internalPopulationInfo() As String()

            '' This list was adapted from the Wikipedia article: http://en.wikipedia.org/wiki/Partition_type

            Dim v As New List(Of String)

            v.Add("""&H00"",""MBR, EBR"",""N/A"",""No"",""Free"",""IBM"",""All"",""Empty partition entry""")
            v.Add("""&H01"",""MBR, EBR"",""CHS, LBA"",""x86, 68000, 8080/Z80"",""Filesystem"",""IBM"",""DOS 2.0+"",""FAT12 as primary partition in first physical 32 MB of disk or as logical drive anywhere on disk (else use 06h instead)""")
            v.Add("""&H02"",""MBR"",""CHS"","""","""",""Microsoft, SCO"",""XENIX"",""XENIX root""")
            v.Add("""&H03"",""MBR"",""CHS"","""","""",""Microsoft, SCO"",""XENIX"",""XENIX usr""")
            v.Add("""&H04"",""MBR, EBR"",""CHS, LBA"",""x86, 68000, 8080/Z80"",""Filesystem"",""Microsoft"",""DOS 3.0+"",""FAT16 with less than 65536 sectors (32 MB). As primary partition it must reside in first physical 32 MB of disk, or as logical drive anywhere on disk (else use 06h instead).""")
            v.Add("""&H05"",""MBR, EBR"",""CHS, (LBA)"",""No, AAP"",""Container"",""IBM"",""DOS (3.2) 3.3+"",""Extended partition with CHS addressing. It must reside in first physical 8 GB of disk, else use 0Fh instead""")
            v.Add("""&H06"",""MBR, EBR"",""CHS, LBA"",""x86"",""Filesystem"",""Compaq"",""DOS 3.31+"",""FAT16B with 65536 or more sectors. It must reside in first physical 8 GB of disk, unless used for logical drives in an 0Fh extended partition (else use 0Eh instead). Also used for FAT12 and FAT16 volumes in primary partitions if they are not residing in first physical 32 MB of disk.""")
            v.Add("""&H07"",""MBR, EBR"",""CHS, LBA"",""x86"",""Filesystem"",""Microsoft, IBM"",""OS/2"",""IFS""")
            v.Add("""&H07"",""MBR, EBR"",""CHS, LBA"",""286"",""Filesystem"",""IBM"",""OS/2, Windows NT"",""HPFS""")
            v.Add("""&H07"",""MBR, EBR"",""CHS, LBA"",""386"",""Filesystem"",""Microsoft"",""Windows NT"",""NTFS""")
            v.Add("""&H07"",""MBR, EBR"",""CHS, LBA"",""Yes"",""Filesystem"",""Microsoft"",""Windows Embedded CE"",""exFAT""")
            v.Add("""&H07"","""","""","""","""","""","""",""Advanced Unix""")
            v.Add("""&H07"","""","""","""","""",""Quantum Software Systems"",""QNX 2"",""QNX 'qnx' (7) (pre-1988 only)""")
            v.Add("""&H08"",""MBR"",""CHS"",""x86"",""Filesystem"",""Commodore"",""Commodore MS-DOS 3.x"",""Logical sectored FAT12 or FAT16""")
            v.Add("""&H08"","""",""CHS"",""x86"",""Filesystem"",""IBM"",""OS/2 1.0-1.3"",""OS/2""")
            v.Add("""&H08"","""","""","""","""",""IBM"",""AIX"",""AIX boot/split""")
            v.Add("""&H08"","""","""","""","""","""","""",""SplitDrive""")
            v.Add("""&H08"","""","""","""","""",""Quantum Software Systems"",""QNX 1.x/2.x"",""QNX 'qny' (8)""")
            v.Add("""&H08"","""","""","""","""",""Dell"","""",""partition spanning multiple drives""")
            v.Add("""&H09"","""","""","""","""",""IBM"",""AIX"",""AIX data/boot""")
            v.Add("""&H09"","""","""","""","""",""Quantum Software Systems"",""QNX 1.x/2.x"",""QNX 'qnz' (9)""")
            v.Add("""&H09"",""MBR"",""CHS"",""286"",""Filesystem"",""Mark Williams Company"",""Coherent"",""Coherent file system""")
            v.Add("""&H09"",""MBR"","""","""",""Filesystem"",""Microware"",""OS-9"",""OS-9 RBF""")
            v.Add("""&H0A"","""","""","""","""",""PowerQuest, IBM"",""OS/2"",""OS/2 Boot Manager""")
            v.Add("""&H0A"","""","""","""","""",""Mark Williams Company"",""Coherent"",""Coherent swap partition""")
            v.Add("""&H0A"","""","""","""","""",""Unisys"",""OPUS"",""Open Parallel Unisys Server""")
            v.Add("""&H0B"",""MBR, EBR"",""CHS, LBA"",""x86"",""Filesystem"",""Microsoft"",""DOS 7.1+"",""FAT32 with CHS addressing""")
            v.Add("""&H0C"",""MBR, EBR"",""LBA"",""x86"",""Filesystem"",""Microsoft"",""DOS 7.1+"",""FAT32X with LBA""")
            v.Add("""&H0E"",""MBR, EBR"",""LBA"",""x86"",""Filesystem"",""Microsoft"",""DOS 7.0+"",""FAT16X with LBA""")
            v.Add("""&H0F"",""MBR, EBR"",""LBA"",""No, AAP"",""Container"",""Microsoft"",""DOS 7.0+"",""Extended partition with LBA""")
            v.Add("""&H10"","""","""","""","""",""Unisys"",""OPUS"",""""")
            v.Add("""&H11"",""MBR"",""CHS"",""x86"",""Filesystem"",""Leading Edge"",""Leading Edge MS-DOS 3.x"",""Logical sectored FAT12 or FAT16""")
            v.Add("""&H11"","""","""","""",""Hidden, Filesystem"",""IBM"",""OS/2 Boot Manager"",""Hidden FAT12 (corresponds with 01h)""")
            v.Add("""&H12"",""MBR"",""CHS, LBA"",""x86"",""Service, Filesystem"",""Compaq"","""",""configuration partition (bootable FAT)""")
            v.Add("""&H12"","""","""","""",""Hibernation"",""Compaq"",""Compaq Contura"",""hibernation partition""")
            v.Add("""&H12"",""MBR"","""",""x86"",""Service, Filesystem"",""NCR"","""",""diagnostics and firmware partition (bootable FAT)""")
            v.Add("""&H12"",""MBR"","""",""x86"",""Service, Filesystem"",""Intel"","""",""service partition (bootable FAT)""")
            v.Add("""&H12"","""","""","""",""Service"",""IBM"","""",""Rescue and Recovery partition""")
            v.Add("""&H14"","""","""","""",""Filesystem"",""AST"",""AST MS-DOS 3.x"",""Logical sectored FAT12 or FAT16""")
            v.Add("""&H14"","""","""",""x86, 68000, 8080/Z80"",""Hidden, Filesystem"",""IBM"",""OS/2 Boot Manager"",""Hidden FAT16 (corresponds with 04h)""")
            v.Add("""&H14"","""",""LBA"","""",""Filesystem"","""",""Maverick OS"",""Omega filesystem""")
            v.Add("""&H15"","""","""",""No, AAP"",""Hidden, Container"",""IBM"",""OS/2 Boot Manager"",""Hidden extended partition with CHS addressing (corresponds with 05h)""")
            v.Add("""&H15"","""",""LBA"","""","""","""",""Maverick OS"",""swap""")
            v.Add("""&H16"","""","""",""x86, 68000, 8080/Z80"",""Hidden, Filesystem"",""IBM"",""OS/2 Boot Manager"",""Hidden FAT16B (corresponds with 06h)""")
            v.Add("""&H17"","""","""","""",""Hidden, Filesystem"",""IBM"",""OS/2 Boot Manager"",""Hidden IFS (corresponds with 07h)""")
            v.Add("""&H17"","""","""","""","""","""","""",""Hidden HPFS (corresponds with 07h)""")
            v.Add("""&H17"","""","""","""","""","""","""",""Hidden NTFS (corresponds with 07h)""")
            v.Add("""&H17"","""","""","""","""","""","""",""Hidden exFAT (corresponds with 07h)""")
            v.Add("""&H18"","""","""",""No"",""Hibernation"",""AST"",""AST Windows"",""AST Zero Volt Suspend or SmartSleep partition""")
            v.Add("""&H19"","""","""","""","""",""Willow Schlanger"",""Willowtech Photon coS"",""Willowtech Photon coS""")
            v.Add("""&H1B"","""","""","""",""Hidden, Filesystem"",""IBM"",""OS/2 Boot Manager"",""Hidden FAT32 (corresponds with 0Bh)""")
            v.Add("""&H1C"","""","""","""",""Hidden, Filesystem"",""IBM"",""OS/2 Boot Manager"",""Hidden FAT32X with LBA (corresponds with 0Ch)""")
            v.Add("""&H1E"","""","""","""",""Hidden, Filesystem"",""IBM"",""OS/2 Boot Manager"",""Hidden FAT16X with LBA (corresponds with 0Eh)""")
            v.Add("""&H1F"",""MBR, EBR"",""LBA"","""",""Hidden, Container"",""IBM"",""OS/2 Boot Manager"",""Hidden extended partition with LBA addressing (corresponds with 0Fh)""")
            v.Add("""&H20"","""","""","""","""",""Microsoft"",""Windows Mobile"",""Windows Mobile update XIP""")
            v.Add("""&H20"","""","""","""","""",""Willow Schlanger"","""",""Willowsoft Overture File System (OFS1)""")
            v.Add("""&H21"",""MBR"","""","""","""",""Hewlett Packard"","""",""HP Volume Expansion (SpeedStor)""")
            v.Add("""&H21"","""","""","""",""Filesystem"",""Dave Poirier"",""Oxygen"",""FSo2 (Oxygen File System)""")
            v.Add("""&H22"","""","""","""",""Container"",""Dave Poirier"",""Oxygen"",""Oxygen Extended Partition Table""")
            v.Add("""&H23"","""","""","""","""",""Microsoft, IBM"","""",""Reserved""")
            v.Add("""&H23"","""","""",""Yes"","""",""Microsoft"",""Windows Mobile"",""Windows Mobile boot XIP""")
            v.Add("""&H24"",""MBR"",""CHS"",""x86"",""Filesystem"",""NEC"",""NEC MS-DOS 3.30"",""Logical sectored FAT12 or FAT16""")
            v.Add("""&H25"","""","""","""","""",""Microsoft"",""Windows Mobile"",""Windows Mobile IMGFS""")
            v.Add("""&H26"","""","""","""","""",""Microsoft, IBM"","""",""Reserved""")
            v.Add("""&H27"","""","""","""",""Service, Filesystem"",""Microsoft"",""Windows"",""Windows recovery environment (RE) partition (hidden NTFS partition type 07h)""")
            v.Add("""&H27"",""MBR"",""CHS, LBA"",""Yes"",""Hidden, Service, Filesystem"",""Acer"",""PQservice"",""FAT32 or NTFS rescue partition""")
            v.Add("""&H27"","""","""","""","""","""",""MirOS BSD"",""MirOS partition""")
            v.Add("""&H27"","""","""","""","""","""",""RooterBOOT"",""RooterBOOT kernel partition (contains a raw ELF Linux kernel, no filesystem)""")
            v.Add("""&H2A"","""","""","""",""Filesystem"",""Kurt Skauen"",""AtheOS"",""AtheOS file system (AthFS, AFS) (an extension of BFS, see 2Bh and EBh)""")
            v.Add("""&H2B"","""","""","""","""",""Kristian van der Vliet"",""SyllableOS"",""SyllableSecure (SylStor), a variant of AthFS (an extension of BFS, see 2Ah and EBh)""")
            v.Add("""&H31"","""","""","""","""",""Microsoft, IBM"","""",""Reserved""")
            v.Add("""&H32"","""","""","""","""",""Alien Internet Services"",""NOS""")
            v.Add("""&H33"","""","""","""","""",""Microsoft, IBM"","""",""Reserved""")
            v.Add("""&H34"","""","""","""","""",""Microsoft, IBM"","""",""Reserved""")
            v.Add("""&H35"",""MBR, EBR"",""CHS, LBA"",""No"",""Filesystem"",""IBM"",""OS/2 Warp Server / eComStation"",""JFS (OS/2 implementation of AIX Journaling Filesystem)""")
            v.Add("""&H36"","""","""","""","""",""Microsoft, IBM"","""",""Reserved""")
            v.Add("""&H38"","""","""","""","""",""Timothy Williams"",""THEOS"",""THEOS version 3.2, 2 GB partition""")
            v.Add("""&H39"","""","""","""",""Container"",""Bell Labs"",""Plan 9"",""Plan 9 edition 3 partition (sub-partitions described in second sector of partition)""")
            v.Add("""&H39"","""","""","""","""",""Timothy Williams"",""THEOS"",""THEOS version 4 spanned partition""")
            v.Add("""&H3A"","""","""","""","""",""Timothy Williams"",""THEOS"",""THEOS version 4, 4 GB partition""")
            v.Add("""&H3B"","""","""","""","""",""Timothy Williams"",""THEOS"",""THEOS version 4 extended partition""")
            v.Add("""&H3C"","""","""","""","""",""PowerQuest"",""PartitionMagic"",""PqRP (PartitionMagic or DriveImage in progress)""")
            v.Add("""&H3D"","""","""","""",""Hidden, Filesystem"",""PowerQuest"",""PartitionMagic"",""Hidden NetWare""")
            v.Add("""&H3F"","""","""","""","""","""",""OS/32""")
            v.Add("""&H40"","""","""","""","""",""PICK Systems"",""PICK"",""PICK R83""")
            v.Add("""&H40"","""","""","""","""",""VenturCom"",""Venix"",""Venix 80286""")
            v.Add("""&H41"","""","""",""Yes"","""","""",""Personal RISC"",""Personal RISC Boot""")
            v.Add("""&H41"","""","""","""","""",""Linux"",""Linux"",""Old Linux/Minix (disk shared with DR DOS 6.0) (corresponds with 81h)""")
            v.Add("""&H41"","""","""",""PowerPC"","""",""PowerPC"",""PowerPC"",""PPC PReP (Power PC Reference Platform) Boot""")
            v.Add("""&H42"","""","""","""",""Secured, Filesystem"",""Peter Gutmann"",""SFS"",""Secure Filesystem (SFS)""")
            v.Add("""&H42"","""","""",""No"","""",""Linux"",""Linux"",""Old Linux swap (disk shared with DR DOS 6.0) (corresponds with 82h)""")
            v.Add("""&H42"","""","""","""",""Container"",""Microsoft"",""Windows 2000, XP, etc."",""Dynamic extended partition marker""")
            v.Add("""&H43"","""","""",""Yes"",""Filesystem"",""Linux"",""Linux"",""Old Linux native (disk shared with DR DOS 6.0) (corresponds with 83h)""")
            v.Add("""&H44"","""","""","""","""",""Wildfile"",""GoBack"",""Norton GoBack, WildFile GoBack, Adaptec GoBack, Roxio GoBack""")
            v.Add("""&H45"","""","""","""","""",""Priam"","""",""Priam""")
            v.Add("""&H45"",""MBR"",""CHS"",""Yes"","""","""",""Boot-US"",""Boot-US boot manager (1 cylinder)""")
            v.Add("""&H45"","""","""","""","""",""Jochen Liedtke, GMD"",""EUMEL/ELAN"",""EUMEL/ELAN (L2)""")
            v.Add("""&H46"","""","""","""","""",""Jochen Liedtke, GMD"",""EUMEL/ELAN"",""EUMEL/ELAN (L2)""")
            v.Add("""&H47"","""","""","""","""",""Jochen Liedtke, GMD"",""EUMEL/ELAN"",""EUMEL/ELAN (L2)""")
            v.Add("""&H48"","""","""","""","""",""Jochen Liedtke, GMD"",""EUMEL/ELAN"",""EUMEL/ELAN (L2)""")
            v.Add("""&H48"","""","""","""","""",""ERGOS"",""ERGOS L3"",""ERGOS L3""")
            v.Add("""&H4A"",""MBR"","""",""Yes"","""",""Nick Roberts"",""AdaOS"",""Aquila""")
            v.Add("""&H4A"",""MBR, EBR"",""CHS, LBA"",""No"",""Filesystem"",""Mark Aitchison"",""ALFS/THIN"",""ALFS/THIN advanced lightweight filesystem for DOS""")
            v.Add("""&H4C"","""","""","""","""",""ETH Zürich"",""ETH Oberon"",""Aos (A2) filesystem (76)""")
            v.Add("""&H4D"","""","""","""","""",""Quantum Software Systems"",""QNX 4.x, Neutrino"",""Primary QNX POSIX volume on disk (77)""")
            v.Add("""&H4E"","""","""","""","""",""Quantum Software Systems"",""QNX 4.x, Neutrino"",""Secondary QNX POSIX volume on disk (78)""")
            v.Add("""&H4F"","""","""","""","""",""Quantum Software Systems"",""QNX 4.x, Neutrino"",""Tertiary QNX POSIX volume on disk (79)""")
            v.Add("""&H4F"","""","""",""Yes"","""",""ETH Zürich"",""ETH Oberon"",""boot / native filesystem (79)""")
            v.Add("""&H50"","""","""","""","""",""ETH Zürich"",""ETH Oberon"",""Alternative native filesystem (80)""")
            v.Add("""&H50"","""","""",""No"","""",""OnTrack"",""Disk Manager 4"",""Read-only partition (old)""")
            v.Add("""&H50"","""","""","""","""","""",""LynxOS"",""Lynx RTOS""")
            v.Add("""&H50"","""","""","""","""","""","""",""Novell""")
            v.Add("""&H51"","""","""","""","""",""Novell"",""""")
            v.Add("""&H50"","""","""",""No"","""",""OnTrack"",""Disk Manager 4-6"",""Read-write partition (Aux 1)""")
            v.Add("""&H52"","""","""","""","""","""",""CP/M"",""CP/M""")
            v.Add("""&H52"","""","""","""","""","""",""Microport"",""System V/AT, V/386""")
            v.Add("""&H53"","""","""","""","""",""OnTrack"",""Disk Manager 6"",""Auxiliary 3 (WO)""")
            v.Add("""&H54"","""","""","""","""",""OnTrack"",""Disk Manager 6"",""Dynamic Drive Overlay (DDO)""")
            v.Add("""&H55"","""","""","""","""",""MicroHouse / StorageSoft"",""EZ-Drive"",""EZ-Drive, Maxtor, MaxBlast, or DriveGuide INT 13h redirector volume""")
            v.Add("""&H56"","""","""","""","""",""AT&T"",""AT&T MS-DOS 3.x"",""Logical sectored FAT12 or FAT16""")
            v.Add("""&H56"","""","""","""","""",""MicroHouse / StorageSoft"",""EZ-Drive"",""Disk Manager partition converted to EZ-BIOS""")
            v.Add("""&H56"","""","""","""","""",""Golden Bow"",""VFeature"",""VFeature partitionned volume""")
            v.Add("""&H57"","""","""","""","""",""MicroHouse / StorageSoft"",""DrivePro""")
            v.Add("""&H56"","""","""","""","""",""Novell"","""",""VNDI partition""")
            v.Add("""&H5C"","""","""","""",""Container"",""Priam"",""EDISK"",""Priam EDisk Partitioned Volume""")
            v.Add("""&H5D"",""MBR, EBR"",""CHS, LBA"",""x86"",""Policy"","""",""APTI (Alternative Partition Table Identification) conformant systems"","" APTI alternative partition""")
            v.Add("""&H5E"",""MBR, EBR"",""LBA"",""No, AAP"",""Policy, Container"","""",""APTI conformant systems"","" APTI alternative extended partition (corresponds with 0Fh)""")
            v.Add("""&H5F"",""MBR, EBR"",""CHS"",""No, AAP"",""Policy, Container"","""",""APTI conformant systems"","" APTI alternative extended partition (< 8 GB) (corresponds with 05h)""")
            v.Add("""&H61"","""","""","""","""",""Storage Dimensions"",""SpeedStor""")
            v.Add("""&H63"","""",""CHS"","""",""Filesystem"","""",""Unix"",""SCO Unix, ISC, UnixWare, AT&T System V/386, ix, MtXinu BSD 4.3 on Mach, GNU HURD""")
            v.Add("""&H64"","""","""","""","""",""Storage Dimensions"",""SpeedStor""")
            v.Add("""&H63"","""","""","""",""Filesystem"",""Novell"",""NetWare"",""NetWare File System 286/2""")
            v.Add("""&H63"","""","""","""","""",""Solomon"","""",""PC-ARMOUR""")
            v.Add("""&H65"","""","""","""",""Filesystem"",""Novell"",""NetWare"",""NetWare File System 386""")
            v.Add("""&H66"","""","""","""",""Filesystem"",""Novell"",""NetWare"",""NetWare File System 386""")
            v.Add("""&H66"","""","""","""","""",""Novell"",""NetWare"",""Storage Management Services (SMS)""")
            v.Add("""&H67"","""","""","""","""",""Novell"",""NetWare"",""Wolf Mountain""")
            v.Add("""&H68"","""","""","""","""",""Novell"",""NetWare""")
            v.Add("""&H69"","""","""","""","""",""Novell"",""NetWare 5""")
            v.Add("""&H67"","""","""","""","""",""Novell"",""NetWare"",""Novell Storage Services (NSS)""")
            v.Add("""&H70"","""","""","""","""","""",""DiskSecure"",""DiskSecure multiboot""")
            v.Add("""&H71"","""","""","""","""",""Microsoft, IBM"","""",""Reserved""")
            v.Add("""&H72"",""MBR, EBR"",""CHS"",""x86"",""Policy, Filesystem"","""",""APTI conformant systems"",""APTI alternative FAT12 (CHS, SFN) (corresponds with 01h)""")
            v.Add("""&H72"","""","""","""","""",""Nordier"",""Unix V7/x86"",""V7/x86""")
            v.Add("""&H73"","""","""","""","""",""Microsoft, IBM"","""",""Reserved""")
            v.Add("""&H74"","""","""","""","""",""Microsoft, IBM"","""",""Reserved""")
            v.Add("""&H74"","""","""","""",""Secured"","""","""",""Scramdisk""")
            v.Add("""&H75"","""","""","""","""",""IBM"",""PC/IX""")
            v.Add("""&H76"","""","""","""","""",""Microsoft, IBM"","""",""Reserved""")
            v.Add("""&H77"","""","""","""",""Filesystem"",""Novell"","""",""VNDI, M2FS, M2CS""")
            v.Add("""&H78"","""","""",""Yes"",""Filesystem"",""Geurt Vos"","""",""XOSL bootloader filesystem""")
            v.Add("""&H79"",""MBR, EBR"",""CHS"",""x86"",""Policy, Filesystem"","""",""APTI conformant systems"",""APTI alternative FAT16 (CHS, SFN) (corresponds with 04h)""")
            v.Add("""&H7A"",""MBR, EBR"",""LBA"",""x86"",""Policy, Filesystem"","""",""APTI conformant systems"",""APTI alternative FAT16X (LBA, SFN) (corresponds with 0Dh)""")
            v.Add("""&H7B"",""MBR, EBR"",""CHS"",""x86"",""Policy, Filesystem"","""",""APTI conformant systems"",""APTI alternative FAT16B (CHS, SFN) (corresponds with 06h)""")
            v.Add("""&H7C"",""MBR, EBR"",""LBA"",""x86"",""Policy, Filesystem"","""",""APTI conformant systems"",""APTI alternative FAT32X (LBA, SFN) (corresponds with 0Ch)""")
            v.Add("""&H7D"",""MBR, EBR"",""CHS"",""x86"",""Policy, Filesystem"","""",""APTI conformant systems"",""APTI alternative FAT32 (CHS, SFN) (corresponds with 0Bh)""")
            v.Add("""&H7E"","""","""","""","""","""",""F.I.X.""")
            v.Add("""&H7F"",""MBR, EBR"",""CHS, LBA"",""Yes"","""",""AODPS"",""Varies"","" Alternative OS Development Partition Standard - reserved for individual or local use and temporary or experimental projects""")
            v.Add("""&H80"","""","""","""",""Filesystem"",""Andrew Tanenbaum"",""Minix 1.1-1.4a"",""Minix file system (old)""")
            v.Add("""&H81"","""","""","""",""Filesystem"",""Andrew Tanenbaum"",""Minix 1.4b+"",""MINIX file system (corresponds with 41h)""")
            v.Add("""&H81"","""","""","""","""","""",""Linux"",""Mitac Advanced Disk Manager""")
            v.Add("""&H82"","""","""",""No"","""",""GNU/Linux"","""",""Linux swap space (corresponds with 42h)""")
            v.Add("""&H82"","""","""",""x86"",""Container"",""Sun Microsystems"","""",""Solaris x86 (for Sun disklabels up to 2005)""")
            v.Add("""&H82"","""","""","""","""","""","""",""Prime""")
            v.Add("""&H83"","""","""","""",""Filesystem"",""GNU/Linux"","""",""Any native Linux file system""")
            v.Add("""&H84"","""","""",""No"",""Hibernation"",""Microsoft"","""",""APM hibernation (suspend to disk, S2D)""")
            v.Add("""&H84"","""","""","""",""Hidden, Filesystem"",""IBM"",""OS/2"",""Hidden C: (FAT16)""")
            v.Add("""&H84"","""","""","""",""Hibernation"",""Intel"",""Windows 7"",""Rapid Start technology""")
            v.Add("""&H85"","""","""",""No, AAP"",""Container"",""GNU/Linux"","""",""Linux extended (corresponds with 05h)""")
            v.Add("""&H86"","""","""","""",""Filesystem"",""Microsoft"",""Windows NT 4 Server"",""Fault-tolerant FAT16B mirrored volume set""")
            v.Add("""&H86"","""","""","""","""",""GNU/Linux"",""Linux"",""Linux RAID superblock with auto-detect (old)""")
            v.Add("""&H87"","""","""","""",""Filesystem"",""Microsoft"",""Windows NT 4 Server"",""Fault-tolerant HPFS/NTFS mirrored volume set""")
            v.Add("""&H88"","""","""","""","""",""GNU/Linux"","""",""Linux plaintext partition table""")
            v.Add("""&H8A"","""","""","""","""",""Martin Kiewitz"",""AiR-BOOT"",""Linux kernel image""")
            v.Add("""&H8B"","""","""","""",""Filesystem"",""Microsoft"",""Windows NT 4 Server"",""Legacy fault-tolerant FAT32 mirrored volume set""")
            v.Add("""&H8C"","""","""","""",""Filesystem"",""Microsoft"",""Windows NT 4 Server"",""Legacy fault-tolerant FAT32X mirrored volume set""")
            v.Add("""&H8D"",""MBR, EBR"",""CHS, LBA"",""x86, 68000, 8080/Z80"",""Hidden, Filesystem"",""FreeDOS"",""Free FDISK"",""Hidden FAT12 (corresponds with 01h)""")
            v.Add("""&H8E"","""","""","""","""",""GNU/Linux"",""Linux"",""Linux LVM""")
            v.Add("""&H90"",""MBR, EBR"",""CHS, LBA"",""x86, 68000, 8080/Z80"",""Hidden, Filesystem"",""FreeDOS"",""Free FDISK"",""Hidden FAT16 (corresponds with 04h)""")
            v.Add("""&H91"",""MBR, EBR"",""CHS, LBA"",""No, AAP"",""Hidden, Container"",""FreeDOS"",""Free FDISK"",""Hidden extended partition with CHS addressing (corresponds with 05h)""")
            v.Add("""&H92"",""MBR, EBR"",""CHS, LBA"",""x86"",""Hidden, Filesystem"",""FreeDOS"",""Free FDISK"",""Hidden FAT16B (corresponds with 06h)""")
            v.Add("""&H93"","""","""","""",""Filesystem"","""",""Amoeba"",""Amoeba native filesystem""")
            v.Add("""&H93"","""","""","""",""Hidden, Filesystem"","""",""Linux"",""Hidden Linux filesystem""")
            v.Add("""&H94"","""","""","""","""","""",""Amoeba"",""Amoeba bad block table""")
            v.Add("""&H95"","""","""","""","""",""MIT"",""EXOPC"",""EXOPC native""")
            v.Add("""&H96"","""","""","""",""Filesystem"","""",""CHRP"",""ISO-9660 filesystem""")
            v.Add("""&H97"",""MBR, EBR"",""CHS, LBA"",""x86"",""Hidden, Filesystem"",""FreeDOS"",""Free FDISK"",""Hidden FAT32 (corresponds with 0Bh)""")
            v.Add("""&H98"",""MBR, EBR"",""LBA"",""x86"",""Hidden, Filesystem"",""FreeDOS"",""Free FDISK"",""Hidden FAT32X (corresponds with 0Ch)""")
            v.Add("""&H98"",""MBR"",""CHS, LBA"",""x86"",""Hidden, Service, Filesystem"",""Datalight"",""ROM-DOS"",""service partition (bootable FAT) ROM-DOS SuperBoot""")
            v.Add("""&H98"",""MBR"",""CHS, LBA"",""x86"",""Hidden, Service, Filesystem"",""Intel"","""",""service partition (bootable FAT)""")
            v.Add("""&H99"","""","""","""",""Filesystem"","""",""early Unix""")
            v.Add("""&H98"","""","""","""",""Container"",""Mylex"",""DCE376"",""EISA SCSI (> 1024)""")
            v.Add("""&H9A"",""MBR, EBR"",""LBA"",""x86"",""Hidden, Filesystem"",""FreeDOS"",""Free FDISK"",""Hidden FAT16X (corresponds with 0Eh)""")
            v.Add("""&H9B"",""MBR, EBR"",""LBA"",""No, AAP"",""Hidden, Container"",""FreeDOS"",""Free FDISK"",""Hidden extended partition with LBA (corresponds with 0Fh)""")
            v.Add("""&H9E"","""","""","""","""",""Andy Valencia"",""VSTA""")
            v.Add("""&H9B"","""","""","""","""",""Andy Valencia"",""ForthOS"",""ForthOS (eForth port)""")
            v.Add("""&H9F"","""","""","""","""","""",""BSD/OS 3.0+, BSDI"",""""")
            v.Add("""&HA0"",""MBR"","""","""",""Service"",""Hewlett Packard"","""",""Diagnostic partition for HP laptops""")
            v.Add("""&HA0"","""","""","""",""Hibernation"",""Phoenix, IBM, Toshiba, Sony"","""",""Hibernate partition""")
            v.Add("""&HA1"","""","""","""","""",""Hewlett Packard"","""",""HP Volume Expansion (SpeedStor)""")
            v.Add("""&HA1"","""","""","""",""Hibernation"",""Phoenix, NEC"","""",""Hibernate partition""")
            v.Add("""&HA3"","""","""","""","""",""Hewlett Packard"","""",""HP Volume Expansion (SpeedStor)""")
            v.Add("""&HA4"","""","""","""","""",""Hewlett Packard"","""",""HP Volume Expansion (SpeedStor)""")
            v.Add("""&HA5"",""MBR"","""","""",""Container"",""FreeBSD"",""BSD"",""BSD slice (BSD/386, 386BSD, NetBSD (old), FreeBSD)""")
            v.Add("""&HA6"","""","""","""","""",""Hewlett Packard"","""",""HP Volume Expansion (SpeedStor)""")
            v.Add("""&HA6"",""MBR"","""","""",""Container"",""OpenBSD"",""OpenBSD"",""OpenBSD slice""")
            v.Add("""&HA7"","""","""",""386"",""Filesystem"",""NeXT"","""",""NeXTSTEP""")
            v.Add("""&HA8"","""","""","""",""Filesystem"",""Apple"",""Darwin, Mac OS X"",""Apple Darwin, Mac OS X UFS""")
            v.Add("""&HA9"",""MBR"","""","""",""Container"",""NetBSD"",""NetBSD"",""NetBSD slice""")
            v.Add("""&HAA"",""MBR"",""CHS"","""",""Service, Image"",""Olivetti"",""MS-DOS"",""Olivetti MS-DOS FAT12 (1.44 MB)""")
            v.Add("""&HAB"","""","""",""Yes"","""",""Apple"",""Darwin, Mac OS X"",""Apple Darwin, Mac OS X boot""")
            v.Add("""&HAB"","""","""","""","""",""Stanislav Karchebny"",""GO! OS"",""GO!""")
            v.Add("""&HAD"","""","""","""",""Filesystem"",""Ben Avison, Acorn"",""RISC OS"",""ADFS / FileCore format""")
            v.Add("""&HAE"","""","""",""x86"",""Filesystem"",""Frank Barrus"",""ShagOS"",""ShagOS file system""")
            v.Add("""&HAF"","""","""","""","""",""Apple"","""",""Apple Mac OS X HFS and HFS+""")
            v.Add("""&HAF"","""","""",""No"","""",""Frank Barrus"",""ShagOS"",""ShagOS swap""")
            v.Add("""&HB0"",""MBR"",""CHS, LBA"",""x86"",""Blocker"",""Star-Tools"",""Boot-Star"",""Boot-Star dummy partition""")
            v.Add("""&HB1"","""","""","""","""",""Hewlett Packard"","""",""HP Volume Expansion (SpeedStor)""")
            v.Add("""&HB1"","""","""","""","""",""QNX Software Systems"",""QNX 6.x"",""QNX Neutrino power-safe file system""")
            v.Add("""&HB2"","""","""","""","""",""QNX Software Systems"",""QNX 6.x"",""QNX Neutrino power-safe file system""")
            v.Add("""&HB3"","""","""","""","""",""Hewlett Packard"","""",""HP Volume Expansion (SpeedStor)""")
            v.Add("""&HB3"","""","""","""","""",""QNX Software Systems"",""QNX 6.x"",""QNX Neutrino power-safe file system""")
            v.Add("""&HB4"","""","""","""","""",""Hewlett Packard"","""",""HP Volume Expansion (SpeedStor)""")
            v.Add("""&HB6"","""","""","""","""",""Hewlett Packard"","""",""HP Volume Expansion (SpeedStor)""")
            v.Add("""&HB6"",""EBR"","""","""","""",""Microsoft"",""Windows NT 4 Server"",""Corrupted fault-tolerant FAT16B mirrored master volume""")
            v.Add("""&HB7"","""","""","""",""Filesystem"","""",""BSDI (before 3.0)"",""BSDI native filesystem / swap""")
            v.Add("""&HB7"",""EBR"","""","""","""",""Microsoft"",""Windows NT 4 Server"",""Corrupted fault-tolerant HPFS/NTFS mirrored master volume""")
            v.Add("""&HB8"","""","""","""",""Filesystem"","""",""BSDI (before 3.0)"",""BSDI swap / native filesystem""")
            v.Add("""&HBB"","""","""","""",""Hidden, (Filesystem)"",""PhysTechSoft, Acronis, SWsoft"",""BootWizard, OS Selector"",""PTS BootWizard 4 / OS Selector 5 for hidden partitions other than 01h, 04h, 06h, 07h, 0Bh, 0Ch, 0Eh and unformatted partitions""")
            v.Add("""&HBB"",""EBR"","""","""","""",""Microsoft"",""Windows NT 4 Server"",""Corrupted fault-tolerant FAT32 mirrored master volume""")
            v.Add("""&HBC"",""EBR"","""","""","""",""Microsoft"",""Windows NT 4 Server"",""Corrupted fault-tolerant FAT32X mirrored master volume""")
            v.Add("""&HBC"",""MBR"",""LBA"","""","""",""Acronis"","""",""Backup / Acronis Secure Zone ('ACRONIS SZ')""")
            v.Add("""&HBC"",""MBR, EBR"","""","""","""",""Paragon Software Group"",""Backup Capsule"",""Backup Capsule""")
            v.Add("""&HBD"","""","""","""","""","""",""BonnyDOS/286""")
            v.Add("""&HBE"","""","""",""Yes"","""",""Sun Microsystems"",""Solaris 8"",""Solaris 8 boot""")
            v.Add("""&HBF"","""","""",""x86"",""Container"",""Sun Microsystems"",""Solaris"",""Solaris x86 (for Sun disklabels, since 2005)""")
            v.Add("""&HC0"",""MBR"",""CHS, LBA"",""x86"",""Secured, (Container)"",""Novell, IMS"",""DR-DOS, Multiuser DOS, REAL/32"",""Secured FAT partition (smaller than 32 MB)""")
            v.Add("""&HC0"","""","""","""","""",""Novell"","""",""NTFT""")
            v.Add("""&HC1"",""MBR, EBR"",""CHS, LBA"",""x86"",""Secured, Hidden, Filesystem"",""Digital Research"",""DR DOS 6.0+"",""Secured FAT12 (corresponds with 01h)""")
            v.Add("""&HC2"","""","""",""Yes"",""Hidden, Filesystem"",""BlueSky Innovations"",""Power Boot"",""Hidden Linux native filesystem""")
            v.Add("""&HC3"","""","""",""No"",""Hidden"",""BlueSky Innovations"",""Power Boot"",""Hidden Linux swap""")
            v.Add("""&HC4"",""MBR, EBR"",""CHS, LBA"",""x86"",""Secured, Hidden, Filesystem"",""Digital Research"",""DR DOS 6.0+"",""Secured FAT16 (corresponds with 04h)""")
            v.Add("""&HC5"",""MBR, EBR"",""CHS, LBA"",""No, AAP"",""Secured, Hidden, Container"",""Digital Research"",""DR DOS 6.0+"",""Secured extended partition with CHS addressing (corresponds with 05h)""")
            v.Add("""&HC6"",""MBR, EBR"",""CHS, LBA"",""x86"",""Secured, Hidden, Filesystem"",""Digital Research"",""DR DOS 6.0+"",""Secured FAT16B (corresponds with 06h)""")
            v.Add("""&HC6"",""EBR"","""","""","""",""Microsoft"",""Windows NT 4 Server"",""Corrupted fault-tolerant FAT16B mirrored slave volume""")
            v.Add("""&HC7"",""MBR"","""",""Yes"","""","""",""Syrinx"",""Syrinx boot""")
            v.Add("""&HC7"",""EBR"","""","""","""",""Microsoft"",""Windows NT 4 Server"",""Corrupted fault-tolerant HPFS/NTFS mirrored slave volume""")
            v.Add("""&HC8"","""","""","""","""","""",""DR-DOS"",""Reserved for DR-DOS""")
            v.Add("""&HC9"","""","""","""","""","""",""DR-DOS"",""Reserved for DR-DOS""")
            v.Add("""&HCA"","""","""","""","""","""",""DR-DOS"",""Reserved for DR-DOS""")
            v.Add("""&HCB"",""MBR, EBR"",""CHS, LBA"",""x86"",""Secured, Hidden, Filesystem"",""Caldera"",""DR-DOS 7.0x"",""Secured FAT32 (corresponds with 0Bh)""")
            v.Add("""&HCB"",""EBR"","""","""","""",""Microsoft"",""Windows NT 4 Server"",""Corrupted fault-tolerant FAT32 mirrored slave volume""")
            v.Add("""&HCC"",""MBR, EBR"",""LBA"",""x86"",""Secured, Hidden, Filesystem"",""Caldera"",""DR-DOS 7.0x"",""Secured FAT32X (corresponds with 0Ch)""")
            v.Add("""&HCC"",""EBR"","""","""","""",""Microsoft"",""Windows NT 4 Server"",""Corrupted fault-tolerant FAT32X mirrored slave volume""")
            v.Add("""&HCD"","""","""",""No"","""",""Convergent Technologies, Unisys"",""CTOS"",""Memory dump""")
            v.Add("""&HCE"",""MBR, EBR"",""LBA"",""x86"",""Secured, Hidden, Filesystem"",""Caldera"",""DR-DOS 7.0x"",""Secured FAT16X (corresponds with 0Eh)""")
            v.Add("""&HCF"",""MBR, EBR"",""LBA"",""No, AAP"",""Secured, Hidden, Container"",""Caldera"",""DR-DOS 7.0x"",""Secured extended partition with LBA (corresponds with 0Fh)""")
            v.Add("""&HD0"",""MBR"",""CHS, LBA"",""386"",""Secured, (Container)"",""Novell, IMS"",""Multiuser DOS, REAL/32"",""Secured FAT partition (larger than 32 MB)""")
            v.Add("""&HD1"",""MBR, EBR"",""CHS"",""386"",""Secured, Hidden, Filesystem"",""Novell"",""Multiuser DOS"",""Secured FAT12 (corresponds with 01h)""")
            v.Add("""&HD4"",""MBR, EBR"",""CHS"",""386"",""Secured, Hidden, Filesystem"",""Novell"",""Multiuser DOS"",""Secured FAT16 (corresponds with 04h)""")
            v.Add("""&HD5"",""MBR, EBR"",""CHS"",""No"",""Secured, Hidden, Container"",""Novell"",""Multiuser DOS"",""Secured extended partition with CHS addressing (corresponds with 05h)""")
            v.Add("""&HD6"",""MBR, EBR"",""CHS"",""386"",""Secured, Hidden, Filesystem"",""Novell"",""Multiuser DOS"",""Secured FAT16B (corresponds with 06h)""")
            v.Add("""&HD8"",""MBR"",""CHS"","""",""Filesystem"",""Digital Research"","""",""CP/M-86""")
            v.Add("""&HDA"","""","""",""No"","""",""John Hardin"","""",""Non-filesystem data""")
            v.Add("""&HDA"","""","""","""","""",""DataPower"",""Powercopy Backup"",""Shielded disk""")
            v.Add("""&HDB"",""MBR"",""CHS"",""x86"",""Filesystem"",""Digital Research"",""CP/M-86, Concurrent CP/M-86, Concurrent DOS"",""CP/M-86, Concurrent CP/M-86, Concurrent DOS""")
            v.Add("""&HDB"","""","""","""","""",""Convergent Technologies, Unisys"",""CTOS"",""""")
            v.Add("""&HDB"","""","""",""x86"","""",""KDG Telemetry"",""D800"",""boot image for x86 supervisor CPU (SCPU) module""")
            v.Add("""&HDB"",""MBR"",""CHS, LBA"",""x86"",""Hidden, Service, Filesystem"",""Dell"",""DRMK"",""FAT32 system restore partition (DSR)""")
            v.Add("""&HDD"","""","""",""No"","""",""Convergent Technologies, Unisys"",""CTOS"",""Hidden memory dump""")
            v.Add("""&HDE"",""MBR"",""CHS, LBA"",""x86"",""Hidden, Service, Filesystem"",""Dell"","""",""FAT16 utility/diagnostic partition""")
            v.Add("""&HDF"","""","""","""","""",""Data General"",""DG/UX"",""DG/UX virtual disk manager""")
            v.Add("""&HDF"",""MBR"","""","""",""Blocker"",""TeraByte Unlimited"",""BootIt"",""EMBRM""")
            v.Add("""&HDF"","""","""","""","""","""","""",""Aviion""")
            v.Add("""&HE0"","""","""","""",""Filesystem"",""STMicroelectronics"","""",""ST AVFS""")
            v.Add("""&HE1"","""","""","""",""Filesystem"",""Storage Dimensions"",""SpeedStor"",""Extended FAT12 (> 1023 cylinder)""")
            v.Add("""&HE2"","""","""","""",""Filesystem"","""","""",""DOS read-only (XFDISK)""")
            v.Add("""&HE3"","""","""","""",""Filesystem"",""Storage Dimensions"",""SpeedStor"",""DOS read-only""")
            v.Add("""&HE4"","""","""","""",""Filesystem"",""Storage Dimensions"",""SpeedStor"",""Extended FAT16 (< 1024 cylinder)""")
            v.Add("""&HE5"",""MBR"",""CHS"",""x86"",""Filesystem"",""Tandy"",""Tandy MS-DOS"",""Logical sectored FAT12 or FAT16""")
            v.Add("""&HE6"","""","""","""","""",""Storage Dimensions"",""SpeedStor""")
            v.Add("""&HE8"","""","""","""","""",""Linux"",""LUKS"",""Linux Unified Key Setup""")
            v.Add("""&HEB"","""","""",""386"",""Filesystem"",""Be Inc."",""BeOS, Haiku"",""BFS""")
            v.Add("""&HEC"","""","""","""",""Filesystem"",""Robert Szeleney"",""SkyOS"",""SkyFS""")
            v.Add("""&HED"",""MBR, EBR"",""CHS, LBA"",""x86"","""",""Matthias Paul"",""Sprytix"",""EDC loader""")
            v.Add("""&HED"",""VirtualMBR"",""CHS, LBA"",""x86"","""",""Robert Elliott, Hewlett Packard"",""EDD 4"",""GPT hybrid MBR""")
            v.Add("""&HEE"",""MBR"","""",""x86"",""Blocker, Policy, Container"",""Microsoft"",""EFI"",""GPT protective MBR""")
            v.Add("""&HEF"",""MBR"","""","""","""",""Intel"",""EFI"",""EFI system partition can be a FAT12, FAT16, FAT32 (or other) file system""")
            v.Add("""&HF0"","""",""CHS"","""","""","""",""Linux"",""PA-RISC Linux boot loader. It must reside in first physical 2 GB.""")
            v.Add("""&HF0"","""","""","""","""","""",""OS/32"",""floppy""")
            v.Add("""&HF1"","""","""","""","""",""Storage Dimensions"",""SpeedStor""")
            v.Add("""&HF2"",""MBR"",""CHS"",""x86"",""Filesystem"",""Sperry IT, Unisys, Digital Research"",""Sperry IT MS-DOS 3.x, Unisys MS-DOS 3.3, Digital Research DOS Plus 2.1"",""Logical sectored FAT12 or FAT16 secondary partition""")
            v.Add("""&HF3"","""","""","""","""",""Storage Dimensions"",""SpeedStor""")
            v.Add("""&HF4"","""","""","""",""Filesystem"",""Storage Dimensions"",""SpeedStor"",""'large' DOS partition""")
            v.Add("""&HF4"","""","""","""",""Filesystem"","""",""Prologue"",""single volume partition for NGF or TwinFS""")
            v.Add("""&HF5"","""","""","""",""Container"","""",""Prologue"",""MD0-MD9 multi volume partition for NGF or TwinFS""")
            v.Add("""&HF6"","""","""","""","""",""Storage Dimensions"",""SpeedStor""")
            v.Add("""&HF7"","""","""","""",""Filesystem"",""Natalia Portillo"",""O.S.G."",""EFAT""")
            v.Add("""&HF7"","""","""","""",""Filesystem"",""DDRdrive"",""X1"",""Solid State file system""")
            v.Add("""&HF9"","""","""","""","""",""ALC Press"",""Linux"",""pCache ext2/ext3 persistent cache""")
            v.Add("""&HFA"","""","""","""","""",""MandrakeSoft"",""Bochs"",""x86 emulator""")
            v.Add("""&HFB"","""","""","""",""Filesystem"",""VMware"",""VMware"",""VMware VMFS filesystem partition""")
            v.Add("""&HFC"","""","""",""No"","""",""VMware"",""VMware"",""VMware swap / VMKCORE kernel dump partition""")
            v.Add("""&HFD"","""","""","""","""",""GNU/Linux"",""Linux"",""Linux RAID superblock with auto-detect""")
            v.Add("""&HFD"",""MBR, EBR"",""CHS, LBA"",""x86"","""",""FreeDOS"",""FreeDOS"",""Reserved for FreeDOS""")
            v.Add("""&HFE"","""","""","""","""",""Storage Dimensions"",""SpeedStor"",""partition > 1024 cylinder""")
            v.Add("""&HFE"","""","""","""","""","""",""Intel"",""LANstep""")
            v.Add("""&HFE"","""","""","""",""Hidden, Service"",""IBM"","""",""PS/2 IML partition""")
            v.Add("""&HFE"",""MBR"",""CHS, LBA"",""x86"",""Hidden, Service, Filesystem"",""IBM"","""",""PS/2 recovery partition (FAT12 reference disk floppy image), (corresponds with 01h if activated, all other partitions +10h then)""")
            v.Add("""&HFE"","""","""","""",""Hidden"",""Microsoft"",""Windows NT"",""Disk Administration hidden partition""")
            v.Add("""&HFE"","""","""","""","""","""",""Linux"",""old Linux LVM""")
            v.Add("""&HFF"",""MBR"",""CHS"",""No"","""",""Microsoft"",""XENIX"",""XENIX bad block table""")

            Return v.ToArray

        End Function

#End Region

    End Class

    ''' <summary>
    ''' Sorter for PartitionCodeInfo.  Sorts Windows NT specific implementations
    ''' of MBR partition codes first.
    ''' </summary>
    ''' <remarks></remarks>
    Class PartCodeSorter
        Implements IComparer(Of PartitionCodeInfo)

        Private los1 As New List(Of String)
        Private los2 As New List(Of String)

        ''' <summary>
        ''' Compare two PartitionCodeInfo classes by Partition ID and Operating System.
        ''' </summary>
        ''' <param name="x"></param>
        ''' <param name="y"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Compare(x As PartitionCodeInfo, y As PartitionCodeInfo) As Integer Implements IComparer(Of PartitionCodeInfo).Compare

            los1.Clear()
            los2.Clear()

            If x.SupporedOSes IsNot Nothing Then los1.AddRange(x.SupporedOSes)
            If y.SupporedOSes IsNot Nothing Then los2.AddRange(y.SupporedOSes)

            If x.PartitionID.Value = y.PartitionID.Value Then

                If los1.Count = 1 AndAlso los1(0) = "Windows NT" AndAlso los2.Count = 1 AndAlso los2(0) = "Windows NT" Then
                    Return String.Compare(x.Description, y.Description)
                ElseIf los1.Count = 1 AndAlso los1(0) = "Windows NT" Then
                    Return -1
                ElseIf los2.Count = 1 AndAlso los2(0) = "Windows NT" Then
                    Return 1
                ElseIf los1.Contains("Windows NT") AndAlso los2.Contains("Windows NT") Then
                    Return String.Compare(x.Description, y.Description)
                ElseIf los1.Contains("Windows NT") Then
                    Return -1
                ElseIf los2.Contains("Windows NT") Then
                    Return 1
                Else
                    Return String.Compare(x.Description, y.Description)
                End If
            Else
                Return CInt(x.PartitionID.Value) - CInt(y.PartitionID.Value)
            End If

        End Function

    End Class

    ''' <summary>
    ''' Represents something that is identifiable by its GUID.
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IGuid
        ReadOnly Property Id As Guid

    End Interface

    ''' <summary>
    ''' Represents the partition configuration for an entire disk device.
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IDiskLayout
        Inherits IEnumerable(Of IDiskPartition)

        ''' <summary>
        ''' Returns the style of the disk or partition (MBR or GPT).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property PartitionStyle As PartitionStyle

        ''' <summary>
        ''' Returns a specific partition by its index in the collection.
        ''' </summary>
        ''' <param name="index"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Partition(index As Integer) As IDiskPartition

        ''' <summary>
        ''' Returns the number of partitions on the disk.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Count As Integer

    End Interface

    ''' <summary>
    ''' Represents a partition on a disk device.
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IDiskPartition

        ''' <summary>
        ''' Returns the logical partition number of the partition on the disk.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property PartitionNumber As Integer

        ''' <summary>
        ''' Returns the starting offset, in bytes, of the partition relative to the beginning of the disk.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Offset As Long

        ''' <summary>
        ''' Returns the size, in bytes, of the partition.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Size As Long

        ''' <summary>
        ''' Returns the friendly name of the partition type.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property TypeName As String

        ''' <summary>
        ''' Represents a technical type identifier.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property TypeString As String

        ''' <summary>
        ''' Returns the style of the disk or partition (MBR or GPT).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property PartitionStyle As PartitionStyle

    End Interface

    ''' <summary>
    ''' Base class for disk device layout information.
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class DiskLayoutInfo
        Implements IDiskLayout

        Friend _Layout As DRIVE_LAYOUT_INFORMATION_EX
        Friend _Parts() As IDiskPartition

        ''' <summary>
        ''' Populates disk layout information from an open disk handle.
        ''' </summary>
        ''' <param name="disk"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Friend Shared Function CreateLayout(disk As IntPtr) As IDiskLayout
            Dim lay As DRIVE_LAYOUT_INFORMATION_EX = Nothing
            Dim p() As PARTITION_INFORMATION_EX

            p = GetPartitions(Nothing, disk, lay)

            If (p Is Nothing) Then Return Nothing

            If lay.PartitionStyle = PartitionStyle.Gpt Then
                CreateLayout = New GptDiskLayoutInfo(lay, p)
            Else
                CreateLayout = New MbrDiskLayoutInfo(lay, p)
            End If
        End Function

        ''' <summary>
        ''' Populates disk layout information from a device path.
        ''' </summary>
        ''' <param name="disk"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Friend Shared Function CreateLayout(disk As String) As IDiskLayout
            Dim lay As DRIVE_LAYOUT_INFORMATION_EX = Nothing
            Dim p() As PARTITION_INFORMATION_EX

            p = GetPartitions(disk, IntPtr.Zero, lay)
            If p Is Nothing Then Return Nothing

            If lay.PartitionStyle = PartitionStyle.Gpt Then
                CreateLayout = New GptDiskLayoutInfo(lay, p)
            Else
                CreateLayout = New MbrDiskLayoutInfo(lay, p)
            End If
        End Function

        ''' <summary>
        ''' Create a new instance of this DiskLayoutInfo-derived class and initialize it with raw data from the operating system.
        ''' </summary>
        ''' <param name="li"></param>
        ''' <param name="p"></param>
        ''' <remarks></remarks>
        Friend Sub New(li As DRIVE_LAYOUT_INFORMATION_EX, p() As PARTITION_INFORMATION_EX)
            _Layout = li

            Dim pts As New List(Of IDiskPartition)

            For Each i In p
                pts.Add(DiskPartitionInfo.CreateInfo(i))
            Next
            _Parts = pts.ToArray
        End Sub


        ''' <summary>
        ''' Returns the partition style of the disk (MBR or GPT).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PartitionStyle As PartitionStyle Implements IDiskLayout.PartitionStyle
            Get
                PartitionStyle = _Layout.PartitionStyle
            End Get
        End Property

        ''' <summary>
        ''' Returns the number of partitions on the disk.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Count As Integer Implements IDiskLayout.Count
            Get
                Count = CInt(_Layout.ParititionCount)
            End Get
        End Property

        ''' <summary>
        ''' Returns a specific partition by its index in the collection.
        ''' </summary>
        ''' <param name="index"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Partition(index As Integer) As IDiskPartition Implements IDiskLayout.Partition
            Get
                Partition = _Parts(index)
            End Get
        End Property

        ''' <summary>
        ''' Converts this object into its string representation.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function ToString() As String
            ToString = _Layout.PartitionStyle.ToString & " disk, " & Count & " partitions."
        End Function

#Region "Enumeration"

        Public Function GetEnumerator() As IEnumerator(Of IDiskPartition) Implements IEnumerable(Of IDiskPartition).GetEnumerator
            GetEnumerator = New Enumerator(Me)
        End Function

        Public Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
            GetEnumerator1 = New Enumerator(Me)
        End Function

        Private Class Enumerator
            Implements IEnumerator(Of IDiskPartition)

            Private subj As DiskLayoutInfo
            Private pos As Integer = -1

            Sub New(subject As DiskLayoutInfo)
                subj = subject
            End Sub

            Public ReadOnly Property Current As IDiskPartition Implements IEnumerator(Of IDiskPartition).Current
                Get
                    Current = subj._Parts(pos)
                End Get
            End Property

            Public ReadOnly Property Current1 As Object Implements IEnumerator.Current
                Get
                    Current1 = subj._Parts(pos)
                End Get
            End Property

            Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
                pos += 1
                If pos >= subj.Count Then Return False Else Return True
            End Function

            Public Sub Reset() Implements IEnumerator.Reset
                pos = -1
            End Sub

#Region "IDisposable Support"
            Private disposedValue As Boolean ' To detect redundant calls

            Protected Overridable Sub Dispose(disposing As Boolean)
                If Not Me.disposedValue Then
                    If disposing Then
                        pos = -1
                        subj = Nothing
                    End If
                End If
                Me.disposedValue = True
            End Sub

            Public Sub Dispose() Implements IDisposable.Dispose
                Dispose(True)
                GC.SuppressFinalize(Me)
            End Sub
#End Region



        End Class


#End Region

    End Class

    ''' <summary>
    ''' Represents Gpt style disk layout information.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class GptDiskLayoutInfo
        Inherits DiskLayoutInfo
        Implements IGuid

        ''' <summary>
        ''' Create a new instance of this DiskLayoutInfo-derived class and initialize it with raw data from the operating system.
        ''' </summary>
        ''' <param name="li"></param>
        ''' <param name="p"></param>
        ''' <remarks></remarks>
        Friend Sub New(li As DRIVE_LAYOUT_INFORMATION_EX, p() As PARTITION_INFORMATION_EX)
            MyBase.New(li, p)
        End Sub

        ''' <summary>
        ''' Returns the Gpt disk Guid.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DiskId As Guid Implements IGuid.Id
            Get
                Return _Layout.Gpt.DiskId
            End Get
        End Property

        ''' <summary>
        ''' Returns the starting position of the first usable byte on the disk.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property StartingUsableOffset As Long
            Get
                Return _Layout.Gpt.StartingUsableOffset
            End Get
        End Property

        ''' <summary>
        ''' Returns the actual usable length of the disk.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property UsableLength As Long
            Get
                Return _Layout.Gpt.UsableLength
            End Get
        End Property

        ''' <summary>
        ''' Returns the maximum number of partitions allowed on the disk device.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MaxPartitionCount As UInteger
            Get
                Return _Layout.Gpt.MaxPartitionCount
            End Get
        End Property

    End Class

    ''' <summary>
    ''' Represents Mbr-style disk layout information.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class MbrDiskLayoutInfo
        Inherits DiskLayoutInfo

        ''' <summary>
        ''' Create a new instance of this DiskLayoutInfo-derived class and initialize it with raw data from the operating system.
        ''' </summary>
        ''' <param name="li"></param>
        ''' <param name="p"></param>
        ''' <remarks></remarks>
        Friend Sub New(li As DRIVE_LAYOUT_INFORMATION_EX, p() As PARTITION_INFORMATION_EX)
            MyBase.New(li, p)
        End Sub

        ''' <summary>
        ''' The Mbr Disk Signature
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Signature As UInteger
            Get
                Return _Layout.Mbr.Signature
            End Get
        End Property

    End Class

    ''' <summary>
    ''' Base class for disk partition information.
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class DiskPartitionInfo
        Implements IDiskPartition

        Friend _partex As PARTITION_INFORMATION_EX

        ''' <summary>
        ''' Create a new object that implements the IDiskPartition interface from operating system information.
        ''' </summary>
        ''' <param name="pe"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Friend Shared Function CreateInfo(pe As PARTITION_INFORMATION_EX) As IDiskPartition
            If pe.PartitionStyle = PartitionStyle.Gpt Then
                Return New GptDiskPartitionInfo(pe)
            Else
                Return New MbrDiskPartitionInfo(pe)
            End If
        End Function

        ''' <summary>
        ''' Creates a new instance of this DiskPartitionInfo-derived class and populates it with information from the operating system.
        ''' </summary>
        ''' <param name="pe"></param>
        ''' <remarks></remarks>
        Friend Sub New(pe As PARTITION_INFORMATION_EX)
            _partex = pe
        End Sub

        ''' <summary>
        ''' Returns the starting offset, in bytes, of the partition relative to the beginning of the disk.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Offset As Long Implements IDiskPartition.Offset
            Get
                Return _partex.StartingOffset
            End Get
        End Property

        Public MustOverride ReadOnly Property TypeString As String Implements IDiskPartition.TypeString

        ''' <summary>
        ''' Returns the logical partition number of the partition on the disk.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PartitionNumber As Integer Implements IDiskPartition.PartitionNumber
            Get
                Return CInt(_partex.PartitionNumber)
            End Get
        End Property

        ''' <summary>
        ''' Returns the size, in bytes, of the partition.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Size As Long Implements IDiskPartition.Size
            Get
                Return _partex.PartitionLength
            End Get
        End Property

        ''' <summary>
        ''' Returns the friendly type name of the partition type.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property TypeName As String Implements IDiskPartition.TypeName
            Get
                If _partex.PartitionStyle = PartitionStyle.Gpt Then
                    Return _partex.Gpt.Name
                Else
                    Return _partex.Mbr.PartitionCode.Name
                End If
            End Get
        End Property

        ''' <summary>
        ''' Returns the style of the partition (MBR or GPT).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PartitionStyle As PartitionStyle Implements IDiskPartition.PartitionStyle
            Get
                Return _partex.PartitionStyle
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return _partex.ToString
        End Function

    End Class

    ''' <summary>
    ''' Represents MBR style partition information.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class MbrDiskPartitionInfo
        Inherits DiskPartitionInfo

        ''' <summary>
        ''' Creates a new instance of this DiskPartitionInfo-derived class and populates it with information from the operating system.
        ''' </summary>
        ''' <param name="pe"></param>
        ''' <remarks></remarks>
        Friend Sub New(pe As PARTITION_INFORMATION_EX)
            MyBase.New(pe)
        End Sub

        ''' <summary>
        ''' Return detailed information about the partition type.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PartitionTypeInfo As PartitionCodeInfo
            Get
                Return _partex.Mbr.PartitionCode
            End Get
        End Property

        ''' <summary>
        ''' Return the MBR partition type byte code.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PartitionType As Byte
            Get
                Return _partex.Mbr.PartitionType
            End Get
        End Property

        ''' <summary>
        ''' Indicates that the partition is bootable.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Bootable As Boolean
            Get
                Return _partex.Mbr.BootIndicator
            End Get
        End Property

        ''' <summary>
        ''' The partition is a recognized partition type by the operating system.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Recognized As Boolean
            Get
                Return _partex.Mbr.RecognizedPartition
            End Get
        End Property

        ''' <summary>
        ''' Total number of hidden sectors on this partition.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property HiddenSectors As UInteger
            Get
                Return _partex.Mbr.HiddenSectors
            End Get
        End Property

        ''' <summary>
        ''' Represents a type identifier byte code.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides ReadOnly Property TypeString As String
            Get
                Return CType(CType(_partex.Mbr.PartitionType, FriendlyPartitionId), String)
            End Get
        End Property

    End Class

    ''' <summary>
    ''' Represents GPT style partition information.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class GptDiskPartitionInfo
        Inherits DiskPartitionInfo
        Implements IGuid

        ''' <summary>
        ''' Creates a new instance of this DiskPartitionInfo-derived class and populates it with information from the operating system.
        ''' </summary>
        ''' <param name="pe"></param>
        ''' <remarks></remarks>
        Friend Sub New(pe As PARTITION_INFORMATION_EX)
            MyBase.New(pe)
        End Sub

        ''' <summary>
        ''' Returns the UEFI attributes for the device.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Attributes As GptPartitionAttributes
            Get
                Return _partex.Gpt.Attributes
            End Get
        End Property

        ''' <summary>
        ''' Returns the Partition Guid.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PartitionId As Guid Implements IGuid.Id
            Get
                Return _partex.Gpt.PartitionId
            End Get
        End Property

        ''' <summary>
        ''' Returns the PartitionType Guid.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PartitionType As Guid
            Get
                Return _partex.Gpt.PartitionType
            End Get
        End Property

        ''' <summary>
        ''' Returns detailed partition type information.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PartitionTypeInfo As GptCodeInfo
            Get
                Return _partex.Gpt.PartitionCode
            End Get
        End Property

        ''' <summary>
        ''' Returns the Guid of the Partition Type.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides ReadOnly Property TypeString As String
            Get
                Return PartitionType.ToString("B")
            End Get
        End Property

    End Class

End Namespace
