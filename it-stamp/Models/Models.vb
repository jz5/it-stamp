Imports System.Data.Entity
Imports System.ComponentModel.DataAnnotations
Imports Microsoft.AspNet.Identity.EntityFramework
Imports System.ComponentModel.DataAnnotations.Schema

Public Class [Event]
    <Key>
    Property Id As Integer

    <Required>
    <StringLength(255)>
    Property Name As String

    Overridable Property Community As Community

    <StringLength(1023)>
    <DataType(DataType.MultilineText)>
    Property Description As String

    <DataType(DataType.Url)>
    <StringLength(255)>
    Property Url As String

    Property StartDateTime As DateTime
    Property EndDateTime As DateTime

    Overridable Property Prefecture As Prefecture
    <StringLength(255)>
    Property Address As String
    <StringLength(255)>
    Property Place As String

    <StringLength(50)>
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
    <StringLength(1023)>
    <DataType(DataType.MultilineText)>
    Property ReportMemo As String

    Property CreationDateTime As DateTime
    Property CreatedBy As ApplicationUser
    Property LastUpdatedDateTime As DateTime
    Property LastUpdatedBy As ApplicationUser

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
    <StringLength(255)>
    Property Name As String
    Property StartDateTime As DateTime
    Property EndDateTime As DateTime
End Class

Public Class Favorite
    <Key>
    Property Id As Long
    <Required>
    Property [Event] As [Event]
    <Required>
    Property User As ApplicationUser
    <Required>
    Property DateTime As DateTime
End Class

Public Class CheckIn
    <Key>
    Property Id As Long
    <Required>
    Property [Event] As [Event]
    <Required>
    Property User As ApplicationUser
    <Required>
    Property Stamp As Stamp
    <Required>
    Property DateTime As DateTime
End Class

Public Class Comment
    <Key>
    Property id As Long
    <Required>
    Property [Event] As [Event]
    <StringLength(255)>
    <DataType(DataType.Text)>
    Property Content As String
    Property CreationDateTime As DateTime
    Property CreatedBy As ApplicationUser
End Class


Public Class Community
    <Key>
    Property Id As Integer

    <Required>
    <StringLength(255)>
    <Display(Name:="名前")>
    Property Name As String

    <StringLength(1023)>
    <DataType(DataType.MultilineText)>
    <Display(Name:="説明")>
    Property Description As String

    <DataType(DataType.Url)>
    <StringLength(255)>
    <Display(Name:="Webサイト")>
    Property Url As String

    <StringLength(255)>
    Property IconPath As String

    Overridable Property Members As ICollection(Of ApplicationUser)
    Overridable Property Owners As ICollection(Of ApplicationUser)

    Overridable Property Stamps As ICollection(Of Stamp)

    Property IsHidden As Boolean
    Property IsLocked As Boolean

    Property CreationDateTime As DateTime
    Property CreatedBy As ApplicationUser
    Property LastUpdatedDateTime As DateTime
    Property LastUpdatedBy As ApplicationUser
End Class

Public Class Stamp
    <Key>
    Property Id As Integer
    <Required>
    Property Community As Community

    Property IsDefault As Boolean

    <StringLength(255)>
    Property Expression As String

    <StringLength(255)>
    Property Path As String

End Class

Public Class Prefecture
    <Key>
    Property Id As Integer
    <Required>
    <StringLength(10)>
    Property Name As String
End Class
