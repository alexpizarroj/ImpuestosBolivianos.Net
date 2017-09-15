Class CustomVerhoeffDigitCalculator
    Private Shared ReadOnly VerhoeffMul(,) As Int32 = New Int32(9, 9) {
        {0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
        {1, 2, 3, 4, 0, 6, 7, 8, 9, 5},
        {2, 3, 4, 0, 1, 7, 8, 9, 5, 6},
        {3, 4, 0, 1, 2, 8, 9, 5, 6, 7},
        {4, 0, 1, 2, 3, 9, 5, 6, 7, 8},
        {5, 9, 8, 7, 6, 0, 4, 3, 2, 1},
        {6, 5, 9, 8, 7, 1, 0, 4, 3, 2},
        {7, 6, 5, 9, 8, 2, 1, 0, 4, 3},
        {8, 7, 6, 5, 9, 3, 2, 1, 0, 4},
        {9, 8, 7, 6, 5, 4, 3, 2, 1, 0}
    }

    Private Shared ReadOnly VerhoeffPer(,) As Int32 = New Int32(7, 9) {
        {0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
        {1, 5, 7, 6, 2, 8, 3, 0, 9, 4},
        {5, 8, 0, 3, 7, 9, 6, 1, 4, 2},
        {8, 9, 1, 6, 0, 4, 3, 5, 2, 7},
        {9, 4, 5, 3, 1, 2, 6, 8, 7, 0},
        {4, 2, 8, 6, 5, 7, 3, 9, 0, 1},
        {2, 7, 9, 3, 8, 0, 6, 4, 1, 5},
        {7, 0, 4, 6, 9, 1, 3, 2, 5, 8}
    }

    Private Shared ReadOnly VerhoeffInv() As Int32 = New Int32(9) {
        0, 4, 3, 2, 1, 5, 6, 7, 8, 9
    }

    Public Shared Function [Get](value As Int32) As Int32
        Return [Get](value.ToString())
    End Function

    Public Shared Function [Get](ByVal value As String) As Int32
        Dim check As Int32 = 0
        For i As Int32 = 0 To (value.Length - 1)
            Dim x = (i + 1) Mod 8
            Dim y = Int32.Parse(value(value.Length - 1 - i))
            check = VerhoeffMul(check, VerhoeffPer(x, y))
        Next
        Return VerhoeffInv(check)
    End Function
End Class