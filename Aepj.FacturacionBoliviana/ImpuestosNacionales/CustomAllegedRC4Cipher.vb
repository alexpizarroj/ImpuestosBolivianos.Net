Imports System.Text

Namespace ImpuestosNacionales
    Class CustomAllegedRC4Cipher
        Public Shared Function Encode(message As String, key As String) As String
            Dim state(255) As Int32, x As Int32 = 0, y As Int32 = 0
            Dim index1 As Int32 = 0, index2 As Int32 = 0, nmen As Int32

            For i As Int32 = 0 To 255
                state(i) = i
            Next

            For i As Int32 = 0 To 255
                index2 = (Convert.ToInt32(key(index1)) + state(i) + index2) Mod 256
                SwapValues(state(i), state(index2))
                index1 = (index1 + 1) Mod key.Length
            Next

            Dim resultStringBuilder = New StringBuilder()
            For i As Int32 = 0 To (message.Length - 1)
                x = (x + 1) Mod 256
                y = (state(x) + y) Mod 256
                SwapValues(state(x), state(y))

                nmen = Convert.ToInt32(message(i)) Xor state((state(x) + state(y)) Mod 256)
                resultStringBuilder.Append("-" + ToHexValue(nmen))
            Next

            Return resultStringBuilder.ToString().Substring(1)
        End Function

        Private Shared Sub SwapValues(ByRef left As Int32, ByRef right As Int32)
            Dim tmp = left
            left = right
            right = tmp
        End Sub

        Private Shared Function ToHexValue(value As Int32) As String
            Return Convert.ToString(value, 16).ToUpper().PadLeft(2, "0"c)
        End Function
    End Class
End Namespace