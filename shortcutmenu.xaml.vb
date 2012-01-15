Public Class shortcutmenu
    'references:    http://visualbasic.about.com/od/usingvbnet/a/menuwpf.htm
    '               http://visualbasic.about.com/od/learnvbnet/a/usrctl02.htm
    '               http://www.daniweb.com/software-development/vbnet/threads/68810
    'IMPORTANT:     
    '   in main: xmlns:local="clr-namespace:YOUR PROJECT NAME"
    '   in main: <local:USERCONTROL x:Name="USERCONTROLNAME"/>
    Public Event search(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
    Public Event collect(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
  
    Private Sub SearchIt(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button1.Click
        RaiseEvent search(sender, e)
    End Sub

    Private Sub CollectIt(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button2.Click
        RaiseEvent collect(sender, e)
    End Sub
End Class
