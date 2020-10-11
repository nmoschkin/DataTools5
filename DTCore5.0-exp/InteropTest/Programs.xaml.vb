Imports System.Windows
Imports System.Windows.Input
Imports DataTools.Interop
Imports DataTools.Interop.Desktop

Public Class Programs

    Public Property FileTypes As AllSystemFileTypes
        Get
            Return GetValue(FileTypesProperty)
        End Get
        Set(value As AllSystemFileTypes)
            SetValue(FileTypesProperty, value)
        End Set
    End Property

    Public Shared ReadOnly FileTypesProperty As DependencyProperty = _
                           DependencyProperty.Register("FileTypes", _
                           GetType(AllSystemFileTypes), GetType(Programs), _
                           New PropertyMetadata(Nothing))

    Public Sub New()

        InitializeComponent()

        Status.Text = "Enumerating File Types..."
    End Sub

    Private Sub _Quit_Click(sender As Object, e As RoutedEventArgs) Handles _Quit.Click
        End
    End Sub

    Private Sub _Close_Click(sender As Object, e As RoutedEventArgs) Handles _Close.Click
        Me.Close()
    End Sub

    Private Sub TypeEnumerated(sender As Object, e As FileTypeEnumEventArgs)

        Dispatcher.Invoke(Sub()
                              Status.Text = "Enumerated " & e.Index & " of " & e.Count & " types.  " & e.Type.Extension & " - " & e.Type.Description

                          End Sub)

    End Sub


    Private Sub Programs_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        Dim th As New System.Threading.Thread( _
            Sub()


                Dispatcher.Invoke(Sub()
                                      FileTypes = New AllSystemFileTypes
                                      AddHandler FileTypes.Populating, AddressOf TypeEnumerated

                                      Me.Cursor = Cursors.Wait
                                      Me.UpdateLayout()


                                      FileTypes.Populate()

                                      RemoveHandler FileTypes.Populating, AddressOf TypeEnumerated

                                      Me.Cursor = Cursors.Arrow
                                      Status.Text = "Finished."

                                  End Sub)

            End Sub)

        th.SetApartmentState(System.Threading.ApartmentState.STA)
        th.IsBackground = True

        th.Start()

    End Sub
End Class
