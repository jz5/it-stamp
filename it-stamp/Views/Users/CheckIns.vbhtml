@ModelType ApplicationUser
@Imports Microsoft.AspNet.Identity
@Code
    
    Layout = "_UserLayout.vbhtml"
    ViewBag.Title = "✅ " & Model.FriendlyName & "のチェックイン"

End Code

<div>
    @If Model.CheckIns.Count = 0 Then
        @<p class="text-muted">まだチェックインしていません。</p>
    Else
        @<table class="table">
            <tbody>
                @For Each item In Model.CheckIns
                Dim src = If(item.Event.Community IsNot Nothing AndAlso item.Event.Community.IconPath <> "", Href("/Uploads/" & item.Event.Community.IconPath), Href("/Uploads/Icons/no-community.png"))
                    @<tr>
                        <td style="border-top-width:0;width:32px;"><a href="@Href("/Events/")@item.Event.Id"><img class="img-rounded icon24" src="@src" /></a></td>
                        <td style="border-top-width:0;vertical-align:bottom;"><a href="@Href("/Events/")@item.Event.Id">@item.Event.Name</a></td>
                        <td style="border-top-width:0;vertical-align:bottom;"><time class="text-muted small" datetime="@item.DateTime.ToString("yyyy-MM-ddTH:mm:ssK")">@item.DateTime.ToString("yyyy/MM/dd HH:mm")</time></td>
                    </tr>
                Next
            </tbody>
        </table>
    End If
</div>