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
                <button type="button" class="btn btn-default delete-button" data-targetid="@item.Id" data-targetsrc="@(If(item.IconPath <> "", Href("/Uploads/" & item.IconPath), "http://placehold.it/96x96"))" data-targetname="@item.FriendlyName">Delete</button>
            </td>
        </tr>
    Next
</table>

<!-- Modal Window -->
<div class="modal fade" id="confirm-modal" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                <h4 class="modal-title" id="modalLabel">確認</h4>
            </div>
            <div class="modal-body">
                <div class="panel">
                    <div class="panel-body">
                        <p id="target-name"></p>
                        <p><img class="img-rounded" src="" id="target-image" /></p>
                    </div>
                </div>
                <p>削除してもよろしいですか？</p>
            </div>
            <div class="modal-footer">
                @Using Html.BeginForm("DeleteOwner", "Communities", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
                    @Html.AntiForgeryToken()
                    @<text>
                        <input id="targetId" type="hidden" name="targetId" value="" />
                        @Html.Hidden("id", Model.Id)
                        <div class="form-group">
                            <input type="submit" value="削除" class="btn btn-primary" />
                        </div>
                    </text>
                End Using
            </div>
        </div>
    </div>
</div>

<hr />
@Html.ActionLink("戻る", "Edit", "Communities", New With {.id = Model.Id}, Nothing)

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")

    <script>
    $(".delete-button").on("click", function () {
        var targetId = $(this).attr("data-targetid");
        var targetName = $(this).attr("data-targetname");
        var targetSrc = $(this).attr("data-targetsrc");

        $("#target-name").text(targetName);
        $("#target-image").attr("src",targetSrc);
        $("#targetId").attr("value",targetId);

        $('#confirm-modal').modal('show');
    });

    </script>
End Section
