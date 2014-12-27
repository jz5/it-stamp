Imports System.Net
Imports Newtonsoft.Json.Linq

Public Class Connpass
    Inherits EventApi

    Private Const EventIdUriFormat As String = "http://connpass.com/api/v1/event/?event_id={0}"
    Private Const EventsUriFormat As String = "http://connpass.com/api/v1/event/?keyword={0}&ymd={1}&count={2}"

    Public Overrides ReadOnly Property Name As String
        Get
            Return "connpass"
        End Get
    End Property


    Overrides Async Function GetEvent(id As String) As Threading.Tasks.Task(Of ApiResult)
        Dim content As String
        Using client = New WebClient
            client.Encoding = Text.Encoding.UTF8
            content = Await client.DownloadStringTaskAsync(New Uri(String.Format(EventIdUriFormat, id)))
        End Using

        Dim o = JObject.Parse(content)
        If o("results_returned") Is Nothing OrElse o("results_returned").Value(Of Integer)() = 0 Then
            Return Nothing
        End If

        Dim result = ParseContent(o("events").ToList.First)
        If result Is Nothing Then
            Return Nothing
        End If

        Return result
    End Function

    Overrides Async Function GetEvents(prefecture As Prefecture, startDate As DateTime, Optional count As Integer = 100) As Threading.Tasks.Task(Of IList(Of ApiResult))

        Dim content As String
        Using client = New WebClient
            client.Encoding = Text.Encoding.UTF8
            content = Await client.DownloadStringTaskAsync(
                New Uri(String.Format(EventsUriFormat, prefecture.Name, startDate.ToString("yyyyMMdd"), count)))
        End Using

        Dim list = New List(Of ApiResult)
        Dim o = JObject.Parse(content)

        For Each e In o("events").ToList
            Dim result = ParseContent(e)
            If result Is Nothing Then
                Continue For
            End If

            If result.Event.Prefecture Is Nothing OrElse result.Event.Prefecture.Id <> prefecture.Id Then
                ' 異なる開催地域のイベント
                Continue For
            End If

            If IncludesNgWords(result.Event) Then
                ' NG word を含むイベント
                Continue For
            End If

            list.Add(result)
        Next

        Return list
    End Function


    Private Function ParseContent(e As JToken) As ApiResult

        If e Is Nothing Then
            Return Nothing
        End If

        Dim st, ed As DateTime
        If Not DateTime.TryParse(e("started_at").ToString, st) OrElse
           Not DateTime.TryParse(e("ended_at").ToString, ed) Then
            ' Do nothing
        End If

        Dim address = If(e("address").ToString, "")

        Dim model = New [Event] With {
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

        Return New ApiResult With {.Event = model, .Name = Me.Name, .EventId = e("event_id").ToString}
    End Function

End Class
