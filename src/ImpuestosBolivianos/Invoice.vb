Public Structure Invoice
    Public NroAutorizacion As Int64
    Public NroFactura As Int64
    Public Fecha As DateTime
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
        ' TO DO: Implement
    End Sub

    Public Sub AssertHasEnoughInfoToRenderQrCode()
        ' TO DO: Implement
    End Sub
End Structure
