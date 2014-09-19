Imports System.Runtime.CompilerServices
Imports System.Drawing

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

    <Extension>
    Function ResizeTo(src As Bitmap, size As Size) As Bitmap

        Dim dst = New Bitmap(size.Width, size.Height)
        Dim g = Graphics.FromImage(dst)

        Dim srcX, srcY, srcSize As Integer
        Dim srcWidth = src.Width
        Dim srcHeight = src.Height

        If src.Width > src.Height Then
            srcX = (src.Width - src.Height) \ 2
            srcY = 0
            srcSize = src.Height
        Else
            srcX = 0
            srcY = (src.Height - src.Width) \ 2
            srcSize = src.Width
        End If

        g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality

        g.DrawImage(src, New Rectangle(0, 0, dst.Width, dst.Height), New Rectangle(srcX, srcY, srcSize, srcSize), GraphicsUnit.Pixel)
        g.Dispose()

        Return dst

    End Function

End Module
