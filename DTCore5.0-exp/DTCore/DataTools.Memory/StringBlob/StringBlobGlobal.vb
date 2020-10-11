'' ************************************************* ''
'' DataTools Visual Basic Utility Library 
''
'' Module: Global Enums for StringBlob
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Namespace Memory


#Region "StringBlob Enums"

    Public Enum StringBlobFormats

        None = 0
        ''' <summary>
        ''' Each string will be quoted with double quotes.
        ''' </summary>
        ''' <remarks></remarks>
        Quoted = 1
        Spaced = 2
        Commas = 4

        ''' <summary>
        ''' Custom format.  Format is provided by the user.  No other options can be used with this option.
        ''' </summary>
        ''' <remarks></remarks>
        Custom = 8

        ''' <summary>
        ''' Only for use with Custom option.
        ''' 
        ''' Formatting: %s - the string; %b - the first string only, %e - the last string only.
        ''' </summary>
        ''' <remarks></remarks>
        CustStrPos = 16

        ''' <summary>
        ''' Treats each string element as a line of text.
        ''' </summary>
        ''' <remarks></remarks>
        CrLf = 32

    End Enum

#End Region

End Namespace