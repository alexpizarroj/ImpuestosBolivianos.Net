Imports System.Runtime.InteropServices

<ComVisible(True)>
<ComClass(ControlCodeBuilder.ClassId, ControlCodeBuilder.InterfaceId, ControlCodeBuilder.EventsId)>
Public Class ControlCodeBuilder
    Public Const ClassId As String = "CA14A5DA-15E8-4E24-A865-8522D58B6A72"
    Public Const InterfaceId As String = "A77BE2F8-CE32-4C19-8507-366031BE5FDE"
    Public Const EventsId As String = "CBE8E013-BD3F-481D-8F4A-C4A90730DDAB"
    Private _NroAutorizacion, _NroFactura, _NitCliente, _Fecha, _Monto, _Llave As String

    Public Sub New()
    End Sub

    Public Function WithNroAutorizacion(nroAutorizacion As String) As ControlCodeBuilder
        _NroAutorizacion = nroAutorizacion
        Return Me
    End Function

    Public Function WithNroFactura(nroFactura As String) As ControlCodeBuilder
        _NroFactura = nroFactura
        Return Me
    End Function

    Public Function WithNitCliente(nitCliente As String) As ControlCodeBuilder
        _NitCliente = nitCliente
        Return Me
    End Function

    Public Function WithFecha(fecha As DateTime) As ControlCodeBuilder
        _Fecha = fecha.ToString("yyyyMMdd")
        Return Me
    End Function

    Public Function WithMonto(monto As Double) As ControlCodeBuilder
        _Monto = Math.Round(monto, 0, MidpointRounding.AwayFromZero).ToString()
        Return Me
    End Function

    Public Function WithLlave(llave As String) As ControlCodeBuilder
        _Llave = llave
        Return Me
    End Function

    Public Function Build() As String
        Dim a = New ControlCodeBuilderImpl(_NroAutorizacion, _NroFactura, _NitCliente, _Fecha, _Monto, _Llave)
        Return a.Build()
    End Function

    Private Class ControlCodeBuilderImpl
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
            Dim digitosPaso1 As String = Paso1()

            Paso2(digitosPaso1)

            Dim cadenaAllegedPaso3 As String = Paso3(digitosPaso1)

            Dim sumaTotalPaso4 As Int32 = 0, sumasParcialesPaso4() As Int32 = New Int32(4) {0, 0, 0, 0, 0}
            Paso4(cadenaAllegedPaso3, sumaTotalPaso4, sumasParcialesPaso4)

            Dim sumatoriaPaso5 As String = Paso5(digitosPaso1, sumaTotalPaso4, sumasParcialesPaso4)

            Dim codigoControl As String = Paso6(digitosPaso1, sumatoriaPaso5)

            Return codigoControl
        End Function

        Private Function Paso1() As String
            _NroFactura = AgregarDigitosVerhoeff(_NroFactura, 2)
            _NitCliente = AgregarDigitosVerhoeff(_NitCliente, 2)
            _Fecha = AgregarDigitosVerhoeff(_Fecha, 2)
            _Monto = AgregarDigitosVerhoeff(_Monto, 2)

            Dim sumaDatos As String = Convert.ToString(
                Convert.ToInt64(_NroFactura) +
                Convert.ToInt64(_NitCliente) +
                Convert.ToInt64(_Fecha) +
                Convert.ToInt64(_Monto))
            Dim sumaDatosConVerhoeff As String = AgregarDigitosVerhoeff(sumaDatos, 5)

            Dim digitosResultado As String = sumaDatosConVerhoeff.Substring(sumaDatos.Length)
            Return digitosResultado
        End Function

        Private Function AgregarDigitosVerhoeff(cadena As String, nDigitos As Int32) As String
            If nDigitos <= 0 Then Return cadena

            Return AgregarDigitosVerhoeff(cadena + VerhoeffDigitCalculator.Get(cadena).ToString(), nDigitos - 1)
        End Function

        Private Sub Paso2(digitosPaso1 As String)
            Dim subcadenas(4) As String

            Dim posInicioCorte As Int32 = 0
            For i As Int32 = 0 To 4
                Dim longitudCorte As Int32 = Int32.Parse(digitosPaso1(i)) + 1
                subcadenas(i) = _Llave.Substring(posInicioCorte, longitudCorte)
                posInicioCorte += longitudCorte
            Next

            _NroAutorizacion += subcadenas(0)
            _NroFactura += subcadenas(1)
            _NitCliente += subcadenas(2)
            _Fecha += subcadenas(3)
            _Monto += subcadenas(4)
        End Sub

        Private Function Paso3(digitosPaso1 As String) As String
            Dim texto = _NroAutorizacion + _NroFactura + _NitCliente + _Fecha + _Monto
            Dim llave = _Llave + digitosPaso1
            Dim resultado = AllegedRC4Cipher.Encode(texto, llave).Replace("-"c, "")
            Return resultado
        End Function

        Private Sub Paso4(
                cadenaAllegedPaso3 As String, ByRef sumaTotal As Int32, ByRef sumasParciales() As Int32)
            sumaTotal = 0
            For i As Int32 = 0 To (cadenaAllegedPaso3.Length - 1) Step 5
                For j As Int32 = 0 To 4
                    Dim k = i + j
                    If (k < cadenaAllegedPaso3.Length) Then
                        Dim valor = Convert.ToInt32(cadenaAllegedPaso3(k))
                        sumaTotal += valor
                        sumasParciales(j) += valor
                    End If
                Next
            Next
        End Sub

        Private Function Paso5(
            digitosPaso1 As String, sumaTotalPaso4 As Int32, sumasParcialesPaso4() As Int32) _
        As String
            Dim suma As Int64 = 0
            For i As Int32 = 0 To 4
                Dim valor As Int64 = sumaTotalPaso4
                valor = valor * sumasParcialesPaso4(i)
                valor = valor \ (Int32.Parse(digitosPaso1(i)) + 1)
                suma += valor
            Next

            Dim resultado = Base64Encoder.Encode(suma)
            Return resultado
        End Function

        Private Function Paso6(digitosPaso1 As String, sumatoriaPaso5 As String) As String
            Dim texto = sumatoriaPaso5
            Dim llave = _Llave + digitosPaso1
            Dim resultado = AllegedRC4Cipher.Encode(texto, llave)
            Return resultado
        End Function
    End Class
End Class

