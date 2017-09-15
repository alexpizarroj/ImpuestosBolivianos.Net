Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports System.Runtime.InteropServices
Imports Gma.QrCodeNet.Encoding
Imports Gma.QrCodeNet.Encoding.Windows.Render

<ComVisible(True)>
<ComClass(QrCodeBuilder.ClassId, QrCodeBuilder.InterfaceId, QrCodeBuilder.EventsId)>
Public Class QrCodeBuilder
    Public Const ClassId As String = "89081d2d-b97f-4d9a-b822-e573c16c6f09"
    Public Const InterfaceId As String = "19c8debe-bced-4a2b-8b26-2a275e5a7550"
    Public Const EventsId As String = "55ffcfe5-9c96-4803-bea4-e027145dff60"

    Private ReadOnly ImpuestosNacionalesNFI As New NumberFormatInfo With
        {.NumberDecimalSeparator = ".", .NumberGroupSeparator = "", .NumberDecimalDigits = 2}
    Private ReadOnly DefaultImageFormat As ImageFormat = ImageFormat.Png
    Private _NroAutorizacion, _NroFactura, _Fecha, _CodigoControl As String
    Private _NitEmisor, _NitCliente As String
    Private _ImporteTotal, _ImporteBaseCf As String
    Private _ImporteIceIehdTasas As String = StringifyForQrContent(0.0)
    Private _ImporteVentasNoGravadas As String = StringifyForQrContent(0.0)
    Private _ImporteNoSujetoCf As String = StringifyForQrContent(0.0)
    Private _DescuentosBonosRebajas As String = StringifyForQrContent(0.0)

    Public Function WithNroAutorizacion(nroAutorizacion As String) As QrCodeBuilder
        _NroAutorizacion = nroAutorizacion
        Return Me
    End Function

    Public Function WithNroFactura(nroFactura As String) As QrCodeBuilder
        _NroFactura = nroFactura
        Return Me
    End Function

    Public Function WithFecha(fecha As DateTime) As QrCodeBuilder
        _Fecha = StringifyForQrContent(fecha)
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
        _ImporteTotal = StringifyForQrContent(importe)
        Return Me
    End Function

    Public Function WithImporteBaseCf(importe As Double) As QrCodeBuilder
        _ImporteBaseCf = StringifyForQrContent(importe)
        Return Me
    End Function

    Public Function WithImporteIceIehdTasas(importe As Double) As QrCodeBuilder
        _ImporteIceIehdTasas = StringifyForQrContent(importe)
        Return Me
    End Function

    Public Function WithImporteVentasNoGravadas(importe As Double) As QrCodeBuilder
        _ImporteVentasNoGravadas = StringifyForQrContent(importe)
        Return Me
    End Function

    Public Function WithImporteNoSujetoCf(importe As Double) As QrCodeBuilder
        _ImporteNoSujetoCf = StringifyForQrContent(importe)
        Return Me
    End Function

    Public Function WithDescuentosBonosRebajas(importe As Double) As QrCodeBuilder
        _DescuentosBonosRebajas = StringifyForQrContent(importe)
        Return Me
    End Function

    Private Function StringifyForQrContent(value As Double) As String
        ' value <= 0: "No corresponde"; se pone exactamente "0"
        If (value <= 0) Then Return "0"
        ' value > 0: Formatear con dos decimas, usando "." como separador decimal
        Return value.ToString("0.00", ImpuestosNacionalesNFI)
    End Function

    Private Function StringifyForQrContent(value As DateTime) As String
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

    <ComVisible(False)>
    Public Sub WriteToStream(stream As Stream)
        WriteToStream(stream, DefaultImageFormat)
    End Sub

    <ComVisible(False)>
    Public Sub WriteToStream(stream As Stream, imageFormat As ImageFormat)
        Dim encoder As New QrEncoder(ErrorCorrectionLevel.M)
        Dim qrCode As QrCode = Nothing
        encoder.TryEncode(TextContents, qrCode)

        Const moduleSizeInPixels As Int32 = 5
        Dim renderer As New GraphicsRenderer(
            New FixedModuleSize(moduleSizeInPixels, QuietZoneModules.Two), Brushes.Black, Brushes.White)

        renderer.WriteToStream(qrCode.Matrix, imageFormat, stream)
    End Sub

    Public Function WriteToTemporaryFile() As String
        Dim path = IO.Path.GetTempFileName()
        WriteToFile(path)
        Return path
    End Function

    Public Sub WriteToFile(path As String)
        WriteToFile(path, DefaultImageFormat)
    End Sub

    <ComVisible(False)>
    Public Sub WriteToFile(path As String, imageFormat As ImageFormat)
        Using fstream As New FileStream(path, FileMode.Create, IO.FileAccess.Write)
            WriteToStream(fstream, imageFormat)
        End Using
    End Sub

    Public Function ToByteArray() As Byte()
        Return ToByteArray(DefaultImageFormat)
    End Function

    <ComVisible(False)>
    Public Function ToByteArray(imageFormat As ImageFormat) As Byte()
        Using mstream As New MemoryStream
            WriteToStream(mstream, imageFormat)
            Return mstream.ToArray()
        End Using
    End Function

    <ComVisible(False)>
    Public Function ToBitmap() As Bitmap
        Using mstream = New MemoryStream()
            WriteToStream(mstream)
            Return DirectCast(Image.FromStream(mstream), Bitmap)
        End Using
    End Function
End Class
