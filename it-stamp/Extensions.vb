Imports System.Runtime.CompilerServices
Imports System.Drawing

Public Module Extensions
    <Extension>
    Function Excerpt(text As String, Optional count As Integer = 300) As String
        If text Is Nothing Then
            Return ""
        ElseIf text.Length > count Then
            Return text.Substring(0, count - 3) & "..."
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

    <Extension>
    Function TextWithUrl(ByVal str As String) As String
        Dim urlRegex As New Regex("https?:\/\/[-_.!~*'()a-zA-Z0-9;\/?:@&=+$,%#]+", RegexOptions.Singleline)
        Dim match As Match = urlRegex.Match(str)
        Dim result As String = ""

        While match IsNot Nothing AndAlso match.Success
            If match.Index <> 0 Then
                Dim prefix = str.Substring(0, match.Index)
                result += HttpUtility.HtmlEncode(prefix)
            End If

            Dim link As String = HttpUtility.HtmlEncode(match.Value)
            result += String.Format("<a href=""{0}"" target = ""_blank"">{1}</a>", link, link)

            str = str.Substring(match.Index + match.Value.Length)
            match = urlRegex.Match(str)
        End While

        If str <> "" Then
            result += HttpUtility.HtmlEncode(str)
        End If

        Return result

    End Function



End Module
