Public NotInheritable Class Facturacion
    Private Sub New()
    End Sub

    Public Shared Function MakeControlCode(
        nroAutorizacion As Int64,
        nroFactura As Int64,
        nitCliente As String,
        fecha As DateTime,
        monto As Decimal,
        llave As String
    ) As String
        Dim invoice As New Invoice With {
            .NroAutorizacion = nroAutorizacion,
            .NroFactura = nroFactura,
            .NitCliente = nitCliente,
            .Fecha = fecha,
            .ImporteTotal = monto,
            .LlaveDosificacion = llave
        }
        Return New ControlCode(invoice).Text
    End Function

    Public Shared Function RenderPngQrCode(
        nroAutorizacion As Int64,
        nroFactura As Int64,
        nitCliente As String,
        fecha As DateTime,
        importeTotal As Decimal,
        codigoControl As String,
        nitEmisor As String,
        importeBaseCf As Decimal,
        Optional importeIceIehdTasas As Decimal = 0,
        Optional importeVentasNoGravadas As Decimal = 0,
        Optional importeNoSujetoCf As Decimal = 0,
        Optional descuentosBonosRebajas As Decimal = 0
    ) As Byte()
        Dim invoice As New Invoice With {
            .NroAutorizacion = nroAutorizacion,
            .NroFactura = nroFactura,
            .NitCliente = nitCliente,
            .Fecha = fecha,
            .ImporteTotal = importeTotal,
            .CodigoControl = codigoControl,
            .NitEmisor = nitEmisor,
            .ImporteBaseCf = importeBaseCf,
            .ImporteIceIehdTasas = importeIceIehdTasas,
            .ImporteVentasNoGravadas = importeVentasNoGravadas,
            .ImporteNoSujetoCf = importeNoSujetoCf,
            .DescuentosBonosRebajas = descuentosBonosRebajas
        }
        Return New QrControlCode(invoice).ToPngByteArray()
    End Function

    Public Shared Function StringifyInvoiceAmount(amount As Decimal) As String
        Return LawConventions.StringifyInvoiceAmount(amount)
    End Function
End Class
