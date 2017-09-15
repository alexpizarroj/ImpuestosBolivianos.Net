Imports System.Runtime.InteropServices

Namespace COM
    <ComVisible(True)>
    <ComClass(Facturacion.ClassId, Facturacion.InterfaceId, Facturacion.EventsId)>
    Public Class Facturacion
        Public Const ClassId As String = "CA14A5DA-15E8-4E24-A865-8522D58B6A72"
        Public Const InterfaceId As String = "A77BE2F8-CE32-4C19-8507-366031BE5FDE"
        Public Const EventsId As String = "CBE8E013-BD3F-481D-8F4A-C4A90730DDAB"

        Public Function MakeControlCode(
            nroAutorizacion As String,
            nroFactura As String,
            nitCliente As String,
            fecha As DateTime,
            monto As String,
            llave As String
        ) As String
            Dim builder = New ControlCodeBuilder()
            With builder
                .WithNroAutorizacion(nroAutorizacion)
                .WithNroFactura(nroFactura)
                .WithNitCliente(nitCliente)
                .WithFecha(fecha)
                .WithMonto(Convert.ToDouble(monto))
                .WithLlave(llave)
            End With
            Return builder.Build()
        End Function

        Public Function RenderPngQrCode(
            nroAutorizacion As String,
            nroFactura As String,
            nitCliente As String,
            fecha As DateTime,
            importeTotal As String,
            codigoControl As String,
            nitEmisor As String,
            importeBaseCf As String,
            Optional importeIceIehdTasas As String = "0",
            Optional importeVentasNoGravadas As String = "0",
            Optional importeNoSujetoCf As String = "0",
            Optional descuentosBonosRebajas As String = "0"
        ) As Byte()
            Dim builder = New QrCodeBuilder()
            With builder
                .WithNroAutorizacion(nroAutorizacion)
                .WithNroFactura(nroFactura)
                .WithNitCliente(nitCliente)
                .WithFecha(fecha)
                .WithImporteTotal(Convert.ToDouble(importeTotal))
                .WithCodigoControl(codigoControl)
                .WithNitEmisor(nitEmisor)
                .WithImporteBaseCf(Convert.ToDouble(importeBaseCf))
                .WithImporteIceIehdTasas(Convert.ToDouble(importeIceIehdTasas))
                .WithImporteVentasNoGravadas(Convert.ToDouble(importeVentasNoGravadas))
                .WithImporteNoSujetoCf(Convert.ToDouble(importeNoSujetoCf))
                .WithDescuentosBonosRebajas(Convert.ToDouble(descuentosBonosRebajas))
            End With
            Return builder.ToPngByteArray()
        End Function
    End Class
End Namespace