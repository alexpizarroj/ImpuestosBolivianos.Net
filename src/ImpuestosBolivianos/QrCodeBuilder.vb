Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports Gma.QrCodeNet.Encoding
Imports Gma.QrCodeNet.Encoding.Windows.Render

Public Class QrCodeBuilder
    Private ReadOnly ImpuestosNacionalesNFI As New NumberFormatInfo With
        {.NumberDecimalSeparator = ".", .NumberGroupSeparator = "", .NumberDecimalDigits = 2}
    Private ReadOnly ResultingImageFormat As ImageFormat = ImageFormat.Png
    Private _NroAutorizacion, _NroFactura, _Fecha, _CodigoControl As String
    Private _NitEmisor, _NitCliente As String
    Private _ImporteTotal, _ImporteBaseCf As String
    Private _ImporteIceIehdTasas As String = StringifyQrCodeTextContentElement(0.0)
    Private _ImporteVentasNoGravadas As String = StringifyQrCodeTextContentElement(0.0)
    Private _ImporteNoSujetoCf As String = StringifyQrCodeTextContentElement(0.0)
    Private _DescuentosBonosRebajas As String = StringifyQrCodeTextContentElement(0.0)

    Public Function WithNroAutorizacion(nroAutorizacion As String) As QrCodeBuilder
        _NroAutorizacion = nroAutorizacion
        Return Me
    End Function

    Public Function WithNroFactura(nroFactura As String) As QrCodeBuilder
        _NroFactura = nroFactura
        Return Me
    End Function

    Public Function WithFecha(fecha As DateTime) As QrCodeBuilder
        _Fecha = StringifyQrCodeTextContentElement(fecha)
        Return Me
    End Function

    Public Function WithCodigoControl(codigoControl As String) As QrCodeBuilder
        _CodigoControl = codigoControl
        Return Me
    End Function

    Public Function WithNitEmisor(nitEmisor As String) As QrCodeBuilder
        _NitEmisor = nitEmisor
        Return Me
    End Function

    Public Function WithNitCliente(nitCliente As String) As QrCodeBuilder
        _NitCliente = nitCliente
        Return Me
    End Function

    Public Function WithImporteTotal(importe As Double) As QrCodeBuilder
        _ImporteTotal = StringifyQrCodeTextContentElement(importe)
        Return Me
    End Function

    Public Function WithImporteBaseCf(importe As Double) As QrCodeBuilder
        _ImporteBaseCf = StringifyQrCodeTextContentElement(importe)
        Return Me
    End Function

    Public Function WithImporteIceIehdTasas(importe As Double) As QrCodeBuilder
        _ImporteIceIehdTasas = StringifyQrCodeTextContentElement(importe)
        Return Me
    End Function

    Public Function WithImporteVentasNoGravadas(importe As Double) As QrCodeBuilder
        _ImporteVentasNoGravadas = StringifyQrCodeTextContentElement(importe)
        Return Me
    End Function

    Public Function WithImporteNoSujetoCf(importe As Double) As QrCodeBuilder
        _ImporteNoSujetoCf = StringifyQrCodeTextContentElement(importe)
        Return Me
    End Function

    Public Function WithDescuentosBonosRebajas(importe As Double) As QrCodeBuilder
        _DescuentosBonosRebajas = StringifyQrCodeTextContentElement(importe)
        Return Me
    End Function

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
