@ModelType Community
@Code
    ViewBag.Title = "コミュニティ管理者"
End Code
<h1>@ViewBag.Title</h1>
<div class="form-group">
    @Html.ActionLink("管理者を追加", "AddOwner", "Communities", New With {.id = Model.Id}, New With {.class = "btn btn-default"})
</div>
<table class="table">
    <thead>
        <tr>
            <th></th>
            <th>名前</th>
            <th></th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td><img class="img-responsive img-rounded" src="@(If(ViewBag.UserIcon <> "", Href("/Uploads/" & ViewBag.UserIcon), "http://placehold.it/96x96"))" /></td>
            <td>@ViewBag.UserFriendlyName</td>
            <td></td>
        </tr>
        @For Each item In Model.Owners
            @If item.Id = ViewBag.SessionUserId Then
                Continue For
            End If
            @<tr>
                <td><img class="img-responsive img-rounded" src="@Href(item.GetIconPath)" /></td>
                <td>@item.FriendlyName</td>
                <td>
                    <button type="button" class="btn btn-default delete-button" data-target-id="@item.Id" data-target-src="@Href(item.GetIconPath)" data-target-name="@item.FriendlyName">削除</button>
                </td>
            </tr>
        Next
    </tbody>
</table>
<!-- Modal Window -->
@Using Html.BeginForm("DeleteOwner", "Communities", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
    @Html.AntiForgeryToken()
    @Html.Hidden("id", Model.Id)
    @<text>
        <input id="targetId" type="hidden" name="targetId" value="" />
        <div class="modal fade" id="confirm-modal" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                        <h4 class="modal-title" id="modalLabel">管理者から除外</h4>
                    </div>
                    <div class="modal-body">
                        <div class="jumbotron">
                            <div class="jumbotron-contents">
                                <div class="media">
                                    <img class="img-responsive img-rounded pull-left" src="" id="target-image" />
                                    <div class="media-body">
                                        <h5 id="target-name" style="margin-top: 0;"></h5>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <p>管理者から除外します。</p>
                        <p>💡 除外してもまた追加できます。</p>
                    </div>
                    <div class="modal-footer">
                        <input type="submit" value="削除" class="btn btn-primary" />
                    </div>
                </div>
            </div>
        </div>
    </text>
End Using
<hr />
@Html.ActionLink("戻る", "Edit", "Communities", New With {.id = Model.Id}, Nothing)
@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
    <script>
        (function ($) {
            $(".delete-button").on("click", function () {
                var targetId = $(this).attr("data-target-id");
                var targetName = $(this).attr("data-target-name");
                var targetSrc = $(this).attr("data-target-src");

                $("#target-name").text(targetName);
                $("#target-image").attr("src", targetSrc);
                $("#targetId").attr("value", targetId);

                $('#confirm-modal').modal('show');
            });
        })(jQuery);

    </script>
End Section
