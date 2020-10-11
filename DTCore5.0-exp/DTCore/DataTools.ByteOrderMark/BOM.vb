'' ************************************************* ''
'' DataTools Visual Basic Utility Library 
''
'' Module: Byte Order Marker Library
''         For Mulitple Character Encodings
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''


Option Explicit On
Option Strict Off
Option Compare Binary

Imports System.Text
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports DataTools.Memory
Imports DataTools.Strings

Namespace ByteOrderMark

#Region "Enums"

    Public Enum CPCharTypes
        Undefined = 0
        Control = 1
        Punctuation = 2
        Alpha = 3
        Digit = 5
        Symbol = 6
        Other = 8
    End Enum

    Public Enum ControlLayers
        C0 = 0
        C1 = 1
    End Enum

    Public Enum ControlCodes

        ' C0 control constants
        NUL = &H0
        SOH = &H1
        STX = &H2
        ETX = &H3
        EOT = &H4
        ENQ = &H5
        ACK = &H6
        BEL = &H7
        BS = &H8
        HT = &H9
        LF = &HA
        VT = &HB
        FF = &HC
        CR = &HD
        SO = &HE
        SI = &HF
        DLE = &H10
        DC1 = &H11
        DC2 = &H12
        DC3 = &H13
        DC4 = &H14
        NAK = &H15
        SYN = &H16
        ETB = &H17
        CAN = &H18
        EM = &H19
        [SUB] = &H1A
        ESC = &H1B
        FS = &H1C
        GS = &H1D
        RS = &H1E
        US = &H1F
        SP = &H20
        DEL = &H7F

        ' C1 control constants
        PAD = &H80
        HOP = &H81
        BPH = &H82
        NBH = &H83
        IND = &H84
        NEL = &H85
        SSA = &H86
        ESA = &H87
        HTS = &H88
        HTJ = &H89
        VTS = &H8A
        PLD = &H8B
        PLU = &H8C
        RI = &H8D
        SS2 = &H8E
        SS3 = &H8F
        DCS = &H90
        PU1 = &H91
        PU2 = &H92
        STS = &H93
        CCH = &H94
        MW = &H95
        SPA = &H96
        EPA = &H97
        SOS = &H98
        SGCI = &H99
        SCI = &H9A
        CSI = &H9B
        ST = &H9C
        OSC = &H9D
        PM = &H9E
        APC = &H9F

        '' EBCDIC Specific
        NSP = &HE1
        EO = &HFF

    End Enum

    Public Enum CPSignalEscapes
        None = 0
        Ctrl = 1
        Esc = 2
    End Enum

#End Region

#Region "CodePage Structures"

    <Description("Contains an escape sequence")> _
    <StructLayout(LayoutKind.Sequential, Pack:=1), TypeConverter(GetType(ExpandableObjectConverter))> _
    Public Structure EscapeSequence
        Public Control As CPSignalEscapes
        Public Character As Char

        Public Sub New(ctrl As CPSignalEscapes, ch As Char)
            Control = ctrl
            Character = ch
        End Sub

        Public Overrides Function ToString() As String

            If Control.ToString <> "None" Then Return Control.ToString & "+" & Character Else Return Character

        End Function
    End Structure

    <Description("Describes a control signal character.")> _
    <StructLayout(LayoutKind.Sequential, Pack:=1), TypeConverter(GetType(ExpandableObjectConverter))> _
    Public Structure SignalElement

        Private _Code As ControlCodes
        Private _Layer As ControlLayers
        Private _Sequence As EscapeSequence
        Private _Symbol As Char
        Private _Escape As Char
        Private _Descrption As String
        Private _Acronym As String
        Private _Name As String

        Public Property Code As ControlCodes
            Get
                Return _Code
            End Get
            Friend Set(value As ControlCodes)
                _Code = value
            End Set
        End Property

        Public Property Layer As ControlLayers
            Get
                Return _Layer
            End Get
            Friend Set(value As ControlLayers)
                _Layer = value
            End Set
        End Property

        Public Property Sequence As EscapeSequence
            Get
                Return _Sequence
            End Get
            Friend Set(value As EscapeSequence)
                _Sequence = value
            End Set
        End Property

        Public Property Symbol As Char
            Get
                Return _Symbol
            End Get
            Friend Set(value As Char)
                _Symbol = value
            End Set
        End Property

        Public Property Escape As Char
            Get
                Return _Escape
            End Get
            Friend Set(value As Char)
                _Escape = value
            End Set
        End Property

        Public Property Descrption As String
            Get
                Return _Descrption
            End Get
            Friend Set(value As String)
                _Descrption = value
            End Set
        End Property

        Public Property Acronym As String
            Get
                Return _Acronym
            End Get
            Friend Set(value As String)
                _Acronym = value
            End Set
        End Property

        Public Property Name As String
            Get
                Return _Name
            End Get
            Friend Set(value As String)
                _Name = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Code.ToString
        End Function

        Public Shared Function Parse(subject As String) As SignalElement
            Dim cc As New SignalElement
            Dim s() As String = BatchParse(subject, "|")
            Dim sd() As String

            sd = BatchParse(subject, vbTab)

            Select Case sd.Length

                Case 8
                    ' C0 code
                    ' Signals.AddSignal("Seq	Dec	Hex	Acro	Symb	Name	C	Description")

                    cc.Layer = ControlLayers.C0

                    If (sd(0).Chars(0) = "^"c) Then
                        cc.Sequence = New EscapeSequence(CPSignalEscapes.Ctrl, sd(0).Chars(1))
                    Else
                        cc.Sequence = New EscapeSequence(CPSignalEscapes.None, sd(0).Chars(0))
                    End If

                    cc.Code = [Enum].Parse(GetType(ControlCodes), sd(3))
                    cc.Acronym = cc.ToString
                    If sd(6) <> "" Then cc.Escape = sd(6).Chars(1)
                    cc.Descrption = SearchReplace(sd(7), "\n", vbCrLf)
                    cc.Name = sd(5)
                    cc.Symbol = sd(4)

                Case 6
                    ' C1 code
                    ' Signals.AddSignal("Seq	Dec	Hex	Acro	Name	Description")

                    cc.Layer = ControlLayers.C1
                    cc.Sequence = New EscapeSequence(CPSignalEscapes.Esc, sd(0).Chars(0))
                    cc.Code = [Enum].Parse(GetType(ControlCodes), sd(3))
                    cc.Acronym = cc.ToString
                    cc.Descrption = SearchReplace(sd(5), "\n", vbCrLf)
                    cc.Name = sd(4)

                Case 5

                    cc.Layer = ControlLayers.C1
                    cc.Sequence = New EscapeSequence(CPSignalEscapes.Esc, sd(0).Chars(0))
                    cc.Code = [Enum].Parse(GetType(ControlCodes), sd(3))
                    cc.Acronym = cc.ToString
                    'cc.Descrption = SearchReplace(sd(4), "\n", vbCrLf)
                    cc.Name = sd(4)

            End Select

            Return cc

        End Function

        Public Shared Widening Operator CType(operand As SignalElement) As ControlCodes
            Return operand.Code
        End Operator

        Public Shared Narrowing Operator CType(operand As ControlCodes) As SignalElement
            Dim cc As New SignalElement
            cc.Code = operand
            cc.Acronym = operand.ToString

            If cc.Code < &H80 Then
                cc.Layer = ControlLayers.C0
            Else
                cc.Layer = ControlLayers.C1
            End If

            Return cc
        End Operator

    End Structure

    <StructLayout(LayoutKind.Sequential, Pack:=1), TypeConverter(GetType(ExpandableObjectConverter))> _
    Public Structure CodePageElement
        Private _Type As CPCharTypes
        Private _Signal As SignalElement
        Private _Code As UInteger
        Private _Unicode As Char
        Private _Ascii As Char
        Private _Description As String

        Public Property Type As CPCharTypes
            Get
                Return _Type
            End Get
            Friend Set(value As CPCharTypes)
                _Type = value
            End Set
        End Property

        Public Property Signal As SignalElement
            Get
                Return _Signal
            End Get
            Friend Set(value As SignalElement)
                _Signal = value
            End Set
        End Property

        Public Property Code As UInteger
            Get
                Return _Code
            End Get
            Friend Set(value As UInteger)
                _Code = value
            End Set
        End Property

        Public Property Unicode As Char
            Get
                Return _Unicode
            End Get
            Friend Set(value As Char)
                _Unicode = value
            End Set
        End Property

        Public Property Ascii As Char
            Get
                Return _Ascii
            End Get
            Friend Set(value As Char)
                _Ascii = value
            End Set
        End Property

        Public Property Description As String
            Get
                Return _Description
            End Get
            Friend Set(value As String)
                _Description = value
            End Set
        End Property

        Public Shared Function Parse(subject As String) As CodePageElement
            Dim cp As New CodePageElement
            Dim s() As String = BatchParse(subject, "|")
            Dim sd() As String
            Dim cc As ControlCodes = ControlCodes.NUL

            cp.Type = [Enum].Parse(GetType(CPCharTypes), s(0))
            If (Trim(s(1))) <> "" Then cp.Unicode = CChar(ChrW(Val(s(1))))
            If s.Length = 3 Then
                cp.Code = Val(s(2))
                Return cp
            End If

            cp.Code = Val(s(3))
            If cp.Type = CPCharTypes.Undefined Then Return cp

            If s(2).IndexOf(",") > -1 Then
                sd = BatchParse(s(2), ",")
                cp.Description = sd(0)
                cp.Signal = New SignalElement
                If [Enum].TryParse(Of ControlCodes)(sd(1), cc) Then
                    cp.Signal = Signals.Item(cc)
                    cp.Ascii = Chr(cp.Signal.Code)
                Else
                    If sd(1).Length > 1 AndAlso sd(1).Chars(0) = "&"c Then
                        cp.Ascii = Chr(Val(sd(1)))
                    Else
                        cp.Ascii = sd(1).Chars(0)
                    End If
                End If

            Else
                If s(2).Length > 1 AndAlso s(2).Chars(0) = "&"c Then
                    cp.Ascii = Chr(Val(s(2)))
                Else
                    cp.Ascii = s(2).Chars(0)
                End If
            End If

            Return cp

        End Function

    End Structure

#End Region

#Region "BOM"

    Public Enum BOMTYPE As Short
        ASCII = 0
        UTF8 = 2
        UTF16BE = 3
        UTF16LE = 4
        UTF32BE = 5
        UTF32LE = 6
        UTF7a = 7
        UTF7b = 8
        UTF7c = 9
        UTF7d = 10
        UTF7e = 11
        UTF1 = 12
        UTFEBCDIC = 13
        SCSU = 14
        SCSU_BOCU1 = 15
        SCSU_GB18030 = 16
    End Enum

    Public Structure BOM

        Friend _Type As BOMTYPE
        Friend _BOM As Byte()

        Private Shared mInit As Boolean
        Private myInit As Boolean

        Public Shared UTF8 As BOM
        Public Shared UTF16BE As BOM
        Public Shared UTF16LE As BOM
        Public Shared UTF32BE As BOM
        Public Shared UTF32LE As BOM
        Public Shared UTF7a As BOM
        Public Shared UTF7b As BOM
        Public Shared UTF7c As BOM
        Public Shared UTF7d As BOM
        Public Shared UTF7e As BOM
        Public Shared UTF1 As BOM
        Public Shared UTFEBCDIC As BOM
        Public Shared SCSU As BOM
        Public Shared SCSU_BOCU1 As BOM
        Public Shared SCSU_GB18030 As BOM

        Shared Sub New()

            If mInit Then Return
            mInit = True

            UTF8 = New BOM({CByte(&HEF), CByte(&HBB), CByte(&HBF)}, "UTF8")
            UTF16BE = New BOM({CByte(&HFE), CByte(&HFF)}, "UTF16BE")

            '' Windows default UTF16LEWindows")
            UTF16LE = New BOM({CByte(&HFF), CByte(&HFE)}, "UTF16LE")

            UTF32BE = New BOM({CByte(&H0), CByte(&H0), CByte(&HFE), CByte(&HFF)}, "UTF32BE")
            UTF32LE = New BOM({CByte(&HFF), CByte(&HFE), CByte(&H0), CByte(&H0)}, "UTF32LE")
            UTF7a = New BOM({CByte(&H2B), CByte(&H2F), CByte(&H76), CByte(&H38)}, "UTF7a")
            UTF7b = New BOM({CByte(&H2B), CByte(&H2F), CByte(&H76), CByte(&H39)}, "UTF7b")
            UTF7c = New BOM({CByte(&H2B), CByte(&H2F), CByte(&H76), CByte(&H2B)}, "UTF7c")
            UTF7d = New BOM({CByte(&H2B), CByte(&H2F), CByte(&H76), CByte(&H2F)}, "UTF7d")
            UTF7e = New BOM({CByte(&H2B), CByte(&H2F), CByte(&H76), CByte(&H38), CByte(&H2D)}, "UTF7e")
            UTF1 = New BOM({CByte(&HF7), CByte(&H64), CByte(&H4C)}, "UTF1")
            UTFEBCDIC = New BOM({CByte(&HDD), CByte(&H73), CByte(&H66), CByte(&H73)}, "UTFEBCDIC")
            SCSU = New BOM({CByte(&HE), CByte(&HFE), CByte(&HFF)}, "SCSU")
            SCSU_BOCU1 = New BOM({CByte(&HFB), CByte(&HEE), CByte(&H28)}, "SCSU_BOCU1")
            SCSU_GB18030 = New BOM({CByte(&H84), CByte(&H31), CByte(&H95), CByte(&H33)}, "SCSU_GB18030")

        End Sub

        Public Sub New(bom As BOM)
            ReDim Me._BOM(bom._BOM.Length - 1)
            Array.Copy(bom._BOM, _BOM, _BOM.Length)

            Me._Type = bom._Type
        End Sub

        Private Sub New(bytes() As Byte, typeName As String)

            ReDim _BOM(bytes.Length - 1)
            bytes.CopyTo(_BOM, 0)
            [Enum].TryParse(Of BOMTYPE)(typeName, _Type)

        End Sub

        Private Sub New(bytes() As Byte, type As BOMTYPE)

            ReDim _BOM(bytes.Length - 1)
            bytes.CopyTo(_BOM, 0)
            _Type = type

        End Sub

        Public Property Type As BOMTYPE
            Get
                If Not myInit Then
                    _Type = BOMTYPE.UTF16LE
                    _BOM = UTF16LE
                    myInit = True
                End If
                Return _Type
            End Get
            Set(value As BOMTYPE)
                SetBOM(value)
            End Set
        End Property

        Public ReadOnly Property Bytes As Byte()
            Get
                If Not myInit Then
                    _Type = BOMTYPE.UTF16LE
                    _BOM = UTF16LE
                    myInit = True
                End If
                Return _BOM
            End Get
        End Property

        Public Sub SetBOM(type As BOMTYPE)
            If Not myInit Then myInit = True
            Erase _BOM
            _Type = type
            If type = BOMTYPE.ASCII Then Return
            _BOM = GetBOM(type)
        End Sub

        Public Shared Function ByteEquals(operand1 As Byte(), operand2 As BOM) As Boolean
            Dim i As Integer, _
                c As Integer = operand2._BOM.Length - 1

            If operand1.Length < operand2._BOM.Length Then Return False

            For i = 0 To c
                If operand1(i) <> operand2._BOM(i) Then Return False
            Next

            Return True
        End Function

        Public Overrides Function Equals(operand As Object) As Boolean
            Return CompareBytes(_BOM, operand.BOM)
        End Function

        Public Overloads Shared Function StripBOM(value As Byte()) As Byte()
            Dim a As New BOM
            Return StripBOM(value, a)
        End Function

        Public Overloads Shared Function StripBOM(value As Byte(), ByRef BOM As BOM) As Byte()
            Dim b As BOM = Parse(value)

            If b._BOM IsNot Nothing AndAlso b._BOM.Length Then
                Dim by() As Byte, _
                    c As Integer = value.Length

                c -= b._BOM.Length
                ReDim by(c - 1)

                Array.Copy(value, b._BOM.Length, by, 0, c)
                Return by
            Else
                BOM.Type = BOMTYPE.ASCII
                Return value
            End If

            ReDim BOM._BOM(b._BOM.Length - 1)
            b._BOM.CopyTo(BOM._BOM, 0)
            BOM._Type = b.Type

            Return value

        End Function

        Public Overloads Shared Function GetBOM(type As String) As Byte()
            Dim b As New BOM

            b._Type = CType(System.Enum.Parse(GetType(BOMTYPE), type, False), BOMTYPE)
            Return GetBOM(b._Type)
        End Function

        Public Overloads Shared Function GetBOM(type As BOMTYPE) As Byte()
            Dim m As System.Reflection.MemberInfo()
            Dim mt As System.Reflection.MemberInfo
            Dim fi As System.Reflection.FieldInfo
            Dim by() As Byte = Nothing
            Dim b As New BOM

            m = GetType(BOM).GetMembers()

            For Each mt In m
                If mt.MemberType = Reflection.MemberTypes.Field Then
                    If mt.Name = type.ToString Then
                        fi = CType(mt, System.Reflection.FieldInfo)
                        b = fi.GetValue(b)
                        by = b._BOM

                        Exit For
                    End If
                End If
            Next

            Return by

        End Function

        Public Overrides Function ToString() As String
            Dim i As Integer, _
                c As Integer

            Dim s As String = ""

            c = _BOM.Length - 1
            For i = 0 To c
                If (i) Then s &= ", "
                s &= PadHex(_BOM(i), 2, "0x", True)
            Next

            Return Type.ToString & " [" & s & "]"
        End Function

        Public Overloads Function Encode(value As Byte()) As Byte()
            Dim b() As Byte

            Dim d() As Byte

            ReDim b(_BOM.Length)
            Array.Copy(_BOM, b, _BOM.Length)

            Select Case _Type
                Case BOMTYPE.ASCII
                    d = Encoding.Convert(Encoding.Unicode, Encoding.ASCII, value)

                Case BOMTYPE.UTF16BE
                    d = Encoding.Convert(Encoding.Unicode, Encoding.BigEndianUnicode, value)

                Case BOMTYPE.UTF16LE
                    d = value

                Case Else
                    d = value

            End Select

            ReDim b(_BOM.Length)
            Array.Copy(_BOM, b, _BOM.Length)

            If value Is Nothing OrElse value.Length = 0 Then Return b
            b.Concat(d)

            Return b
        End Function

        Public Overloads Function Encode(value As String) As Byte()
            Return Encode(Encoding.Unicode.GetBytes(value))
        End Function

        Public Overloads Shared Function Parse(value() As Byte) As BOM
            Dim b As BOM
            Dim m As System.Reflection.MemberInfo()
            Dim mt As System.Reflection.MemberInfo
            Dim fi As System.Reflection.FieldInfo
            Dim by() As Byte
            Dim i As Integer,
                c As Integer

            Dim y As Boolean = False

            b = New BOM
            b.Type = BOMTYPE.ASCII

            m = GetType(BOM).GetMembers()

            For Each mt In m
                If mt.MemberType = Reflection.MemberTypes.Field Then
                    If mt.Name <> "BOM" And mt.Name <> "Type" Then
                        fi = CType(mt, System.Reflection.FieldInfo)
                        If fi.FieldType <> GetType(BOM) Then Continue For
                        by = CType(fi.GetValue(b), BOM)._BOM

                        If value.Length < by.Length Then Continue For
                        c = by.Length - 1

                        y = True
                        For i = 0 To c
                            If value(i) <> by(i) Then
                                y = False
                                Exit For
                            End If
                        Next

                        If y Then
                            If b._Type = BOMTYPE.ASCII OrElse by.Length > b._BOM.Length Then
                                b._Type = CType(System.Enum.Parse(GetType(BOMTYPE), fi.Name, False), BOMTYPE)
                                If b._BOM IsNot Nothing AndAlso b._BOM.Length > 0 Then Erase b._BOM
                                ReDim b._BOM(by.Length - 1)
                                Array.Copy(by, b._BOM, by.Length)
                            End If

                            y = False
                        End If

                    End If
                End If
            Next

            Return b
        End Function

        Public Shared Widening Operator CType(operand As BOM) As String
            Return Encoding.ASCII.GetString(operand._BOM)
        End Operator

        Public Shared Narrowing Operator CType(operand As String) As BOM
            Dim b As BOM = Parse(Encoding.ASCII.GetBytes(operand))
            Return b
        End Operator

        Public Shared Widening Operator CType(operand As BOM) As Byte()
            Dim b() As Byte
            ReDim b(operand._BOM.Length - 1)
            operand._BOM.CopyTo(b, 0)
            Return b
        End Operator

        Public Shared Narrowing Operator CType(operand As Byte()) As BOM
            Dim b As BOM = Parse(CType(operand, Byte()))
            Return b
        End Operator

        Public Shared Widening Operator CType(operand As BOM) As Integer()
            Dim bl As Blob = operand._BOM
            Blob.ReRank(bl, BlobTypes.Integer)

            Return CType(CType(bl, MemPtr), Integer())
        End Operator

        Public Shared Narrowing Operator CType(operand As Integer()) As BOM
            Dim bl As Blob = operand
            Blob.ReRank(bl, BlobTypes.Byte)
            Return New BOM(CType(bl, Byte()))
        End Operator

        Public Shared Operator =(operand1 As BOM, operand2 As BOM) As Boolean
            Return operand1.Equals(operand2)
        End Operator

        Public Shared Operator <>(operand1 As BOM, operand2 As BOM) As Boolean
            Return Not operand1.Equals(operand2)
        End Operator

        Public Shared Operator =(operand1 As BOM, operand2 As Byte()) As Boolean
            Return ByteEquals(operand2, operand1)
        End Operator

        Public Shared Operator <>(operand1 As BOM, operand2 As Byte()) As Boolean
            Return Not ByteEquals(operand2, operand1)
        End Operator

        Public Shared Operator =(operand1 As Byte(), operand2 As BOM) As Boolean
            Return ByteEquals(operand1, operand2)
        End Operator

        Public Shared Operator <>(operand1 As Byte(), operand2 As BOM) As Boolean
            Return Not ByteEquals(operand1, operand2)
        End Operator

    End Structure

#End Region

#Region "Signal Collection for C0 and C1 standard control signals"
    <Serializable(), TypeConverter(GetType(ExpandableObjectConverter))> _
    Public Class ControlSignals
        Inherits CollectionBase

        Public Class SignalSorter
            Implements IComparer

            Public Function Compare(x As Object, y As Object) As Integer Implements System.Collections.IComparer.Compare
                Dim s1 As SignalElement = CType(x, SignalElement)
                Dim s2 As SignalElement = CType(y, SignalElement)

                Return (s1.Code - s2.Code)

            End Function
        End Class

        Public Sub Sort()
            Array.Sort(List, New SignalSorter)
        End Sub

        Public Sub AddSignal(subject As String)

            Dim cc As SignalElement = SignalElement.Parse(subject)
            List.Add(cc)

        End Sub

        Public ReadOnly Property InnerArray() As SignalElement()
            Get
                If Not signalInit Then
                    InitControlSignalCatalog()
                    signalInit = True
                End If
                Return CType(InnerList.ToArray(GetType(SignalElement)), SignalElement())
            End Get
        End Property

        Public Sub AddRange(sel() As SignalElement)
            InnerList.AddRange(sel)
        End Sub

        Public Sub InsertRange(index As Integer, sel() As SignalElement)
            InnerList.InsertRange(index, sel)
        End Sub

        Default Public Overloads Property Item(index As ControlCodes) As SignalElement
            Get
                If Not signalInit Then
                    InitControlSignalCatalog()
                    signalInit = True
                End If

                For Each cc As SignalElement In List
                    If cc.Code = index Then Return cc
                Next
                Return Nothing

            End Get
            Set(value As SignalElement)
                List(index) = CType(value, SignalElement)
            End Set
        End Property

        Default Public Overloads Property Item(index As Integer) As SignalElement
            Get
                If Not signalInit Then
                    InitControlSignalCatalog()
                    signalInit = True
                End If
                Return CType(List(index), SignalElement)
            End Get
            Set(value As SignalElement)
                List(index) = CType(value, SignalElement)
            End Set
        End Property

        Public Overloads Function IndexOf(value As SignalElement) As Integer
            If Not signalInit Then
                InitControlSignalCatalog()
                signalInit = True
            End If
            Return List.IndexOf(value)
        End Function 'IndexOf(value)

        Public Function Contains(value As SignalElement) As Boolean
            ' If value is not of type SignalElement, this will return false.
            Return List.Contains(value)
        End Function 'Contains

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            ' Insert additional code to be run only when inserting values.
        End Sub 'OnInsert

        Protected Overrides Sub OnRemove(index As Integer, value As Object)
            ' Insert additional code to be run only when removing values.
        End Sub 'OnRemove

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            ' Insert additional code to be run only when setting values.
        End Sub 'OnSet

        Protected Overrides Sub OnValidate(value As Object)
            If Not GetType(SignalElement).IsAssignableFrom(value.GetType()) Then
                Throw New ArgumentException("value must be of type Object.", "value")
            End If
        End Sub 'OnValidate 

        Public Sub New()
            
            '' initialization code goes here
        End Sub

    End Class

#End Region

#Region "Misc"

#Region "CodePage Class"

    <Serializable(), TypeConverter(GetType(ExpandableObjectConverter))> _
    Friend Class CodePage
        Inherits CollectionBase

        Private _Name As String
        Private mCA() As Char

        Friend Class CPSorter
            Implements IComparer

            Public Function Compare(x As Object, y As Object) As Integer Implements System.Collections.IComparer.Compare
                Dim cp1 As CodePageElement = CType(x, CodePageElement)
                Dim cp2 As CodePageElement = CType(y, CodePageElement)

                Return (cp1.Code - cp2.Code)
            End Function
        End Class

        Public Property Name() As String
            Get
                Return _Name
            End Get
            Set(value As String)
                _Name = value
            End Set
        End Property

        Public Sub Sort()
            Array.Sort(List, New CPSorter)
        End Sub

        Public Sub AddCode(code As String)
            Dim cp As CodePageElement = CodePageElement.Parse(code)
            Me.Add(cp)
            mCA = Nothing
        End Sub

        Public ReadOnly Property CodeArray As Char()
            Get
                If mCA Is Nothing Then
                    mCA = GetCodeArray()
                End If
                Return mCA
            End Get
        End Property

        Public Function ToUnicodeString(subject() As Byte) As String

            If mCA Is Nothing Then mCA = GetCodeArray()

            Dim i As Integer, _
                c As Integer = subject.Length - 1

            Dim ch() As Char

            ReDim ch(c)

            For i = 0 To c
                ch(i) = mCA(subject(i))
            Next

            Return ch
        End Function


        Public Function FromUnicodeString(subject As String) As Byte()

            If mCA Is Nothing Then mCA = GetCodeArray()

            Dim i As Integer, _
                c As Integer = subject.Length - 1

            Dim ch() As Byte

            ReDim ch(c)

            For i = 0 To c
                ch(i) = Array.IndexOf(mCA, subject.Chars(i))
            Next

            Return ch
        End Function

        Public Function GetCodeArray() As Char()
            Me.Sort()
            Dim objCP As CodePageElement
            Dim codes() As Char, _
                i As Integer

            objCP = List.Item(List.Count - 1)
            i = objCP.Code
            ReDim codes(i - 1)

            For Each objCP In List
                codes(objCP.Code) = objCP.Unicode
            Next

            Return codes
        End Function

        Public ReadOnly Property InnerArray() As CodePageElement()
            Get
                Return CType(InnerList.ToArray(GetType(CodePageElement)), CodePageElement())
            End Get
        End Property

        Public Sub AddRange(sel() As CodePageElement)

            InnerList.AddRange(sel)

        End Sub

        Public Sub InsertRange(index As Integer, sel() As CodePageElement)
            InnerList.InsertRange(index, sel)
        End Sub

        Default Public Overloads Property Item(index As Integer) As CodePageElement
            Get
                Return CType(List(index), CodePageElement)
            End Get
            Set(value As CodePageElement)
                List(index) = CType(value, CodePageElement)
            End Set
        End Property

        Public Overloads Function IndexOf(value As CodePageElement) As Integer
            Return List.IndexOf(value)
        End Function 'IndexOf(value)

        Public Sub Insert(index As Integer, value As CodePageElement)
            List.Insert(index, value)
        End Sub 'Insert

        Public Function Add(value As CodePageElement) As Integer
            Return List.Add(value)
        End Function

        Public Overloads Sub RemoveAt(index As Integer)
            If List.Count = 0 Then Return
            List.RemoveAt(index)
        End Sub

        Public Sub Remove(value As CodePageElement)
            List.Remove(value)
        End Sub 'Remove

        Public Function Contains(value As CodePageElement) As Boolean
            ' If value is not of type CodePageElement, this will return false.
            Return List.Contains(value)
        End Function 'Contains

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            ' Insert additional code to be run only when inserting values.
        End Sub 'OnInsert

        Protected Overrides Sub OnRemove(index As Integer, value As Object)
            ' Insert additional code to be run only when removing values.
        End Sub 'OnRemove

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            ' Insert additional code to be run only when setting values.
        End Sub 'OnSet

        Protected Overrides Sub OnValidate(value As Object)
            If Not GetType(CodePageElement).IsAssignableFrom(value.GetType()) Then
                Throw New ArgumentException("value must be of type Object.", "value")
            End If
        End Sub 'OnValidate 

        Public Sub New()
            MyBase.New()
            '' initialization code goes here
        End Sub

    End Class

#End Region

#Region "CodePageCollection Class"


    <Serializable(), TypeConverter(GetType(ExpandableObjectConverter))> _
    Friend Class CodePageCollection
        Inherits CollectionBase

        Public ReadOnly Property InnerArray() As CodePage()
            Get
                Return CType(InnerList.ToArray(GetType(CodePage)), CodePage())
            End Get
        End Property

        Public Sub AddRange(sel() As CodePage)

            InnerList.AddRange(sel)

        End Sub

        Public Function Add(value As CodePage) As Integer
            Return List.Add(value)
        End Function

        Public Sub InsertRange(index As Integer, sel() As CodePage)
            InnerList.InsertRange(index, sel)
        End Sub

        <Browsable(True)> _
        Default Public Overloads Property Item(index As Integer) As CodePage
            Get
                Return CType(List(index), CodePage)
            End Get
            Set(value As CodePage)
                List(index) = CType(value, CodePage)
            End Set
        End Property

        <Browsable(True)> _
        Default Public Overloads Property Item(name As String) As CodePage
            Get
                name = name.ToLower
                For Each cp As CodePage In List
                    If cp.Name.ToLower = name Then Return cp

                Next

                Return Nothing
            End Get
            Set(value As CodePage)
                Dim c As Integer = 0
                For Each cp As CodePage In List
                    If cp.Name.ToLower = name Then
                        List(c) = value
                        Exit For
                    End If
                    c += 1

                Next
            End Set
        End Property

        Public Overloads Function IndexOf(value As CodePage) As Integer
            Return List.IndexOf(value)
        End Function 'IndexOf(value)

        Public Sub Insert(index As Integer, value As CodePage, Optional key As String = "")
            List.Insert(index, value)
        End Sub 'Insert

        Public Overloads Sub RemoveAt(index As Integer)
            If List.Count = 0 Then Return
            List.RemoveAt(index)
        End Sub

        Public Sub Remove(value As CodePage)
            List.Remove(value)
        End Sub 'Remove

        Public Function Contains(value As CodePage) As Boolean
            ' If value is not of type CodePage, this will return false.
            Return List.Contains(value)
        End Function 'Contains

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            ' Insert additional code to be run only when inserting values.
        End Sub 'OnInsert

        Protected Overrides Sub OnRemove(index As Integer, value As Object)
            ' Insert additional code to be run only when removing values.
        End Sub 'OnRemove

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            ' Insert additional code to be run only when setting values.
        End Sub 'OnSet

        Protected Overrides Sub OnValidate(value As Object)
            If Not GetType(CodePage).IsAssignableFrom(value.GetType()) Then
                Throw New ArgumentException("value must be of type Object.", "value")
            End If
        End Sub 'OnValidate 

        Friend Sub New()
            MyBase.New()

            If signalInit = False Then
                InitControlSignalCatalog()
            End If

            If cpInit = False Then
                InitEBCDIC()
            End If

            cpInit = True
        End Sub

    End Class

#End Region

#Region "Global CodePage Namespace"

    Public Module CPGlobal

        Friend CodesPages As New CodePageCollection
        Friend Signals As New ControlSignals

        Friend signalInit As Boolean = False
        Friend cpInit As Boolean = False

#Region "Reader-Writer functions that use BOMs to read and write documents."

        Public Function SafeTextRead(b() As Byte) As String

            Dim bm As New BOM
            Dim s As String

            Dim bOut() As Byte = BOM.StripBOM(b, bm)

            Select Case bm.Type

                Case BOMTYPE.UTF16LE
                    s = Encoding.Unicode.GetString(bOut)

                Case BOMTYPE.UTF16BE

                    s = Encoding.BigEndianUnicode.GetString(bOut)

                Case BOMTYPE.UTF8

                    s = Encoding.UTF8.GetString(bOut)

                Case BOMTYPE.UTF7a, BOMTYPE.UTF7b, BOMTYPE.UTF7c, BOMTYPE.UTF7d, BOMTYPE.UTF7e

#Disable Warning SYSLIB0001 ' Type or member is obsolete
                    s = Encoding.UTF7.GetString(bOut)
#Enable Warning SYSLIB0001 ' Type or member is obsolete

                Case BOMTYPE.UTF32LE

                    s = Encoding.UTF32.GetString(bOut)

                Case BOMTYPE.UTF32BE

                    Dim nenc As New UTF32Encoding(True, False)

                    s = nenc.GetString(bOut)

                Case Else

                    s = Encoding.UTF8.GetString(bOut)

            End Select

            Return s

        End Function

        Public Function SafeTextWrite(subject As String, Optional enc As BOMTYPE = BOMTYPE.UTF16LE) As Byte()

            Dim bm As New BOM
            Dim s As String = vbNullString
            Dim bl As Blob

            bm.Type = enc

            Dim bOut() As Byte

            Select Case bm.Type

                Case BOMTYPE.UTF16LE
                    bOut = Encoding.Unicode.GetBytes(subject)

                Case BOMTYPE.UTF16BE
                    bOut = Encoding.BigEndianUnicode.GetBytes(subject)

                Case BOMTYPE.UTF8

                    bOut = Encoding.UTF8.GetBytes(subject)

                Case BOMTYPE.UTF7a, BOMTYPE.UTF7b, BOMTYPE.UTF7c, BOMTYPE.UTF7d, BOMTYPE.UTF7e
#Disable Warning SYSLIB0001 ' Type or member is obsolete
                    bOut = Encoding.UTF7.GetBytes(subject)
#Enable Warning SYSLIB0001 ' Type or member is obsolete

                Case BOMTYPE.UTF32LE

                    bOut = Encoding.UTF32.GetBytes(subject)

                Case BOMTYPE.UTF32BE
                    bOut = Encoding.BigEndianUnicode.GetBytes(subject)

                Case Else

                    bOut = Encoding.UTF8.GetBytes(subject)

            End Select

            bl = bm._BOM

            bl &= bOut
            bOut = bl

            bl.Dispose()

            Return bOut

        End Function

#End Region

#Region "Temporary Initializers"

        Friend Sub InitControlSignalCatalog()

            ' Signals.AddSignal("C0")
            ' Signals.AddSignal("Seq	Dec	Hex	Acro	Symb	Name	C	Description")
            Signals.AddSignal("^@	00	00	NUL	␀	Null	\0	Originally used to allow gaps to be left on paper tape for edits. Later used for padding after a code that might take a terminal some time to process (e.g. a carriage return or line feed on a printing terminal). Now often used as a string terminator, especially in the C programming language.")
            Signals.AddSignal("^A	01	01	SOH	␁	Start of Heading		First character of a message header.")
            Signals.AddSignal("^B	02	02	STX	␂	Start of text		First character of message text, and may be used to terminate the message heading.")
            Signals.AddSignal("^C	03	03	ETX	␃	End of Text		Often used as a ""break"" character (Ctrl-C) to interrupt or terminate a program or process.")
            Signals.AddSignal("^D	04	04	EOT	␄	End of Transmission		Used on Unix to signal end-of-file condition on, or to logout from a terminal.")
            Signals.AddSignal("^E	05	05	ENQ	␅	Enquiry		Signal intended to trigger a response at the receiving end, to see if it is still present.")
            Signals.AddSignal("^F	06	06	ACK	␆	Acknowledge		Response to an ENQ, or an indication of successful receipt of a message.")
            Signals.AddSignal("^G	07	07	BEL	␇	Bell	\a	Originally used to sound a bell on the terminal. Later used for a beep on systems that didn't have a physical bell. May also quickly turn on and off inverse video (a visual bell).")
            Signals.AddSignal("^H	08	08	BS	␈	Backspace	\b	Move the cursor one position leftwards. On input, this may delete the character to the left of the cursor. On output, where in early computer technology a character once printed could not be erased, the backspace was sometimes used to generate accented characters in ASCII. For example, à could be produced using the three character sequence a BS ` (0x61 0x08 0x60). This usage is now deprecated and generally not supported. To provide disambiguation between the two potential uses of backspace, the cancel character control code was made part of the standard C1 control set.")
            Signals.AddSignal("^I	09	09	HT	␉	Character Tabulation, Horizontal Tabulation	\t	Position to the next character tab stop.")
            Signals.AddSignal("^J	10	0A	LF	␊	Line Feed	\n	On typewriters, printers, and some terminal emulators, moves the cursor down one row without affecting its column position. On Unix, used to mark end-of-line. In MS-DOS, Windows, and various network standards, LF is used following CR as part of the end-of-line mark.")
            Signals.AddSignal("^K	11	0B	VT	␋	Line Tabulation, Vertical Tabulation	\v	Position the form at the next line tab stop.")
            Signals.AddSignal("^L	12	0C	FF	␌	Form Feed	\f	On printers, load the next page. Treated as whitespace in many programming languages, and may be used to separate logical divisions in code. In some terminal emulators, it clears the screen.")
            Signals.AddSignal("^M	13	0D	CR	␍	Carriage Return	\r	Originally used to move the cursor to column zero while staying on the same line. On Mac OS (pre-Mac OS X), as well as in earlier systems such as the Apple II and Commodore 64, used to mark end-of-line. In MS-DOS, Windows, and various network standards, it is used preceding LF as part of the end-of-line mark. The Enter or Return key on a keyboard will send this character, but it may be converted to a different end-of-line sequence by a terminal program.")
            Signals.AddSignal("^N	14	0E	SO	␎	Shift Out		Switch to an alternate character set.")
            Signals.AddSignal("^O	15	0F	SI	␏	Shift In		Return to regular character set after Shift Out.")
            Signals.AddSignal("^P	16	10	DLE	␐	Data Link Escape		Cause the following octets to be interpreted as raw data, not as control codes or graphic characters. Returning to normal usage would be implementation dependent.")
            Signals.AddSignal("^Q	17	11	DC1	␑	Device Control One (XON)		These four control codes are reserved for device control, with the interpretation dependent upon the device they were connected. DC1 and DC2 were intended primarily to indicate activating a device while DC3 and DC4 were intended primarily to indicate pausing or turning off a device. In actual practice DC1 and DC3 (known also as XON and XOFF respectively in this usage) quickly became the de facto standard for software flow control.")
            Signals.AddSignal("^R	18	12	DC2	␒	Device Control Two		 ")
            Signals.AddSignal("^S	19	13	DC3	␓	Device Control Three (XOFF)		 ")
            Signals.AddSignal("^T	20	14	DC4	␔	Device Control Four		 ")
            Signals.AddSignal("^U	21	15	NAK	␕	Negative Acknowledge		Sent by a station as a negative response to the station with which the connection has been set up. In binary synchronous communication protocol, the NAK is used to indicate that an error was detected in the previously received block and that the receiver is ready to accept retransmission of that block. In multipoint systems, the NAK is used as the not-ready reply to a poll.")
            Signals.AddSignal("^V	22	16	SYN	␖	Synchronous Idle		Used in synchronous transmission systems to provide a signal from which synchronous correction may be achieved between data terminal equipment, particularly when no other character is being transmitted.")
            Signals.AddSignal("^W	23	17	ETB	␗	End of Transmission Block		Indicates the end of a transmission block of data when data are divided into such blocks for transmission purposes.")
            Signals.AddSignal("^X	24	18	CAN	␘	Cancel		Indicates that the data preceding it are in error or are to be disregarded.")
            Signals.AddSignal("^Y	25	19	EM	␙	End of medium		Intended as means of indicating on paper or magnetic tapes that the end of the usable portion of the tape had been reached.")
            Signals.AddSignal("^Z	26	1A	SUB	␚	Substitute		Originally intended for use as a transmission control character to indicate that garbled or invalid characters had been received. It has often been put to use for other purposes when the in-band signaling of errors it provides is unneeded, especially where robust methods of error detection and correction are used, or where errors are expected to be rare enough to make using the character for other purposes advisable.")
            Signals.AddSignal("^[	27	1B	ESC	␛	Escape		The Esc key on the keyboard will cause this character to be sent on most systems. It can be used in software user interfaces to exit from a screen, menu, or mode, or in device-control protocols (e.g., printers and terminals) to signal that what follows is a special command sequence rather than normal text. In systems based on ISO/IEC 2022, even if another set of C0 control codes are used, this octet is required to always represent the escape character.")
            Signals.AddSignal("^\	28	1C	FS	␜	File Separator		Can be used as delimiters to mark fields of data structures. If used for hierarchical levels, US is the lowest level (dividing plain-text data items), while RS, GS, and FS are of increasing level to divide groups made up of items of the level beneath it.")
            Signals.AddSignal("^]	29	1D	GS	␝	Group separator		 ")
            Signals.AddSignal("^^	30	1E	RS	␞	Record Separator		 ")
            Signals.AddSignal("^_	31	1F	US	␟	Unit separator	\nWhile not technically part of the C0 control character range, the following two characters are defined in ISO/IEC 2022 as always being available regardless of which sets of control characters and graphics characters have been registered. They can be thought of as having some characteristics of control characters.")
            Signals.AddSignal(" 	32	20	SP	␠	Space		Space is a graphic character. It has a visual representation consisting of the absence of a graphic symbol. It causes the active position to be advanced by one character position. In some applications, Space can be considered a lowest-level ""word separator"" to be used with the adjacent separator characters.")
            Signals.AddSignal("^?	127	7F	DEL	␡	Delete		Not technically part of the C0 control character range, this was originally used to mark deleted characters on paper tape, since any character could be changed to all ones by punching holes everywhere. On VT100 compatible terminals, this is the character generated by the key labelled ⌫, usually called backspace on modern machines, and does not correspond to the PC delete key.")
            ' Signals.AddSignal("C1")
            ' Signals.AddSignal("Seq	Dec	Hex	Acro	Name	Description")
            Signals.AddSignal("@	128	80	PAD	Padding Character	Listed with no name in Unicode. Not part of ISO/IEC 6429 (ECMA-48).")
            Signals.AddSignal("A	129	81	HOP	High Octet Preset")
            Signals.AddSignal("B	130	82	BPH	Break Permitted Here	Follows a graphic character where a line break is permitted. Roughly equivalent to a soft hyphen except that the means for indicating a line break is not necessarily a hyphen. Not part of the first edition of ISO/IEC 6429.[1]")
            Signals.AddSignal("C	131	83	NBH	No Break Here	Follows the graphic character that is not to be broken. Not part of the first edition of ISO/IEC 6429.[1]")
            Signals.AddSignal("D	132	84	IND	Index	Move the active position one line down, to eliminate ambiguity about the meaning of LF. Deprecated in 1988 and withdrawn in 1992 from ISO/IEC 6429 (1986 and 1991 respectively for ECMA-48).")
            Signals.AddSignal("E	133	85	NEL	Next Line	Equivalent to CR+LF. Used to mark end-of-line on some IBM mainframes.")
            Signals.AddSignal("F	134	86	SSA	Start of Selected Area	Used by block-oriented terminals.")
            Signals.AddSignal("G	135	87	ESA	End of Selected Area")
            Signals.AddSignal("H	136	88	HTS	Character Tabulation Set / Horizontal Tabulation Set	Causes a character tabulation stop to be set at the active position.")
            Signals.AddSignal("I	137	89	HTJ	Character Tabulation With Justification / Horizontal Tabulation With Justification	Similar to Character Tabulation, except that instead of spaces or lines being placed after the preceding characters until the next tab stop is reached, the spaces or lines are placed preceding the active field so that preceding graphic character is placed just before the next tab stop.")
            Signals.AddSignal("J	138	8A	VTS	Line Tabulation Set / Vertical Tabulation Set	Causes a line tabulation stop to be set at the active position.")
            Signals.AddSignal("K	139	8B	PLD	Partial Line Forward / Partial Line Down	Used to produce subscripts and superscripts in ISO/IEC 6429, e.g., in a printer. Subscripts use PLD text PLU while superscripts use PLU text PLD..")
            Signals.AddSignal("L	140	8C	PLU	Partial Line Backward / Partial Line Up")
            Signals.AddSignal("M	141	8D	RI	Reverse Line Feed / Reverse Index	")
            Signals.AddSignal("N	142	8E	SS2	Single-Shift 2	Next character invokes a graphic character from the G2 or G3 graphic sets respectively. In systems that conform to ISO/IEC 4873 (ECMA-43), even if a C1 set other than the default is used, these two octets may only be used for this purpose.")
            Signals.AddSignal("O	143	8F	SS3	Single-Shift 3")
            Signals.AddSignal("P	144	90	DCS	Device Control String	Followed by a string of printable characters (0x20 through 0x7E) and format effectors (0x08 through 0x0D), terminated by ST (0x9C).")
            Signals.AddSignal("Q	145	91	PU1	Private Use 1	Reserved for a function without standardized meaning for private use as required, subject to the prior agreement of the sender and the recipient of the data.")
            Signals.AddSignal("R	146	92	PU2	Private Use 2")
            Signals.AddSignal("S	147	93	STS	Set Transmit State	")
            Signals.AddSignal("T	148	94	CCH	Cancel character	Destructive backspace, intended to eliminate ambiguity about meaning of BS.")
            Signals.AddSignal("U	149	95	MW	Message Waiting	")
            Signals.AddSignal("V	150	96	SPA	Start of Protected Area	Used by block-oriented terminals.")
            Signals.AddSignal("W	151	97	EPA	End of Protected Area")
            Signals.AddSignal("X	152	98	SOS	Start of String	Followed by a control string terminated by ST (0x9C) that may contain any character except SOS or ST. Not part of the first edition of ISO/IEC 6429.[1]")
            Signals.AddSignal("Y	153	99	SGCI	Single Graphic Character Introducer	Listed with no name in Unicode. Not part of ISO/IEC 6429.")
            Signals.AddSignal("Z	154	9A	SCI	Single Character Introducer	To be followed by a single printable character (0x20 through 0x7E) or format effector (0x08 through 0x0D). The intent was to provide a means by which a control function or a graphic character that would be available regardless of which graphic or control sets were in use could be defined. Definitions of what the following byte would invoke was never implemented in an international standard. Not part of the first edition of ISO/IEC 6429.[1]")
            Signals.AddSignal("[	155	9B	CSI	Control Sequence Introducer	Used to introduce control sequences that take parameters.")
            Signals.AddSignal("\	156	9C	ST	String Terminator	")
            Signals.AddSignal("]	157	9D	OSC	Operating System Command	Followed by a string of printable characters (0x20 through 0x7E) and format effectors (0x08 through 0x0D), terminated by ST (0x9C). These three control codes were intended for use to allow in-band signaling of protocol information, but are rarely used for that purpose.")
            Signals.AddSignal("^	158	9E	PM	Privacy Message")
            Signals.AddSignal("_	159	9F	APC	Application Program Command")
            Signals.AddSignal("_	225	E1	NSP	A figure space is a typographic unit equal to the size of a single typographic figure (numeral or letter), minus leading. Its size can fluctuate somewhat depending on which font is being used. In fonts with monospaced digits, it is equal to the width of one digit.")
            Signals.AddSignal("^	255	FF	EO	Eighty-Ones")

        End Sub

        Public Sub InitEBCDIC()
            Dim objCP As New CodePage

            objCP.AddCode("Control|0000|Null character,NUL|0")
            objCP.AddCode("Control|0001|Start of heading,SOH|1")
            objCP.AddCode("Control|0002|Start of text,STX|2")
            objCP.AddCode("Control|0003|End of text,ETX|3")
            objCP.AddCode("Control|    |SEL|4")
            objCP.AddCode("Control|0009|Horizontal tab,HT|5")
            objCP.AddCode("Control|    |RNL|6")
            objCP.AddCode("Control|007F|Delete character,DEL|7")
            objCP.AddCode("Control|    |GE|8")
            objCP.AddCode("Control|    |SPS|9")
            objCP.AddCode("Control|    |RPT|10")
            objCP.AddCode("Control|000B|Vertical tab,VT|11")
            objCP.AddCode("Control|000C|Form feed,FF|12")
            objCP.AddCode("Control|000D|Carriage return,CR|13")
            objCP.AddCode("Control|000E|Shift out,SO|14")
            objCP.AddCode("Control|000F|Shift in,SI|15")
            objCP.AddCode("Control|0010|Data Link Escape,DLE|16")
            objCP.AddCode("Control|0011|Device Control 1,DC1|17")
            objCP.AddCode("Control|0012|Device Control 2,DC2|18")
            objCP.AddCode("Control|0013|Device Control 3,DC3|19")
            objCP.AddCode("Control|    |RES ENP|20")
            objCP.AddCode("Control|0085|Newline,NL|21")
            objCP.AddCode("Control|0008|Backspace,BS|22")
            objCP.AddCode("Control|    |POC|23")
            objCP.AddCode("Control|0018|Cancel character,CAN|24")
            objCP.AddCode("Control|0019|End of medium,EM|25")
            objCP.AddCode("Control|    |UBS|26")
            objCP.AddCode("Control|    |CU1|27")
            objCP.AddCode("Control|001C|Field separator,IFS|28")
            objCP.AddCode("Control|001D|Group separator,IGS|29")
            objCP.AddCode("Control|001E|Record separator,IRS|30")
            objCP.AddCode("Control|001F|Unit separator,IUS ITB|31")
            objCP.AddCode("Control|    |DS|32")
            objCP.AddCode("Control|    |SOS|33")
            objCP.AddCode("Control|    |FS|34")
            objCP.AddCode("Control|    |WUS|35")
            objCP.AddCode("Control|    |BYP INP|36")
            objCP.AddCode("Control|000A|Line feed,LF|37")
            objCP.AddCode("Control|0017|End transmission block,ETB|38")
            objCP.AddCode("Control|001B|Escape character,ESC|39")
            objCP.AddCode("Control|    |SA|40")
            objCP.AddCode("Control|    |SFE|41")
            objCP.AddCode("Control|    |SM SW|42")
            objCP.AddCode("Control|    |CSP|43")
            objCP.AddCode("Control|    |MFA|44")
            objCP.AddCode("Control|0005|Enquire character,ENQ|45")
            objCP.AddCode("Control|0006|Acknowledge character,ACK|46")
            objCP.AddCode("Control|0007|Bell character,BEL|47")
            objCP.AddCode("Control|    |48")
            objCP.AddCode("Control|    |49")
            objCP.AddCode("Control|0016|Synchronous idle,SYN|50")
            objCP.AddCode("Control|    |IR|51")
            objCP.AddCode("Control|    |PP|52")
            objCP.AddCode("Control|    |TRN|53")
            objCP.AddCode("Control|    |NBS|54")
            objCP.AddCode("Control|0004|End-of-transmission character,EOT|55")
            objCP.AddCode("Control|    |SBS|56")
            objCP.AddCode("Control|    |IT|57")
            objCP.AddCode("Control|    |RFF|58")
            objCP.AddCode("Control|    |CU3|59")
            objCP.AddCode("Control|0014|Device control 4,DC4|60")
            objCP.AddCode("Control|0015|Negative-acknowledge character,NAK|61")
            objCP.AddCode("Control|    |62")
            objCP.AddCode("Control|001A|Substitute character,SUB|63")
            objCP.AddCode("Punctuation|0020|Space character,SP|64")
            objCP.AddCode("Punctuation|00A0|Non-breaking space,RSP|65")
            objCP.AddCode("Undefined|    | |66")
            objCP.AddCode("Undefined|    | |67")
            objCP.AddCode("Undefined|    | |68")
            objCP.AddCode("Undefined|    | |69")
            objCP.AddCode("Undefined|    | |70")
            objCP.AddCode("Undefined|    | |71")
            objCP.AddCode("Undefined|    | |72")
            objCP.AddCode("Undefined|    | |73")
            objCP.AddCode("Undefined|    | |74")
            objCP.AddCode("Punctuation|002E|full stop,.|75")
            objCP.AddCode("Punctuation|003C|less-than sign,<|76")
            objCP.AddCode("Punctuation|0028|parenthesis,(|77")
            objCP.AddCode("Punctuation|002B|+|78")
            objCP.AddCode("Symbol|007C|vertical bar,&H7C|79")
            objCP.AddCode("Punctuation|0026|ampersand,&|80")
            objCP.AddCode("Undefined|    | |81")
            objCP.AddCode("Undefined|    | |82")
            objCP.AddCode("Undefined|    | |83")
            objCP.AddCode("Undefined|    | |84")
            objCP.AddCode("Undefined|    | |85")
            objCP.AddCode("Undefined|    | |86")
            objCP.AddCode("Undefined|    | |87")
            objCP.AddCode("Undefined|    | |88")
            objCP.AddCode("Undefined|    | |89")
            objCP.AddCode("Symbol|0021|!|90")
            objCP.AddCode("Symbol|0024|$|91")
            objCP.AddCode("Punctuation|002A|Asterisk,*|92")
            objCP.AddCode("Punctuation|0029|parenthesis,)|93")
            objCP.AddCode("Punctuation|003B| |94")
            objCP.AddCode("Symbol|00AC|¬|95")
            objCP.AddCode("Punctuation|002D|Hyphen-minus,-|96")
            objCP.AddCode("Symbol|002F|/|97")
            objCP.AddCode("Undefined|    | |98")
            objCP.AddCode("Undefined|    | |99")
            objCP.AddCode("Undefined|    | |100")
            objCP.AddCode("Undefined|    | |101")
            objCP.AddCode("Undefined|    | |102")
            objCP.AddCode("Undefined|    | |103")
            objCP.AddCode("Undefined|    | |104")
            objCP.AddCode("Undefined|    | |105")
            objCP.AddCode("Punctuation|00A6|¦|106")
            objCP.AddCode("Punctuation|002C|&H2C|107")
            objCP.AddCode("Punctuation|0025|%|108")
            objCP.AddCode("Punctuation|005F|underscore,_|109")
            objCP.AddCode("Punctuation|003E|greater-than sign,>|110")
            objCP.AddCode("Punctuation|003F|?|111")
            objCP.AddCode("Undefined|    | |112")
            objCP.AddCode("Undefined|    | |113")
            objCP.AddCode("Undefined|    | |114")
            objCP.AddCode("Undefined|    | |115")
            objCP.AddCode("Undefined|    | |116")
            objCP.AddCode("Undefined|    | |117")
            objCP.AddCode("Undefined|    | |118")
            objCP.AddCode("Undefined|    | |119")
            objCP.AddCode("Undefined|    | |120")
            objCP.AddCode("Symbol|0060|`|121")
            objCP.AddCode("Punctuation|003A|colon (Punctuationuation),&H3A|122")
            objCP.AddCode("Symbol|0023|number sign,&H23|123")
            objCP.AddCode("Punctuation|0040|@|124")
            objCP.AddCode("Punctuation|0027|apostrophe,&H27|125")
            objCP.AddCode("Punctuation|003D|equals sign,&H3D|126")
            objCP.AddCode("Punctuation|0022|&H22|127")
            objCP.AddCode("Undefined|    | |128")
            objCP.AddCode("Alpha|0061|a|129")
            objCP.AddCode("Alpha|0062|b|130")
            objCP.AddCode("Alpha|0063|c|131")
            objCP.AddCode("Alpha|0064|d|132")
            objCP.AddCode("Alpha|0065|e|133")
            objCP.AddCode("Alpha|0066|f|134")
            objCP.AddCode("Alpha|0067|g|135")
            objCP.AddCode("Alpha|0068|h|136")
            objCP.AddCode("Alpha|0069|i|137")
            objCP.AddCode("Undefined|    | |138")
            objCP.AddCode("Undefined|    | |139")
            objCP.AddCode("Undefined|    | |140")
            objCP.AddCode("Undefined|    | |141")
            objCP.AddCode("Undefined|    | |142")
            objCP.AddCode("Symbol|00B1|±|143")
            objCP.AddCode("Undefined|    | |144")
            objCP.AddCode("Alpha|006A|j|145")
            objCP.AddCode("Alpha|006B|k|146")
            objCP.AddCode("Alpha|006C|l|147")
            objCP.AddCode("Alpha|006D|m|148")
            objCP.AddCode("Alpha|006E|n|149")
            objCP.AddCode("Alpha|006F|o|150")
            objCP.AddCode("Alpha|0070|p|151")
            objCP.AddCode("Alpha|0071|q|152")
            objCP.AddCode("Alpha|0072|r|153")
            objCP.AddCode("Undefined|    | |154")
            objCP.AddCode("Undefined|    | |155")
            objCP.AddCode("Undefined|    | |156")
            objCP.AddCode("Undefined|    | |157")
            objCP.AddCode("Undefined|    | |158")
            objCP.AddCode("Undefined|    | |159")
            objCP.AddCode("Undefined|    | |160")
            objCP.AddCode("Symbol|007E|~|161")
            objCP.AddCode("Alpha|0073|s|162")
            objCP.AddCode("Alpha|0074|t|163")
            objCP.AddCode("Alpha|0075|u|164")
            objCP.AddCode("Alpha|0076|v|165")
            objCP.AddCode("Alpha|0077|w|166")
            objCP.AddCode("Alpha|0078|x|167")
            objCP.AddCode("Alpha|0079|y|168")
            objCP.AddCode("Alpha|007A|z|169")
            objCP.AddCode("Undefined|    | |170")
            objCP.AddCode("Undefined|    | |171")
            objCP.AddCode("Undefined|    | |172")
            objCP.AddCode("Undefined|    | |173")
            objCP.AddCode("Undefined|    | |174")
            objCP.AddCode("Undefined|    | |175")
            objCP.AddCode("Symbol|005E|^|176")
            objCP.AddCode("Undefined|    | |177")
            objCP.AddCode("Undefined|    | |178")
            objCP.AddCode("Undefined|    | |179")
            objCP.AddCode("Undefined|    | |180")
            objCP.AddCode("Undefined|    | |181")
            objCP.AddCode("Undefined|    | |182")
            objCP.AddCode("Undefined|    | |183")
            objCP.AddCode("Undefined|    | |184")
            objCP.AddCode("Undefined|    | |185")
            objCP.AddCode("Symbol|005B|square brackets,&H5B|186")
            objCP.AddCode("Symbol|005D|square brackets,&H5D|187")
            objCP.AddCode("Undefined|    | |188")
            objCP.AddCode("Undefined|    | |189")
            objCP.AddCode("Undefined|    | |190")
            objCP.AddCode("Undefined|    | |191")
            objCP.AddCode("Symbol|007B|brace (Punctuationuation),&H7B|192")
            objCP.AddCode("Alpha|0041|A|193")
            objCP.AddCode("Alpha|0042|B|194")
            objCP.AddCode("Alpha|0043|C|195")
            objCP.AddCode("Alpha|0044|D|196")
            objCP.AddCode("Alpha|0045|E|197")
            objCP.AddCode("Alpha|0046|F|198")
            objCP.AddCode("Alpha|0047|G|199")
            objCP.AddCode("Alpha|0048|H|200")
            objCP.AddCode("Alpha|0049|I|201")
            objCP.AddCode("Punctuation|00AD|Soft hyphen,SHY|202")
            objCP.AddCode("Undefined|    | |203")
            objCP.AddCode("Undefined|    | |204")
            objCP.AddCode("Undefined|    | |205")
            objCP.AddCode("Undefined|    | |206")
            objCP.AddCode("Undefined|    | |207")
            objCP.AddCode("Symbol|007D|brace (Punctuationuation),&H7D|208")
            objCP.AddCode("Alpha|004A|J|209")
            objCP.AddCode("Alpha|004B|K|210")
            objCP.AddCode("Alpha|004C|L|211")
            objCP.AddCode("Alpha|004D|M|212")
            objCP.AddCode("Alpha|004E|N|213")
            objCP.AddCode("Alpha|004F|O|214")
            objCP.AddCode("Alpha|0050|P|215")
            objCP.AddCode("Alpha|0051|Q|216")
            objCP.AddCode("Alpha|0052|R|217")
            objCP.AddCode("Undefined|    | |218")
            objCP.AddCode("Undefined|    | |219")
            objCP.AddCode("Undefined|    | |220")
            objCP.AddCode("Undefined|    | |221")
            objCP.AddCode("Undefined|    | |222")
            objCP.AddCode("Undefined|    | |223")
            objCP.AddCode("Symbol|005C|\|224")
            objCP.AddCode("Symbol|2007|figure space,NSP|225")
            objCP.AddCode("Alpha|0053|S|226")
            objCP.AddCode("Alpha|0054|T|227")
            objCP.AddCode("Alpha|0055|U|228")
            objCP.AddCode("Alpha|0056|V|229")
            objCP.AddCode("Alpha|0057|W|230")
            objCP.AddCode("Alpha|0058|X|231")
            objCP.AddCode("Alpha|0059|Y|232")
            objCP.AddCode("Alpha|005A|Z|233")
            objCP.AddCode("Undefined|    | |234")
            objCP.AddCode("Undefined|    | |235")
            objCP.AddCode("Undefined|    | |236")
            objCP.AddCode("Undefined|    | |237")
            objCP.AddCode("Undefined|    | |238")
            objCP.AddCode("Undefined|    | |239")
            objCP.AddCode("Digit|0030|0 (number),0|240")
            objCP.AddCode("Digit|0031|1 (number),1|241")
            objCP.AddCode("Digit|0032|2 (number),2|242")
            objCP.AddCode("Digit|0033|3 (number),3|243")
            objCP.AddCode("Digit|0034|4 (number),4|244")
            objCP.AddCode("Digit|0035|5 (number),5|245")
            objCP.AddCode("Digit|0036|6 (number),6|246")
            objCP.AddCode("Digit|0037|7 (number),7|247")
            objCP.AddCode("Digit|0038|8 (number),8|248")
            objCP.AddCode("Digit|0039|9 (number),9|249")
            objCP.AddCode("Undefined|    | |250")
            objCP.AddCode("Undefined|    | |251")
            objCP.AddCode("Undefined|    | |252")
            objCP.AddCode("Undefined|    | |253")
            objCP.AddCode("Undefined|    | |254")
            objCP.AddCode("Control|    |Eight Ones,EO|255")

            objCP.Name = "EBCDIC"
            CodesPages.Add(objCP)

        End Sub

#End Region

    End Module

#End Region

#End Region

End Namespace
