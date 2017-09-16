Imports System.Globalization

Class LawConventions
    Class ControlCode
        Public Shared Function StringifyDateTime(value As DateTime) As String
            Return value.ToString("yyyyMMdd")
        End Function

        Public Shared Function RoundImporteTotal(value As Decimal) As Decimal
            Return Math.Round(value, 0, MidpointRounding.AwayFromZero)
        End Function
    End Class

    Class QrControlCode
        Public Shared Function StringifyDateTime(value As DateTime) As String
            Return value.ToString("dd/MM/yyyy")
        End Function

        Public Shared Function StringifyDecimal(value As Decimal) As String
            If (value <= 0) Then Return "0"
            Return value.ToString("0.00", ImpuestosNacionalesNFI)
        End Function

        Private Shared ReadOnly Property ImpuestosNacionalesNFI As New NumberFormatInfo With
            {.NumberDecimalSeparator = ".", .NumberGroupSeparator = "", .NumberDecimalDigits = 2}
    End Class
End Class
