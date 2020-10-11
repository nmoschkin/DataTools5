// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library - Interop
// '
// ' Module: SecurityDescriptor
// '         Some system security structures translated 
// '         from the Windows API.  These are Not used 
// '         all that often in this assembly.
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''



using System;
using System.Runtime.InteropServices;
using CoreCT.Memory;
using Microsoft.VisualBasic;

namespace DataTools.Interop.Native
{
    [HideModuleName()]
    internal static class SecurityDescriptor
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            // <MarshalAs(UnmanagedType.Bool)>
            public byte bInheritHandle;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct ACL
        {
            public byte AclRevision;
            public byte Sbz1;
            public ushort AclSize;
            public ushort AceCount;
            public ushort Sbz2;
        }

        public enum SECURITY_IMPERSONATION_LEVEL
        {
            SecurityAnonymous,
            SecurityIdentification,
            SecurityImpersonation,
            SecurityDelegation
        }

        public const int SECURITY_ANONYMOUS = 0;
        public const int SECURITY_IDENTIFICATION = 65536;
        public const int SECURITY_IMPERSONATION = 131072;
        public const int SECURITY_DELEGATION = 196608;
        public const int SECURITY_CONTEXT_TRACKING = 0x40000;
        public const int SECURITY_EFFECTIVE_ONLY = 0x80000;
        public const int SECURITY_SQOS_PRESENT = 0x100000;
        public const int SECURITY_VALID_SQOS_FLAGS = 0x1F0000;


        // '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // '                                                                    ''
        // '                             SECURITY_DESCRIPTOR                    ''
        // '                                                                    ''
        // '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // '
        // '  Define the Security Descriptor and related data types.
        // '  This is an opaque data structure.
        // '

        // ' begin_wdm
        // '
        // ' Current security descriptor revision value
        // '

        public const int SECURITY_DESCRIPTOR_REVISION = 1;
        public const int SECURITY_DESCRIPTOR_REVISION1 = 1;

        // ' end_wdm
        // ' begin_ntifs

        public static int SECURITY_DESCRIPTOR_MIN_LENGTH()
        {
            int SECURITY_DESCRIPTOR_MIN_LENGTHRet = default;
            SECURITY_DESCRIPTOR_MIN_LENGTHRet = Marshal.SizeOf<SECURITY_DESCRIPTOR>();
            return SECURITY_DESCRIPTOR_MIN_LENGTHRet;
        }

        public enum SECURITY_DESCRIPTOR_CONTROL
        {
            SE_OWNER_DEFAULTED = 0x1,
            SE_GROUP_DEFAULTED = 0x2,
            SE_DACL_PRESENT = 0x4,
            SE_DACL_DEFAULTED = 0x8,
            SE_SACL_PRESENT = 0x10,
            SE_SACL_DEFAULTED = 0x20,
            SE_DACL_AUTO_INHERIT_REQ = 0x100,
            SE_SACL_AUTO_INHERIT_REQ = 0x200,
            SE_DACL_AUTO_INHERITED = 0x400,
            SE_SACL_AUTO_INHERITED = 0x800,
            SE_DACL_PROTECTED = 0x1000,
            SE_SACL_PROTECTED = 0x2000,
            SE_RM_CONTROL_VALID = 0x4000,
            SE_SELF_RELATIVE = 0x8000
        }

        // '
        // '  Where:
        // '
        // '      SE_OWNER_DEFAULTED - This boolean flag, when set, indicates that the
        // '          SID pointed to by the Owner field was provided by a
        // '          defaulting mechanism rather than explicitly provided by the
        // '          original provider of the security descriptor.  This may
        // '          affect the treatment of the SID with respect to inheritence
        // '          of an owner.
        // '
        // '      SE_GROUP_DEFAULTED - This boolean flag, when set, indicates that the
        // '          SID in the Group field was provided by a defaulting mechanism
        // '          rather than explicitly provided by the original provider of
        // '          the security descriptor.  This may affect the treatment of
        // '          the SID with respect to inheritence of a primary group.
        // '
        // '      SE_DACL_PRESENT - This boolean flag, when set, indicates that the
        // '          security descriptor contains a discretionary ACL.  If this
        // '          flag is set and the Dacl field of the SECURITY_DESCRIPTOR is
        // '          null, then a null ACL is explicitly being specified.
        // '
        // '      SE_DACL_DEFAULTED - This boolean flag, when set, indicates that the
        // '          ACL pointed to by the Dacl field was provided by a defaulting
        // '          mechanism rather than explicitly provided by the original
        // '          provider of the security descriptor.  This may affect the
        // '          treatment of the ACL with respect to inheritence of an ACL.
        // '          This flag is ignored if the DaclPresent flag is not set.
        // '
        // '      SE_SACL_PRESENT - This boolean flag, when set,  indicates that the
        // '          security descriptor contains a system ACL pointed to by the
        // '          Sacl field.  If this flag is set and the Sacl field of the
        // '          SECURITY_DESCRIPTOR is null, then an empty (but present)
        // '          ACL is being specified.
        // '
        // '      SE_SACL_DEFAULTED - This boolean flag, when set, indicates that the
        // '          ACL pointed to by the Sacl field was provided by a defaulting
        // '          mechanism rather than explicitly provided by the original
        // '          provider of the security descriptor.  This may affect the
        // '          treatment of the ACL with respect to inheritence of an ACL.
        // '          This flag is ignored if the SaclPresent flag is not set.
        // '
        // '      SE_SELF_RELATIVE - This boolean flag, when set, indicates that the
        // '          security descriptor is in self-relative form.  In this form,
        // '          all fields of the security descriptor are contiguous in memory
        // '          and all pointer fields are expressed as offsets from the
        // '          beginning of the security descriptor.  This form is useful
        // '          for treating security descriptors as opaque data structures
        // '          for transmission in communication protocol or for storage on
        // '          secondary media.
        // '
        // '
        // '
        // ' Pictorially the structure of a security descriptor is as follows:
        // '
        // '       3 3 2 2 2 2 2 2 2 2 2 2 1 1 1 1 1 1 1 1 1 1
        // '       1 0 9 8 7 6 5 4 3 2 1 0 9 8 7 6 5 4 3 2 1 0 9 8 7 6 5 4 3 2 1 0
        // '      +---------------------------------------------------------------+
        // '      |            Control            |Reserved1 (SBZ)|   Revision    |
        // '      +---------------------------------------------------------------+
        // '      |                            Owner                              |
        // '      +---------------------------------------------------------------+
        // '      |                            Group                              |
        // '      +---------------------------------------------------------------+
        // '      |                            Sacl                               |
        // '      +---------------------------------------------------------------+
        // '      |                            Dacl                               |
        // '      +---------------------------------------------------------------+
        // '
        // ' In general, this data structure should be treated opaquely to ensure future
        // ' compatibility.
        // '
        // '
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SECURITY_DESCRIPTOR_RELATIVE
        {
            public byte Revision;
            public byte Sbz1;
            public SECURITY_DESCRIPTOR_CONTROL Control;
            public uint Owner;
            public uint Group;
            public uint Sacl;
            public uint Dacl;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SECURITY_DESCRIPTOR_REAL
        {
            public byte Revision;
            public byte Sbz1;
            public SECURITY_DESCRIPTOR_CONTROL Control;
            public IntPtr Owner;
            public IntPtr Group;
            public ACL Sacl;
            public ACL Dacl;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SECURITY_DESCRIPTOR : IDisposable
        {
            public byte Revision;
            public byte Sbz1;
            public SECURITY_DESCRIPTOR_CONTROL Control;
            public IntPtr Owner;
            public IntPtr Group;
            public IntPtr Sacl;
            public IntPtr Dacl;

            public void Dispose()
            {
                if (Sacl != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(Sacl);
                }

                if (Dacl != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(Dacl);
                }

                GC.SuppressFinalize(this);
            }

        }

        public static SECURITY_DESCRIPTOR_REAL SecurityDescriptorToReal(SECURITY_DESCRIPTOR sd)
        {
            SECURITY_DESCRIPTOR_REAL SecurityDescriptorToRealRet = default;
            var sr = new SECURITY_DESCRIPTOR_REAL();
            MemPtr msacl = sd.Sacl;
            MemPtr mdacl = sd.Dacl;
            sr.Sacl = msacl.ToStruct<ACL>();
            sr.Dacl = mdacl.ToStruct<ACL>();
            SecurityDescriptorToRealRet = sr;
            return SecurityDescriptorToRealRet;
        }

        public static SECURITY_DESCRIPTOR RealToSecurityDescriptor(SECURITY_DESCRIPTOR_REAL sr)
        {
            SECURITY_DESCRIPTOR RealToSecurityDescriptorRet = default;
            var sd = NewSecurityDescriptor();
            Marshal.StructureToPtr(sr.Sacl, sd.Sacl, false);
            Marshal.StructureToPtr(sr.Dacl, sd.Dacl, false);
            RealToSecurityDescriptorRet = sd;
            return RealToSecurityDescriptorRet;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SECURITY_OBJECT_AI_PARAMS
        {
            public uint Size;
            public uint ConstraintMask;
        }

        public static SECURITY_DESCRIPTOR NewSecurityDescriptor()
        {
            SECURITY_DESCRIPTOR NewSecurityDescriptorRet = default;
            var sd = new SECURITY_DESCRIPTOR();
            sd.Revision = SECURITY_DESCRIPTOR_REVISION;
            sd.Sbz1 = (byte)Marshal.SizeOf(sd);
            sd.Dacl = Marshal.AllocCoTaskMem(Marshal.SizeOf(sd.Dacl));
            sd.Sacl = Marshal.AllocCoTaskMem(Marshal.SizeOf(sd.Sacl));
            NewSecurityDescriptorRet = sd;
            return NewSecurityDescriptorRet;
        }

        public static SECURITY_OBJECT_AI_PARAMS NewAIParams()
        {
            SECURITY_OBJECT_AI_PARAMS NewAIParamsRet = default;
            var ai = new SECURITY_OBJECT_AI_PARAMS();
            ai.Size = (uint)Marshal.SizeOf<SECURITY_OBJECT_AI_PARAMS>();
            NewAIParamsRet = ai;
            return NewAIParamsRet;
        }

        [DllImport("advapi32.dll", EntryPoint = "ConvertStringSecurityDescriptorToSecurityDescriptorW")]

        public static extern bool ConvertStringSecurityDescriptorToSecurityDescriptor([MarshalAs(UnmanagedType.LPWStr)] string StringSecurityDescriptor, uint StringSDRevision, ref IntPtr SecurityDescriptor, ref uint SecurityDescriptorSize);

        public static SECURITY_DESCRIPTOR StringToSecurityDescriptor(string strSD)
        {
            SECURITY_DESCRIPTOR StringToSecurityDescriptorRet = default;
            MemPtr ptr = IntPtr.Zero;
            uint ls = 0U;
            SECURITY_DESCRIPTOR sd;
            IntPtr argSecurityDescriptor = ptr;
            SecurityDescriptor.ConvertStringSecurityDescriptorToSecurityDescriptor(strSD, 1U, ref argSecurityDescriptor, ref ls);
            sd = ptr.ToStruct<SECURITY_DESCRIPTOR>();
            ptr.LocalFree();
            StringToSecurityDescriptorRet = sd;
            return StringToSecurityDescriptorRet;
        }

        // ' end_ntifs

        // ' Where:
        // '
        // '     Revision - Contains the revision level of the security
        // '         descriptor.  This allows this structure to be passed between
        // '         systems or stored on disk even though it is expected to
        // '         change in the future.
        // '
        // '     Control - A set of flags which qualify the meaning of the
        // '         security descriptor or individual fields of the security
        // '         descriptor.
        // '
        // '     Owner - is a pointer to an SID representing an object's owner.
        // '         If this field is null, then no owner SID is present in the
        // '         security descriptor.  If the security descriptor is in
        // '         self-relative form, then this field contains an offset to
        // '         the SID, rather than a pointer.
        // '
        // '     Group - is a pointer to an SID representing an object's primary
        // '         group.  If this field is null, then no primary group SID is
        // '         present in the security descriptor.  If the security descriptor
        // '         is in self-relative form, then this field contains an offset to
        // '         the SID, rather than a pointer.
        // '
        // '     Sacl - is a pointer to a system ACL.  This field value is only
        // '         valid if the DaclPresent control flag is set.  If the
        // '         SaclPresent flag is set and this field is null, then a null
        // '         ACL  is specified.  If the security descriptor is in
        // '         self-relative form, then this field contains an offset to
        // '         the ACL, rather than a pointer.
        // '
        // '     Dacl - is a pointer to a discretionary ACL.  This field value is
        // '         only valid if the DaclPresent control flag is set.  If the
        // '         DaclPresent flag is set and this field is null, then a null
        // '         ACL (unconditionally granting access) is specified.  If the
        // '         security descriptor is in self-relative form, then this field
        // '         contains an offset to the ACL, rather than a pointer.
        // '

    }
}