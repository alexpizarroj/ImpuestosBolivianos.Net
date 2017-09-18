Public Structure Invoice
    Public NroAutorizacion As Int64
    Public NroFactura As Int64
    Public Fecha As Nullable(Of DateTime)
    Public LlaveDosificacion As String
    Public CodigoControl As String
    Public NitEmisor As String
    Public NitCliente As String
    Public ImporteTotal As Decimal
    Public ImporteBaseCf As Decimal
    Public ImporteIceIehdTasas As Decimal
    Public ImporteVentasNoGravadas As Decimal
    Public ImporteNoSujetoCf As Decimal
    Public DescuentosBonosRebajas As Decimal

    Public Sub AssertHasEnoughInfoToMakeCodigoControl()
        ThrowIfInvalid(NroAutorizacion, NameOf(NroAutorizacion))
        ThrowIfInvalid(NroFactura, NameOf(NroFactura))
        ThrowIfInvalid(NitCliente, NameOf(NitCliente))
        ThrowIfInvalid(Fecha, NameOf(Fecha))
        ThrowIfInvalid(ImporteTotal, NameOf(ImporteTotal))
        ThrowIfInvalid(LlaveDosificacion, NameOf(LlaveDosificacion))
    End Sub

    Public Sub AssertHasEnoughInfoToRenderQrCode()
        ThrowIfInvalid(NitEmisor, NameOf(NitEmisor))
        ThrowIfInvalid(NroFactura, NameOf(NroFactura))
        ThrowIfInvalid(NroAutorizacion, NameOf(NroAutorizacion))
        ThrowIfInvalid(Fecha, NameOf(Fecha))
        ThrowIfInvalid(ImporteTotal, NameOf(ImporteTotal))
        ThrowIfInvalid(ImporteBaseCf, NameOf(ImporteBaseCf))
        ThrowIfInvalid(CodigoControl, NameOf(CodigoControl))
        ThrowIfInvalid(NitCliente, NameOf(NitCliente))
        ThrowIfInvalid(ImporteIceIehdTasas, NameOf(ImporteIceIehdTasas))
        ThrowIfInvalid(ImporteVentasNoGravadas, NameOf(ImporteVentasNoGravadas))
        ThrowIfInvalid(ImporteNoSujetoCf, NameOf(ImporteNoSujetoCf))
        ThrowIfInvalid(DescuentosBonosRebajas, NameOf(DescuentosBonosRebajas))
    End Sub

    Private Sub ThrowIfInvalid(value As Int64, argumentName As String)
        If value = 0 Then ThrowArgumentException(argumentName)
    End Sub

    Private Sub ThrowIfInvalid(value As String, argumentName As String)
        If value = String.Empty Then ThrowArgumentException(argumentName)
    End Sub

    Private Sub ThrowIfInvalid(value As Nullable(Of DateTime), argumentName As String)
        If Not value.HasValue Then ThrowArgumentException(argumentName)
    End Sub

    Private Sub ThrowIfInvalid(value As Decimal, argumentName As String)
        If argumentName <> NameOf(ImporteTotal) Then Return

        If value = 0 Then ThrowArgumentException(argumentName)
    End Sub

    Private Sub ThrowArgumentException(argumentName As String)
        Throw New ArgumentException($"{argumentName} is required for this operation.")
    End Sub
End Structure
