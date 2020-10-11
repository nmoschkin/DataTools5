'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: SecurityDescriptor
''         Some system security structures translated 
''         from the Windows API.  These are Not used 
''         all that often in this assembly.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''



Imports System
Imports System.IO
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Reflection
Imports CoreCT.Memory

Namespace Native

    <HideModuleName()>
    Friend Module SecurityDescriptor

        <StructLayout(LayoutKind.Sequential, Pack:=1)>
        Structure SECURITY_ATTRIBUTES
            Public nLength As Int32
            Public lpSecurityDescriptor As IntPtr
            '        <MarshalAs(UnmanagedType.Bool)>
            Public bInheritHandle As Byte
        End Structure


        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure ACL
            Public AclRevision As Byte
            Public Sbz1 As Byte
            Public AclSize As UShort
            Public AceCount As UShort
            Public Sbz2 As UShort
        End Structure

        Public Enum SECURITY_IMPERSONATION_LEVEL
            SecurityAnonymous
            SecurityIdentification
            SecurityImpersonation
            SecurityDelegation
        End Enum

        Public Const SECURITY_ANONYMOUS = (SECURITY_IMPERSONATION_LEVEL.SecurityAnonymous << 16)
        Public Const SECURITY_IDENTIFICATION = (SECURITY_IMPERSONATION_LEVEL.SecurityIdentification << 16)
        Public Const SECURITY_IMPERSONATION = (SECURITY_IMPERSONATION_LEVEL.SecurityImpersonation << 16)
        Public Const SECURITY_DELEGATION = (SECURITY_IMPERSONATION_LEVEL.SecurityDelegation << 16)

        Public Const SECURITY_CONTEXT_TRACKING = &H40000
        Public Const SECURITY_EFFECTIVE_ONLY = &H80000

        Public Const SECURITY_SQOS_PRESENT = &H100000
        Public Const SECURITY_VALID_SQOS_FLAGS = &H1F0000


        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''                                                                    ''
        ''                             SECURITY_DESCRIPTOR                    ''
        ''                                                                    ''
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''
        ''  Define the Security Descriptor and related data types.
        ''  This is an opaque data structure.
        ''

        '' begin_wdm
        ''
        '' Current security descriptor revision value
        ''

        Public Const SECURITY_DESCRIPTOR_REVISION = (1)
        Public Const SECURITY_DESCRIPTOR_REVISION1 = (1)

        '' end_wdm
        '' begin_ntifs

        Public Function SECURITY_DESCRIPTOR_MIN_LENGTH() As Integer
            SECURITY_DESCRIPTOR_MIN_LENGTH = Marshal.SizeOf(Of SECURITY_DESCRIPTOR)
        End Function

        Public Enum SECURITY_DESCRIPTOR_CONTROL
            SE_OWNER_DEFAULTED = (&H1)
            SE_GROUP_DEFAULTED = (&H2)
            SE_DACL_PRESENT = (&H4)
            SE_DACL_DEFAULTED = (&H8)
            SE_SACL_PRESENT = (&H10)
            SE_SACL_DEFAULTED = (&H20)
            SE_DACL_AUTO_INHERIT_REQ = (&H100)
            SE_SACL_AUTO_INHERIT_REQ = (&H200)
            SE_DACL_AUTO_INHERITED = (&H400)
            SE_SACL_AUTO_INHERITED = (&H800)
            SE_DACL_PROTECTED = (&H1000)
            SE_SACL_PROTECTED = (&H2000)
            SE_RM_CONTROL_VALID = (&H4000)
            SE_SELF_RELATIVE = (&H8000)
        End Enum

        ''
        ''  Where:
        ''
        ''      SE_OWNER_DEFAULTED - This boolean flag, when set, indicates that the
        ''          SID pointed to by the Owner field was provided by a
        ''          defaulting mechanism rather than explicitly provided by the
        ''          original provider of the security descriptor.  This may
        ''          affect the treatment of the SID with respect to inheritence
        ''          of an owner.
        ''
        ''      SE_GROUP_DEFAULTED - This boolean flag, when set, indicates that the
        ''          SID in the Group field was provided by a defaulting mechanism
        ''          rather than explicitly provided by the original provider of
        ''          the security descriptor.  This may affect the treatment of
        ''          the SID with respect to inheritence of a primary group.
        ''
        ''      SE_DACL_PRESENT - This boolean flag, when set, indicates that the
        ''          security descriptor contains a discretionary ACL.  If this
        ''          flag is set and the Dacl field of the SECURITY_DESCRIPTOR is
        ''          null, then a null ACL is explicitly being specified.
        ''
        ''      SE_DACL_DEFAULTED - This boolean flag, when set, indicates that the
        ''          ACL pointed to by the Dacl field was provided by a defaulting
        ''          mechanism rather than explicitly provided by the original
        ''          provider of the security descriptor.  This may affect the
        ''          treatment of the ACL with respect to inheritence of an ACL.
        ''          This flag is ignored if the DaclPresent flag is not set.
        ''
        ''      SE_SACL_PRESENT - This boolean flag, when set,  indicates that the
        ''          security descriptor contains a system ACL pointed to by the
        ''          Sacl field.  If this flag is set and the Sacl field of the
        ''          SECURITY_DESCRIPTOR is null, then an empty (but present)
        ''          ACL is being specified.
        ''
        ''      SE_SACL_DEFAULTED - This boolean flag, when set, indicates that the
        ''          ACL pointed to by the Sacl field was provided by a defaulting
        ''          mechanism rather than explicitly provided by the original
        ''          provider of the security descriptor.  This may affect the
        ''          treatment of the ACL with respect to inheritence of an ACL.
        ''          This flag is ignored if the SaclPresent flag is not set.
        ''
        ''      SE_SELF_RELATIVE - This boolean flag, when set, indicates that the
        ''          security descriptor is in self-relative form.  In this form,
        ''          all fields of the security descriptor are contiguous in memory
        ''          and all pointer fields are expressed as offsets from the
        ''          beginning of the security descriptor.  This form is useful
        ''          for treating security descriptors as opaque data structures
        ''          for transmission in communication protocol or for storage on
        ''          secondary media.
        ''
        ''
        ''
        '' Pictorially the structure of a security descriptor is as follows:
        ''
        ''       3 3 2 2 2 2 2 2 2 2 2 2 1 1 1 1 1 1 1 1 1 1
        ''       1 0 9 8 7 6 5 4 3 2 1 0 9 8 7 6 5 4 3 2 1 0 9 8 7 6 5 4 3 2 1 0
        ''      +---------------------------------------------------------------+
        ''      |            Control            |Reserved1 (SBZ)|   Revision    |
        ''      +---------------------------------------------------------------+
        ''      |                            Owner                              |
        ''      +---------------------------------------------------------------+
        ''      |                            Group                              |
        ''      +---------------------------------------------------------------+
        ''      |                            Sacl                               |
        ''      +---------------------------------------------------------------+
        ''      |                            Dacl                               |
        ''      +---------------------------------------------------------------+
        ''
        '' In general, this data structure should be treated opaquely to ensure future
        '' compatibility.
        ''
        ''
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure SECURITY_DESCRIPTOR_RELATIVE
            Public Revision As Byte
            Public Sbz1 As Byte
            Public Control As SECURITY_DESCRIPTOR_CONTROL
            Public Owner As UInteger
            Public [Group] As UInteger
            Public Sacl As UInteger
            Public Dacl As UInteger
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure SECURITY_DESCRIPTOR_REAL
            Public Revision As Byte
            Public Sbz1 As Byte
            Public Control As SECURITY_DESCRIPTOR_CONTROL
            Public Owner As IntPtr
            Public [Group] As IntPtr
            Public Sacl As ACL
            Public Dacl As ACL
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure SECURITY_DESCRIPTOR
            Implements IDisposable

            Public Revision As Byte
            Public Sbz1 As Byte
            Public Control As SECURITY_DESCRIPTOR_CONTROL
            Public Owner As IntPtr
            Public [Group] As IntPtr
            Public Sacl As IntPtr
            Public Dacl As IntPtr

            Public Sub Dispose() Implements IDisposable.Dispose
                If Sacl <> IntPtr.Zero Then
                    Marshal.FreeCoTaskMem(Sacl)
                End If

                If Dacl <> IntPtr.Zero Then
                    Marshal.FreeCoTaskMem(Dacl)
                End If

                GC.SuppressFinalize(Me)
            End Sub

            Protected Overrides Sub Finalize()
                Dispose()
            End Sub
        End Structure

        Public Function SecurityDescriptorToReal(sd As SECURITY_DESCRIPTOR) As SECURITY_DESCRIPTOR_REAL
            Dim sr As New SECURITY_DESCRIPTOR_REAL
            Dim msacl As MemPtr = sd.Sacl
            Dim mdacl As MemPtr = sd.Dacl

            sr.Sacl = msacl.ToStruct(Of ACL)
            sr.Dacl = mdacl.ToStruct(Of ACL)

            SecurityDescriptorToReal = sr
        End Function

        Public Function RealToSecurityDescriptor(sr As SECURITY_DESCRIPTOR_REAL) As SECURITY_DESCRIPTOR
            Dim sd As SECURITY_DESCRIPTOR = NewSecurityDescriptor()
            Marshal.StructureToPtr(sr.Sacl, sd.Sacl, False)
            Marshal.StructureToPtr(sr.Dacl, sd.Dacl, False)
            RealToSecurityDescriptor = sd
        End Function

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure SECURITY_OBJECT_AI_PARAMS
            Public Size As UInteger
            Public ConstraintMask As UInteger
        End Structure

        Public Function NewSecurityDescriptor() As SECURITY_DESCRIPTOR
            Dim sd As New SECURITY_DESCRIPTOR
            sd.Revision = SECURITY_DESCRIPTOR_REVISION
            sd.Sbz1 = CByte(Marshal.SizeOf(sd))
            sd.Dacl = Marshal.AllocCoTaskMem(Marshal.SizeOf(sd.Dacl))
            sd.Sacl = Marshal.AllocCoTaskMem(Marshal.SizeOf(sd.Sacl))
            NewSecurityDescriptor = sd
        End Function

        Public Function NewAIParams() As SECURITY_OBJECT_AI_PARAMS
            Dim ai As New SECURITY_OBJECT_AI_PARAMS
            ai.Size = CUInt(Marshal.SizeOf(Of SECURITY_OBJECT_AI_PARAMS))
            NewAIParams = ai
        End Function

        Public Declare Function ConvertStringSecurityDescriptorToSecurityDescriptor Lib "advapi32.dll" _
        Alias "ConvertStringSecurityDescriptorToSecurityDescriptorW" _
        (<MarshalAs(UnmanagedType.LPWStr)> StringSecurityDescriptor As String,
         StringSDRevision As UInteger,
         ByRef SecurityDescriptor As IntPtr,
         ByRef SecurityDescriptorSize As UInteger) As <MarshalAs(UnmanagedType.Bool)> Boolean

        Public Function StringToSecurityDescriptor(strSD As String) As SECURITY_DESCRIPTOR

            Dim ptr As MemPtr = IntPtr.Zero
            Dim ls As UInteger = 0
            Dim sd As SECURITY_DESCRIPTOR

            ConvertStringSecurityDescriptorToSecurityDescriptor(strSD, 1, ptr, ls)
            sd = ptr.ToStruct(Of SECURITY_DESCRIPTOR)

            ptr.LocalFree()
            StringToSecurityDescriptor = sd
        End Function

        '' end_ntifs

        '' Where:
        ''
        ''     Revision - Contains the revision level of the security
        ''         descriptor.  This allows this structure to be passed between
        ''         systems or stored on disk even though it is expected to
        ''         change in the future.
        ''
        ''     Control - A set of flags which qualify the meaning of the
        ''         security descriptor or individual fields of the security
        ''         descriptor.
        ''
        ''     Owner - is a pointer to an SID representing an object's owner.
        ''         If this field is null, then no owner SID is present in the
        ''         security descriptor.  If the security descriptor is in
        ''         self-relative form, then this field contains an offset to
        ''         the SID, rather than a pointer.
        ''
        ''     Group - is a pointer to an SID representing an object's primary
        ''         group.  If this field is null, then no primary group SID is
        ''         present in the security descriptor.  If the security descriptor
        ''         is in self-relative form, then this field contains an offset to
        ''         the SID, rather than a pointer.
        ''
        ''     Sacl - is a pointer to a system ACL.  This field value is only
        ''         valid if the DaclPresent control flag is set.  If the
        ''         SaclPresent flag is set and this field is null, then a null
        ''         ACL  is specified.  If the security descriptor is in
        ''         self-relative form, then this field contains an offset to
        ''         the ACL, rather than a pointer.
        ''
        ''     Dacl - is a pointer to a discretionary ACL.  This field value is
        ''         only valid if the DaclPresent control flag is set.  If the
        ''         DaclPresent flag is set and this field is null, then a null
        ''         ACL (unconditionally granting access) is specified.  If the
        ''         security descriptor is in self-relative form, then this field
        ''         contains an offset to the ACL, rather than a pointer.
        ''

    End Module

End Namespace
