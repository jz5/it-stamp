Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Web.Mvc

Public Class RoleViewModel
    Property Id As String

    <Required(AllowEmptyStrings:=False)>
    <Display(Name:="RoleName")>
    Property Name As String
End Class

Public Class EditUserViewModel
    Property Id As String

    <Required(AllowEmptyStrings:=False)>
    <Display(Name:="Email")>
    <EmailAddress>
    Property Email As String

    Property RolesList As IEnumerable(Of SelectListItem)

    Property CommunitiesList As SelectList

    Property OwnerCommunitiesList As SelectList

    Property CommunitiesSelectList As SelectList

    <Display(Name:="コミュニティ")>
    Property OwnerCommunityId As Integer?
    Property CommunityId As Integer?


End Class