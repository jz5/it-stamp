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
        If str Is Nothing Then
            Return ""
        End If

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

    <Extension>
    Function GetIconPath(ev As [Event]) As String
        If ev.Community IsNot Nothing Then
            Return ev.Community.GetIconPath
        End If
        Return "/Uploads/Icons/no-community.png"
    End Function

    <Extension>
    Function GetIconPath(com As Community) As String
        If com IsNot Nothing AndAlso com.IconPath <> "" Then
            Return "/Uploads/" &
                com.IconPath &
                If(com.LastUpdatedBy IsNot Nothing, "?" & com.LastUpdatedDateTime.ToString("yyyyMMddHHmmss"), "")
        End If
        Return "/Uploads/Icons/no-community.png"
    End Function

    <Extension>
    Function GetIconPath(user As ApplicationUser) As String
        If user.IconPath <> "" Then
            Return "/Uploads/" & user.IconPath
        End If
        Return "/Uploads/Icons/anon.png"
    End Function

    <Extension>
    Function GetEventSiteName(ev As [Event]) As String
        If ev Is Nothing OrElse ev.Url Is Nothing Then
            Return Nothing
        End If

        Dim sites = New Dictionary(Of String, String) From {
            {"^https?://atnd\.org/", "atnd"},
            {"^https?://(.+?\.)?connpass\.com/", "connpass"},
            {"^https?://(.+?\.)?doorkeeper\.jp/", "doorkeeper"},
            {"^https?://kokucheese\.com/", "kokucheese"},
            {"^https?://(.+?\.)?peatix\.com/", "peatix"},
            {"^https?://(.+?\.)?zusaar\.com/", "zusaar"}
        }
        For Each s In sites
            If Regex.IsMatch(ev.Url, s.Key) Then
                Return s.Value
            End If
        Next
        Return Nothing
    End Function
End Module
