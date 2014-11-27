Imports System.ComponentModel.DataAnnotations

Public Class AddEventViewModel

    Property PrefectureSelectList As SelectList

    <Required>
    <Display(Name:="開催地域")>
    Property PrefectureId As Integer

    <Required>
    <DisplayFormat(DataFormatString:="{0:yyyy/MM/dd}")>
    <DataType(DataType.Date)>
    <Display(Name:="開催日")>
    Property StartDate As DateTime


End Class

Public Class AddEventDetailsViewModel

    Property PrefectureSelectList As SelectList

    <Required>
    <Display(Name:="開催地域")>
    Property PrefectureId As Integer

    Property Prefecture As String

    <Required>
    <DataType(DataType.Date)>
    <Display(Name:="開始日")>
    Property StartDate As DateTime

    <Required>
    <DataType(DataType.Date)>
    <Display(Name:="終了日")>
    Property EndDate As DateTime

    <DataType(DataType.Time)>
    <Display(Name:="開始時間")>
    Property StartTime As DateTime?

    <DataType(DataType.Time)>
    <Display(Name:="終了時間")>
    Property EndTime As DateTime?


    <Required>
    <StringLength(100)>
    <Display(Name:="名前")>
    Property Name As String

    <StringLength(1000)>
    <Display(Name:="説明")>
    <DataType(DataType.MultilineText)>
    Property Description As String

    <Url>
    <StringLength(256)>
    <DataType(DataType.Url)>
    <Display(Name:="Webサイト")>
    Property Url As String

    <StringLength(100)>
    <Display(Name:="住所")>
    Property Address As String

    <StringLength(100)>
    <Display(Name:="会場名")>
    Property Place As String


    Property CommunitiesSelectList As SelectList

    <Display(Name:="コミュニティ")>
    Property CommunityId As Integer?

    Function IsOnline() As Boolean
        Return PrefectureId = 48
    End Function
End Class

Public Class EventDetailsViewModel

    Property Id As Integer

    Property PrefectureSelectList As SelectList

    <Required>
    <Display(Name:="開催地域")>
    Property PrefectureId As Integer

    <Required>
    <DataType(DataType.Date)>
    <Display(Name:="開始日")>
    Property StartDate As DateTime

    <Required>
    <DataType(DataType.Date)>
    <Display(Name:="終了日")>
    Property EndDate As DateTime

    <DataType(DataType.Time)>
    <Display(Name:="開始時間")>
    Property StartTime As DateTime?

    <DataType(DataType.Time)>
    <Display(Name:="終了時間")>
    Property EndTime As DateTime?


    <Required>
    <StringLength(100)>
    <Display(Name:="名前")>
    Property Name As String

    <StringLength(1000)>
    <Display(Name:="説明")>
    <DataType(DataType.MultilineText)>
    Property Description As String

    <Url>
    <StringLength(256)>
    <DataType(DataType.Url)>
    <Display(Name:="Webサイト")>
    Property Url As String

    <StringLength(100)>
    <Display(Name:="住所")>
    Property Address As String

    <StringLength(100)>
    <Display(Name:="会場名")>
    Property Place As String


    Property CommunitiesSelectList As SelectList

    <Display(Name:="コミュニティ")>
    Property CommunityId As Integer?

    Property Community As Community


    Property SpecialEventsSelectList As SelectList

    <StringLength(20)>
    <Display(Name:="チェックインコード")>
    Property CheckInCode As String

    <Display(Name:="イベント")>
    Property SpecialEventId As Integer?

    <Display(Name:="IT勉強会一覧に含めない")>
    Property IsHidden As Boolean
    <Display(Name:="一般ユーザーの編集禁止")>
    Property IsLocked As Boolean
    <Display(Name:="中止されたIT勉強会")>
    Property IsCanceled As Boolean

    Property IsReported As Boolean

    <Display(Name:="参加人数（オフライン）")>
    Property ParticipantsOfflineCount As Integer
    <Display(Name:="参加人数（オンライン）")>
    Property ParticipantsOnlineCount As Integer

    <StringLength(1000)>
    <DataType(DataType.MultilineText)>
    <Display(Name:="備考")>
    Property ReportMemo As String
End Class


Public Class SearchEventsViewModel

    Property Results As List(Of [Event])

    Property StartPage As Integer
    Property EndPage As Integer

    Property CurrentPage As Integer
    Property TotalPages As Integer

    Property TotalCount As Integer

    Property Past As Boolean

End Class

Public Class CheckInViewModel

    Property [Event] As [Event]

    Property PostComment As Boolean

    Property ShareTwitter As Boolean

    Property ShareFacebook As Boolean

    <StringLength(256)>
    <Display(Name:="コメント")>
    <DataType(DataType.MultilineText)>
    Property AdditionalMessage As String

End Class