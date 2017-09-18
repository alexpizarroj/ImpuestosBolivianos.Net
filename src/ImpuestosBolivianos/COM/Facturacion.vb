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
            monto As Double,
            llave As String
        ) As String
            Return ImpuestosBolivianos.Facturacion.MakeControlCode(
                Convert.ToInt64(nroAutorizacion),
                Convert.ToInt64(nroFactura),
                nitCliente,
                fecha,
                Convert.ToDecimal(monto),
                llave
            )
        End Function

        Public Function RenderPngQrCode(
            nroAutorizacion As String,
            nroFactura As String,
            nitCliente As String,
            fecha As DateTime,
            importeTotal As Double,
            codigoControl As String,
            nitEmisor As String,
            importeBaseCf As Double,
            Optional importeIceIehdTasas As Double = 0.0,
            Optional importeVentasNoGravadas As Double = 0.0,
            Optional importeNoSujetoCf As Double = 0.0,
            Optional descuentosBonosRebajas As Double = 0.0
        ) As Byte()
            Return ImpuestosBolivianos.Facturacion.RenderPngQrCode(
                Convert.ToInt64(nroAutorizacion),
                Convert.ToInt64(nroFactura),
                nitCliente,
                fecha,
                Convert.ToDecimal(importeTotal),
                codigoControl,
                nitEmisor,
                Convert.ToDecimal(importeBaseCf),
                Convert.ToDecimal(importeIceIehdTasas),
                Convert.ToDecimal(importeVentasNoGravadas),
                Convert.ToDecimal(importeNoSujetoCf),
                Convert.ToDecimal(descuentosBonosRebajas)
            )
        End Function

        Public Function StringifyInvoiceAmount(amount As Double) As String
            Return ImpuestosBolivianos.Facturacion.StringifyInvoiceAmount(Convert.ToDecimal(amount))
        End Function
    End Class
End Namespace