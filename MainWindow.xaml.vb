Imports mshtml
Class MainWindow
    ' Make an event capable shortcutmenu
    WithEvents shortcutmenu As shortcutmenu '= New shortcutmenu()
    Private Sub btnExternal_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim outputString As String = ""
        Dim productIdCounter As Integer = 0
        'fetch image url
        'check if document exists
        If myBrowser.Document IsNot Nothing Then
            'try to find specific id on zippo site "imgHero"
            Dim element As Object = myBrowser.Document.GetElementById("imgHero")
            'if the element exists
            If element IsNot Nothing Then
                'get the image location
                Dim imgsrc As String = element.GetAttribute("src")
                If imgsrc IsNot Nothing Then
                    outputString &= " imgUrl: " & imgsrc & Environment.NewLine
                End If
            Else
                'no element found
                outputString &= "imgUrl: No suited image url found" & Environment.NewLine
            End If

            Dim currentDocument As mshtml.IHTMLDocument = myBrowser.Document
            Dim Elems As IHTMLElementCollection

            Elems = currentDocument.body.all
            'loop through all gathered elements on the page and filter out the item specs
            For Each elem As IHTMLElement In Elems
                If elem.getAttribute("itemprop") IsNot Nothing Then
                    Dim itemprop As Object = elem.getAttribute("itemprop")
                    Select Case itemprop.ToString
                        Case "name"
                            outputString &= "Category: " & elem.innerText & Environment.NewLine
                        Case "offers"
                            outputString &= "Price: " & elem.innerText & Environment.NewLine
                        Case "productID"
                            If productIdCounter < 1 Then
                                outputString &= "productID: " & elem.innerText & Environment.NewLine
                                productIdCounter += 1
                            End If
                        Case "description"
                            outputString &= "Finishing: " & elem.innerHTML.Split("<br>")(1).Substring(4) & Environment.NewLine
                            outputString &= "Description: " & elem.innerHTML.Replace("<BR>", Environment.NewLine) & Environment.NewLine
                    End Select
                End If
            Next
            outputString &= "Uri: " & txtLoad.Text & Environment.NewLine
            Try
                Dim parameters As String = txtLoad.Text.Split("?")(1)
                outputString &= "Id: " & parameters.Split("=")(1) & Environment.NewLine
            Catch
            End Try
            MessageBox.Show(outputString)
        End If

        

    End Sub

    Private Sub btnInternal_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        'load default page
        myBrowser.Navigate("http://www.zippo.com/search.aspx")
    End Sub
    ' Handle the search event 
    'IMPORTANT: REFERENCE TO NAME INSTEAD OF CLASS!!!
    Private Sub Search(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Shortcutmenu1.search
        Me.btnInternal_Click(sender, e)
    End Sub
    ' Handle the search event 
    'IMPORTANT: REFERENCE TO NAME INSTEAD OF CLASS!!!
    Private Sub Collect(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Shortcutmenu1.collect
        Me.btnExternal_Click(sender, e)
    End Sub

    Private Sub myBrowser_Navigating(ByVal sender As System.Object, ByVal e As System.Windows.Navigation.NavigatingCancelEventArgs) Handles myBrowser.Navigating
        'load uri to text
        txtLoad.Text = e.Uri.ToString
    End Sub

    Private Sub myBrowser_LoadCompleted(ByVal sender As System.Object, ByVal e As System.Windows.Navigation.NavigationEventArgs) Handles myBrowser.LoadCompleted

        '####REGION
        'TODO:  Overflow:hidden to disable scrollbars on document
        '       //Dim currentDocument As mshtml.HTMLDocument = myBrowser.Document.DomDocument
        '       Dim currentDocument As mshtml.IHTMLDocument = myBrowser.Document
        '       Dim length As Integer = currentDocument.styleSheets.length
        '       Dim stylesheet As mshtml.IHTMLStyleSheet = currentDocument.createStyleSheet("", length + 1)
        '       stylesheet.cssText = "body{overflow:hidden;color:red;background-color:green;}"
        '###END REGION


        If (myBrowser.Document IsNot Nothing) Then
            
            'hiding header
            Dim header As IHTMLElement = myBrowser.Document.GetElementById("hdrSite")
            header.style.display = "none"
            'hiding irritating sliced malfunctioning footer
            Dim footer As IHTMLElement = myBrowser.Document.GetElementById("ftrSite")
            footer.style.display = "none"

            'hiding sidebar
            Dim currentDocument As mshtml.IHTMLDocument = myBrowser.Document
            Dim Elems As IHTMLElementCollection

            Elems = currentDocument.body.all
            'loop through all gathered elements on the page and filter out the item specs
            For Each elem As IHTMLElement In Elems
                If elem.getAttribute("class") IsNot Nothing Then
                    Dim itemprop As Object = elem.getAttribute("class")
                    Select Case itemprop.ToString
                        Case "listKickersRight"
                            elem.style.display = "none"
                    End Select
                End If
            Next
            'when on search page, scroll the search container to the top of the page
            If e.Uri.ToString = "http://www.zippo.com/search.aspx" Then
                Dim element As Object = myBrowser.Document.GetElementById("fSearch")
                If element IsNot Nothing Then
                    element.ScrollIntoView(True)
                End If
            End If
        End If
    End Sub

End Class
