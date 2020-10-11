Imports System.Reflection

Namespace Strings

    Public Module SharedProp

#Region "Generic Named Property Value Diggers"

        '' These functions are useful for objects whose valid values come from shared members of specific classes, such as Color and FontWeight.

        ''' <summary>
        ''' Returns the string name of an object that is set to one of its own (or another type's) shared members.
        ''' </summary>
        ''' <param name="value">The object to query.</param>
        ''' <param name="altReference">Alternate reference type.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SharedPropToName(Of T)(value As T, Optional altReference As System.Type = Nothing) As String

            Dim p() As PropertyInfo
            If altReference Is Nothing Then
                p = value.GetType.GetProperties(BindingFlags.Public Or BindingFlags.Static)
            Else
                p = altReference.GetProperties(BindingFlags.Public Or BindingFlags.Static)
            End If

            Dim vl As Object

            For Each pe In p

                vl = pe.GetValue(value)

                If vl.Equals(value) Then
                    Return pe.Name
                End If

            Next

            Return Nothing
        End Function

        ''' <summary>
        ''' Returns an object whose value corresponds to a named value of its own (or another type's) shared members.
        ''' </summary>
        ''' <typeparam name="T">A type that can be instantiated.</typeparam>
        ''' <param name="value">The name of the value to seek.</param>
        ''' <param name="altReference">Alternate reference type.</param>
        ''' <param name="caseSensitive">Specifies whether the search is case-sensitive.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function NameToSharedProp(Of T As New)(value As String, Optional altReference As System.Type = Nothing, Optional caseSensitive As Boolean = True) As T

            Dim x As New T()
            Dim b As Boolean

            b = NameToSharedProp(Of T)(value, x, altReference, caseSensitive)
            If b Then Return x Else Return Nothing

        End Function

        ''' <summary>
        ''' Returns an object whose value corresponds to a named value of its own (or another type's) shared members.
        ''' </summary>
        ''' <param name="value">The name of the value to seek.</param>
        ''' <param name="instance">An active instance of the object whose value to seek.</param>
        ''' <param name="altReference">Alternate reference type.</param>
        ''' <param name="caseSensitive">Specifies whether the search is case-sensitive.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function NameToSharedProp(Of T)(value As String, ByRef instance As T, Optional altReference As System.Type = Nothing, Optional caseSensitive As Boolean = True) As Boolean

            Dim p() As PropertyInfo
            If altReference IsNot Nothing Then
                p = altReference.GetProperties(BindingFlags.Public Or BindingFlags.Static)
            Else
                p = instance.GetType.GetProperties(BindingFlags.Public Or BindingFlags.Static)
            End If

            If instance Is Nothing Then
                ' let's just try to create it, it may work.
                instance = If(instance IsNot Nothing, instance, System.ComponentModel.TypeDescriptor.CreateInstance(Nothing, GetType(T), Nothing, Nothing))
                If instance Is Nothing Then Return Nothing
            End If

            Dim c1 As String = value

            If caseSensitive Then

                For Each pe In p

                    If pe.Name = c1 Then
                        instance = CType(pe.GetValue(instance), T)
                        Return True
                    End If

                Next
            Else
                c1 = c1.ToLower

                For Each pe In p

                    If pe.Name.ToLower = c1 Then
                        instance = CType(pe.GetValue(instance), T)
                        Return True
                    End If

                Next

            End If

            Return False

        End Function

#End Region

    End Module

End Namespace