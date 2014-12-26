Public MustInherit Class EventApi

    MustOverride Function GetEvent(id As String) As Threading.Tasks.Task(Of ApiResult)

    MustOverride Function GetEvents(prefecture As Prefecture, startDate As DateTime, Optional count As Integer = 100) As Threading.Tasks.Task(Of IList(Of ApiResult))

    MustOverride ReadOnly Property Name As String

    Private NgWords() As String = {"婚活", "恋活", "合コン"}

    Protected Function IncludesNgWords(e As [Event]) As Boolean
        For Each w In NgWords
            If e.Name.Contains(w) Then
                Return True
            End If
        Next
        Return False
    End Function
End Class
