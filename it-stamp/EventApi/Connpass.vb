Imports System.Net
Imports Newtonsoft.Json.Linq

Public Class Connpass

    'Private Const EventIdUriFormat As String = "http://api.atnd.org/events/?event_id={0}"
    Private Const EventsUriFormat As String = "http://connpass.com/api/v1/event/?keyword={0}&ymd={1}&count={2}"

    'Async Function GetEvent(id As String) As Threading.Tasks.Task(Of [Event])
    '    Dim content As String
    '    Using client = New WebClient
    '        client.Encoding = Text.Encoding.UTF8
    '        content = Await client.DownloadStringTaskAsync(New Uri(String.Format(EventIdUriFormat, id)))
    '    End Using

    '    Dim d = XDocument.Parse(content)
    '    Dim count As Integer
    '    If d.<results_returned> Is Nothing OrElse Not Integer.TryParse(d.<results_returned>.Value, count) Then
    '        Return Nothing
    '    End If

    '    Return ParseContent(d.<event>.First)
    'End Function

    Async Function GetEvents(prefecture As Prefecture, startDate As DateTime, Optional count As Integer = 100) As Threading.Tasks.Task(Of IList(Of [Event]))
        Dim content As String
        Using client = New WebClient
            client.Encoding = Text.Encoding.UTF8
            content = Await client.DownloadStringTaskAsync(
                New Uri(String.Format(EventsUriFormat, prefecture.Name, startDate.ToString("yyyyMMdd"), count)))
        End Using

        Dim list = New List(Of [Event])
        Dim o = JObject.Parse(content)

        For Each e In o("events").ToList
            Dim ev = ParseContent(e)
            If ev Is Nothing Then
                Continue For
            End If

            If ev.Prefecture IsNot Nothing AndAlso ev.Prefecture.Id <> prefecture.Id Then
                ' 異なる開催地域のイベント
                Continue For
            End If

            If IncludesNgWords(ev) Then
                ' NG word を含むイベント
                Continue For
            End If

            list.Add(ev)
        Next

        Return list
    End Function


    Private Function ParseContent(e As JToken) As [Event]

        If e Is Nothing Then
            Return Nothing
        End If

        Dim st, ed As DateTime
        If Not DateTime.TryParse(e("started_at").ToString, st) OrElse
           Not DateTime.TryParse(e("ended_at").ToString, ed) Then
            ' Do nothing
        End If

        Dim address = If(e("address").ToString, "")

        Dim m = New [Event] With {
            .Name = e("title").ToString,
            .Description = "",
            .Url = e("event_url").ToString,
            .Prefecture = address.GetPrefecture,
            .Address = address.RemovePrefecture,
            .Place = e("place").ToString,
            .StartDateTime = st,
            .EndDateTime = ed
            }

        If e("accepted") Is Nothing OrElse e("accepted").ToString = "0" Then
            ' MEMO: 参加者 0 のイベントは使用しない
            Return Nothing
        End If

        Return m
    End Function

    Private NgWords() As String = {"婚活", "恋活", "合コン"}

    Private Function IncludesNgWords(e As [Event]) As Boolean
        For Each w In NgWords
            If e.Name.Contains(w) Then
                Return True
            End If
        Next
        Return False
    End Function
End Class
