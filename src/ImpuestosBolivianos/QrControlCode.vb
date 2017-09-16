Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports Gma.QrCodeNet.Encoding
Imports Gma.QrCodeNet.Encoding.Windows.Render

Public Class QrControlCode
    Public Sub New(invoice As Invoice)
        invoice.AssertHasEnoughInfoToRenderQrCode()

        Dim nroAutorizacion = invoice.NroAutorizacion.ToString()
        Dim nroFactura = invoice.NroFactura.ToString()
        Dim fecha = LawConventions.QrControlCode.StringifyDateTime(invoice.Fecha)
        Dim codigoControl = invoice.CodigoControl
        Dim nitEmisor = invoice.NitEmisor
        Dim nitCliente = invoice.NitCliente
        Dim importeTotal = LawConventions.QrControlCode.StringifyDecimal(invoice.ImporteTotal)
        Dim importeBaseCf = LawConventions.QrControlCode.StringifyDecimal(invoice.ImporteBaseCf)
        Dim importeIceIehdTasas = LawConventions.QrControlCode.StringifyDecimal(invoice.ImporteIceIehdTasas)
        Dim importeVentasNoGravadas = LawConventions.QrControlCode.StringifyDecimal(invoice.ImporteVentasNoGravadas)
        Dim importeNoSujetoCf = LawConventions.QrControlCode.StringifyDecimal(invoice.ImporteNoSujetoCf)
        Dim descuentosBonosRebajas = LawConventions.QrControlCode.StringifyDecimal(invoice.DescuentosBonosRebajas)

        Dim textElements = New String() {
            nitEmisor,
            nroFactura,
            nroAutorizacion,
            fecha,
            importeTotal,
            importeBaseCf,
            codigoControl,
            nitCliente,
            importeIceIehdTasas,
            importeVentasNoGravadas,
            importeNoSujetoCf,
            descuentosBonosRebajas
        }
        Text = String.Join("|", textElements)
    End Sub

    Public ReadOnly Property Text As String

    Public Function ToPngByteArray() As Byte()
        Dim encoder As New QrEncoder(ErrorCorrectionLevel.M)
        Dim resultingQrCode As QrCode = Nothing
        encoder.TryEncode(Text, resultingQrCode)

        Const moduleSizeInPixels As Int32 = 5
        Dim renderer As New GraphicsRenderer(
            New FixedModuleSize(moduleSizeInPixels, QuietZoneModules.Two), Brushes.Black, Brushes.White)

        Using mstream = New MemoryStream()
            renderer.WriteToStream(resultingQrCode.Matrix, ImageFormat.Png, mstream)
            Return mstream.ToArray()
        End Using
    End Function
End Class
