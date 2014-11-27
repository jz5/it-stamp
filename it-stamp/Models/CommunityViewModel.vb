Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel

'Public Class CommunityViewModel
'    <Key>
'    Property Id As Integer

'    <Required>
'    <StringLength(255)>
'    <Display(Name:="名前")>
'    Property Name As String

'    <StringLength(1023)>
'    <DataType(DataType.MultilineText)>
'    <Display(Name:="説明")>
'    Property Description As String

'    <Url>
'    <StringLength(255)>
'    <DataType(DataType.Url)>
'    <Display(Name:="Webサイト")>
'    Property Url As String

'    Property IconPath As String

'    Overridable Property Members As ICollection(Of ApplicationUser)
'    Overridable Property Owners As ICollection(Of ApplicationUser)

'    Overridable Property Stamps As ICollection(Of Stamp)

'    Property IsHidden As Boolean
'    Property IsLocked As Boolean

'    Property CreationDateTime As DateTime
'    Property CreatedBy As ApplicationUser
'    Property LastUpdatedDateTime As DateTime
'    Property LastUpdatedBy As ApplicationUser

'End Class

Public Class UploadCommunityIconViewModel

    <Key>
    Property Id As Integer

    <Display(Name:="名前")>
    Property Name As String

    Property IconPath As String

    <Required(ErrorMessageResourceName:="ImageFilePropertyValueRequired")>
    <DisplayName("アイコン")>
    <UploadFile(Extensions:="png;jpeg;jpg", MaxLength:=1024 * 1024)>
    Property File As HttpPostedFileBase

End Class

Public Class SearchCommunitiesViewModel

    Property Results As List(Of Community)

    Property StartPage As Integer
    Property EndPage As Integer

    Property CurrentPage As Integer
    Property TotalPages As Integer

    Property TotalCount As Integer
End Class

Public Class UploadCommunityStampViewModel

    <Key>
    Property Id As Integer

    <Required>
    <StringLength(50)>
    <Display(Name:="スタンプの名前")>
    Property Name As String

    Property StampPath As String

    <Required(ErrorMessageResourceName:="ImageFilePropertyValueRequired")>
    <DisplayName("スタンプ")>
    <UploadFile(Extensions:="png;jpeg;jpg", MaxLength:=1024 * 1024)>
    Property File As HttpPostedFileBase

End Class

Public Class AddCommunityOwnerViewModel

    <Key>
    Property Id As Integer

    <Required>
    <RegularExpression("^[A-Za-z0-9_]+$", ErrorMessage:="英数字のみ使えます。")>
    <Display(Name:="ユーザー名")>
    Property UserName As String

End Class
