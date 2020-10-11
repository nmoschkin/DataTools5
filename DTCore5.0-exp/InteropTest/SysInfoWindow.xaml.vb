Public Class SysInfoWindow

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.


        _props.SelectedObject = CoreCT.SystemInformation.SysInfo.LogicalProcessors

        Dim mcache As Long = 0

        Dim lcache(0 To 3) As Integer

        For Each fp In CoreCT.SystemInformation.SysInfo.LogicalProcessors

            If fp.Relationship = CoreCT.SystemInformation.LOGICAL_PROCESSOR_RELATIONSHIP.RelationCache Then
                If (fp.CacheDescriptor.Size > 1) Then
                    mcache += fp.CacheDescriptor.Size
                    lcache(fp.CacheDescriptor.Level) += 1
                End If

                fp.ToString()
            End If
        Next

    End Sub


End Class
