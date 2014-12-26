Imports System.Net
Imports Newtonsoft.Json.Linq

Public Class Doorkeeper
    Inherits EventApi

    Private Const EventIdUriFormat As String = "http://api.doorkeeper.jp/events/{0}"
    Private Const EventsUriFormat As String = "http://api.doorkeeper.jp/events?since={0}&until={0}&locale=ja"

    Public Overrides ReadOnly Property Name As String
        Get
            Return "doorkeeper"
        End Get
    End Property


    Overrides Async Function GetEvent(id As String) As Threading.Tasks.Task(Of ApiResult)
        Dim content As String
        Using client = New WebClient
            client.Encoding = Text.Encoding.UTF8
            content = Await client.DownloadStringTaskAsync(New Uri(String.Format(EventIdUriFormat, id)))
        End Using

        Dim o = JObject.Parse(content)
        If o("event") Is Nothing Then
            Return Nothing
        End If

        Dim result = ParseContent(o("event"))
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
                New Uri(String.Format(EventsUriFormat, startDate.ToString("yyyy-MM-ddTH:mm:ssK"))))
        End Using

        Dim list = New List(Of ApiResult)
        Dim o = JArray.Parse(content)

        For Each e In o.ToList

            Dim result = ParseContent(e("event"))
            If result Is Nothing Then
                Continue For
            End If

            If result.Event.Prefecture IsNot Nothing AndAlso result.Event.Prefecture.Id <> prefecture.Id Then
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
        ' UTC
        If Not DateTime.TryParse(e("starts_at").ToString & "Z", st) OrElse
           Not DateTime.TryParse(e("ends_at").ToString & "Z", ed) Then
            ' Do nothing
        End If

        Dim address = If(e("address").ToString, "")

        Dim model = New [Event] With {
            .Name = e("title").ToString,
            .Description = "",
            .Url = e("public_url").ToString,
            .Prefecture = address.GetPrefecture,
            .Address = address.RemovePrefecture,
            .Place = e("venue_name").ToString,
            .StartDateTime = TokyoTime.ToLocalTime(st),
            .EndDateTime = TokyoTime.ToLocalTime(ed)
            }

        If e("participants") Is Nothing OrElse e("participants").ToString = "0" Then
            ' MEMO: 参加者 0 のイベントは使用しない
            Return Nothing
        End If

        Return New ApiResult With {.Event = model, .Name = Me.Name, .EventId = e("id").ToString}
    End Function

End Class
