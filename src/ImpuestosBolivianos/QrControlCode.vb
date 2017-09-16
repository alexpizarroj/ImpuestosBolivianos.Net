Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports ZXing
Imports ZXing.QrCode
Imports ZXing.QrCode.Internal

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

    Private Shared ReadOnly Property QrCodeWriterOptions As New QrCodeEncodingOptions With
    {
        .ErrorCorrection = ErrorCorrectionLevel.M,
        .Width = 400,
        .Height = 400
    }

    Private Shared ReadOnly Property QrCodeWriter As New BarcodeWriter With {
        .Format = BarcodeFormat.QR_CODE,
        .Options = QrCodeWriterOptions
    }

    Public Function ToPngByteArray() As Byte()
        Using bitmap = QrCodeWriter.Write(Text)
            Dim mstream = New MemoryStream()
            bitmap.Save(mstream, ImageFormat.Png)
            Return mstream.ToArray()
        End Using
    End Function
End Class
