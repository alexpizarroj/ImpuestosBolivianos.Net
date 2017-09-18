Imports System.Text

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
    Public Shared Function ToCardinal(
        value As UInt64,
        Optional apocopate As Boolean = False,
        Optional useVerboseBusinessMode As Boolean = False
    ) As String
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

        Dim resultBuilder As New StringBuilder

        If nTrillones > 0 Then
            resultBuilder.Append(ToCardinal(Convert.ToUInt64(nTrillones), True, useVerboseBusinessMode))
            resultBuilder.Append(If(nTrillones > 1, " trillones ", " trillón "))
        End If

        If nBillones > 0 Then
            resultBuilder.Append(ToCardinal(Convert.ToUInt64(nBillones), True, useVerboseBusinessMode))
            resultBuilder.Append(If(nBillones > 1, " billones ", " billón "))
        End If

        If nMillones > 0 Then
            resultBuilder.Append(ToCardinal(Convert.ToUInt64(nMillones), True, useVerboseBusinessMode))
            resultBuilder.Append(If(nMillones > 1, " millones ", " millón "))
        End If

        If nMiles > 0 Then
            If nMiles >= 2 Or useVerboseBusinessMode Then
                resultBuilder.Append(ToCardinal(Convert.ToUInt64(nMiles), True, useVerboseBusinessMode))
                resultBuilder.Append(" mil ")
            Else
                resultBuilder.Append("mil ")
            End If
        End If

        If nCentenas > 0 Then
            If nCentenas = 1 And nDecenas = 0 And value = 0 Then
                resultBuilder.Append("cien")
            Else
                resultBuilder.Append(HundredsString(nCentenas))
                resultBuilder.Append(" ")
            End If
        End If

        If nDecenas > 0 Then
            resultBuilder.Append(TensString(nDecenas))
            If nDecenas >= 3 And value > 0 Then
                resultBuilder.Append(" y ")
            End If
        End If

        ' Default case: 1 <= value <= 20
        If value > 0 Then
            If nDecenas = 2 And value = 1 And apocopate Then
                resultBuilder.Append("ún")
            ElseIf nDecenas = 2 And value = 2 Then
                resultBuilder.Append("dós")
            ElseIf nDecenas = 2 And value = 3 Then
                resultBuilder.Append("trés")
            ElseIf nDecenas = 2 And value = 6 Then
                resultBuilder.Append("séis")
            Else
                resultBuilder.Append(UnitsPlusString(Convert.ToInt32(value)))
            End If

            If value = 1 And Not apocopate Then
                resultBuilder.Append("o")
            End If
        End If

        Return resultBuilder.ToString().Trim()
    End Function
End Class