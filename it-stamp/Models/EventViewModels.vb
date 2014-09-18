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
    <Display(Name:="名前")>
    Property Name As String

    <StringLength(1023)>
    <Display(Name:="説明")>
    <DataType(DataType.MultilineText)>
    Property Description As String

    <Url>
    <StringLength(255)>
    <DataType(DataType.Url)>
    <Display(Name:="Webサイト")>
    Property Url As String

    <StringLength(255)>
    <Display(Name:="住所")>
    Property Address As String

    <StringLength(255)>
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
    <Display(Name:="名前")>
    Property Name As String

    <StringLength(1023)>
    <Display(Name:="説明")>
    <DataType(DataType.MultilineText)>
    Property Description As String

    <Url>
    <StringLength(255)>
    <DataType(DataType.Url)>
    <Display(Name:="Webサイト")>
    Property Url As String

    <StringLength(255)>
    <Display(Name:="住所")>
    Property Address As String

    <StringLength(255)>
    <Display(Name:="会場名")>
    Property Place As String


    Property CommunitiesSelectList As SelectList

    <Display(Name:="コミュニティ")>
    Property CommunityId As Integer?

    Property Community As Community


    Property SpecialEventsSelectList As SelectList

    <Display(Name:="イベント")>
    Property SpecialEventId As Integer?

    Property IsHidden As Boolean
    Property IsLocked As Boolean
    Property IsCanceled As Boolean

    Property ParticipantsOfflineCount As Integer
    Property ParticipantsOnlineCount As Integer

    <StringLength(1023)>
    <DataType(DataType.MultilineText)>
    Property ReportMemo As String
End Class


Public Class SearchEventsViewModel

    Property Results As List(Of [Event])

    Property StartPage As Integer
    Property EndPage As Integer

    Property CurrentPage As Integer
    Property TotalPages As Integer

    Property TotalCount As Integer

End Class