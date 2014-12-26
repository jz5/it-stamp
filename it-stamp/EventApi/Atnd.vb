Imports System.Net

Public Class Atnd
    Inherits EventApi

    Private Const EventIdUriFormat As String = "http://api.atnd.org/events/?event_id={0}"
    Private Const EventsUriFormat As String = "http://api.atnd.org/events/?keyword={0}&ymd={1}&count={2}"

    Public Overrides ReadOnly Property Name As String
        Get
            Return "atnd"
        End Get
    End Property

    Overrides Async Function GetEvent(id As String) As Threading.Tasks.Task(Of ApiResult)
        Dim content As String
        Using client = New WebClient
            client.Encoding = Text.Encoding.UTF8
            content = Await client.DownloadStringTaskAsync(New Uri(String.Format(EventIdUriFormat, id)))
        End Using

        Dim d = XDocument.Parse(content)
        Dim resultsReturned As Integer
        If d.<hash>.<results_returned> Is Nothing OrElse Not Integer.TryParse(d.<hash>.<results_returned>.Value, resultsReturned) Then
            Return Nothing
        End If

        Return ParseContent(d.<hash>.<events>.<event>.First)
    End Function

    Overrides Async Function GetEvents(prefecture As Prefecture, startDate As DateTime, Optional count As Integer = 100) As Threading.Tasks.Task(Of IList(Of ApiResult))
        Dim content As String
        Using client = New WebClient
            client.Encoding = Text.Encoding.UTF8
            content = Await client.DownloadStringTaskAsync(
                New Uri(String.Format(EventsUriFormat, prefecture.Name, startDate.ToString("yyyyMMdd"), count)))
        End Using

        Dim list = New List(Of ApiResult)

        Dim d = XDocument.Parse(content)
        Dim resultsReturned As Integer
        If d.<hash>.<results_returned> Is Nothing OrElse Not Integer.TryParse(d.<hash>.<results_returned>.Value, resultsReturned) Then
            Return list
        End If

        For Each e In d...<event>
            Dim result = ParseContent(e)
            If result Is Nothing Then
                Continue For
            End If

            If result.Event.Prefecture IsNot Nothing AndAlso result.Event.Prefecture.Id <> prefecture.Id Then
                ' 異なる開催地域のイベント
                Continue For
            End If

            If result IsNot Nothing AndAlso IncludesNgWords(result.Event) Then
                ' NG word を含むイベント
                Continue For
            End If

            list.Add(result)
        Next

        Return list
    End Function


    Private Function ParseContent(e As XElement) As ApiResult

        If e Is Nothing Then
            Return Nothing
        End If

        Dim st, ed As DateTime
        If Not DateTime.TryParse(e.<started_at>.Value, st) OrElse
           Not DateTime.TryParse(e.<ended_at>.Value, ed) Then
            ' Do nothing
        End If

        Dim address = If(e.<address>.Value, "")

        Dim model = New [Event] With {
            .Name = e.<title>.Value,
            .Description = "",
            .Url = e.<event_url>.Value,
            .Prefecture = address.GetPrefecture,
            .Address = address.RemovePrefecture,
            .Place = e.<place>.Value,
            .StartDateTime = st,
            .EndDateTime = ed
            }

        If e.<accepted>.Value Is Nothing OrElse e.<accepted>.Value = "0" Then
            ' MEMO: 参加者 0 のイベントは使用しない
            Return Nothing
        End If

        Return New ApiResult With {.Event = model, .Name = Me.Name, .EventId = e.<event_id>.Value}
    End Function

End Class
