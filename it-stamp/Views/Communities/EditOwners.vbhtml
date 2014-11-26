@ModelType Community
@Code
    ViewData("Title") = "管理者の管理"
End Code
<h2>管理者の管理</h2>
@Html.ActionLink("管理者を追加", "AddOwner", "Communities", New With {.id = Model.Id}, New With {.class = "btn btn-default"})
<table class="table">
    <tr>
        <th>Image</th>
        <th>Name</th>
        <th></th>
    </tr>
    <tr>
        <td><p><img class="img-rounded" src="@(If(ViewBag.UserIcon <> "", Href("/Uploads/" & ViewBag.UserIcon), "http://placehold.it/96x96"))" /></p></td>
        <td>@ViewBag.UserFriendlyName</td>
        <td></td>
    </tr>
    @For Each item In Model.Owners
        @If item.Id = ViewBag.SessionUserId Then
            Continue For
        End If
        @<tr>
            <td><p><img class="img-rounded" src="@(If(item.IconPath <> "", Href("/Uploads/" & item.IconPath), "http://placehold.it/96x96"))" /></p></td>
            <td>@Html.Label(item.FriendlyName)</td>
            <td>
                @Using Html.BeginForm("DeleteOwner", "Communities", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
                    @Html.AntiForgeryToken()
                    @Html.Hidden("communityId", Model.Id)
                    @Html.Hidden("targetId", item.Id)

                    @Html.ValidationSummary(False, "", New With {.class = "text-danger"})

                    @<div class="form-group">
                        <input type="submit" value="削除" class="btn btn-primary" />
                    </div>
                End Using
            </td>
        </tr>
    Next
</table>
