Public Class ControlCode
    Public Sub New(invoice As Invoice)
        invoice.AssertHasEnoughInfoToMakeCodigoControl()

        Dim nroAutorizacion = invoice.NroAutorizacion.ToString()
        Dim nroFactura = invoice.NroFactura.ToString()
        Dim nitCliente = invoice.NitCliente
        Dim fecha = LawConventions.ControlCode.StringifyDateTime(invoice.Fecha)
        Dim importeTotal = LawConventions.ControlCode.RoundImporteTotal(invoice.ImporteTotal).ToString()
        Dim llave = invoice.LlaveDosificacion

        Dim builder = New ControlCodeBuilder(nroAutorizacion, nroFactura, nitCliente, fecha, importeTotal, llave)
        Text = builder.Build()
    End Sub

    Public ReadOnly Property Text As String

    Private Class ControlCodeBuilder
        Private _BuildStepOneResult As String
        Private _BuildStepThreeResult As String
        Private _BuildStepFourResult(5) As Int32
        Private _BuildStepFiveResult As String
        Private _BuildStepSixResult As String
        Private _NroAutorizacion, _NroFactura, _NitCliente, _Fecha, _Monto, _Llave As String

        Public Sub New(
                nroAutorizacion As String,
                nroFactura As String,
                nitCliente As String,
                fecha As String,
                monto As String,
                llave As String)
            _NroAutorizacion = nroAutorizacion
            _NroFactura = nroFactura
            _NitCliente = nitCliente
            _Fecha = fecha
            _Monto = monto
            _Llave = llave
        End Sub

        Public Function Build() As String
            BuildStepOne()
            BuildStepTwo()
            BuildStepThree()
            BuildStepFour()
            BuildStepFive()
            BuildStepSix()
            Return _BuildStepSixResult
        End Function

        Private Sub BuildStepOne()
            _NroFactura = AppendVerhoeffDigits(_NroFactura, 2)
            _NitCliente = AppendVerhoeffDigits(_NitCliente, 2)
            _Fecha = AppendVerhoeffDigits(_Fecha, 2)
            _Monto = AppendVerhoeffDigits(_Monto, 2)

            Dim dataSum As String = Convert.ToString(
                Convert.ToInt64(_NroFactura) +
                Convert.ToInt64(_NitCliente) +
                Convert.ToInt64(_Fecha) +
                Convert.ToInt64(_Monto))
            Dim dataSumWithTrailingVerhoeffDigits As String = AppendVerhoeffDigits(dataSum, 5)

            _BuildStepOneResult = dataSumWithTrailingVerhoeffDigits.Substring(dataSum.Length)
        End Sub

        Private Function AppendVerhoeffDigits(text As String, nDigits As Int32) As String
            If nDigits <= 0 Then Return text
            Return AppendVerhoeffDigits(text + CustomVerhoeffDigitCalculator.Get(text).ToString(), nDigits - 1)
        End Function

        Private Sub BuildStepTwo()
            Dim cuts(4) As String
            Dim cutStartingPos As Int32 = 0
            For i As Int32 = 0 To 4
                Dim cutLength As Int32 = Int32.Parse(_BuildStepOneResult(i)) + 1
                cuts(i) = _Llave.Substring(cutStartingPos, cutLength)
                cutStartingPos += cutLength
            Next

            _NroAutorizacion += cuts(0)
            _NroFactura += cuts(1)
            _NitCliente += cuts(2)
            _Fecha += cuts(3)
            _Monto += cuts(4)
        End Sub

        Private Sub BuildStepThree()
            Dim text = _NroAutorizacion + _NroFactura + _NitCliente + _Fecha + _Monto
            Dim key = _Llave + _BuildStepOneResult
            _BuildStepThreeResult = CustomAllegedRC4Cipher.Encode(text, key).Replace("-"c, "")
        End Sub

        Private Sub BuildStepFour()
            For i As Int32 = 0 To (_BuildStepFourResult.Length - 1)
                _BuildStepFourResult(i) = 0
            Next

            For i As Int32 = 0 To (_BuildStepThreeResult.Length - 1) Step 5
                For j As Int32 = 0 To 4
                    Dim k = i + j
                    If (k < _BuildStepThreeResult.Length) Then
                        Dim value = Convert.ToInt32(_BuildStepThreeResult(k))
                        _BuildStepFourResult(0) += value
                        _BuildStepFourResult(j + 1) += value
                    End If
                Next
            Next
        End Sub

        Private Sub BuildStepFive()
            Dim sum As Int64 = 0
            For i As Int32 = 0 To 4
                Dim value As Int64 = _BuildStepFourResult(0)
                value = value * _BuildStepFourResult(i + 1)
                value = value \ (Int32.Parse(_BuildStepOneResult(i)) + 1)
                sum += value
            Next

            _BuildStepFiveResult = CustomBase64Encoder.Encode(sum)
        End Sub

        Private Sub BuildStepSix()
            Dim text = _BuildStepFiveResult
            Dim key = _Llave + _BuildStepOneResult
            _BuildStepSixResult = CustomAllegedRC4Cipher.Encode(text, key)
        End Sub
    End Class
End Class

