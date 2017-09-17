Public NotInheritable Class SpanishNumericStrings
    Private Sub New()
    End Sub

    Private Shared ReadOnly UnitsPlusString As String() = New String() {
        String.Empty,
        "un", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve", "diez",
        "once", "doce", "trece", "catorce", "quince", "dieciséis", "diecisiete", "dieciocho", "diecinueve", "veinte"
    }

    Private Shared ReadOnly TensString As String() = New String() {
        String.Empty,
        "diez", "veinti", "treinta", "cuarenta", "cincuenta",
        "sesenta", "setenta", "ochenta", "noventa", "cien"
    }

    Private Shared ReadOnly HundredsString As String() = New String() {
        String.Empty,
        "ciento", "doscientos", "trescientos", "cuatrocientos", "quinientos",
        "seiscientos", "setecientos", "ochocientos", "novecientos"
    }

    ' *
    ' * RAE's Spanish Cardinal Numerals Specification (seen on 9/17/2017):
    ' * http://lema.rae.es/dpd/srv/search?key=cardinales&origen=RAE&lema=cardinales
    ' *
    Public Shared Function ToCardinal(value As UInt64, Optional apocopate As Boolean = False) As String
        If value = 0 Then Return "cero"

        Dim nTrillones = Convert.ToInt32(value \ 1_000_000_000_000_000_000UL)
        value -= Convert.ToUInt64(nTrillones) * 1_000_000_000_000_000_000UL

        Dim nBillones = Convert.ToInt32(value \ 1_000_000_000_000UL)
        value -= Convert.ToUInt64(nBillones) * 1_000_000_000_000UL

        Dim nMillones = Convert.ToInt32(value \ 1_000_000UL)
        value -= Convert.ToUInt64(nMillones) * 1_000_000UL

        Dim nMiles = Convert.ToInt32(value \ 1000UL)
        value -= Convert.ToUInt64(nMiles) * 1000UL

        Dim nCentenas = Convert.ToInt32(value \ 100UL)
        value -= Convert.ToUInt64(nCentenas) * 100UL

        Dim nDecenas As Int32
        If value > 20 Then
            nDecenas = Convert.ToInt32(value \ 10UL)
            value -= Convert.ToUInt64(nDecenas) * 10UL
        End If

        Dim result = String.Empty

        If nTrillones > 0 Then
            result &= ToCardinal(Convert.ToUInt64(nTrillones), True)
            result &= If(nTrillones > 1, " trillones ", " trillón ")
        End If

        If nBillones > 0 Then
            result &= ToCardinal(Convert.ToUInt64(nBillones), True)
            result &= If(nBillones > 1, " billones ", " billón ")
        End If

        If nMillones > 0 Then
            result &= ToCardinal(Convert.ToUInt64(nMillones), True)
            result &= If(nMillones > 1, " millones ", " millón ")
        End If

        If nMiles > 0 Then
            If nMiles >= 2 Then
                result &= ToCardinal(Convert.ToUInt64(nMiles), True) & " mil "
            Else
                result &= "mil "
            End If
        End If

        If nCentenas > 0 Then
            If nCentenas = 1 And nDecenas = 0 And value = 0 Then
                result &= "cien"
            Else
                result &= HundredsString(nCentenas) & " "
            End If
        End If

        If nDecenas > 0 Then
            result &= TensString(nDecenas)
            If nDecenas >= 3 And value > 0 Then
                result &= " y "
            End If
        End If

        ' Default case: 1 <= value <= 20
        If value > 0 Then
            If nDecenas = 2 And value = 1 And apocopate Then
                result &= "ún"
            ElseIf nDecenas = 2 And value = 2 Then
                result &= "dós"
            ElseIf nDecenas = 2 And value = 3 Then
                result &= "trés"
            ElseIf nDecenas = 2 And value = 6 Then
                result &= "séis"
            Else
                result &= UnitsPlusString(Convert.ToInt32(value))
            End If

            If value = 1 And Not apocopate Then
                result &= "o"
            End If
        End If

        Return result.Trim()
    End Function
End Class