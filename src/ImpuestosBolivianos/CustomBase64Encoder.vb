Class CustomBase64Encoder
    Private Shared ReadOnly BaseSymbols() As Char = New Char(63) {
        "0"c, "1"c, "2"c, "3"c, "4"c, "5"c, "6"c, "7"c, "8"c, "9"c,
        "A"c, "B"c, "C"c, "D"c, "E"c, "F"c, "G"c, "H"c, "I"c, "J"c,
        "K"c, "L"c, "M"c, "N"c, "O"c, "P"c, "Q"c, "R"c, "S"c, "T"c,
        "U"c, "V"c, "W"c, "X"c, "Y"c, "Z"c, "a"c, "b"c, "c"c, "d"c,
        "e"c, "f"c, "g"c, "h"c, "i"c, "j"c, "k"c, "l"c, "m"c, "n"c,
        "o"c, "p"c, "q"c, "r"c, "s"c, "t"c, "u"c, "v"c, "w"c, "x"c,
        "y"c, "z"c, "+"c, "/"c
    }

    Public Shared Function Encode(ByVal value As Int64) As String
        Dim result As String = String.Empty

        Dim quotient As Int64 = 1, remainder As Int32
        While quotient > 0
            quotient = value \ 64
            remainder = Convert.ToInt32(value Mod 64)
            result = BaseSymbols(remainder) + result
            value = quotient
        End While

        Return result
    End Function
End Class
