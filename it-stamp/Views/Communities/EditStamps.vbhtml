@ModelType Community
@Code
    ViewBag.Title = "スタンプの管理"
End Code
<h1>@ViewBag.Title</h1>

<div class="form-group">
    @Html.ActionLink("スタンプを追加", "UploadStamp", "Communities", New With {.id = Model.Id}, New With {.class = "btn btn-default"})
</div>

@If Model.Stamps IsNot Nothing AndAlso Model.Stamps.Any Then
    @Using Html.BeginForm("EditDefaultStamp", "Communities", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
        @Html.AntiForgeryToken()
        @<text>
            @Html.Hidden("id", Model.Id)
            <table class="table">
                <thead>
                    <tr>
                        <th>既定のスタンプ</th>
                        <th>スタンプの名前</th>
                        <th>スタンプ画像</th>
                        <th>Expression（未実装）</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    @For Each item In Model.Stamps
                        @<tr>
                            <td>@Html.RadioButton("defaultStamp", item.Id, (Model.DefaultStamp.Id = item.Id), New With {.id = Nothing})</td>
                            <td>@Html.Label(item.Name)</td>
                            <td><p><img class="img-rounded" src="@Href("/Uploads/" & item.Path)" /></p></td>
                            <td>@Html.Label(item.Expression)</td>
                            <td>
                                @If item.Id <> Model.DefaultStamp.Id Then
                                    @*@Html.Label("Edit")*@
                                    @<button type="button" class="btn btn-default delete-button" data-target-id="@item.Id" data-target-src="@Href("/Uploads/" & item.Path)" data-target-name="@item.Name">削除</button>
                                End If
                            </td>
                        </tr>
                    Next
                </tbody>
            </table>
            <div class="form-group">
                <input type="submit" value="保存" class="btn btn-primary" />
            </div>
        </text>
    End Using

    @<!-- Modal Window -->
    @Using Html.BeginForm("DeleteStamp", "Communities", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
        @Html.AntiForgeryToken()
        @Html.Hidden("id", Model.Id)

        @<text>
            <input id="stampId" type="hidden" name="stampId" value="" />
            <div class="modal fade" id="confirm-modal" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            <h4 class="modal-title" id="modalLabel">スタンプの削除</h4>
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
                            <p>💡 削除すると元に戻せません。</p>
                        </div>
                        <div class="modal-footer">
                            <input type="submit" value="削除" class="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>
        </text>
    End Using

Else
    @<p>💡 コミュニティのスタンプを登録してみませんか？</p>
End If
<hr />
@Html.ActionLink("戻る", "Edit", "Communities", New With {.id = Model.Id}, Nothing)

@Section Styles
    @Styles.Render("~/Content/skins/square/blue.css")
End Section

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
    <script>
        (function ($) {
            $('input').iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-blue'
            });

            $(".delete-button").on("click", function () {
                var targetId = $(this).attr("data-target-id");
                var targetName = $(this).attr("data-target-name");
                var targetSrc = $(this).attr("data-target-src");

                $("#target-name").text(targetName);
                $("#target-image").attr("src", targetSrc);
                $("#stampId").attr("value", targetId);

                $('#confirm-modal').modal('show');
            });

        })(jQuery);
    </script>
End Section
