Imports System.Linq.Expressions
Imports System.Web.Mvc

Public Module DateTimeHelpers
    <System.Runtime.CompilerServices.Extension> _
    Public Function ToRelativeTimeString(ByVal t As DateTime, Optional ByVal baseTime As DateTime? = Nothing) As MvcHtmlString
        Dim baseDateTime = DateTime.Now

        ' 値があればそれを適用
        If baseTime.HasValue Then
            baseDateTime = baseTime.Value
        End If

        ' 比較する
        Dim diff = baseDateTime - t

        ' 1日以上経過している
        If diff.Days > 0 Then
            Dim year = Math.Floor(diff.Days / 365)
            If year > 0 Then
                ' 1年以上経過している
                Return MvcHtmlString.Create(year.ToString + "年")
            Else
                Dim month = Math.Floor(diff.Days / 31)
                If month > 0 Then
                    ' 1月以上経過している
                    Return MvcHtmlString.Create(month.ToString + "月")
                Else
                    ' 1日以上1月以下
                    Return MvcHtmlString.Create(diff.Days.ToString + "日")
                End If
            End If
        Else

            If diff.Hours > 0 Then
                ' n時間以上1日以下
                Return MvcHtmlString.Create(diff.Hours.ToString + "時間")
            ElseIf diff.Minutes > 0 Then
                ' n分以上1時間以下
                Return MvcHtmlString.Create(diff.Minutes.ToString + "分")
            Else
                ' n秒以上1分以下
                Return MvcHtmlString.Create(diff.Seconds.ToString + "秒")
            End If
        End If
    End Function
End Module
