Public Class extUtilities

    'This class has some random snippets that would be useful in the zippo application
    'They need modification to fit custom version of the application
    'DO NOT USE AS IS, it will NOT work!

    Public Function strCurrencyToDecimal(Value As String) As Decimal
        'Converts input string to output decimal
        'Based on: http://www.velocityreviews.com/forums/t87759-convert-currency-formatted-string-back-to-decimal.html
        If Value.Length = 0 Then
            Return 0
        Else
            Value = Value.Replace(",", "").Replace("$", "€").Replace("£", "€").Replace(".", ",")
            Return Decimal.Parse(Value.Replace(" ", ""), System.Globalization.NumberStyles.Any Or System.Globalization.NumberStyles.AllowCurrencySymbol _
                             Or System.Globalization.NumberStyles.AllowThousands Or System.Globalization.NumberStyles.AllowDecimalPoint)
        End If
    End Function
    Public Function ImageSource(ByVal FullPath As String) As Object
        'Based on: http://stackoverflow.com/questions/20586/image-urisource-and-data-binding
        Dim image As New BitmapImage()

        Try
            image.BeginInit()
            image.CacheOption = BitmapCacheOption.OnLoad
            image.CreateOptions = BitmapCreateOptions.IgnoreImageCache
            image.UriSource = New Uri(FullPath, UriKind.Absolute)
            image.EndInit()
        Catch
            Return DependencyProperty.UnsetValue
        End Try

        Return image

    End Function
    Public Sub PopulateImages()
        Dim intImages As Integer = 0
        Dim intControls As Integer = 0

        ' Enumerate all the descendants of the visual object.
        ' Based on: http://msdn.microsoft.com/en-us/library/system.windows.media.visualtreehelper.getchild.aspx
        While intControls < VisualTreeHelper.GetChildrenCount(Me.Content) - 1 And intImages < _zippo.Images.Count
            ' Retrieve child visual at specified index value.
            Dim childVisual As Visual = CType(VisualTreeHelper.GetChild(Me.Content, intControls), Visual)
            ' Check if visual is an Image type
            If childVisual.GetType().Name = "Image" Then
                Dim currentImage As Image = CType(childVisual, Image)
                ' Place uri to source
                currentImage.Source = ImageSource(_zippo.Images(intImages).ToString)
                currentImage.Stretch = Stretch.Uniform
                intImages += 1
            End If
            intControls += 1
        End While

    End Sub
    'Hiding windows
    Private Sub Window_IsVisibleChanged(sender As System.Object, e As System.Windows.DependencyPropertyChangedEventArgs) Handles MyBase.IsVisibleChanged
        If Me.Visibility = Windows.Visibility.Hidden Then
            Me.Owner.Activate()
        End If
    End Sub
    Private Sub Window_Closing(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        For Each Window As winBrowser In Me.OwnedWindows.OfType(Of winBrowser)()
            Window.ParentCloses = True
        Next
    End Sub

    Dim parentWindow As Window

    'file browser for images
    Private Sub btnBrowse_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnBrowse.Click
        Dim openfile As New OpenFileDialog
        openfile.Filter = "Image files (*.gif, *.png, *.jpg)|*.gif*;*.png*;*.jpg"
        Dim result As DialogResult = openfile.ShowDialog()
        If result = DialogResult.OK Then
            _filePath = openfile.FileName
            'Dim safeFilePath As String = openfile.SafeFileName
            txtFilePath.Text = _filePath
        End If
    End Sub

    Private Sub btnReplace_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnReplace.Click
        Dim uriExtension As String = ""
        uriExtension = txtFilePath.Text.Split(".")(txtFilePath.Text.Split(".").Count - 1)
        If uriExtension = "jpg" Or uriExtension = "gif" Or uriExtension = "png" Then
            If _filePath <> txtFilePath.Text Then
                _filePath = txtFilePath.Text
            End If
            RaiseEvent ReplaceMe(_filePath.ToString)
            'Insinuating fresh startup window when reloaded next time
            txtFilePath.Text = String.Empty
            parentWindow.Visibility = Windows.Visibility.Hidden
        Else
            MessageBox.Show("Not a valid image!")
        End If

    End Sub

    Private Sub UserControl_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        parentWindow = Window.GetWindow(Me)
    End Sub
End Class
