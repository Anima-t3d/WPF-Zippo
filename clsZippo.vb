Public Class clsZippo
    Private _price As Decimal
    Private _imagesList As New ArrayList

    Public Property Id As Integer
    Public Property Number As String
    Public Property Name As String
    Public Property Description As String
    Public Property Price As String
        Get
            Return _price.ToString()
        End Get
        Set(value As String)
            _price = strCurrencyToDecimal(value)
        End Set
    End Property
    Public Property Images As ArrayList
        Get
            Return _imagesList
        End Get
        Set(value As ArrayList)
            _imagesList = value
        End Set
    End Property
    Public Property Finishing As String     'Change to object from clsFinishes
    Public Property Category As String      'Change to object from clsCategories
    Public Property Address As String       'Change to object from clsAddresses
    '===================== OPTIONAL DATA =====================
    Public Property Url As Uri              'Url to webpage
    Public Property PUID As String          'Product serial from website
    Public Property ZID As Integer          'Id from webpage

    Public Sub addImage(ByVal stringUri As String)
        Dim uri As New Uri(stringUri)
        _imagesList.Add(uri)
    End Sub
    Public Sub removeImage(ByVal stringUri As String)
        Dim uri As New Uri(stringUri)
        _imagesList.Remove(uri)
    End Sub
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
End Class