'' ************************************************* ''
'' DataTools Visual Basic Utility Library 
''
'' Module: TypeConverter for Blob
''         
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Option Explicit On
Option Compare Binary
Option Strict Off

Imports System
Imports System.Text
Imports System.IO

Imports System.ComponentModel
Imports System.ComponentModel.Design.Serialization
Imports System.Runtime
Imports System.Runtime.InteropServices
Imports System.Numerics

Imports DataTools.ByteOrderMark
Imports DataTools.Memory.Internal
Imports System.Drawing


Namespace Memory

#Region "BlobConverter"

    Public Class BlobConverter
        Inherits ExpandableObjectConverter

#Region "Shared Functions"

#Region "Exerimental Structure Conversion Functions"


        Public Shared Function StructureToBlob(operand As Object, ByRef instance As Blob) As Boolean

            Dim cb As Integer = Marshal.SizeOf(operand)
            instance = New Blob

            instance.Length = cb
            Marshal.StructureToPtr(operand, instance.DangerousGetHandle, True)
            Return True

        End Function

        ''' <summary>
        ''' Attempts to recover a structure from a blob.  If it is unsuccessful, it will return the bytes in the instance instead.
        ''' </summary>
        ''' <param name="operand"></param>
        ''' <param name="instance"></param>
        ''' <remarks></remarks>
        Public Shared Function BlobToStructure(operand As Blob, ByRef instance As Object, Optional type As System.Type = Nothing, Optional COMObjectFullName As String = "") As Boolean

            If type Is Nothing Then
                If COMObjectFullName IsNot Nothing AndAlso COMObjectFullName <> "" Then
                    instance = CreateObject(COMObjectFullName)
                    type = instance.GetType
                ElseIf instance Is Nothing Then
                    Return False
                Else
                    type = instance.GetType
                End If
            End If

            instance = Marshal.PtrToStructure(operand.DangerousGetHandle, type)
            Return (instance IsNot Nothing)
        End Function

#End Region

        ''' <summary>
        ''' Converts the specified image into a byte array using the specified System.Drawing.Imaging.ImageFormat object.
        ''' </summary>
        ''' <param name="operand">The image to convert</param>
        ''' <param name="fmt">The format object to use for conversion</param>
        ''' <returns>A byte array representing the image in the specified format</returns>
        ''' <remarks></remarks>
        Public Shared Function ImageToBytes(operand As System.Drawing.Image, fmt As System.Drawing.Imaging.ImageFormat) As Byte()
            Dim ms As New MemoryStream

            operand.Save(ms, fmt)

            Dim b() As Byte = (ms.ToArray)
            ms.Close()
            Return b
        End Function

        ''' <summary>
        ''' Converts the specified image into a byte array using the System.Drawing.Imagein.ImageFormat.MemoryBmp object.
        ''' </summary>
        ''' <param name="operand">The image to convert</param>
        ''' <returns>A byte array representing the image as a memory bitmap</returns>
        ''' <remarks></remarks>
        Public Shared Function ImageToBytes(operand As System.Drawing.Image) As Byte()
            Return ImageToBytes(operand, System.Drawing.Imaging.ImageFormat.MemoryBmp)
        End Function


        ''' <summary>
        ''' Attempts to convert the specified byte array into an Image object.
        ''' </summary>
        ''' <param name="operand"></param>
        ''' <returns>A new image based on the byte array passed.</returns>
        ''' <remarks></remarks>
        Public Shared Function BytesToImage(operand As Byte()) As System.Drawing.Image
            Dim ms As New MemoryStream(operand),
                img As System.Drawing.Image

            img = Image.FromStream(ms)
            ms.Close()
            Return img
        End Function

        Public Shared Function BytesToDate(b() As Byte, Optional startIndex As Integer = 0) As Date
            BytesToDate = ToDate(b, 0)
        End Function

        Public Shared Function DateToBytes(d As Date) As Byte()
            DateToBytes = ToBytes(d)
        End Function

        Public Shared Function BytesToDateArray(b() As Byte) As Date()
            BytesToDateArray = ToDates(b, 0)
        End Function

        Public Shared Function DateArrayToBytes(d() As Date) As Byte()
            DateArrayToBytes = ToBytes(d)
        End Function

        Public Shared Function ToByteArray(s() As SByte) As Byte()
            Dim sb = GCHandle.Alloc(s, GCHandleType.Pinned)
            ReDim ToByteArray(s.Length - 1)

            MemCpy(ToByteArray, sb.AddrOfPinnedObject, s.Length)
            sb.Free()
        End Function

        Public Shared Function ToSByteArray(s() As Byte) As SByte()
            Dim sb = GCHandle.Alloc(s, GCHandleType.Pinned)
            ReDim ToSByteArray(s.Length - 1)

            MemCpy(ToSByteArray, sb.AddrOfPinnedObject, s.Length)
            sb.Free()
        End Function

        Public Shared Function ToUShortArray(s() As Short) As UShort()
            Dim sb = GCHandle.Alloc(s, GCHandleType.Pinned)
            ReDim ToUShortArray(s.Length - 1)

            MemCpy(ToUShortArray, sb.AddrOfPinnedObject, s.Length << 1)
            sb.Free()
        End Function


        Public Shared Function ToShortArray(s() As UShort) As Short()
            Dim sb = GCHandle.Alloc(s, GCHandleType.Pinned)
            ReDim ToShortArray(s.Length - 1)

            MemCpy(ToShortArray, sb.AddrOfPinnedObject, s.Length << 1)
            sb.Free()
        End Function

        Public Shared Function ToUIntegerArray(s() As Integer) As UInteger()
            Dim sb = GCHandle.Alloc(s, GCHandleType.Pinned)
            ReDim ToUIntegerArray(s.Length - 1)

            MemCpy(ToUIntegerArray, sb.AddrOfPinnedObject, s.Length << 2)
            sb.Free()
        End Function

        Public Shared Function ToIntegerArray(s() As UInteger) As Integer()
            Dim sb = GCHandle.Alloc(s, GCHandleType.Pinned)
            ReDim ToIntegerArray(s.Length - 1)

            MemCpy(ToIntegerArray, sb.AddrOfPinnedObject, s.Length << 2)
            sb.Free()
        End Function

        Public Shared Function ToULongArray(s() As Long) As ULong()
            Dim sb = GCHandle.Alloc(s, GCHandleType.Pinned)
            ReDim ToULongArray(s.Length - 1)

            MemCpy(ToULongArray, sb.AddrOfPinnedObject, s.Length << 3)
            sb.Free()
        End Function

        Public Shared Function ToLongArray(s() As ULong) As Long()
            Dim sb = GCHandle.Alloc(s, GCHandleType.Pinned)
            ReDim ToLongArray(s.Length - 1)

            MemCpy(ToLongArray, sb.AddrOfPinnedObject, s.Length << 3)
            sb.Free()
        End Function

#End Region

#Region "TypeConverter Implementation"

        Public Overrides Function CanConvertFrom(context As System.ComponentModel.ITypeDescriptorContext, sourceType As System.Type) As Boolean

            Dim i As Integer,
                c As Integer = BlobConst.UBound

            For i = 0 To c
                If sourceType = Blob.Types(i) Then Return True
                If sourceType = Blob.ArrayTypes(i) Then Return True
            Next

            If sourceType.IsValueType Then
                Return True
            End If

            If sourceType.IsClass AndAlso sourceType.BaseType = Blob.Types(BlobTypes.Image) Then Return True

            Return MyBase.CanConvertFrom(context, sourceType)
        End Function

        Public Overrides Function CanConvertTo(context As System.ComponentModel.ITypeDescriptorContext, destinationType As System.Type) As Boolean

            Dim i As Integer,
                c As Integer = BlobConst.UBound

            For i = 0 To c
                If destinationType = Blob.Types(i) Then Return True
                If destinationType = Blob.ArrayTypes(i) Then Return True
            Next

            If destinationType.IsValueType Then
                Return True
            End If

            If destinationType.IsClass AndAlso destinationType.BaseType = Blob.Types(BlobTypes.Image) Then Return True
            If destinationType = GetType(InstanceDescriptor) Then Return True

            Return MyBase.CanConvertTo(context, destinationType)
        End Function

        Public Overrides Function ConvertFrom(context As System.ComponentModel.ITypeDescriptorContext, culture As System.Globalization.CultureInfo, value As Object) As Object
            If value Is Nothing Then Return Nothing

            If value.GetType.IsEnum = False AndAlso value.GetType.IsArray = True Then
                If value.Length = 0 Then Return Array.CreateInstance(GetType(Byte), 0)
            End If

            Dim bl As New Blob

            Dim vType As Type = value.GetType
            Dim eType As Type

            If vType = GetType(Blob) Then
                bl = New Blob(CType(value, Blob))
                Return bl
            End If

            If vType.IsArray Then eType = value.GetType.GetElementType Else eType = vType

            If vType.IsClass AndAlso ((vType.BaseType = Blob.Types(BlobTypes.Image)) OrElse (vType = Blob.Types(BlobTypes.Image))) Then
                bl = New Blob(ImageToBytes(value))
                bl.BlobType = BlobTypes.Image
                Return bl
            End If

            If vType.IsEnum = True Then
                value = EnumToBytes(value)

                bl.Type = vType
                bl = New Blob(CType(value, Byte()))
                bl.TypeLen = bl.Length

                Return bl

            ElseIf eType.IsEnum = True Then

                bl = EnumToBytes(value)
                bl.Type = vType
                bl.TypeLen = Marshal.SizeOf(value(0))

                Return bl

            End If

            Select Case vType

                Case GetType(Boolean)
                    Dim bol As Boolean = value

                    If bl Is Nothing Then bl = New Blob

                    bl.Type = vType
                    bl.TypeLen = 1
                    If bl.Length < 1 Then bl.Length = 1

                    If bol Then
                        bl.ByteAt(0) = 1
                    Else
                        bl.ByteAt(0) = 0
                    End If

                    Return bl

                Case GetType(BigInteger)
                    Dim be As BigInteger = value
                    bl = be.ToByteArray
                    bl.Type = vType
                    bl.TypeLen = bl.Length

                    Return bl

                Case GetType(Date)
                    bl = DateToBytes(value)
                    bl.Type = vType
                    bl.TypeLen = Marshal.SizeOf(value)

                    Return bl

                Case GetType(Date())
                    bl = DateArrayToBytes(value)
                    bl.Type = vType
                    bl.TypeLen = Marshal.SizeOf(value(0))

                    Return bl

                Case GetType(Byte())
                    bl.Type = vType
                    bl.TypeLen = 1
                    bl = CType(value, Byte())

                    Return bl

                Case GetType(SByte())
                    bl = ToByteArray(value)
                    bl.Type = vType
                    bl.TypeLen = 1

                    Return bl

                Case GetType(String)
                    If value Is Nothing Then
                        value = ""
                    Else
                        value = CType(value, String).Trim(ChrW(0))
                    End If

                    If value = "" Then
                        bl.Type = GetType(String)
                        bl.TypeLen = Marshal.SizeOf("A"c)

                        Return bl
                    End If

                    bl = Blob.Parse(value)

                    'bl.Type = GetType(String)
                    'bl.TypeLen = Marshal.SizeOf("A"c)
                    'bl.Alloc(CStr(value).Length * 2)
                    'Native.CopyMemory(bl.handle, CStr(value), CType(CStr(value).Length * 2, IntPtr))
                    Return bl

                Case GetType(Guid)

                    bl &= (value.ToByteArray)
                    bl.BlobType = BlobTypes.Guid
                    bl.ToString()
                    Return bl

                Case GetType(Guid())

                    Dim g As Guid

                    bl.BlobType = BlobTypes.Guid

                    For Each g In value
                        bl &= (g.ToByteArray)
                    Next

                    Return bl

                Case GetType(System.Drawing.Color)

                    Dim cl As Color
                    cl = value

                    bl = cl.ToArgb
                    bl.BlobType = BlobTypes.Color

                    Return bl

                Case GetType(System.Drawing.Color())

                    Dim cl() As Color
                    cl = value
                    bl.BlobType = BlobTypes.Color
                    bl.Length = 4 * cl.Length

                    Dim gh = GCHandle.Alloc(cl, GCHandleType.Pinned)
                    MemCpy(bl.DangerousGetHandle, gh.AddrOfPinnedObject, bl.Length)
                    gh.Free()
                    Return bl

                Case GetType(Decimal())
                    Dim dec() As Decimal = value
                    bl.Length = dec.Length * 16
                    bl.BlobType = BlobTypes.Decimal
                    MemCpy(bl.DangerousGetHandle, dec, dec.Length * 16)

                Case GetType(Decimal)
                    bl.BlobType = BlobTypes.Decimal
                    bl.Length = 16
                    bl.DecimalAt(0) = CDec(value)

                Case GetType(Double), GetType(Single), GetType(Long), GetType(ULong),
                     GetType(Integer), GetType(UInteger), GetType(Short), GetType(UShort), GetType(Byte), GetType(SByte), GetType(Char)

                    Select Case vType
                        Case GetType(ULong)
                            bl.Length = 8
                            bl.ULongAt(0) = value

                        Case GetType(UInteger)
                            bl.Length = 4
                            bl.UIntegerAt(0) = value

                        Case GetType(UShort)
                            bl.Length = 2
                            bl.UShortAt(0) = value

                        Case GetType(SByte)
                            Dim u() As Byte = ToByteArray({value})
                            bl = u
                            bl.Type = vType
                            bl.TypeLen = Marshal.SizeOf(u(0))

                        Case GetType(Byte)
                            bl = CType({value}, Byte())
                            bl.Type = vType
                            bl.TypeLen = Marshal.SizeOf(value)

                        Case Else

                            bl = CType(BitConverter.GetBytes(value), Byte())
                            bl.Type = vType
                            bl.TypeLen = Marshal.SizeOf(value)

                    End Select
                    bl.BlobType = Blob.TypeToBlobType(vType)
                    Return bl

                Case GetType(Double()), GetType(Single()), GetType(Long()), GetType(ULong()),
                     GetType(Integer()), GetType(UInteger()), GetType(Short()), GetType(UShort()), GetType(SByte()), GetType(Char())

                    Dim a As Object

                    Select Case vType
                        Case GetType(SByte())
                            a = ToByteArray(value)

                        Case GetType(UShort())
                            a = ToShortArray(value)

                        Case GetType(UInteger())
                            a = ToIntegerArray(value)

                        Case GetType(ULong())
                            a = ToLongArray(value)

                        Case Else
                            a = value

                    End Select

                    Dim l As Integer,
                        e As Integer

                    l = value.Length
                    e = Marshal.SizeOf(value(0))
                    bl.Length = l * e

                    Marshal.Copy(a, 0, bl.DangerousGetHandle, l)

                    bl.Type = vType
                    bl.TypeLen = e

                    Return bl

            End Select

            If value.GetType.IsValueType Then
                StructureToBlob(value, bl)
                Return bl
            End If

            Return MyBase.ConvertFrom(context, culture, value)
        End Function

        Public Overrides Function ConvertTo(context As System.ComponentModel.ITypeDescriptorContext, culture As System.Globalization.CultureInfo, value As Object, destinationType As System.Type) As Object
            Dim bl As Blob = CType(value, Blob)
            bl.Align()

            If bl.fOwn = True Then
                If bl.Length = 0 Then
                    If destinationType = GetType(String) Then
                        Return ""
                    Else
                        Return Nothing
                    End If
                End If
            ElseIf destinationType = GetType(String) Then
                Return bl.GrabString(0)
            End If

            If (destinationType Is GetType(InstanceDescriptor)) Then

                '' See the next class converter for details on
                '' InstanceDescriptor conversion

                Dim objC As System.Reflection.ConstructorInfo
                'objC = objT.GetType.GetConstructor(New Type() {GetType(Single), GetType(Single), GetType(Single), GetType(Single), GetType(Ruler.RulerUnits)})
                'Return New InstanceDescriptor(objC, New Object() {objT.Left, objT.Top, objT.Width, objT.Height, objT.Units()})

                objC = bl.GetType.GetConstructor(New Type() {GetType(Byte()), GetType(System.Type)})
                Return New InstanceDescriptor(objC, New Object() {CType(value, Blob), CType(value, Blob).Type})

            End If

            If destinationType.IsEnum = True Then
                Return BytesToEnum(bl, destinationType)
            ElseIf destinationType.IsArray = True AndAlso destinationType.GetElementType.IsEnum = True Then
                Return BytesToEnum(bl, destinationType.GetElementType)
            End If

            If destinationType.IsClass AndAlso ((destinationType.BaseType = Blob.Types(BlobTypes.Image)) OrElse (destinationType = Blob.Types(BlobTypes.Image))) Then
                Return BytesToImage(bl.GrabBytes)
            End If

            Select Case destinationType

                Case GetType(Boolean)
                    Return (bl.ByteAt(0) <> 0)

                Case GetType(BigInteger)
                    Return New BigInteger(CType(bl, Byte()))

                Case GetType(Date)
                    Return BytesToDate(bl)

                Case GetType(Date())
                    Return BytesToDateArray(bl)

                Case GetType(Byte())
                    Dim a() As Byte

                    ReDim a(bl.Length - 1)
                    Array.Copy(CType(bl, Byte()), a, bl.Length)
                    Return a

                Case GetType(SByte())
                    Return ToSByteArray(bl)

                Case GetType(Guid()), GetType(Guid)

                    If destinationType = GetType(Guid) Then
                        Return New Guid(bl.GrabBytes(0, 16))
                    End If

                    Dim l As Integer = 16
                    Dim e As Integer = bl.Length / l

                    Dim i As Integer,
                        c As Integer = bl.Length - 1

                    Dim gs() As Guid
                    ReDim gs(e - 1)
                    e = 0

                    For i = 0 To c Step l
                        gs(e) = New Guid(bl.GrabBytes(i, l))
                        e += 1
                    Next

                    Return gs


                Case GetType(Color()), GetType(Color)

                    If destinationType = GetType(Color) Then
                        Dim cc As Color
                        cc = Color.FromArgb(bl)
                        Return cc
                    End If

                    Dim l As Integer = 4
                    Dim e As Integer = bl.Length / l

                    Dim i As Integer,
                        c As Integer = bl.Length - 1

                    Dim cs() As Color
                    ReDim cs(e - 1)
                    e = 0
                    Dim ptr As IntPtr = bl.DangerousGetHandle

                    For i = 0 To c Step l
                        Native.CopyMemory(l, ptr, CType(4, IntPtr))
                        cs(e) = Color.FromArgb(l)
                        ptr += l
                        e += 1
                    Next

                    Return cs

                Case GetType(String)
                    If bl.Length = 0 Then Return ""
                    Return bl.ToString

                Case GetType(Decimal()), GetType(Decimal)

                    Dim d() As Decimal,
                        ints() As Integer = (bl)

                    If ints.Length Mod 4 Then
                        Throw New ArgumentException("Byte array is not aligned for the Decimal data type.")
                        Return Nothing
                    End If

                    If destinationType = GetType(Decimal) Then
                        If ints.Length <> 4 Then ReDim Preserve ints(3)
                        Return New Decimal(ints)
                    End If

                    Dim dec(3) As Integer

                    Dim e As Integer = bl.Count - 1,
                        i As Integer

                    ReDim d(e)

                    For i = 0 To e
                        Array.Copy(ints, i, dec, 0, 4)
                        d(i) = New Decimal(dec)
                    Next

                    Return d

                Case GetType(Double)

                    Return BitConverter.ToDouble(CType(bl, Byte()), 0)

                Case GetType(Single)

                    Return BitConverter.ToSingle(CType(bl, Byte()), 0)

                Case GetType(ULong)

                    Dim u() As ULong = ToULongArray({BitConverter.ToInt64(CType(bl, Byte()), 0)})
                    Return u(0)

                Case GetType(Long)

                    Return BitConverter.ToInt64(CType(bl, Byte()), 0)

                Case GetType(UInteger)

                    Dim u() As UInteger = ToUIntegerArray({BitConverter.ToInt32(CType(bl, Byte()), 0)})
                    Return u(0)

                Case GetType(Integer)

                    Return BitConverter.ToInt32(CType(bl, Byte()), 0)

                Case GetType(UShort)

                    Dim u() As UShort = ToUShortArray({BitConverter.ToInt16(CType(bl, Byte()), 0)})
                    Return u(0)

                Case GetType(Short)

                    Return BitConverter.ToInt16(CType(bl, Byte()), 0)

                Case GetType(Char)

                    Return BitConverter.ToChar(CType(bl, Byte()), 0)

                Case GetType(Byte)

                    Return bl.ByteAt(0)

                Case GetType(SByte)

                    Dim u() As SByte = ToSByteArray(CType(bl, Byte()))
                    Return u(0)

                Case GetType(Double()), GetType(Single()), GetType(Long()), GetType(ULong()),
                     GetType(Integer()), GetType(UInteger()), GetType(Short()), GetType(UShort()), GetType(Char())

                    Dim a As Object = Array.CreateInstance(destinationType.GetElementType, 1)

                    Dim l As Integer,
                        e As Integer,
                        f As Integer = 0

                    Dim ptr As IntPtr

                    Dim b() As Byte = bl

                    l = Marshal.SizeOf(a(0))
                    e = b.Length / l
                    l = b.Length

                    Select Case destinationType.GetElementType

                        Case GetType(SByte)
                            a = Array.CreateInstance(GetType(Byte), e)

                        Case GetType(UShort)
                            a = Array.CreateInstance(GetType(Short), e)

                        Case GetType(UInteger)
                            a = Array.CreateInstance(GetType(Integer), e)

                        Case GetType(ULong)
                            a = Array.CreateInstance(GetType(Long), e)

                        Case Else
                            a = Array.CreateInstance(destinationType.GetElementType, e)

                    End Select

                    ptr = Marshal.AllocCoTaskMem(l)

                    Marshal.Copy(b, 0, ptr, l)
                    Marshal.Copy(ptr, a, 0, e)

                    Marshal.FreeCoTaskMem(ptr)

                    Select Case destinationType.GetElementType

                        Case GetType(SByte)
                            a = ToSByteArray(a)

                        Case GetType(UShort)
                            a = ToUShortArray(a)

                        Case GetType(UInteger)
                            a = ToUIntegerArray(a)

                        Case GetType(ULong)
                            a = ToULongArray(a)

                    End Select

                    Return a

            End Select

            If destinationType.IsValueType Then
                Dim o As Object = Nothing
                BlobToStructure(bl, o)
                Return o
            End If

            Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Function

        Public Overrides Function CreateInstance(context As System.ComponentModel.ITypeDescriptorContext, propertyValues As System.Collections.IDictionary) As Object
            Dim bl As New Blob


            Return bl
        End Function

#End Region

        Public Sub New()

        End Sub
    End Class

#End Region

End Namespace
