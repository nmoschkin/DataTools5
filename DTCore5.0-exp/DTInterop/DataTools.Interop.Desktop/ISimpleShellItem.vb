'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: ISimpleShellItem.
''         Represents a shell item with common properties.
''
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License
'' ************************************************* ''

Imports System.ComponentModel
Imports System.IO

Namespace Desktop

    Public Interface ISimpleShellItem
        Inherits INotifyPropertyChanged

        Property Attributes As FileAttributes
        ReadOnly Property Children As ICollection(Of ISimpleShellItem)
        Property CreationTime As Date
        Property DisplayName As String
        ReadOnly Property Folders As ICollection(Of ISimpleShellItem)
        ReadOnly Property Icon As Windows.Media.Imaging.BitmapSource
        Property IconSize As StandardIcons
        ReadOnly Property IsFolder As Boolean
        ReadOnly Property IsSpecial As Boolean
        Property LastAccessTime As Date
        Property LastWriteTime As Date
        ReadOnly Property Parent As ISimpleShellItem
        ReadOnly Property ParsingName As String
        ReadOnly Property Size As Long

        Sub Refresh(Optional iconSize As StandardIcons? = Nothing)

    End Interface

End Namespace