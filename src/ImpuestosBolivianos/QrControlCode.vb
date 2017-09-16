Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports Gma.QrCodeNet.Encoding
Imports Gma.QrCodeNet.Encoding.Windows.Render

Public Class QrControlCode
    Private ReadOnly ImpuestosNacionalesNFI As New NumberFormatInfo With
        {.NumberDecimalSeparator = ".", .NumberGroupSeparator = "", .NumberDecimalDigits = 2}
    Private ReadOnly ResultingImageFormat As ImageFormat = ImageFormat.Png

    Private _NroAutorizacion, _NroFactura As String
    Private _Fecha, _CodigoControl As String
    Private _NitEmisor, _NitCliente As String
    Private _ImporteTotal, _ImporteBaseCf As String
    Private _ImporteIceIehdTasas, _ImporteVentasNoGravadas As String
    Private _ImporteNoSujetoCf, _DescuentosBonosRebajas As String

    Public Sub New(invoice As Invoice)
        invoice.AssertHasEnoughInfoToRenderQrCode()

        _NroAutorizacion = invoice.NroAutorizacion.ToString()
        _NroFactura = invoice.NroFactura.ToString()
        _Fecha = StringifyQrCodeTextContentElement(invoice.Fecha)
        _CodigoControl = invoice.CodigoControl
        _NitEmisor = invoice.NitEmisor
        _NitCliente = invoice.NitCliente
        _ImporteTotal = StringifyQrCodeTextContentElement(invoice.ImporteTotal)
        _ImporteBaseCf = StringifyQrCodeTextContentElement(invoice.ImporteBaseCf)
        _ImporteIceIehdTasas = StringifyQrCodeTextContentElement(invoice.ImporteIceIehdTasas)
        _ImporteVentasNoGravadas = StringifyQrCodeTextContentElement(invoice.ImporteVentasNoGravadas)
        _ImporteNoSujetoCf = StringifyQrCodeTextContentElement(invoice.ImporteNoSujetoCf)
        _DescuentosBonosRebajas = StringifyQrCodeTextContentElement(invoice.DescuentosBonosRebajas)
    End Sub

    Private Function StringifyQrCodeTextContentElement(value As Double) As String
        ' value <= 0: "No corresponde"; se pone exactamente "0"
        If (value <= 0) Then Return "0"
        ' value > 0: Formatear con dos decimas, usando "." como separador decimal
        Return value.ToString("0.00", ImpuestosNacionalesNFI)
    End Function

    Private Function StringifyQrCodeTextContentElement(value As DateTime) As String
        Return value.ToString("dd/MM/yyyy")
    End Function

    Public ReadOnly Property TextContents As String
        Get
            Dim parts = New String() {
                _NitEmisor,
                _NroFactura,
                _NroAutorizacion,
                _Fecha,
                _ImporteTotal,
                _ImporteBaseCf,
                _CodigoControl,
                _NitCliente,
                _ImporteIceIehdTasas,
                _ImporteVentasNoGravadas,
                _ImporteNoSujetoCf,
                _DescuentosBonosRebajas
            }
            Return String.Join("|", parts)
        End Get
    End Property

    Public Function ToPngByteArray() As Byte()
        Dim encoder As New QrEncoder(ErrorCorrectionLevel.M)
        Dim resultingQrCode As QrCode = Nothing
        encoder.TryEncode(TextContents, resultingQrCode)

        Const moduleSizeInPixels As Int32 = 5
        Dim renderer As New GraphicsRenderer(
            New FixedModuleSize(moduleSizeInPixels, QuietZoneModules.Two), Brushes.Black, Brushes.White)

        Using mstream = New MemoryStream()
            renderer.WriteToStream(resultingQrCode.Matrix, ResultingImageFormat, mstream)
            Return mstream.ToArray()
        End Using
    End Function
End Class
