@ModelType Community
@Code
    ViewData("Title") = "コミュニティスタンプの管理"
End Code
<h2>コミュニティスタンプの管理</h2>
@Html.ActionLink("スタンプを追加", "UploadStamp", "Communities", New With {.id = Model.Id}, New With {.class = "btn btn-default"})
@If Model.Stamps IsNot Nothing AndAlso Model.Stamps.Count > 0 Then
    @Using Html.BeginForm("EditDefaultStamp", "Communities", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
        @Html.AntiForgeryToken()
        @<text>
            @Html.Hidden("id", Model.Id)
            <table class="table">
                <tr>
                    <th>IsDefault</th>
                    <th>Name</th>
                    <th>Image</th>
                    <th>Expression</th>
                    <th></th>
                </tr>
                @For Each item In Model.Stamps
                    @<tr>
                        <td>@Html.RadioButton("defaultStamp", item.Id, (Model.DefaultStamp.Id = item.Id), New With {.id = Nothing})</td>
                        <td>@Html.Label(item.Name)</td>
                        <td><p><img class="img-rounded" src="@Href("/Uploads/" & item.Path)" /></p></td>
                        <td>@Html.Label(item.Expression)</td>
                        <td>
                            @If item.Id <> Model.DefaultStamp.Id Then
                                @Html.Label("Edit")
                                @<button type="button" class="btn btn-default delete-button" data-targetid="@item.Id" data-targetsrc="@Href("/Uploads/" & item.Path)" data-targetname="@item.Name" data-targetex="@item.Expression">Delete</button>
                            End If
                        </td>
                    </tr>
                Next
            </table>
            <div class="form-group">
                <input type="submit" value="設定" class="btn btn-primary" />
            </div>
        </text>
    End Using

    @<!-- Modal Window -->
    @<div class="modal fade" id="confirm-modal" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
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
                            <p id="target-expression"></p>
                        </div>
                    </div>
                    <p>削除してもよろしいですか？</p>
                </div>
                <div class="modal-footer">
                    @Using Html.BeginForm("DeleteStamp", "Communities", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
                        @Html.AntiForgeryToken()
                        @<text>
                            <input id="stampId" type="hidden" name="stampId" value="" />
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
Else
    @<p>コミュニティスタンプがまだ登録されていません。</p>
End If
<hr />
@Html.ActionLink("戻る", "Edit", "Communities", New With {.id = Model.Id}, Nothing)
@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")

    <script>
    $(".delete-button").on("click", function () {
        var targetId = $(this).attr("data-targetid");
        var targetName = $(this).attr("data-targetname");
        var targetExpression = $(this).attr("data-targetex");
        var targetSrc = $(this).attr("data-targetsrc");

        $("#target-name").text(targetName);
        $("#target-image").attr("src",targetSrc);
        $("#target-expression").text(targetExpression);
        $("#stampId").attr("value",targetId);

        $('#confirm-modal').modal('show');
    });

    </script>
End Section

