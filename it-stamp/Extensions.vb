Imports System.Runtime.CompilerServices

Public Module Extensions
    <Extension>
    Function Excerpt(text As String, Optional count As Integer = 300) As String
        If text Is Nothing Then
            Return ""
        ElseIf text.Length > count Then
            Return text.Substring(0, count) & "..."
        Else
            Return text
        End If
    End Function

End Module
