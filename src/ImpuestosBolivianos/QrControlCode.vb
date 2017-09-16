Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports Gma.QrCodeNet.Encoding
Imports Gma.QrCodeNet.Encoding.Windows.Render

Public Class QrControlCode
    Public Sub New(invoice As Invoice)
        invoice.AssertHasEnoughInfoToRenderQrCode()

        Dim nroAutorizacion = invoice.NroAutorizacion.ToString()
        Dim nroFactura = invoice.NroFactura.ToString()
        Dim fecha = invoice.Fecha.ToString("dd/MM/yyyy")
        Dim codigoControl = invoice.CodigoControl
        Dim nitEmisor = invoice.NitEmisor
        Dim nitCliente = invoice.NitCliente
        Dim importeTotal = StringifyDecimal(invoice.ImporteTotal)
        Dim importeBaseCf = StringifyDecimal(invoice.ImporteBaseCf)
        Dim importeIceIehdTasas = StringifyDecimal(invoice.ImporteIceIehdTasas)
        Dim importeVentasNoGravadas = StringifyDecimal(invoice.ImporteVentasNoGravadas)
        Dim importeNoSujetoCf = StringifyDecimal(invoice.ImporteNoSujetoCf)
        Dim descuentosBonosRebajas = StringifyDecimal(invoice.DescuentosBonosRebajas)

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

    Private ReadOnly Property ImpuestosNacionalesNFI As New NumberFormatInfo With
        {.NumberDecimalSeparator = ".", .NumberGroupSeparator = "", .NumberDecimalDigits = 2}

    Private ReadOnly Property ResultingImageFormat As ImageFormat = ImageFormat.Png

    Private Function StringifyDecimal(value As Decimal) As String
        ' value <= 0: "No corresponde"; se pone exactamente "0"
        If (value <= 0) Then Return "0"
        ' value > 0: Formatear con dos decimas, usando "." como separador decimal
        Return value.ToString("0.00", ImpuestosNacionalesNFI)
    End Function

    Public Function ToPngByteArray() As Byte()
        Dim encoder As New QrEncoder(ErrorCorrectionLevel.M)
        Dim resultingQrCode As QrCode = Nothing
        encoder.TryEncode(Text, resultingQrCode)

        Const moduleSizeInPixels As Int32 = 5
        Dim renderer As New GraphicsRenderer(
            New FixedModuleSize(moduleSizeInPixels, QuietZoneModules.Two), Brushes.Black, Brushes.White)

        Using mstream = New MemoryStream()
            renderer.WriteToStream(resultingQrCode.Matrix, ResultingImageFormat, mstream)
            Return mstream.ToArray()
        End Using
    End Function
End Class
