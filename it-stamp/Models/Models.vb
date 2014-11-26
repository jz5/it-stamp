Imports System.Data.Entity
Imports System.ComponentModel.DataAnnotations
Imports Microsoft.AspNet.Identity.EntityFramework
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

Public Class [Event]
    <Key>
    Property Id As Integer

    <Required>
    <StringLength(100)>
    Property Name As String

    Overridable Property Community As Community

    <StringLength(1000)>
    <DataType(DataType.MultilineText)>
    Property Description As String

    <Url>
    <StringLength(256)>
    <DataType(DataType.Url)>
    Property Url As String

    Property StartDateTime As DateTime
    Property EndDateTime As DateTime

    Overridable Property Prefecture As Prefecture
    <StringLength(100)>
    Property Address As String
    Property Location As DbGeography
    <StringLength(100)>
    Property Place As String
    <StringLength(20)>
    Property Hashtag As String

    <StringLength(20)>
    Property CheckInCode As String
    Overridable Property CheckIns As ICollection(Of CheckIn)

    Overridable Property Favorites As ICollection(Of Favorite)
    Overridable Property SpecialEvents As SpecialEvent
    Overridable Property Comments As ICollection(Of Comment)

    Property IsHidden As Boolean
    Property IsLocked As Boolean
    Property IsCanceled As Boolean

    Property IsReported As Boolean
    Property ParticipantsOfflineCount As Integer
    Property ParticipantsOnlineCount As Integer
    <StringLength(1000)>
    <DataType(DataType.MultilineText)>
    Property ReportMemo As String

    Property CreationDateTime As DateTime
    Overridable Property CreatedBy As ApplicationUser
    Property LastUpdatedDateTime As DateTime
    Overridable Property LastUpdatedBy As ApplicationUser

    Function IsOnline() As Boolean
        Return Prefecture IsNot Nothing AndAlso Prefecture.Id = 48
    End Function

    Function FriendlyDateTime() As String

        If StartDateTime = EndDateTime Then
            Return StartDateTime.ToString("yyyy/M/d（ddd）")
        ElseIf StartDateTime.Date = EndDateTime.Date Then
            Return StartDateTime.ToString("yyyy/M/d（ddd） H:mm") & "～" & EndDateTime.ToString("H:mm")
        Else
            Return StartDateTime.ToString("yyyy/M/d（ddd） H:mm") & "～" & EndDateTime.ToString("M/d（ddd） HH:mm")
        End If
    End Function
End Class

Public Class SpecialEvent
    <Key>
    Property Id As Integer
    <Required>
    <StringLength(50)>
    Property Name As String
    Property StartDateTime As DateTime
    Property EndDateTime As DateTime
End Class

Public Class Favorite
    <Key>
    Property Id As Long
    <Required>
    Overridable Property [Event] As [Event]
    <Required>
    Overridable Property User As ApplicationUser
    <Required>
    Property DateTime As DateTime
End Class

Public Class CheckIn
    <Key>
    Property Id As Long
    <Required>
    Property [Event] As [Event]
    <Required>
    Overridable Property User As ApplicationUser

    Property Location As DbGeography

    Overridable Property Stamp As Stamp
    <Required>
    Property DateTime As DateTime
End Class

Public Class Comment
    <Key>
    Property id As Long
    <Required>
    Property [Event] As [Event]
    <StringLength(256)>
    <DataType(DataType.Text)>
    Property Content As String
    Property CreationDateTime As DateTime
    Overridable Property CreatedBy As ApplicationUser
End Class


Public Class Community
    <Key>
    Property Id As Integer

    <Required>
    <StringLength(50)>
    <Display(Name:="名前")>
    Property Name As String

    <StringLength(1000)>
    <DataType(DataType.MultilineText)>
    <Display(Name:="説明")>
    Property Description As String

    <DataType(DataType.Url)>
    <StringLength(256)>
    <Display(Name:="Webサイト")>
    Property Url As String

    <DataType(DataType.Url)>
    <StringLength(256)>
    <Display(Name:="Webサイト")>
    Property SubUrl1 As String

    <DataType(DataType.Url)>
    <StringLength(256)>
    <Display(Name:="Webサイト")>
    Property SubUrl2 As String

    <StringLength(100)>
    Property IconPath As String

    Overridable Property Members As ICollection(Of ApplicationUser)
    Overridable Property Owners As ICollection(Of ApplicationUser)

    Overridable Property Stamps As ICollection(Of Stamp)
    Property DefaultStamp As Stamp

    <Display(Name:="プライベートコミュニティ")>
    Property IsHidden As Boolean

    <Display(Name:="管理者のみが編集可能")>
    Property IsLocked As Boolean

    Property CreationDateTime As DateTime
    Overridable Property CreatedBy As ApplicationUser
    Property LastUpdatedDateTime As DateTime
    Overridable Property LastUpdatedBy As ApplicationUser
End Class

Public Class Stamp
    <Key>
    Property Id As Integer
    <Required>
    Property Community As Community

    <StringLength(50)>
    <Display(Name:="名前")>
    Property Name As String

    'Property IsDefault As Boolean

    <StringLength(100)>
    Property Expression As String

    <StringLength(100)>
    Property Path As String

    Property CreationDateTime As DateTime
    Overridable Property CreatedBy As ApplicationUser
    Property LastUpdatedDateTime As DateTime
    Overridable Property LastUpdatedBy As ApplicationUser
End Class

Public Class Prefecture
    <Key>
    Property Id As Integer
    <Required>
    <StringLength(10)>
    Property Name As String
End Class
